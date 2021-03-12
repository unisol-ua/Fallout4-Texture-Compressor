using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Archive2Interop;
using System.Collections.ObjectModel;
using System.IO;

namespace Fallout4_Texture_Compressor
{
    public class Archive : INotifyPropertyChanged
    {
        private ulong uiMaxArchiveSize = Archive2Interop.Archive.UnlimitedSizeS;
        private uint uiMaxChunkCount = 4;
        private uint uiSingleMipChunkAreaX = 512;
        private uint uiSingleMipChunkAreaY = 512;
        private ObservableCollection<FileEntry> EntriesA = new ObservableCollection<FileEntry>();
        private bool isOpenedArchive;
        private Archive.LoggingDelegate MessageDelegate;
        private Archive.LoggingDelegate ErrorDelegate;
        private uint uiArchiveCount;
        private FileFormat archiveFormat;
        private FileChunkStorageType chunkStorageType;
        private CompressionType fileCompressionType;

        public Archive()
        {
            this.ArchiveFormat = FileFormat.General;
            this.ChunkStorageType = FileChunkStorageType.Memory;
            this.FileCompressionType = CompressionType.Default;
        }

        public Archive(Archive.LoggingDelegate aMessageDelegate, Archive.LoggingDelegate aErrorDelegate)
        {
            this.ArchiveFormat = FileFormat.General;
            this.ChunkStorageType = FileChunkStorageType.Memory;
            this.FileCompressionType = CompressionType.Default;
            this.MessageDelegate = aMessageDelegate;
            this.ErrorDelegate = aErrorDelegate;
        }

        public Archive(Archive.LoggingDelegate aMessageDelegate, Archive.LoggingDelegate aErrorDelegate, FileChunkStorageType aChunkStorageType)
        {
            this.ArchiveFormat = FileFormat.General;
            this.ChunkStorageType = aChunkStorageType;
            this.FileCompressionType = CompressionType.Default;
            this.MessageDelegate = aMessageDelegate;
            this.ErrorDelegate = aErrorDelegate;
        }

        public ulong MaxArchiveSize
        {
            get
            {
                return this.uiMaxArchiveSize;
            }
            set
            {
                if ((long)this.uiMaxArchiveSize == (long)value)
                    return;
                this.uiMaxArchiveSize = value;
                this.NotifyPropertyChanged("MaxArchiveSize");
                this.NotifyPropertyChanged("MaxArchiveSizeMB");
            }
        }

        public ulong MaxArchiveSizeMB
        {
            get
            {
                return this.uiMaxArchiveSize / 1024UL / 1024UL;
            }
            set
            {
                ulong num = value * 1024UL * 1024UL;
                if ((long)this.uiMaxArchiveSize == (long)num)
                    return;
                this.uiMaxArchiveSize = num;
                this.NotifyPropertyChanged("MaxArchiveSize");
                this.NotifyPropertyChanged("MaxArchiveSizeMB");
            }
        }

        public uint ArchiveCount
        {
            get
            {
                return this.uiArchiveCount;
            }
            set
            {
                if ((int)this.uiArchiveCount == (int)value)
                    return;
                this.uiArchiveCount = value;
                this.NotifyPropertyChanged("ArchiveCount");
            }
        }

        public uint MaxChunkCount
        {
            get
            {
                return this.uiMaxChunkCount;
            }
            set
            {
                if ((int)this.uiMaxChunkCount == (int)value)
                    return;
                this.uiMaxChunkCount = value;
                this.NotifyPropertyChanged("MaxChunkCount");
            }
        }

        public uint SingleMipChunkAreaX
        {
            get
            {
                return this.uiSingleMipChunkAreaX;
            }
            set
            {
                if ((int)this.uiSingleMipChunkAreaX == (int)value)
                    return;
                this.uiSingleMipChunkAreaX = value;
                this.NotifyPropertyChanged("SingleMipChunkAreaX");
                this.NotifyPropertyChanged("SingleMipChunkArea");
            }
        }

        public uint SingleMipChunkAreaY
        {
            get
            {
                return this.uiSingleMipChunkAreaY;
            }
            set
            {
                if ((int)this.uiSingleMipChunkAreaY == (int)value)
                    return;
                this.uiSingleMipChunkAreaY = value;
                this.NotifyPropertyChanged("SingleMipChunkAreaY");
                this.NotifyPropertyChanged("SingleMipChunkArea");
            }
        }

        public uint SingleMipChunkArea
        {
            get
            {
                return this.SingleMipChunkAreaX * this.SingleMipChunkAreaY;
            }
        }

        public FileFormat ArchiveFormat
        {
            get
            {
                return this.archiveFormat;
            }
            set
            {
                if (this.archiveFormat == value)
                    return;
                this.archiveFormat = value;
                this.NotifyPropertyChanged("ArchiveFormat");
                if (this.archiveFormat != FileFormat.XBoxDDS)
                    return;
                this.FileCompressionType = CompressionType.XBox;
            }
        }

        public FileChunkStorageType ChunkStorageType
        {
            get
            {
                return this.chunkStorageType;
            }
            set
            {
                if (this.chunkStorageType == value)
                    return;
                this.chunkStorageType = value;
                this.NotifyPropertyChanged("ChunkStorageType");
            }
        }

        public CompressionType FileCompressionType
        {
            get
            {
                return this.fileCompressionType;
            }
            set
            {
                if (this.fileCompressionType == value || this.ArchiveFormat == FileFormat.XBoxDDS && value != CompressionType.XBox)
                    return;
                this.fileCompressionType = value;
                this.NotifyPropertyChanged("FileCompressionType");
            }
        }

        public ObservableCollection<FileEntry> Entries
        {
            get
            {
                return this.EntriesA;
            }
        }

        public static bool HasDataInPath(string asFileOnDisk)
        {
            for (DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(asFileOnDisk)); directoryInfo != null; directoryInfo = directoryInfo.Parent)
            {
                if (string.Compare(directoryInfo.Name, "data", true) == 0)
                    return true;
            }
            return false;
        }

        public static string GetPathRelativeToData(string asFileOnDisk)
        {
            for (DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(asFileOnDisk)); directoryInfo != null; directoryInfo = directoryInfo.Parent)
            {
                if (string.Compare(directoryInfo.Name, "data", true) == 0)
                    return asFileOnDisk.Substring(directoryInfo.FullName.Length + 1);
            }
            return Path.GetFileName(asFileOnDisk);
        }

        public static string GetPathRelativeToOther(string asFileOnDisk, string asPathRelativeTo)
        {
            Uri uri = new Uri(asFileOnDisk);
            if (!asPathRelativeTo.EndsWith(Path.DirectorySeparatorChar.ToString()) && !asPathRelativeTo.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                asPathRelativeTo += Path.DirectorySeparatorChar.ToString();
            return Uri.UnescapeDataString(new Uri(asPathRelativeTo).MakeRelativeUri(uri).ToString());
        }

        public void NewArchive()
        {
            this.Entries.Clear();
            this.isOpenedArchive = false;
        }

        public void AddFile(FileEntry aEntry)
        {
            if (this.EntriesA.SingleOrDefault<FileEntry>((Func<FileEntry, bool>)(item => Archive.ArchiveNamesEqual(item.ArchiveName, aEntry.ArchiveName))) != null)
                throw new ResourceIDCollisionException(aEntry.ArchiveName);
            this.EntriesA.Add(aEntry);
        }

        public void AddOrReplaceFile(FileEntry aEntry)
        {
            bool flag = false;
            for (int index = 0; index < this.EntriesA.Count; ++index)
            {
                if (Archive.ArchiveNamesEqual(aEntry.ArchiveName, this.EntriesA[index].ArchiveName))
                {
                    this.EntriesA[index] = aEntry;
                    flag = true;
                    break;
                }
            }
            if (flag)
                return;
            this.EntriesA.Add(aEntry);
        }

        public bool RemoveFiles(HashSet<string> aFileNames)
        {
            bool flag = false;
            for (int index = this.EntriesA.Count - 1; index >= 0; --index)
            {
                if (aFileNames.Contains(this.EntriesA[index].ArchiveName))
                {
                    flag = true;
                    this.EntriesA.RemoveAt(index);
                }
            }
            return flag;
        }

        public void LoadArchive(string aArchiveName, bool aLoadData)
        {
            this.EntriesA.Clear();
            using (Archive2Interop.Archive archive = new Archive2Interop.Archive(FileFormat.General, this.chunkStorageType))
            {
                archive.ReadFromArchive(aArchiveName, aLoadData);
                foreach (FileBase aFile in archive)
                    this.AddArchiveFile(aFile, aArchiveName, aLoadData);
                this.ArchiveFormat = archive.Format;
            }
            this.isOpenedArchive = true;
        }

        public void SaveArchive(string asArchiveName, bool abWriteStringTable)
        {
            ulong aSizeLimitBytes = (int)this.ArchiveCount != 0 || this.isOpenedArchive ? Archive2Interop.Archive.UnlimitedSizeS : this.MaxArchiveSize;
            string format = (int)this.ArchiveCount == 0 || this.ArchiveCount >= 10U ? "{0}{1:D2}{2}" : "{0}{1}{2}";
            uint num1 = (int)this.ArchiveCount == 0 || this.isOpenedArchive ? 0U : (uint)Math.Ceiling((double)this.EntriesA.Count / (double)this.ArchiveCount);
            using (Archive2Interop.Archive archive = new Archive2Interop.Archive(this.ArchiveFormat, this.chunkStorageType, aSizeLimitBytes))
            {
                string str = Path.ChangeExtension(asArchiveName, (string)null);
                string extension = Path.GetExtension(asArchiveName);
                string directoryName = Path.GetDirectoryName(asArchiveName);
                if (!string.IsNullOrEmpty(directoryName))
                    Directory.CreateDirectory(directoryName);
                int num2 = 1;
                int num3 = 0;
                this.PreLoadArchiveFiles();
                bool flag1 = true;
                int aNextFile = 0;
                while (flag1)
                {
                    FileBase[] fileBaseArray = this.ConvertFileBatch(100, ref aNextFile);
                    flag1 = fileBaseArray != null;
                    if (flag1)
                    {
                        foreach (FileBase fileBase in fileBaseArray)
                        {
                            if (fileBase != null)
                            {
                                string resourceIdCollision = archive.GetResourceIDCollision(fileBase);
                                if (string.IsNullOrEmpty(resourceIdCollision))
                                {
                                    bool flag2 = false;
                                    if ((int)num1 != 0)
                                    {
                                        if ((long)num3 >= (long)num1)
                                        {
                                            flag2 = true;
                                            goto label_13;
                                        }
                                    }
                                    try
                                    {
                                        archive.AddFile(fileBase);
                                    }
                                    catch (ArchiveFullException ex)
                                    {
                                        flag2 = true;
                                    }
                                    label_13:
                                    if (flag2)
                                    {
                                        archive.WriteToDisk(string.Format(format, (object)str, (object)num2, (object)extension), abWriteStringTable);
                                        ++num2;
                                        num3 = 0;
                                        archive.RemoveAllFiles();
                                        archive.AddFile(fileBase);
                                    }
                                    ++num3;
                                }
                                else
                                    this.ErrorDelegate(string.Format("Resource ID collision between \"{0}\" (skipped) and \"{1}\"", (object)fileBase.Name, (object)resourceIdCollision));
                            }
                        }
                    }
                }
                if (archive.GetFileCount() <= 0U)
                    return;
                string asArchiveFile = num2 != 1 ? string.Format(format, (object)str, (object)num2, (object)extension) : string.Format("{0}{1}", (object)str, (object)extension);
                archive.WriteToDisk(asArchiveFile, abWriteStringTable);
            }
        }

        private static bool ArchiveNamesEqual(string aName1, string aName2)
        {
            return string.Compare(aName1.Replace('\\', '/'), aName2.Replace('\\', '/'), true) == 0;
        }

        private void AddArchiveFile(FileBase aFile, string aArchiveName, bool aLoadData)
        {
            this.FileCompressionType = aFile.GetCompressionType();
            this.EntriesA.Add((FileEntry)new ArchiveFileEntry(aFile, aArchiveName, aLoadData));
        }

        private void PreLoadArchiveFiles()
        {
            Parallel.For(0, this.EntriesA.Count, (Action<int>)(i =>
            {
                ArchiveFileEntry archiveFileEntry = this.EntriesA[i] as ArchiveFileEntry;
                if (archiveFileEntry == null)
                    return;
                archiveFileEntry.EnsureDataLoaded();
            }));
        }

        private FileBase[] ConvertFileBatch(int aBatchSize, ref int aNextFile)
        {
            if (aNextFile >= this.EntriesA.Count)
                return (FileBase[])null;
            int toExclusive = Math.Min(this.EntriesA.Count - aNextFile, aBatchSize);
            FileBase[] convertedFiles = new FileBase[toExclusive];
            int entriesStart = aNextFile;
            Parallel.For(0, toExclusive, (Action<int>)(i =>
            {
                FileEntry fileEntry = this.EntriesA[i + entriesStart];
                if (fileEntry.CanBeExportedAsFormat(this.ArchiveFormat))
                {
                    try
                    {
                        convertedFiles[i] = fileEntry.CreateFileAsFormat(this.ArchiveFormat, this.chunkStorageType, this.fileCompressionType, this.MaxChunkCount, this.SingleMipChunkArea);
                    }
                    catch (UnsupportedFeatureException ex)
                    {
                        this.ErrorDelegate(string.Format("Skipping \"{0}\" since it contains features not supported by the archiver", (object)fileEntry.ArchiveName));
                    }
                    catch (InvalidFileFormatException ex)
                    {
                        this.ErrorDelegate(string.Format("Skipping \"{0}\" since the file format is not supported by the archiver", (object)fileEntry.ArchiveName));
                    }
                }
                else
                    this.ErrorDelegate(string.Format("Skipping \"{0}\" since it is not supported for this archive type", (object)fileEntry.ArchiveName));
            }));
            aNextFile += toExclusive;
            return convertedFiles;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string asPropertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged((object)this, new PropertyChangedEventArgs(asPropertyName));
        }

        public delegate void LoggingDelegate(string asMessage);
    }
}

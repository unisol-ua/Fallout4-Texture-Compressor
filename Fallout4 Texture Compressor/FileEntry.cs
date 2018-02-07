using Archive2Interop;
using System.ComponentModel;
using System.IO;

namespace Fallout4_Texture_Compressor
{
    public abstract class FileEntry : INotifyPropertyChanged
    {
        private FileFormat format;
        private string archiveName;

        protected FileEntry(string aArchiveName, FileFormat aFormat)
        {
            this.archiveName = aArchiveName;
            this.format = aFormat;
        }

        public FileFormat Format
        {
            get
            {
                return this.format;
            }
        }

        public string ArchiveName
        {
            get
            {
                return this.archiveName;
            }
        }

        public bool CanBeExportedAsFormat(FileFormat aFormat)
        {
            if (this.Format != aFormat)
                return this.CanBeExportedAsFormatImpl(aFormat);
            return true;
        }

        public FileBase CreateFileAsFormat(FileFormat aFormat, FileChunkStorageType aChunkStorageType, CompressionType aCompressionType, uint aMaxChunks, uint aSingleMipChunkArea)
        {
            if (!this.CanBeExportedAsFormat(aFormat))
                return (FileBase)null;
            return this.CreateFileAsFormatImpl(aFormat, aChunkStorageType, aCompressionType, aMaxChunks, aSingleMipChunkArea);
        }

        public void ExtractTo(string aFolder)
        {
            string str = Path.Combine(aFolder, this.archiveName);
            Directory.CreateDirectory(Path.GetDirectoryName(str));
            this.ExtractToImpl(str);
        }

        protected abstract bool CanBeExportedAsFormatImpl(FileFormat aeFormat);

        protected abstract FileBase CreateFileAsFormatImpl(FileFormat aFormat, FileChunkStorageType aChunkStorageType, CompressionType aCompressionType, uint aMaxChunks, uint aSingleMipChunkArea);

        protected abstract void ExtractToImpl(string asFileAndPath);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string asPropertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged((object)this, new PropertyChangedEventArgs(asPropertyName));
        }
    }
}

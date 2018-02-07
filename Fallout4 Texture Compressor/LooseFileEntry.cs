using Archive2Interop;
using System.IO;

namespace Fallout4_Texture_Compressor
{
    internal class LooseFileEntry : FileEntry
    {
        private string looseFilename;

        public LooseFileEntry(string aArchiveName, string aLooseFilename)
          : base(aArchiveName, Common.GetFileFormat(aLooseFilename))
        {
            this.looseFilename = aLooseFilename;
        }

        public string LooseFilename
        {
            get
            {
                return this.looseFilename;
            }
        }

        protected override bool CanBeExportedAsFormatImpl(FileFormat aFormat)
        {
            if (aFormat == FileFormat.General)
                return true;
            if (aFormat == FileFormat.XBoxDDS)
                return this.Format == FileFormat.DDS;
            return false;
        }

        protected override FileBase CreateFileAsFormatImpl(FileFormat aFormat, FileChunkStorageType aChunkStorageType, CompressionType aCompressionType, uint aMaxChunks, uint aSingleMipChunkArea)
        {
            FileBase fileBase = (FileBase)null;
            switch (aFormat)
            {
                case FileFormat.General:
                    fileBase = (FileBase)new GeneralFile(aChunkStorageType, this.ArchiveName);
                    break;
                case FileFormat.DDS:
                case FileFormat.XBoxDDS:
                    fileBase = (FileBase)new ImageDDSFile(aChunkStorageType, this.ArchiveName, aMaxChunks, aSingleMipChunkArea);
                    break;
                case FileFormat.GNF:
                    fileBase = (FileBase)new ImageGNFFile(aChunkStorageType, this.ArchiveName, aMaxChunks, aSingleMipChunkArea);
                    break;
            }
            fileBase.LoadFromFile(this.looseFilename, aCompressionType);
            return fileBase;
        }

        protected override void ExtractToImpl(string asFileAndPath)
        {
            File.Copy(this.looseFilename, asFileAndPath);
        }
    }
}

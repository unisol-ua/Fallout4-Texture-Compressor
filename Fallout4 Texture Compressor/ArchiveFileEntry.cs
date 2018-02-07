using Archive2Interop;

namespace Fallout4_Texture_Compressor
{
    internal class ArchiveFileEntry : FileEntry
    {
        private FileBase archivedFile;
        private bool dataLoaded;
        private string archiveFileAndPath;

        public ArchiveFileEntry(FileBase aFileBase, string aArchiveFileAndPath, bool aDataLoaded)
          : base(aFileBase.Name, aFileBase.GetFileType())
        {
            this.archivedFile = aFileBase;
            this.dataLoaded = aDataLoaded;
            this.archiveFileAndPath = aArchiveFileAndPath;
        }

        public void EnsureDataLoaded()
        {
            if (this.dataLoaded)
                return;
            this.archivedFile.ReadDataFromArchive(this.archiveFileAndPath);
            this.dataLoaded = true;
        }

        protected override bool CanBeExportedAsFormatImpl(FileFormat aFormat)
        {
            return false;
        }

        protected override FileBase CreateFileAsFormatImpl(FileFormat aFormat, FileChunkStorageType aChunkStorageType, CompressionType aCompressionType, uint aMaxChunks, uint aSingleMipChunkArea)
        {
            this.EnsureDataLoaded();
            return this.archivedFile;
        }

        protected override void ExtractToImpl(string asFileAndPath)
        {
            this.EnsureDataLoaded();
            this.archivedFile.WriteToFile(asFileAndPath);
        }
    }
}

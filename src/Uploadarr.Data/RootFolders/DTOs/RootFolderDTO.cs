namespace Uploadarr.Data
{
    public class RootFolderDTO : DtoBase
    {
        public string Path { get; set; }
        public RootFolderTypeDTO Type { get; set; }
        public bool Accessible { get; set; }
        public long? FreeSpace { get; set; }
        public long? TotalSpace { get; set; }

    }
}

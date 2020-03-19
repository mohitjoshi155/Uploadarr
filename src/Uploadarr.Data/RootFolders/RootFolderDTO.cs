namespace Uploadarr.Data
{
    public class RootFolderDTO
    {
        public string Path { get; set; }

        public bool Accessible { get; set; }
        public long? FreeSpace { get; set; }
        public long? TotalSpace { get; set; }

    }
}

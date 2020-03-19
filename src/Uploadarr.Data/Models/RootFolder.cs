using System.Collections.Generic;

namespace Uploadarr.Data
{
    public class RootFolder : ModelBase
    {
        public string Path { get; set; }

        public bool Accessible { get; set; }
        public long? FreeSpace { get; set; }
        public long? TotalSpace { get; set; }

        public List<UnmappedFolder> UnmappedFolders { get; set; }
    }
}

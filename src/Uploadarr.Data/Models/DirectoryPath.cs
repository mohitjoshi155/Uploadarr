using System.ComponentModel.DataAnnotations.Schema;

namespace Uploadarr.Data
{
    public class DirectoryPath
    {
        [Column(Order = 1)]
        public int Id { get; set; }

        public string Path { get; set; }

        public DirectoryPathType PathType { get; set; }
    }
}

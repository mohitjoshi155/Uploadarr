using System.ComponentModel.DataAnnotations.Schema;

namespace Uploadarr.Data
{
    public class DirectoryPathType
    {
        [Column(Order = 1)]
        public int Id { get; set; }

        public string Type { get; set; }
    }
}

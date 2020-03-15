using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Uploadarr.Common
{
    public class DirectoryPath
    {
        [Column(Order = 1)]
        public int Id { get; set; }

        public string Path { get; set; }

        public DirectoryPathType PathType { get; set; }
    }
}

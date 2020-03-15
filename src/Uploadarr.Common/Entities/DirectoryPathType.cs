using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Uploadarr.Common
{
    public class DirectoryPathType
    {
        [Column(Order = 1)]
        public int Id { get; set; }

        public string Type { get; set; }
    }
}

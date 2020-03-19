using System.Diagnostics;

namespace Uploadarr.Data
{
    [DebuggerDisplay("{GetType()} ID = {Id}")]
    public abstract class ModelBase
    {
        public int Id { get; set; }
    }
}

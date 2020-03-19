namespace Uploadarr.Data
{
    public class RootFolderType : ModelBase
    {
        public string Name { get; set; }

        public RootFolderTypeEnum ToEnum => (RootFolderTypeEnum)Id;
    }
}

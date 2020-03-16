namespace Uploadarr.Common
{

    public interface IOsVersionAdapter
    {
        bool Enabled { get; }
        OsVersionModel Read();
    }
}
using System;
using System.IO;
using EnsureThat;
using EnsureThat.Enforcers;

namespace Uploadarr.Common
{
    public static class GuardExtensions
    {
        public static void IsValidPath(this StringArg strArg, string path)
        {
            Ensure.That(path).IsNotEmptyOrWhiteSpace();

            if (path.IsPathValid())
            {
                return;
            }
            
            // Check if path is valid length
            try
            {
                Path.GetFullPath(path);
            }
            catch (PathTooLongException ex)
            {
                throw new PathTooLongException("Please keep the filepath under 240 chars so that you still are able to provide a name for the file.", ex);
            }


            if (OsInfo.IsWindows)
            {
                throw new ArgumentException($"value [{path}] is not a valid Windows path. paths must be a full path eg. C:\\Windows");
            }

            throw new ArgumentException($"value [{path}] is not a valid *nix path. paths must start with /");

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace DriveManagement
{
    public static class DriveOperations
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<DriveDto> GetLocalDrives()
        {
            return Directory.GetLogicalDrives().Select((X,index) => new DriveDto() { DriveLetter = X, IsSelected = true, Order = index }).ToList();           
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="drivePath"></param>
       /// <returns></returns>
        public static List<LocalFileDto> GetLocalFiles(string drivePath)
        {
            var localFileList = new List<LocalFileDto>();

            var paths = Traverse(drivePath);
            localFileList = paths.Select(X => new LocalFileDto() { DriveLetter = drivePath, FileExtension = Path.GetExtension(X), FileName = Path.GetFileName(X), FullPath = Path.GetDirectoryName(X) }).ToList();
            return localFileList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <returns></returns>
        private static IEnumerable<string> Traverse(string rootDirectory)
        {
            IEnumerable<string> files = Enumerable.Empty<string>();
            IEnumerable<string> directories = Enumerable.Empty<string>();
            try
            {
                // The test for UnauthorizedAccessException.
                var permission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, rootDirectory);
                permission.Demand();

                files = Directory.GetFiles(rootDirectory);
                directories = Directory.GetDirectories(rootDirectory);
            }
            catch
            {
                // Ignore folder (access denied).
                rootDirectory = null;
            }

            if (rootDirectory != null)
                yield return rootDirectory;

            foreach (var file in files)
            {
                yield return file;
            }

            // Recursive call for SelectMany.
            var subdirectoryItems = directories.SelectMany(Traverse);
            foreach (var result in subdirectoryItems)
            {
                yield return result;
            }
        }
    }

    
}

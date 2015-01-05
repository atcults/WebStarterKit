using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.SystemSettings
{
    public static class Globals
    {
        private static string _rootPath;

        public static string RootFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_rootPath)) return _rootPath;
                
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                
                var di = new FileInfo(path).Directory;
                
                while (di != null && (di != di.Root))
                {
                    if (di.GetDirectories().Any(x => x.Name == "Deployment")) break;
                    di = di.Parent;
                }

                if (di == null || di == di.Parent) throw new Exception("Invalid directory structure");

                _rootPath = di.FullName + "\\";

                return _rootPath;
            }
        }

        public static string LogFolder
        {
            get
            {
                return CreateOrGetCustomPath("Logs\\");
            }
        }

        public static string ConfigFolder
        {
            get
            {
                return CreateOrGetCustomPath("Deployment\\Configs\\");
            }
        }

        public static string DatabaseScriptFolder
        {
            get
            {
                return CreateOrGetCustomPath("Deployment\\SqlScripts\\");
            }
        }

        public static string CreateOrGetCustomPath(string relativePath)
        {
            var path = RootFolder + relativePath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
    }
}
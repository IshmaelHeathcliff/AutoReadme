using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoSummary
{
    class Program
    {
        static StringBuilder _summary = new StringBuilder();
        static bool _isRoot = true;
        static String _tab = "    ";
        static int _tabCount = 0;

        public static void AutoSummary(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);


            if (_isRoot)
            {
                _summary.Append("# SUMMARY\n");
                _isRoot = false;
            }
            
            FileSystemInfo[] subDirsAndFilesInfo = dirInfo.GetFileSystemInfos();

            foreach (FileSystemInfo fsInfo in subDirsAndFilesInfo)
            {
                if (fsInfo is DirectoryInfo)
                {
                    if (fsInfo.Name == "assets") continue;

                    for (int i = 0; i < _tabCount; i++) _summary.Append(_tab);
                    _summary.Append($"- {fsInfo.Name}\n");
                    _tabCount += 1;
                    AutoSummary(fsInfo.FullName);
                }
                else if(fsInfo.Name.EndsWith(".md"))
                {
                    if (fsInfo.Name == "SUMMARY.md") continue;
                        for (int i = 0; i < _tabCount; i++) _summary.Append(_tab);
                    _summary.Append(
                        $"- [{fsInfo.Name.Split(new[]{".md"}, StringSplitOptions.RemoveEmptyEntries)[0]}]({dir}\\{fsInfo.Name})\n"
                        );
                }
            }

            _tabCount -= 1;
        }

        public static void Main(string[] args)
        {
            AutoSummary(".");
            var writer = new StreamWriter("SUMMARY.md");
            using (writer)
            {
                writer.Write(_summary.ToString());
            }
        }
    }
}
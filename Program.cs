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
        static int _level = 2;
        static Stack<string> _dirStack = new Stack<string>();


        static bool CheckNameIgnore(string name)
        {
            var nameIgnore = new List<string>
            {
                "SUMMARY.md",
                "README.md",
                "readme.md",
                "readme.txt",
                "assets",
                "photos",
                ".git"
            };
            
            foreach (string n in nameIgnore)
            {
                if (name == n) return true;
            }

            return false;
        }

        static void DirectorySummary(FileSystemInfo fsInfo)
        {
            for (int i = 0; i < _level; i++) _summary.Append("#");
            _summary.Append($" {fsInfo.Name}\n\n");
            _level += 1;
            _dirStack.Push(_dirStack.Peek() + fsInfo.Name + "\\");
        }

        static void FileSummary(FileSystemInfo fsInfo)
        {
            string fileName = fsInfo.Name.Split(new[] {".md"}, StringSplitOptions.RemoveEmptyEntries)[0];
            string dirName = _dirStack.Peek();
            _summary.Append($"- [{fileName}]({dirName}{fsInfo.Name})\n");
        }

        static void AutoSummary(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);


            if (_isRoot)
            {
                _summary.Append("## Introduction\n\n");

                using (var reader = new StreamReader("readme.txt"))
                {
                    _summary.Append(reader.ReadToEnd() + "\n\n");
                }
                
                _dirStack.Push("");
                
                _isRoot = false;
            }
            
            FileSystemInfo[] subDirsAndFilesInfo = dirInfo.GetFileSystemInfos();

            foreach (FileSystemInfo fsInfo in subDirsAndFilesInfo)
            {
                if (CheckNameIgnore(fsInfo.Name)) continue;

                if (fsInfo is DirectoryInfo)
                {
                    DirectorySummary(fsInfo);
                    AutoSummary(fsInfo.FullName);
                }
                else if(fsInfo.Name.EndsWith(".md"))
                {
                    FileSummary(fsInfo);
                }
            }

            _summary.Append("\n");
            _level -= 1;
            _dirStack.Pop();
        }

        public static void Main(string[] args)
        {
            AutoSummary(".");
            var writer = new StreamWriter("README.md");
            using (writer)
            {
                writer.Write(_summary.ToString());
            }
        }
    }
}
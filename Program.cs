using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoReadme
{
    class Program
    {
        static StringBuilder _summary = new StringBuilder();
        static bool _isRoot = true;
        static int _level = 2;
        static Stack<string> _dirStack = new Stack<string>();
        static string[] _extensions = {
            ".md",
            ".txt",
            ".puml",
            ".plantuml"
        };


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

        static bool CheckFileExtension(string name)
        {
            foreach (string e in _extensions)
            {
                if (name.EndsWith(e)) return true;
            }

            return false;

        }

        static void DirectorySummary(FileSystemInfo fsInfo)
        {
            for (int i = 0; i < _level; i++) _summary.Append("#");
            _summary.Append($" {fsInfo.Name}\n\n");
            _level += 1;
            _dirStack.Push(_dirStack.Peek() + fsInfo.Name + "/");
        }

        static void FileSummary(FileSystemInfo fsInfo)
        {
            string fileName = fsInfo.Name.Split(_extensions, StringSplitOptions.RemoveEmptyEntries)[0];
            string dirName = _dirStack.Peek();
            _summary.Append($"- [{fileName}]({dirName}{fsInfo.Name})\n");
        }

        static int FsComparer(FileSystemInfo fileA, FileSystemInfo fileB)
        {
            int a, b;
            a = fileA is DirectoryInfo ? 1 : 0;
            b = fileB is DirectoryInfo ? 1 : 0;
            if (a < b) return -1;
            return a > b ? 1 : 0;
        }

        static void AutoReadme(string dir)
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
            Array.Sort(subDirsAndFilesInfo, FsComparer);

            foreach (FileSystemInfo fsInfo in subDirsAndFilesInfo)
            {
                if (CheckNameIgnore(fsInfo.Name)) continue;

                if (fsInfo is DirectoryInfo)
                {
                    if (((DirectoryInfo) fsInfo).GetFileSystemInfos().Length == 0) continue;
                    DirectorySummary(fsInfo);
                    AutoReadme(fsInfo.FullName);
                }
                else if(CheckFileExtension(fsInfo.Name))
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
            AutoReadme(".");
            var writer = new StreamWriter("README.md");
            using (writer)
            {
                writer.Write(_summary.ToString());
            }
        }
    }
}
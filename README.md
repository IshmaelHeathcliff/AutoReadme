## Introduction

此工具可自动创建目录下markdown文件的结构和引用

结果将保存在README.md中

build后将bin/Debug/AutoReadme.exe复制到相应目录下双击

## Attention

会将readme.txt的内容放在Introduction下，请自行创建文件，建议将其加入.gitignore

会加入以下结尾的文件

-   md
-   txt
-   puml
-   platuml

会忽略如下文件和目录名

-   SUMMARY.md

-   README.md

-   readme.md

-   readme.txt

-   assets

-   photos

-   .git

具体请查看Program.cs中的CheckNameIgnore方法中的nameIgnore
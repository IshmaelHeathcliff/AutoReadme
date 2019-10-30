## Introduction

此工具可自动创建目录下markdown文件的结构和引用

将AutoSummary.exe放在目标文件夹中并双击运行

结果将保存在README.md中

## Attention

会将readme.txt的内容放在Introduction下，请自行创建文件，建议将其加入.gitignore

只会加入.md结尾的文件

会忽略如下文件和目录名

-   SUMMARY.md

-   README.md

-   readme.md

-   readme.txt

-   assets

-   photos

-   .git

具体请查看Program.cs中的CheckNameIgnore方法中的nameIgnore
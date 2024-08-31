#!/bin/bash
# Fati Iseni

WorkingDir="$(pwd)"

########## Make sure you're not deleting your whole computer :)
safetyCheck()
{
if [[ ( "$WorkingDir" = "" || "$WorkingDir" = "/" || "$WorkingDir" = '/c' || "$WorkingDir" = '/d' || "$WorkingDir" = 'c:\' || "$WorkingDir" = 'd:\' || "$WorkingDir" = 'C:\' || "$WorkingDir" = 'D:\') ]]; then
	echo "Please cross check the WorkingDir value";
	exit 1;
fi
}

########## Delete .vs directories.
deleteVSDir()
{
echo "Deleting .vs files and directories...";

find "$WorkingDir/" -type d -name ".vs" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete bin and obj directories.
cleanBinObj()
{
echo "Deleting bin and obj directories...";

find "$WorkingDir/" -type d -name "bin" -exec rm -rf {} \; > /dev/null 2>&1;
find "$WorkingDir/" -type d -name "obj" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete Logs directories.
cleanLogs()
{
echo "Deleting Logs directories...";

find "$WorkingDir/" -type d -name "Logs" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete .csproj.user files
cleanUserFiles()
{
echo "Deleting *.csproj.user files...";

find "$WorkingDir/" -type f -name "*.csproj.user" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete all unused local branches.
cleanLocalGitBranches()
{
echo "Deleting local unused git branches (e.g. no corresponding remote branch)...";

git fetch -p && git branch -vv | awk '/: gone\]/{print $1}' | xargs -I {} git branch -D {}
}

safetyCheck;
echo "";

if [ "$1" = "vs" ]; then
	deleteVSDir;
elif [ "$1" = "logs" ]; then
	cleanLogs;
elif [ "$1" = "user" ]; then
	cleanUserFiles;
elif [ "$1" = "branches" ]; then
	cleanLocalGitBranches;
else
	cleanBinObj;
fi

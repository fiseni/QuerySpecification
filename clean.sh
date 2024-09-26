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
deleteBinObj()
{
echo "Deleting bin and obj directories...";

find "$WorkingDir/" -type d -name "bin" -exec rm -rf {} \; > /dev/null 2>&1;
find "$WorkingDir/" -type d -name "obj" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete Logs directories.
deleteLogs()
{
echo "Deleting Logs directories...";

find "$WorkingDir/" -type d -name "Logs" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete .csproj.user files
deleteUserCsprojFiles()
{
echo "Deleting *.csproj.user files...";

find "$WorkingDir/" -type f -name "*.csproj.user" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete test and coverage artifacts
deleteTestResults()
{
echo "Deleting test and coverage artifacts...";

find "$WorkingDir/" -type d -name "TestResults" -exec rm -rf {} \; > /dev/null 2>&1;
}

########## Delete all unused local branches.
deleteLocalGitBranches()
{
echo "Deleting local unused git branches (e.g. no corresponding remote branch)...";

git fetch -p && git branch -vv | awk '/: gone\]/{print $1}' | xargs -I {} git branch -D {}
}

safetyCheck;
echo "";

if [ "$1" = "help" ]; then
	echo "Usage: clean.sh [bin|vs|logs|user|coverages|branches]";
elif [ "$1" = "bin" ]; then
	deleteBinObj;
elif [ "$1" = "vs" ]; then
	deleteVSDir;
elif [ "$1" = "logs" ]; then
	deleteLogs;
elif [ "$1" = "user" ]; then
	deleteUserCsprojFiles;
elif [ "$1" = "coverages" ]; then
	deleteTestResults;
elif [ "$1" = "branches" ]; then
	deleteLocalGitBranches;
else
	deleteBinObj;
fi

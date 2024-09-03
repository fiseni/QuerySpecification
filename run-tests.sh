#!/bin/bash

find . -type d -name TestResults -exec rm -rf {} \; > /dev/null 2>&1

testtarget="$1"

if [ "$testtarget" = "" ]; then
testtarget="QuerySpecification.sln"
fi

dotnet build "$testtarget" --configuration Release
dotnet test "$testtarget" --configuration Release --no-build --no-restore --collect:"xplat code coverage"
reportgenerator -reports:tests/**/coverage.cobertura.xml -targetdir:TestResults -assemblyfilters:"-*Tests*;"

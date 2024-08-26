#!/bin/bash

rootFolder="/home/finn/Lang3"
CompiledSourceFolder="$rootFolder/bin/Debug/net8.0"

compBuild="-cb"
compTemp="-ct"

noCompile="-nc"
noRun="-nr"

rmCompExe=true

if [[ "$@" == *"$compBuild"* ]]; then
    dotnet build "$rootFolder/Lang3.csproj" --nologo
fi

if [[ "$@" != *"$noCompile"* && $? != 1 ]]; then
    rmCompExe=false
    mArgs="${@//$compBuild/}"
    mArgs="${mArgs//$compTemp/}"

    "$CompiledSourceFolder/Lang3" $mArgs
fi

if [[ $rmCompExe || "$@" == *"$compTemp"* ]]; then
    rm -f "$rootFolder/CompiledSource/"*
fi

$rootFolder = "C:\Users\finne\OneDrive\Documents\0coding\Lang3\Lang3"

$compBuild = "-cb"
$compTemp = "-ct"

$noCompile = "-nc"
$noRun = "-nr"

if ($args -contains $compBuild) {
    dotnet build "$rootFolder\Lang3.csproj" -o "$rootFolder\CompiledSource\" --os win --nologo -v q
}

if (!($args -contains $noCompile -or $LASTEXITCODE -eq 1)) {
    $rmCompExe = $args -contains $compTemp
    $mArgs = $args -replace $compBuild, "" -replace $compTemp, "";

    &"$rootFolder\CompiledSource\Lang3.exe" $mArgs
}

if ($rmCompExe -or $args -contains $compTemp) {
    #Remove-Item "$rootFolder\Lang3\CompiledSource\Lang3.exe"
    $srcPath = "$rootFolder\CompiledSource\"
    Get-ChildItem -Path $srcPath | ForEach-Object -Process { 
            Remove-Item -Path $_.FullName -Force
    }
}
param(
# Full path to NuGet.exe
$aNuGetExe,
# Full path to Squirrel.exe
$aSquirrelExe,
# Output directory including trailing backslash
$aOutDir,
# Version string typically in the following format: "1.2.3"
$aVersion,
# Full path to nuspec
$aNuSpecPath,
# Application download URL, including trailing forwardslash
$aUrl,
# Application ID, should match the name of the NuSpec file
$aAppId
)

# Generate nupkg from nuspec, version and output directory
Write-Output("Generating nupkg...")
[System.Diagnostics.Process]::Start($aNuGetExe,
"pack $aNuSpecPath -Properties Configuration=Release;Version=$aVersion -OutputDirectory $aOutDir -BasePath $aOutDir"
).WaitForExit()

# Create download folder below our output directory, if needed
$squirrelReleaseDir = $aOutDir + "Squirrel\";
if (-not(Test-Path $squirrelReleaseDir))
{
    New-Item $squirrelReleaseDir -ItemType Directory | Out-Null;
}

# Download RELEASES file
Write-Output("Downloading Squirrel RELEASES...")
$localFileName = $squirrelReleaseDir + "RELEASES"
$remoteFileName = $aUrl + "RELEASES"
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

# Parse RELEASES file to obtain the name of our last package
$reader = [System.IO.File]::OpenText( $localFileName )
while($null -ne ($line = $reader.ReadLine())) {
    $lastFileName = $line.Split(" ")[1]	
}
$reader.Close();

# Work out version number
$version = $lastFileName.Split("-")[1]
$major = $version.Split(".")[0]
$minor = $version.Split(".")[1]
$build = $version.Split(".")[2]

if ($aVersion -eq $version)
{
    # Warn
    Write-Warning ("Version $version already published!")
    # Delete downloaded RELEASES to avoid uploading them back pointlessly 
    Remove-Item $localFileName
    # Still a successful exit as this should not fail the build
    exit 0
}

# Download last package
Write-Output("Downloading last Squirrel package...")
$localFileName = $squirrelReleaseDir + $lastFileName
$remoteFileName = $aUrl + $lastFileName
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

# Do our Squirrel release
Write-Output("Generate Squirrel release...")
[System.Diagnostics.Process]::Start($aSquirrelExe,
" --r $squirrelReleaseDir --releasify $aOutDir$aAppId.$aVersion.nupkg").WaitForExit()

# Clean-up by removing the downloaded Squirrel package
Remove-Item $localFileName

# From here on the Squirrel folder contains only stuff we need to upload

# Success
exit 0
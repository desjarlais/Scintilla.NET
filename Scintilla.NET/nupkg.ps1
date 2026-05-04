param (
    [Parameter(Mandatory, ParameterSetName="Clean")][switch]$Clean,
    [Parameter(Mandatory, ParameterSetName="Add")][switch]$Add,
    [Parameter(Mandatory)][string]$MSBuildProjectDirectory,
    [Parameter(Mandatory, ParameterSetName="Clean")][string]$NuGetPackageRoot,
    [Parameter(Mandatory)][string]$SolutionDir,
    [Parameter(Mandatory)][string]$PackageOutputPath,
    [Parameter(Mandatory)][string]$PackageId,
    [Parameter(Mandatory)][string]$PackageVersion
)

Set-Location -Path $MSBuildProjectDirectory

$NuGetPackageSourceDir = Join-Path $SolutionDir "registry"

if ($Clean) {
    $PackageInRegistry = Join-Path $NuGetPackageSourceDir $PackageId
    $PackageInRoot = Join-Path $NuGetPackageRoot $PackageId
    $TestAppObjDir = Join-Path $SolutionDir "Scintilla.NET.TestApp\obj"
    
    Remove-Item $PackageInRegistry -Recurse -Force -ErrorAction Ignore
    Remove-Item $PackageInRoot -Recurse -Force -ErrorAction Ignore
    Get-ChildItem $TestAppObjDir -Filter "*.nuget.*" -ErrorAction Ignore | Remove-Item -Force -ErrorAction Ignore
}

if ($Add) {
    $PackageVersionList = $PackageVersion.Split('.')
    
    if ($PackageVersionList.Count -Eq 4 -And [int]::Parse($PackageVersionList[3]) -Eq 0) {
        $PackageVersion = [string]::Join('.', $PackageVersionList[0..2])
    }
    
    $PackagePath = Join-Path $PackageOutputPath "$PackageId.$PackageVersion.nupkg"
    
    nuget add $PackagePath -Source $NuGetPackageSourceDir
}

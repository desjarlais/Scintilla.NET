param (
	[Parameter(Mandatory=$true)][string]$MSBuildProjectDirectory,
	[Parameter(Mandatory=$true)][string]$NuGetPackageRoot,
	[Parameter(Mandatory=$true)][string]$NuGetPackageSourceDir,
	[Parameter(Mandatory=$true)][string]$PackageOutputPath,
	[Parameter(Mandatory=$true)][string]$PackageId,
	[Parameter(Mandatory=$true)][string]$PackageVersion
)

Set-Location -Path $MSBuildProjectDirectory

$PackageVersionList = $PackageVersion.Split('.')

if ($PackageVersionList.Count -Eq 4 -And [int]::Parse($PackageVersionList[3]) -Eq 0) {
	$PackageVersion = [string]::Join('.', $PackageVersionList[0..2])
}

$LocalNuGetPackageRegistry = [System.IO.Path]::GetFullPath((Join-Path $NuGetPackageSourceDir ..\registry))
$PackagePath = Join-Path $PackageOutputPath "$PackageId.$PackageVersion.nupkg"

$PackageInRegistry = Join-Path $LocalNuGetPackageRegistry $PackageId
$PackageInRoot = Join-Path $NuGetPackageRoot $PackageId

Remove-Item $PackageInRegistry -Recurse -Force -ErrorAction Ignore
Remove-Item $PackageInRoot -Recurse -Force -ErrorAction Ignore

nuget add $PackagePath -Source $LocalNuGetPackageRegistry

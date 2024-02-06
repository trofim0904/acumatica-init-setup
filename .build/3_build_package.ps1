param (
    [string]$srcPath,
    [string]$packageName,
    [string]$version,
    [Parameter(Mandatory=$false)][int]$packageLevel
)

$build_tool_path = "./tool/CI_tool/CustomizationPackageTools.exe"

# remove existing package
Remove-Item -Path "$packageName" -Force -ErrorAction SilentlyContinue

$description = "";
if (-Not [string]::IsNullOrWhiteSpace($version)) {
    $description = $version;
}

$level = 0;
if ($packageLevel -gt 0) {
    $level = $packageLevel;
}

# make an zip archive
& $build_tool_path build --customizationpath "$srcPath" --packagefilename "$packageName" --description "$description" --level $level --product-version "23.114"

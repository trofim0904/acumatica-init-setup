param (
    [string]$version
)

& ".\2_1_prepare_output_core.ps1"
& ".\3_build_package.ps1" -srcPath "output" -packageName "DTInitial.zip" -version $version -packageLevel 2

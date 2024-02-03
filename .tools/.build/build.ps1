param (
    [string]$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"
)

$current_location = Get-Location

$project_dir = (get-item $current_location ).parent.FullName

$project = "$project_dir\TrialBalanceConversionTool\TrialBalanceConversionTool.sln"

Remove-Item -Path "$project_dir\output\TrialBalanceConversionTool*" -Force -ErrorAction SilentlyContinue

#run msbuild.exe for release file building
& $msbuild $project `
    /t:Build /p:Configuration=Release /p:TargetFramework=v4.7.2 /p:OutputPath="$project_dir/output"

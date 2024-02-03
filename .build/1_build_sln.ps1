param (
    [string]$msbuildPath
)

# Returns msbuild path for default installation of VS2019, VS2017, VS2022 Professional, Enterprise and Community editions
function Get-MsBuild-Path {
    $msbuildPathList = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2017\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files (x86)\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\JetBrains\JetBrains Rider 2023.1.3\tools\MSBuild\Current\Bin\MSBuild.exe",
                       "C:\Program Files\JetBrains\JetBrains Rider 2023.2.2\tools\MSBuild\Current\Bin\MSBuild.exe"

    Foreach($msPath in $msbuildPathList) {
        if (Test-Path $msPath) {
            return $msPath
        }
    }
}

if ([string]::IsNullOrWhiteSpace($msbuildPath)) {
    $msbuildPath = Get-MsBuild-Path
}
Write-Host "MsBuld path:" $msbuildPath

$sln_path = "..\customization\src\VestaEnergy\VestaEnergy.sln"

# Restore NuGet packages
& "..\.nugets\nuget.exe" restore "$sln_path"

# Compile customization project
& "$msbuildPath" /t:Rebuild /p:Configuration=Release "$sln_path"
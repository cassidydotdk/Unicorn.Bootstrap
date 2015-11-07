param($scriptRoot)

$ErrorActionPreference = "Stop"

$msBuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"
#$msBuild = "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
$nuGet = "$scriptRoot..\tools\NuGet.exe"
$solution = "$scriptRoot\..\Unicorn.Bootstrap.sln"

& $nuGet restore $solution
& $msBuild $solution /p:Configuration=Release /t:Rebuild /m

$UnicornBootstrapAssembly = Get-Item "$scriptRoot\..\src\Unicorn.Bootstrap\bin\Release\Unicorn.Bootstrap.dll" | Select-Object -ExpandProperty VersionInfo
$targetAssemblyVersion = $UnicornBootstrapAssembly.ProductVersion

$unicornVersion = "3.0.2"

& $nuGet pack "$scriptRoot\..\src\Unicorn.Bootstrap\Unicorn.Bootstrap.csproj" -Symbols -Prop "Configuration=Release;unicornVersion=$unicornVersion"

COPY *.nupkg "D:\NuGet Repository"
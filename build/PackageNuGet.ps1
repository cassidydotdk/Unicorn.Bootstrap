param($scriptRoot)

$ErrorActionPreference = "Stop"

$msBuild = "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
$nuGet = "$scriptRoot..\tools\NuGet.exe"
$solution = "$scriptRoot\..\Unicorn.Bootstrap.sln"

& $nuGet restore $solution
& $msBuild $solution /p:Configuration=Release /t:Rebuild /m

$targetAssemblyVersion = "1.0.0.0"
$unicornVersion = "3.0.2"

& $nuGet pack "$scriptRoot\Unicorn.Bootstrap.nuget\Unicorn.Bootstrap.nuspec" -version $targetAssemblyVersion -Prop "unicornVersion=$unicornVersion"

& $nuGet pack "$scriptRoot\..\src\Unicorn.Bootstrap\Unicorn.Bootstrap.csproj" -Symbols -Prop "Configuration=Release;unicornVersion=$unicornVersion"
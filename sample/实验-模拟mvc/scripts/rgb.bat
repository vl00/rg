@echo off & title rgb

cd /d "%~dp0..\TestCL.NetCore"
dotnet restore

cd /d "%~dp0src\rgb"
del "log*.log" 1>nul 2>nul
del "rg.info.json" 1>nul 2>nul
call "..\..\..\..\rgb\rgb.exe" "rg.csproj"
copy /y "rg.info.json" "..\rg\rg.info.json"

pause
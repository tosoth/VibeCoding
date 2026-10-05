@echo off
set CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe
"%CSC%" /nologo /target:winexe /codepage:65001 /out:VideoCutter.exe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll VideoCutter.cs
if %ERRORLEVEL%==0 (echo 编译成功: VideoCutter.exe) else (echo 编译失败)
pause

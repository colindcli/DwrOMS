::文件必须是gb2312编码保存
@echo off
set FolderName=%cd%
set dis=%~d0
::找到所有的sln文件，如果想找到特定的，可以修改.sln处
for /f "delims=\" %%a in ('dir /b /a-d /o-d "%FolderName%\*.sln"') do (
? set names=%%a
)
::这里更换你要编译的版本（路径）
cd C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319
C:
@echo on
MSBuild.exe "%FolderName%\%names%"
@echo off
echo Click any key to start deleting the bin/obj/Logs folder!
pause>nul
@echo off
cd %FolderName%
%dis%
@echo on
::删除obj和bin、Logs目录
for /f "tokens=*" %%a in ('dir obj /b /ad /s ^|sort') do rd "%%a" /s/q
for /f "tokens=*" %%a in ('dir bin /b /ad /s ^|sort') do rd "%%a" /s/q
for /f "tokens=*" %%a in ('dir Logs /b /ad /s ^|sort') do rd "%%a" /s/q
del *.sln.cache
@echo off
echo Click any key to exit!
pause>nul
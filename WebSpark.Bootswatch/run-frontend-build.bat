@echo off
echo ===== Running WebSpark.Bootswatch Frontend Build =====
cd %~dp0
echo Current directory: %CD%

echo Running npm clean...
call npm run clean

echo Running npm build...
call npm run build

echo ===== Frontend Build Complete =====

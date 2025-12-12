@echo off
echo ======================================
echo Triple Calendar Build Script
echo ======================================
echo.

echo [1/3] Checking environment...
where node >nul 2>nul
if %errorlevel% neq 0 (
    echo Error: Node.js not found!
    echo Please install Node.js from https://nodejs.org
    pause
    exit /b 1
)
for /f "tokens=*" %%i in ('node --version') do set NODE_VERSION=%%i
echo Node.js: %NODE_VERSION%

where npm >nul 2>nul
if %errorlevel% neq 0 (
    echo Error: npm not found!
    pause
    exit /b 1
)
for /f "tokens=*" %%i in ('npm --version') do set NPM_VERSION=%%i
echo npm: %NPM_VERSION%

echo.
echo [2/3] Installing dependencies...
call npm install
if %errorlevel% neq 0 (
    echo Error installing dependencies
    pause
    exit /b 1
)
echo Dependencies installed

echo.
echo [3/3] Building application...
call npm run build
if %errorlevel% neq 0 (
    echo Build failed
    pause
    exit /b 1
)
echo Build completed successfully!

echo.
echo ======================================
echo Build output is in the 'dist' folder
echo ======================================
pause

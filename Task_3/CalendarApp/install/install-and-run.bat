@echo off
echo ======================================
echo Triple Calendar Installation ^& Setup
echo ======================================
echo.

echo [1/5] Checking Node.js...
where node >nul 2>nul
if %errorlevel% neq 0 (
    echo Error: Node.js not found!
    echo Please install Node.js from https://nodejs.org
    pause
    exit /b 1
)
for /f "tokens=*" %%i in ('node --version') do set NODE_VERSION=%%i
echo Node.js is installed: %NODE_VERSION%

echo.
echo [2/5] Installing core dependencies...
call npm install vue@3.5.0 pinia@2.2.0 moment-jalaali@0.10.1 moment-hijri@2.30.0
if %errorlevel% neq 0 (
    echo Error installing core dependencies
    pause
    exit /b 1
)
echo Core dependencies installed

echo.
echo [3/5] Installing development tools...
call npm install --save-dev @vitejs/plugin-vue@5.0.0 vite@5.4.0 electron@32.0.0 electron-builder@25.0.0 concurrently@8.2.0 wait-on@7.2.0
if %errorlevel% neq 0 (
    echo Error installing development tools
    pause
    exit /b 1
)
echo Development tools installed

echo.
echo [4/5] Setting up package.json...
if exist "electron-package.json" (
    copy /y electron-package.json package.json >nul
    echo package.json updated
) else (
    echo Warning: electron-package.json not found
)

echo.
echo [5/5] Starting the application...
echo ======================================
echo Application is running...
echo Press Ctrl+C to stop the application
echo ======================================
echo.

call npm run electron:dev

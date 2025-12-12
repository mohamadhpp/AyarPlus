#!/bin/bash

echo "======================================"
echo "Triple Calendar Installation & Setup"
echo "======================================"
echo ""

echo "[1/5] Checking Node.js..."
if ! command -v node &> /dev/null
then
    echo "[ERROR] Node.js not found!"
    echo "Please install Node.js from https://nodejs.org"
    exit 1
fi
echo "[OK] Node.js is installed: $(node --version)"

echo ""
echo "[2/5] Installing core dependencies..."
npm install vue@3.5.0 pinia@2.2.0 moment-jalaali@0.10.1 moment-hijri@2.30.0
if [ $? -ne 0 ]; then
    echo "[ERROR] Error installing core dependencies"
    exit 1
fi
echo "[OK] Core dependencies installed"

echo ""
echo "[3/5] Installing development tools..."
npm install --save-dev @vitejs/plugin-vue@5.0.0 vite@5.4.0 electron@32.0.0 electron-builder@25.0.0 concurrently@8.2.0 wait-on@7.2.0
if [ $? -ne 0 ]; then
    echo "[ERROR] Error installing development tools"
    exit 1
fi
echo "[OK] Development tools installed"

echo ""
echo "[4/5] Setting up package.json..."
if [ -f "electron-package.json" ]; then
    cp electron-package.json package.json
    echo "[OK] package.json updated"
else
    echo "[WARNING] electron-package.json not found"
fi

echo ""
echo "[5/5] Starting the application..."
echo "======================================"
echo "Application is running..."
echo "Press Ctrl+C to stop the application"
echo "======================================"
echo ""

npm run electron:dev

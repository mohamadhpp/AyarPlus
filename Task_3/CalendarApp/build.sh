#!/bin/bash

echo "======================================"
echo "Triple Calendar Build Script"
echo "======================================"
echo ""

echo "[1/3] Checking environment..."
if ! command -v node &> /dev/null
then
    echo "[ERROR] Node.js not found!"
    echo "Please install Node.js from https://nodejs.org"
    exit 1
fi
echo "[OK] Node.js: $(node --version)"

if ! command -v npm &> /dev/null
then
    echo "[ERROR] npm not found!"
    exit 1
fi
echo "[OK] npm: $(npm --version)"

echo ""
echo "[2/3] Installing dependencies..."
npm install
if [ $? -ne 0 ]; then
    echo "[ERROR] Error installing dependencies"
    exit 1
fi
echo "[OK] Dependencies installed"

echo ""
echo "[3/3] Building application..."
npm run build
if [ $? -ne 0 ]; then
    echo "[ERROR] Build failed"
    exit 1
fi
echo "[OK] Build completed successfully!"

echo ""
echo "======================================"
echo "Build output is in the 'dist' folder"
echo "======================================"

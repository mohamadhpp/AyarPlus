import { app, BrowserWindow, ipcMain, Notification } from 'electron'
import path from 'path'
import { fileURLToPath } from 'url'

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

let mainWindow: BrowserWindow | null;

function createWindow()
{
    mainWindow = new BrowserWindow({
        width: 1200,
        height: 800,
        minWidth: 800,
        minHeight: 600,
        frame: true,
        backgroundColor: '#1a1a2e',
        webPreferences:
        {
            preload: path.join(__dirname, 'preload.js'),
            contextIsolation: true,
            nodeIntegration: false,
        },
        icon: path.join(__dirname, '../public/icon.png'),
    })

    // Load the app
    if (process.env.NODE_ENV === 'development' || !app.isPackaged)
    {
        mainWindow.loadURL('http://localhost:5173');
        mainWindow.webContents.openDevTools();
    }
    else
    {
        mainWindow.loadFile(path.join(__dirname, '../dist/index.html'));
    }

    mainWindow.on('closed', () =>
    {
        mainWindow = null
    })
}

app.whenReady().then(() =>
{
    createWindow()

    app.on('activate', () =>
    {
        if (BrowserWindow.getAllWindows().length === 0)
        {
            createWindow();
        }
    })
})

app.on('window-all-closed', () =>
{
    if (process.platform !== 'darwin')
    {
        app.quit();
    }
})

// Handle notifications
ipcMain.on('show-notification', (event, { title, body }: { title: string; body: string }) => {
    if (Notification.isSupported())
    {
        new Notification(
        {
            title,
            body,
            icon: path.join(__dirname, '../public/icon.png'),
        }).show();
    }
});

// Handle reminder storage
ipcMain.handle('get-reminders', async () =>
{
    return [];
})

ipcMain.handle('save-reminder', async (event, reminder: unknown) =>
{
    return { success: true };
});
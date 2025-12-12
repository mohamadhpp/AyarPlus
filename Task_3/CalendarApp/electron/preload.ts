import { contextBridge, ipcRenderer } from 'electron'

interface ElectronAPI
{
    showNotification: (data: { title: string; body: string }) => void,
    getReminders: () => Promise<unknown[]>,
    saveReminder: (reminder: unknown) => Promise<{ success: boolean }>
}

contextBridge.exposeInMainWorld('electronAPI',
{
    showNotification: (data: { title: string; body: string }) => ipcRenderer.send('show-notification', data),
    getReminders: () => ipcRenderer.invoke('get-reminders'),
    saveReminder: (reminder: unknown) => ipcRenderer.invoke('save-reminder', reminder),
} as ElectronAPI);

import { onMounted, onUnmounted } from 'vue';

type ShortcutHandler = () => void;

interface Shortcuts
{
    [key: string]: ShortcutHandler
}

export function useKeyboardShortcuts(shortcuts: Shortcuts): void
{
    const handleKeyPress = (event: KeyboardEvent): void =>
    {
        // Check for modifier keys
        const ctrl = event.ctrlKey || event.metaKey;
        const shift = event.shiftKey;
        const alt = event.altKey;
        const key = event.key.toLowerCase();

        // Build shortcut string
        let shortcutString = '';

        if (ctrl)
        {
            shortcutString += 'ctrl+';
        }

        if (shift)
        {
            shortcutString += 'shift+';
        }

        if (alt)
        {
            shortcutString += 'alt+';
        }

        shortcutString += key;

        // Execute matching shortcut
        if (shortcuts[shortcutString])
        {
            event.preventDefault()
            shortcuts[shortcutString]()
        }
    }

    onMounted(() =>
    {
        window.addEventListener('keydown', handleKeyPress)
    });

    onUnmounted(() =>
    {
        window.removeEventListener('keydown', handleKeyPress)
    });
}

// Shortcuts
export const SHORTCUTS =
{
    TOGGLE_THEME: 'ctrl+shift+t',
    OPEN_REMINDERS: 'ctrl+r',
    OPEN_SETTINGS: 'ctrl+,',
    NEW_REMINDER: 'ctrl+n',
    CLOSE_MODAL: 'escape',
    NEXT_MONTH: 'ctrl+right',
    PREV_MONTH: 'ctrl+left',
    TODAY: 'ctrl+t',
    SEARCH: 'ctrl+f'
}
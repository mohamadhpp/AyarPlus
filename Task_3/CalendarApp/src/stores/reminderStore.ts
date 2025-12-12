import { defineStore } from 'pinia'
import moment from 'moment-jalaali'

interface Reminder
{
    id: string
    title: string
    description?: string
    date: string
    time: string
    createdAt: string
    notified: boolean
}

interface ReminderInput
{
    title: string
    description?: string
    date: string
    time: string
}

interface ReminderState
{
    reminders: Reminder[]
    lastCheckTime: string | null
}

export const useReminderStore = defineStore('reminder',
{
    state: (): ReminderState =>
    ({
        reminders: [],
        lastCheckTime: null
    }),

    actions:
    {
        loadReminders(): void
        {
            const saved = localStorage.getItem('reminders');

            if (saved)
            {
                this.reminders = JSON.parse(saved);
            }
        },

        saveReminders(): void
        {
            localStorage.setItem('reminders', JSON.stringify(this.reminders))
        },

        addReminder(reminder: ReminderInput): void
        {
            const newReminder: Reminder =
            {
                id: Date.now().toString(),
                ...reminder,
                createdAt: new Date().toISOString(),
                notified: false
            }

            this.reminders.push(newReminder);
            this.saveReminders();
        },

        deleteReminder(id: string): void
        {
            this.reminders = this.reminders.filter(r => r.id !== id);
            this.saveReminders();
        },

        updateReminder(id: string, updates: Partial<Reminder>): void
        {
            const index = this.reminders.findIndex(r => r.id === id);

            if (index !== -1)
            {
                this.reminders[index] = { ...this.reminders[index], ...updates };
                this.saveReminders();
            }
        },

        checkDueReminders(onReminder?: (reminder: Reminder) => void): void
        {
            const now = moment();

            this.reminders.forEach(reminder =>
            {
                if (reminder.notified)
                {
                    return
                }

                const reminderDateTime = moment(`${reminder.date} ${reminder.time}`, 'YYYY-MM-DD HH:mm');

                // Check if reminder is due (within 1 minute window)
                const diffMinutes = reminderDateTime.diff(now, 'minutes');

                if (diffMinutes <= 0 && diffMinutes > -2)
                {
                    // Mark as notified
                    this.updateReminder(reminder.id, { notified: true });

                    // Trigger notification callback
                    if (onReminder)
                    {
                        onReminder(reminder);
                    }
                }
            })
        },

        getUpcomingReminders(count: number = 5): Reminder[]
        {
            const now = moment();
            return this.reminders
                .filter(r => !r.notified)
                .filter(r =>
                {
                    const reminderDateTime = moment(`${r.date} ${r.time}`, 'YYYY-MM-DD HH:mm')
                    return reminderDateTime.isAfter(now)
                })
                .sort((a, b) =>
                {
                    const aTime = moment(`${a.date} ${a.time}`, 'YYYY-MM-DD HH:mm')
                    const bTime = moment(`${b.date} ${b.time}`, 'YYYY-MM-DD HH:mm')
                    return aTime.diff(bTime)
                })
                .slice(0, count);
        }
    }
})
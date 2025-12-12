<script setup>

import { ref, onMounted, watch } from 'vue'
import DateConverter from './components/shared/layout/DateConverter.vue'
import CalendarGrid from './components/shared/layout/CalendarGrid.vue'
import RemindersModal from './components/modals/RemindersModal.vue'
import SettingsModal from './components/modals/SettingsModal.vue'
import NotificationToast from './components/notification/NotificationToast.vue'
import { useReminderStore } from './stores/reminderStore.ts'
import { useKeyboardShortcuts } from './composables/useKeyboardShortcuts.ts'

const reminderStore = useReminderStore();
const reminders = ref([]);

const selectedDate = ref(null);
const showReminders = ref(false);
const showSettings = ref(false);
const theme = ref('light');

const notification = ref({
    show: false,
    title: '',
    message: ''
})

// Setup keyboard shortcuts
useKeyboardShortcuts({
    'ctrl+shift+t': toggleTheme,
    'ctrl+r': openReminders,
    'ctrl+,': openSettings,
    'escape': closeModals
});

onMounted(() =>
{
    // Load theme from localStorage
    const savedTheme = localStorage.getItem('theme') || 'light';
    theme.value = savedTheme;

    document.documentElement.setAttribute('data-theme', savedTheme);

    // Load reminders
    reminderStore.loadReminders();
    reminders.value = reminderStore.reminders;

    // Check for due reminders every minute
    setInterval(() =>
    {
        reminderStore.checkDueReminders((reminder) =>
        {
            showNotification('Reminder', reminder.title);
        });
    }, 60000);

    // Show welcome notification
    setTimeout(() =>
    {
        showNotification('خوش آمدگویی', 'سلام، به برنامه یادآور خوش اومدید!');
    }, 1000);
})

watch(() => reminderStore.reminders, (newReminders) =>
{
    reminders.value = newReminders
}, { deep: true });

function toggleTheme()
{
    theme.value = theme.value === 'light' ? 'dark' : 'light';

    document.documentElement.setAttribute('data-theme', theme.value);
    localStorage.setItem('theme', theme.value)
}

function openReminders()
{
    showReminders.value = true;
}

function openSettings()
{
    showSettings.value = true;
}

function handleDateSelected(date)
{
    selectedDate.value = date;
}

function handleDateClick(date)
{
    selectedDate.value = date;
}

function closeModals()
{
    showReminders.value = false;
    showSettings.value = false;
}

function showNotification(title, message)
{
    notification.value =
    {
        show: true,
        title,
        message
    };

    // Also show native notification if available
    if (window.electronAPI)
    {
        window.electronAPI.showNotification({ title, body: message });
    }

    // Auto close after 5 seconds
    setTimeout(() =>
    {
        notification.value.show = false
    }, 5000);
}

</script>

<template>

    <div class="min-h-screen transition-colors duration-300 py-7 px-10"
         :data-theme="theme"
         style="background: var(--bg-primary)"
    >
        <!-- Header -->
        <header class="glass sticky top-5 z-[100] px-8 py-4">
            <div class="flex justify-between items-center max-w-[1400px] mx-auto">
                <div class="flex items-center gap-4">
                    <svg width="32" height="32" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" style="color: var(--accent-primary)">
                        <path d="M8 2V5M16 2V5M3.5 9.09H20.5M21 8.5V17C21 20 19.5 22 16 22H8C4.5 22 3 20 3 17V8.5C3 5.5 4.5 3.5 8 3.5H16C19.5 3.5 21 5.5 21 8.5Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
                        <path d="M15.6947 13.7H15.7037M15.6947 16.7H15.7037M11.9955 13.7H12.0045M11.9955 16.7H12.0045M8.29431 13.7H8.30329M8.29431 16.7H8.30329" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    </svg>

                    <h1 class="text-gradient text-2xl font-bold m-0">
                        تقویم
                    </h1>
                </div>

                <div class="flex gap-3 items-center">
                    <button @click="openReminders"
                            class="icon-btn"
                            title="یادآور"
                    >
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <path d="M12.02 2.91C8.71 2.91 6.02 5.6 6.02 8.91V11.8C6.02 12.41 5.76 13.34 5.45 13.86L4.3 15.77C3.59 16.95 4.08 18.26 5.38 18.7C9.69 20.14 14.34 20.14 18.65 18.7C19.86 18.3 20.39 16.87 19.73 15.77L18.58 13.86C18.28 13.34 18.02 12.41 18.02 11.8V8.91C18.02 5.61 15.32 2.91 12.02 2.91Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round"/>
                            <path d="M13.87 3.2C13.56 3.11 13.24 3.04 12.91 3C11.95 2.88 11.03 2.95 10.17 3.2C10.46 2.46 11.18 1.94 12.02 1.94C12.86 1.94 13.58 2.46 13.87 3.2Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
                            <path d="M15.02 19.06C15.02 20.71 13.67 22.06 12.02 22.06C11.2 22.06 10.44 21.72 9.90002 21.18C9.36002 20.64 9.02002 19.88 9.02002 19.06" stroke="currentColor" stroke-width="2" stroke-miterlimit="10"/>
                        </svg>

                        <span v-if="reminders.length > 0"
                              class="absolute -top-1 -right-1 bg-gradient-to-br from-red-400 to-red-500 text-white text-xs font-semibold px-1.5 py-0.5 rounded-[10px] min-w-[20px] text-center">
                            {{ reminders.length }}
                        </span>
                    </button>

                    <button @click="openSettings"
                            class="icon-btn"
                            title="تنظیمات"
                    >
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <path d="M12 15C13.6569 15 15 13.6569 15 12C15 10.3431 13.6569 9 12 9C10.3431 9 9 10.3431 9 12C9 13.6569 10.3431 15 12 15Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
                            <path d="M2 12.88V11.12C2 10.08 2.85 9.22 3.9 9.22C5.71 9.22 6.45 7.94 5.54 6.37C5.02 5.47 5.33 4.3 6.24 3.78L7.97 2.79C8.76 2.32 9.78 2.6 10.25 3.39L10.36 3.58C11.26 5.15 12.74 5.15 13.65 3.58L13.76 3.39C14.23 2.6 15.25 2.32 16.04 2.79L17.77 3.78C18.68 4.3 18.99 5.47 18.47 6.37C17.56 7.94 18.3 9.22 20.11 9.22C21.15 9.22 22.01 10.07 22.01 11.12V12.88C22.01 13.92 21.16 14.78 20.11 14.78C18.3 14.78 17.56 16.06 18.47 17.63C18.99 18.54 18.68 19.7 17.77 20.22L16.04 21.21C15.25 21.68 14.23 21.4 13.76 20.61L13.65 20.42C12.75 18.85 11.27 18.85 10.36 20.42L10.25 20.61C9.78 21.4 8.76 21.68 7.97 21.21L6.24 20.22C5.33 19.7 5.02 18.53 5.54 17.63C6.45 16.06 5.71 14.78 3.9 14.78C2.85 14.78 2 13.92 2 12.88Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>
                    </button>

                    <button @click="toggleTheme"
                            class="relative w-11 h-11 border-none rounded-xl text-white cursor-pointer flex items-center justify-center transition-all duration-300 hover:-translate-y-0.5 hover:rotate-[15deg]"
                            style="background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary))"
                            title="تغییر ‌زمینه"
                    >
                        <svg v-if="theme === 'light'" width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <path d="M21.5287 15.9294C21.3687 15.6594 20.9187 15.2394 19.7987 15.4394C19.1787 15.5494 18.5487 15.5994 17.9187 15.5694C15.5887 15.4694 13.4787 14.3994 12.0087 12.7494C10.7087 11.2994 9.90873 9.40938 9.89873 7.36938C9.89873 6.22938 10.1287 5.12938 10.5787 4.08938C11.0087 3.08938 10.7087 2.54938 10.4787 2.28938C10.2487 2.03938 9.65873 1.65938 8.53873 2.11938C4.85873 3.70938 2.29873 7.34938 2.29873 11.5894C2.29873 17.1794 6.67873 21.6694 12.1887 21.6694C15.5887 21.6694 18.6487 19.9594 20.4787 17.2194C21.0787 16.3194 21.6887 16.1994 21.5287 15.9294Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>

                        <svg v-else width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <path d="M12 18.5C15.5899 18.5 18.5 15.5899 18.5 12C18.5 8.41015 15.5899 5.5 12 5.5C8.41015 5.5 5.5 8.41015 5.5 12C5.5 15.5899 8.41015 18.5 12 18.5Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                            <path d="M19.14 19.14L19.01 19.01M19.01 4.99L19.14 4.86L19.01 4.99ZM4.86 19.14L4.99 19.01L4.86 19.14ZM12 2.08V2V2.08ZM12 22V21.92V22ZM2.08 12H2H2.08ZM22 12H21.92H22ZM4.99 4.99L4.86 4.86L4.99 4.99Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>
                    </button>
                </div>
            </div>
        </header>

        <!-- Main Content -->
        <main class="w-full mx-auto pb-8 pt-10">
            <!-- Date Converter Section -->
            <section class="mb-8">
                <div class="card glass">
                    <h2 class="text-xl font-semibold mb-6" style="color: var(--text-primary)">
                        تبدیل تاریخ
                    </h2>

                    <DateConverter @date-selected="handleDateSelected" />
                </div>
            </section>

            <!-- Triple Calendar View -->
            <section class="animate-slide-in [animation-delay:0.2s]">
                <CalendarGrid :selected-date="selectedDate"
                              @date-click="handleDateClick"
                />
            </section>
        </main>

        <!-- Modals -->
        <RemindersModal v-if="showReminders"
                        @close="showReminders = false"
        />

        <SettingsModal v-if="showSettings"
                       @close="showSettings = false"
        />

        <NotificationToast v-if="notification.show"
                           :title="notification.title"
                           :message="notification.message"
                           @close="notification.show = false"
        />
    </div>

</template>

<style scoped>

.icon-btn
{
    position: relative;
    width: 44px;
    height: 44px;
    border: none;
    border-radius: 12px;
    background: var(--bg-secondary);
    color: var(--text-primary);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
    border: 1px solid var(--border-color);
}

.icon-btn:hover
{
    background: var(--bg-tertiary);
    box-shadow: var(--shadow-md);
}

.icon-btn svg
{
    width: 24px;
    height: 24px;
}

@media (max-width: 768px)
{
    header
    {
        padding: 1rem !important;
    }

    h1
    {
        font-size: 1.25rem !important;
    }

    main
    {
        padding-left: 1rem !important;
        padding-right: 1rem !important;
        padding-bottom: 1rem !important;
    }

    .icon-btn
    {
        width: 40px;
        height: 40px;
    }
}

</style>
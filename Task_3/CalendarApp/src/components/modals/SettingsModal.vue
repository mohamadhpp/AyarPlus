<script setup>

import { ref, onMounted } from 'vue'

const emit = defineEmits(['close']);

const settings = ref({
    theme: 'light',
    notifications: true,
    defaultReminderTime: '12:00',
    defaultCalendar: 'jalali',
    showHolidays: true
});

onMounted(() =>
{
    loadSettings()
})

function loadSettings()
{
    const savedSettings = localStorage.getItem('settings');

    if (savedSettings)
    {
        settings.value = { ...settings.value, ...JSON.parse(savedSettings) }
    }
}

function saveSettings()
{
    localStorage.setItem('settings', JSON.stringify(settings.value));
}

</script>

<template>

    <div class="fixed inset-0 bg-opacity-75 flex items-center justify-center z-[1000] p-4"
         style="backdrop-filter: blur(4px)"
         @click.self="$emit('close')"
    >
        <div class="glass w-full max-w-[600px] max-h-[90vh] overflow-y-auto rounded-[20px] animate-fade-in hide-scrollbar">
            <div class="flex justify-between items-center p-6 border-b" style="border-color: var(--border-color)">
                <h2 class="flex items-center gap-3 m-0 text-2xl font-bold" style="color: var(--text-primary)">
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" style="color: var(--accent-primary)">
                        <path d="M12 15C13.6569 15 15 13.6569 15 12C15 10.3431 13.6569 9 12 9C10.3431 9 9 10.3431 9 12C9 13.6569 10.3431 15 12 15Z" stroke="currentColor" stroke-width="2"/>
                        <path d="M2 12.88V11.12C2 10.08 2.85 9.22 3.9 9.22C5.71 9.22 6.45 7.94 5.54 6.37C5.02 5.47 5.33 4.3 6.24 3.78L7.97 2.79C8.76 2.32 9.78 2.6 10.25 3.39L10.36 3.58C11.26 5.15 12.74 5.15 13.65 3.58L13.76 3.39C14.23 2.6 15.25 2.32 16.04 2.79L17.77 3.78C18.68 4.3 18.99 5.47 18.47 6.37C17.56 7.94 18.3 9.22 20.11 9.22C21.15 9.22 22.01 10.07 22.01 11.12V12.88C22.01 13.92 21.16 14.78 20.11 14.78C18.3 14.78 17.56 16.06 18.47 17.63C18.99 18.54 18.68 19.7 17.77 20.22L16.04 21.21C15.25 21.68 14.23 21.4 13.76 20.61L13.65 20.42C12.75 18.85 11.27 18.85 10.36 20.42L10.25 20.61C9.78 21.4 8.76 21.68 7.97 21.21L6.24 20.22C5.33 19.7 5.02 18.53 5.54 17.63C6.45 16.06 5.71 14.78 3.9 14.78C2.85 14.78 2 13.92 2 12.88Z" stroke="currentColor" stroke-width="2"/>
                    </svg>

                    تنظیمات
                </h2>

                <button @click="$emit('close')"
                        class="close-btn"
                >
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                        <path d="M18 6L6 18M6 6L18 18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    </svg>
                </button>
            </div>

            <div class="p-6">
                <div class="mb-8">
                    <h3 class="text-base font-semibold uppercase tracking-wider m-0 mb-4"
                        style="color: var(--text-secondary); letter-spacing: 0.5px"
                    >
                        اعلانات
                    </h3>

                    <div class="setting-item items-center mb-3">
                        <div class="flex items-center gap-4 flex-1">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" style="color: var(--accent-primary)" class="shrink-0">
                                <path d="M12.02 2.91C8.71 2.91 6.02 5.6 6.02 8.91V11.8C6.02 12.41 5.76 13.34 5.45 13.86L4.3 15.77C3.59 16.95 4.08 18.26 5.38 18.7C9.69 20.14 14.34 20.14 18.65 18.7C19.86 18.3 20.39 16.87 19.73 15.77L18.58 13.86C18.28 13.34 18.02 12.41 18.02 11.8V8.91C18.02 5.61 15.32 2.91 12.02 2.91Z" stroke="currentColor" stroke-width="2"/>
                            </svg>

                            <div>
                                <h4 class="m-0 mb-1 text-base font-semibold"
                                    style="color: var(--text-primary)"
                                >
                                    اعلان‌های یادآوری
                                </h4>

                                <p class="m-0 text-sm"
                                   style="color: var(--text-secondary)"
                                >
                                    نمایش اعلان‌ها برای یادآوری‌ها
                                </p>
                            </div>
                        </div>

                        <label class="toggle-switch">
                            <input type="checkbox"
                                   v-model="settings.notifications"
                                   @change="saveSettings"
                            />

                            <span class="toggle-slider"></span>
                        </label>
                    </div>

                    <div class="setting-item flex flex-col items-start">
                        <div class="flex items-start gap-4 flex-1">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" style="color: var(--accent-primary)" class="shrink-0">
                                <path d="M12 8V12L14.5 14.5" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                                <path d="M12 22C17.5228 22 22 17.5228 22 12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12C2 17.5228 6.47715 22 12 22Z" stroke="currentColor" stroke-width="2"/>
                            </svg>

                            <div>
                                <h4 class="m-0 mb-1 text-base font-semibold"
                                    style="color: var(--text-primary)"
                                >
                                    زمان یادآوری پیش‌فرض
                                </h4>

                                <p class="m-0 text-sm"
                                   style="color: var(--text-secondary)"
                                >
                                    زمان پیش‌فرض برای یادآوری‌های جدید
                                </p>
                            </div>
                        </div>

                        <select v-model="settings.defaultReminderTime"
                                @change="saveSettings"
                                class="settings-select mt-3"
                        >
                            <option value="09:00">
                                9:00 قبل از ظهر
                            </option>

                            <option value="12:00">
                                12:00 بعد از ظهر
                            </option>

                            <option value="15:00">
                                3:00 بعد از ظهر
                            </option>

                            <option value="18:00">
                                6:00 بعد از ظهر
                            </option>

                            <option value="21:00">
                                9:00 بعد از ظهر
                            </option>
                        </select>
                    </div>
                </div>

                <div class="mb-8">
                    <h3 class="text-base font-semibold uppercase tracking-wider m-0 mb-4"
                        style="color: var(--text-secondary); letter-spacing: 0.5px">
                        تقویم
                    </h3>

                    <div class="setting-item flex flex-col items-start mb-3">
                        <div class="flex items-start gap-4 flex-1">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" style="color: var(--accent-primary)" class="shrink-0">
                                <path d="M8 2V5M16 2V5M3.5 9.09H20.5" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                            </svg>

                            <div>
                                <h4 class="m-0 mb-1 text-base font-semibold"
                                    style="color: var(--text-primary)"
                                >
                                    تقویم پیش‌فرض
                                </h4>

                                <p class="m-0 text-sm"
                                   style="color: var(--text-secondary)"
                                >
                                    تقویم اصلی برای نمایش
                                </p>
                            </div>
                        </div>

                        <select v-model="settings.defaultCalendar"
                                @change="saveSettings"
                                class="settings-select mt-3"
                        >
                            <option value="jalali">
                                شمسی
                            </option>

                            <option value="hijri">
                                قمری
                            </option>

                            <option value="gregorian">
                                میلادی
                            </option>
                        </select>
                    </div>

                    <div class="setting-item items-center">
                        <div class="flex items-center gap-4 flex-1">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" style="color: var(--accent-primary)" class="shrink-0">
                                <path d="M12 2V22M2 12H22" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                            </svg>

                            <div>
                                <h4 class="m-0 mb-1 text-base font-semibold"
                                    style="color: var(--text-primary)"
                                >
                                    نمایش تعطیلات
                                </h4>

                                <p class="m-0 text-sm"
                                   style="color: var(--text-secondary)"
                                >
                                    تعطیلات را نشان بده
                                </p>
                            </div>
                        </div>

                        <label class="toggle-switch">
                            <input type="checkbox"
                                   v-model="settings.showHolidays"
                                   @change="saveSettings"
                            />

                            <span class="toggle-slider"></span>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped>

.close-btn
{
    width: 40px;
    height: 40px;
    border: none;
    border-radius: 10px;
    background: var(--bg-tertiary);
    color: var(--text-primary);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
}

.close-btn:hover
{
    background: var(--accent-primary);
    color: white;
    transform: rotate(90deg);
}

.setting-item
{
    display: flex;
    justify-content: space-between;
    padding: 1.25rem;
    background: var(--bg-tertiary);
    border-radius: 12px;
    border: 1px solid var(--border-color);
    transition: all 0.3s ease;
}

.setting-item:hover
{
    border-color: var(--accent-primary);
}

.theme-btn
{
    display: flex;
    align-items: center;
    gap: 0.5rem;
    padding: 0.5rem 1rem;
    border: 1px solid var(--border-color);
    border-radius: 8px;
    background: var(--bg-secondary);
    color: var(--text-primary);
    font-family: inherit;
    font-size: 0.875rem;
    cursor: pointer;
    transition: all 0.3s ease;
}

.theme-btn:hover
{
    border-color: var(--accent-primary);
    background: var(--bg-tertiary);
}

.theme-btn.active
{
    background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary));
    color: white;
    border-color: transparent;
}

.toggle-switch
{
    position: relative;
    width: 52px;
    height: 28px;
    cursor: pointer;
}

.toggle-switch input
{
    opacity: 0;
    width: 0;
    height: 0;
}

.toggle-slider
{
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: var(--bg-secondary);
    border: 2px solid var(--border-color);
    border-radius: 28px;
    transition: all 0.3s ease;
}

.toggle-slider::before
{
    content: '';
    position: absolute;
    height: 20px;
    width: 20px;
    left: 2px;
    bottom: 2px;
    background: var(--text-muted);
    border-radius: 50%;
    transition: all 0.3s ease;
}

.toggle-switch input:checked + .toggle-slider
{
    background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary));
    border-color: transparent;
}

.toggle-switch input:checked + .toggle-slider::before
{
    transform: translateX(24px);
    background: white;
}

.settings-select
{
    padding: 0.5rem 1rem;
    border: 1px solid var(--border-color);
    border-radius: 8px;
    background: var(--bg-secondary);
    color: var(--text-primary);
    font-family: inherit;
    font-size: 0.875rem;
    cursor: pointer;
    transition: all 0.3s ease;
}

.settings-select:focus
{
    outline: none;
    border-color: var(--accent-primary);
}

@media (max-width: 600px)
{
    .setting-item
    {
        flex-direction: column;
        align-items: flex-start;
        gap: 1rem;
    }

    .theme-btn
    {
        flex: 1;
        justify-content: center;
    }

    .glass
    {
        margin: 0;
        max-height: 100vh;
        border-radius: 0;
    }
}

</style>
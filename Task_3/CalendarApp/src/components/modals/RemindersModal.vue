<script setup>

import { ref, onMounted, computed } from 'vue'
import { useReminderStore } from '../../stores/reminderStore.ts'
import moment from 'moment-jalaali'
import ConfirmModal from './ConfirmModal.vue'

const emit = defineEmits(['close']);

const reminderStore = useReminderStore();

// Use computed instead of ref to ensure reactivity
const reminders = computed(() => reminderStore.reminders.filter(r => !r.notified));

const newReminder = ref({
    title: '',
    description: '',
    date: moment().format('YYYY-MM-DD'),
    time: '12:00'
});

const showDeleteConfirm = ref(false);
const reminderToDelete = ref(null);

onMounted(() =>
{
    // Load reminders when modal opens
    reminderStore.loadReminders();
})

function addReminder()
{
    if (!newReminder.value.title || !newReminder.value.date || !newReminder.value.time)
    {
        // Don't use alert - just return without showing message
        // The browser validation should handle this
        return;
    }

    reminderStore.addReminder(
    {
        title: newReminder.value.title,
        description: newReminder.value.description,
        date: newReminder.value.date,
        time: newReminder.value.time
    });

    // Reset form
    newReminder.value =
    {
        title: '',
        description: '',
        date: moment().format('YYYY-MM-DD'),
        time: '12:00'
    };

    // reminders will update automatically via computed property
}

function deleteReminder(id)
{
    reminderToDelete.value = id;
    showDeleteConfirm.value = true;
}

function confirmDelete()
{
    if (reminderToDelete.value)
    {
        reminderStore.deleteReminder(reminderToDelete.value);
        // reminders will update automatically via computed property
    }
    showDeleteConfirm.value = false;
    reminderToDelete.value = null;
}

function cancelDelete()
{
    showDeleteConfirm.value = false;
    reminderToDelete.value = null;
}

function formatDateTime(date, time)
{
    const dateObj = moment(date);
    return `${dateObj.format('jYYYY/jMM/jDD')} - ${time}`;
}

</script>

<template>

    <div class="fixed inset-0 bg-opacity-75 flex items-center justify-center z-[1000] p-4" style="backdrop-filter: blur(4px)" @click.self="$emit('close')">
        <div class="glass w-full max-w-[600px] max-h-[90vh] overflow-y-auto rounded-[20px] animate-fade-in hide-scrollbar">
            <div class="flex justify-between items-center p-6 border-b" style="border-color: var(--border-color)">
                <h2 class="flex items-center gap-3 m-0 text-2xl font-bold" style="color: var(--text-primary)">
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" style="color: var(--accent-primary)">
                        <path d="M12.02 2.91C8.71 2.91 6.02 5.6 6.02 8.91V11.8C6.02 12.41 5.76 13.34 5.45 13.86L4.3 15.77C3.59 16.95 4.08 18.26 5.38 18.7C9.69 20.14 14.34 20.14 18.65 18.7C19.86 18.3 20.39 16.87 19.73 15.77L18.58 13.86C18.28 13.34 18.02 12.41 18.02 11.8V8.91C18.02 5.61 15.32 2.91 12.02 2.91Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round"/>
                    </svg>

                    یادآوری‌ها
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
                <div class="p-6 rounded-xl mb-8"
                     style="background: var(--bg-tertiary)"
                >
                    <h3 class="m-0 mb-4 text-lg font-semibold"
                        style="color: var(--text-primary)"
                    >
                        افزودن یادآور جدید
                    </h3>

                    <div class="mb-4">
                        <label class="block mb-2 text-sm font-medium"
                               style="color: var(--text-secondary)"
                        >
                            عنوان
                        </label>

                        <input
                            v-model="newReminder.title"
                            type="text"
                            placeholder="عنوان"
                            required
                            @keyup.enter="addReminder"
                            class="w-full px-4 py-3 border rounded-lg font-inherit transition-all duration-300"
                            style="background: var(--bg-secondary); color: var(--text-primary); border-color: var(--border-color)"
                        />
                    </div>

                    <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
                        <div>
                            <label class="block mb-2 text-sm font-medium"
                                   style="color: var(--text-secondary)"
                            >
                                تاریخ
                            </label>

                            <input v-model="newReminder.date"
                                   type="date"
                                   class="w-full px-4 py-3 border rounded-lg font-inherit transition-all duration-300"
                                   style="background: var(--bg-secondary); color: var(--text-primary); border-color: var(--border-color)"
                            />
                        </div>

                        <div>
                            <label class="block mb-2 text-sm font-medium"
                                   style="color: var(--text-secondary)"
                            >
                                ساعت
                            </label>

                            <input v-model="newReminder.time"
                                   type="time"
                                   class="w-full px-4 py-3 border rounded-lg font-inherit transition-all duration-300"
                                   style="background: var(--bg-secondary); color: var(--text-primary); border-color: var(--border-color)"
                            />
                        </div>
                    </div>

                    <div class="mb-4">
                        <label class="block mb-2 text-sm font-medium"
                               style="color: var(--text-secondary)"
                        >
                            توضیحات (اختیاری)
                        </label>

                        <textarea
                            v-model="newReminder.description"
                            rows="3"
                            placeholder="توضیحات"
                            class="w-full px-4 py-3 border rounded-lg font-inherit transition-all duration-300"
                            style="background: var(--bg-secondary); color: var(--text-primary); border-color: var(--border-color)"
                        ></textarea>
                    </div>

                    <button @click="addReminder"
                            class="btn btn-primary w-full mt-4 flex items-center justify-center gap-2"
                    >
                        <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                            <path d="M12 5V19M5 12H19" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>

                        افزودن
                    </button>
                </div>

                <div>
                    <h3 class="m-0 mb-4 text-lg font-semibold"
                        style="color: var(--text-primary)"
                    >
                        یادآورهای فعال
                    </h3>
                    <div v-if="reminders.length === 0"
                         class="text-center py-12 px-4"
                         style="color: var(--text-muted)"
                    >
                        <svg width="64" height="64" viewBox="0 0 24 24" fill="none" class="mx-auto mb-4 opacity-50">
                            <path d="M8 2V5M16 2V5M3.5 9.09H20.5M21 8.5V17C21 20 19.5 22 16 22H8C4.5 22 3 20 3 17V8.5C3 5.5 4.5 3.5 8 3.5H16C19.5 3.5 21 5.5 21 8.5Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>

                        <p class="m-0">
                            یادآوری یافت نشد!
                        </p>
                    </div>

                    <div v-else
                         class="flex flex-col gap-3"
                    >
                        <div
                            v-for="reminder in reminders"
                            :key="reminder.id"
                            class="reminder-item"
                        >
                            <div class="w-10 h-10 flex items-center justify-center rounded-[10px] shrink-0" style="background: var(--bg-secondary); color: var(--accent-primary)">
                                <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                                    <path d="M12 8V12L14.5 14.5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                                    <path d="M12 22C17.5228 22 22 17.5228 22 12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12C2 17.5228 6.47715 22 12 22Z" stroke="currentColor" stroke-width="2"/>
                                </svg>
                            </div>

                            <div class="flex-1">
                                <h4 class="m-0 mb-1 text-base font-semibold"
                                    style="color: var(--text-primary)"
                                >
                                    {{ reminder.title }}
                                </h4>

                                <p v-if="reminder.description"
                                   class="my-1 text-sm"
                                   style="color: var(--text-secondary)"
                                >
                                    {{ reminder.description }}
                                </p>

                                <div class="flex items-center gap-2 text-sm mt-2"
                                     style="color: var(--text-muted)"
                                >
                                    <svg width="16"
                                         height="16"
                                         viewBox="0 0 24 24"
                                         fill="none"
                                         style="color: var(--accent-primary)"
                                    >
                                        <path d="M8 2V5M16 2V5M3.5 9.09H20.5" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
                                    </svg>

                                    {{ formatDateTime(reminder.date, reminder.time) }}
                                </div>
                            </div>

                            <button @click="deleteReminder(reminder.id)"
                                    class="delete-btn"
                            >
                                <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                                    <path d="M21 5.98C17.67 5.65 14.32 5.48 10.98 5.48C9 5.48 7.02 5.58 5.04 5.78L3 5.98M8.5 4.97L8.72 3.66C8.88 2.71 9 2 10.69 2H13.31C15 2 15.13 2.75 15.28 3.67L15.5 4.97M18.85 9.14L18.2 19.21C18.09 20.78 18 22 15.21 22H8.79C6 22 5.91 20.78 5.8 19.21L5.15 9.14M10.33 16.5H13.66M9.5 12.5H14.5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Delete Confirmation Modal -->
        <ConfirmModal
            v-if="showDeleteConfirm"
            title="تایید حذف"
            message="آیا مطمئن هستید که می‌خواهید این یادآوری را حذف کنید؟"
            confirm-text="حذف"
            cancel-text="انصراف"
            confirm-type="danger"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
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

input:focus,
textarea:focus
{
    outline: none;
    border-color: var(--accent-primary);
    box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.reminder-item
{
    display: flex;
    gap: 1rem;
    padding: 1rem;
    background: var(--bg-tertiary);
    border-radius: 12px;
    border: 1px solid var(--border-color);
    transition: all 0.3s ease;
    animation: slideIn 0.3s ease-out;
}

.reminder-item:hover
{
    border-color: var(--accent-primary);
}

.delete-btn
{
    width: 40px;
    height: 40px;
    border: none;
    border-radius: 10px;
    background: var(--bg-secondary);
    color: #f56565;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
    flex-shrink: 0;
}

.delete-btn:hover
{
    background: #f56565;
    color: white;
    transform: scale(1.1);
}

@media (max-width: 600px)
{
    .glass
    {
        margin: 0;
        max-height: 100vh;
        border-radius: 0;
    }
}

</style>
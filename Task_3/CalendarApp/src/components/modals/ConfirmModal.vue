<script setup>

import { defineProps, defineEmits } from 'vue';

const props = defineProps(
{
    title:
    {
        type: String,
        default: 'تایید'
    },

    message:
    {
        type: String,
        required: true
    },

    confirmText:
    {
        type: String,
        default: 'تایید'
    },

    cancelText:
    {
        type: String,
        default: 'انصراف'
    },

    confirmType:
    {
        type: String,
        default: 'danger', // 'danger', 'primary', 'success'
        validator: (value) => ['danger', 'primary', 'success'].includes(value)
    }
});

const emit = defineEmits(['confirm', 'cancel']);

function handleConfirm()
{
    emit('confirm');
}

function handleCancel()
{
    emit('cancel');
}

function getConfirmButtonStyle()
{
    const styles =
    {
        danger: 'background: linear-gradient(135deg, #f56565, #e53e3e); color: white',
        primary: 'background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary)); color: white',
        success: 'background: linear-gradient(135deg, #48bb78, #38a169); color: white'
    }

    return styles[props.confirmType];
}

</script>

<template>

    <div class="modal-backdrop fixed inset-0 flex items-center justify-center z-[1001]"
         @click.self="handleCancel"
    >
        <div class="glass p-6 rounded-xl max-w-md w-full mx-4 animate-fade-in">
            <h3 class="text-xl font-bold mb-4" style="color: var(--text-primary)">
                {{ title }}
            </h3>

            <p class="mb-6" style="color: var(--text-secondary)">
                {{ message }}
            </p>

            <div class="flex gap-3 justify-end">
                <button @click="handleCancel"
                        class="btn-secondary px-6 py-2 rounded-lg font-medium transition-all duration-300"
                        style="background: var(--bg-tertiary); color: var(--text-primary)"
                >
                    {{ cancelText }}
                </button>

                <button @click="handleConfirm"
                        class="btn-confirm px-6 py-2 rounded-lg font-medium transition-all duration-300"
                        :style="getConfirmButtonStyle()"
                >
                    {{ confirmText }}
                </button>
            </div>
        </div>
    </div>

</template>

<style scoped>

.modal-backdrop
{
    background-color: rgba(0, 0, 0, 0.5);
    backdrop-filter: blur(4px);
}

[data-theme="light"] .modal-backdrop
{
    background-color: rgba(0, 0, 0, 0.4);
}

[data-theme="dark"] .modal-backdrop
{
    background-color: rgba(0, 0, 0, 0.6);
}

.btn-secondary:hover
{
    background: var(--bg-secondary) !important;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.btn-confirm:hover
{
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
}

@keyframes fade-in
{
    from
    {
        opacity: 0;
        transform: scale(0.95);
    }

    to
    {
        opacity: 1;
        transform: scale(1);
    }
}

.animate-fade-in
{
    animation: fade-in 0.2s ease-out;
}

</style>

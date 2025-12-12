<script setup>

defineProps({
    title: String,
    message: String
});

defineEmits(['close']);

</script>

<template>

    <div class="notification-toast glass fixed top-5 right-5 flex items-center gap-4 px-6 py-4 min-w-[300px] max-w-[400px] rounded-xl z-[10000]" style="box-shadow: var(--shadow-lg)">
        <div class="w-10 h-10 flex items-center justify-center rounded-[10px] text-white shrink-0" style="background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary))">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                <path d="M12.02 2.91C8.71 2.91 6.02 5.6 6.02 8.91V11.8C6.02 12.41 5.76 13.34 5.45 13.86L4.3 15.77C3.59 16.95 4.08 18.26 5.38 18.7C9.69 20.14 14.34 20.14 18.65 18.7C19.86 18.3 20.39 16.87 19.73 15.77L18.58 13.86C18.28 13.34 18.02 12.41 18.02 11.8V8.91C18.02 5.61 15.32 2.91 12.02 2.91Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round"/>
                <path d="M13.87 3.2C13.56 3.11 13.24 3.04 12.91 3C11.95 2.88 11.03 2.95 10.17 3.2C10.46 2.46 11.18 1.94 12.02 1.94C12.86 1.94 13.58 2.46 13.87 3.2Z" stroke="currentColor" stroke-width="2" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        </div>

        <div class="flex-1">
            <h4 class="m-0 mb-1 text-base font-semibold"
                style="color: var(--text-primary)"
            >
                {{ title }}
            </h4>

            <p class="m-0 text-sm"
               style="color: var(--text-secondary)"
            >
                {{ message }}
            </p>
        </div>

        <button @click="$emit('close')"
                class="toast-close"
        >
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                <path d="M18 6L6 18M6 6L18 18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        </button>
    </div>

</template>

<style scoped>

.notification-toast
{
    animation: slideInRight 0.3s ease-out;
}

@keyframes slideInRight
{
    from
    {
        opacity: 0;
        transform: translateX(100px);
    }

    to
    {
        opacity: 1;
        transform: translateX(0);
    }
}

.toast-close
{
    width: 32px;
    height: 32px;
    border: none;
    border-radius: 8px;
    background: var(--bg-tertiary);
    color: var(--text-primary);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
    flex-shrink: 0;
}

.toast-close:hover
{
    background: var(--accent-primary);
    color: white;
    transform: scale(1.1);
}

@media (max-width: 600px)
{
    .notification-toast
    {
        top: 10px;
        right: 10px;
        left: 10px;
        min-width: auto;
        max-width: none;
    }
}

</style>
<script setup>

import { ref, computed, onMounted, watch } from 'vue'
import moment from 'moment-jalaali'
import momentHijri from 'moment-hijri'

const props = defineProps({
    selectedDate: Object
});

const emit = defineEmits(['date-click']);

const jalaliMonths =
[
    'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
    'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'
];

const hijriMonths =
[
    'محرم', 'صفر', 'ربیع‌الاول', 'ربیع‌الثانی', 'جمادی‌الاول', 'جمادی‌الثانی',
    'رجب', 'شعبان', 'رمضان', 'شوال', 'ذی‌القعده', 'ذی‌الحجه'
];

const gregorianMonths =
[
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
];

const persianWeekDays = ['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج'];
const englishWeekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

// Persian holidays (sample)
const persianHolidays = {
    '1/1':  'عید نوروز',
    '1/2':  'عید نوروز',
    '1/3':  'عید نوروز',
    '1/4':  'عید نوروز',
    '1/12': 'روز جمهوری اسلامی',
    '1/13': 'روز طبیعت',
    '3/14': 'رحلت امام خمینی',
    '3/15': 'قیام خونین ۱۵ خرداد',
    '11/22':'پیروزی انقلاب اسلامی',
    '12/29': 'روز ملی شدن صنعت نفت'
};

const currentJalaliMonth = ref(moment().jMonth());
const currentJalaliYear = ref(moment().jYear());
const currentHijriMonth = ref(momentHijri().iMonth());
const currentHijriYear = ref(momentHijri().iYear());
const currentGregorianMonth = ref(moment().month());
const currentGregorianYear = ref(moment().year());

const calendars = computed(() =>
[
    {
        type: 'jalali',
        name: 'تقویم شمسی',
        icon: '',
        currentMonth: `${jalaliMonths[currentJalaliMonth.value]} ${currentJalaliYear.value}`,
        weekDays: persianWeekDays,
        days: getJalaliDays()
    },
    {
        type: 'hijri',
        name: 'تقویم قمری',
        icon: '',
        currentMonth: `${hijriMonths[currentHijriMonth.value]} ${currentHijriYear.value}`,
        weekDays: persianWeekDays,
        days: getHijriDays()
    },
    {
        type: 'gregorian',
        name: 'تقویم میلادی',
        icon: '',
        currentMonth: `${gregorianMonths[currentGregorianMonth.value]} ${currentGregorianYear.value}`,
        weekDays: englishWeekDays,
        days: getGregorianDays()
    }
]);

function getJalaliDays()
{
    const days = [];
    const firstDay = moment().jYear(currentJalaliYear.value).jMonth(currentJalaliMonth.value).jDate(1);
    const daysInMonth = moment.jDaysInMonth(currentJalaliYear.value, currentJalaliMonth.value);

    // Add empty cells for days before month start
    let startDay = firstDay.day();

    if (startDay === 6)
    {
        startDay = -1;
    }

    for (let i = 0; i < (startDay + 1); i++)
    {
        days.push({ number: null });
    }

    // Add month days
    const today = moment();

    for (let i = 1; i <= daysInMonth; i++)
    {
        const date = moment().jYear(currentJalaliYear.value).jMonth(currentJalaliMonth.value).jDate(i);
        const isToday = date.jYear() === today.jYear() && date.jMonth() === today.jMonth() && date.jDate() === today.jDate();
        const monthDay = `${currentJalaliMonth.value + 1}/${i}`;
        const isHoliday = !!persianHolidays[monthDay];
        const isFriday = (date.day() === 5);

        days.push({
            number: i,
            isToday,
            isSelected: false,
            isHoliday: isHoliday || isFriday,
            hasEvent: isHoliday,
            event: persianHolidays[monthDay],
            date: { day: i, month: currentJalaliMonth.value + 1, year: currentJalaliYear.value }
        });
    }

    return days;
}

function getHijriDays()
{
    const days = [];
    const firstDay = momentHijri().iYear(currentHijriYear.value).iMonth(currentHijriMonth.value).iDate(1);
    const daysInMonth = firstDay.iDaysInMonth();

    // Add empty cells
    let startDay = firstDay.day();
    if (startDay === 6)
    {
        startDay = -1;
    }

    for (let i = 0; i < (startDay + 1); i++)
    {
        days.push({ number: null });
    }

    // Add month days
    const today = momentHijri();

    for (let i = 1; i <= daysInMonth; i++)
    {
        const date = momentHijri().iYear(currentHijriYear.value).iMonth(currentHijriMonth.value).iDate(i);
        const isToday = (date.iYear() === today.iYear() && date.iMonth() === today.iMonth() && date.iDate() === today.iDate());
        const isFriday = (date.day() === 5);

        days.push({
            number: i,
            isToday,
            isSelected: false,
            isHoliday: isFriday,
            hasEvent: false,
            event: null,
            date: { day: i, month: currentHijriMonth.value + 1, year: currentHijriYear.value }
        })
    }

    return days;
}

function getGregorianDays()
{
    const days = [];
    const firstDay = moment().year(currentGregorianYear.value).month(currentGregorianMonth.value).date(1);
    const daysInMonth = firstDay.daysInMonth();

    // Add empty cells
    const startDay = firstDay.day();
    for (let i = 0; i < startDay; i++)
    {
        days.push({ number: null });
    }

    // Add month days
    const today = moment();

    for (let i = 1; i <= daysInMonth; i++)
    {
        const date = moment().year(currentGregorianYear.value).month(currentGregorianMonth.value).date(i);
        const isToday = (date.year() === today.year() && date.month() === today.month() && date.date() === today.date());
        const isSunday = (date.day() === 0);

        days.push({
            number: i,
            isToday,
            isSelected: false,
            isHoliday: isSunday,
            hasEvent: false,
            event: null,
            date: { day: i, month: currentGregorianMonth.value + 1, year: currentGregorianYear.value }
        });
    }

    return days;
}

function previousMonth(type)
{
    if (type === 'jalali')
    {
        currentJalaliMonth.value--;

        if (currentJalaliMonth.value < 0)
        {
            currentJalaliMonth.value = 11;
            currentJalaliYear.value--;
        }
    }
    else if (type === 'hijri')
    {
        currentHijriMonth.value--;

        if (currentHijriMonth.value < 0)
        {
            currentHijriMonth.value = 11;
            currentHijriYear.value--;
        }
    }
    else
    {
        currentGregorianMonth.value--;

        if (currentGregorianMonth.value < 0)
        {
            currentGregorianMonth.value = 11;
            currentGregorianYear.value--;
        }
    }
}

function nextMonth(type)
{
    if (type === 'jalali')
    {
        currentJalaliMonth.value++;

        if (currentJalaliMonth.value > 11)
        {
            currentJalaliMonth.value = 0;
            currentJalaliYear.value++;
        }
    }
    else if (type === 'hijri')
    {
        currentHijriMonth.value++;

        if (currentHijriMonth.value > 11)
        {
            currentHijriMonth.value = 0;
            currentHijriYear.value++;
        }
    }
    else
    {
        currentGregorianMonth.value++;

        if (currentGregorianMonth.value > 11)
        {
            currentGregorianMonth.value = 0;
            currentGregorianYear.value++;
        }
    }
}

function selectDate(type, day)
{
    if (!day.number)
    {
        return;
    }

    emit('date-click', { type, ...day.date });
}

</script>

<template>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div v-for="calendar in calendars"
             :key="calendar.type"
             class="card animate-fade-in"
             :style="`animation-delay: ${calendar.type === 'jalali' ? '0s' : calendar.type === 'hijri' ? '0.1s' : '0.2s'}`"
        >
            <div class="mb-6 pb-4 border-b" style="border-color: var(--border-color)">
                <div class="flex items-center gap-3 mb-4">
                    <span class="text-2xl">
                        {{ calendar.icon }}
                    </span>

                    <h3 class="text-lg font-semibold m-0" style="color: var(--text-primary)">
                        {{ calendar.name }}
                    </h3>
                </div>

                <div class="flex items-center justify-between gap-4">
                    <button @click="previousMonth(calendar.type)"
                            class="nav-btn"
                    >
                        <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                            <path d="M9 5L16 12L9 19" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>
                    </button>

                    <span class="flex-1 text-center font-semibold text-base" style="color: var(--text-primary)">
                        {{ calendar.currentMonth }}
                    </span>

                    <button @click="nextMonth(calendar.type)"
                            class="nav-btn"
                    >
                        <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                            <path d="M15 19L8 12L15 5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        </svg>
                    </button>
                </div>
            </div>

            <div class="w-full">
                <div class="grid grid-cols-7 gap-1 mb-2">
                    <div v-for="day in calendar.weekDays"
                         :key="day"
                         class="text-center text-sm font-semibold py-2"
                         style="color: var(--text-secondary)"
                    >
                        {{ day }}
                    </div>
                </div>

                <div class="grid grid-cols-7 gap-1">
                    <div
                        v-for="(day, index) in calendar.days"
                        :key="index"
                        :class="[
                                  'day-cell aspect-square flex flex-col items-center justify-center rounded-lg cursor-pointer transition-all duration-300 relative border border-transparent',
                                  {
                                    'empty': !day.number,
                                    'today': day.isToday,
                                    'selected': day.isSelected,
                                    'holiday': day.isHoliday,
                                    'has-event': day.hasEvent
                                  }
                                ]"
                        @click="selectDate(calendar.type, day)"
                    >
                        <span class="day-number text-sm font-medium"
                              style="color: var(--text-primary)"
                        >
                            {{ day.number }}
                        </span>

                        <span v-if="day.event"
                              class="event-indicator absolute bottom-1 text-2xl leading-none"
                              :title="day.event"
                              style="color: var(--cal-event)"
                        >
                            •
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped>

.nav-btn
{
    width: 36px;
    height: 36px;
    border: none;
    border-radius: 8px;
    background: var(--bg-tertiary);
    color: var(--text-primary);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
}

.nav-btn:hover
{
    background: var(--accent-primary);
    color: white;
}

.day-cell
{
    background: var(--bg-tertiary);
}

.day-cell:not(.empty):hover
{
    background: var(--cal-hover);
    border-color: var(--accent-primary);
}

.day-cell.empty
{
    background: transparent;
    cursor: default;
}

.day-cell.today
{
    background: var(--cal-today);
    color: white;
    font-weight: 700;
    animation: glow 2s infinite;
}

.day-cell.today .day-number,
.day-cell.today .event-indicator
{
    color: white;
}

.day-cell.selected
{
    background: var(--cal-selected);
    color: white;
    font-weight: 700;
    border-color: var(--accent-secondary);
}

.day-cell.selected .day-number,
.day-cell.selected .event-indicator
{
    color: white;
}

.day-cell.holiday .day-number
{
    color: var(--cal-holiday);
}

.day-cell.today.holiday .day-number,
.day-cell.selected.holiday .day-number
{
    color: white;
}

@media (max-width: 480px)
{
    .day-number
    {
        font-size: 0.75rem;
    }

    .day-cell
    {
        border-radius: 6px;
    }

    h3
    {
        font-size: 1rem;
    }
}

</style>
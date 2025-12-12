<script setup>

import { ref, computed, onMounted } from 'vue'
import moment from 'moment-jalaali'
import momentHijri from 'moment-hijri'

const emit = defineEmits(['date-selected'])

const calendarTypes =
[
    { type: 'jalali', name: 'شمسی' },
    { type: 'hijri', name: 'قمری' },
    { type: 'gregorian', name: 'میلادی' }
];

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

const persianDayNames = ['یکشنبه', 'دوشنبه', 'سه‌شنبه', 'چهارشنبه', 'پنج‌شنبه', 'جمعه', 'شنبه'];

const selectedType = ref('jalali');
const convertedDates = ref(null);

const day = ref(1);
const month = ref(1);
const year = ref(1403);

const selectedTypeLabel = computed(() =>
{
    const labels =
    {
        jalali: { day: 'روز', month: 'ماه', year: 'سال' },
        hijri: { day: 'روز', month: 'ماه', year: 'سال' },
        gregorian: { day: 'روز', month: 'ماه', year: 'سال' }
    };

    return labels[selectedType.value];
})

onMounted(() =>
{
    // Set today's date
    const now = moment();

    if (selectedType.value === 'jalali')
    {
        day.value = now.jDate();
        month.value = now.jMonth() + 1;
        year.value = now.jYear();
    }

    convertDate();
});

function selectType(type)
{
    selectedType.value = type;
    // Reset to current date in selected calendar
    const now = moment();

    if (type === 'jalali')
    {
        day.value = now.jDate();
        month.value = now.jMonth() + 1;
        year.value = now.jYear();
    }
    else if (type === 'hijri')
    {
        const hijriDate = momentHijri();

        day.value = hijriDate.iDate();
        month.value = hijriDate.iMonth() + 1;
        year.value = hijriDate.iYear();
    }
    else
    {
        day.value = now.date();
        month.value = now.month() + 1;
        year.value = now.year();
    }

    convertDate();
}

function getMonthsForType()
{
    if (selectedType.value === 'jalali')
    {
        return jalaliMonths;
    }

    if (selectedType.value === 'hijri')
    {
        return hijriMonths;
    }

    return gregorianMonths;
}

function convertDate()
{
    try
    {
        let baseDate;

        // Create base date from selected calendar
        if (selectedType.value === 'jalali')
        {
            baseDate = moment(`${year.value}/${month.value}/${day.value}`, 'jYYYY/jM/jD');
        }
        else if (selectedType.value === 'hijri')
        {
            baseDate = momentHijri(`${year.value}/${month.value}/${day.value}`, 'iYYYY/iM/iD');
        }
        else
        {
            baseDate = moment(`${year.value}/${month.value}/${day.value}`, 'YYYY/M/D');
        }

        if (!baseDate.isValid())
        {
            return;
        }

        // Convert to all three calendars
        const jalaliDate = moment(baseDate);
        const hijriDate = momentHijri(baseDate);
        const gregorianDate = moment(baseDate);

        const dayOfWeek = persianDayNames[baseDate.day()];

        convertedDates.value =
        [
            {
                type: 'jalali',
                name: 'تقویم شمسی',
                icon: '',
                date: `${jalaliDate.jDate()} ${jalaliMonths[jalaliDate.jMonth()]} ${jalaliDate.jYear()}`,
                dayName: dayOfWeek
            },
            {
                type: 'hijri',
                name: 'تقویم قمری',
                icon: '',
                date: `${hijriDate.iDate()} ${hijriMonths[hijriDate.iMonth()]} ${hijriDate.iYear()}`,
                dayName: dayOfWeek
            },
            {
                type: 'gregorian',
                name: 'تقویم میلادی',
                icon: '',
                date: `${gregorianDate.date()} ${gregorianMonths[gregorianDate.month()]} ${gregorianDate.year()}`,
                dayName: dayOfWeek
            }
        ];

        // Emit selected date
        emit('date-selected',
        {
            jalali:
            {
                day: jalaliDate.jDate(),
                month: jalaliDate.jMonth() + 1,
                year: jalaliDate.jYear()
            },
            hijri:
            {
                day: hijriDate.iDate(),
                month: hijriDate.iMonth() + 1,
                year: hijriDate.iYear()
            },
            gregorian:
            {
                day: gregorianDate.date(),
                month: gregorianDate.month() + 1,
                year: gregorianDate.year()
            }
        });
    }
    catch (error)
    {
        console.error('Date conversion error:', error);
    }
}
</script>

<template>

    <div class="w-full">
        <div class="mb-8">
            <div class="flex gap-2 mb-6 p-2 rounded-xl"
                 style="background: var(--bg-tertiary)"
            >
                <button
                    v-for="cal in calendarTypes"
                    :key="cal.type"
                    :class="[
                        'flex-1 px-6 py-3 rounded-lg border-none font-medium font-inherit cursor-pointer transition-all duration-300',
                         selectedType === cal.type ? 'type-btn-active' : 'type-btn'
                    ]"
                    @click="selectType(cal.type)"
                >
                    {{ cal.name }}
                </button>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div class="flex flex-col gap-2">
                    <label class="text-sm font-medium"
                           style="color: var(--text-secondary)"
                    >
                        {{ selectedTypeLabel.year }}
                    </label>

                    <input
                        type="number"
                        v-model.number="year"
                        :min="1300"
                        :max="1500"
                        dir="ltr"
                        @input="convertDate"
                        class="px-4 py-3 border rounded-lg font-inherit text-base transition-all duration-300"
                        style="background: var(--bg-tertiary); color: var(--text-primary); border-color: var(--border-color)"
                    />
                </div>

                <div class="flex flex-col gap-2">
                    <label class="text-sm font-medium"
                           style="color: var(--text-secondary)"
                    >
                        {{ selectedTypeLabel.month }}
                    </label>

                    <select v-model.number="month"
                            @change="convertDate"
                            class="px-4 py-3 border rounded-lg font-inherit text-base transition-all duration-300 cursor-pointer"
                            style="background: var(--bg-tertiary); color: var(--text-primary); border-color: var(--border-color)"
                    >
                        <option v-for="(m, index) in getMonthsForType()"
                                :key="index"
                                :value="index + 1"
                        >
                            {{ m }}
                        </option>
                    </select>
                </div>

                <div class="flex flex-col gap-2">
                    <label class="text-sm font-medium"
                           style="color: var(--text-secondary)"
                    >
                        {{ selectedTypeLabel.day }}
                    </label>

                    <input
                        type="number"
                        v-model.number="day"
                        :min="1"
                        :max="31"
                        dir="ltr"
                        @input="convertDate"
                        class="px-4 py-3 border rounded-lg font-inherit text-base transition-all duration-300"
                        style="background: var(--bg-tertiary); color: var(--text-primary); border-color: var(--border-color)"
                    />
                </div>
            </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-4"
             v-if="convertedDates"
        >
            <div class="result-card"
                 v-for="result in convertedDates"
                 :key="result.type"
            >
                <div class="w-[60px] h-[60px] flex items-center justify-center rounded-xl text-3xl"
                     style="background: var(--bg-secondary)"
                >
                    {{ result.icon }}
                </div>

                <div class="flex-1">
                    <h3 class="text-sm font-semibold mb-2 m-0"
                        style="color: var(--text-secondary)"
                    >
                        {{ result.name }}
                    </h3>

                    <p class="text-lg font-semibold mb-1 m-0"
                       style="color: var(--text-primary)"
                    >
                        {{ result.date }}
                    </p>

                    <p class="text-sm m-0"
                       style="color: var(--text-muted)"
                    >
                        {{ result.dayName }}
                    </p>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped>

.type-btn
{
    background: transparent;
    color: var(--text-secondary);
}

.type-btn:hover
{
    background: var(--bg-secondary);
    color: var(--text-primary);
}

.type-btn-active
{
    background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary));
    color: white;
    box-shadow: var(--shadow-md);
}

input:focus,
select:focus
{
    outline: none;
    border-color: var(--accent-primary);
    box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.result-card
{
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 1.5rem;
    background: var(--bg-tertiary);
    border-radius: 12px;
    border: 1px solid var(--border-color);
    transition: all 0.3s ease;
    animation: fadeIn 0.5s ease-out;
}

.result-card:hover
{
    box-shadow: var(--shadow-lg);
    border-color: var(--accent-primary);
}

</style>

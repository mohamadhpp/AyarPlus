(function ()
{
    const translateMap =
    {
        'Select a definition': 'انتخاب نسخه API',
        'Authorize': 'احراز هویت',
        'Available authorizations': 'مجوزهای در دسترس',
        'Try it out': 'امتحان کن',
        'Execute': 'اجرا',
        'Clear': 'پاک کردن',
        'Responses': 'پاسخ‌ها',
        'Response Body': 'بدنه پاسخ',
        'Response Headers': 'هدرهای پاسخ',
        'Request URL': 'آدرس درخواست',
        'Server responses': 'پاسخ‌های سرور',
        'Model': 'مدل',
        'No parameters': 'بدون پارامتر',
        'Schemas': 'شِماها',
        'Schema': ' شِما ',
        'Operations': 'عملیات‌ها',
        'Controls Accept header': 'کنترل‌ها، هدر را می‌پذیرند',
        'Failed to load API definition': 'داکیومنت API بارگیری نشد',
        'Parameters': 'پارامترها',
        'Request body': 'بدنه درخواست',
        'Description': 'توضیحات',
        'Summary': 'خلاصه',
        'Servers': 'سرورها',
        'Default': 'پیش‌فرض',
        'Cancel': 'لغو',
        'Close': 'بستن',
        'Download': 'دانلود',
        'Show/Hide': 'نمایش/مخفی کردن',
        'List Operations': 'لیست عملیات‌ها',
        'Expand Operations': 'گسترش عملیات‌ها',
        'Collapse Operations': 'جمع کردن عملیات‌ها',
        'Example Value': 'مقدار نمونه',
        'Media type': 'نوع درخواست (Media type)',
        'Value': 'مقدار',
        'Click to set as parameter value': 'کلیک کنید تا به عنوان مقدار پارامتر تنظیم شود',
        'No response received': 'هیچ پاسخی دریافت نشد',
        'Curl': 'Curl',
        'Request Samples': 'نمونه‌های درخواست',
        'Response Samples': 'نمونه‌های پاسخ',
        'Security': 'امنیت',
        'Tags': 'برچسب‌ها',
        'API Documentation': 'مستندات API',
        'Terms of Service': 'شرایط خدمات',
        'License': 'مجوز',
        'External Docs': 'مستندات خارجی',
        'Name': 'نام',
        'Links': 'لینک‌ها',
        'No links': 'بدون لینک',
    };

    function translateElementText(el)
    {
        if (el.childNodes.length === 1 && el.childNodes[0].nodeType === 3)
        {
            const originalText = el.textContent.trim();

            if (translateMap[originalText])
            {
                el.textContent = translateMap[originalText];
            }
        }
    }

    function translateAllText()
    {
        document.querySelectorAll('*').forEach(translateElementText);
    }

    const waitForBody = setInterval(() =>
    {
        if (document.body)
        {
            clearInterval(waitForBody);

            // Initial translation
            translateAllText();

            // Start observing for DOM changes
            const observer = new MutationObserver(mutations =>
            {
                mutations.forEach(mutation =>
                {
                    mutation.addedNodes.forEach(node =>
                    {
                        if (node.nodeType === 1)
                        {
                            node.querySelectorAll('*').forEach(translateElementText);
                            translateElementText(node);
                        }
                    });
                });
            });

            observer.observe(document.body,
            {
                childList: true,
                subtree: true
            });
        }
    }, 100);

    
    // Init input
    const initInputs = () =>
    {
        $("#select").select2();
    }

    setTimeout(() =>
    {
        // Init
        initInputs();
    }, 1);
})();
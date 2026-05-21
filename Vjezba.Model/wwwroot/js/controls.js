document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.ac-wrapper').forEach(function (wrapper) {
        const endpoint = wrapper.dataset.endpoint;
        const input = wrapper.querySelector('.ac-input');
        const hidden = wrapper.querySelector('.ac-value');
        const results = wrapper.querySelector('.ac-results');

        if (!endpoint || !input || !results) {
            return;
        }

        const isMulti = wrapper.dataset.multi === 'true';
        const chipsContainer = isMulti
            ? (wrapper.querySelector('.ac-chips') || (function () {
                const c = document.createElement('div');
                c.className = 'ac-chips';
                wrapper.appendChild(c);
                return c;
            })())
            : null;

        let timer = 0;

        input.addEventListener('input', function () {
            const q = input.value.trim();
            clearTimeout(timer);

            timer = setTimeout(function () {
                fetch(endpoint + '?q=' + encodeURIComponent(q))
                    .then(function (r) { return r.json(); })
                    .then(function (items) {
                        results.innerHTML = '';

                        if (!items || items.length === 0) {
                            results.hidden = true;
                            return;
                        }

                        items.forEach(function (it) {
                            const li = document.createElement('li');
                            li.className = 'ac-item';
                            li.tabIndex = 0;
                            li.textContent = it.text;
                            li.dataset.id = it.id;

                            li.addEventListener('click', function () {
                                if (isMulti) {
                                    const name = wrapper.dataset.inputName || 'DogIds';
                                    const newHidden = document.createElement('input');
                                    newHidden.type = 'hidden';
                                    newHidden.name = name;
                                    newHidden.value = it.id;

                                    const chip = document.createElement('span');
                                    chip.className = 'ac-chip';
                                    chip.textContent = it.text;

                                    const remove = document.createElement('button');
                                    remove.type = 'button';
                                    remove.className = 'ac-chip-remove';
                                    remove.textContent = 'x';
                                    remove.addEventListener('click', function () {
                                        chip.remove();
                                        if (newHidden.parentNode) {
                                            newHidden.parentNode.removeChild(newHidden);
                                        }
                                    });

                                    chip.appendChild(remove);
                                    chipsContainer.appendChild(chip);
                                    wrapper.appendChild(newHidden);
                                    input.value = '';
                                } else if (hidden) {
                                    input.value = it.text;
                                    hidden.value = it.id;
                                }

                                results.hidden = true;
                            });

                            li.addEventListener('keydown', function (ev) {
                                if (ev.key === 'Enter') {
                                    li.click();
                                }
                            });

                            results.appendChild(li);
                        });

                        results.hidden = false;
                    })
                    .catch(function () {
                        results.hidden = true;
                    });
            }, 250);
        });

        document.addEventListener('click', function (ev) {
            if (!wrapper.contains(ev.target)) {
                results.hidden = true;
            }
        });

        if (!isMulti && hidden && hidden.value && !input.value.trim()) {
            fetch(endpoint + '?id=' + encodeURIComponent(hidden.value))
                .then(function (r) { return r.json(); })
                .then(function (items) {
                    if (!items || !Array.isArray(items) || items.length === 0) {
                        return;
                    }

                    const selected = items[0];
                    if (selected && selected.text) {
                        input.value = selected.text;
                    }
                })
                .catch(function () {
                    // Keep hidden ID as source of truth if label hydration fails.
                });
        }
    });

    document.querySelectorAll('.ac-chip-remove').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const chip = btn.parentElement;
            if (!chip) {
                return;
            }

            const next = chip.nextElementSibling;
            if (next && next.tagName.toLowerCase() === 'input' && next.type === 'hidden') {
                next.remove();
            }
            chip.remove();
        });
    });

    function getDatePickerOptions() {
        var docLang = (document.documentElement && document.documentElement.lang) || '';
        var browserLang = (navigator.languages && navigator.languages[0]) || navigator.language || '';
        var lang = (docLang || browserLang || '').toLowerCase();
        var isEnglish = lang.indexOf('en') === 0;

        return {
            enableTime: true,
            dateFormat: isEnglish ? 'm/d/Y h:i K' : 'd.m.Y H:i',
            locale: isEnglish ? 'default' : ((window.flatpickr && window.flatpickr.l10ns && window.flatpickr.l10ns.hr) ? 'hr' : 'default'),
            time_24hr: !isEnglish
        };
    }

    if (window.flatpickr) {
        var options = getDatePickerOptions();
        document.querySelectorAll('.datetime-picker').forEach(function (el) {
            flatpickr(el, options);
        });
    } else {
        setTimeout(function () {
            if (window.flatpickr) {
                var delayedOptions = getDatePickerOptions();
                document.querySelectorAll('.datetime-picker').forEach(function (el) {
                    flatpickr(el, delayedOptions);
                });
            }
        }, 300);
    }
});

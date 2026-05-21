document.addEventListener('DOMContentLoaded', function () {
    // Autocomplete: bind to all wrappers
    document.querySelectorAll('.ac-wrapper').forEach(function (wrapper) {
        const endpoint = wrapper.dataset.endpoint;
        const input = wrapper.querySelector('.ac-input');
        const hidden = wrapper.querySelector('.ac-value');
        const results = wrapper.querySelector('.ac-results');
        const isMulti = wrapper.dataset.multi === 'true';
        const chipsContainer = isMulti ? (wrapper.querySelector('.ac-chips') || (function(){ const c=document.createElement('div'); c.className='ac-chips'; wrapper.appendChild(c); return c; })()) : null;

        let timer = 0;

        input.addEventListener('input', function () {
            const q = input.value.trim();
            clearTimeout(timer);
            timer = setTimeout(function () {
                fetch(endpoint + '?q=' + encodeURIComponent(q))
                    .then(r => r.json())
                    .then(items => {
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
                                    // create chip label + hidden input
                                    const chip = document.createElement('span');
                                    chip.className = 'ac-chip';
                                    chip.textContent = it.text;
                                    const remove = document.createElement('button');
                                    remove.type = 'button';
                                    remove.className = 'ac-chip-remove';
                                    remove.textContent = '×';
                                    remove.addEventListener('click', function () { chip.remove(); newHidden.parentNode && newHidden.parentNode.removeChild(newHidden); });
                                    chip.appendChild(remove);
                                    chipsContainer.appendChild(chip);

                                    // append a new hidden input to wrapper with name from data-name or default
                                    const name = wrapper.dataset.inputName || 'DogIds';
                                    const newHidden = document.createElement('input');
                                    newHidden.type = 'hidden';
                                    newHidden.name = name;
                                    newHidden.value = it.id;
                                    wrapper.appendChild(newHidden);
                                    input.value = '';
                                } else {
                                    input.value = it.text;
                                    hidden.value = it.id;
                                }
                                results.hidden = true;
                            });
                            li.addEventListener('keydown', function (ev) {
                                if (ev.key === 'Enter') { li.click(); }
                            });
                            results.appendChild(li);
                        });

                        results.hidden = false;
                    });
            }, 250);
        });

        document.addEventListener('click', function (ev) {
            if (!wrapper.contains(ev.target)) results.hidden = true;
        });
    });

    // wire existing chip remove buttons (prepopulated in server-rendered edit forms)
    document.querySelectorAll('.ac-chip-remove').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const chip = btn.parentElement;
            if (!chip) return;
            // remove following hidden input if present
            const next = chip.nextElementSibling;
            if (next && next.tagName.toLowerCase() === 'input' && next.type === 'hidden') next.remove();
            chip.remove();
        });
    });

    // Initialize flatpickr on any .datetime-picker
    if (window.flatpickr) {
        document.querySelectorAll('.datetime-picker').forEach(function (el) {
            flatpickr(el, { enableTime: true, dateFormat: 'd.m.Y H:i' });
        });
    } else {
        // If flatpickr not yet loaded, wait briefly
        setTimeout(function () {
            if (window.flatpickr) {
                document.querySelectorAll('.datetime-picker').forEach(function (el) {
                    flatpickr(el, { enableTime: true, dateFormat: 'd.m.Y H:i' });
                });
            }
        }, 300);
    }
});

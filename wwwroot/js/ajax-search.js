// Simple reusable AJAX search module with debounce, paging and rendering
(function (global) {
    function debounce(fn, wait) {
        let t = null;
        return function () {
            const args = arguments;
            clearTimeout(t);
            t = setTimeout(function () { fn.apply(null, args); }, wait);
        };
    }

    function renderCards(container, items) {
        container.innerHTML = '';
        if (!items || items.length === 0) {
            container.innerHTML = '<section class="empty-state"><h2>Nema rezultata</h2></section>';
            return;
        }

        const section = document.createElement('section');
        section.className = 'entity-grid';

        items.forEach(function (it) {
            const article = document.createElement('article');
            article.className = 'entity-card';

            const title = document.createElement('div');
            title.className = 'entity-card__title';
            title.innerHTML = `<div><h2>${escapeHtml(it.name)}</h2><p class="entity-card__meta">${escapeHtml(it.owner || '')}</p></div><span class="badge-soft">${escapeHtml(it.age ? it.age.toString() : '')} godina</span>`;

            const tags = document.createElement('div');
            tags.className = 'entity-tags';
            tags.innerHTML = `<span class="entity-tag">${escapeHtml(it.breed || '')}</span>`;

            const actions = document.createElement('div');
            actions.className = 'entity-card__actions';
            actions.innerHTML = `<a class="btn btn-primary" href="/Dogs/Details/${it.id}">Detalji</a> <a class="btn btn-outline" href="/Dogs/Edit/${it.id}">Uredi</a> <a class="btn btn-outline" href="/Dogs/Delete/${it.id}">Obriši</a>`;

            article.appendChild(title);
            article.appendChild(tags);
            article.appendChild(actions);
            section.appendChild(article);
        });

        container.appendChild(section);
    }

    function renderPagination(container, total, page, pageSize, onPage) {
        // remove existing pagination if any
        const existing = container.querySelector('.ajax-search-pagination');
        if (existing) existing.remove();

        const totalPages = Math.ceil((total || 0) / pageSize);
        if (totalPages <= 1) return;

        const nav = document.createElement('nav');
        nav.className = 'ajax-search-pagination';
        nav.setAttribute('role', 'navigation');
        nav.setAttribute('aria-label', 'Straniciranje rezultata pretrage');
        const ul = document.createElement('ul');
        ul.className = 'pagination';

        function pageItem(label, p, disabled, isPageNumber) {
            const li = document.createElement('li');
            li.className = 'page-item' + (disabled ? ' disabled' : '');
            const a = document.createElement('a');
            a.className = 'page-link';
            a.href = '#';
            a.textContent = label;
            a.setAttribute('aria-label', isPageNumber ? ('Stranica ' + label) : label);
            if (isPageNumber && p === page) a.setAttribute('aria-current', 'page');
            a.addEventListener('click', function (ev) { ev.preventDefault(); if (!disabled) onPage(p); });
            a.addEventListener('keydown', function (ev) { if (!disabled && (ev.key === 'Enter' || ev.key === ' ')) { ev.preventDefault(); onPage(p); } });
            li.appendChild(a);
            return li;
        }

        ul.appendChild(pageItem('«', Math.max(1, page - 1), page <= 1));

        // show a sliding window of pages
        const maxWindow = 7;
        let start = Math.max(1, page - Math.floor(maxWindow / 2));
        let end = Math.min(totalPages, start + maxWindow - 1);
        if (end - start + 1 < maxWindow) start = Math.max(1, end - maxWindow + 1);

        for (let p = start; p <= end; p++) {
            const li = pageItem(p.toString(), p, false, true);
            if (p === page) li.classList.add('active');
            ul.appendChild(li);
        }

        ul.appendChild(pageItem('»', Math.min(totalPages, page + 1), page >= totalPages));
        nav.appendChild(ul);
        container.appendChild(nav);
    }

    function escapeHtml(str) {
        if (!str) return '';
        return String(str).replace(/[&<>"]/g, function (s) { return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' })[s]; });
    }

    function attach(options) {
        const input = options.input;
        const endpoint = options.endpoint;
        const container = options.container; // DOM element
        const pageSize = options.pageSize || 12;
        const debounceMs = typeof options.debounceMs === 'number' ? options.debounceMs : 300;
        const showPagination = options.showPagination !== false;

        let currentPage = 1;
        let lastQuery = '';

        const doSearch = function (q) {
            lastQuery = q || '';
                // show loading state with aria-busy
                container.innerHTML = '';
                const loading = document.createElement('div');
                loading.className = 'loading';
                loading.setAttribute('aria-live', 'polite');
                loading.setAttribute('aria-busy', 'true');
                loading.textContent = 'Učitavanje…';
                container.appendChild(loading);

                fetch(endpoint + '?q=' + encodeURIComponent(q || '') + '&page=' + currentPage + '&pageSize=' + pageSize)
                    .then(function (r) {
                        if (!r.ok) throw new Error('Network response was not ok');
                        return r.json();
                    })
                    .then(function (data) {
                        container.innerHTML = '';
                        if (!data || !data.items || data.items.length === 0) {
                            container.innerHTML = '<div class="empty-state" aria-live="polite"><h2>Nema rezultata</h2></div>';
                            return;
                        }
                        renderCards(container, data.items);
                        if (showPagination) renderPagination(container, data.total, currentPage, pageSize, function (p) { currentPage = p; doSearch(lastQuery); });
                    })
                    .catch(function (err) {
                        container.innerHTML = '';
                        const errEl = document.createElement('div');
                        errEl.className = 'empty-state';
                        errEl.setAttribute('role', 'status');
                        errEl.innerHTML = '<h2>Greška pri dohvaćanju</h2><p>Provjerite mrežnu vezu i pokušajte ponovno.</p>';
                        const retry = document.createElement('button');
                        retry.className = 'btn btn-primary';
                        retry.textContent = 'Pokušaj ponovno';
                        retry.addEventListener('click', function () { doSearch(lastQuery); });
                        errEl.appendChild(retry);
                        container.appendChild(errEl);
                        console.error('Ajax search error', err);
                    });
        };

        input.addEventListener('input', debounce(function (ev) {
            currentPage = 1;
            doSearch(ev.target.value.trim());
        }, debounceMs));

        // allow manual trigger (press Enter)
        input.addEventListener('keydown', function (ev) { if (ev.key === 'Enter') { currentPage = 1; doSearch(input.value.trim()); } });

        // initial empty search to load first page if desired
        if (options.init) doSearch('');

        return { search: doSearch };
    }

    global.ajaxSearch = { attach: attach };
})(window);

// Simple reusable AJAX search module with debounce, paging and rendering
(function (global) {
    function prefersReducedMotion() {
        return !!(global.matchMedia && global.matchMedia('(prefers-reduced-motion: reduce)').matches);
    }

    function debounce(fn, wait) {
        let t = null;
        return function () {
            const args = arguments;
            clearTimeout(t);
            t = setTimeout(function () { fn.apply(null, args); }, wait);
        };
    }

    function toEntityName(endpoint) {
        if (!endpoint) return '';
        const parts = endpoint.split('/').filter(Boolean);
        return (parts[parts.length - 1] || '').toLowerCase();
    }

    function toControllerPath(entity) {
        const map = {
            dogs: 'Dogs',
            owners: 'Owners',
            walkers: 'Walkers',
            bookings: 'Bookings',
            payments: 'Payments',
            reviews: 'Reviews'
        };

        return map[entity] || 'Dogs';
    }

    function buildCardView(entity, item) {
        const id = item && item.id ? item.id : '';
        const view = {
            id: id,
            title: '',
            subtitle: '',
            badge: '',
            badgeClass: 'badge-soft',
            tags: [],
            summary: ''
        };

        if (entity === 'dogs') {
            view.title = item.name || '';
            view.subtitle = item.owner || '';
            view.badge = item.age ? item.age + ' godina' : '';
            if (item.breed) view.tags.push(item.breed);
            view.tags.push(item.isVaccinated ? 'Cijepljen' : 'Nije cijepljen');
            return view;
        }

        if (entity === 'owners') {
            view.title = item.name || '';
            const dogNames = Array.isArray(item.dogs) ? item.dogs : null;
            if (dogNames) {
                view.tags.push('Psi: ' + (dogNames.length > 0 ? dogNames.join(', ') : 'Nema pasa unesenih u profil.'));
            }
            if (item.address) view.tags.push('Lokacija: ' + item.address);
            return view;
        }

        if (entity === 'walkers') {
            view.title = item.name || '';
            view.subtitle = item.address || '';
            if (item.rating != null) view.badge = '⭐ ' + Number(item.rating).toFixed(1);
            if (item.hourlyRate != null) view.tags.push('€' + Number(item.hourlyRate).toFixed(2) + ' / sat');
            if (item.bookingCount != null) {
                view.tags.push(item.bookingCount + ' rezervacija');
            }
            return view;
        }

        if (entity === 'bookings') {
            if (item.text && !item.startTime && !item.owner && !item.walker) {
                view.title = item.text;
                return view;
            }

            view.title = 'Rezervacija #' + id;
            const start = item.startTime || '';
            const end = item.endTime || '';
            view.subtitle = start && end ? (start + ' - ' + end) : start;
            view.badge = item.statusLabel || '';

            const statusClassMap = {
                0: 'status-pending',
                1: 'status-confirmed',
                2: 'status-completed',
                3: 'status-cancelled'
            };
            if (item.status != null && statusClassMap[item.status]) {
                view.badgeClass = 'badge-soft ' + statusClassMap[item.status];
            }

            if (item.owner) view.tags.push('Vlasnik: ' + item.owner);
            if (item.walker) view.tags.push('Šetač: ' + item.walker);

            const dogNames = Array.isArray(item.dogs) ? item.dogs : null;
            if (dogNames) {
                view.summary = 'Psi: ' + (dogNames.length > 0 ? dogNames.join(', ') : 'Nema dodanih pasa.');
            }
            return view;
        }

        if (entity === 'payments') {
            view.title = 'Uplata #' + id;
            view.subtitle = item.date || '';
            if (item.amount != null) view.badge = '€' + Number(item.amount).toFixed(2);
            if (item.paymentMethod) view.tags.push(item.paymentMethod);
            view.summary = item.isSuccessful ? 'Uplata je uspješna.' : 'Uplata nije uspješna.';
            if (item.bookingId != null) view.tags.push('Rezervacija #' + item.bookingId);
            return view;
        }

        if (entity === 'reviews') {
            view.title = item.owner || ('Recenzija #' + id);
            view.subtitle = item.date || '';
            if (item.rating != null) view.badge = '⭐ ' + Number(item.rating).toFixed(1);
            if (item.walker) view.tags.push('Šetač: ' + item.walker);
            if (item.comment) view.summary = item.comment;
            return view;
        }

        view.title = item.name || item.text || ('Stavka #' + id);
        return view;
    }

    function renderCards(container, items, options) {
        container.innerHTML = '';
        if (!items || items.length === 0) {
            renderEmptyState(container, {
                title: 'Nema rezultata',
                message: 'Pokušajte s drugim pojmom pretrage.'
            });
            return;
        }

        const entity = options.entity || 'dogs';
        const controllerPath = options.controllerPath || 'Dogs';

        const section = document.createElement('section');
        section.className = 'entity-grid';

        items.forEach(function (it) {
            const card = buildCardView(entity, it);
            const article = document.createElement('article');
            article.className = 'entity-card';

            const title = document.createElement('div');
            title.className = 'entity-card__title';
            title.innerHTML = `<div><h2>${escapeHtml(card.title)}</h2><p class="entity-card__meta">${escapeHtml(card.subtitle)}</p></div><span class="${escapeHtml(card.badgeClass || 'badge-soft')}">${escapeHtml(card.badge)}</span>`;

            const tags = document.createElement('div');
            tags.className = 'entity-tags';
            tags.innerHTML = card.tags.map(function (tag) { return `<span class="entity-tag">${escapeHtml(tag)}</span>`; }).join('');

            const summary = document.createElement('p');
            summary.className = 'entity-card__summary';
            summary.textContent = card.summary;

            const actions = document.createElement('div');
            actions.className = 'entity-card__actions';
            actions.innerHTML = `<a class="btn btn-primary" href="/${controllerPath}/Details/${card.id}">Detalji</a> <a class="btn btn-outline" href="/${controllerPath}/Edit/${card.id}">Uredi</a> <a class="btn btn-outline" href="/${controllerPath}/Delete/${card.id}">Obriši</a>`;

            article.appendChild(title);
            if (card.tags.length > 0) article.appendChild(tags);
            if (card.summary) article.appendChild(summary);
            article.appendChild(actions);
            section.appendChild(article);
        });

        container.appendChild(section);

        if (!prefersReducedMotion()) {
            const cards = section.querySelectorAll('.entity-card');
            cards.forEach(function (card, index) {
                card.classList.add('entity-card--enter');
                card.style.setProperty('--enter-delay', (index * 35) + 'ms');
            });

            requestAnimationFrame(function () {
                cards.forEach(function (card) {
                    card.classList.add('is-visible');
                });
            });
        }
    }

    function renderEmptyState(container, options) {
        const title = (options && options.title) ? options.title : 'Nema rezultata';
        const message = (options && options.message) ? options.message : '';
        const actionNode = options && options.actionNode ? options.actionNode : null;

        const wrapper = document.createElement('section');
        wrapper.className = 'empty-state empty-state--enter';

        const heading = document.createElement('h2');
        heading.textContent = title;
        wrapper.appendChild(heading);

        if (message) {
            const paragraph = document.createElement('p');
            paragraph.textContent = message;
            wrapper.appendChild(paragraph);
        }

        if (actionNode) {
            wrapper.appendChild(actionNode);
        }

        container.innerHTML = '';
        container.appendChild(wrapper);

        if (!prefersReducedMotion()) {
            requestAnimationFrame(function () {
                wrapper.classList.add('is-visible');
            });
        } else {
            wrapper.classList.add('is-visible');
        }
    }

    function ensureStatusNode(container) {
        let status = container.querySelector('.search-status');
        if (status) return status;

        status = document.createElement('p');
        status.className = 'search-status visually-hidden';
        status.setAttribute('role', 'status');
        status.setAttribute('aria-live', 'polite');
        container.appendChild(status);
        return status;
    }

    function setBusy(container, isBusy, text) {
        container.setAttribute('aria-busy', isBusy ? 'true' : 'false');
        const status = ensureStatusNode(container);
        if (typeof text === 'string' && text.length > 0) {
            status.textContent = text;
        }
    }

    function createLoadingIndicator() {
        const loading = document.createElement('div');
        loading.className = 'search-loading-indicator';
        loading.setAttribute('role', 'status');
        loading.setAttribute('aria-live', 'polite');
        loading.innerHTML = '<span class="search-loading-indicator__spinner" aria-hidden="true"></span><span>Učitavanje rezultata…</span>';
        return loading;
    }

    function renderSearchSkeleton(container, count) {
        const limit = Math.max(1, Math.min(6, count || 3));
        const grid = document.createElement('div');
        grid.className = 'search-skeleton-grid';
        grid.setAttribute('aria-hidden', 'true');

        for (let i = 0; i < limit; i++) {
            const card = document.createElement('div');
            card.className = 'search-skeleton-card';
            card.innerHTML = '<div class="search-skeleton-line search-skeleton-line--title"></div><div class="search-skeleton-line search-skeleton-line--meta"></div><div class="search-skeleton-line search-skeleton-line--cta"></div>';
            grid.appendChild(card);
        }

        container.appendChild(grid);
    }

    function renderPagination(container, total, page, pageSize, onPage) {
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
        const container = options.container;
        const entity = options.entity || toEntityName(endpoint);
        const controllerPath = options.controllerPath || toControllerPath(entity);
        const pageSize = options.pageSize || 12;
        const debounceMs = typeof options.debounceMs === 'number' ? options.debounceMs : 300;
        const showPagination = options.showPagination !== false;

        let currentPage = 1;
        const mainListSelector = options.mainListSelector || null;
        const mainListEl = mainListSelector ? document.querySelector(mainListSelector) : null;
        let lastQuery = '';

        const doSearch = function (q) {
            lastQuery = q || '';

            // if query is empty, show the main list (if provided) and clear results
            if (!q || q.trim() === '') {
                if (mainListEl) mainListEl.style.display = '';
                container.innerHTML = '';
                setBusy(container, false, 'Pretraga je spremna.');
                return;
            }

            // hide the static main list when showing search results
            if (mainListEl) mainListEl.style.display = 'none';

            // show loading state with aria-busy
            container.innerHTML = '';
            const loading = createLoadingIndicator();
            container.appendChild(loading);
            renderSearchSkeleton(container, 3);
            setBusy(container, true, 'Učitavanje rezultata pretrage.');

            fetch(endpoint + '?q=' + encodeURIComponent(q || '') + '&page=' + currentPage + '&pageSize=' + pageSize)
                .then(function (r) {
                    if (!r.ok) throw new Error('Network response was not ok');
                    return r.json();
                })
                .then(function (data) {
                    container.innerHTML = '';
                    if (!data || !data.items || data.items.length === 0) {
                        renderEmptyState(container, {
                            title: 'Nema rezultata',
                            message: 'Pokušajte proširiti ili promijeniti upit pretrage.'
                        });
                        setBusy(container, false, 'Nema rezultata za zadani upit.');
                        return;
                    }

                    renderCards(container, data.items, { entity: entity, controllerPath: controllerPath });
                    if (showPagination) renderPagination(container, data.total, currentPage, pageSize, function (p) { currentPage = p; doSearch(lastQuery); });
                    setBusy(container, false, 'Prikazano rezultata: ' + data.items.length + '.');
                })
                .catch(function (err) {
                    container.innerHTML = '';
                    const retry = document.createElement('button');
                    retry.className = 'btn btn-primary';
                    retry.textContent = 'Pokušaj ponovno';
                    retry.addEventListener('click', function () { doSearch(lastQuery); });
                    renderEmptyState(container, {
                        title: 'Greška pri dohvaćanju',
                        message: 'Provjerite mrežnu vezu i pokušajte ponovno.',
                        actionNode: retry
                    });
                    setBusy(container, false, 'Greška pri dohvaćanju rezultata.');
                    console.error('Ajax search error', err);
                });
        };

        input.addEventListener('input', debounce(function (ev) {
            currentPage = 1;
            doSearch(ev.target.value.trim());
        }, debounceMs));

        input.addEventListener('keydown', function (ev) { if (ev.key === 'Enter') { currentPage = 1; doSearch(input.value.trim()); } });

        if (options.init) doSearch('');

        return { search: doSearch };
    }

    global.ajaxSearch = { attach: attach };
})(window);

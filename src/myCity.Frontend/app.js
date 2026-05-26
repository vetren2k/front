const API_BASE_URL = 'http://localhost:5018/api';
let map = null;
let currentMarkers = [];
let allTickets = [];
let currentFilter = 'all';

// State
let token = localStorage.getItem('token');
let isSelectingLocation = false;
let selectedCoords = null;
let currentUserRole = 'GOŚĆ';

// Elements
const els = {
    loginModal: document.getElementById('login-modal'),
    loginForm: document.getElementById('login-form'),
    btnAuthAction: document.getElementById('btn-auth-action'),
    btnCloseLogin: document.getElementById('btn-close-login'),
    roleBadge: document.getElementById('role-badge'),
    
    btnReportNew: document.getElementById('btn-report-new'),
    reportModal: document.getElementById('report-modal'),
    btnCloseReport: document.getElementById('btn-close-report'),
    reportForm: document.getElementById('report-form'),
    mapPrompt: document.getElementById('map-prompt'),
    
    detailsPanel: document.getElementById('details-panel'),
    btnCloseDetails: document.getElementById('btn-close-details'),
    
    filterItems: document.querySelectorAll('.filter-list li'),
    viewToggles: document.querySelectorAll('.toggle-btn'),
    mapView: document.getElementById('map-view'),
    listView: document.getElementById('list-view'),
    listContainer: document.getElementById('list-container'),
    
    // Admin elements
    btnViewAdmin: document.getElementById('btn-view-admin'),
    adminView: document.getElementById('admin-view'),
    createUserModal: document.getElementById('create-user-modal'),
    createUserForm: document.getElementById('create-user-form'),
    adminUsersList: document.getElementById('admin-users-list'),
    
    // Stats
    counts: {
        all: document.getElementById('count-all'),
        pending: document.getElementById('count-pending'),
        progress: document.getElementById('count-progress'),
        resolved: document.getElementById('count-resolved'),
        rejected: document.getElementById('count-rejected'),
    },
    stats: {
        total: document.getElementById('stat-total'),
        pending: document.getElementById('stat-pending'),
        inprogress: document.getElementById('stat-inprogress'),
        resolved: document.getElementById('stat-resolved-val')
    }
};

// Init
function init() {
    initMap();
    bindEvents();
    
    updateAuthState();
    loadTickets(); // Try to load tickets whether logged in or not
}

function updateAuthState() {
    if (token) {
        els.btnAuthAction.innerHTML = '<i class="fa-solid fa-arrow-right-from-bracket"></i> Wyloguj się';
        els.btnAuthAction.classList.replace('btn-primary', 'btn-secondary');
        
        // Very basic JWT parsing to get role
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            const payload = JSON.parse(jsonPayload);
            currentUserRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || payload["role"] || payload["Role"] || localStorage.getItem('role') || "Mieszkaniec";
            els.roleBadge.textContent = currentUserRole.toUpperCase();
        } catch (e) {
            currentUserRole = localStorage.getItem('role') || 'Mieszkaniec';
            els.roleBadge.textContent = currentUserRole.toUpperCase();
        }
    } else {
        currentUserRole = 'GOŚĆ';
        els.btnAuthAction.innerHTML = '<i class="fa-solid fa-right-to-bracket"></i> Zaloguj się';
        els.btnAuthAction.classList.replace('btn-secondary', 'btn-primary');
        els.roleBadge.textContent = 'GOŚĆ';
    }

    if (currentUserRole.toUpperCase() === 'ADMIN') {
        if (els.btnViewAdmin) els.btnViewAdmin.style.display = 'block';
    } else {
        if (els.btnViewAdmin) els.btnViewAdmin.style.display = 'none';
        const activeToggle = document.querySelector('.toggle-btn.active');
        if (activeToggle && activeToggle.dataset.view === 'admin') {
            const mapToggle = document.querySelector('.toggle-btn[data-view="map"]');
            if (mapToggle) mapToggle.click();
        }
    }
}

function initMap() {
    // Initialize map centered on Gdańsk
    map = L.map('map', { zoomControl: false }).setView([54.3520, 18.6466], 13);
    L.control.zoom({ position: 'bottomright' }).addTo(map);
    
    // Add CartoDB Positron tile layer (light and clean)
    L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', {
        attribution: '&copy; OpenStreetMap contributors',
        subdomains: 'abcd',
        maxZoom: 20
    }).addTo(map);

    map.on('click', (e) => {
        closeDetails(); // Close details if open
        if (!token) {
            els.loginModal.classList.remove('hidden');
            return;
        }
        selectedCoords = e.latlng;
        openReportModal();
    });
}

function bindEvents() {
    // Login
    els.loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const email = document.getElementById('login-email').value;
        const pass = document.getElementById('login-pass').value;
        const err = document.getElementById('login-error');
        
        try {
            const res = await fetch(`${API_BASE_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password: pass })
            });
            if (!res.ok) throw new Error('Nieprawidłowe dane logowania');
            const data = await res.json();
            token = data.token;
            localStorage.setItem('token', token);
            if (data.role) {
                localStorage.setItem('role', data.role);
            }
            els.loginModal.classList.add('hidden');
            updateAuthState();
            loadTickets();
        } catch (error) {
            err.textContent = error.message;
            err.style.display = 'block';
        }
    });

    els.btnAuthAction.addEventListener('click', () => {
        if (token) {
            // Handle Logout
            token = null;
            localStorage.removeItem('token');
            localStorage.removeItem('role');
            updateAuthState();
        } else {
            // Handle Login Prompt
            els.loginModal.classList.remove('hidden');
        }
    });
    
    els.btnCloseLogin.addEventListener('click', () => {
        els.loginModal.classList.add('hidden');
    });

    // Report New Event Button
    els.btnReportNew.addEventListener('click', () => {
        if (!token) {
            els.loginModal.classList.remove('hidden');
            return;
        }
        // Use map center if they just click the button
        selectedCoords = map.getCenter();
        openReportModal();
    });

    els.btnCloseReport.addEventListener('click', () => {
        els.reportModal.classList.add('hidden');
        els.reportForm.reset();
    });

    // Submit Report Form
    els.reportForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const title = document.getElementById('report-title').value;
        const desc = document.getElementById('report-desc').value;
        const priority = document.querySelector('input[name="severity"]:checked').value;
        const address = document.getElementById('report-address').value;
        const city = document.getElementById('report-city').value || "Gdańsk";
        
        // Category radio isn't in backend schema, but user can put it in description or note
        const category = document.querySelector('input[name="category"]:checked').value;

        const payload = {
            title,
            description: `[${category}] ${desc}`, // Store category in description
            latitude: selectedCoords.lat,
            longitude: selectedCoords.lng,
            city: city,
            street: address || "Unknown",
            district: "Center",
            buildingNumber: "",
            postcode: "00-000",
            priority: parseInt(priority)
        };

        try {
            const res = await fetch(`${API_BASE_URL}/tickets`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
                body: JSON.stringify(payload)
            });
            if (res.ok) {
                els.reportModal.classList.add('hidden');
                els.reportForm.reset();
                loadTickets();
            }
        } catch (e) {
            console.error(e);
        }
    });

    // Details Panel Close
    els.btnCloseDetails.addEventListener('click', closeDetails);

    // Filters
    els.filterItems.forEach(item => {
        item.addEventListener('click', () => {
            els.filterItems.forEach(i => i.classList.remove('active'));
            item.classList.add('active');
            currentFilter = item.dataset.status;
            renderMarkers();
        });
    });

    // View Toggles
    els.viewToggles.forEach(btn => {
        btn.addEventListener('click', () => {
            els.viewToggles.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            
            els.mapView.classList.remove('active');
            els.listView.classList.remove('active');
            if (els.adminView) els.adminView.classList.remove('active');
            
            const view = btn.dataset.view;
            if (view === 'map') {
                els.mapView.classList.add('active');
                if (map) map.invalidateSize();
            } else if (view === 'list') {
                els.listView.classList.add('active');
            } else if (view === 'admin') {
                if (els.adminView) {
                    els.adminView.classList.add('active');
                    loadAdminUsers();
                }
            }
        });
    });

    if (els.createUserForm) {
        els.createUserForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const firstName = document.getElementById('admin-user-firstname').value;
            const lastName = document.getElementById('admin-user-lastname').value;
            const email = document.getElementById('admin-user-email').value;
            const password = document.getElementById('admin-user-password').value;
            const role = document.getElementById('admin-user-role').value;
            const err = document.getElementById('create-user-error');
            
            try {
                const res = await fetch(`${API_BASE_URL}/users`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
                    body: JSON.stringify({ firstName, lastName, mail: email, password, role })
                });
                if (!res.ok) {
                    const errText = await res.text();
                    throw new Error(errText || 'Failed to create user');
                }
                closeCreateUserModal();
                loadAdminUsers();
            } catch (error) {
                err.textContent = error.message;
                err.style.display = 'block';
            }
        });
    }
}

function openReportModal() {
    document.getElementById('report-coords').textContent = `${selectedCoords.lat.toFixed(4)}, ${selectedCoords.lng.toFixed(4)}`;
    els.reportModal.classList.remove('hidden');
}

async function loadTickets() {
    try {
        const headers = {};
        if (token) headers['Authorization'] = `Bearer ${token}`;
        
        const res = await fetch(`${API_BASE_URL}/tickets`, {
            headers: headers
        });
        if (!res.ok) {
            if (res.status === 401 && !token) {
                console.warn("Serwer wymaga uwierzytelnienia, aby wyświetlić zgłoszenia. Zgłoszenia nie zostaną załadowane dla gości.");
            }
            return;
        }
        allTickets = await res.json();
        updateStats();
        renderMarkers();
    } catch (e) {
        console.error("Failed to load tickets", e);
    }
}

function updateStats() {
    let stats = { all: 0, pending: 0, progress: 0, resolved: 0, rejected: 0 };
    allTickets.forEach(t => {
        stats.all++;
        if (t.status === 1 || t.status === 'New') stats.pending++;
        else if (t.status === 2 || t.status === 'InProgress') stats.progress++;
        else if (t.status === 3 || t.status === 'Resolved') stats.resolved++;
        else if (t.status === 4 || t.status === 'Rejected') stats.rejected++;
    });

    els.counts.all.textContent = stats.all;
    els.counts.pending.textContent = stats.pending;
    els.counts.progress.textContent = stats.progress;
    els.counts.resolved.textContent = stats.resolved;
    els.counts.rejected.textContent = stats.rejected;

    els.stats.total.textContent = stats.all;
    els.stats.pending.textContent = stats.pending;
    els.stats.inprogress.textContent = stats.progress;
    els.stats.resolved.textContent = stats.resolved;
}

function renderMarkers() {
    // Clear existing
    currentMarkers.forEach(m => map.removeLayer(m));
    currentMarkers = [];

    const filtered = allTickets.filter(t => {
        if (currentFilter === 'all') return true;
        let s = t.status;
        if (currentFilter === '1' && (s === 1 || s === 'New')) return true;
        if (currentFilter === '2' && (s === 2 || s === 'InProgress')) return true;
        if (currentFilter === '3' && (s === 3 || s === 'Resolved')) return true;
        if (currentFilter === '4' && (s === 4 || s === 'Rejected')) return true;
        return false;
    });

    // Render List
    els.listContainer.innerHTML = '';

    filtered.forEach(t => {
        if (!t.latitude || !t.longitude) return;

        // Priority mapping
        const p = t.priority || 2; 
        let iconHtml = '<i class="fa-solid fa-triangle-exclamation"></i>';
        let prioClass = 'tag-high';
        let prioText = 'ŚREDNI';
        if (p === 1) { iconHtml = '<i class="fa-solid fa-info"></i>'; prioText = 'NISKI'; prioClass = 'tag-verified'; }
        if (p === 2) { iconHtml = '<i class="fa-solid fa-wrench"></i>'; }
        if (p === 3) { iconHtml = '<i class="fa-solid fa-fire"></i>'; prioText = 'WYSOKI'; prioClass = 'tag-high'; }
        if (p === 4) { iconHtml = '<i class="fa-solid fa-fire"></i>'; prioText = 'KRYTYCZNY'; prioClass = 'tag-critical'; }
        
        let statusText = 'NOWE';
        if (t.status === 2 || t.status === 'InProgress') statusText = 'W TOKU';
        if (t.status === 3 || t.status === 'Resolved') statusText = 'ROZWIĄZANE';
        if (t.status === 4 || t.status === 'Rejected') statusText = 'ODRZUCONE';

        // Add Marker
        const icon = L.divIcon({
            html: `<div class="custom-marker sev-${p}">${iconHtml}</div>`,
            className: '',
            iconSize: [32, 32],
            iconAnchor: [16, 16]
        });

        const marker = L.marker([t.latitude, t.longitude], { icon }).addTo(map);
        marker.on('click', () => showDetails(t));
        currentMarkers.push(marker);

        // Add List Item
        const item = document.createElement('div');
        item.style.padding = '1.25rem';
        item.style.backgroundColor = 'white';
        item.style.borderRadius = '0.5rem';
        item.style.boxShadow = '0 1px 3px rgba(0,0,0,0.1)';
        item.style.cursor = 'pointer';
        item.style.transition = 'transform 0.2s, box-shadow 0.2s';
        
        item.addEventListener('mouseenter', () => {
            item.style.transform = 'translateY(-2px)';
            item.style.boxShadow = '0 4px 6px rgba(0,0,0,0.1)';
        });
        item.addEventListener('mouseleave', () => {
            item.style.transform = 'translateY(0)';
            item.style.boxShadow = '0 1px 3px rgba(0,0,0,0.1)';
        });
        
        item.innerHTML = `
            <div style="display:flex; justify-content:space-between; align-items:flex-start; margin-bottom: 0.5rem;">
                <h4 style="margin:0; font-size: 1rem; color: var(--text-dark);">${t.title}</h4>
                <div class="tags">
                    <span class="tag tag-gray">${statusText}</span>
                    <span class="tag ${prioClass}">${prioText}</span>
                </div>
            </div>
            <p style="font-size: 0.875rem; color: var(--text-gray); margin-bottom: 0.5rem;"><i class="fa-solid fa-location-dot"></i> ${t.fullAddress || 'Nieznana lokalizacja'}</p>
            <p style="font-size: 0.875rem; color: var(--text-gray); margin:0;">${t.creationTimestamp ? new Date(t.creationTimestamp).toLocaleDateString() : 'brak'}</p>
        `;
        
        item.addEventListener('click', () => showDetails(t));
        els.listContainer.appendChild(item);
    });
}

async function showDetails(t) {
    if (isSelectingLocation) return;
    
    document.getElementById('det-title').textContent = t.title;
    
    // Extract category if we embedded it in description
    let desc = t.description || 'Brak opisu';
    let cat = "INNE";
    if (desc.startsWith('[')) {
        const end = desc.indexOf(']');
        if (end > 0) {
            cat = desc.substring(1, end).toUpperCase();
            desc = desc.substring(end + 1).trim();
        }
    }
    document.getElementById('det-desc').textContent = desc;
    
    document.querySelector('#details-panel .tag-gray').textContent = cat.toUpperCase();
    
    document.getElementById('det-address').textContent = t.fullAddress || 'Nieznana lokalizacja';
    document.getElementById('det-coords').textContent = `Współrzędne: ${t.latitude ? t.latitude.toFixed(4) : '0'}, ${t.longitude ? t.longitude.toFixed(4) : '0'}`;
    
    document.getElementById('det-user').textContent = t.creatorName || "Nieznany użytkownik";
    if(t.creationTimestamp) {
        document.getElementById('det-date').textContent = new Date(t.creationTimestamp).toLocaleString();
    } else {
        document.getElementById('det-date').textContent = 'brak';
    }

    // Tags
    let statusText = 'NOWE';
    if (t.status === 2 || t.status === 'InProgress') statusText = 'W TOKU';
    if (t.status === 3 || t.status === 'Resolved') statusText = 'ROZWIĄZANE';
    if (t.status === 4 || t.status === 'Rejected') statusText = 'ODRZUCONE';
    
    let prioText = 'ŚREDNI';
    let prioClass = 'tag-high';
    if (t.priority === 1) { prioText = 'NISKI'; prioClass = 'tag-verified'; } // Use blue
    if (t.priority === 3) { prioText = 'WYSOKI'; prioClass = 'tag-high'; } // Yellow/Orange
    if (t.priority === 4) { prioText = 'KRYTYCZNY'; prioClass = 'tag-critical'; } // Red

    document.getElementById('det-status').textContent = statusText;
    document.getElementById('det-severity').textContent = prioText;
    document.getElementById('det-severity').className = `tag ${prioClass}`;

    // Clear comments initially
    const commentsBox = document.getElementById('det-comments');
    const commentsHeader = commentsBox.previousElementSibling;
    if (commentsBox) {
        commentsBox.innerHTML = '<p style="font-size:0.875rem; color:var(--text-gray);">Ładowanie komentarzy...</p>';
    }
    if (commentsHeader) {
        commentsHeader.innerHTML = `<i class="fa-regular fa-comment"></i> Komentarze (...)`;
    }

    // Role-based Actions
    const actionBox = document.getElementById('det-actions');
    actionBox.innerHTML = '';
    
    if (currentUserRole.toUpperCase() === 'URZĘDNIK') {
        let innerHtml = `
            <div style="margin-top: 1.5rem; padding-top: 1.5rem; border-top: 1px solid var(--border-color);">
                <h3 style="font-size: 0.875rem; color: var(--text-dark); margin-bottom: 0.75rem;"><i class="fa-solid fa-gavel"></i> Akcje urzędnika</h3>
                <div style="display:flex; flex-direction:column; gap:0.5rem;">
                    
                    <div style="display:flex; gap:0.5rem; align-items: center;">
                        <label style="font-size: 0.8rem; width: 60px;">Status:</label>
                        <select id="official-status-select" style="flex:1; padding: 0.5rem; border: 1px solid var(--border-color); border-radius: 0.5rem;">
                            <option value="1" ${(t.status === 1 || t.status === 'New') ? 'selected' : ''}>Nowe</option>
                            <option value="2" ${(t.status === 2 || t.status === 'InProgress') ? 'selected' : ''}>W toku</option>
                            <option value="3" ${(t.status === 3 || t.status === 'Resolved') ? 'selected' : ''}>Rozwiązane</option>
                            <option value="4" ${(t.status === 4 || t.status === 'Rejected') ? 'selected' : ''}>Odrzucone</option>
                        </select>
                        <button class="btn-primary" onclick="changeStatusOfficial(${t.id})" style="flex:1;"><i class="fa-solid fa-pen"></i> Aktualizuj status</button>
                    </div>
                    
                    <div style="display:flex; gap:0.5rem; align-items: center; margin-bottom: 0.5rem;">
                        <label style="font-size: 0.8rem; width: 60px;">Komentarz:</label>
                        <textarea id="official-comment-input" rows="1" style="flex:1; padding: 0.5rem; border: 1px solid var(--border-color); border-radius: 0.5rem; font-family: inherit; font-size: 0.875rem;" placeholder="Treść komentarza..."></textarea>
                        <button class="btn-primary" onclick="addCommentOfficial(${t.id})" style="background: var(--primary-color);"><i class="fa-solid fa-comment"></i> Dodaj komentarz</button>
                    </div>

                    <div style="display:flex; gap:0.5rem; align-items: center;">
                        <label style="font-size: 0.8rem; width: 60px;">Priorytet:</label>
                        <select id="official-priority-select" style="flex:1; padding: 0.5rem; border: 1px solid var(--border-color); border-radius: 0.5rem;">
                            <option value="1" ${(t.priority === 1) ? 'selected' : ''}>Niski</option>
                            <option value="2" ${(t.priority === 2) ? 'selected' : ''}>Średni</option>
                            <option value="3" ${(t.priority === 3) ? 'selected' : ''}>Wysoki</option>
                            <option value="4" ${(t.priority === 4) ? 'selected' : ''}>Krytyczny</option>
                        </select>
                    </div>
                    <div style="display:flex; gap:0.5rem; align-items: center;">
                        <label style="font-size: 0.8rem; width: 60px;">Wydział:</label>
                        <select id="official-department-select" style="flex:1; padding: 0.5rem; border: 1px solid var(--border-color); border-radius: 0.5rem;">
                            <option value="1" ${(t.departmentName === "Wydział Bezpieczeństwa i Zarządzania Kryzysowego") ? 'selected' : ''}>Wydział Bezpieczeństwa i Zarządzania Kryzysowego</option>
                            <option value="2" ${(t.departmentName === "Gdański Zarząd Dróg") ? 'selected' : ''}>Gdański Zarząd Dróg</option>
                            <option value="3" ${(t.departmentName === "Gdański Zarząd Zieleni") ? 'selected' : ''}>Gdański Zarząd Zieleni</option>
                            <option value="4" ${(t.departmentName === "Wydział Ekologii i Energetyki") ? 'selected' : ''}>Wydział Ekologii i Energetyki</option>
                            <option value="5" ${(t.departmentName === "Wydział Gospodarki Komunalnej") ? 'selected' : ''}>Wydział Gospodarki Komunalnej</option>
                            <option value="6" ${(t.departmentName === "Wydział Infrastruktury") ? 'selected' : ''}>Wydział Infrastruktury</option>
                            <option value="7" ${(t.departmentName === "Straż Miejska") ? 'selected' : ''}>Straż Miejska</option>
                            <option value="8" ${(t.departmentName === "Zarząd Transportu Miejskiego") ? 'selected' : ''}>Zarząd Transportu Miejskiego</option>
                            <option value="9" ${(t.departmentName === "Gdańskie Wodociągi") ? 'selected' : ''}>Gdańskie Wodociągi</option>
                        </select>
                        <button class="btn-primary" onclick="modifyTicketOfficial(${t.id})" style="flex:1; background: var(--secondary-color);"><i class="fa-solid fa-edit"></i> Modyfikuj</button>
                    </div>

                    <button class="btn-primary full-width" onclick="rejectTicket(${t.id})" style="background: var(--sev-critical); margin-top: 0.5rem;"><i class="fa-solid fa-trash"></i> Usuń zgłoszenie</button>
                </div>
            </div>`;
        actionBox.innerHTML = innerHtml;
        
    } else if (currentUserRole.toUpperCase() === 'WYKONAWCA') {
        let innerHtml = `
            <div style="margin-top: 1.5rem; padding-top: 1.5rem; border-top: 1px solid var(--border-color);">
                <h3 style="font-size: 0.875rem; color: var(--text-dark); margin-bottom: 0.75rem;"><i class="fa-solid fa-hard-hat"></i> Akcje wykonawcy</h3>`;
                
        if (t.status === 2 || t.status === 'InProgress') {
            innerHtml += `
                <div style="display:flex; flex-direction:column; gap:0.5rem;">
                    <button class="btn-primary full-width" onclick="markTicketResolved(${t.id})" style="background: var(--status-resolved);"><i class="fa-solid fa-check-double"></i> Oznacz jako rozwiązane</button>
                    <div style="border-top: 1px solid var(--border-color); margin-top: 0.5rem; padding-top: 0.5rem;">
                        <textarea id="contractor-comment" rows="2" placeholder="Dodaj komentarz bez rozwiązywania..." style="width: 100%; margin-bottom: 0.5rem; padding: 0.5rem; border: 1px solid var(--border-color); border-radius: 0.5rem; font-family: inherit; font-size: 0.875rem;"></textarea>
                        <button class="btn-primary full-width" onclick="addContractorComment(${t.id})" style="background: var(--primary-color);"><i class="fa-solid fa-comment"></i> Dodaj komentarz</button>
                    </div>
                </div>`;
        } else {
            innerHtml += `<p style="font-size: 0.8rem; color: var(--text-gray);">Brak dostępnych akcji. Możesz modyfikować tylko zgłoszenia "W toku".</p>`;
        }
        innerHtml += `</div>`;
        actionBox.innerHTML = innerHtml;
    }

    els.detailsPanel.classList.remove('hidden');

    // Fetch full details for History (status logs)
    if (commentsBox) {
        try {
            const res = await fetch(`${API_BASE_URL}/tickets/${t.id}`);
            if(res.ok) {
                const details = await res.json();
                
                // Render comments
                if (details.history && details.history.length > 0) {
                    if(commentsHeader) commentsHeader.innerHTML = `<i class="fa-regular fa-comment"></i> Komentarze (${details.history.length})`;
                    commentsBox.innerHTML = '';
                    details.history.forEach(log => {
                        const logDate = new Date(log.timestamp).toLocaleString();
                        let logStatus = 'Aktualizacja';
                        if (log.title === 1 || log.title === 'New') logStatus = 'NOWE';
                        if (log.title === 2 || log.title === 'InProgress') logStatus = 'W TOKU';
                        if (log.title === 3 || log.title === 'Resolved') logStatus = 'ROZWIĄZANE';
                        if (log.title === 4 || log.title === 'Rejected') logStatus = 'ODRZUCONE';
                        
                        
                        let deleteBtnHtml = '';
                        if (currentUserRole.toUpperCase() === 'URZĘDNIK') {
                            deleteBtnHtml = `<button onclick="deleteCommentOfficial(${log.id})" style="background:none; border:none; color:var(--sev-critical); cursor:pointer; font-size:0.75rem;"><i class="fa-solid fa-trash"></i> Usuń</button>`;
                        }

                        commentsBox.innerHTML += `
                            <div style="margin-bottom: 1rem; padding-bottom: 1rem; border-bottom: 1px solid var(--border-color);">
                                <div style="display:flex; justify-content:space-between; margin-bottom:0.25rem;">
                                    <strong style="font-size: 0.875rem; color: var(--text-dark);">${log.creatorName || 'System'}</strong>
                                    <div style="display:flex; gap:0.5rem; align-items:center;">
                                        <span style="font-size: 0.75rem; color: var(--text-gray);">${logDate}</span>
                                        ${deleteBtnHtml}
                                    </div>
                                </div>
                                <div style="font-size: 0.75rem; color: var(--text-gray); margin-bottom: 0.5rem;">
                                    Status: <strong>${logStatus}</strong>
                                </div>
                                <p style="font-size: 0.875rem; color: var(--text-dark); margin:0;">${log.comment}</p>
                            </div>
                        `;
                    });
                } else {
                    if(commentsHeader) commentsHeader.innerHTML = `<i class="fa-regular fa-comment"></i> Komentarze (0)`;
                    commentsBox.innerHTML = '<p style="font-size:0.875rem; color:var(--text-gray);">Brak komentarzy.</p>';
                }
            } else {
                commentsBox.innerHTML = '<p style="font-size:0.875rem; color:var(--sev-critical);">Nie udało się załadować komentarzy.</p>';
            }
        } catch(e) {
            commentsBox.innerHTML = '<p style="font-size:0.875rem; color:var(--sev-critical);">Nie udało się załadować komentarzy.</p>';
        }
    }
}

function closeDetails() {
    els.detailsPanel.classList.add('hidden');
}

// Global API Action Handlers for HTML onclick
window.changeStatusOfficial = async (id) => {
    const statusSelect = document.getElementById('official-status-select');
    const newStatus = parseInt(statusSelect.value);
    const commentInput = document.getElementById('official-comment-input');
    const comment = commentInput && commentInput.value.trim() !== '' ? commentInput.value : 'Status zaktualizowany przez urzędnika.';
    
    const res = await fetch(`${API_BASE_URL}/tickets/${id}/official-status`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ newStatus: newStatus, comment: comment })
    });
    if(res.ok) {
        closeDetails();
        loadTickets();
    } else {
        alert('Operacja nie powiodła się. Upewnij się, że jesteś zalogowany jako urzędnik.');
    }
};

window.modifyTicketOfficial = async (id) => {
    const prioritySelect = document.getElementById('official-priority-select');
    const deptSelect = document.getElementById('official-department-select');
    
    const newPriority = parseInt(prioritySelect.value);
    const newDept = parseInt(deptSelect.value);
    
    const res = await fetch(`${API_BASE_URL}/tickets/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ publicBodyDepartmentId: newDept, priority: newPriority })
    });
    
    if(res.ok) {
        closeDetails();
        loadTickets();
    } else {
        alert('Operacja nie powiodła się. Nie udało się zmodyfikować zgłoszenia.');
    }
};

window.rejectTicket = async (id) => {
    if(!confirm("Czy na pewno chcesz całkowicie odrzucić i usunąć to zgłoszenie?")) return;
    const res = await fetch(`${API_BASE_URL}/tickets/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if(res.ok) {
        closeDetails();
        loadTickets();
    } else {
        alert('Nie udało się usunąć zgłoszenia.');
    }
};

window.markTicketResolved = async (id) => {
    const res = await fetch(`${API_BASE_URL}/tickets/${id}/status`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ newStatus: 3, comment: "Zgłoszenie rozwiązane przez wykonawcę." })
    });
    if(res.ok) {
        closeDetails();
        loadTickets();
    } else {
        const errText = await res.text();
        alert('Operacja nie powiodła się. Upewnij się, że jesteś przypisany do tego zgłoszenia. Błąd: ' + errText);
    }
};

window.addContractorComment = async (id) => {
    const comment = document.getElementById('contractor-comment').value;
    if(!comment.trim()) {
        alert('Wpisz treść komentarza.');
        return;
    }
    const res = await fetch(`${API_BASE_URL}/tickets/${id}/status`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ newStatus: 2, comment: comment })
    });
    if(res.ok) {
        closeDetails();
        loadTickets();
    } else {
        const errText = await res.text();
        alert('Operacja nie powiodła się. Upewnij się, że jesteś przypisany do tego zgłoszenia. Błąd: ' + errText);
    }
};

window.addCommentOfficial = async (id) => {
    const commentInput = document.getElementById('official-comment-input');
    if (!commentInput || !commentInput.value.trim()) {
        alert('Proszę wpisać treść komentarza.');
        return;
    }
    
    const res = await fetch(`${API_BASE_URL}/tickets/${id}/comments`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ comment: commentInput.value.trim() })
    });
    
    if (res.ok) {
        closeDetails();
        loadTickets();
    } else {
        const errText = await res.text();
        alert('Operacja nie powiodła się. Błąd: ' + errText);
    }
};

window.deleteCommentOfficial = async (commentId) => {
    if (!confirm('Czy na pewno chcesz usunąć ten komentarz?')) return;
    
    const res = await fetch(`${API_BASE_URL}/tickets/comments/${commentId}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });
    
    if (res.ok) {
        closeDetails();
        loadTickets();
    } else {
        alert('Nie udało się usunąć komentarza.');
    }
};

window.openCreateUserModal = () => {
    if (els.createUserModal) {
        els.createUserModal.classList.remove('hidden');
    }
};

window.closeCreateUserModal = () => {
    if (els.createUserModal) {
        els.createUserModal.classList.add('hidden');
        els.createUserForm.reset();
        const err = document.getElementById('create-user-error');
        if (err) err.style.display = 'none';
    }
};

window.loadAdminUsers = async () => {
    if (!els.adminUsersList) return;
    els.adminUsersList.innerHTML = '<tr><td colspan="4" style="padding:2rem; text-align:center; color:var(--text-gray);">Ładowanie użytkowników...</td></tr>';
    
    try {
        const res = await fetch(`${API_BASE_URL}/users`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (!res.ok) throw new Error('Nie udało się załadować użytkowników');
        const users = await res.json();
        
        els.adminUsersList.innerHTML = '';
        if (users.length === 0) {
            els.adminUsersList.innerHTML = '<tr><td colspan="4" style="padding:2rem; text-align:center; color:var(--text-gray);">Nie znaleziono użytkowników.</td></tr>';
            return;
        }
        
        users.forEach(u => {
            els.adminUsersList.innerHTML += `
                <tr style="border-bottom: 1px solid var(--border-color);">
                    <td style="padding:1rem; font-weight:500; color:var(--text-dark);">${u.firstName} ${u.lastName}</td>
                    <td style="padding:1rem; color:var(--text-gray);">${u.mail}</td>
                    <td style="padding:1rem;">
                        <select onchange="changeAdminUserRole(${u.id}, this.value)" style="padding:0.4rem; border:1px solid var(--border-color); border-radius:0.375rem; background:#fff; font-size:0.875rem;">
                            <option value="Resident" ${u.role === 'Resident' ? 'selected' : ''}>Mieszkaniec</option>
                            <option value="Official" ${u.role === 'Official' ? 'selected' : ''}>Urzędnik</option>
                            <option value="Contractor" ${u.role === 'Contractor' ? 'selected' : ''}>Wykonawca</option>
                            <option value="Admin" ${u.role === 'Admin' ? 'selected' : ''}>Administrator</option>
                        </select>
                    </td>
                    <td style="padding:1rem; text-align:right;">
                        <button class="btn-secondary" onclick="deleteAdminUser(${u.id})" style="background:var(--sev-critical); padding:0.4rem 0.8rem; font-size:0.75rem; border-radius:0.375rem; border:none; color:white; cursor:pointer;"><i class="fa-solid fa-trash"></i> Usuń</button>
                    </td>
                </tr>
            `;
        });
    } catch (e) {
        els.adminUsersList.innerHTML = `<tr><td colspan="4" style="padding:2rem; text-align:center; color:red;">Błąd: ${e.message}</td></tr>`;
    }
};

window.deleteAdminUser = async (id) => {
    if (!confirm('Czy na pewno chcesz usunąć tego użytkownika?')) return;
    
    try {
        const res = await fetch(`${API_BASE_URL}/users/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (!res.ok) throw new Error('Nie udało się usunąć użytkownika');
        
        if (res.status === 200) {
            // Check if backend returned soft-delete notification instead of 204
            const text = await res.text();
            alert(text);
        }
        loadAdminUsers();
    } catch (e) {
        alert(e.message);
    }
};

window.changeAdminUserRole = async (id, newRole) => {
    try {
        const res = await fetch(`${API_BASE_URL}/users/${id}/role`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
            body: JSON.stringify({ role: newRole })
        });
        if (!res.ok) throw new Error('Nie udało się zmienić roli użytkownika');
        alert('Rola została zaktualizowana pomyślnie.');
        loadAdminUsers();
    } catch (e) {
        alert(e.message);
        loadAdminUsers();
    }
};

document.addEventListener('DOMContentLoaded', init);

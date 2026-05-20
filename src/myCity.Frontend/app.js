const API_BASE_URL = 'http://localhost:5018/api';
let map = null;
let currentMarkers = [];
let allTickets = [];
let currentFilter = 'all';

// State
let token = localStorage.getItem('token');
let isSelectingLocation = false;
let selectedCoords = null;

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
        els.btnAuthAction.innerHTML = '<i class="fa-solid fa-arrow-right-from-bracket"></i> Logout';
        els.btnAuthAction.classList.replace('btn-primary', 'btn-secondary');
        
        // Very basic JWT parsing to get role
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || "CITIZEN";
            els.roleBadge.textContent = role.toUpperCase();
        } catch (e) {
            els.roleBadge.textContent = 'CITIZEN';
        }
    } else {
        els.btnAuthAction.innerHTML = '<i class="fa-solid fa-right-to-bracket"></i> Login';
        els.btnAuthAction.classList.replace('btn-secondary', 'btn-primary');
        els.roleBadge.textContent = 'GUEST';
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
            if (!res.ok) throw new Error('Invalid credentials');
            const data = await res.json();
            token = data.token;
            localStorage.setItem('token', token);
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
                console.warn("Backend requires authentication to view tickets. Tickets will not be loaded for guests.");
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
        if (t.currentStatus === 1 || t.currentStatus === 'New') stats.pending++;
        else if (t.currentStatus === 2 || t.currentStatus === 'InProgress') stats.progress++;
        else if (t.currentStatus === 3 || t.currentStatus === 'Resolved') stats.resolved++;
        else if (t.currentStatus === 4 || t.currentStatus === 'Rejected') stats.rejected++;
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
        let s = t.currentStatus;
        if (currentFilter === '1' && (s === 1 || s === 'New')) return true;
        if (currentFilter === '2' && (s === 2 || s === 'InProgress')) return true;
        if (currentFilter === '3' && (s === 3 || s === 'Resolved')) return true;
        if (currentFilter === '4' && (s === 4 || s === 'Rejected')) return true;
        return false;
    });

    filtered.forEach(t => {
        if (!t.latitude || !t.longitude) return;

        // Priority icon mapping
        const p = t.priority || 2; 
        let iconHtml = '<i class="fa-solid fa-triangle-exclamation"></i>';
        if (p === 1) iconHtml = '<i class="fa-solid fa-info"></i>';
        if (p === 2) iconHtml = '<i class="fa-solid fa-wrench"></i>';
        if (p === 3) iconHtml = '<i class="fa-solid fa-fire"></i>';
        
        const icon = L.divIcon({
            html: `<div class="custom-marker sev-${p}">${iconHtml}</div>`,
            className: '',
            iconSize: [32, 32],
            iconAnchor: [16, 16]
        });

        const marker = L.marker([t.latitude, t.longitude], { icon }).addTo(map);
        marker.on('click', () => showDetails(t));
        currentMarkers.push(marker);
    });
}

function showDetails(t) {
    if (isSelectingLocation) return;
    
    document.getElementById('det-title').textContent = t.title;
    
    // Extract category if we embedded it in description
    let desc = t.description || 'No description provided';
    let cat = "OTHER";
    if (desc.startsWith('[')) {
        const end = desc.indexOf(']');
        if (end > 0) {
            cat = desc.substring(1, end).toUpperCase();
            desc = desc.substring(end + 1).trim();
        }
    }
    document.getElementById('det-desc').textContent = desc;
    document.querySelector('#details-panel .tag-gray').textContent = cat;
    
    document.getElementById('det-address').textContent = `${t.street} ${t.buildingNumber || ''}, ${t.city}`;
    document.getElementById('det-coords').textContent = `Coordinates: ${t.latitude.toFixed(4)}, ${t.longitude.toFixed(4)}`;
    
    const d = new Date(t.creationTimestamp);
    document.getElementById('det-date').textContent = d.toLocaleString();

    // Tags
    let statusText = 'PENDING';
    if (t.currentStatus === 2 || t.currentStatus === 'InProgress') statusText = 'IN PROGRESS';
    if (t.currentStatus === 3 || t.currentStatus === 'Resolved') statusText = 'RESOLVED';
    if (t.currentStatus === 4 || t.currentStatus === 'Rejected') statusText = 'REJECTED';
    
    let prioText = 'MEDIUM';
    let prioClass = 'tag-high';
    if (t.priority === 1) { prioText = 'LOW'; prioClass = 'tag-verified'; } // Use blue
    if (t.priority === 3) { prioText = 'HIGH'; prioClass = 'tag-high'; } // Yellow/Orange
    if (t.priority === 4) { prioText = 'CRITICAL'; prioClass = 'tag-critical'; } // Red

    document.getElementById('det-status').textContent = statusText;
    document.getElementById('det-severity').textContent = prioText;
    document.getElementById('det-severity').className = `tag ${prioClass}`;

    els.detailsPanel.classList.remove('hidden');
}

function closeDetails() {
    els.detailsPanel.classList.add('hidden');
}

document.addEventListener('DOMContentLoaded', init);

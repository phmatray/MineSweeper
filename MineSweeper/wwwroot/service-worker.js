// Service Worker for MineSweeper PWA
const CACHE_NAME = 'minesweeper-v1.0.0';
const urlsToCache = [
    './',
    './index.html',
    './css/tailwind.css',
    './css/app.css',
    './js/particles.js',
    './manifest.json',
    './icon-192.png',
    './icon-512.png',
    // Blazor framework files will be added dynamically
];

// Install event - cache resources
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
            .catch(err => {
                console.error('Failed to cache:', err);
            })
    );
});

// Activate event - clean up old caches
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== CACHE_NAME && cacheName.startsWith('minesweeper-')) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

// Fetch event - serve from cache when offline
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // Cache hit - return response
                if (response) {
                    return response;
                }

                // Clone the request
                const fetchRequest = event.request.clone();

                return fetch(fetchRequest).then(response => {
                    // Check if valid response
                    if (!response || response.status !== 200 || response.type !== 'basic') {
                        return response;
                    }

                    // Clone the response
                    const responseToCache = response.clone();

                    // Add to cache
                    caches.open(CACHE_NAME)
                        .then(cache => {
                            // Cache Blazor framework files
                            if (event.request.url.includes('_framework') || 
                                event.request.url.includes('.dll') ||
                                event.request.url.includes('.wasm')) {
                                cache.put(event.request, responseToCache);
                            }
                        });

                    return response;
                });
            })
            .catch(() => {
                // Offline fallback
                if (event.request.destination === 'document') {
                    return caches.match('./index.html');
                }
            })
    );
});

// Listen for messages from the app
self.addEventListener('message', event => {
    if (event.data && event.data.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }
});
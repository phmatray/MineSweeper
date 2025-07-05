// Service Worker Registration for MineSweeper PWA
window.updateAvailable = false;

if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
        navigator.serviceWorker.register('./service-worker.js')
            .then(registration => {
                console.log('ServiceWorker registration successful');
                
                // Check for updates
                registration.addEventListener('updatefound', () => {
                    const newWorker = registration.installing;
                    newWorker.addEventListener('statechange', () => {
                        if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                            // New content available
                            window.updateAvailable = true;
                            console.log('New content available, refresh to update');
                        }
                    });
                });
            })
            .catch(err => {
                console.error('ServiceWorker registration failed:', err);
            });
    });
}
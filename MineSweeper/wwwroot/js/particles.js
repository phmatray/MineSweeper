// Particle Effects for MineSweeper
window.ParticleEffects = {
    // Create explosion particles when hitting a mine
    createExplosion: function(x, y) {
        const container = document.getElementById('particle-container');
        if (!container) return;
        
        const colors = ['#ef4444', '#f59e0b', '#f97316', '#dc2626'];
        const particleCount = 20;
        
        for (let i = 0; i < particleCount; i++) {
            const particle = document.createElement('div');
            particle.className = 'particle explosion-particle';
            particle.style.left = x + 'px';
            particle.style.top = y + 'px';
            particle.style.backgroundColor = colors[Math.floor(Math.random() * colors.length)];
            
            const angle = (Math.PI * 2 * i) / particleCount;
            const velocity = 50 + Math.random() * 100;
            const size = 4 + Math.random() * 8;
            
            particle.style.width = size + 'px';
            particle.style.height = size + 'px';
            particle.style.setProperty('--dx', Math.cos(angle) * velocity + 'px');
            particle.style.setProperty('--dy', Math.sin(angle) * velocity + 'px');
            
            container.appendChild(particle);
            
            // Remove particle after animation
            setTimeout(() => particle.remove(), 1000);
        }
    },
    
    // Create confetti particles for winning
    createConfetti: function() {
        const container = document.getElementById('particle-container');
        if (!container) return;
        
        const colors = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#ec4899'];
        const confettiCount = 50;
        
        for (let i = 0; i < confettiCount; i++) {
            setTimeout(() => {
                const confetti = document.createElement('div');
                confetti.className = 'particle confetti-particle';
                confetti.style.left = Math.random() * window.innerWidth + 'px';
                confetti.style.top = -20 + 'px';
                confetti.style.backgroundColor = colors[Math.floor(Math.random() * colors.length)];
                confetti.style.setProperty('--rotation', Math.random() * 360 + 'deg');
                confetti.style.setProperty('--drift', (Math.random() - 0.5) * 200 + 'px');
                
                container.appendChild(confetti);
                
                // Remove confetti after animation
                setTimeout(() => confetti.remove(), 3000);
            }, i * 50);
        }
    },
    
    // Create sparkle effect for successful cell reveal
    createSparkle: function(x, y) {
        const container = document.getElementById('particle-container');
        if (!container) return;
        
        const sparkle = document.createElement('div');
        sparkle.className = 'particle sparkle-particle';
        sparkle.style.left = x + 'px';
        sparkle.style.top = y + 'px';
        
        container.appendChild(sparkle);
        
        // Remove sparkle after animation
        setTimeout(() => sparkle.remove(), 600);
    },
    
    // Initialize particle container
    init: function() {
        if (!document.getElementById('particle-container')) {
            const container = document.createElement('div');
            container.id = 'particle-container';
            container.className = 'particle-container';
            document.body.appendChild(container);
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.ParticleEffects.init();
});
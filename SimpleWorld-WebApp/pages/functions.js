document.addEventListener('DOMContentLoaded', function() {
    var carousel = document.querySelector('.carousel');
    var indicators = document.querySelectorAll('.indicator');
    var images = document.querySelectorAll('.carousel-image');
    var currentIndex = 0;
    var autoplay = true;
    var interval;

    function showImage(index) {
        images.forEach(function(image) {
            image.style.display = 'none';
        });
        images[index].style.display = 'block';
        indicators.forEach(function(indicator) {
            indicator.classList.remove('active');
        });
        indicators[index].classList.add('active');
    }

    function nextImage() {
        currentIndex = (currentIndex + 1) % images.length;
        showImage(currentIndex);
    }

    function toggleAutoplay() {
        if (autoplay) {
            clearInterval(interval);
            toggleIndicator.textContent = '▶️'; // Símbolo de continuar
        } else {
            interval = setInterval(nextImage, 3000); // 3 segundos
            toggleIndicator.textContent = '⏸️'; // Símbolo de pausar
        }
        autoplay = !autoplay;
    }

    indicators.forEach(function(indicator, index) {
        indicator.addEventListener('click', function() {
            showImage(index);
            currentIndex = index;
        });
    });

    interval = setInterval(nextImage, 3000); // 3 segundos

    // Adicione um indicador de alternância (toggle) para pausar/continuar
    var toggleIndicator = document.createElement('span');
    toggleIndicator.className = 'toggle';
    toggleIndicator.textContent = '⏸️'; // Símbolo de pausar inicialmente
    toggleIndicator.addEventListener('click', toggleAutoplay);
    document.querySelector('.carousel-indicators').appendChild(toggleIndicator);
});

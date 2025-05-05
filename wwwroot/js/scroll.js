let scrollButton;

export function initScrollToTop() {
    // Создаем наблюдатель за изменениями DOM
    const observer = new MutationObserver((mutations, obs) => {
        scrollButton = document.getElementById('scrollToTop');
        if (scrollButton) {
            // Отключаем наблюдатель, когда нашли кнопку
            obs.disconnect();
            
            // Инициализируем обработчики
            initScrollHandlers();
        }
    });

    // Начинаем наблюдение
    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
}

function initScrollHandlers() {
    // Проверяем начальное состояние прокрутки
    checkScroll();
    
    // Используем throttle для оптимизации производительности
    let ticking = false;
    
    window.addEventListener('scroll', function() {
        if (!ticking) {
            window.requestAnimationFrame(function() {
                checkScroll();
                ticking = false;
            });
            ticking = true;
        }
    });
}

function checkScroll() {
    if (!scrollButton) return;
    
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    const scrollHeight = document.documentElement.scrollHeight;
    const clientHeight = document.documentElement.clientHeight;
    
    // Показываем кнопку, если прокрутили больше 300px ИЛИ достигли 80% страницы
    if (scrollTop > 300 || scrollTop > (scrollHeight - clientHeight) * 0.8) {
        scrollButton.classList.add('visible');
    } else {
        scrollButton.classList.remove('visible');
    }
}

export function scrollToTop() {
    if (!scrollButton) return;
    
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
} 
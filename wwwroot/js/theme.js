// Сохраняем текущую тему в localStorage
function saveTheme(theme) {
    localStorage.setItem('theme', theme);
}

// Получаем сохраненную тему
function getSavedTheme() {
    return localStorage.getItem('theme') || 'dark';
}

// Применяем тему
function applyTheme(theme) {
    document.body.className = theme + '-theme';
    saveTheme(theme);
}

// Инициализация темы при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    const savedTheme = getSavedTheme();
    applyTheme(savedTheme);
});

// Экспортируем функции для использования в Blazor
window.themeManager = {
    toggleTheme: function() {
        const currentTheme = getSavedTheme();
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        applyTheme(newTheme);
        return newTheme;
    },
    getCurrentTheme: function() {
        return getSavedTheme();
    }
}; 
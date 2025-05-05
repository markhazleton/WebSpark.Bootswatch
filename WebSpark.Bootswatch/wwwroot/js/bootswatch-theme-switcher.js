/**
 * WebSpark.Bootswatch Theme Switcher
 * JavaScript functionality for theme switching and light/dark mode toggle
 */

document.addEventListener('DOMContentLoaded', function () {
    // Initialize color mode toggle
    initColorModeToggle();
    
    // Theme switching functionality
    initThemeSwitcher();
});

/**
 * Initializes the color mode toggle button functionality
 */
function initColorModeToggle() {
    const colorModeToggle = document.getElementById('bootswatch-color-mode-toggle');
    const colorModeIcon = document.querySelector('.bootswatch-color-mode-icon');
    const colorModeText = document.querySelector('.bootswatch-color-mode-text');
    
    if (colorModeToggle && colorModeIcon && colorModeText) {
        // Set initial state
        updateColorModeToggle();
        
        // Add click event listener
        colorModeToggle.addEventListener('click', function () {
            // Toggle color mode
            const currentTheme = document.documentElement.getAttribute('data-bs-theme');
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
            
            // Update HTML attribute
            document.documentElement.setAttribute('data-bs-theme', newTheme);
            
            // Update button appearance
            updateColorModeToggle();
            
            // Save preference in cookie
            setColorModeCookie(newTheme, 30);
        });
    }
}

/**
 * Updates the color mode toggle button appearance based on current mode
 */
function updateColorModeToggle() {
    const currentTheme = document.documentElement.getAttribute('data-bs-theme');
    const colorModeIcon = document.querySelector('.bootswatch-color-mode-icon');
    const colorModeText = document.querySelector('.bootswatch-color-mode-text');
    
    if (currentTheme === 'dark') {
        colorModeIcon.textContent = '☀️';
        colorModeText.textContent = 'Light';
    } else {
        colorModeIcon.textContent = '🌙';
        colorModeText.textContent = 'Dark';
    }
}

/**
 * Sets a cookie to remember the user's color mode preference
 * @param {string} colorMode - The color mode ('light' or 'dark')
 * @param {number} expirationDays - Number of days until cookie expires
 */
function setColorModeCookie(colorMode, expirationDays) {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = "bootswatch-color-mode=" + colorMode + ";" + expires + ";path=/;samesite=strict";
}

/**
 * Initializes the theme switcher dropdown functionality
 */
function initThemeSwitcher() {
    // Get all theme dropdown items
    const themeDropdownItems = document.querySelectorAll('.bootswatch-dropdown-menu a[data-theme]');
    
    // Add click event listener to each item
    themeDropdownItems.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            
            // Get theme name and URL from data attributes
            const themeName = this.getAttribute('data-theme');
            const themeUrl = this.getAttribute('data-theme-url');
            
            // Switch the theme by changing the stylesheet href
            const themeStylesheet = document.getElementById('bootswatch-theme-stylesheet');
            if (themeStylesheet && themeUrl) {
                themeStylesheet.href = themeUrl;
                
                // Remove active class from all items and add to selected
                themeDropdownItems.forEach(item => item.classList.remove('active'));
                this.classList.add('active');
                
                // Save selection in a cookie that expires in 30 days
                setThemeCookie(themeName, 30);
            }
        });
    });
}

/**
 * Sets a cookie to remember the user's theme preference
 * @param {string} themeName - The name of the selected theme
 * @param {number} expirationDays - Number of days until cookie expires
 */
function setThemeCookie(themeName, expirationDays) {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = "bootswatch-theme=" + themeName + ";" + expires + ";path=/;samesite=strict";
}

/**
 * Sets the theme based on a theme name without requiring a page reload
 * @param {string} themeName - The name of the theme to apply
 */
function bootswatchSetTheme(themeName, themeUrl) {
    // Set the cookie
    setThemeCookie(themeName, 30);
    
    // Update the stylesheet
    const themeStylesheet = document.getElementById('bootswatch-theme-stylesheet');
    if (themeStylesheet && themeUrl) {
        themeStylesheet.href = themeUrl;
    }
}
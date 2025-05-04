// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function () {
    // Initialize color mode toggle
    initColorModeToggle();
    
    // Theme switching functionality
    initThemeSwitcher();
});

// Function to initialize color mode toggle
function initColorModeToggle() {
    const colorModeToggle = document.getElementById('color-mode-toggle');
    const colorModeIcon = document.querySelector('.color-mode-icon');
    const colorModeText = document.querySelector('.color-mode-text');
    
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

// Function to update color mode toggle appearance
function updateColorModeToggle() {
    const currentTheme = document.documentElement.getAttribute('data-bs-theme');
    const colorModeIcon = document.querySelector('.color-mode-icon');
    const colorModeText = document.querySelector('.color-mode-text');
    
    if (currentTheme === 'dark') {
        colorModeIcon.textContent = '☀️';
        colorModeText.textContent = 'Light';
    } else {
        colorModeIcon.textContent = '🌙';
        colorModeText.textContent = 'Dark';
    }
}

// Function to set color mode cookie
function setColorModeCookie(colorMode, expirationDays) {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = "color-mode=" + colorMode + ";" + expires + ";path=/;samesite=strict";
}

// Function to initialize theme switcher
function initThemeSwitcher() {
    // Get all theme dropdown items
    const themeDropdownItems = document.querySelectorAll('.dropdown-menu a[data-theme]');
    
    // Add click event listener to each item
    themeDropdownItems.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            
            // Get theme name and URL from data attributes
            const themeName = this.getAttribute('data-theme');
            const themeUrl = this.getAttribute('data-theme-url');
            
            // Switch the theme by changing the stylesheet href
            const themeStylesheet = document.getElementById('theme-stylesheet');
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

// Function to set the theme cookie
function setThemeCookie(themeName, expirationDays) {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = "bootswatch-theme=" + themeName + ";" + expires + ";path=/;samesite=strict";
}

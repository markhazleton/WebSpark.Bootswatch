// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Theme switching functionality
document.addEventListener('DOMContentLoaded', function () {
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
});

// Function to set the theme cookie
function setThemeCookie(themeName, expirationDays) {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = "bootswatch-theme=" + themeName + ";" + expires + ";path=/;samesite=strict";
}

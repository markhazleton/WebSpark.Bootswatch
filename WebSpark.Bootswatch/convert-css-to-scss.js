const fs = require('fs');
const path = require('path');

// Debugging helper
function log(message) {
  console.log(message);
  fs.appendFileSync("convert-scss.log", message + "\n");
}

// Clear previous log
if (fs.existsSync("convert-scss.log")) {
  fs.unlinkSync("convert-scss.log");
}

/**
 * Ensures a directory exists
 */
function ensureDirSync(dir) {
  log(`Ensuring directory exists: ${dir}`);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
    log(`Created directory: ${dir}`);
  } else {
    log(`Directory already exists: ${dir}`);
  }
}

/**
 * Converts CSS files to SCSS in a theme directory
 */
function convertCssToScss(themeDir) {
  const cssDir = path.join('src/style', themeDir, 'css');
  const scssDir = path.join('src/style', themeDir, 'scss');
  
  // Ensure the SCSS directory exists
  ensureDirSync(scssDir);
  
  // Check if CSS directory exists
  if (!fs.existsSync(cssDir)) {
    log(`CSS directory does not exist: ${cssDir}`);
    return;
  }
  
  try {
    const cssFiles = fs.readdirSync(cssDir)
      .filter(file => file.endsWith('.css'));
    
    for (const cssFile of cssFiles) {
      const cssPath = path.join(cssDir, cssFile);
      const scssFile = cssFile.replace('.css', '.scss');
      const scssPath = path.join(scssDir, scssFile);
      
      log(`Converting ${cssPath} to ${scssPath}`);
      
      // Read CSS content
      const cssContent = fs.readFileSync(cssPath, 'utf8');
      
      // Write as SCSS (no transformation needed, SCSS is a superset of CSS)
      fs.writeFileSync(scssPath, cssContent);
      
      log(`Converted ${cssFile} to ${scssFile}`);
    }
  } catch (error) {
    log(`Error converting CSS to SCSS: ${error.message}`);
  }
}

// Main execution
log("Starting CSS to SCSS conversion...");

try {
  // Get all theme directories
  const styleDir = path.resolve(__dirname, 'src/style');
  const themeDirs = fs.readdirSync(styleDir)
    .filter(item => fs.statSync(path.join(styleDir, item)).isDirectory());
  
  // Process each theme directory
  for (const themeDir of themeDirs) {
    log(`Processing theme directory: ${themeDir}`);
    convertCssToScss(themeDir);
  }
  
  log("CSS to SCSS conversion completed successfully.");
} catch (error) {
  log(`Error in conversion process: ${error.message}`);
  log(`Stack trace: ${error.stack}`);
}

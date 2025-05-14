const fs = require(`fs`);
const path = require(`path`);

/**
 * Recursively removes a directory and all its contents
 * @param {string} dir - The directory to remove
 */
function removeDirSync(dir) {
  console.log(`Removing directory: ${dir}`);
  
  if (!fs.existsSync(dir)) {
    console.log(`Directory does not exist: ${dir}`);
    return;
  }
  
  try {
    fs.rmSync(dir, { recursive: true, force: true });
    console.log(`Successfully removed directory: ${dir}`);
  } catch (error) {
    console.error(`Error removing directory ${dir}: ${error.message}`);
  }
}

/**
 * Creates a directory if it doesn`t exist
 * @param {string} dir - The directory to create
 */
function createDirSync(dir) {
  console.log(`Creating directory: ${dir}`);
  
  if (!fs.existsSync(dir)) {
    try {
      fs.mkdirSync(dir, { recursive: true });
      console.log(`Successfully created directory: ${dir}`);
    } catch (error) {
      console.error(`Error creating directory ${dir}: ${error.message}`);
    }
  } else {
    console.log(`Directory already exists: ${dir}`);
  }
}

/**
 * Clears a directory by removing it and recreating it
 * @param {string} dir - The directory to clear
 */
function clearDirSync(dir) {
  console.log(`Clearing directory: ${dir}`);
  removeDirSync(dir);
  createDirSync(dir);
}

// Main execution
console.log(`Starting cleanup process...`);

try {
  // Clear wwwroot directory
  clearDirSync(`wwwroot`);
  
  console.log(`Cleanup process completed successfully.`);
} catch (error) {
  console.error(`Error in cleanup process: ${error.message}`);
  console.error(`Stack trace: ${error.stack}`);
}

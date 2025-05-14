const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');
const fs = require('fs');

// Function to recursively get all SCSS files from theme directories
function getThemeEntries() {
  const entries = {};
  const styleDir = path.resolve(__dirname, 'src/style');
  
  // Get all theme directories
  const themeDirs = fs.readdirSync(styleDir)
    .filter(item => fs.statSync(path.join(styleDir, item)).isDirectory());
  
  // For each theme, create entry points for their SCSS files
  themeDirs.forEach(theme => {
    const themePath = path.join(styleDir, theme);
    const scssPath = path.join(themePath, 'scss');
    
    // Check if scss directory exists, if not, we'll create it in the build process
    if (fs.existsSync(scssPath)) {
      const scssFiles = fs.readdirSync(scssPath)
        .filter(file => file.endsWith('.scss'));
      
      scssFiles.forEach(file => {
        const name = file.replace('.scss', '');
        // Create two entries for each file - one for standard and one for minified
        entries[`style/${theme}/css/${name}`] = path.join(scssPath, file);
        entries[`style/${theme}/css/${name}.min`] = path.join(scssPath, file);
      });
    }
  });
  
  return entries;
}

module.exports = {
  mode: 'production',
  entry: getThemeEntries(),
  output: {
    path: path.resolve(__dirname, 'wwwroot'),
    filename: '[name].js', // This is not used for CSS files but is required
    clean: false, // Don't clean the output directory as we have other files there
  },
  module: {
    rules: [
      {
        test: /\.scss$/,
        use: [
          MiniCssExtractPlugin.loader,
          'css-loader',
          'sass-loader',
        ],
      },
    ],
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: '[name].css',
    }),
  ],
  optimization: {
    minimize: true,
    minimizer: [
      new CssMinimizerPlugin({
        minimizerOptions: {
          preset: [
            'default',
            {
              discardComments: { removeAll: true },
            },
          ],
        },
        minify: [
          CssMinimizerPlugin.cssnanoMinify,
        ],
      }),
      '...',
    ],
  },
};

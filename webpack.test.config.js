//
// Webpack build config for running tests.
//

var path = require('path');
var webpack = require('webpack');

var CONFIG = {
    testProjectDir: './test',
    testProjectFile: 'Gamurs.Fable.JsonAdapter.Test.fsproj',
    outputDir: 'bin',
};

module.exports = {
    mode: 'development',
    entry: path.join(__dirname, CONFIG.testProjectDir, CONFIG.testProjectFile),
    output: {
        path: path.join(__dirname, CONFIG.testProjectDir, CONFIG.outputDir),
        filename: 'bundle.js',
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: 'fable-loader'
        }],
    },
    plugins: [
        new webpack.NamedModulesPlugin()
    ],
};

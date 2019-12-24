var path = require('path');

module.exports = {
    entry: {
        client: './Scripts/main.jsx',
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, "wwwroot")
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    'style-loader',
                    'css-loader',
                    'sass-loader',
                ],
            },
        ],
    },
    devServer: {
        contentBase: path.resolve(__dirname, "wwwroot"),
        hot: true,
    },
    mode: 'development',
    resolve: {
        extensions: ['.js', '.jsx'],
    },
};
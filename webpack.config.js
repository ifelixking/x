const HtmlWebpackPlugin = require('html-webpack-plugin')

module.exports = {
	entry: {
		index: './src/web/index.js',
		admin: './src/web/admin/index.js'
	},
	output: {
		path: __dirname + '/dist/static',
		filename: '[name]-[hash].bundle.js'
	},
	plugins: [
		new HtmlWebpackPlugin({
			filename: 'index.html',
			chunks: ['index'],
		}),
		new HtmlWebpackPlugin({
			filename: 'admin.html',
			chunks: ['admin'],
		})
	]
}
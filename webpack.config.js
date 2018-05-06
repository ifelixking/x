const HtmlWebpackPlugin = require('html-webpack-plugin')
const path = require('path')

module.exports = {
	entry: {
		index: './src/web/index.js',
		admin: './src/web/admin/index.js'
	},
	output: {
		// output.path 不仅作用于js, 也作用于下面的 HtmlWebpackPlugin
		// 我的目的是要将除了.html文件之外的所有文件都放到staitc/asset/中, .html文件则放在static目录下
		path: __dirname + '/dist/static',		
		filename: 'asset/[name]-bundle.js',
		// 这里需要制定 publicPath, 时 HtmlWebpackPlugin 生成引用 js 文件为绝对路径, 因为使用了 react-router 的缘故, 会导致相对路径失效
		// node Server 会根据路径 转换为对某个具体的 html 文件的访问
		publicPath: '/'
	},
	plugins: [
		new HtmlWebpackPlugin({
			filename: 'index.html',
			chunks: ['index'],
			title: 'Welcom to x site',
			template: 'src/web/index.tpl.html'
		}),
		new HtmlWebpackPlugin({
			filename: 'admin.html',
			chunks: ['admin'],
			title: 'Admin console',
			template: 'src/web/index.tpl.html'
		})
	],
	devServer:{
		port: 8080,
		proxy: {
			'/api': "http://localhost"
		}
	},
	module: {
		rules: [
			{
				test: /\.js$/,
				exclude: /(node_modules|bower_components)/,
				include: path.resolve(__dirname, 'src/web'),
				use: {
					loader: 'babel-loader',
					options: {
						presets: ['@babel/preset-react']
					}
				}
			}
		]
	}
}
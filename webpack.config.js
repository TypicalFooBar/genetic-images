var path = require("path");

module.exports = {
	entry: './wwwsrc/js/main.js',
	output: {
		path: path.resolve(__dirname, "wwwroot/js"),
		publicPath: 'wwwroot/',
		filename: 'build.js'
	},
	module: {
		loaders: [
			{
				test: /\.vue$/,
				loader: 'vue-loader'
			}
		]
	},
	resolve: {
		alias: {
			vue: 'vue/dist/vue.js'
		}
	}
}
const { defineConfig } = require('@vue/cli-service');
const NodePolyfillPlugin = require('node-polyfill-webpack-plugin');
const webpack = require('webpack');

module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    port: 6989, // 更改为8081端口
    proxy: {
      "/api": {
        target: "https://localhost:7074/api", // 服务器请求的地址
        secure: false, // HTTPS需要配置这个参数
        changeOrigin: true, // 请求头host属性，true表示伪装为目标地址
        pathRewrite: {
          "^/api": "", // 移除 "/api" 前缀
        },
      },
    },
    client: {
      overlay: {
        errors: true,  // 显示错误覆盖层
        warnings: false,  // 隐藏警告覆盖层
      }
    }
  },
  configureWebpack: {
    plugins: [
      new NodePolyfillPlugin(),
      new webpack.ProvidePlugin({
        process: 'process/browser', // 显式提供 process 的 polyfill
      }),
    ],
    resolve: {
      fallback: {
        stream: require.resolve('stream-browserify'),
      },
    },
  },
});

const webpack = require('webpack');
const path = require('path');
const CleanPlugin = require("clean-webpack-plugin");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const WebpackCleanupPlugin = require('webpack-cleanup-plugin');
const CompressionPlugin = require("compression-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const GitRevisionPlugin = require("git-revision-webpack-plugin");
const WebpackBundleAnalzyer = require("webpack-bundle-analyzer").BundleAnalyzerPlugin;
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');

// config helpers:
const ensureArray = (config) => config && (Array.isArray(config) ? config : [config]) || [];
const when = (condition, config, negativeConfig) =>
  condition ? ensureArray(config) : ensureArray(negativeConfig);

const gitRevisionPlugin = new GitRevisionPlugin();

const currentDateTime = new Date();
const currentDate = currentDateTime.toLocaleDateString("en-GB").replace(/\//g, "-");
const currentTime = currentDateTime.toLocaleTimeString("en-GB", {
  hour12: false
}).replace(/:/g, "-");
const fileDateTime = currentDate + "-" + currentTime;


// primary config:
const title = 'eShopOnContainers';
const outDir = path.resolve(__dirname, 'dist');
const srcDir = path.resolve(__dirname, 'src');
const staticDir = path.resolve(__dirname, 'static');
const nodeModulesDir = path.resolve(__dirname, 'node_modules');
const baseUrl = '/';


module.exports = ({
  production
} = {}) => ({
  context: srcDir,
  entry: {
    app: [
      ...when(!production, [
        'webpack-hud',
      ]),
      './index.ts'
    ]
  },
  output: {
    path: outDir,
    publicPath: baseUrl,
    filename: production ? '[name].[chunkhash].bundle.js' : '[name].[hash].bundle.js',
    sourceMapFilename: production ? '[name].[chunkhash].bundle.map' : '[name].[hash].bundle.map',
    chunkFilename: production ? '[name].[chunkhash].chunk.js' : '[name].[hash].chunk.js'
  },
  target: 'web',
  // disable certain nodejs polyfills (larger bundle size)
  node: {
    console: !production,
    global: true,
    process: false,
    __filename: "mock",
    __dirname: "mock",
    Buffer: false,
    setImmediate: true,
    // workaround for webpack-dev-server issue
    // https://github.com/webpack/webpack-dev-server/issues/60#issuecomment-103411179
    fs: 'empty',
    net: 'empty'
  },
  resolve: {
    extensions: ['.js', '.ts', '.tsx'],
    // Fix webpack's default behavior to not load packages with jsnext:main module
    // (jsnext:main directs not usually distributable es6 format, but es6 sources)
    mainFields: ['module', 'browser', 'main'],
    alias: {
      'app': path.resolve(__dirname, 'src/app/')
    }
  },
  devtool: !production ? 'cheap-module-eval-source-map' : 'nosources-source-map',
  module: {
    rules: [{
        enforce: "pre",
        test: /\.tsx?$/,
        use: "source-map-loader"
      },
      {
        enforce: "pre",
        test: /\.js$/,
        use: "source-map-loader",
        "exclude": [
          // instead of /\/node_modules\//
          path.join(process.cwd(), 'node_modules')
        ]
      },
      {
        test: /\.tsx?$/,
        enforce: 'pre',
        loader: 'tslint-loader',
        options: {
          fix: true
        }
      },
      // CSS required in JS/TS files should use the style-loader that auto-injects it into the website
      // only when the issuer is a .js/.ts file, so the loaders are not applied inside html templates
      {
        test: /\.css$/,
        use: [
          MiniCssExtractPlugin.loader,
          {
            loader: 'css-loader',
            query: {
              sourceMap: !production,
              minimize: production,
            }
          }
        ],
      },
      // .ts, .tsx
      {
        test: /\.tsx?$/,
        exclude: /(node_modules|test-utils|\.test\.ts$)/,
        use: ['awesome-typescript-loader']
      },

      {
        test: /\.html$/i,
        loader: 'html-loader'
      },
      {
        test: /\.json$/i,
        loader: 'json-loader'
      },
      // use Bluebird as the global Promise implementation:
      {
        test: /[\/\\]node_modules[\/\\]bluebird[\/\\].+\.js$/,
        loader: 'expose-loader?Promise'
      },
      // embed small images and fonts as Data Urls and larger ones as files:
      {
        test: /\.(png|gif|jpg|cur)$/i,
        loader: 'url-loader',
        options: {
          limit: 8192
        }
      },
      {
        test: /\.woff2(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
        loader: 'url-loader',
        options: {
          limit: 10000,
          mimetype: 'application/font-woff2'
        }
      },
      {
        test: /\.woff(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
        loader: 'url-loader',
        options: {
          limit: 10000,
          mimetype: 'application/font-woff'
        }
      },
      // load these fonts normally, as files:
      {
        test: /\.(ttf|eot|svg|otf)(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
        loader: 'file-loader'
      }
    ]
  },
  optimization: {
    splitChunks: {
      name: true,
      cacheGroups: {
        commons: {
          chunks: 'initial',
          minChunks: 2
        },
        vendors: {
          test: /[\\/]node_modules[\\/]/,
          chunks: 'all',
          priority: -10
        }
      }
    },
    runtimeChunk: true
  },
  plugins: [
    ...when(production, [
      new CleanPlugin([outDir]),
    ]),
    new webpack.DefinePlugin({
      __DEV__: !production,
      "process.env.NODE_ENV": JSON.stringify(process.env.NODE_ENV),
      "API_SERVER": JSON.stringify(process.env.API_SERVER),
      "VERSION": JSON.stringify(gitRevisionPlugin.version()),
      "COMMITHASH": JSON.stringify(gitRevisionPlugin.commithash()),
      'BRANCH': JSON.stringify(gitRevisionPlugin.branch()),
      "TIMESTAMP": JSON.stringify(fileDateTime),
      "DATE": JSON.stringify(currentDate),
      "TIME": JSON.stringify(currentTime)
    }),
    new webpack.ProvidePlugin({
      // babel issue workaround? https://github.com/s-panferov/awesome-typescript-loader/issues/169
      'regeneratorRuntime': 'regenerator-runtime/runtime',
      "Promise": "bluebird"
    }),
    new HtmlWebpackPlugin({
      template: staticDir + '/index.ejs',
      metadata: {
        // available in index.ejs //
        title,
        baseUrl
      }
    }),
    // ignore moment locales
    new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
    new WebpackBundleAnalzyer({
      analyzerMode: 'static',
      openAnalyzer: false
    }),
    new MiniCssExtractPlugin({
      filename: "[name].css",
      chunkFilename: "[id].css"
    }),
    ...when(!production, [
      new WebpackCleanupPlugin(),
      new webpack.NamedModulesPlugin(),
    ]),
    ...when(production, [
      new CopyWebpackPlugin([{
        from: staticDir + '/favicon.ico',
        to: outDir + '/favicon.ico'
      }]),
      new webpack.optimize.AggressiveMergingPlugin(),
      new UglifyJSPlugin({
        sourceMap: true,
        parallel: true,
        uglifyOptions: {
          compress: {
            drop_console: true
          },
          output: {
            comments: false
          }
        }
      }),
      new CompressionPlugin({
        asset: "[file].gz[query]",
        algorithm: "gzip",
        threshold: 10240,
        minRatio: 0.8
      }),
    ])
  ],
  devServer: {
    contentBase: srcDir,
    publicPath: baseUrl,
    hot: true,
    inline: true,
    open: true,
    historyApiFallback: true,
    stats: 'minimal',
    host: 'localhost',
    port: 9000
  }
});

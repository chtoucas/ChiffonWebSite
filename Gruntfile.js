
module.exports = function(grunt) {
  'use strict';

  var config = {
    baseDir: './src/Chiffon.WebSite/assets'
  };

  function rebase_js(src) { return config.baseDir + '/js/' + src; }
  function rebase_css(src) { return config.baseDir + '/css/' + src; }

  var chiffon_css = [
    'normalize-1.1.3.css',
    '01-chiffon.base.css',
    '02-chiffon.helpers.css',
    '03-chiffon.css'
  ].map(rebase_css);
  var chiffon_js = ['app.js', 'chiffon.js'].map(rebase_js);

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    concat: {
      options: { separator: ';' },
      app_js: {
        src: [
          'vendor/yepnope-1.5.4.js',
          'vendor/lodash.compat-2.0.0.js',
          'app.js'
        ].map(rebase_js),
        dest: '_tmp/app.js'
      },
      chiffon_js: {
        src: [
          'jquery.plugins.js',
          'vendor/l10n-2013.09.19.js',
          'localization.js',
          'chiffon.js'
        ].map(rebase_js),
        dest: '_tmp/chiffon.js'
      },
      chiffon_css: {
        src: chiffon_css,
        dest: '_tmp/chiffon.css'
      }
    },

    csslint: {
      chiffon: {
        options: { formatters: [ {id: 'text', dest: '_tmp/csslint.txt'} ] },
        src: chiffon_css
      }
    },

    cssmin: {
      chiffon: {
        files: { '_tmp/chiffon.min.css': [ '_tmp/chiffon.css' ] }
      }
    },

    jshint: {
      files: chiffon_js
    },

    jslint: {
      chiffon: {
        src: chiffon_js,
        options: {
          log: '_tmp/jslint.log',
          errorsOnly: true,
          failOnError: false
        }
      }
    },

    uglify: {
      options: {
        compress: true,
        mangle: true
      },
      chiffon: {
        files: {
          '_tmp/app.min.js': ['_tmp/app.js'],
          '_tmp/chiffon.min.js': ['_tmp/chiffon.js']
        }
      }
    }
  });

  grunt.loadNpmTasks('grunt-verbosity');
  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('test', ['jshint', 'jslint', 'csslint']);
  grunt.registerTask('default', ['concat', 'uglify', 'cssmin']);
};

module.exports = function(grunt) {
  'use strict';

  var config = {
    projectDir: __dirname + '/src/Chiffon.WebSite/assets'
    , reportsDir: __dirname + '/_work/reports'
  };

  function jspath(src) { return config.projectDir + '/js/' + src; }
  function csspath(src) { return config.projectDir + '/css/' + src; }
  function logpath(src) { return config.reportsDir + '/' + src; }

  function readVersion() {
    var fs = require('fs')
      , parseString = require('xml2js').parseString
      , version;

    var xml = grunt.file.read(__dirname + '/etc/VersionInfo.xml');
    parseString(xml, function(err, result) {
      var versionNumber = result.VersionNumber;
      version = versionNumber.Major + '.' + versionNumber.Minor + '.' + versionNumber.Build;
    });

    return version;
  }

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json')

    , version: readVersion()

    , _filesToLint: {
      // Fichiers CSS à analyser.
      css: [
        '01-chiffon.base.css'
        , '02-chiffon.helpers.css'
        , '03-chiffon.css'
      ].map(csspath)
      // Fichiers JavaScript à analyser.
      , js: ['app.js', 'chiffon.js'].map(jspath)
    }

    , _bundles: {
      // CSS bundles.
      css: {
        // Screen CSS bundle.
        screen: {
          src: [
            'normalize-1.1.3.css'
            , '01-chiffon.base.css'
            , '02-chiffon.helpers.css'
            , '03-chiffon.css'
          ].map(csspath)
          , dest: csspath('chiffon-<%= version %>.min.css')
        }
      }
      // JavaScript bundles.
      , js: {
        // Application JavaScript bundle.
        app: {
          src: [
            'vendor/yepnope-1.5.4.js'
            , 'vendor/lodash.compat-2.0.0.js'
            , 'app.js'
          ].map(jspath)
          , map: [
          ]
          , dest: jspath('app-<%= version %>.min.js')
        }
        // Chiffon JavaScript bundle.
        , chiffon: {
          src: [
            'jquery.plugins.js'
            , 'vendor/l10n-2013.09.19.js'
            , 'localization.js'
            , 'chiffon.js'
          ].map(jspath)
          , dest: jspath('chiffon-<%= version %>.min.js')
        }
      }
    }

    /*
     * Analyse des fichiers CSS via CSSLint.
     */
    , csslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: { formatters: [ {id: 'text', dest: logpath('csslint.log') } ] }
        , src: '<%= _filesToLint.css %>'
      }
    }

    /*
     * Minification des fichiers JavaScript via Clean-CSS.
     */
    , cssmin: {
      chiffon: {
        options: {
          banner: '/*! Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */'
          , keepSpecialComments: 0
          , report: 'min'
        }
        , files: { '<%= _bundles.css.screen.dest %>': '<%= _bundles.css.screen.src %>' }
      }
    }

    /*
     * Analyse des fichiers JavaScript via JSLint.
     */
    , jshint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      files: '<%= _filesToLint.js %>'
    }

    /*
     * Analyse des fichiers JavaScript via JSHint.
     */
    , jslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: {
          log: logpath('jslint.log')
          , errorsOnly: true
          , failOnError: false
        }
        , src: '<%= _filesToLint.js %>'
      }
    }

    /*
     * Minification des fichiers JavaScript via UglifyJS.
     */
    , uglify: {
      options: {
        //banner: '/*! Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */\n'
        compress: { unused: false }
        , mangle: true
      }
      , app: {
        options: {
          sourceMap: jspath('app-<%= version %>.min.map')
          , sourceMappingURL: 'app-<%= version %>.min.map'
          , sourceMapPrefix: 7
        }
        , files: { '<%= _bundles.js.app.dest %>' : '<%= _bundles.js.app.src %>' }
      }
      , chiffon: {
        options: {
          sourceMap: jspath('chiffon-<%= version %>.min.map')
          , sourceMappingURL: 'chiffon-<%= version %>.min.map'
          , sourceMapPrefix: 7
        }
        , files: { '<%= _bundles.js.chiffon.dest %>': '<%= _bundles.js.chiffon.src %>' }
      }
    }
  });

  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('analyze', ['jshint', 'jslint', 'csslint']);
  grunt.registerTask('minify', ['uglify', 'cssmin']);
  grunt.registerTask('default', ['minify']);
};

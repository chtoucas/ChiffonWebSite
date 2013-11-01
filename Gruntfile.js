/*jshint laxcomma: true*/
/*jslint white: true, todo: true*/

module.exports = function (grunt) {
  'use strict';

  var config = {
    projectDir: __dirname + '/src/Chiffon.WebSite/assets'
    , reportsDir: __dirname + '/_work/reports'
  };

  function mapCss(value) { return config.projectDir + '/css/' + value; }
  function mapJs(value) { return config.projectDir + '/js/' + value; }
  function mapLog(value) { return config.reportsDir + '/' + value; }

  function readVersion() {
    var parseString = require('xml2js').parseString
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

    , filesToLint: {
      // Fichiers CSS à analyser.
      css: [
        '01-chiffon.base.css'
        , '02-chiffon.helpers.css'
        , '03-chiffon.css'
      ].map(mapCss)
      // Fichiers JavaScript à analyser.
      // TODO: ajouter Gruntfile.js
      , js: ['app.js', 'chiffon.js'].map(mapJs).concat('Gruntfile.js')
    }

    , bundles: {
      // CSS bundles.
      css: {
        // Screen CSS bundle.
        screen: {
          src: [
            'normalize-2.1.3.css'
            , '01-chiffon.base.css'
            , '02-chiffon.helpers.css'
            , '03-chiffon.css'
          ].map(mapCss)
          , dest: mapCss('chiffon-<%= version %>.min.css')
        }
      }
      // JavaScript bundles.
      , js: {
        // Application JavaScript bundle.
        app: {
          src: [
            'vendor/yepnope-1.5.4.js'
            , 'vendor/lodash.compat-2.2.1.js'
            , 'app.js'
          ].map(mapJs)
          , dest: mapJs('app-<%= version %>.min.js')
        }
        // Chiffon JavaScript bundle.
        , chiffon: {
          src: [
            'jquery.plugins.js'
            , 'vendor/l10n-2013.09.12.js'
            , 'localization.js'
            , 'chiffon.js'
          ].map(mapJs)
          , dest: mapJs('chiffon-<%= version %>.min.js')
        }
      }
    }

    /*
     * Analyse des fichiers CSS via CSSLint.
     */
    , csslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: { formatters: [ {id: 'text', dest: mapLog('csslint.log')} ] }
        , src: '<%= filesToLint.css %>'
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
        , files: { '<%= bundles.css.screen.dest %>': '<%= bundles.css.screen.src %>' }
      }
    }

    /*
     * Analyse des fichiers JavaScript via JSLint.
     */
    , jshint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      files: '<%= filesToLint.js %>'
    }

    /*
     * Analyse des fichiers JavaScript via JSHint.
     */
    , jslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: {
          log: mapLog('jslint.log')
          , errorsOnly: true
          , failOnError: false
        }
        , src: '<%= filesToLint.js %>'
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
          sourceMap: mapJs('app-<%= version %>.min.map')
          , sourceMappingURL: 'app-<%= version %>.min.map'
          , sourceMapPrefix: 8
        }
        , files: { '<%= bundles.js.app.dest %>' : '<%= bundles.js.app.src %>' }
      }
      , chiffon: {
        options: {
          sourceMap: mapJs('chiffon-<%= version %>.min.map')
          , sourceMappingURL: 'chiffon-<%= version %>.min.map'
          , sourceMapPrefix: 8
        }
        , files: { '<%= bundles.js.chiffon.dest %>': '<%= bundles.js.chiffon.src %>' }
      }
    }
  });

  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('analyze', ['jshint', 'jslint', 'csslint']);
  grunt.registerTask('build', ['uglify', 'cssmin']);
  grunt.registerTask('default', ['build']);
};

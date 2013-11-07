/*jshint laxcomma: true*/
/*jslint white: true, todo: true*/

//if (typeof exports !== 'undefined') {
//  exports.BundlerFactory = BundlerFactory;
//}

module.exports = function(grunt) {
  'use strict';

  var config = {
    projectDir: __dirname + '/src/Chiffon.WebSite/assets'
    , reportsDir: __dirname + '/_work/reports'
  };

  function mapCss(value) { return config.projectDir + '/css/' + value; }
  function mapJs(value) { return config.projectDir + '/js/' + value; }
  function mapLog(value) { return config.reportsDir + '/' + value; }

  function readSemVer() {
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

    , version: readSemVer()

    , filesToLint: {
      // Fichiers CSS à analyser.
      css: [
        '01-chiffon.base.css'
        , '02-chiffon.helpers.css'
        , '03-chiffon.css'
      ].map(mapCss)
      // Fichiers JavaScript à analyser.
      , js: ['jquery.plugins.js', 'boot.js', 'chiffon.js'].map(mapJs).concat('Gruntfile.js')
    }

    , bundles: {
      // Groupes CSS.
      css: {
        // Groupe CSS pour les écrans.
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
      // Groupes JavaScript.
      , js: {
        // Groupe JavaScript de démarrage.
        boot: {
          src: ['vendor/yepnope-1.5.4.js', 'boot.js'].map(mapJs)
          , dest: mapJs('boot-<%= version %>.min.js')
        }
        // Groupe JavaScript chargé en différé.
        , chiffon: {
          src: [
            'vendor/l10n-2013.09.12.js'
            , 'jquery.plugins.js'
            , 'jquery.modal.js'
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
        // TODO: On ne peut pas combiner les options banner et minification.
        //banner: '/*! Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */\n'
        compress: { unused: false }
        , mangle: true
      }
      , boot: {
        options: {
          sourceMap: mapJs('boot-<%= version %>.min.map')
          , sourceMappingURL: 'boot-<%= version %>.min.map'
          , sourceMapPrefix: 8
        }
        , files: { '<%= bundles.js.boot.dest %>' : '<%= bundles.js.boot.src %>' }
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

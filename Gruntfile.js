/*jshint node:true*/
/*jslint node:true*/

//if (typeof exports !== 'undefined') {
//  exports.BundlerFactory = BundlerFactory;
//}

module.exports = function(grunt) {
  'use strict';

  var config = {
    projectDir: './src/Chiffon.WebSite/assets'
    , reportsDir: __dirname + '/_work/reports'
  };

  function mapCss(value) { return config.projectDir + '/css/' + value; }
  function mapJs(value) { return config.projectDir + '/js/' + value; }
  function mapLog(value) { return config.reportsDir + '/' + value; }

  function readSemVer() {
    var semVer
      , parseString = require('xml2js').parseString
      , xml = grunt.file.read(__dirname + '/etc/VersionInfo.xml');

    parseString(xml, function(err, result) {
      var versionNumber = result.VersionNumber;
      semVer = versionNumber.Major + '.' + versionNumber.Minor + '.' + versionNumber.Build;
    });

    return semVer;
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
      , js: [
        'jquery.plugins.js'
        , 'chiffon.es5.js'
        , 'chiffon.boot.js'
        , 'chiffon.localization.js'
        , 'chiffon.js'
      ].map(mapJs).concat('Gruntfile.js')
      , jslint: [
        'jquery.plugins.js'
        , 'chiffon.boot.js'
        , 'chiffon.localization.js'
        , 'chiffon.js'
      ].map(mapJs).concat('Gruntfile.js')
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
          , dest: mapCss('_screen-<%= version %>.css')
        }
      }
      // Groupes JavaScript.
      , js: {
        // Groupe JavaScript de démarrage.
        chiffon_boot: {
          src: ['vendor/yepnope-1.5.4.js', 'chiffon.es5.js', 'chiffon.boot.js'].map(mapJs)
          , dest: mapJs('_boot-<%= version %>.js')
          , srcmap: '_boot-<%= version %>.map'
        }
        // Groupe JavaScript des librairies externes.
        , libraries_modern: {
          src: ['vendor/jquery-2.0.3.min.js', 'vendor/lodash-2.2.1.min.js'].map(mapJs)
          , dest: mapJs('_lib-<%= version %>.js')
        }
        , libraries_compat: {
          src: ['vendor/jquery-1.10.2.min.js', 'vendor/lodash.compat-2.2.1.min.js'].map(mapJs)
          , dest: mapJs('_lib.compat-<%= version %>.js')
        }
        // Groupe JavaScript chargé en différé.
        , chiffon_core: {
          src: [
            'vendor/l10n-2013.09.12.js'
            , 'jquery.plugins.js'
            , 'jquery.modal.js'
            , 'chiffon.localization.js'
            , 'chiffon.js'
          ].map(mapJs)
          , dest: mapJs('_core-<%= version %>.js')
          , srcmap: '_core-<%= version %>.map'
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
          //banner: '/*! Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */'
          keepSpecialComments: 0
          , report: 'min'
        }
        , files: { '<%= bundles.css.screen.dest %>': '<%= bundles.css.screen.src %>' }
      }
    }

    /*
     * Analyse des fichiers JavaScript via JSHint.
     */
    , jshint: {
      options: {
        // Règles strictes.
        bitwise: true             // À dséactiver si besoin est.
        //, camelcase: true       // Ne peut pas la différence entre fonctions & variables "pures".
        , curly: true
        , eqeqeq: true
        , es3: true
        , forin: true
        , freeze: true
        , immed: true
        //, indent: 2             // Incompatible avec laxcomma et laxbreak.
        , latedef: true
        //, maxcomplexity: true
        , maxdepth: 2
        //, maxlen:
        , maxparams: 3
        //, maxstatements: 10
        , newcap: true
        , noarg: true
        , noempty: true
        , nonew: true
        //, plusplus: true
        , quotmark: 'single'
        , strict: true
        , trailing: true
        , undef: true
        , unused: true

        // Règles désactivées.
        , laxcomma: true
        , laxbreak:true

        // Environnement
        , browser:true
      }
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      , files: '<%= filesToLint.js %>'
    }

    /*
     * Analyse des fichiers JavaScript via JSLint.
     */
    , jslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: {
          log: mapLog('jslint.log')
          , errorsOnly: true
          , failOnError: false
        }
        , directives: {
          nomen: true
          , white: true
          , todo: true
          , plusplus: true
          , browser: true
        }
        , src: '<%= filesToLint.jslint %>'
      }
    }

    /*
     * Fusion des fichiers JavaScript.
     */
    , concat: {
      options: {
        stripBanners: true
        , block: true
        , line: true
      }
      , libraries_modern: {
        src: '<%= bundles.js.libraries_modern.src %>'
        , dest: '<%= bundles.js.libraries_modern.dest %>'
      }
      , libraries_compat: {
        src: '<%= bundles.js.libraries_compat.src %>'
        , dest: '<%= bundles.js.libraries_compat.dest %>'
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
        , sourceMapPrefix: 5
      }
      , chiffon_boot: {
        options: {
          preserveComments: false
          , sourceMap: mapJs('<%= bundles.js.chiffon_boot.srcmap %>')
          , sourceMappingURL: '<%= bundles.js.chiffon_boot.srcmap %>'
        }
        , files: {
          '<%= bundles.js.chiffon_boot.dest %>' : '<%= bundles.js.chiffon_boot.src %>'
        }
      }
      , chiffon_core: {
        options: {
          preserveComments: false
          , sourceMap: mapJs('<%= bundles.js.chiffon_core.srcmap %>')
          , sourceMappingURL: '<%= bundles.js.chiffon_core.srcmap %>'
        }
        , files: {
          '<%= bundles.js.chiffon_core.dest %>': '<%= bundles.js.chiffon_core.src %>'
        }
      }
    }
  });

  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('analyze', ['jshint', 'csslint']);
  grunt.registerTask('build', ['concat', 'uglify', 'cssmin']);
  grunt.registerTask('default', ['build']);
};

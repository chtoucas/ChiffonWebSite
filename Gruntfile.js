/*jshint node:true, indent:false*/
/*jslint node:true*/

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
    var semVer;
    var parseString = require('xml2js').parseString;
    var xml = grunt.file.read(__dirname + '/etc/VersionInfo.xml');

    parseString(xml, function(err, result) {
      var versionNumber = result.VersionNumber;
      semVer = versionNumber.Major + '.' + versionNumber.Minor + '.' + versionNumber.Build;
    });

    return semVer;
  }

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json')

    , version: readSemVer()

    , sources: {
      // Fichiers CSS à analyser.
      css: [
        '01-chiffon.base.css'
        , '02-chiffon.helpers.css'
        , '03-chiffon.css'
      ].map(mapCss)
      // Fichiers JavaScript à analyser.
      , js: [
        'chiffon.jquery.js'
        , 'chiffon.main.js'
        , 'chiffon.localization.js'
        , 'chiffon.core.js'
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
        chiffon_main: {
          src: ['vendor/yepnope-1.5.4.js', 'chiffon.main.js'].map(mapJs)
          , dest: mapJs('_main-<%= version %>.js')
          , srcmap: '_main-<%= version %>.map'
        }
        // Groupe JavaScript des librairies externes.
        , libraries: {
          src: ['vendor/jquery-2.0.3.min.js', 'vendor/lodash-2.2.1.min.js'].map(mapJs)
          , dest: mapJs('_lib-<%= version %>.js')
        }
        // Groupe JavaScript chargé en différé.
        , chiffon_core: {
          src: [
            'vendor/l10n-2013.09.12.js'
            , 'jquery.modal.js'
            , 'chiffon.jquery.js'
            , 'chiffon.localization.js'
            , 'chiffon.core.js'
          ].map(mapJs)
          , dest: mapJs('_core-<%= version %>.js')
          , srcmap: '_core-<%= version %>.map'
        }
      }
    }

    , nobom: {
      js: { src: '<%= sources.js %>' }
      , css: { src: '<%= sources.css %>' }
    }

    /*
     * Analyse des fichiers CSS via CSSLint.
     */
    , csslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: { formatters: [ {id: 'text', dest: mapLog('csslint.log')} ] }
        , src: '<%= sources.css %>'
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
        bitwise: true             // Peut être désactivé localement.
        //, camelcase: true
        , curly: true
        , eqeqeq: true
        , es3: true
        , forin: true
        , freeze: true
        , immed: true
        , indent: 2             // Incompatible avec laxcomma et laxbreak ?
        , latedef: true
        //, maxcomplexity: true
        , maxdepth: 2
        //, maxlen: 1000
        , maxparams: 3            // Peut être désactivé localement.
        //, maxstatements: 10
        , newcap: true
        , noarg: true
        , noempty: true
        , nonew: true
        //, plusplus: true        // Aucune raison de ne pas utiliser les opérateurs ++ et --.
        , quotmark: 'single'
        , strict: true
        , trailing: true
        , undef: true
        , unused: true

        // Règles désactivées.
        , laxcomma: true
        , laxbreak:true

        // Environnement
        //, browser:true
      }
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      , files: '<%= sources.js %>'
    }

    /*
     * Analyse des fichiers JavaScript via JSLint.
     */
    /* , jslint: {
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
        , src: '<%= sources.js %>'
      }
    } */

    /*
     * Fusion des fichiers JavaScript.
     */
    , concat: {
      options: {
        stripBanners: true
        , block: true
        , line: true
      }
      , libraries: {
        src: '<%= bundles.js.libraries.src %>'
        , dest: '<%= bundles.js.libraries.dest %>'
      }
    }

    /*
     * Minification des fichiers JavaScript via UglifyJS.
     */
    , uglify: {
      options: {
        // TODO: On ne peut pas combiner les options banner et minification.
        //banner: '/*! Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */\n'
        // TODO: Vérifier les options.
        compress: { unused: true }
        , mangle: true
        , sourceMapPrefix: 5
      }
      , chiffon_main: {
        options: {
          preserveComments: false
          , sourceMap: mapJs('<%= bundles.js.chiffon_main.srcmap %>')
          , sourceMappingURL: '<%= bundles.js.chiffon_main.srcmap %>'
        }
        , files: {
          '<%= bundles.js.chiffon_main.dest %>' : '<%= bundles.js.chiffon_main.src %>'
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

  // Emprunté à https://github.com/david-driscoll/grunt-bom/
  // Cf. https://github.com/zandroid/grunt-bom-removal/blob/master/tasks/bom.js
  grunt.registerMultiTask('nobom', 'byte order mark remove files.', function() {
    this.filesSrc.forEach(function(filepath) {
      if (!grunt.file.exists(filepath)) {
        grunt.log.error('Source file "' + filepath + '" not found.');
        return;
      }

      var buf = grunt.file.read(filepath, { encoding: null });
      if (buf[0] === 0xEF && buf[1] === 0xBB && buf[2] === 0xBF) {
        buf = buf.slice(3);
        grunt.file.write(filepath, buf);
        grunt.log.writeln('File "' + filepath + '" rewritten.');
      }

      /* var content = grunt.file.read(filepath);
      if (/^\uFEFF/.test(content)) {
        content = content.replace(/^\uFEFF/, '');
        grunt.file.write(filepath, content);
        grunt.log.writeln('File "' + filepath + '" rewritten.');
      } */
    });

    if (this.errorCount) { return false; }
  });

  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('lint', ['jshint', 'csslint']);
  grunt.registerTask('build', ['concat', 'uglify', 'cssmin']);
  grunt.registerTask('default', ['lint', 'build']);
};

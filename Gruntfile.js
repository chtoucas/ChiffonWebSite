/*jshint node:true*/
/*jslint node:true*/

// TODO: tests !

module.exports = function(grunt) {
  'use strict';

  var config = {
    // Utiliser un chemin relatif pour les sourcemaps.
    projectDir: './src/Chiffon.WebSite/assets',
    reportsDir: __dirname + '/_work/reports',
    tmpDir: __dirname + '/_work/tmp'
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
    pkg: grunt.file.readJSON('package.json'),

    version: readSemVer(),

    sources: {
      // Fichiers CSS à analyser.
      css: [
        '01-chiffon.base.css',
        '02-chiffon.helpers.css',
        '03-chiffon.css'
      ].map(mapCss),
      // Fichiers JavaScript à analyser.
      js: [
        'chiffon.jquery.js',
        'chiffon.js',
        'chiffon.localization.js',
        'chiffon.core.js'
      ].map(mapJs).concat('Gruntfile.js')
    },

    // WARNING: Conserver le même ordre que celui utilisé par le site web.
    bundles: {
      // Groupes CSS.
      css: {
        screen: {
          src: [
            'normalize-1.1.3.css',
            '01-chiffon.base.css',
            '02-chiffon.helpers.css',
            '03-chiffon.css'
          ].map(mapCss),
          dest: mapCss('_screen-<%= version %>.css')
        }
      },
      // Groupes JavaScript.
      js: {
        main: {
          src: [
            'vendor/yepnope-1.5.4.js',
            'vendor/lodash.custom-<%= pkg.devDependencies["lodash-cli"].replace("~", "") %>.js',
            'chiffon.js'
          ].map(mapJs),
          dest: mapJs('_main-<%= version %>.js'),
          srcmap: '_main-<%= version %>.map'
        },
        //lib: {
        //  src: ['vendor/jquery-2.0.3.min.js'].map(mapJs),
        //  dest: mapJs('_lib-<%= version %>.js')
        //},
        core: {
          src: [
            'vendor/l10n-2013.09.12.js',
            'jquery.modal.js',
            'chiffon.jquery.js',
            'chiffon.localization.js',
            'chiffon.core.js'
          ].map(mapJs),
          dest: mapJs('_core-<%= version %>.js'),
          srcmap: '_core-<%= version %>.map'
        }
      }
    },

    // NB: Désactivé pour le moment car Visual Studio ne gère pas les fichiers UTF8 sans BOM.
    //nobom: {
    //  js: { src: '<%= sources.js %>' },
    //  css: { src: '<%= sources.css %>' }
    //},

    /*
     * Analyse des fichiers CSS via CSSLint.
     */
    csslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: { formatters: [ {id: 'text', dest: mapLog('csslint.log')} ] },
        src: '<%= sources.css %>'
      }
    },

    /*
     * Minification des fichiers JavaScript via Clean-CSS.
     */
    cssmin: {
      chiffon: {
        options: {
          banner: '/*! Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */',
          keepSpecialComments: 0,
          report: 'min'
        },
        files: { '<%= bundles.css.screen.dest %>': '<%= bundles.css.screen.src %>' }
      }
    },

    /*
     * Analyse des fichiers JavaScript via JSHint.
     */
    jshint: {
      options: {
        // Règles strictes.
        bitwise: true,      // On peut désactiver cette directive localement.
        camelcase: true,
        curly: true,
        eqeqeq: true,
        es3: true,
        forin: true,
        freeze: true,
        immed: true,
        indent: 2,          // WARNING: Cette directive semble incompatible avec laxcomma et laxbreak.
        latedef: true,
        maxcomplexity: 5,   // On peut ajuster ce chiffre localement.
        maxdepth: 2,
        maxlen: 700,        // Ajuster ce chiffre.
        maxparams: 3,       // On peut ajuster ce chiffre localement.
        maxstatements: 20,  // Ajuster ce chiffre.
        newcap: true,
        noarg: true,
        noempty: true,
        nonew: true,
        //plusplus: true,
        quotmark: 'single',
        strict: true,
        trailing: true,
        undef: true,
        unused: true,

        // Règles désactivées.
        //laxcomma: true,
        //laxbreak: true,
        //browser: true

        globals: { DEBUG: true, VERSION: true, console: true }
      },
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      files: '<%= sources.js %>'
    },

    /*
     * Analyse des fichiers JavaScript via JSLint.
     * Désactivé car jslint ne permet pas d'exclure deux règles problématiques :
     * - tous les fichiers sont sauvegardés en UTF8/BOM par Visual Studio ;
     * - on préfère utiliser des déclarations de variables séparées
     *   NB: UglifyJS s'occupe de ça automatiquement.
     */
    //jslint: {
    //  chiffon: {
    //    options: {
    //      log: mapLog('jslint.log'),
    //      errorsOnly: true,
    //      failOnError: false
    //    },
    //    directives: {
    //      nomen: true,
    //      white: true,
    //      todo: true,
    //      plusplus: true,
    //      browser: true
    //    },
    //    src: '<%= sources.js %>'
    //  }
    //},

    lodash: {
      custom: {
        dest: mapJs('vendor/lodash.custom-<%= pkg.devDependencies["lodash-cli"].replace("~", "") %>.js'),
        options: {
          modifier: 'modern',
          include: ['defaults', 'extend', 'debounce'],
          exports: ['none']
        }
      }
    },

    /*
     * Fusion des fichiers JavaScript.
     */
    //concat: {
    //  options: {
    //    stripBanners: true
    //  },
    //  lib: {
    //    src: '<%= bundles.js.lib.src %>',
    //    dest: '<%= bundles.js.lib.dest %>'
    //  }
    //},

    /*
     * Minification des fichiers JavaScript via UglifyJS.
     */
    uglify: {
      options: {
        // TODO: Vérifier les options.
        /*jshint -W106*/
        compress: {
          unused: true,
          dead_code: true,
          global_defs: {
            DEBUG: false,
            VERSION: '<%= version %>'
          }
        },
        /*jshint +W106*/
        mangle: true,
        sourceMapPrefix: 5
      },
      main: {
        options: {
          banner: '// Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %>. Contains yepnope & lodash.',
          preserveComments: false,
          sourceMap: mapJs('<%= bundles.js.main.srcmap %>'),
          sourceMappingURL: '<%= bundles.js.main.srcmap %>'
        },
        files: {
          '<%= bundles.js.main.dest %>' : '<%= bundles.js.main.src %>'
        }
      },
      core: {
        options: {
          banner: '// Generated on <%= grunt.template.today("yyyy-mm-dd HH:mm") %>. Contains l10n & jquery.modal.',
          preserveComments: false,
          sourceMap: mapJs('<%= bundles.js.core.srcmap %>'),
          sourceMappingURL: '<%= bundles.js.core.srcmap %>'
        },
        files: {
          '<%= bundles.js.core.dest %>': '<%= bundles.js.core.src %>'
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
        // XXX: Je pense que la vérification du premier caractère suffit.
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

  grunt.loadNpmTasks('grunt-lodash');
  //grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-jslint');

  grunt.registerTask('lint', ['jshint', 'csslint']);
  grunt.registerTask('build', ['lodash', 'uglify', 'cssmin']);
  grunt.registerTask('default', ['lint', 'build']);
};

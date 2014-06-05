/*jshint node:true*/
/*jslint node:true*/

// TODO: tests !

/*jshint -W071*/
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
      css: ['chiffon.css', 'chiffon.print.css'].map(mapCss),
      // Fichiers JavaScript à analyser.
      js: [
        //'jquery.modal.js',
        'chiffon.jquery.js',
        'chiffon.js',
        //'chiffon.localization.js',
        'chiffon.views.js'
      ].map(mapJs).concat('Gruntfile.js')
    },

    lessSources: {
      main: {
        src: mapCss('chiffon.less'),
        dest: mapCss('chiffon.css')
      },
      print: {
        src: mapCss('chiffon.print.less'),
        dest: mapCss('chiffon.print.css')
      }
    },

    // WARNING: Conserver le même ordre que celui utilisé par le site web.
    bundles: {
      // Groupes CSS.
      css: {
        main: {
          src: [
            'normalize-2.1.3.css',
            'nprogress-0.1.2.css',
            'chiffon.h5bp.css',
            'chiffon.css',
            'chiffon.print.css'
          ].map(mapCss),
          dest: mapCss('_main-<%= version %>.css')
        }
      },
      // Groupes JavaScript.
      js: {
        main: {
          src: [
            'vendor/yepnope-1.5.4.js',
            'vendor/lodash.custom-<%= pkg.devDependencies["lodash-cli"].replace("~", "").replace("^", "") %>.js',
            'vendor/fastclick-1.0.2.js',
            'chiffon.js'
          ].map(mapJs),
          dest: mapJs('_main-<%= version %>.js'),
          srcmap: '_main-<%= version %>.map'
        },
        //lib: {
        //  src: ['vendor/jquery-2.1.1.min.js'].map(mapJs),
        //  dest: mapJs('_lib-<%= version %>.js')
        //},
        views: {
          src: [
            //'vendor/l10n-2014.05.02.js',
            'vendor/nprogress-0.1.2.js',
            //'jquery.modal.js',
            'chiffon.jquery.js',
            //'chiffon.localization.js',
            'chiffon.views.js'
          ].map(mapJs),
          dest: mapJs('_views-<%= version %>.js'),
          srcmap: '_views-<%= version %>.map'
        }
      }
    },

    // NB: Désactivé pour le moment car Visual Studio ne gère pas les fichiers UTF8 sans BOM.
    //nobom: {
    //  js: { src: '<%= sources.js %>' },
    //  css: { src: '<%= sources.css %>' }
    //},


    // Analyse des fichiers CSS via CSSLint.
    csslint: {
      // NB: Chaque fichier contient ses propres instructions d'analyse.
      chiffon: {
        options: { formatters: [ {id: 'text', dest: mapLog('csslint.log')} ] },
        src: '<%= sources.css %>'
      }
    },

    recess: {
      options: {
        noOverqualifying: false,
        noUnderscores: false
      },
      main: {
        src: mapCss('chiffon.less')
      },
      print: {
        src: mapCss('chiffon.print.less')
      }
    },

    // Minification des fichiers Css via Clean-CSS.
    cssmin: {
      main: {
        options: {
          banner: '/* Timestamp: <%= grunt.template.today("yyyy-mm-dd HH:mm") %> */',
          keepSpecialComments: 0,
          report: 'min'
        },
        files: {
          '<%= bundles.css.main.dest %>': '<%= bundles.css.main.src %>'
        }
      }
    },

    // Analyse des fichiers JavaScript via JSHint.
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
        maxlen: 500,        // Ce choix semble raisonnable.
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

    // Analyse des fichiers JavaScript via JSLint.
    // Désactivé car jslint ne permet pas d'exclure deux règles problématiques :
    // - tous les fichiers sont sauvegardés en UTF8/BOM par Visual Studio ;
    // - on préfère utiliser des déclarations de variables séparées
    //   NB: UglifyJS s'occupe de ça automatiquement.
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

    less: {
      // TODO: Les chemins vers les sources sont incorrects.
      options: {
        ieCompat: true,
        strictMath: true,
        strictUnits: true,
        sourceMap: true
      },
      main: {
        files: {
          '<%= lessSources.main.dest %>': '<%= lessSources.main.src %>',
          '<%= lessSources.print.dest %>': '<%= lessSources.print.src %>'
        }
      }
    },

    watch: {
      files: [
        'chiffon.vars.less',
        'chiffon.base.less',
        'chiffon.layout.less',
        'chiffon.utils.less',
        'chiffon.keyframes.less',
        'chiffon.modules.less',
        'chiffon.pages.less',
        'chiffon.responsive.less',
        'chiffon.less',
        'chiffon.print.less'
      ].map(mapCss),
      tasks: ['less']
    },

    lodash: {
      custom: {
        dest: mapJs('vendor/lodash.custom-<%= pkg.devDependencies["lodash-cli"].replace("~", "").replace("^", "") %>.js'),
        options: {
          modifier: 'modern',
          include: ['debounce', 'defaults', 'extend', 'rest'],
          exports: ['global'],
          flags: ['source-map']
        }
      }
    },

    // Fusion des fichiers JavaScript.
    //concat: {
    //  options: {
    //    stripBanners: true
    //  },
    //  lib: {
    //    src: '<%= bundles.js.lib.src %>',
    //    dest: '<%= bundles.js.lib.dest %>'
    //  }
    //},

    // Minification des fichiers JavaScript via UglifyJS.
    uglify: {
      options: {
        /*jshint -W106*/
        compress: {
          global_defs: {
            DEBUG: false,
            VERSION: '<%= version %>'
          }
        },
        /*jshint +W106*/
        sourceMapPrefix: 5
      },
      main: {
        options: {
          banner: '// Timestamp: <%= grunt.template.today("yyyy-mm-dd HH:mm") %>.',
          preserveComments: false,
          sourceMap: mapJs('<%= bundles.js.main.srcmap %>'),
          sourceMappingURL: '<%= bundles.js.main.srcmap %>'
        },
        files: {
          '<%= bundles.js.main.dest %>' : '<%= bundles.js.main.src %>'
        }
      },
      views: {
        options: {
          banner: '// Timestamp: <%= grunt.template.today("yyyy-mm-dd HH:mm") %>.',
          preserveComments: false,
          sourceMap: mapJs('<%= bundles.js.views.srcmap %>'),
          sourceMappingURL: '<%= bundles.js.views.srcmap %>'
        },
        files: {
          '<%= bundles.js.views.dest %>': '<%= bundles.js.views.src %>'
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

  //grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-less');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-lodash');
  grunt.loadNpmTasks('grunt-recess');

  grunt.registerTask('lintcss', ['recess', 'csslint']);
  grunt.registerTask('lintjs', ['jshint']);
  grunt.registerTask('lint', ['lintcss', 'lintjs']);

  grunt.registerTask('buildcss', ['less', 'cssmin']);
  grunt.registerTask('buildjs', ['lodash', 'uglify']);
  grunt.registerTask('build', ['buildcss', 'buildjs']);

  grunt.registerTask('default', ['lint', 'build']);
};
/*jshint +W071*/
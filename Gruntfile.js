
module.exports = function(grunt) {
  'use strict';

  var config = {
    projectDir: __dirname + '/src/Chiffon.WebSite/assets'
    , reportsDir: __dirname + '/_work/reports'
    , version: null
    , cssbundle: null
    , appjsbundle: null
    , chiffonjsbundle: null
  };

  function jspath(src) { return config.projectDir + '/js/' + src; }
  function csspath(src) { return config.projectDir + '/css/' + src; }
  function logpath(src) { return config.reportsDir + '/' + src; }

  var
    // CSS to lint.
    css_to_lint = [
      '01-chiffon.base.css'
      , '02-chiffon.helpers.css'
      , '03-chiffon.css'
    ].map(csspath)
    // CSS bundles.
    , chiffon_cssbundle = [
      'normalize-1.1.3.css'
      , '01-chiffon.base.css'
      , '02-chiffon.helpers.css'
      , '03-chiffon.css'
    ].map(csspath)
    // JavaScript to lint.
    , js_to_lint = ['app.js', 'chiffon.js'].map(jspath)
    // JavaScript bundles.
    , app_jsbundle = [
      'vendor/yepnope-1.5.4.js'
      , 'vendor/lodash.compat-2.0.0.js'
      , 'app.js'
    ].map(jspath)
    , chiffon_jsbundle = [
      'jquery.plugins.js'
      , 'vendor/l10n-2013.09.19.js'
      , 'localization.js'
      , 'chiffon.js'
    ].map(jspath)
  ;

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json')

    , concat: {
      options: { separator: ';' }
      , app_jsbundle: {
        src: app_jsbundle
        , dest: '_tmp/app.js'
      }
      , chiffon_jsbundle: {
        src: chiffon_jsbundle
        , dest: '_tmp/chiffon.js'
      }
      , chiffon_cssbundle: {
        src: chiffon_cssbundle
        , dest: '_tmp/chiffon.css'
      }
    }

    , csslint: {
      chiffon: {
        options: { formatters: [ {id: 'text', dest: logpath('csslint.log') } ] }
        , src: css_to_lint
      }
    }

    , cssmin: {
      chiffon: {
        options: {
          keepSpecialComments: 0
          , report: 'min'
        }
        , src: '_tmp/chiffon.css'
        , dest: '_tmp/chiffon.' + config.version + '.min.css'
        //, files: { '_tmp/chiffon.min.css': [ '_tmp/chiffon.css' ] }
      }
    }

    , jshint: {
      files: js_to_lint
    }

    , jslint: {
      chiffon: {
        src: js_to_lint
        , options: {
          log: logpath('jslint.log')
          , errorsOnly: true
          , failOnError: false
        }
      }
    }

    , uglify: {
      options: {
        compress: true
        , mangle: true
      }
      , chiffon: {
        files: {
          '_tmp/app.min.js' : ['_tmp/app.js']
          , '_tmp/chiffon.min.js': ['_tmp/chiffon.js']
        }
      }
    }
  });

  grunt.registerTask('version', 'Read the version.', function() {
    var fs = require('fs')
      , parseString = require('xml2js').parseString;

    var xml = grunt.file.read(__dirname + '/etc/VersionInfo.xml');
    parseString(xml, function(err, result) {
      var versionNumber = result.VersionNumber;
      config.version = versionNumber.Major + '.' + versionNumber.Minor + '.' + versionNumber.Build;

      config.cssbundle = '_tmp/chiffon.' + config.version + '.min.css';
      config.appjsbundle = '_tmp/app.' + config.version + '.min.js';
      config.chiffonjsbundle = '_tmp/chiffon.' + config.version + '.min.js';
    });
  });

  grunt.loadNpmTasks('grunt-verbosity');
  grunt.loadNpmTasks('grunt-jslint');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-csslint');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('analyze', ['jshint', 'jslint', 'csslint']);
  grunt.registerTask('default', ['version', 'concat', 'uglify', 'cssmin']);
};
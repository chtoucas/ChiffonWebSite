require 'fileutils'
require 'rake'

PKG_VERSION = '0.9.1'

PROJECT_DIR = File.expand_path(File.dirname(__FILE__))
WORK_DIR    = File.join(PROJECT_DIR, '_work')

ASSETS_DIR  = File.join(PROJECT_DIR, 'src/Chiffon.WebSite/assets')
CSS_DIR     = File.join(ASSETS_DIR, 'css')
JS_DIR      = File.join(ASSETS_DIR, 'js')

EXT_DIR     = File.join(PROJECT_DIR, 'libexec')
CLOSURE_DIR = File.join(EXT_DIR,     'closure')
CLOSURE_JAR = File.join(EXT_DIR,     'closure/compiler.jar')

task :default   => [:clean, 'js:build_boot']

task :clean do
  rmtree WORK_DIR
end

directory WORK_DIR

namespace :css do
  task :build => WORK_DIR do
    puts 'Building site.css'
    src = get_work_path 'site.css'
    target = get_css_path 'site.css'

    files = [get_css_path('normalize-1.1.2.css')] + Dir.glob(get_css_path '0*.css')
    concat_files files.sort, src

    yui_minify src, target
  end
end

namespace :js do
  task :build => [:build_main, :build_site]

  task :build_boot => WORK_DIR do
    puts 'Building boot.js'
    closure_compile get_js_path('boot.js'), get_work_path('boot.js')
  end

  task :build_main => WORK_DIR do
    puts 'Building main.js'
    files = [
      'debug/yepnope.js',
      'debug/main.js'
    ].map { |f| get_js_path f }
    src = get_work_path 'main.js'
    target = get_js_path 'main.js'

    concat_files files, src
    closure_compile src, target
    compress_file target
  end

  task :build_site => WORK_DIR do
    puts 'Building site.js'
    files = [
      'debug/jquery.cookie.js',
      'debug/narvalo.js',
      'debug/site.js'
    ].map { |f| get_js_path f }
    src = get_work_path 'site.js'
    tmp = get_work_path 'site.min.js'
    target = get_js_path 'site.js'

    concat_files files, src
    closure_compile src, tmp
    concat_files [get_js_path('debug/jquery.js'), tmp], target
    compress_file target
  end
end

def get_css_path(file)
  File.join(CSS_DIR, file)
end

def get_ext_path(file)
  File.join(EXT_DIR, file)
end

def get_js_path(file)
  File.join(JS_DIR, file)
end

def get_js_debug_path(file)
  File.join(JS_DEBUG_DIR, file)
end

def get_php_path(file)
  File.join(PHP_DIR, file)
end

def get_work_path(file)
  File.join(WORK_DIR, file)
end

def concat_files(files, target)
  File.open(target, 'w') do |f|
    f.puts files.map { |file|
      File.read file
    }
  end
end

def closure_compile(src, target)
  `java -jar #{CLOSURE_JAR} --compilation_level SIMPLE_OPTIMIZATIONS --js #{src} --js_output_file #{target}`
end

def yui_minify(src, target)
  `yuicompressor #{src} -o #{target} --charset utf-8 --type css`
end

def compress_file(file)
  target = "#{file}.gz"
  `gzip -c -f -9 #{file} > #{target}`
end


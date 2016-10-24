/// <binding />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

// Include Gulp
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var filter = require('gulp-filter');
var sourcemaps = require('gulp-sourcemaps');
var mainBowerFiles = require('main-bower-files');
var cleanCSS = require('gulp-clean-css');
var order = require('gulp-order');
var exclude = require('arr-exclude');
var print = require('gulp-print');

var config = {
    //Include all js files but exclude any min.js files
    js: ['app/**/*.js', '!app/**/*.min.js'],
    css: ['css/**/*.css', '!**/*.min.css']
}

gulp.task('watch',
    function () {
        gulp.watch('app/**/*.js', ['build-app:js']);
        gulp.watch('css/*.css', ['build-app:css']);

        gulp.watch('lib/**/*.js', ['build-vendor:js']);
        gulp.watch('lib/**/*.css', ['build-vendor:css']);
    });

gulp.task('default', function () {
    // place code for your default task here
});

gulp.task('build-app:js', function () {
    return gulp.src(config.js)
        .pipe(filter(['app/**/*.js']))
        .pipe(order([
            '**/app.module.js',
            '**/core/core.module.js',
            '**/core/config.js',
            '**/*.module.js'
        ]))
        .pipe(print())
        .pipe(sourcemaps.init())
        //.pipe(uglify())
        .pipe(concat('modules.min.js'))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('app/'));
});

gulp.task('build-app:css', function () {
    return gulp.src(config.css)
      .pipe(filter(['**/*.css']))
      .pipe(print())
      .pipe(sourcemaps.init())
      .pipe(cleanCSS())
      .pipe(concat('site.min.css'))
      .pipe(sourcemaps.write())
      .pipe(gulp.dest('css/'));
});

gulp.task('fonts', function () {
    return gulp.src(mainBowerFiles({
        // Only return the font files
        filter: /.*\.(eot|svg|ttf|woff|woff2)$/i
    }))
      .pipe(print())
      .pipe(gulp.dest('fonts'));
});

gulp.task('build-vendor:js', function () {
    return gulp.src(mainBowerFiles(['**/*.js']))
      .pipe(filter(['**/*.js']))
      //.pipe(filter(['**/*.js', '!*AdminLTE*']))
      .pipe(print())
      .pipe(sourcemaps.init())
      .pipe(uglify())
      .pipe(concat('vendor.min.js'))
      .pipe(sourcemaps.write())
      .pipe(gulp.dest('js/'));
});

gulp.task('build-vendor:css', function () {
    return gulp.src(mainBowerFiles())
      .pipe(filter(['**/*.css']))
      .pipe(print())
      .pipe(sourcemaps.init())
      .pipe(cleanCSS())
      .pipe(concat('vendor.min.css'))
      .pipe(sourcemaps.write())
      .pipe(gulp.dest('css/'));
});
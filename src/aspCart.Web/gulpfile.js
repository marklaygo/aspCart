"use strict";

var gulp = require("gulp");

var paths = {
    lib: './wwwroot/lib/',
    node: './node_modules/'
}

gulp.task("copy-dependencies", function () {
    // admin lte
    gulp.src(paths.node + 'admin-lte/dist/css/AdminLTE.css')
        .pipe(gulp.dest(paths.lib + 'adminlte/dist/css'));

    gulp.src(paths.node + 'admin-lte/dist/css/skins/skin-blue.css')
        .pipe(gulp.dest(paths.lib + 'adminlte/dist/css/skins'));

    gulp.src(paths.node + 'admin-lte/dist/img/credit/**/*')
        .pipe(gulp.dest(paths.lib + 'adminlte/dist/img/credit'));

    gulp.src(paths.node + 'admin-lte/dist/js/app.js')
        .pipe(gulp.dest(paths.lib + 'adminlte/dist/js'));

    // bootstrap
    gulp.src([
        paths.node + 'bootstrap/dist/**/*',
        '!' + paths.node + 'bootstrap/dist/**/*.map'
    ])
        .pipe(gulp.dest(paths.lib + 'bootstrap/dist'));

    // datatables
    gulp.src([
        paths.node + 'datatables/media/**/*',
        paths.node + 'datatables-bootstrap/**/*',
        '!' + paths.node + 'datatables-bootstrap/**/*.{txt,json,md}',
    ])
        .pipe(gulp.dest(paths.lib + 'datatables/dist'));

    // font-awesome
    gulp.src([
         paths.node + 'font-awesome/css/**/*',
        '!' + paths.node + 'font-awesome/css/**/*.map'
    ])
        .pipe(gulp.dest(paths.lib + 'font-awesome/css'));

    gulp.src(paths.node + 'font-awesome/fonts/**/*')
        .pipe(gulp.dest(paths.lib + 'font-awesome/fonts'));

    // icheck
    gulp.src([
        paths.node + 'icheck/skins/square/blue.css',
        paths.node + 'icheck/skins/square/blue.png',
        paths.node + 'icheck/skins/square/blue@2x.png'
    ])
        .pipe(gulp.dest(paths.lib + 'icheck/skins/square'));

    gulp.src([
        paths.node + 'icheck/icheck.js',
        paths.node + 'icheck/icheck.min.js',
    ])
        .pipe(gulp.dest(paths.lib + 'icheck'));

    // jquery
    gulp.src([
        paths.node + 'jquery/dist/**/*',
        '!' + paths.node + 'jquery/dist/**/*.map'
    ])
        .pipe(gulp.dest(paths.lib + 'jquery/dist'));

    // jquery validation
    gulp.src([
        paths.node + 'jquery-validation/dist/jquery.validate.js',
        paths.node + 'jquery-validation/dist/jquery.validate.min.js'
    ])
        .pipe(gulp.dest(paths.lib + 'jquery-validation/dist'));

    // jquery validation unobtrusive
    gulp.src(paths.node + 'jquery-validation-unobtrusive/jquery.validate.unobtrusive.js')
        .pipe(gulp.dest(paths.lib + 'jquery-validation-unobtrusive'));
});
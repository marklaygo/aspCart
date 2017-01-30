"use strict";

var gulp = require("gulp");

var paths = {
    lib: './wwwroot/lib/',
    node: './node_modules/'
}

gulp.task("copy-dependencies", function () {
    // bootstrap
    gulp.src([
        paths.node + 'bootstrap/dist/**/*',
        '!' + paths.node + 'bootstrap/dist/**/*.map'
    ])
        .pipe(gulp.dest(paths.lib + 'bootstrap/dist'));

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
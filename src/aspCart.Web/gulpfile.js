"use strict";

var gulp = require("gulp");

var paths = {
    lib: './wwwroot/lib/',
    node: './node_modules/'
}

gulp.task("copy-dependencies", function () {
    // vendor
    gulp.src('./vendor/**/*')
        .pipe(gulp.dest(paths.lib));

    // admin lte
    gulp.src(paths.node + 'admin-lte/dist/css/AdminLTE.min.css')
        .pipe(gulp.dest(paths.lib + 'adminlte/dist/css'));

    gulp.src(paths.node + 'admin-lte/dist/css/skins/skin-blue.min.css')
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

    // ckeditor
    gulp.src([
        paths.node + 'ckeditor/ckeditor.js',
        paths.node + 'ckeditor/contents.css',
        paths.node + 'ckeditor/styles.js'
    ])
        .pipe(gulp.dest(paths.lib + 'ckeditor'));

    gulp.src(paths.node + 'ckeditor/lang/en.js')
        .pipe(gulp.dest(paths.lib + 'ckeditor/lang'));

    gulp.src(paths.node + 'ckeditor/skins/moono-lisa/**/*')
        .pipe(gulp.dest(paths.lib + 'ckeditor/skins/moono-lisa'));

    gulp.src([
        paths.node + 'ckeditor/plugins/about/**/*',
        paths.node + 'ckeditor/plugins/image/**/*',
        paths.node + 'ckeditor/plugins/link/**/*',
        paths.node + 'ckeditor/plugins/icons.png',
        paths.node + 'ckeditor/plugins/icons_hidpi.png'
    ], { base: paths.node })
        .pipe(gulp.dest(paths.lib));

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

    // morris js
    gulp.src(paths.node + 'morris.js/morris.css')
        .pipe(gulp.dest(paths.lib + 'morris.js'));

    gulp.src(paths.node + 'morris.js/morris.min.js')
        .pipe(gulp.dest(paths.lib + 'morris.js'));

    // raphael
    gulp.src(paths.node + 'raphael/raphael.min.js')
        .pipe(gulp.dest(paths.lib + 'raphael'));
});

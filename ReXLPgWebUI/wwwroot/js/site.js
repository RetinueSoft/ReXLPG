// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function ($) {
    "use strict";

    // Dropdown on mouse hover
    $(document).ready(function () {

        //date-picker
        $('.input-group.date').datepicker({
            startView: 0,
            todayBtn: "linked",
            multidate: false,
            daysOfWeekHighlighted: "0",
            autoclose: true,
            todayHighlight: true
        });
    });
})(jQuery);


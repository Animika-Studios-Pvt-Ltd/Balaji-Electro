/* File Created: August 20, 2012 */
$(document).ready(function () {

    $(this).joyride({
        'tipLocation': 'bottom'
    });

    $("a:contains('Take a Tour')").click(function () {
        showOverlay();
    });

    $("a:contains('Close'),a:contains('Stop')").click(function () {
        hideOverlay();
    });

    $("div[id*='dec_show']").addClass("bModal");
    $("a[id*='show']").click(function () {
        $('#dec_' + $(this).attr('id')).bPopup();
    });

    $("#progress").bind("ajaxStart", function () {
        $(this).show();
    }).bind("ajaxStop", function () {
        $(this).hide();
    });

    $("div#tips_title").before('<div class="nav" id="pager1">').cycle({
        fx: 'fade',
        speed: 'slow',
        timeout: 0,
        pager: '#pager1'
    });

    $("div#tips_description").before('<div class="nav"  id="pager2">').cycle({
        fx: 'fade',
        speed: 'slow',
        timeout: 0,
        pager: '#pager2'
    });

    $("div#tips_keywords").before('<div class="nav"  id="pager3">').cycle({
        fx: 'fade',
        speed: 'slow',
        timeout: 0,
        pager: '#pager3'
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            $('.validation-summary-errors').remove();
        }
    });

    function showOverlay() {
        var $overlay = '<div id="tour_overlay" class="overlay"></div>';
        $('BODY').prepend($overlay);
    }

    function hideOverlay() {
        $('#tour_overlay').remove();
    }
});
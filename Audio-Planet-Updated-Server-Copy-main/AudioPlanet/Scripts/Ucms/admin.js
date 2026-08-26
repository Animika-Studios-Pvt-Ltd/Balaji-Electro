$(document).ready(function () {
    $('.menu').fixedMenu();

    $('.display').dataTable({
        "sPaginationType": "full_numbers",
        "bJQueryUI": true,
        "aLengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "aoColumnDefs": [{ "sWidth": "20%", "aTargets": [ -1 ]}]
    });

    // Toggle the dropdown menu's
    $(".dropdown .button, .dropdown button").click(function () {
        if (!$(this).find('span.toggle').hasClass('active')) {
            $('.dropdown-slider').slideUp();
            $('span.toggle').removeClass('active');
        }

        // open selected dropown
        $(this).parent().find('.dropdown-slider').slideToggle('fast');
        $(this).find('span.toggle').toggleClass('active');

        return false;
    });

    // Launch TipTip tooltip
    $('.tiptip a.button, .tiptip button').tipTip();

    $('#dec_show8').addClass("profile");
    $('a#show8').toggle(function () {
        var left = $('a#show8').offset().left;
        $('#dec_show8').bPopup({
            position: [left - 200, 60],
            modal: false
        });
    }, function () {
        $('#dec_show8').bPopup().close();
    });
});

// Close open dropdown slider by clicking elsewhwere on page
$(document).bind('click', function (e) {
    if (e.target.id != $('.dropdown').attr('class')) {
        $('.dropdown-slider').slideUp();
        $('span.toggle').removeClass('active');
    }
});
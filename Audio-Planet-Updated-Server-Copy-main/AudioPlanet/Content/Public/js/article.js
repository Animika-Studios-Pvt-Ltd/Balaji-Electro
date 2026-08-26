$(document).ready(function () {
    $("img").bind("contextmenu", function () {
        return false;
    });
    
    $(function () {
        $(this).ajaxStart(function () { $("#ajaxLoading").show(); });
        $(this).ajaxStop(function () { $("#ajaxLoading").hide(); });
    });

    if (!$("#SecondaryMenu:empty").length) {
        $("#SecondaryMenu_bg").hide();
    }

    $(function () {
        $(".Enquery").hide();
        $(".enqThumb").click(function () {
            $(".Enquery").toggle("slow");
            return false;
        });
        $(".contactUsPopUp").click(function () {
            $(".Enquery").toggle("slow");
        });
        $(".cancle").click(function () {
            $(".Enquery").hide();
            return false;
        });
    });

    $(function () {
        var divs = $("#LatestArticlesPanel li");
        if (divs.length > 4) {
            for (var i = 0; i < divs.length; i += 4) {
                divs.slice(i, i + 4).wrapAll("<ul></ul>");
            }
        }
        else {
            divs.wrapAll("<ul></ul>");
        }

        if (divs.length > 4) {
            $('#Listarticles').cycle({
                fx: 'fade',
                speed: 'fast',
                timeout: 0,
                pager: '.nav',
                next: '.next',
                prev: '.previous'
            });
            $(".Navigation").show();
        }

        if ($('ul.aCategories').length > 0) {
            var aCategories = $("ul.aCategories li");
            for (var j = 0; j < aCategories.length; j += 8) {
                aCategories.slice(j, j + 8).wrapAll('<li><ul  class="aCategories"></ul></li>');
            }
            if (aCategories.length > 8) {
                $('ul.aCategories').cycle({
                    fx: 'fade',
                    speed: 'fast',
                    timeout: 0,
                    pager: '#anav',
                    next: '#anext',
                    prev: '#aprevious',
                    height: '350px',
                    width: '90%',
                    fit: '1'
                });
                $("#aNavigation").show();
            }
        }

    });

    $(".shareFB").click(function () {
        var urlToShare = 'http://audioplanet.co.in' + $(this).attr('data-src');
        window.open('https://www.facebook.com/sharer/sharer.php?u=' + urlToShare, 'facebook-share-dialog', 'width=626,height=436');
        return false;
    });

    $(".categoryLink").click(function () {
        var categoryId = $(this).attr('id');
        $.ajax({
            url: "/Home/GetArticlesByCategory", data: "category=" + categoryId, datatype: "html",
            success: function (data) {
                if (data != '') {
                    $("div#ArticlesLeftPanel").empty().html(data);
                }
            }
        });
        return false;
    });

    $("#searchForm").submit(function () {
        var searchKeyword = $('.searcharticle').val();
        if (searchKeyword != '') {
            $.ajax({
                url: "/Home/GetArticlesByKeyword",
                data: "keyword=" + searchKeyword,
                datatype: "html",
                success: function (data) {
                    if (data != '') {
                        $("div#ArticlesLeftPanel").empty().html(data);
                    }
                }
            });
        }
        $('.searcharticle').focus();
        return false;
    });

    $("#searchFormProduct").submit(function () {
        var searchKeyword = $('.searcharticle').val();
        if (searchKeyword != '') {
            $.ajax({
                url: "/Home/GetProductReviewsByKeyword",
                data: "keyword=" + searchKeyword,
                datatype: "html",
                success: function (data) {
                    if (data != '') {
                        $("div#ArticlesLeftPanel").empty().html(data);
                    }
                }
            });
        }
        $('.searcharticle').focus();
        return false;
    });
});

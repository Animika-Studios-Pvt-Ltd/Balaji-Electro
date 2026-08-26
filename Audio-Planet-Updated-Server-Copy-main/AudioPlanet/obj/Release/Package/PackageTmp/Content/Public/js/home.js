$(document).ready(function () {
    $("img").bind("contextmenu", function () {
        return false;
    });
    
    //Load the Home Banner, After loading the 2nd image of the gallery,
    //Since it takes more time load
    if ($("#img2").length > 0) {
        $("#img2").load(function () { $("#HomeBanner_bg").show("slow"); });
    }

    //Set the Interval of 10 seconds to trigger gallery
    $(function () {
        interval = setInterval(callFunc, 10000);
    });

    function callFunc() {
        $("img#img1").trigger("click");
    }

    $("img#img1").click(function () {
        var b = $(this).attr("src");
        var a = $("img#img2").attr("src");
        var c = $("img#img3").attr("src");
        $("img#img1").attr("src", c);
        $("img#img2").fadeOut("slow", function () {
            $("img#img2").attr("src", b.replace("1", "1L").replace("2", "2L").replace("3", "3L"));
            $("img#img2").effect("slide", 1000);
        });
        //$("img#img2").attr("src", b.replace("1", "1L").replace("2", "2L").replace("3", "3L")).effect("slide", 1000);
        $("img#img3").attr("src", (a.indexOf("L") >= 0) ? a.replace("1L", "1").replace("2L", "2").replace("3L", "3") : a);
    });
    $("img#img3").click(function () {
        var c = $(this).attr("src");
        var a = $("img#img1").attr("src");
        var b = $("img#img2").attr("src");
        $("img#img1").attr("src", (b.indexOf("L") >= 0) ? b.replace("1L", "1").replace("2L", "2").replace("3L", "3") : b);
        $("img#img2").fadeOut("slow", function () {
            $("img#img2").attr("src", c.replace("1", "1L").replace("2", "2L").replace("3", "3L"));
            $("img#img2").effect("slide", { direction: "right" }, 1000);
        });
        $("img#img3").attr("src", a);
    });

    $(function () {
        $(".Enquery").hide();
        $(".enqThumb").click(function () { $(".Enquery").toggle("slow"); });
        $(".cancle").click(function () { $(".Enquery").hide(); });
    });

    if ($("div#testimonialList").length > 0) {
        $("div#testimonialList").cycle({ fx: "fade", speed: 300, timeout: 4000, next: "#s3", pause: 1 });
    }
});
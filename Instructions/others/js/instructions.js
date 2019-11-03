$(function() {
    $("a[href^='#']").click(function() {
        var duration = 1000;
        var href = $(this).attr("href");
        var target = $(href == "#" || href == "" ? "html" : href);
        var top = target.offset().top;
        $("body,html").animate({scrollTop: top}, duration, "swing");
        return false;
    });
});
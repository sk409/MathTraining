$(function() {

    function getParam(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    var stageNumber = getParam("stage_number");
    var title = "ステージ" + stageNumber + "の答え";
    document.title = title
    $("#title").text(title);
    $("#answer-video").attr("src", "answers/answer-" + stageNumber + ".mp4");

});
$(function () {
    $("form").hover(function () {
        $(this).stop().fadeTo(200, 1);
    }, function () {
        $(this).stop().fadeTo(1000, 0.5);
    }).mouseout();
});
$(function () {

    $.each($(".md"),
        function (index, value) {
            new SimpleMDE({
                element: value,
                hideIcons: ["image", "heading", "guide"],
                status: false
            });
        });

});
var readURL = function (input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#uploadedImage').attr('src', e.target.result);
            $('#uploadedImage').attr('alt', input.files[0].name);
            $('#uploadedImage').removeClass("hidden");
            $('#cancelUploadImage').removeClass("hidden");
            $('#uploadImagePlaceholder').addClass("hidden");
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$(function () {

    $.each($(".md"),
        function (index, value) {
            new SimpleMDE({
                element: value,
                hideIcons: ["image", "heading", "guide"],
                status: false
            });
        });

    $("#fileUpload").change(function () {
        readURL(this);
    });

    $("#cancelUploadImage").click(function () {
        $('#uploadedImage').attr('src', "");
        $('#uploadedImage').attr('alt', "");
        $('#uploadedImage').addClass("hidden");
        $('#cancelUploadImage').addClass("hidden");
        $('#uploadImagePlaceholder').removeClass("hidden");
    });

});
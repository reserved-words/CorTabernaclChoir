tinymce.init({
    selector: 'textarea.bbcode',
    height: 400,
    menubar: false,
    plugins: ["bbcode link charmap"],
    toolbar: 'bold italic | link | charmap',
    content_css: '//www.tinymce.com/css/codepen.min.css',
    default_link_target: "_blank",
    elementpath: false,
    entity_encoding: "raw"
});
var editor = CKEDITOR.instances['Body'];
if (editor) { editor.destroy(true); }
CKEDITOR.replace('Body', {
    enterMode: CKEDITOR.ENTER_BR,
    language: 'ru'
});
editor = CKEDITOR.instances['Body'];

checkTagsValidity = function () {
    var isTagInvalid = $(tagsInputId).val().isEmpty();
    if (isTagInvalid && wasTagValid) {
        showErrorMessage("TagsString", tagsErrMsg, tagsEditor, tagsEditor);
        wasTagValid = false;
    }
    else if (!isTagInvalid && !wasTagValid) {
        hideErrorMessage("TagsString", tagsEditor, tagsEditor);
        wasTagValid = true;
    }
}

handleTagRemove = function () {
    checkTagsValidity();
    $("#TagsString_tag").focus();
}

var maxTagLength = 20;
var tagsInputId = "#TagsString";
$(tagsInputId).tagsInput({
    'width': '100%',
    'maxChars': maxTagLength,
    'onAddTag': checkTagsValidity,
    'onRemoveTag': handleTagRemove,
    'defaultText': 'добавить тег',
    'autocomplete_url': '/Admin/AdminTags/Autocomplete',
    'autocomplete': { 'minLength': 2 }
});
$(tagsInputId + '_tag').attr("maxlength", maxTagLength);
$(tagsInputId + '_tag').maxlength();
var tagsEditor = $("#TagsString_tagsinput");

String.prototype.isEmpty = function () {
    return (this.length === 0 || /^((?:<br \/>)|(?:&nbsp;)|(?:\s))*$/i.test(this));
};

var bodyErrMsg = $("textarea[name='Body']").attr("data-val-required");
var tagsErrMsg = $("input[name='TagsString']").attr("data-val-required");
var wasBodyValid = true;
var wasTagValid = true;

checkBodyValidity = function () {
    var isBodyInvalid = editor.getData().isEmpty();
    if (isBodyInvalid && wasBodyValid) {
        showErrorMessage("Body", bodyErrMsg, $("#cke_Body"), $("iframe").contents().find("body"));
        wasBodyValid = false;
    }
    else if (!isBodyInvalid && !wasBodyValid) {
        hideErrorMessage("Body", $("#cke_Body"), $("iframe").contents().find("body"));
        wasBodyValid = true;
    }
}



showErrorMessage = function (fieldName, errMsg, elemToSetBorder, elemToSetBackground) {
    $("span[data-valmsg-for='" + fieldName + "']").attr("class", "text-danger field-validation-error")
            .append("<span for='" + fieldName + "'>" + errMsg + "</span>");
    elemToSetBorder.css("border", "1px solid #f00");
    elemToSetBackground.css("background-color", "#fee");
}

hideErrorMessage = function (fieldName, elemToSetBorder, elemToSetBackground) {
    $("span[data-valmsg-for='" + fieldName + "']").attr("class", "text-danger field-validation-valid")
            .empty();
    elemToSetBorder.css("border", "");
    elemToSetBackground.css("background-color", "");
}

editor.on("change", checkBodyValidity);

$("form").submit(function () {
    checkBodyValidity();
    checkTagsValidity();
    if (!wasBodyValid) {
        editor.focus();
        $("body").scrollTop($("#cke_Body").offset().top);
    }
    else if (!wasTagValid) {
        $(tagsInputId + '_tag').focus();
        $("body").scrollTop(tagsEditor.offset().top);
    }
    return wasBodyValid && wasTagValid;
});


function PreviewImage() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("titleImage").files[0]);
    $("#titleText").attr("value", document.getElementById("titleImage").files[0].name);
    oFReader.onload = function (oFREvent) {
        document.getElementById("uploadPreview").src = oFREvent.target.result;
        if ($("#previewRow").attr("hidden"))
            $("#previewRow").removeAttr("hidden");
    };
}
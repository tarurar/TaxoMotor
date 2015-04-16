function DefaultUploadOverwriteOff() {
    $("input[id$='OverwriteSingle'][type=checkbox]")
        .prop("checked", false)
        .parent().hide();
    $("input[id$='OverwriteMultiple'][type=checkbox]")
        .prop("checked", false)
        .parent().hide();
}

_spBodyOnLoadFunctionNames.push('DefaultUploadOverwriteOff');
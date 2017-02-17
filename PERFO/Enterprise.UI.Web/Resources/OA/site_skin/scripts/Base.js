Array.prototype.remove = function (obj) {
    return RemoveArray(this, obj);
};

function RemoveArray(array, attachId) {
    for (var i = 0, n = 0 ; i < array.length; i++) {
        if (array[i] != attachId) {
            array[n++] = array[i]
        }
    }
    array.length -= 1;
}

//by pengwei  扩展EhtmlEditor的验证
function EHtmlEditor(id) {
    var obj = new Object();
    obj.required = $('#' + id).attr('required');
    obj.maxlength = $('#' + id).attr('maxlength');
    obj.invalidMessage = $('#' + id).attr('invalidMessage');
    obj.validate = function () {
        if (this.required == "required") {
            if ($('#' + id).val().length > 0 && $('#' + id).val().length < this.maxlength) {
                return true;
            }
            else {
                //alert(this.invalidMessage);
                $.messager.alert('系统提示', this.invalidMessage);
                return false;
            }
        }
        else {
            if ($('#' + id).val().length < this.maxlength) {
                return true;
            }
            else {
                //alert(this.invalidMessage);
                $.messager.alert('系统提示', this.invalidMessage);
                return false;
            }
        }
    }
    return obj;
}

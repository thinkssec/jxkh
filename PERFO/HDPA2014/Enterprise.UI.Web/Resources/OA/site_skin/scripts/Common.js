//错误屏蔽
window.onerror = function () {
    return false;
};

//提交后显示提示信息
function showLoading() {
    var win = $.messager.progress({
        title: '请您稍侯',
        msg: '正在提交数据...'
    });
    //alert(win);
    setTimeout(function () {
        $.messager.progress('close');
    }, 10000)
}

//检测输入值为数字
function regInput(obj, inputStr) {
    var reg = /^-?\d*\.?\d{0,2}$/;
    var str = inputStr;
    return reg.test(str);
}

//删除提示
function aa() {
    if (confirm('删除不可恢复!您确定要删除数据？')) {
        return true;
    }
    return false;
}

//上传窗口弹出
function openwin(url) {
    $('#upwin').window('open');
    $('#upwin').html("<iframe Scrolling=\"yes\" Frameborder=\"0\" Src=\"" + url + "\" Style=\"width:98%;height:97%;\"></iframe>");
}

function DoNothing(arrmsg) {

}

function openwin(winid, url) {
    $('#' + winid).window('center');
    $('#' + winid).window('open');
    $('#' + winid).html("<iframe Scrolling=\"yes\" Frameborder=\"0\" Src=\"" + url + "\" Style=\"width:98%;height:97%;\"></iframe>");
}

function openwin(winid, url, ww, hh) {
    $('#' + winid).window({
        width: ww,
        height: hh,
        modal: true
    });
    $('#' + winid).window('center');
    $('#' + winid).window('open');
    $('#' + winid).html("<iframe Scrolling=\"yes\" Frameborder=\"0\" Src=\"" + url + "\" Style=\"width:98%;height:97%;\"></iframe>");
}

////project-table 隔行换色
//$(document).ready(function () {
//    $(".project-table tr").mouseover(function () {
//        //如果鼠标移到class为stripe的表格的tr上时，执行函数  
//        $(this).addClass("over");
//    }).mouseout(function () {
//        //给这行添加class值为over，并且当鼠标一出该行时执行函数  
//        $(this).removeClass("over");
//    }) //移除该行的class  
//    $(".project-table tr:even").addClass("alt");
//    //给class为stripe的表格的偶数行添加class值为alt
//});

////在Iframe中调用父窗口的Tree重新加载数据
//function ReloadProjectRunningTree() {
//    parent.$('#ProjectRunningTree').tree('reload');
//}


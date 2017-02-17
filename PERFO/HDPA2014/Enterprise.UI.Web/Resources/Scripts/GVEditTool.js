//********************************************************************
// 文件名：GVEditTool.js
// 功能描述:实现GridView在线编辑
// 创建人：乔巍
// 创建日期：2012.9.13
//*********************************************************************

////处理错误
//window.onerror = function () {
//    return false;
//};

//********************
//检测输入值为数字
//********************
function regInput(obj, inputStr) {
    var reg = /^-?\d*\.?\d{0,4}$/;
    var docSel = document.selection.createRange();
    if (docSel.parentElement().tagName != "INPUT") return false;
    oSel = docSel.duplicate();
    oSel.text = "";
    var srcRange = obj.createTextRange()
    oSel.setEndPoint("StartToStart", srcRange);
    var str = oSel.text + inputStr + srcRange.text.substr(oSel.text.length);
    return reg.test(str);
}

//********************
//获取粘贴板中的数据
//********************
function getClipboard() {
    if (window.clipboardData) {
        return (window.clipboardData.getData('Text'));
    }
    return "";
}

//***************************
//自动加载数据控件的编辑功能
//***************************
$(function () {

    //找到所有需要编辑的单元格
    var numTd = $("#MainPH_GridView1 tbody td");
    //表格总行数
    var trCount = $("#MainPH_GridView1 tbody tr").length;

    //单元格值的修改
    var inputs = $(":input[type='text']", "#MainPH_GridView1 tbody td");
    //    alert(inputs.length + "=====" + trCount);
    //    inputs.each(function () {
    //        var ww = $(this).attr("inputwidth");
    //        if (ww == undefined || ww == "") {
    //            ww = "100%";
    //        }
    //        $(this).css("border-width", "0").css("font-size", "12px")
    //                .css("display", "block")
    //                .css("background-color", "#ADFF2F").width(ww).height("100%");
    //    });
    inputs.click(function () {
        var inputObj = $(this);
        var col = parseInt(inputObj.attr("colnum"));
        var row = parseInt(inputObj.attr("rownum"));
        var dataType = inputObj.attr("datatype");
        var isData = (dataType == "number") ? true : false;
        //alert(col + "," + row + "," + dataType);
        //使文本框的内容添加后就被选中(trigger可以执行javascript中的方法)
        inputObj.trigger("focus").trigger("select");

        //处理文本框上回车和ESC按键的操作
        inputObj.keyup(function (event) {
            //获取当前按下的键盘的键值
            //处理粘贴板数据,Ctrl+V
            if (event.ctrlKey && event.keyCode == 86) {
                var clipStr = getClipboard();
                //alert(clipStr);
                if (clipStr) {
                    var len = clipStr.split("\n"); //获取行数
                    if (len && len.length > 1) {
                        for (var i = 0; i < len.length - 1; i++) {
                            // && 
                            if (i + row < trCount) {
                                var prefix = "Txt" + col + "_";
                                if (isData && !isNaN(len[i])) {
                                    $(":input[txtname^= " + prefix + "]", "#MainPH_GridView1 tbody tr:eq(" + (i + row) + ")").val(len[i]);
                                    //$(":input[type='text']", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ") td:eq(" + col + ")").val(len[i]);
                                }
                                else if (!isData) {
                                    //alert(trCount + ",hang===" + (row + i) + ",lie===" + col);
                                    //alert($(":input", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ") td:eq(" + col + ")").val());
                                    $(":input[txtname^= " + prefix + "]", "#MainPH_GridView1 tbody tr:eq(" + (i + row) + ")").val(len[i]);
                                    //$(":input[type='text']", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ") td:eq(" + col + ")").val(len[i]);
                                }
                            }
                        }
                    }
                    else {
                        if (isData && !isNaN(clipStr)) {
                            inputObj.val(clipStr);
                        }
                        else if (!isData) {
                            inputObj.val(clipStr);
                        }
                    }
                }
            }
        });
    });
});
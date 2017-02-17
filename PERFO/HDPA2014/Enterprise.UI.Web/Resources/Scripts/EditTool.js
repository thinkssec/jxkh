//********************************************************************
// 文件名：EditTool.js
// 功能描述：解决 Infragistics 数据控件与IE9的不兼容问题，增加编辑功能。
// 创建人：乔巍
// 创建日期：2012.7.23
//*********************************************************************


//********************
//检测输入值为数字
//********************
function regInput(obj, inputStr) {
    var reg = /^\d*\.?\d{0,4}$/;
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
    return null;
}

//***************************
//自动加载数据控件的编辑功能
//***************************
$(function () {

    //找到所有需要编辑的单元格
    var numTd = $("#G_UltraWebGrid1 tbody td");
    //表格总行数
    var trCount = $("#G_UltraWebGrid1 tbody tr").length;

    //单元格值的修改
    var inputs = $(":input[type='text']", "#G_UltraWebGrid1 tbody td");
    //alert(inputs.length);
    inputs.each(function () {
        $(this).css("border-width", "0").css("font-size", "11px")
                .css("display", "block")
                .css("background-color", "#ADFF2F").width("100%").height("100%");
    });
    inputs.click(function () {
        var inputObj = $(this);
        var col = parseInt(inputObj.attr("colnum"));
        var row = parseInt(inputObj.attr("rownum"));
        var dataType = inputObj.attr("datatype");
        var isData = (dataType == "number") ? true : false;

        //使文本框的内容添加后就被选中(trigger可以执行javascript中的方法)
        inputObj.trigger("focus").trigger("select");

        //处理文本框上回车和ESC按键的操作
        inputObj.keyup(function (event) {
            //获取当前按下的键盘的键值
            //处理粘贴板数据,Ctrl+V
            if (event.ctrlKey && event.keyCode == 86) {
                var clipStr = getClipboard();
                if (clipStr) {
                    var len = clipStr.split("\n"); //获取行数
                    if (len && len.length > 1) {
                        for (var i = 0; i < len.length - 1; i++) {
                            // && 
                            if (i + row < trCount) {
                                var prefix = "Txt" + col + "_";
                                if (isData && !isNaN(len[i])) {
                                    $(":input[name^= " + prefix + "]", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ")").val(len[i]);
                                    //$(":input[type='text']", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ") td:eq(" + col + ")").val(len[i]);
                                }
                                else if (!isData) {
                                    //alert(trCount + ",hang===" + (row + i) + ",lie===" + col);
                                    //alert($(":input", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ") td:eq(" + col + ")").val());
                                    $(":input[name^= " + prefix + "]", "#G_UltraWebGrid1 tbody tr:eq(" + (i + row) + ")").val(len[i]);
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



//            //给这些单元格注册Click事件
//            numTd.click(function (evt) {
//                var currTd = $(this);
//                //当前行
//                var hang = $(this).parent("tr").prevAll().length;
//                //当前列
//                var lie = $(this).prevAll().length;
//                //alert($(":input", currTd).length + ",hang==" + hang + ",lie==" + lie);

//                var inputOjb = $(":input", currTd);

//                //alert(inputOjb);

//                //处理文本框上回车和ESC按键的操作
//                currTd.keyup(function (event) {
//                    //获取当前按下的键盘的键值
//                    //处理粘贴板数据,Ctrl+V
//                    if (event.ctrlKey && event.keyCode == 86) {
//                        var clipStr = getClipboard();
//                        if (clipStr) {
//                            var len = clipStr.split("\n"); //获取行数
//                            if (len && len.length > 1) {
//                                for (var i = 0; i < len.length - 1; i++) {
//                                    if (i + hang < trCount && !isNaN(len[i])) {
//                                        alert(len[i]);
//                                        //alert(len[i] + $("#G_UltraWebGrid1 tr:eq(" + (i + hang) + ") td:eq(" + lie + ")").text());
//                                        $("#G_UltraWebGrid1 tr:eq(" + (i + hang) + ") td:eq(" + lie + ")").html(len[i]);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                });

//            });
//                //此句将取消默认的单击事件。
//                evt.preventDefault();
//                //精简上面的代码

//                //找到当前鼠标点击的那个td,this对应的就是响应了click的那个td
//                var currTd = $(this);
//                //var allowEdit = $(this).attr("allowedit");
//                //当前行
//                var hang = $(this).parent("tr").prevAll().length;
//                //当前列
//                var lie = $(this).prevAll().length;
//                //alert($(":input", currTd).length + ",hang=="+hang+",lie=="+lie);

//                //hang = Number(hang) + 1; //字符串变为数字 
//                //lie = Number(lie) + 1;
//                //alert("第" + hang + "行" + "第" + lie + "列" + "," + trCount);
//                //alert($(this).attr("AllowEdit"));
//                // || !allowEdit || allowEdit == "no"
//                //                if (currTd.children("input").length > 0) {
//                //                    //如果当前td中已包含有文本框元素，则不执行click事件
//                //                    var inputObj = currTd.children("input")[0];

//                //                    //alert(inputObj + ",hang==" + hang + ",lie==" + lie);

//                //                    //return false;
//                //                }
//                //                //当前td的内容
//                //                var tdtext = currTd.html();
//                //                tdtext = tdtext.replace("&nbsp;", "");
//                //                //tdtext = $('NOBR', tdtext).text();
//                //                //alert(tdtext); 
//                //                //清除td的内容
//                //                currTd.html("");
//                //                //创建一个文本框
//                //                //去掉文本框的边框
//                //                //设置文本框中字体大小和当前td中的字体大小一样,为16px;
//                //                //设置文本框的背景色和当前td背景色一样  currTd.css("background-color")
//                //                //让文本框的宽度和td的宽度一样
//                //                //将td的内容放到文本框中
//                //                //将文本框插入到td中去
//                //                var inputOjb = $("<input type='text' onkeypress = \"return regInput(this,String.fromCharCode(event.keyCode))\" onpaste = \"return regInput(this,window.clipboardData.getData('Text'))\" ondrop = \"return regInput(this,event.dataTransfer.getData('Text'))\" />")
//                //                            .css("border-width", "0").css("font-size", "11px")
//                //                            .css("background-color", "#ffccff").width(currTd.width())
//                //                            .val(tdtext).appendTo(currTd);

//                //                //使文本框的内容添加后就被选中(trigger可以执行javascript中的方法)
//                //                inputOjb.trigger("focus").trigger("select");
//                //                //去掉文本框的点击事件,(javascript事件是冒泡型的)
//                //                inputOjb.click(function () {
//                //                    return false;
//                //                });
//                //                //失去焦点
//                //                inputOjb.blur(
//                //                    function () {
//                //                        //保存当前输入的内容
//                //                        var inputText = $(this).val();
//                //                        currTd.html(inputText);
//                //                    }
//                //                );
//                //                //处理文本框上回车和ESC按键的操作
//                //                inputOjb.keyup(function (event) {
//                //                    //获取当前按下的键盘的键值
//                //                    // 不同的按键可以做不同的事情
//                //                    var keyCode = event.which;
//                //                    //alert(keyCode);
//                //                    //                    //处理回车键 ,不同的浏览器的keycode不同   
//                //                    //                    if (keyCode == 13) {
//                //                    //                        //保存当前输入的内容
//                //                    //                        var inputText = $(this).val();
//                //                    //                        currTd.html(inputText);
//                //                    //                    }
//                //                    //处理ESC键的操作
//                //                    if (keyCode == 27) {
//                //                        //将当前TD的内容还原成tdtext
//                //                        currTd.html(tdtext);
//                //                    }
//                //                    //处理粘贴板数据,Ctrl+V
//                //                    if (event.ctrlKey && event.keyCode == 86) {
//                //                        var clipStr = getClipboard();
//                //                        if (clipStr) {
//                //                            var len = clipStr.split("\n"); //获取行数
//                //                            if (len && len.length > 1) {
//                //                                for (var i = 0; i < len.length - 1; i++) {
//                //                                    if (i + hang < trCount && !isNaN(len[i])) {
//                //                                        //alert(len[i] + $("#GridView1 tr:eq(" + hang + ") td:eq(1)").text());
//                //                                        $("#UltraWebGrid1_main tr:eq(" + (i + hang) + ") td:eq(" + lie + ")").html(len[i]);
//                //                                    }
//                //                                }
//                //                            }
//                //                        }
//                //                    }
//                //                });
//            }); 
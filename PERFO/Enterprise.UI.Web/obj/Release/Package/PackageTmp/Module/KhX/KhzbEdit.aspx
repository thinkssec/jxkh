<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhzbEdit.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhzbEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考核指标维护</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
    <style type="text/css">
        .width {
            width: 370px;
            max-width: 370px;
        }
        textarea {
            font-size: 12px;
            font-family:"微软雅黑";
        }
    </style>
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            if ("<%=mbid%>" != "")//编辑 数据初始化
            {
                $.ajax({
                    url: "data/KhHandler.ashx",
                    type: "post",
                    data: { type: "getZbDetail", zbid: "<%=zbid%>" },
                    success: function (result) {
                        if (result != "") {
                            if (result.indexOf("失败") > -1) {
                                alert(result);
                            }
                            else {
                                var json = eval("(" + result + ")")[0];
                                $("#selectKhxm1").combobox("setValue", json.XM1);
                                $("#selectKhxm2").combobox("setValue", json.XM2);
                                $("#taKhnr").val(json.NR);
                                $("#taKhbz").val(json.BZ);
                                $("#txtKhzq").val(json.ZQ);
                                $("#selectKhfs").combobox("setValue", json.FS);
                                $("#txtBzfz").val(json.BZF);
                                $("#txtPx").val(json.ORD);
                            }
                        }
                    }
                });
            }
            $("#selectKhxm1").combobox({
                url: 'data/KhHandler.ashx?type=getKhxm&lb=xm1',
                valueField: 'XM1',
                textField: 'XM1'
            });
            $("#selectKhxm2").combobox({
                url: 'data/KhHandler.ashx?type=getKhxm&lb=xm2',
                valueField: 'XM2',
                textField: 'XM2'
            });
            $("#btnCancel").bind("click", function () {
                window.history.go(-1);
            });
            $("#btnSave").bind("click", function () {
                var xm1 = $("#selectKhxm1").combobox("getValue");
                if (xm1 == null) {
                    alert("项目1不能为空！"); return;
                }
                var xm2 = $("#selectKhxm2").combobox("getValue");
                if (xm2 == null) xm2 = "";
                var nr = $("#taKhnr").val();
                if (nr == "") {
                    alert("考核内容不能为空！");
                    return;
                }
                var bz = $("#taKhbz").val();
                if (bz == "") {
                    alert("考核标准不能为空！");
                    return;
                }
                var zq = $("#txtKhzq").val();
                if (zq == "") {
                    alert("考核周期不能为空！"); return;
                }
                var fs = $("#selectKhfs").combobox("getValue");
                var bzf = $("#txtBzfz").val();
                var ord = $("#txtPx").val();
               
                $.ajax({
                    url: "data/KhHandler.ashx",
                    type: "post",
                    data: {
                        type: "saveKhzb", id: '<%=zbid%>', xm1: xm1, xm2: xm2, nr: nr,
                        bz: bz, zq: zq, fs: fs, bzf: bzf, mbid: '<%=mbid%>', pftype: '<%=type%>', ord: ord
                    },
                    success: function (result) {
                        if (result != "")
                        {
                            alert(result);
                        }
                        else
                        {
                            //alert("/Module/KhX/KhZbLr.aspx?mbid=<%=mbid%>&bm=<%=bm%>&id=<%=khdzbid%>");
                            location.href = "/Module/KhX/KhZbLr.aspx?mbid=<%=mbid%>&bm=<%=bm%>&id=<%=khdzbid%>";
                        }
                    }
                });

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; font-size: 12px">
            <table style="margin:0 auto;">
                <tr>
                    <td>考核项目1：(*)</td>
                    <td style="width: 385px;">
                        <input id="selectKhxm1" style="width: 370px" /></td>
                </tr>
                <tr>
                    <td>考核项目2：</td>
                    <td>
                        <input id="selectKhxm2" style="width: 370px" /></td>
                </tr>
                <tr>
                    <td>考核内容：(*)</td>
                    <td>
                        <textarea id="taKhnr" class="width"></textarea></td>
                </tr>
                <tr>
                    <td>考核标准：(*)</td>
                    <td>
                        <textarea id="taKhbz" class="width" style="height: 120px;"></textarea></td>
                </tr>
                <tr>
                    <td>考核周期：</td>
                    <td>
                        <input id="txtKhzq" type="text" value="季度" class="width" /></td>
                </tr>
                <tr>
                    <td>考核方式：</td>
                    <td>
                        <select id="selectKhfs" style="width: 370px" class="easyui-combobox">
                            <option>日常检查</option>
                            <option>现场检查</option>
                            <option>资料检查</option>
                        </select></td>
                </tr>
                <tr>
                    <td>标准分值：</td>
                    <td>
                        <input id="txtBzfz" value="10" class="easyui-numberspinner" style="width: 370px;"
                            data-options="min:0,max:100,editable:true" />
                    </td>
                </tr>
                  <tr>
                    <td>排序：(*)</td>
                    <td>
                        <input id="txtPx" class="easyui-numberspinner" precision="2" style="width: 370px;"
                            data-options="min:0,max:100,editable:true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <a id="btnSave" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'">保存</a>
                        <a id="btnCancel" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" style="margin-left: 10px;">返回</a>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

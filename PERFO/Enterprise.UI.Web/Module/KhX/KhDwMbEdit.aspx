<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhDwMbEdit.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhDwMbEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>单位模板配置</title>
     <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#selectMb").combobox({
                url: 'data/KhHandler.ashx?type=getKhmbByDzb&khdzbid=<%=khdzbid%>',
                valueField: 'ID',
                textField: 'NAME'
            });
            $("#selectBkhdw").combobox({
                url: "data/KhHandler.ashx?type=getbkhdw",
                valueField: "JGBM",
                textField: "JGMC"
            });
            $("#btnSave").bind("click", function () {
                var mbid = $("#selectMb").combobox("getValue");
                var bkhdw = $("#selectBkhdw").combobox("getValue");
                if (mbid == null)
                {
                    alert("请选择考核项！"); return;
                }
                if (bkhdw == null) {
                    alert("请选择被考核单位！"); return;
                }
                $.ajax({
                    url: "data/KhHandler.ashx",
                    type: "post",
                    data: { type: "saveDwmb", mbid: mbid, bkhdw: bkhdw, khdzbid: "<%=khdzbid%>", id: '<%=id%>' },
                    success: function (result) {
                        if (result != "") {
                            alert(result);
                        }
                        else location.href = "KhDwmb.aspx?khid=<%=khid%>&bm=<%=bm%>&khdzbid=<%=khdzbid%>";
                    }
                });
            });
            $("#btnCancel").bind("click", function () {
                location.href = "KhDwmb.aspx?khid=<%=khid%>&bm=<%=bm%>&khdzbid=<%=khdzbid%>";
            });

            if ("<%=id%>" != "")//编辑
            {
                $.ajax({
                    url: "data/KhHandler.ashx",
                    type: "post",
                    data: { type: "getKhDwmbByID", id: "<%=id%>" },
                    success: function (result)
                    {
                        if (result != "")
                        {
                            var array = eval("(" + result + ")");
                            var json = array[0];
                            var mbid = json.MBID;
                            var bkhdw = json.JGBM;
                            $("#selectMb").combobox("setValue", mbid);
                            $("#selectBkhdw").combobox("setValue", bkhdw);
                        }
                    }
                });
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center;font-size:12px;">
            <table>
                <tr>
                    <td>考核项：</td>
                    <td><input id="selectMb" editable="false" style="width:260px" />  </td>
                </tr>
                <tr>
                    <td>被考核单位：</td>
                    <td><input id="selectBkhdw" editable="false" style="width:260px" /> </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <a href="#" id="btnSave" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">保存</a>
                        <a href="#" id="btnCancel" class="easyui-linkbutton" style="margin-left: 2px" data-options="iconCls:'icon-add',plain:true">返回</a>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

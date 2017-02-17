<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhFq.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhFq" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>发起考核</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>  
    <script type="text/javascript">
        $(function () {
            var date = new Date();
            var year = date.getFullYear();
            var jd = parseInt(date.getMonth() / 3);//季度
            $("#txtYear").val(year);
            $("#txtJd").val(jd);
            $('#selectKhdw').combobox({
                url: "data/KhHandler.ashx?type=getkhpjdw",
                valueField: "JGBM",
                textField: "JGMC",
                onSelect: function (record) {
                    $('#selectBkhdw').combobox({
                        url: "data/KhHandler.ashx?type=getbkhdw&bm=" + record.JGBM,
                        valueField: "JGBM",
                        textField: "JGMC"
                    });
                }
            });
            $('#selectBkhdw').combobox({
                url: "data/KhHandler.ashx?type=getbkhdw",
                valueField: "JGBM",
                textField: "JGMC"
            });
            $("#btnSure").bind("click", function () {
                if (!$("#txtYear").val()) {
                    alert("考核年度不能为空！"); return;
                }
                if (!$("#txtJd").val()) {
                    alert("考核季度不能为空！"); return;
                }
                if ($("#selectKhdw").combobox("getValue") == "全部")
                    pjdw = null;
                else pjdw = $("#selectKhdw").combobox("getValue");
                if ($("#selectBkhdw").combobox("getValue") == "全部")
                    khdw = null;
                else khdw = $("#selectBkhdw").combobox("getValue");
                if (khdw != null && pjdw != null && khdw == pjdw) {
                    alert("考核单位与被考核单位不能相同！");
                    return;
                }
                var year = $("#txtYear").val();
                var jd = $("#txtJd").val();
                $.ajax({
                    url: "data/KhHandler.ashx?type=saveKhfq",
                    type: "post",
                    data: { name: year + "年第" + jd + "季度考核", year: year, jd: jd, pjdw: pjdw, khdw: khdw },
                    success: function (result) {
                        if (result != "") {
                            alert(result);
                        }
                        else {
                            $("#p").show();
                            $("#dd").hide();
                            //刷新考核列表
                            $("#dg").datagrid("reload");
                        }
                    }
                });
            });
            $("#btnCancel").bind("click", function () {
                $("#p").show();
                $("#dd").hide();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table style="margin: 3px auto;font-size:12px;">
                <tr>
                    <td style="width: 100px;">名称:</td>
                    <td style="width: 150px">
                        <input id="txtYear" class="easyui-validatebox" style="width: 30px" type="text" data-optios="required:true" />
                        年第<input id="txtJd" type="text" style="width: 20px;" />季度考核</td>
                </tr>
                <tr>
                    <td>考核单位:</td>
                    <td>
                        <input id="selectKhdw" style="width: 140px" editable="false" value="全部" />
                    </td>
                </tr>
                <tr>
                    <td>被考核单位:</td>
                    <td>
                        <input id="selectBkhdw" style="width: 140px"  class="easyui-combobox" editable="false" value="全部" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <a id="btnSure" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'">确定</a>
                        <a id="btnCancel" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'">取消</a></td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>

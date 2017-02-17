<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhDwMb.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhDwMb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考核单位模板配置</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#dg").datagrid({
                url: 'data/KhHandler.ashx?type=getKhdwmb&khdzbid=<%=khdzbid%>',
                singleSelect: true,
                rownumbers: true,
                columns: [[
                    { field: 'ID', title: 'ID', hidden: true },//MBJG
                    { field: 'KHDZBID', title: 'KHDZBID', hidden: true },
                    { field: 'MBID', title: 'MBID', hidden: true },
                    { field: 'JGBM', title: 'JGBM', hidden: true },
                    { field: 'MBMC', title: '考核项', width: 320, align: "center" },
                    { field: 'JGMC', title: '考核单位', width: 220, align: "center" }
                ]],
                toolbar: '#tb'
            });
            $("#btnAdd").bind("click", function () {
                location.href = "KhDwMbEdit.aspx?khdzbid=<%=khdzbid%>&khid=<%=khid%>&bm=<%=bm%>";
            });
            $("#btnEdit").bind("click", function () {
                var selectRow = $("#dg").datagrid("getSelected");
                if (selectRow == null) {
                    alert("请选择要编辑的数据！");
                }
                else {
                    location.href = "KhDwMbEdit.aspx?khdzbid=<%=khdzbid%>&khid=<%=khid%>&bm=<%=bm%>&id=" + selectRow.ID;
                }
            });
            $("#btnDelete").bind("click", function () {
                var selectRow = $("#dg").datagrid("getSelected");
                if (selectRow == null) {
                    alert("请选择要删除的数据！");
                }
                else if (confirm("确认删除？")) {
                    $.ajax({
                        url: "data/KhHandler.ashx",
                        type: "post",
                        data: { type: "deleteDwmb", id: selectRow.ID },
                        success: function (result) {
                            if (result != "")
                                alert(result);
                            else $("#dg").datagrid("reload");
                        }
                    });
                }
            });

            //返回考核指标配置界面
            $("#btnBackToView").bind("click", function () {
                location.href = "khjgView.aspx";
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="考核单位配置" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <table id="dg"></table>
            <div id="tb">
                <a href="#" id="btnAdd" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-add',plain:true">新增</a>
                <a href="#" id="btnEdit" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-add',plain:true">编辑</a>
                <a href="#" id="btnDelete" class="easyui-linkbutton" data-options="iconCls:'icon-delete',plain:true">删除</a>
                <a href="#" id="btnBackToView" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">返回考核配置</a>
            </div>
        </div>
    </form>
</body>
</html>

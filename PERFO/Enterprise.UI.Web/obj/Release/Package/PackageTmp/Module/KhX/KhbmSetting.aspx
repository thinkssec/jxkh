<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhbmSetting.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhbmSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>考核部门设置</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function ()
        {
            $("#dg").datagrid({
                url: 'data/KhHandler.ashx?type=getKhdw',
                singleSelect: true,
                rownumbers: true,
                columns: [[
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: 'MBMC', title: '模板', width: 320, align: "center" },
                    { field: 'PJDW', title: '考核单位', width: 220, align: "center" },
                    {
                        field: 'KHDW', title: '被考核单位', width: 220, align: "center"
                    }
                ]],
                toolbar: '#tb'
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="考核单位列表" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <table id="dg"></table>
            <div id="tb">
                <a href="#" id="btnAdd" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-add',plain:true">新增</a>
                <a href="#" id="btnEdit" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-add',plain:true">编辑</a>
                <a href="#" id="btnDelete" class="easyui-linkbutton" data-options="iconCls:'icon-delete',plain:true">删除</a>
            </div>
        </div>
    </form>
</body>
</html>

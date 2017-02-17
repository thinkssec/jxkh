<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhZbMb.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhZbMb" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考核指标项维护</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        var editIndex = null;
        $(function ()
        {
            $("#dg").datagrid({
                url: 'data/KhHandler.ashx?type=getKhmb&bm=<%=bm%>&khid=<%=khid%>',
                singleSelect: true,
                rownumbers: true,
                columns: [[
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: 'NAME', title: '名称', width: 320, align: "center", editor: "text" },
                    { field: 'QZ', title: '权重(%)', width: 80, align: "center", editor: "numberbox" },
                    { field: 'MEMO', title: '说明', width: 320, align: "center", editor: "text" }
                ]],
                toolbar: '#tb',
                onClickRow: function (rowIndex)
                {
                    if (editIndex != null && $("#dg").datagrid("validateRow", editIndex))
                    {
                        $("#dg").datagrid("endEdit", editIndex);
                        editIndex = null;
                    }
                },
                onDblClickCell: function (rowIndex)
                {
                    if (editIndex != null && $("#dg").datagrid("validateRow", editIndex))
                    {
                        $("#dg").datagrid("endEdit", editIndex);
                    }
                    $("#dg").datagrid("beginEdit", rowIndex);
                    editIndex = rowIndex;
                }
            });
            $('#btnAdd').bind('click', function ()
            {
                $('#dg').datagrid('insertRow', {
                    index: 0,	// 索引从0开始
                    row: {}
                }).datagrid("beginEdit", 0);
                editIndex = 0;
            });
            $("#btnDelete").bind('click', function ()
            {
                var selectRow = $("#dg").datagrid('getSelected');
                if (selectRow != null)
                {
                    selectIndex = $("#dg").datagrid("getRowIndex", selectRow);
                    $("#dg").datagrid("deleteRow", selectIndex);
                }
                else
                {
                    alert("请选中要删除的数据行！");
                }
            });
            $("#btnSave").bind('click', function ()
            {
                if (editIndex != null)
                    $("#dg").datagrid("endEdit", editIndex);
                var data = $("#dg").datagrid("getChanges", "");
                if (data == "")
                {
                    alert("数据未作修改无需保存！");
                    return;
                }
                if (confirm("确认保存修改？"))
                {
                    var effectRow = new Object();
                    var inserted = $('#dg').datagrid('getChanges', "inserted");
                    var updated = $('#dg').datagrid('getChanges', "updated");
                    var deleted = $('#dg').datagrid('getChanges', "deleted");
                    $.ajax({
                        url: "data/KhHandler.ashx",
                        data: {
                            type: "saveMb",
                            khid: "<%=khid%>",
                            bm: "<%=bm%>",
                            id: "<%=id%>",
                            mb: "[{data:" + JSON.stringify(inserted) + "},{data:" + JSON.stringify(updated) + "},{data:" + JSON.stringify(deleted) + "}]"
                        },
                        type: "post",
                        success: function (result)
                        {
                            if (result != "")
                            {
                                alert(result);
                            }
                            else
                            {
                                $("#dg").datagrid("acceptChanges");
                                alert("保存成功！");
                            }
                        }
                    });
                }
            });
            $("#btnZbEdit").bind("click", function ()
            {
                var selectRow = $("#dg").datagrid('getSelected');
                if (selectRow != null)
                    location.href = "Khzblr.aspx?mbid=" + selectRow.ID + "&bm=<%=bm%>&id=<%=id%>&khid=<%=khid%>";
                else alert("请选择要维护的数据！");
            });
            //返回考核指标配置界面
            $("#btnBackToView").bind("click", function ()
            {
                location.href = "khjgView.aspx";
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="考核指标" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <table id="dg"></table>
            <div id="tb">
                <a href="#" id="btnAdd" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-add',plain:true">新增</a>
                <a href="#" id="btnDelete" class="easyui-linkbutton" data-options="iconCls:'icon-delete',plain:true">删除</a>
                <a href="#" id="btnSave" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true">保存</a>
                <a href="#" id="btnZbEdit" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">考核内容维护</a>
                <a href="#" id="btnBackToView" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">返回考核配置</a>
            </div>
        </div>
    </form>
</body>
</html>

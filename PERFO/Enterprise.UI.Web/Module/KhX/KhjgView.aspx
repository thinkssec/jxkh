<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhjgView.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhjgView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模板维护代办界面</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        var editIndex = null, pjdw = null, khdw = null;
        $(function () {
            $("#dg").datagrid({
                url: 'data/KhHandler.ashx?type=getKhList&bm=<%=this.userModel.Bmjg.JGBM%>',
                rownumbers: true,
                columns: [[
                     { field: 'ID', title: 'ID', hidden: true },//perfo_khs_khdzb表ID，考核指标和khid关联的ID
                    { field: 'KHID', title: 'KHID', hidden: true },
                    { field: 'KHMC', title: '名称', width: 220, align: "center" },
                    { field: 'ZBMC', title: '考核指标', width: 180, align: "center" },
                    {
                        field: 'KSSJ', title: '结束时间', width: 100, align: "center", formatter: function (val, row) {
                            var date = new Date(val);
                            return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
                        }
                    },
                    {
                        field: 'GBSJ', title: '结束时间', width: 100, align: "center", formatter: function (val, row) {
                            var date = new Date(val);
                            return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
                        }
                    },
                    { field: 'KHZT', title: '状态', width: 80, align: "center" },
                     { field: 'LXID', title: '考核类型', width: 80, align: "center", hidden: true },
                    { field: 'STATUS', title: '操作', width: 130, align: "center", formatter: formatOper }
                ]]
            });
        });

        function formatOper(val, row, index)
        {
            return '<a href="#" onclick="editMb(' + index + ')">考核项</a> | <a href="#" onclick="editKhbm(' + index + ')">考核部门</a> | <a href="#" onclick="pfScore(' + index + ')">评分</a>';
        }

        function editMb(index)
        {
            $('#dg').datagrid('selectRow', index);
            var row = $('#dg').datagrid('getSelected');
            if (row)
            {
                //模板编辑
                this.location.href = "KhzbMb.aspx?khid=" + row.KHID + "&bm=<%=this.userModel.Bmjg.JGBM%>&id=" + row.ID;
            }
        }

        function editKhbm(index)
        {
            $('#dg').datagrid('selectRow', index);
            var row = $('#dg').datagrid('getSelected');
            if (row)
            {
                //考核部门编辑
                this.location.href = "KhDwmb.aspx?khid=" + row.KHID + "&bm=<%=this.userModel.Bmjg.JGBM%>&khdzbid=" + row.ID;
            }
        }

        function pfScore(index)
        {
            $('#dg').datagrid('selectRow', index);
            var row = $('#dg').datagrid('getSelected');
            if (row)
            {
                location.href = "khzbpf.aspx?khid=" + row.KHID + "&bm=<%=this.userModel.Bmjg.JGBM%>&khdzbid=" + row.ID;
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="考核列表" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <table id="dg"></table>
        </div>
    </form>
</body>
</html>

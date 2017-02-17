<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhjgHz.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhjgHz" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考核结果汇总</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
    <style type="text/css">
        .center {
            text-align: center;
        }
        .right {
            text-align: right;
        }
    </style>
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#selectKhsj").combobox({
                url: 'data/KhHandler.ashx?type=getKhsj',
                valueField: 'NY',
                textField: 'NY'
            });
            $("#divKhResult").width($("#p").width());
            $("#btnSearch").bind("click", function () {
                var type = $("#type").combobox("getValue");
                var sj = $("#selectKhsj").combobox("getValue");
                if (sj == null || sj == "") {
                    alert("请选择考核时间！"); return;
                }
                if (type == "机关单位") { alert("暂不支持机关单位汇总查询！"); return; }
                $.ajax({
                    url: "data/KhHandler.ashx",
                    type: "post",
                    data: { type: "getJcdwHz", ny: sj },
                    success: function (result) {
                        if (result != "") {
                            if (result.indexOf("失败") > -1) {
                                alert(result);
                            }
                            else {
                                $("#divKhResult").html(result);
                            }
                        }
                    }
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="考核汇总" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            类别：<select id="type" class="easyui-combobox" style="width: 100px;">
                <option value="aa">机关单位</option>
                <option>基层单位</option>
            </select> 时间：<input id="selectKhsj" class="easyui-combobox"  style="width:80px;"  />  
            <a id="btnSearch" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" >查询</a>  
            <div id="divKhResult" style="overflow:auto;"></div>
        </div>
    </form>
</body>
</html>

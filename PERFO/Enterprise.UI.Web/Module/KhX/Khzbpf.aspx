<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Khzbpf.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.Khzbpf" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>评分</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
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
        $(function ()
        {
            $("#p").height($(document).height() - 100);
            $.ajax({
                url: "data/KhHandler.ashx",
                type: "post",
                data: { type: "getKhDetail", khid: '<%=khid%>',mbjgid:'<%=mbjgid%>',audit:'<%=audit%>' },
                success: function (result) {
                    if (result != "")
                        $("#p").html(result);
                }
            });
        });
        function Save(isSubmit)
        {
            isSubmit = isSubmit == null ? false : isSubmit;
            var submitStr = isSubmit ? "1" : "0";
            var pass = true;
            var json = "[";
            $("input[name='txtScore']").each(function () {
                if ($(this).val() == "") {
                    alert("请将分数打完整！");
                    pass = false;
                    return false;
                }
                json += "{name:'" + $(this).attr("id") + "',value:'" + $(this).val()
                    + "',memo:'" + $("#ta_" + $(this).attr("dddd")).val() + "',type:'" + $("#span_" + $(this).attr("dddd")).text() + "'},";
                //alert($("#ta_dddd" + $(this).attr("dddd")).html);
            });
            //document.write(json);
            if (pass) {
                json += "]";
                $.ajax({
                    url: "data/KhHandler.ashx",
                    type: "post",
                    data: { type: "saveKhfs", data: json, khid: '<%=khid%>', mbjgid: '<%=mbjgid%>', submit: submitStr },
                    success: function (result) {
                        if (result != "")
                            alert(result);
                        else
                        {
                            //location.reload();
                            document.location = "/Module/Kh/<%=back%>.aspx?khid=<%=khid%>";
                        }
                    }
                });
            }
        }
        function Submit()
        {
            Save(true);
        }

        function Cancel() {
            //alert("/Module/Kh/<%=back%>.aspx?khid=<%=khid%>&dwdm=<%=dwdm%>");
            document.location = "/Module/Kh/<%=back%>.aspx?khid=<%=khid%>&dwdm=<%=dwdm%>";
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="评分" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">            
        </div>
    </form>
</body>
</html>

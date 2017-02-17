<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongzhiView.aspx.cs" Inherits="Enterprise.UI.Web.Kh.TongzhiView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function aa() {
            if (confirm('您确定要删除数据?')) {
                return true;
            }
            return false;
        }
    </script>
    <style type="text/css">
        .label {
            display: inline;
            padding: .2em .6em .3em;
            font-size: 75%;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: .25em;
        }
        .label-info {
            background-color: #033c73;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="mk" class="easyui-panel" title="详细内容" collapsible="false" style="padding: 10px; background: #fafafa;">
            <asp:HiddenField ID="Hid_TZID" runat="server" />
            <div style="float: left">
                <asp:Panel ID="adminPanel" runat="server">
                    <asp:LinkButton ID="btn_edit" runat="server" CssClass="easyui-linkbutton"
                        iconCls="icon-edit" OnClick="btn_edit_Click">修改</asp:LinkButton>
                    <asp:LinkButton ID="btn_del" runat="server" CssClass="easyui-linkbutton"
                        iconCls="icon-cancel"
                        OnClientClick="return aa();" OnClick="btn_del_Click">删除</asp:LinkButton>
                </asp:Panel>
            </div>
            <div style="float: left; padding-left: 5px;">
                <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                    OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
            </div>
            <div style="padding-top: 20px;">
                <h3 style="text-align: center; font-size: 16px;"><%=ThisArticle.TZBT%></h3>
                <hr />
                <h5 style="text-align: center; font-size: 12px;">发布人：<%=ThisArticle.TZZZ%>&nbsp;&nbsp;
                    发布时间：<%=ThisArticle.TJRQ.ToDateYMDFormat()%>&nbsp;&nbsp;
                    浏览次数：<%=ThisArticle.LLCS.ToInt()%>次</h5>
                <p style="font-size: 14px;"><%=ThisArticle.TZNR%></p>
            </div>
            <br/>
            <hr/>
            <div style="float: left; padding-left: 5px;">
                <h5 style="text-align: center; font-size: 12px;">已浏览用户：</h5><%=GetViewInfo() %>
            </div>
        </div>
    </form>
</body>
</html>

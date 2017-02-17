<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="Enterprise.UI.Web.ChangePwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改口令</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Styles/gridview.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function ShowInfo(title, message) {
            $.messager.show({
                title: title,
                msg: message,
                showType: 'show'
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="修改密码" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <div id="content">
                <table id="roletable">
                    <tr>
                        <td>原密码</td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>新密码</td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>重复新密码</td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Btn_Save" runat="server" OnClick="Btn_Save_Click" Text="提交" />
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>

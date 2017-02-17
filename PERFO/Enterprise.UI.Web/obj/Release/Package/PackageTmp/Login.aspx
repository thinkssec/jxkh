<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Enterprise.UI.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户登录</title>
    <link href="Resources/Css/login2.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body>
    <div class="xlogin">
        <form id="form1" runat="server" defaultbutton="BtnLogin">
            <table class="loginp">
                <tr>
                    <td>
                        <table border="0" cellpadding="0">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">用&nbsp;户&nbsp;名：</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server" CssClass="username"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密&nbsp;码：</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                        ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">语言：
                                </td>
                                <td>
                                    <asp:DropDownList ID="Language" runat="server">
                                        <asp:ListItem Text="简体中文(zhcn)" Value="zhcn"></asp:ListItem>

                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2" style="color: Red;">
                                    <asp:LinkButton ID="BtnLogin" runat="server" OnClick="BtnLogin_Click" CssClass="button" CommandName="Login" ValidationGroup="Login1"
                                        ><span style="font-weight:bold">登录</span></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: Red;">
                                    <asp:Label ID="Lbl_Msg" runat="server" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: Red;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: Red;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: Red;">&nbsp;</td>
                            </tr>
                            <%--<tr>
                                <td align="left" colspan="2" style="color: gray;">天然气川气东送管道分公司
                                </td>
                            </tr>--%>

                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>

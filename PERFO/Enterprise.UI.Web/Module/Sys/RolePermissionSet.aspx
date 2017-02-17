<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Enterprise.UI.Web.Sys.RolePermissionSet" CodeBehind="RolePermissionSet.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>权限设置</title>
    <link href="/Resources/Css/Pstyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton"
                iconCls="icon-save" OnClick="LnkBtn_Upd_Click">保存</asp:LinkButton>
            <!--操作页面 开始 -->
            <div class="main">
                <div class="select">
                    角色：<asp:DropDownList ID="Roleddl" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="Roleddl_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div>
                    <table>
                        <asp:Repeater ID="rp_module" runat="server" OnItemDataBound="RP_Module_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td class="roottd">
                                        <asp:Label ID="Label1" runat="server"></asp:Label><%#Eval("MNAME") %>
                                    </td>
                                    <asp:Repeater ID="rp_pm" runat="server" OnItemDataBound="RP_PM_ItemDataBound">
                                        <ItemTemplate>
                                            <td class="childtd">
                                                <asp:CheckBox ID="chk_pm" runat="server" /><%#Eval("ACTIONNAME")%>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

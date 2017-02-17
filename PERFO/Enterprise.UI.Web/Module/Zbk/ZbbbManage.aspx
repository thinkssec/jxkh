<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZbbbManage.aspx.cs"
    Inherits="Enterprise.UI.Web.Zbk.ZbbbManage" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>指标版本管理</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function checkform() {
            return $("#form1").form('validate');
        }
        function aa() {
            if (confirm('您确定要删除数据？')) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="指标版本管理" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <asp:TextBox ID="Txt_ZBBB_Search" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Find" runat="server" Text="查询" class="easyui-linkbutton" plain="true"
                iconCls="icon-search" OnClick="Btn_Find_Click"></asp:LinkButton>
            &nbsp;&nbsp;<asp:LinkButton ID="LnkBtn_Ins" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>
            <div class="main-gridview">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                    CssClass="GridViewStyle" AllowPaging="true" PageSize="20" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                <%#(Container.DataItemIndex + 1) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BBMC" HeaderText="版本名称" ItemStyle-Width="100px"
                            ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="QYSJ" HeaderText="启用时间" ItemStyle-Width="100px" DataFormatString="{0:D}"
                            ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:TemplateField HeaderText="应用考核期" ItemStyle-Width="500px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("BBMC") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                                &nbsp;|&nbsp;
                                <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("BBMC") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa()" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                </asp:GridView>
            </div>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <div id="Div1" class="easyui-panel" title="编辑" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
                <asp:HiddenField ID="Hid_ZBID" runat="server" />
                <table>
                    <tr>
                        <td>版本名称</td>
                        <td>
                            <asp:TextBox ID="Txt_BBMC" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>版本状态</td>
                        <td>
                            <asp:CheckBox ID="Chk_QYSJ" runat="server" />
                        </td>
                    </tr>
                </table>
                <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton"
                    iconCls="icon-save" OnClientClick="return checkform();" OnClick="btn_save_Click">保存</asp:LinkButton>
                <asp:LinkButton ID="Button1" runat="server" class="easyui-linkbutton"
                    iconCls="icon-back" OnClick="Button1_Click">取消</asp:LinkButton>
            </div>
        </asp:Panel>

    </form>
</body>
</html>

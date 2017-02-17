<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BmjgManage.aspx.cs" Inherits="Enterprise.UI.Web.Sys.BmjgManage" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>机构管理</title>
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
        <div id="p" class="easyui-panel" title="机构管理" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <asp:TextBox ID="Txt_JGMC_Search" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:Button ID="Btn_Find" runat="server" Text="查询"
                OnClick="Btn_Find_Click" />&nbsp;&nbsp;<asp:Button ID="Btn_Add" runat="server" Text="新增"
                    OnClick="Btn_Add_Click" />&nbsp;&nbsp;
            <asp:RadioButtonList ID="Rdl_OrderBy" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Rdl_OrderBy_SelectedIndexChanged">
                <asp:ListItem Text="按层级" Value="按层级" Selected="True"></asp:ListItem>
                <asp:ListItem Text="按同级" Value="按同级"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                CssClass="GridViewStyle" AllowPaging="false">
                <Columns>
                    <asp:BoundField HeaderText="序号"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="JGMC" HeaderText="单位名称" ItemStyle-Width="300px"
                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="JGLX" HeaderText="机构类型" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:TemplateField HeaderText="是否考核">
                        <ItemTemplate>
                            <img alt="否" src="<%#(Eval("SFKH").ToRequestString()=="1")?"/Resources/Images/right.gif":"/Resources/Images/wrong.gif" %>" border="0" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa()" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="排序">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="up" ImageUrl="/Resources/OA/easyui1.32/themes/icons/up.gif" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="down" ImageUrl="/Resources/OA/easyui1.32/themes/icons/down.gif" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <HeaderStyle CssClass="GridViewHeaderStyle" />
            </asp:GridView>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <div id="Div1" class="easyui-panel" title="编辑" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
                <asp:HiddenField ID="Hid_JGBM" runat="server" />
                <table>
                    <tr>
                        <td>上级机构</td>
                        <td>
                            <asp:DropDownList ID="Ddl_XSXH" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>机构名称</td>
                        <td>
                            <asp:TextBox ID="Txt_JGMC" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>机构类型</td>
                        <td>
                            <asp:DropDownList ID="Ddl_JGLX" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>基层单位</asp:ListItem>
                                <asp:ListItem>职能部门</asp:ListItem>
                                <asp:ListItem>虚拟部门</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>是否考核</td>
                        <td>
                            <asp:CheckBox ID="Chk_SFKH" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>同级单位顺序号</td>
                        <td>
                            <asp:TextBox ID="Txt_BZ" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:LinkButton ID="btn_save" runat="server" class="easyui-linkbutton"
                    iconCls="icon-save" OnClientClick="return checkform();" OnClick="btn_save_Click">保存</asp:LinkButton>
                <asp:LinkButton ID="Button1" runat="server" class="easyui-linkbutton"
                    iconCls="icon-back" OnClick="Button1_Click">取消</asp:LinkButton>
            </div>
        </asp:Panel>

    </form>
</body>
</html>

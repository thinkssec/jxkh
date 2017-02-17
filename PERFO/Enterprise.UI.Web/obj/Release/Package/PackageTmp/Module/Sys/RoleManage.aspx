<%@ Page Language="C#" AutoEventWireup="true" Inherits="Enterprise.UI.Web.Sys.RoleManage" CodeBehind="RoleManage.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色管理</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <link href="/Resources/Css/Pstyle.css" rel="stylesheet" type="text/css" />
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
        <div id="p" class="easyui-panel" title="所有角色" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <div class="clear">
                <asp:Button ID="Btn_Add" runat="server" Text="新增" OnClick="Btn_Add_Click" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                    BorderWidth="0px" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Select" Width="65px" Height="65px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ROLENAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ROLENAME", "{0}") %>'></asp:Label>
                                <br />
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("ROLEID", "{0}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ROLEPICTURE" DataFormatString="/Resources/Images/role/{0}" />
                        <asp:BoundField DataField="ROLEID" DataFormatString="{0}" />
                    </Columns>
                    <SelectedRowStyle CssClass="rolerowselected" />
                    <HeaderStyle BorderStyle="None" BorderWidth="0px" />
                </asp:GridView>
            </div>
            <!--操作页面 开始 -->
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <div id="Div1" class="easyui-panel" title="角色操作" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa; overflow: hidden;">
                <table id="roletable">
                    <tr>
                        <td>角色编码</td>
                        <td>
                            <asp:TextBox ID="Txt_ROLEID" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" Height="14px" ImageUrl="/Resources/Images/hasuser.png" Visible="False" />
                        </td>
                        <td rowspan="6">
                            <asp:Image ID="txtimgshow" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>角色名称</td>
                        <td>
                            <asp:TextBox ID="Txt_ROLENAME" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>禁用</td>
                        <td>
                            <asp:CheckBox ID="Chk_ROLEDISABLE" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>图片</td>
                        <td>
                            <asp:DropDownList ID="Ddl_ROLEPICTURE" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Txtpicddl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton"
                                iconCls="icon-add" OnClientClick="return checkform();"
                                OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton"
                                iconCls="icon-save" OnClientClick="return checkform();"
                                OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton"
                                iconCls="icon-remove" OnClientClick="return aa();" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton"
                                iconCls="icon-back" OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </form>
</body>
</html>

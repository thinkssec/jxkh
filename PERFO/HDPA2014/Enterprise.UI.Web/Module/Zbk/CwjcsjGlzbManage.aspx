<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CwjcsjGlzbManage.aspx.cs" ValidateRequest="false"
    Inherits="Enterprise.UI.Web.Zbk.CwjcsjGlzbManage" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Import Namespace="Enterprise.Model.Perfo.Zbk" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>关联指标对应</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
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
        function alert(msg) {
            $.messager.alert('详细', msg, 'info');
        }
        function showInfo(tt, msg) {
            $.messager.show({
                title: tt,
                msg: msg,
                height: '280',
                width: '400',
                showType: 'show',
                timeout: 30000
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="财务基础数据与关联指标对应" icon="icon-save" collapsible="true"
            style="padding: 10px; background: #fafafa;">
            <asp:DropDownList ID="Ddl_CWZB_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_CWZB_Search_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:TextBox ID="Txt_ZBXMC_Search" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Find" runat="server" Text="查询" class="easyui-linkbutton" plain="true"
                iconCls="icon-search" OnClick="Btn_Find_Click"></asp:LinkButton>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Add" runat="server" Text="初始化" class="easyui-linkbutton" plain="true"
                iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound"
                OnRowCommand="GridView1_RowCommand" CssClass="GridViewStyle" AllowPaging="True"
                PageSize="15" OnPageIndexChanging="GridView1_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZBXMC" HeaderText="关联指标项">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>                    
                    <asp:BoundField DataField="JCSJZB" HeaderText="财务基础数据项">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="数据项类型">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BY1" HeaderText="备用1">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="BY2" HeaderText="备用2">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <PagerSettings FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PageButtonCount="5"
                    PreviousPageText="上一页" />
                <PagerStyle CssClass="GridViewPagerStyle" />
            </asp:GridView>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <div id="Div1" class="easyui-panel" title="编辑" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
                <asp:HiddenField ID="Hid_ID" runat="server" />
                <table>
                    <tr>
                        <td>关联指标项
                        </td>
                        <td>
                            <asp:Label ID="Lbl_ZBXMC" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>财务基础数据项
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_JCSJZB" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>数据类型
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_JCSJLX" runat="server">
                                <asp:ListItem Text="当年数据" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="上一年数据" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>备用1</td>
                        <td>
                            <asp:TextBox ID="Txt_BY1" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                   <tr>
                        <td>备用2</td>
                        <td>
                            <asp:TextBox ID="Txt_BY2" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                           <%-- <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                                OnClientClick="return checkform();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>--%>
                            <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                                OnClientClick="return checkform();" OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton" iconCls="icon-remove"
                                OnClientClick="return aa();" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                                OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </form>
</body>
</html>

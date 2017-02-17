<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZbManage.aspx.cs"
    Inherits="Enterprise.UI.Web.Zbk.ZbManage" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>指标管理</title>
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
        <div id="p" class="easyui-panel" title="指标管理" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            指标类型：
            <asp:DropDownList ID="Ddl_Zblx_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Zblx_Search_SelectedIndexChanged">
                <asp:ListItem Text="" Value=""></asp:ListItem>
                <asp:ListItem Text="定量指标" Value="定量指标"></asp:ListItem>
                <asp:ListItem Text="定性指标" Value="定性指标"></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;
            一级分类：
            <asp:DropDownList ID="Ddl_Yjzbmc_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Yjzbmc_Search_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:TextBox ID="Txt_ZBMC_Search" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Find" runat="server" Text="查询" class="easyui-linkbutton" plain="true"
                iconCls="icon-search" OnClick="Btn_Find_Click"></asp:LinkButton>
            &nbsp;&nbsp;<asp:LinkButton ID="LnkBtn_Ins" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                CssClass="GridViewStyle" AllowPaging="true" PageSize="15" OnPageIndexChanging="GridView1_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZBLX" HeaderText="指标类型" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="ZBMC" HeaderText="指标名称" ItemStyle-Width="200px"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="YJZBMC" HeaderText="一级分类" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="EJZBMC" HeaderText="二级分类" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="SJZBMC" HeaderText="三级分类" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:TemplateField HeaderText="指标状态">
                        <ItemTemplate>
                            <img alt="无效" src="<%#(Eval("ZBZT").ToRequestString()=="1")?"/Resources/Images/right.gif":"" %>" border="0" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TJRQ" HeaderText="添加日期" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:D}"></asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("ZBID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ZBID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa()" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument='<%#Eval("ZBID") %>' CommandName="up" ImageUrl="/Resources/OA/easyui1.32/themes/icons/up.gif" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument='<%#Eval("ZBID") %>' CommandName="down" ImageUrl="/Resources/OA/easyui1.32/themes/icons/down.gif" />
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
                <asp:HiddenField ID="Hid_ZBID" runat="server" />
                <table>
                    <tr>
                        <td>指标类型</td>
                        <td>
                            <asp:DropDownList ID="Ddl_ZBLX" runat="server">
                                <asp:ListItem>定量指标</asp:ListItem>
                                <asp:ListItem>定性指标</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>一级分类名称</td>
                        <td>
                            <asp:TextBox ID="Txt_YJZBMC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>二级分类名称</td>
                        <td>
                            <asp:TextBox ID="Txt_EJZBMC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>三级分类名称</td>
                        <td>
                            <asp:TextBox ID="Txt_SJZBMC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>指标名称</td>
                        <td>
                            <asp:TextBox ID="Txt_ZBMC" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>指标状态</td>
                        <td>
                            <asp:CheckBox ID="Chk_ZBZT" runat="server" Checked="true" />
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

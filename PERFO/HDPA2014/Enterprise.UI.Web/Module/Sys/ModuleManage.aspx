<%@ Page Language="C#" AutoEventWireup="true" Inherits="Enterprise.UI.Web.Sys.ModuleManage" CodeBehind="ModuleManage.aspx.cs" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模块管理</title>
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
        <div id="Div2" class="easyui-panel" title="模块列表" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
           <%-- <div class="main-gridview">
                <table style="width: 100%; text-align: center;" id="Table2">
                    <tr>
                        <th style="width: 50px;" class="td-bold">选择</th>
                        <th class="td-bold">模块编码</th>
                        <th class="td-bold">模块名称</th>
                        <th class="td-bold">模块地址</th>
                        <th class="td-bold">上级模块编码</th>
                        <th class="td-bold">禁止显示</th>
                        <th class="td-bold">备注</th>
                        <th class="td-bold">排序</th>
                    </tr>
                    <tr style="text-align: left;">
                        <td>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="/Resources/Images/select.gif" /></td>
                        <td>10</td>
                        <td>系统管理</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>1</td>
                    </tr>
                    <tr style="text-align: left;">
                        <td>
                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="/Resources/Images/select.gif" /></td>
                        <td style="text-align: left;">1010</td>
                        <td style="text-align: left;">╘══模块管理</td>
                        <td>Manage/ModuleManage.aspx</td>
                        <td>10</td>
                        <td></td>
                        <td></td>
                        <td>001</td>
                    </tr>
                    <tr style="text-align: left;">
                        <td>
                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="/Resources/Images/select.gif" /></td>
                        <td style="text-align: left;">1010</td>
                        <td style="text-align: left;">╘══机构管理</td>
                        <td>Manage/BmjgManage.aspx</td>
                        <td>10</td>
                        <td></td>
                        <td></td>
                        <td>002</td>
                    </tr>
                </table>
            </div>--%>
            <asp:Button ID="Btn_Add" runat="server" Text="新增" OnClick="Btn_Add_Click" />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                CssClass="GridViewStyle" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="选择">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="/Resources/Images/select.gif" />
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="MID" HeaderText="模块编码" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="MNAME" HeaderText="模块名称" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="MURL" HeaderText="模块地址"  ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WEBURL" HeaderText="WEB映射"  ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="MPARENTID" HeaderText="上级模块编码" />
                    <asp:TemplateField HeaderText="禁止显示">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Convert.ToBoolean(Eval("DISABLE").ToInt()) %>' Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BZ" HeaderText="备注" />
                    <asp:BoundField DataField="XSXH" HeaderText="排序" />
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" class="popwin" Visible="false">
            <asp:HiddenField ID="Hid_MID" runat="server" />
            <div id="Div1" class="easyui-panel" title="模块操作" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa; overflow: hidden;">
                <table>
                    <tr>
                        <td>模块名称</td>
                        <td>
                            <asp:TextBox ID="Txt_MNAME" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>所属模块</td>
                        <td>
                            <asp:DropDownList ID="Ddl_MPARENTID" runat="server" CssClass="easyui-validatebox" required="true" missingMessage="请选择所属模块">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>模块地址</td>
                        <td>
                            <asp:TextBox ID="Txt_MURL" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>WEB映射地址</td>
                        <td>
                            <asp:TextBox ID="Txt_WEBURL" runat="server" class="easyui-validatebox" required="true" missingMessage="必填项" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>禁止显示</td>
                        <td>
                            <asp:CheckBox ID="Chk_DISABLE" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>备注</td>
                        <td>
                            <asp:TextBox ID="Txt_BZ" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>排序</td>
                        <td>
                            <asp:TextBox ID="Txt_XSXH" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton"
                                iconCls="icon-add" OnClientClick="return checkform();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
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

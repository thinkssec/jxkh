<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsgsbdsManage.aspx.cs" ValidateRequest="false"
    Inherits="Enterprise.UI.Web.Zbk.JsgsbdsManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>计算规则维护</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function checkform() {
            var bds = encodeURIComponent($('#Txt_BDS').val());
            //$('#Txt_BDS').val("");
            $('#Hid_GZBDS').val(bds);
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
        <div id="p" class="easyui-panel" title="计算规则维护" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            <asp:DropDownList ID="Ddl_BBMC_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_BBMC_Search_SelectedIndexChanged">
            </asp:DropDownList>&nbsp;&nbsp;
            <asp:TextBox ID="Txt_Jsgz_Search" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Find" runat="server" Text="查询" class="easyui-linkbutton" plain="true"
                iconCls="icon-search" OnClick="Btn_Find_Click"></asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="LnkBtn_Add" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                iconCls="icon-add" OnClick="LnkBtn_Add_Click"></asp:LinkButton>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView2_RowCommand"
                OnRowDataBound="GridView2_RowDataBound" AllowPaging="true" PageSize="15" OnPageIndexChanging="GridView2_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="GZMC" HeaderText="规则名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="BBMC" HeaderText="应用版本">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="GZBDS" HeaderText="表达式">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="METHODNAME" HeaderText="方法名称">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MAXV" HeaderText="封顶值">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MINV" HeaderText="保底值">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UPPER" HeaderText="上浮程度值" DataFormatString="{0:P}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LOWER" HeaderText="下浮程度值" DataFormatString="{0:P}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("GZID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("GZID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <div id="Div1" class="easyui-panel" title="编辑" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
                <asp:HiddenField ID="Hid_GZID" runat="server" />
                <table runat="server" id="roletable">
                    <tr>
                        <td>规则名称
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_GZMC" runat="server" class="easyui-validatebox" required="true"
                                missingMessage="必填项" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>应用版本
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_BBMC" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>表达式
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_BDS" runat="server" Width="400px"></asp:TextBox>
                            <asp:HiddenField ID="Hid_GZBDS" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>方法名称
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_METHODNAME" runat="server" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>封顶值
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_MAXV" runat="server" class="easyui-numberbox" max="999" precision="2"></asp:TextBox>(最大3位整数 保留2位小数)
                        </td>
                    </tr>
                    <tr>
                        <td>保底值
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_MINV" runat="server" class="easyui-numberbox" max="999" precision="2"></asp:TextBox>(最大3位整数 保留2位小数)
                        </td>
                    </tr>
                    <tr>
                        <td>上浮程度值
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_UPPER" runat="server" class="easyui-numberbox" max="9" precision="2"></asp:TextBox>(0-9之间,保留2位小数)*100%
                        </td>
                    </tr>
                    <tr>
                        <td>下浮程度值
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_LOWER" runat="server" class="easyui-numberbox" max="9" precision="2"></asp:TextBox>(0-9之间,保留2位小数)*100%
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                                OnClientClick="return checkform();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                                OnClientClick="return checkform();" OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton" iconCls="icon-remove"
                                OnClientClick="return aa();" Visible="false" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>
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

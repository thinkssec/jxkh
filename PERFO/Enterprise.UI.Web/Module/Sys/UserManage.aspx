<%@ Page Language="C#" AutoEventWireup="true" Inherits="Enterprise.UI.Web.Sys.UserManage"
    CodeBehind="UserManage.aspx.cs" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户管理</title>
    <link rel="stylesheet" type="text/css" href="/Resources/Css/Pstyle.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
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
        <div>
            <div id="p" class="easyui-panel" title="所有用户" icon="icon-save" collapsible="true"
                style="padding: 10px; background: #fafafa;">
                <asp:TextBox ID="Txt_User_Search" runat="server"></asp:TextBox>
                &nbsp;&nbsp;<asp:Button ID="Btn_Find" runat="server" Text="查询" OnClick="Btn_Find_Click" />&nbsp;&nbsp;
                <asp:Button ID="Btn_Add" runat="server" Text="新增" OnClick="Btn_Add_Click" />
                <asp:ListView ID="ListView1" runat="server" DataKeyNames="LOGINID" GroupItemCount="6"
                    OnItemDataBound="ListView1_ItemDataBound" OnSelectedIndexChanged="ListView1_SelectedIndexChanged"
                    OnSelectedIndexChanging="ListView1_SelectedIndexChanging"
                    OnPagePropertiesChanging="ListView1_PagePropertiesChanging">
                    <EmptyItemTemplate>
                        <td runat="server" />
                    </EmptyItemTemplate>
                    <ItemTemplate>
                        <td runat="server" class="usertd">
                            <div class="userrolepic">
                                <asp:Image ID="lbpic" runat="server" Width="50px" Height="50px" />
                            </div>
                            <div class="rolename">
                                <asp:Label ID="lbrolename" runat="server" Text='<%# Eval("ROLEID", "{0}") %>'></asp:Label>
                            </div>
                            <div class="userdisable">
                                <asp:Image ID="lbdisable" runat="server" Width="20px" Height="20px" ImageUrl='<%# Eval("DISABLE", "{0}") %>' />
                            </div>
                            <div class="userloginid">
                                <asp:LinkButton ID="lbloginid" runat="server" Text='<%# Eval("LOGINID", "{0}") %>'
                                    CommandName="select"></asp:LinkButton>
                            </div>
                            <div class="username">
                                <div style="padding: 2px;">
                                    <asp:Label ID="lbjgmc" runat="server"></asp:Label>
                                </div>
                                <div style="padding: 2px;">
                                    <asp:Label ID="lbusername" runat="server" Text='<%# Eval("USERNAME", "{0}") %>'></asp:Label>
                                </div>
                            </div>
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table runat="server" style="">
                            <tr>
                                <td>No Data
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table id="groupPlaceholderContainer" runat="server" border="0" style="">
                                        <tr id="groupPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <GroupTemplate>
                        <tr id="itemPlaceholderContainer" runat="server">
                            <td id="itemPlaceholder" runat="server"></td>
                        </tr>
                    </GroupTemplate>
                    <SelectedItemTemplate>
                        <td id="Td1" runat="server" class="usertdselect">
                            <div class="userrolepic">
                                <asp:Image ID="lbpic" runat="server" />
                            </div>
                            <div class="rolename">
                                <asp:Label ID="lbrolename" runat="server"></asp:Label>
                            </div>
                            <div class="userdisable">
                                <asp:Image ID="lbdisable" runat="server" ImageUrl='<%# Eval("DISABLE", "{0}") %>' />
                            </div>
                            <div class="userloginid">
                                <asp:LinkButton ID="lbloginid" runat="server" Text='<%# Eval("LOGINID", "{0}") %>'
                                    CommandName="select"></asp:LinkButton>
                            </div>
                            <div class="username">
                                <div style="padding: 2px;">
                                    <asp:Label ID="lbjgmc" runat="server"></asp:Label>
                                </div>
                                <div style="padding: 2px;">
                                    <asp:Label ID="lbusername" runat="server" Text='<%# Eval("USERNAME", "{0}") %>'></asp:Label>
                                </div>
                            </div>
                        </td>
                    </SelectedItemTemplate>
                </asp:ListView>
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="25">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False"
                            ShowPreviousPageButton="False" FirstPageText="首页" LastPageText="末页" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False"
                            ShowPreviousPageButton="False" FirstPageText="首页" LastPageText="末页" />
                    </Fields>
                </asp:DataPager>
            </div>

            <!--操作页面 开始 -->
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div id="Div1" class="easyui-panel" title="用户操作" icon="icon-save"
                    collapsible="true" style="padding: 10px; background: #fafafa;">
                    <asp:HiddenField ID="Hid_LOGINID" runat="server" />
                    <table id="roletable">
                        <tr>
                            <td>登录名称
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_LOGINID" runat="server" class="easyui-validatebox" required="true"
                                    missingMessage="必填项"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>姓名
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_USERNAME" runat="server" class="easyui-validatebox" required="true"
                                    missingMessage="必填项"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>口令
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_PASSWORD" TextMode="Password" runat="server" class="easyui-validatebox"
                                    required="true" missingMessage="必填项"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>禁用
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_DISABLE" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <table id="Table1">
                                    <tr>
                                        <td>所属单位
                                        </td>
                                        <td>分管机构
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:RadioButtonList ID="Rdl_JGBM" runat="server" RepeatDirection="Vertical">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="Chk_FGJGBM" runat="server" RepeatDirection="Vertical">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>所属角色
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_ROLEID" runat="server" CssClass="easyui-validatebox" required="true"
                                    missingMessage="请选择">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>职务
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_DUTY" runat="server" CssClass="easyui-validatebox" required="true"
                                    missingMessage="请选择">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>打分人员</asp:ListItem>
                                    <asp:ListItem>分公司领导</asp:ListItem>
                                    <asp:ListItem>基层单位领导</asp:ListItem>
                                    <asp:ListItem>职能部门负责人</asp:ListItem>
                                    <asp:ListItem>员工</asp:ListItem>
                                    <asp:ListItem>四级单位</asp:ListItem>
                                    <asp:ListItem>基层机关</asp:ListItem>
                                    <asp:ListItem>分管领导</asp:ListItem>
                                    <asp:ListItem>专业部门分管领导</asp:ListItem>
                                    <asp:ListItem>绩效考核办公室</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>IP地址
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_IPADDR" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                                    OnClientClick="return checkform();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
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
        </div>
    </form>
</body>
</html>

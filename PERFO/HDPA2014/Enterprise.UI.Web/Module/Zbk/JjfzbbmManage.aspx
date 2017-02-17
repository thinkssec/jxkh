<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JjfzbbmManage.aspx.cs" ValidateRequest="false"
    Inherits="Enterprise.UI.Web.Zbk.JjfzbbmManage" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Import Namespace="Enterprise.Model.Perfo.Zbk" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>打分指标维护</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/gridview.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function checkform() {
            //var bds = encodeURIComponent($('#Txt_BDS').val());
            ////$('#Txt_BDS').val("");
            //$('#Hid_DFBDS').val(bds);
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
            //msg = decodeURI(msg);
            //msg = decodeURIComponent(msg);
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
        <div id="p" class="easyui-panel" title="打分指标维护" icon="icon-save" collapsible="true"
            style="padding: 10px; background: #fafafa;">
            <asp:DropDownList ID="Ddl_BBMC_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_BBMC_Search_SelectedIndexChanged">
            </asp:DropDownList>&nbsp;&nbsp;
            <asp:DropDownList ID="Ddl_Pflx_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Pflx_Search_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>加分+</asp:ListItem>
                <asp:ListItem>减分-</asp:ListItem>
                <asp:ListItem>加减分±</asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;
            <asp:TextBox ID="Txt_Dfzb_Search" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Find" runat="server" Text="查询" class="easyui-linkbutton" plain="true"
                iconCls="icon-search" OnClick="Btn_Find_Click"></asp:LinkButton>
            &nbsp;&nbsp;<asp:LinkButton ID="Btn_Add" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound"
                OnRowCommand="GridView1_RowCommand" CssClass="GridViewStyle" AllowPaging="True"
                PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标名称" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <%#((ZbkZbxxModel)Eval("Zbxx")).ZBMC %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="分类名称">
                        <ItemTemplate>
                            <%#string.Format("{0}※{1}※{2}",((ZbkZbxxModel)Eval("Zbxx")).YJZBMC,((ZbkZbxxModel)Eval("Zbxx")).EJZBMC,((ZbkZbxxModel)Eval("Zbxx")).SJZBMC) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BBMC" HeaderText="版本名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PFLX" HeaderText="评分类型">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="JXFZ" HeaderText="极限分值">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="否决项">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Convert.ToBoolean(Eval("SFFJX").ToInt()) %>' Enabled="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="打分者">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="评分标准" HtmlEncode="False" HtmlEncodeFormatString="False">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="是否禁用">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("DISABLE").ToInt()) %>' Enabled="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="计算规则">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("DFZBBM") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("DFZBBM") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
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
                <asp:HiddenField ID="Hid_DFZBBM" runat="server" />
                <asp:HiddenField ID="Hid_ZBID2" runat="server" />
                <table>
                    <tr>
                        <td>指标名称
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_ZBID" runat="server"></asp:DropDownList>
                            <asp:Label ID="Lbl_ZBMC" runat="server"></asp:Label>
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
                        <td>评分类型
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_PFLX" runat="server">
                                <asp:ListItem>加分+</asp:ListItem>
                                <asp:ListItem>减分-</asp:ListItem>
                                <asp:ListItem>加减分±</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>极限分值
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_JXFZ" runat="server" class="easyui-numberbox" required="true"
                                min="0" max="100" precision="1" missingMessage="必填数字(0~100)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>否决项
                        </td>
                        <td>
                            <asp:CheckBox ID="Chk_SFFJX" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>机构设置</td>
                        <td>
                            <a href="#" onclick="javascript:$('#shezhi_bkhjg').toggle();">点击设置</a>
                        </td>
                    </tr>
                    <tr id="shezhi_bkhjg" style="display: none">
                        <td colspan="2">
                            <table style="width: 100%; text-align: center;" id="Table2">
                                <tr>
                                    <th class="td-bold">被考核机构</th>
                                    <th class="td-bold">打分机构</th>
                                </tr>
                                <tr align="left" valign="top">
                                    <td>
                                        <asp:CheckBoxList ID="Chk_BKHJG" runat="server">
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="Chk_DFZ" runat="server">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>评分标准</td>
                        <td>
                            <FCKeditorV2:FCKeditor ID="Txt_PFBZ" runat="server" Width="468px" Height="160px" ToolbarSet="Basic">
                            </FCKeditorV2:FCKeditor>
                            <%--<asp:TextBox ID="Txt_PFBZ" runat="server" Width="468px" Height="160px" TextMode="MultiLine"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>是否禁用</td>
                        <td>
                            <asp:CheckBox ID="Chk_DISABLE" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>计算规则
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_GZID" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
<%--                    <tr>
                        <td>表达式</td>
                        <td>
                            <asp:TextBox ID="Txt_BDS" runat="server" Width="400px"></asp:TextBox>
                            <asp:HiddenField ID="Hid_DFBDS" runat="server" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td>最大值</td>
                        <td>
                            <asp:TextBox ID="Txt_MAXV" runat="server" class="easyui-numberbox" max="999" precision="1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>最小值</td>
                        <td>
                            <asp:TextBox ID="Txt_MINV" runat="server" class="easyui-numberbox" max="999" precision="1"></asp:TextBox>
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
    </form>
</body>
</html>

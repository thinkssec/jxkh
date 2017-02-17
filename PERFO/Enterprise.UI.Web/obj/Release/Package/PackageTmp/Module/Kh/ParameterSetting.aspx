<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="ParameterSetting.aspx.cs" Inherits="Enterprise.UI.Web.Kh.ParameterSetting" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.alert(tt, msg);
        }
        //检测输入值为数字
        function regInput(obj, inputStr) {
            var reg = /^-?\d*\.?\d{0,2}$/;
            var str = inputStr;
            return reg.test(str);
        }
        function aa() {
            if (confirm('删除不可恢复!您确定要删除数据？')) {
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>考核参数设置</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<Ent:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                <asp:HiddenField ID="Hid_TabTitle" runat="server" />
            </div>
            <div id="tt" class="easyui-tabs" style="width: auto;">
                <div title="经营难度系数" data-options="iconCls:'icon-cog'" style="padding: 4px">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="GridView1_RowDataBound" AllowPaging="false" DataKeyNames="JGBM,KHID">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="归属单位">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="单位名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="机构类型">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="考核类型">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="难度系数">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                        <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                            OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="Chk_InitNdxs" runat="server" Text="系数初始化" />
                    </div>
                </div>
                <div title="成绩区间设置" data-options="iconCls:'icon-cog'" style="padding: 4px">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="GridView2_RowDataBound" OnRowCommand="GridView2_RowCommand" AllowPaging="false" DataKeyNames="KHID">
                            <Columns>
                                <asp:TemplateField HeaderText="区间等级">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="最大值">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="最小值">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("QJDJ") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                        <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                            OnClick="LnkBtn_Upd_Click">保存</asp:LinkButton>
                    </div>
                </div>
                <div title="排名范围设置" data-options="iconCls:'icon-cog'" style="padding: 4px">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView3" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowCommand="GridView3_RowCommand" OnRowDataBound="GridView3_RowDataBound" AllowPaging="false" DataKeyNames="KHID">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="排名范围">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="考核对象">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server"
                                            CommandArgument='<%#Eval("ID") %>' CommandName="shanchu"
                                            ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                        <asp:Panel ID="Pnl_DfpxfwEdit" runat="server">
                            <div id="contents" class="ssec-form">
                                <table>
                                    <tr>
                                        <td>考核名称</td>
                                        <td>
                                            <asp:Label ID="Lbl_KHMC" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>排名范围选择
                                        </td>
                                        <td>
                                            <asp:CheckBoxList RepeatDirection="Horizontal" ID="ChkBox_GSDW" runat="server">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>考核对象选择
                                        </td>
                                        <td>
                                            <asp:RadioButtonList RepeatDirection="Horizontal" ID="Rdl_KHDX" runat="server">
                                                <asp:ListItem Text="基层单位" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="领导班子" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:LinkButton ID="LnkBtn_Dfpxfw" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                                    OnClick="LnkBtn_Dfpxfw_Click">保存</asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div title="合并计分规则" data-options="iconCls:'icon-cog'" style="padding: 4px">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView4" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="GridView4_RowDataBound" OnRowCommand="GridView4_RowCommand" AllowPaging="false" DataKeyNames="KHID">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="合并计分名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="合并计分单位">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="配置公式名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("HBJFID") %>' 
                                            CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="ImageButton2" runat="server"
                                            CommandArgument='<%#Eval("HBJFID") %>' CommandName="shanchu"
                                            ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                        <asp:Panel ID="Pnl_HbjsEdit" runat="server">
                            <div id="Div2" class="ssec-form">
                                <table>
                                    <tr>
                                        <td>考核名称</td>
                                        <td>
                                            <asp:Label ID="Lbl_KHMC2" runat="server"></asp:Label>
                                            <asp:HiddenField ID="Hid_HBJFID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>合并规则名称</td>
                                        <td>
                                            <asp:TextBox ID="Txt_HBJFMC" runat="server" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>合并计分单位
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="ChkBox_HBJFDW" runat="server">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>计算规则</td>
                                        <td>
                                            <asp:DropDownList ID="Ddl_GZID" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:LinkButton ID="LnkBtn_Hbjsgz" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                                    OnClick="LnkBtn_Hbjsgz_Click">保存</asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <script type="text/javascript">
        $('#tt').tabs({
            onSelect: function (title, index) {
                $('#tt').tabs('getTab', index).show();
                $('#<%=Hid_TabTitle.ClientID%>').val(title);
            }
        });
        $(function () {
            //tabs
            $('#tt').tabs('select', '<%=TabTitle%>');
        });
    </script>
</asp:Content>

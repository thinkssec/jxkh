<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KhbzAudit2.aspx.cs" Inherits="Enterprise.UI.Web.Kh.KhbzAudit2" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
           
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.alert(tt, msg);
        }
        function aa() {
            if (confirm('删除不可恢复!您确定要删除本次所有数据？')) {
                return true;
            }
            return false;
        }
        function tb() {
            if (confirm('您确定要重新进行考核指标和单位的数据同步？（只能追加）')) {
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <!--IPHONE Style-->
    <link href='/Resources/charisma/css/jquery.iphone.toggle.css' rel='stylesheet' />
    <script src="/Resources/charisma/js/jquery.iphone.toggle.js"></script>
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>评分标准审核</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">

                <%--<asp:LinkButton ID="Btn_Add" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                    iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>--%>
                <asp:LinkButton ID="LinkButton2" runat="server" Text="通过" class="easyui-linkbutton" plain="true"
                    iconCls="icon-ok" OnClick="LnkBtn_Yes_Click"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" runat="server" Text="不通过" class="easyui-linkbutton" plain="true"
                    iconCls="icon-cancel" OnClick="LnkBtn_No_Click"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="LnkBtn_Cancel_Click"></asp:LinkButton>
                <asp:HiddenField ID="Hid_KHDZBID" runat="server" />
                <asp:Label ID="Lb_Title" runat="server"></asp:Label>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                PageSize="15">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NAME" HeaderText="考核标准名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MEMO" HeaderText="备注">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:TemplateField HeaderText="考核项目">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="允许查询">
                        <ItemTemplate>
                            <img alt="否" src="<%#(Eval("SFKC").ToRequestString()=="1")?"/Resources/Images/right.gif":"/Resources/Images/wrong.gif" %>" border="0" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("ID") %>'
                                CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" ToolTip="编辑" />
                             &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ID") %>'
                                CommandName="khnr" ImageUrl="/Resources/Styles/icon/b.gif" ToolTip="考核内容" />
<%--                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ID") %>'
                                CommandName="gbzt" ImageUrl="/Resources/Styles/icon/b.gif" ToolTip="改变状态" />--%>
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
            <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div id="contents" class="ssec-form">
                    <div class="ssec-group ssec-group-hasicon">
                        <div class="icon-form"></div>
                        <span>编辑</span>
                    </div>
                    <asp:HiddenField ID="Hid_ID" Value="" runat="server" />
                    <table>
                        <tr>
                            <td>考核标准名称
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_NAME" runat="server" class="easyui-validatebox" required="true"
                                    missingMessage="必填项" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>备注
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_MEMO" runat="server" class="easyui-validatebox"
                                    missingMessage="必填项" Width="300px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>考核对象
                            </td>
                            <td>
                                <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" 
                                    CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="JGBM" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="序号">
                                            <ItemTemplate>
                                                <%#(Container.DataItemIndex + 1) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                        </asp:TemplateField>
                    <%--                    <asp:TemplateField HeaderText="考核项目名称">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>--%>
   
                                        <%--<asp:BoundField DataField="ZBMC" HeaderText="考核项目名称">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="部门名称">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Cb_DZB" runat="server" /><%#Eval("JGMC") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="排序">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_PX" runat="server" class="easyui-numberbox"  min="0"
                                    max="999"  missingMessage="必须填写数字" Width="30" precision="1"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <%--<asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton" iconCls="icon-remove"
                        OnClientClick="return aa();" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>--%>
                    <%--<asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back" OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>--%>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('.iphone-toggle').iphoneStyle();
        });
    </script>
</asp:Content>

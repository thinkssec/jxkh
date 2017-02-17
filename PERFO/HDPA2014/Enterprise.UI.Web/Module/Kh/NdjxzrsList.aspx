<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="NdjxzrsList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.NdjxzrsList" %>

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
                <span>年度绩效考核责任书</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核年度：<asp:DropDownList ID="Ddl_Niandu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Niandu_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                考核类型：
                <asp:DropDownList ID="Ddl_LXID_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_LXID_Search_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                单位：<asp:DropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                    iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                PageSize="10">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="责任书名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="JGBM" HeaderText="单位名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="考核类型">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SZND" HeaderText="考核年度">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FZBM" HeaderText="负责部门">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ZRSFJ" HeaderText="附件">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="指标数量">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZRSZT" HeaderText="责任书状态">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="XDRQ" HeaderText="下达日期" DataFormatString="{0:yyyy-MM-dd}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("ZRSID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ZRSID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument='<%#Eval("ZRSID") %>' CommandName="xiada" ImageUrl="~/Resources/themes/icons/zbgl.png" ToolTip="下达" />
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
            <div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                </div>
            </div>
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div id="contents" class="ssec-form">
                    <div class="ssec-group ssec-group-hasicon">
                        <div class="icon-form"></div>
                        <span>编辑</span>
                    </div>
                    <asp:HiddenField ID="Hid_ZRSID" Value="" runat="server" />
                    <table>
                        <tr>
                            <td>责任书名称
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_ZRSMC" runat="server" class="easyui-validatebox" required="true"
                                    missingMessage="必填项" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>考核年度
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_SZND" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>考核类型
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_LXID" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>单位名称
                            </td>
                            <td>
                                <Ent:SSECDropDownList ID="Ddl_JGBM" runat="server">
                                </Ent:SSECDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>负责部门
                            </td>
                            <td>
                                <Ent:SSECDropDownList ID="Ddl_FZBM" runat="server">
                                </Ent:SSECDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>责任书附件
                            </td>
                            <td>
                                <UC:PopWinUploadMuti runat="server" ID="Txt_ZRSFJ" Muti="false" Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td>责任书状态
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_ZRSZT" runat="server">
                                    <asp:ListItem Text="制定中" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="已下达" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton" iconCls="icon-remove"
                        OnClientClick="return aa();" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back" OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

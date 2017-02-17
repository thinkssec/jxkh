<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KaoheZdhbList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.KaoheZdhbList" %>

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>考核制度汇编-列表</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                <asp:TextBox ID="Txt_Wjmc_Search" runat="server"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Find" runat="server" class="easyui-linkbutton" iconCls="icon-search" plain="true"
                    OnClick="LnkBtn_Find_Click">查询</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                    iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                PageSize="15">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="WJMC" HeaderText="文件名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="考核类型">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="文件下载">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="在线浏览">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TJRQ" HeaderText="发布日期" DataFormatString="{0:yyyy-MM-dd}">
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("WJID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("WJID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
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
                    <asp:HiddenField ID="Hid_WJID" Value="" runat="server" />
                    <table>
                        <tr>
                            <td>制度名称</td>
                            <td>
                                <asp:TextBox ID="Txt_WJMC" runat="server" class="easyui-validatebox" required="true"
                                    missingMessage="必填项" Width="300px"></asp:TextBox>
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
                            <td>考核制度附件
                            </td>
                            <td>
                                <UC:PopWinUploadMuti runat="server" ID="Txt_WJFJ" Muti="false" Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td>在线浏览附件
                            </td>
                            <td>
                                <UC:PopWinUploadMuti runat="server" ID="Txt_ZXLL" Muti="false" Width="300px" />
                            </td>
                        </tr>
                    </table>
                    <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                        OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                        OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

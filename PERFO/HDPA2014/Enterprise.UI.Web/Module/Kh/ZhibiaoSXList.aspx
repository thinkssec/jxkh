<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="ZhibiaoSXList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.ZhibiaoSXList" %>

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
        function bb() {
            if (confirm('您确定要正式开始本次考核吗？考核发起后数据会初始化！')) {
                return true;
            }
            return false;
        }
        function cc() {
            if (confirm('您确定要追加指标吗？原有考核指标的得分可能需要重新计算！')) {
                return true;
            }
            return false;
        }
        function dd() {
            if (confirm('您确定要重新发起该单位的考核吗？该单位下的有原有数据会全部初始化！')) {
                return true;
            }
            return false;
        }
        function ee() {
            if (confirm('您确定要初始化该单位的财务基础数据表吗？考核年度和上一年度的数据会被清空！')) {
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
                <span>二级单位考核指标筛选</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<Ent:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;
                单位：<Ent:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="维护" class="easyui-linkbutton" plain="true"
                    iconCls="icon-save" OnClick="Btn_Add_Click"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_FQKH" runat="server" ToolTip="发起所选单位及其下级单位的正式考核" Text="发起考核" class="easyui-linkbutton" plain="true"
                    iconCls="icon-jiaofu" OnClick="LnkBtn_FQKH_Click" OnClientClick="return bb();"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Append" runat="server" ToolTip="为所选单位及其下级单位追加考核指标" Text="追加指标" class="easyui-linkbutton" plain="true"
                    iconCls="icon-add" OnClick="LnkBtn_Append_Click" OnClientClick="return cc();"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Remove" runat="server" ToolTip="清除所选单位及其下级单位的考核指标数据" Text="清除考核数据" class="easyui-linkbutton" plain="true"
                    iconCls="icon-cancel" OnClientClick="return aa();" OnClick="LnkBtn_Remove_Click"></asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" AllowPaging="false">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
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
                    <asp:TemplateField HeaderText="考核年度">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标数量">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="权重合计">
                        <ItemTemplate>
                            </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                         <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" ToolTip="修改该单位下的筛选指标" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" ToolTip="删除该单位下的筛选指标" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton3" ToolTip="只为该单位向已发起的考核中追加指标" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="append" ImageUrl="/Resources/Common/img/icon/edit.gif" OnClientClick="return cc();" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton4" ToolTip="重新发起该单位的本次考核(其数据会全部初始化)" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="faqi" ImageUrl="/Resources/Common/img/icon/jiaofu.png" OnClientClick="return dd();" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton5" ToolTip="初始化财务基础数据表" runat="server" CommandArgument='<%#Eval("JGBM") %>' CommandName="caiwu" ImageUrl="/Resources/Common/img/icon/list.gif" OnClientClick="return ee();" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
            </asp:GridView>
            <div class="msg-info">
		        <div class="msg-tip icon-tip"></div>
		        <div><asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;</div>
	        </div>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="NdjxzrsZbList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.NdjxzrsZbList" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Register Src="~/Component/UserControl/PopWinUploadMuti.ascx" TagPrefix="uc1" TagName="PopWinUploadMuti" %>
<%@ Register Assembly="Enterprise.Component.Infrastructure" Namespace="Enterprise.Component.Infrastructure" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.alert(tt, msg);
            //$.messager.show(
            //    {
            //        title: tt,
            //        msg: msg,
            //        height: '200',
            //        showType: 'show'
            //    });
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
                <span>年度绩效考核责任书-指标列表</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核年度：<asp:DropDownList ID="Ddl_Niandu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Niandu_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                单位：<cc1:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </cc1:SSECDropDownList>
                &nbsp;&nbsp;
                应用版本：<asp:DropDownList ID="Ddl_Bbmc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Bbmc_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="维护" class="easyui-linkbutton" plain="true"
                    iconCls="icon-save" OnClick="Btn_Add_Click"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Back" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="Btn_Back_Click"></asp:LinkButton>
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
                    <asp:BoundField DataField="ZRSID" HeaderText="责任书名称" Visible="false">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="指标类别">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZZBXZ" HeaderText="指标性质">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="BBMC" HeaderText="版本名称" Visible="false">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="权重或分值">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="年度目标值">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="目标值说明">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="计算关系式">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZZBDH" HeaderText="指标代号">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="up" ImageUrl="/Resources/OA/easyui1.32/themes/icons/up.gif" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="down" ImageUrl="/Resources/OA/easyui1.32/themes/icons/down.gif" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
            </asp:GridView>
            <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView2_RowDataBound" AllowPaging="True" OnPageIndexChanging="GridView2_PageIndexChanging"
                PageSize="10" Visible="false">
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
		        <div><asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;</div>
	        </div>
        </div>
    </div>
</asp:Content>

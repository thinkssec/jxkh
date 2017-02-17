<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="TongzhiList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.TongzhiList" %>

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
    <%--<div data-options="region:'north',split:false,border:false" style="padding: 0px; overflow: hidden;">
        <div class="vDaohangtiaoHolder module">
            <div class="vDaohangtiao">
                <ul>
                    <li class="first">
                        <a href="/Main" target="_parent">首页</a>
                    </li>
                    <li class="last">考核管理</li>
                </ul>
            </div>
        </div>
        <div id="main-tool">
        </div>
    </div>--%>
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>绩效考核通知公告</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核年度：<asp:DropDownList ID="Ddl_Niandu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Niandu_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:TextBox ID="Txt_Tzbt_Search" runat="server"></asp:TextBox>
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
                    <asp:BoundField DataField="TZBT" HeaderText="标题名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TJRQ" HeaderText="发布日期" DataFormatString="{0:yyyy-MM-dd}">
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LLCS" HeaderText="浏览次数">
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("TZID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("TZID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" />
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
            <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>

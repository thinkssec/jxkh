<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KhUnlock.aspx.cs" Inherits="Enterprise.UI.Web.Kh.KhUnlock" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Register Assembly="Enterprise.Component.Infrastructure" Namespace="Enterprise.Component.Infrastructure" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>数据解锁操作</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<cc1:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
                </cc1:SSECDropDownList>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" AllowPaging="false" DataKeyNames="SID">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="单位名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核类型">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="数据状态">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="提交人">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="提交时间">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
            </asp:GridView>
            <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>
            <br />
            <asp:LinkButton ID="LnkBtn_All" runat="server" class="easyui-linkbutton" iconCls="icon-ok" OnClick="LnkBtn_All_Click">全选</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="LnkBtn_UnAll" runat="server" class="easyui-linkbutton" iconCls="icon-cancel" OnClick="LnkBtn_UnAll_Click">取消全选</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="LnkBtn_Lock" runat="server" class="easyui-linkbutton" iconCls="icon-lock" OnClick="LnkBtn_Lock_Click">锁定</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="LnkBtn_UnLock" runat="server" class="easyui-linkbutton" iconCls="icon-unlock" OnClick="LnkBtn_UnLock_Click">解除锁定</asp:LinkButton>
        </div>
    </div>
</asp:Content>

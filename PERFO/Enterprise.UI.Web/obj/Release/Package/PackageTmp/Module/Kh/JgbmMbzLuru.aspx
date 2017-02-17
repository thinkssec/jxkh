<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="JgbmMbzLuru.aspx.cs" Inherits="Enterprise.UI.Web.Kh.JgbmMbzLuru" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Register Src="~/Component/UserControl/PopWinUploadMuti.ascx" TagPrefix="uc1" TagName="PopWinUploadMuti" %>
<%@ Register Assembly="Enterprise.Component.Infrastructure" Namespace="Enterprise.Component.Infrastructure" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.show({
                title: tt,
                msg: msg,
                height: '200',
                width: '400',
                showType: 'show',
                timeout: 30000,
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>机关部门考核目标值录入</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<cc1:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
                </cc1:SSECDropDownList>
                &nbsp;&nbsp;
                单位：<cc1:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </cc1:SSECDropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="录入" class="easyui-linkbutton" plain="true"
                    iconCls="icon-edit" OnClick="Btn_Add_Click"></asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                AllowPaging="false" DataKeyNames="ID">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标类别">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核权重">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="年度目标">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核权重%" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核目标值">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核目标值说明">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作权限">
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" />
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
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-shenhe"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">正式提交</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                    OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

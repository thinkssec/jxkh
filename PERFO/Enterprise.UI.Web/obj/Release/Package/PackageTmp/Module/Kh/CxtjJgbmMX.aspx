<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="CxtjJgbmMX.aspx.cs" Inherits="Enterprise.UI.Web.Kh.CxtjJgbmMX" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

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
                <span>机关部门考核明细表</span>
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
                <asp:LinkButton ID="LnkBtn_Report" runat="server" Text="导出" class="easyui-linkbutton" plain="true"
                    iconCls="icon-xls" OnClick="LnkBtn_Report_Click"></asp:LinkButton>
            </div>
            <div class="main-gridview">
                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                    OnRowDataBound="GridView1_RowDataBound" AllowPaging="false" ShowFooter="true" DataKeyNames="ID">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                <%#(Container.DataItemIndex + 1) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="考核指标类型与权重">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"  Width="150px"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="考核主要内容">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="分值">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="考核目标">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="最终得分">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                </asp:GridView>
            </div>
            <div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                </div>
            </div>
        </div>
    </div>
</asp:Content>

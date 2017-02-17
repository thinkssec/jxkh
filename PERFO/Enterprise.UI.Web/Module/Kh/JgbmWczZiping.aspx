<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="JgbmWczZiping.aspx.cs" Inherits="Enterprise.UI.Web.Kh.JgbmWczZiping" %>

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
        function showLoading() {
            var win = $.messager.progress({
                title: '请您稍侯',
                msg: '正在提交数据...'
            });
            setTimeout(function () {
                $.messager.progress('close');
            }, 10000)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>机关部门考核自评表</span>
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
                <asp:LinkButton ID="Btn_Add" runat="server" Text="开始自评" class="easyui-linkbutton" plain="true"
                    iconCls="icon-edit" OnClick="Btn_Add_Click"></asp:LinkButton>
            </div>
            <div class="main-gridview">
                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                    OnRowDataBound="GridView1_RowDataBound" AllowPaging="false" ShowFooter="true" DataKeyNames="ID">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                <%--#(Container.DataItemIndex + 1) --%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="考核指标类型与权重">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
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
                        <asp:BoundField HeaderText="完成率">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="自评得分">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="自评说明">
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
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
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div class="btn-bgcolor">
                    <%-- <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>--%>
                    <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-shenhe"
                        OnClientClick="showLoading();" OnClick="LnkBtn_Upd_Click">正式提交</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                        OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

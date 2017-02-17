<%@ Page Title="" Language="C#" MasterPageFile="Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KaoheShowInfo.aspx.cs" Inherits="Enterprise.UI.Web.KaoheShowInfo" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>考核进展情况统计</span>
            </div>
        </div>
        <div id="tt" class="easyui-tabs" style="width: auto;">
            <div id="DIV_Ejdw" runat="server" title="基层单位及领导班子考核" data-options="iconCls:'icon-application'" style="padding: 4px; overflow-y: hidden;">
                <div class="main-gridview">
                    <div class="main-gridview-title">
                        考核名称：<asp:Label ID="Lbl_EjdwKaohe" runat="server" ForeColor="Black"></asp:Label>
                    </div>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                        OnRowDataBound="GridView1_RowDataBound" ShowFooter="false" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#(Container.DataItemIndex + 1) %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参评单位">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="考核值确认">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="数据填报">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="完成值审核">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="结果审定">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="文件提交">
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
            </div>
            <div id="DIV_Jgbm" runat="server" title="机关部门及负责人考核" data-options="iconCls:'icon-application'" style="padding: 4px; overflow-y: hidden;">
                <div class="main-gridview">
                    <div class="main-gridview-title">
                        考核名称：<asp:Label ID="Lbl_JgbmKaohe" runat="server" ForeColor="Black"></asp:Label>
                    </div>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                         OnRowDataBound="GridView2_RowDataBound" ShowFooter="false"  Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#(Container.DataItemIndex + 1) %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参评部门">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="自评情况">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="上级测评">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="同级测评">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="结果审定">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="文件提交">
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
            </div>
        </div>
        <div class="msg-info">
            <div class="msg-tip icon-tip"></div>
            <div>
                <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
            </div>
        </div>
    </div>
</asp:Content>

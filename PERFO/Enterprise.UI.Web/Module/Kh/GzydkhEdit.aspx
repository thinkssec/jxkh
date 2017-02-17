<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="GzydkhEdit.aspx.cs" Inherits="Enterprise.UI.Web.Kh.GzydkhEdit" %>

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
    <!--IPHONE Style-->
    <link href='/Resources/charisma/css/jquery.iphone.toggle.css' rel='stylesheet' />
    <script src="/Resources/charisma/js/jquery.iphone.toggle.js"></script>
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>工作要点评分</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">保存</asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="LnkBtn_Cancel_Click"></asp:LinkButton>
                <asp:HiddenField ID="Hid_KHID" runat="server" />
                <asp:HiddenField ID="Hid_KHDZBID" runat="server" />
            </div>
            <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" 
                CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="JGBM" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                    </asp:TemplateField>
<%--                    <asp:TemplateField HeaderText="考核项目名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="被打分部门">
                        <ItemTemplate>
                            <%#Eval("JGMC")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="得分">
                        <ItemTemplate>
                            <asp:TextBox ID="Txt_PF" runat="server" class="easyui-numberbox"  min="0" required="true"
                max="100"  missingMessage="必须填写数字" Width="60" precision="1"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注">
                        <ItemTemplate>
                            <asp:TextBox ID="Txt_BZ" runat="server" class="easyui-validatebox" Width="300"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
<%--                    <asp:BoundField DataField="LRSJ" HeaderText="打分时间">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                </Columns>
            </asp:GridView>
           
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('.iphone-toggle').iphoneStyle();
        });
    </script>
</asp:Content>

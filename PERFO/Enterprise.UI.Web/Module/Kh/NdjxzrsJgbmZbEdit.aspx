<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="NdjxzrsJgbmZbEdit.aspx.cs" Inherits="Enterprise.UI.Web.Kh.NdjxzrsJgbmZbEdit" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            Heji();
            var r = $("#form1").form('validate');
            if (r) {
                if (hjz != 100) {
                    if (confirm('当前主指标权重合计值不等于100？您确定保存吗？')) {
                        return true;
                    }
                    return false;
                }
                else {
                    return true;
                }
            }
            return false;
        }
        //计算合计值
        var hjz = 0;
        function Heji(txtObj) {
            try {
                hjz = 0;//先清零
                var lbArray = new Array();
                var txtObjs = $("input[name^='Txt4_']");
                txtObjs.each(function () {
                    hjz += parseFloat($(this).val());
                });
                //显示合计值
                var tdObj = document.getElementById("QzhjDiv");
                if (tdObj)
                    tdObj.innerHTML = "<font color='Red'>" + (Math.round(hjz * 100) / 100) + "%</font>";
            }
            catch (e) {
                alert(e);
            }
        }
        window.onload = Heji;
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
                考核年度：<asp:Label ID="Lbl_Niandu" runat="server" ForeColor="Black"></asp:Label>
                &nbsp;&nbsp;
                单位：<asp:Label ID="Lbl_Danwei" runat="server" ForeColor="Black"></asp:Label>
                &nbsp;&nbsp;
                应用版本：<asp:Label ID="Lbl_Bbmc" runat="server" ForeColor="Black"></asp:Label>
                &nbsp;&nbsp;
                责任书名称：<asp:Label ID="Lbl_Zrsmc" runat="server" ForeColor="Black"></asp:Label>
            </div>
            <asp:HiddenField ID="Hid_Type" runat="server" />
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                AllowPaging="false" DataKeyNames="ZRSZBID,LHZBBM,DFZBBM,ZRSID,ZZBDH,ZZBXZ,ZSJZB" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="选择">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标类别">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="120px"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"  Width="200px"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标性质" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="权重%">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="分值">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="年度目标值" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="目标值说明" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="计算关系式" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标代号" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <FooterStyle HorizontalAlign="Center" Height="32px" />
            </asp:GridView>
            <div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;</div>
            </div>
            <div class="btn-bgcolor">
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                    OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>

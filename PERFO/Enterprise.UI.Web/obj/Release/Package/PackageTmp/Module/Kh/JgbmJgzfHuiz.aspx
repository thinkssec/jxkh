<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="JgbmJgzfHuiz.aspx.cs" Inherits="Enterprise.UI.Web.Kh.JgbmJgzfHuiz" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function c1() {
            if (confirm('您确定要重新提取本次考核的单位信息吗？原先数据会自动清空!')) {
                return true;
            }
            return false;
        }
        function c2() {
            if (confirm('您确定要重新汇总得分吗？原先数据会自动清空!')) {
                return true;
            }
            return false;
        }
        function showWin(kh, bm, zb, lx) {
            $('#w').window('open');
            var url = "KH=" + kh + "&BM=" + bm + "&ZB=" + zb + "&LX=" + lx;
            $('#tt').datagrid('options').url = "/Module/Kh/KhDataHandler.ashx?" + url;
            $('#tt').datagrid('reload');
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
        $(function () {
            $('#tt').datagrid({
                url: '',
                width: 'auto',
                height: 'auto',
                fitColumns: true,
                singleSelect: true,
                columns: [[
					{ field: 'XH', title: '序号', width: 50 },
					{ field: 'DWMC', title: '打分单位', width: 150 },
					{ field: 'ZBMC', title: '指标名称', width: 90 },
                    { field: 'DF', title: '得分', width: 70 }
                ]]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>机关作风建设-打分结果汇总</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<Ent:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_InitData" runat="server" Text="数据初始化" class="easyui-linkbutton" plain="true"
                    iconCls="icon-info" OnClick="LnkBtn_InitData_Click" OnClientClick="return c1();"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Calculate" runat="server" Text="汇总得分" class="easyui-linkbutton" plain="true"
                    iconCls="icon-sum" OnClick="LnkBtn_Calculate_Click" OnClientClick="return c2();"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Edit" runat="server" Text="结果审定" class="easyui-linkbutton" plain="true"
                    iconCls="icon-edit" OnClick="LnkBtn_Edit_Click" Visible="false"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Report" runat="server" Text="导出" class="easyui-linkbutton" plain="true"
                    iconCls="icon-xls" OnClick="LnkBtn_Report_Click"></asp:LinkButton>
            </div>
            <table style="width: 100%; text-align: center;" id="Table4">
                <%=GetJgbmTable() %>
            </table>
            <div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                </div>
            </div>
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div class="btn-bgcolor">
                    <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-shenhe"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">保存</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                        OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
    <%-- 打分详情窗口 --%>
    <div id="w" class="easyui-window" title="打分明细" iconcls="icon-yunxing" style="width: 500px; height: 350px; padding: 5px;" closed="true" modal="true" maximizable="false" minimizable="false" collapsible="false" resizable="true" inline="false">
        <table id="tt"></table>
    </div>
</asp:Content>

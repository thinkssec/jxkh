<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="JgbmJgzfDetail.aspx.cs" Inherits="Enterprise.UI.Web.Kh.JgbmJgzfDetail" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.alert(tt, msg);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>机关作风建设打分详细情况</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<asp:Label ID="Lbl_Kaohe" runat="server" Text="" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
                测评部门：<asp:Label ID="Lbl_Cpbm" runat="server" Text="" ForeColor="Black"></asp:Label>
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
        </div>
    </div>
</asp:Content>

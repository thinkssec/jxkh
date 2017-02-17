<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongzhiEdit.aspx.cs" Inherits="Enterprise.UI.Web.Kh.TongzhiEdit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Resources/xheditor/xheditor-1.1.12-zh-cn.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/easyui-lang-zhcn.js"></script>
    <script type="text/javascript">
        function checkform() {
            var v = $('#Txt_TZNR').val();
            $('#Hid_TZNR').val(v);
            //alert($('#Hid_TZNR').val());
            return $("#form1").form('validate');
        }
        $(document).ready(function () {
            $('#Txt_TZNR').xheditor({ upLinkUrl: "Upload.aspx", upLinkExt: "zip,rar,txt,pdf,docx,doc,xls,xlsc", upImgUrl: "upload.aspx", upImgExt: "jpg,jpeg,gif,png" });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="mk" class="easyui-panel" title="通知公告管理" collapsible="false" style="padding: 10px; background: #fafafa;">
            <div style="font-size: 12px;">
                <asp:HiddenField ID="Hid_TZID" runat="server" />
                <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                    OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
                <table style="width: 100%">
                    <tr>
                        <td>标题:</td>
                        <td>
                            <asp:TextBox ID="Txt_TZBT" runat="server" class="easyui-validatebox" required="true" validType="length[0,300]" invalidMessage="不得超过150个字!" missingMessage="必填项" Width="550px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>内容:</td>
                        <td>
                            <asp:TextBox ID="Txt_TZNR" ClientIDMode="Static" runat="server" TextMode="MultiLine"
                                Width="85%" Height="190px"></asp:TextBox>
                            <asp:HiddenField ID="Hid_TZNR" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>时间:</td>
                        <td>
                            <asp:TextBox ID="Txt_TJRQ" runat="server" class="easyui-datebox" editable="false">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>发布人:</td>
                        <td>
                            <asp:TextBox ID="Txt_TZZZ" runat="server" Width="125px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                                OnClientClick="return checkform();" OnClick="LnkBtn_Upd_Click">保存</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>

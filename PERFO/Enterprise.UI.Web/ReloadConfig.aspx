<%@ Page Language="C#" AutoEventWireup="true" Inherits="Enterprise.UI.Web.ReloadConfig"
    CodeBehind="ReloadConfig.aspx.cs" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统复位</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="position: absolute; top: 20%; text-align: center; left: 30%; right: 402px; background-image: url(Images/error.gif); width: 441px; height: 281px;">
            <div style="position: absolute; top: 135px; text-align: center; left: 30%; width: 230px;">
                <asp:Label ID="msg" runat="server" Text="" Font-Size="Small"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>

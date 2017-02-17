<%@ Page Language="C#" AutoEventWireup="true" Inherits="Enterprise.UI.Web.Error" CodeBehind="Error.aspx.cs" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Error Page</title>
    <style type="text/css">
        body {
            font-size: 12px;
        }
    </style>
</head>
<body style="margin: 0px; background-color: #FFFFFF;">
    <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <table width="514" border="0" cellspacing="0" cellpadding="0" align="center" style="height: 202px">
            <tbody>
                <tr>
                    <td>
                        <img src="Resources/OA/err/img2.gif" width="514" height="32"></td>
                </tr>
                <tr>
                    <td align="center" valign="top" nowrap="" style="background-image: url(Resources/OA/err/img1.gif);">
                        <br />
                        <table width="355" border="0" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td colspan="2">
                                        <img src="Resources/OA/err/img13.gif" width="355" height="10"></td>
                                </tr>
                                <tr>
                                    <td width="86" align="right" valign="top" style="background-image: url(Resources/OA/err/img16.gif);">
                                        <img src="Resources/OA/err/I.gif" width="65" height="59" /></td>
                                    <td width="269" align="right" valign="top" style="background-image: url(Resources/OA/err/img15.gif);">
                                        <table width="245" border="0" cellspacing="0" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <img src="Resources/OA/err/cc.gif" width="8" height="5" /></td>
                                                </tr>
                                                <tr align="left">
                                                    <td><font color="red"><b>系统提示:</b></font></td>
                                                </tr>
                                                <tr align="left">
                                                    <td>
                                                        <asp:Label ID="errmsg" runat="server" Text="Label" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img src="Resources/OA/err/img14.gif" width="355" height="9" /></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <br />
                                        <a href="javascript:window.history.back(-1)">
                                            <img src="Resources/OA/err/back.jpg" border="0" /></a></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="Resources/OA/err/img3.gif" width="514" height="9" /></td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>

<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="PdfViewer.aspx.cs"
    Inherits="Enterprise.UI.Web.Component.FlexPaper.PdfViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PDF_Viewer</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1,user-scalable=no,maximum-scale=1,width=device-width" />
    <link rel="stylesheet" type="text/css" href="/Component/FlexPaper/css/flexpaper.css" />
    <script type="text/javascript" src="/Component/FlexPaper/js/jquery.min.js"></script>
    <script type="text/javascript" src="/Component/FlexPaper/js/flexpaper.js"></script>
    <script type="text/javascript" src="/Component/FlexPaper/js/flexpaper_handlers.js"></script>
</head>
<body style="background-image: url(/Resources/Styles/images/body_2.png); background-repeat: repeat-x;">
    <form id="form1" runat="server">
        <table border="1" cellpadding="0" cellspacing="0" width="90%" align="center" style="background-color: white; grid-cell: inherit; border-collapse: collapse;">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" id="articlecontent" width="100%">
                        <tr id="s_cn" clientidmode="Static">
                            <td style="width: 100%; height: 600px; padding: 10px;" valign="top" align="left">
                                <div id="documentViewer" class="flexpaper_viewer" style="width: 100%; height: 100%">
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        var swfFile = "<%=SwfFileName %>";
        $('#documentViewer').FlexPaperViewer(
                {
                    config: {
                        SWFFile: swfFile,
                        Scale: 0.6,
                        ZoomTransition: 'easeOut',
                        ZoomTime: 0.5,
                        ZoomInterval: 0.2,
                        FitPageOnLoad: true,
                        FitWidthOnLoad: true,
                        FullScreenAsMaxWindow: false,
                        ProgressiveLoading: false,
                        MinZoomSize: 0.2,
                        MaxZoomSize: 5,
                        SearchMatchAll: false,
                        InitViewMode: 'Portrait',
                        RenderingOrder: 'flash',
                        StartAtPage: '',
                        ViewModeToolsVisible: true,
                        ZoomToolsVisible: true,
                        NavToolsVisible: true,
                        CursorToolsVisible: true,
                        SearchToolsVisible: true,
                        WMode: 'window',
                        localeChain: 'en_US',
                        jsDirectory: '/Component/FlexPaper/js/',
                        cssDirectory: '/Component/FlexPaper/css/'
                    }
                }
                );
    });
</script>

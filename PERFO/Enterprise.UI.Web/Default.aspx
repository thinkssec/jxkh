<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Enterprise.UI.Web.Default" %>

<html>
<head>
    <title>绩效考核系统</title>
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <link rel="stylesheet" type="text/css" href="/Resources/Css/mainStyle.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/popup.css" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <style type="text/css">
        .easyui-tree {
            font: 13px "Microsoft YaHei", "u5FAEu8F6Fu96C5u9ED1", "Arial", "SimSun", "u5B8Bu4F53";
            font-weight: bold;
            display: inline-block;
            line-height: 190%;
            padding: 4px 4px 4px 4px;
        }
        Td {
            font-size: 12px;
        }
    </style>
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/ShowTips.js"></script>
    <script type="text/javascript">
        //最后一个标签
        var lastTitle = "";
        var urls = new Array();
        function addTab(url, title) {
            var isExist = $('#tt').tabs('exists', title);
            if (title == "欢迎" && isExist) return;
            if (isExist == false) {
                $('#tt').tabs('add', {
                    title: title,
                    content: "<iframe Scrolling=\"yes\" Frameborder=\"0\" Src=\"" + url + "\" Style=\"width:100%;height:100%;\"></iframe>",
                    iconCls: 'icon-app',
                    closable: true
                });
                if (title != "基础数据表") {
                    if (title != lastTitle) {
                        $('#tt').tabs('close', lastTitle);
                    }
                    lastTitle = title;
                }
                urls[title] = url;//保存URL
            }
            else {
                $('#tt').tabs('select', title);
            }
        }
        function refresh() {
            var currTab = $('#tt').tabs('getSelected'); //获得当前tab
            if (currTab) {
                var url = $(currTab.panel('options').content).attr('src');
                var title = currTab.panel('options').title;
                $('#tt').tabs('close', title);
                if (title == "欢迎") {
                    $('#tt').tabs('add', {
                        title: title,
                        content: "<iframe Scrolling=\"yes\" Frameborder=\"0\" Src=\"<%=url%>\" Style=\"width:100%;height:100%;\"></iframe>",
                        closable: false
                    });
                }
                else if (url) {
                    addTab(url, title);
                }
            }
        }
        //初始加载
        $(document).ready(function () {
            $('.easyui-accordion a').click(function () {
                for (var i = 0; i < $('.leftMenuCurrent').length; i++) {
                    $('.easyui-accordion a').removeClass("leftMenuCurrent");
                    $('.easyui-accordion a').addClass("leftMenu");
                }
                $(this).removeClass();
                $(this).addClass('leftMenuCurrent leftMenuCurrentlink');
            });
            $('#treeUl').tree('expandAll');
            $('#tt').tabs({
                tools: [{
                    iconCls: 'icon-menu',
                    handler: function () {
                        $.messager.confirm('系统提示', '您想将‘左侧主菜单’切换成‘<%=ReverseUMenu%>’风格吗？', function (r) {
                            if (r) {
                                location.href = '/Main?menu=<%=ReverseUMenuValue%>';
                            }
                        });
                    }
                }, {
                    iconCls: 'icon-newwin',
                    handler: function () {
                        var pp = $('#tt').tabs('getSelected');
                        if (pp) {
                            var selectTitle = pp.panel('options').title;
                            if (urls[selectTitle] != undefined) {
                                var url = urls[selectTitle];
                                if (url.indexOf('?') > 0) {
                                    url += "&WinMax=1";
                                }
                                else {
                                    url += "?WinMax=1";
                                }
                                window.open(url);
                            }
                        }
                    }
                }, {
                    iconCls: 'icon-calculate',
                    handler: function () {
                        window.open('Calculate.htm', 'newwindow', 'height=400,width=300,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');
                    }
                }, {
                    iconCls: 'icon-reload',
                    handler: function () {
                        refresh();
                    }
                }]
            });
        });
    </script>
    <script type="text/javascript">
        window.history.forward(1);
    </script>
</head>
<body class="easyui-layout">
    <div region="north" border="false" style="height: 65px; background-color: #E2E6EA; background-image: url('/Resources/Images/bg2.jpg'); background-position: right; overflow: hidden;">
        <img src="/Resources/Images/logo2.gif" />
        <div style="right: 25px; bottom: 5px; position: absolute; background: #efefef; padding: 0; text-decoration: none; float: left;">
            <a id="H01" href="#" onmouseover="showHelper('#H01', '提示', '<%=GetUserInfo() %>',60)" onmouseout="$('#_Popup_help').remove()" class="easyui-linkbutton" plain="true" iconcls="icon-boss">当前用户：<%=userModel.USERNAME %></a> <a href="javascript:addTab('ChangePwd.aspx','<%=Trans("修改密码") %>')"
                class="easyui-linkbutton" plain="true" iconcls="icon-pwd">修改密码</a> 
            <%--<a href="javascript:addTab('Component/Map/SiteMap.aspx','帮助')" class="easyui-linkbutton" plain="true"
                    iconcls="icon-help">帮助</a>--%> <a href="Loginout.aspx" class="easyui-linkbutton"
                        plain="true" iconcls="icon-quit">退出</a><!--plain="true"-->
        </div>
    </div>
    <div region="west" split="true" title="<%=Trans("主菜单")%>" style="width: 230px; padding: 1px; overflow-x: hidden;">
        <%=GetUserMenu() %>
        <%--<ul id="treeUl" class="easyui-tree" animate="false" dnd="false">
            <%=Enterprise.Service.Perfo.Sys.SysUserService.LoadTreeMenu(userModel) %>
        </ul>--%>
        <%--<div id="accordionMenu" class="easyui-accordion" fit="true" border="false">
            <%=Enterprise.Service.Perfo.Sys.SysUserService.LoadAccordionMenu(userModel)%>
        </div>--%>
    </div>
    <div region="south" border="false" style="height: 25px; background-color: #ffffff; padding: 5px; overflow: hidden;">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="text-align: left;">∠欢迎您！今天是：<%=DateTime.Now.ToString("yyyy年MM月dd日") %>！最近一次操作：<asp:Label ID="Lbl_LastLogin" runat="server" /></td>
                <td style="text-align: right;">天然气川气东送管道分公司 Copyright 2014-2016.All Rights Reserved.</td>
            </tr>
        </table>
    </div>
    <div region="center" title="<%=Trans("工作区")%>" style="overflow: hidden;">
        <div class="easyui-tabs" fit="true" border="false" id="tt">
            <div title="<%=Trans("欢迎")%>" style="padding: 0px; overflow: hidden;">
                <iframe scrolling="auto" frameborder="0" src="<%=url %>" style="width: 100%; height: 100%;"></iframe>
            </div>
        </div>
    </div>
</body>
</html>

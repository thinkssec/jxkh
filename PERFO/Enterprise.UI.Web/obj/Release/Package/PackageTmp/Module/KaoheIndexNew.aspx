<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KaoheIndexNew.aspx.cs" ValidateRequest="false"
    Inherits="Enterprise.UI.Web.Module.KaoheIndexNew" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>主页面</title>
    <%--IE Mode -- %>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <%--icon--%>
    <link rel="stylesheet" href="/Resources/OA/easyui1.32/themes/icon.css" />
    <%--default--%>
    <link rel="Stylesheet" href="/Resources/OA/easyui1.32/themes/default/easyui.css" title="default" />
    <link rel="Stylesheet" href="/Resources/OA/site_skin/default/site.main.css" title="default" />
    <link rel="Stylesheet" href="/Resources/Css/Site.css" type="text/css" />
    <%--script--%>
    <script type="text/javascript" src="/Resources/OA/jquery/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/easyui1.32/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/easyui1.32/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="/Resources/OA/site_skin/scripts/Base.js"></script>
    <script type="text/javascript" src="/Resources/OA/site_skin/scripts/Common.js"></script>

    <script type="text/javascript">
        function ShowProgress(lx, v) {
            $('#progressWinDiv').window('open');
            var url = "LX=" + lx + "&PV=" + v;
            $('#progressTable').treegrid('options').url = "/Module/Kh/KhDataHandler.ashx?" + url;
            $('#progressTable').treegrid('reload');
        }
        $(function () {
            $('#progressTable').treegrid({
                title: '',
                iconCls: 'icon-ok',
                width: 565,
                height: 450,
                rownumbers: true,
                animate: true,
                collapsible: true,
                fitColumns: true,
                url: '',
                idField: 'name',
                treeField: 'name',
                showFooter: true,
                rowStyler: function (row) {
                    if (row.name == "总进度") {
                        return 'background:#ccffff;color:#000000';
                    }
                },
                columns: [[
	                { field: 'name', title: '节点名称', width: 220 },
					{
					    field: 'progress', title: '当前进度', width: 240,
					    formatter: function (value) {
					        if (value || value == '0') {
					            var s = '<div style="width:100%;background:#fff;border:1px solid #ccc;">' +
						    			'<div style="width:' + value + '%;background:#99ff00">' + value + '%</div></div>';
					            return s;
					        } else {
					            return '';
					        }
					    }
					}
                ]],
                onLoadSuccess: function () {
                    //alert("aaaaaa");
                },
                onDblClickRow: function () {
                    $('#progressWinDiv').window('close');
                }
            });
        });
    </script>
    <link rel="stylesheet" type="text/css" href="/Resources/Css/Index.css" />
</head>
<body style="padding: 0; margin: 0 auto;">
    <form id="form1" runat="server">
        <div style="padding: 5px;">
            <div data-options="region:'north',split:false,border:false" style="padding: 0px; overflow: hidden;">
                <%--导航条开始--%>
                <div class="vDaohangtiaoHolder module">
                    <div class="vDaohangtiao">
                        <ul>
                            <li>
                                <img src="/Resources/OA/easyui1.32/themes/icons/sound.png" /></li>
                            <li class="last">
                                <asp:Label ID="Lbl_TopTongzhi" runat="server"></asp:Label>
                            </li>
                        </ul>
                    </div>
                </div>
                <%--end--%>
                <%--权限按钮开始--%>
                <div id="main-tool">
                    <%--<div id="scrollDiv"> 
                <ul> 
                    <li>11</li> 
                    <li>22</li> 
                    <li>33</li> 
                </ul> 
            </div> --%>
                </div>
                <%--end--%>
            </div>
            <br />
            <div data-options="region:'center'">
                <!--综合信息  start-->
                <asp:HiddenField ID="Hid_TabTitle" runat="server" />
                <div id="p2" class="easyui-panel" title=""
                    style="width: auto; padding: 2px;"
                    data-options="closable:false,collapsible:false,minimizable:false,maximizable:false">
                    <div id="tt" class="easyui-tabs" style="width: auto;">
                        <div title="考核信息" data-options="iconCls:'icon-sum'" style="padding: 4px">
                            <div class="main-gridview">
                                <table style="width: 100%; text-align: center;" id="Table3">
                                    <tr>
                                        <td colspan="2">
                                            <!--当前年度的所有考核信息-->
                                            <asp:GridView ID="GridView_Kaohe" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                                AllowPaging="false" OnRowDataBound="GridView_Kaohe_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="序号">
                                                        <ItemTemplate>
                                                            <%#(Container.DataItemIndex + 1) %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="考核名称">
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="考核年度">
                                                        <ItemTemplate>
                                                            <%#Eval("KHND") %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="考核范围">
                                                        <ItemTemplate>
                                                            <%#((KhKhglModel)GetDataItem()).Kind.LXMC %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="当前状态">
                                                        <ItemTemplate>
                                                            <%#(Eval("KHZT").Equals("1")?"<span class=\"label-success label label-default\">已完成</span>":"<span class=\"label-warning label label-default\">进行中</span>") %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                <RowStyle CssClass="GridViewRowStyle" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </div>
                        </div>
                        <div title="我的待办箱【<%=MessageCount %>】" data-options="iconCls:'icon-wode'" style="padding: 4px">
                            <div class="main-gridview">
                                <asp:GridView ID="GridView_MyMessage" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                    AllowPaging="false" OnRowDataBound="GridView_MyMessage_RowDataBound" OnRowCommand="GridView_MyMessage_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="序号">
                                            <ItemTemplate>
                                                <%#(Container.DataItemIndex + 1) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="事项名称">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="待办说明">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="接收日期">
                                            <ItemTemplate>
                                                <%#Eval("JSRQ").ToDateYMDFormat() %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="当前状态">
                                            <ItemTemplate>
                                                <%#(Eval("DQZT").Equals("1")?"<span class=\"label-success label label-default\">已完成</span>":"<span class=\"label-warning label label-default\">待处理</span>") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="操作">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("MSGID") %>'
                                                    CommandName="bianji" ImageUrl="/Resources/Styles/_img/right.gif"
                                                    ToolTip="处理完了" OnClientClick="return confirm('您确定要关闭该待办吗？');" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <font color="blue">您目前还没有未处理的待办事务!</font>
                                    </EmptyDataTemplate>
                                    <HeaderStyle CssClass="GridViewHeaderStyle" HorizontalAlign="Center" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div title="考核制度汇编" data-options="iconCls:'icon-doc'" style="padding: 4px">
                            <div class="main-gridview">
                                <asp:GridView ID="GridView_Khzdhb" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridViewStyle" OnRowDataBound="GridView_Khzdhb_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="序号">
                                            <ItemTemplate>
                                                <%#(Container.DataItemIndex + 1) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="考核文件名称">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="考核类型">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="文件下载">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="在线浏览">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!--综合信息  end-->
                    <br />
                    <!--通知公告  start-->
                    <div id="p1" class="easyui-panel" title="通知公告"
                        style="width: auto; padding: 4px;"
                        data-options="closable:false,collapsible:false,minimizable:false,maximizable:false">
                        <div class="main-gridview">
                            <table style="padding: 0px; margin: 0px; width: 100%">
                                <tr>
                                    <td style="vertical-align: top; text-align: left; width: auto;">
                                        <asp:GridView ID="GridView_Tongzhi" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                            OnRowDataBound="GridView_Tongzhi_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="序号">
                                                    <ItemTemplate>
                                                        <%#(Container.DataItemIndex + 1) %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TZBT" HeaderText="标题名称">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TJRQ" HeaderText="发布日期" DataFormatString="{0:yyyy-MM-dd}">
                                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LLCS" HeaderText="浏览次数">
                                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                            <RowStyle CssClass="GridViewRowStyle" />
                                        </asp:GridView>
                                    </td>
                                    <td style="text-align: center; vertical-align: central; width: 190px;">
                                        <%-- 日历 --%>
                                        <div id="cc" class="easyui-calendar" style="height: 215px; width: 190px;"></div>
                                        <div id="dd"></div>
                                        <script type="text/javascript">
                                            $('#cc').calendar({
                                                onSelect: function (date) {
                                                    $('#dd').dialog({
                                                        title: '万年历',
                                                        width: 630,
                                                        height: 380,
                                                        closed: false,
                                                        cache: false,
                                                        href: '/rili.htm',
                                                        modal: true
                                                    });
                                                }
                                            });
                                        </script>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <!--通知公告  end-->
                    <%-- 进度显示窗口 --%>
                    <div id="progressWinDiv" data-options="title:'考核进度(双击可关闭)',collapsible:false,minimizable:false,maximizable:false,draggable:true,resizable:true,inline:false" class="easyui-window" style="width: 500px; height: 300px; overflow: hidden" closed="true" modal="true">

                        <!--项目进度详单-->
                        <table id="progressTable"></table>

                    </div>
                </div>
            </div>
            <script type="text/javascript">
                $('#tt').tabs({
                    onSelect: function (title, index) {
                        $('#tt').tabs('getTab', index).show();
                        $('#<%=Hid_TabTitle.ClientID%>').val(title);
                    }
                });
                $(function () {
                    //tabs
                    $('#tt').tabs('select', '<%=TabTitle%>');
                });
            </script>
        </div>
    </form>
</body>
</html>

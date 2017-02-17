<%@ Page Title="" Language="C#" MasterPageFile="Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KaoheDanweiIndex.aspx.cs" Inherits="Enterprise.UI.Web.KaoheDanweiIndex" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/Resources/OA/jqplot-1.08/jquery.jqplot.min.css" />
    <!--[if lt IE 9]>
    <script language="javascript" type="text/javascript" src="/Resources/OA/jqplot-1.08/excanvas.js"></script>
    <![endif]-->
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/jquery.jqplot.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.pieRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.highlighter.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.logAxisRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.canvasTextRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.canvasAxisLabelRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.canvasAxisTickRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.dateAxisRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.categoryAxisRenderer.min.js"></script>
    <script type="text/javascript" src="/Resources/OA/jqplot-1.08/plugins/jqplot.barRenderer.min.js"></script>
    <link rel="stylesheet" type="text/css" href="/Resources/OA/jqplot-1.08/jquery.jqplot.min.css" />
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
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
        <asp:HiddenField ID="Hid_TabTitle" runat="server" />

        <!--我的待办箱  start-->
        <div id="p1" class="easyui-panel" title="我的待办箱【<%=MessageCount %>】"
            style="width: auto; padding: 4px;"
            data-options="iconCls:'icon-wode',closable:false,
            collapsible:false,minimizable:false,maximizable:false">
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
        <!--我的待办箱  end-->

        <!--综合信息  start-->
        <div id="p2" class="easyui-panel" title=""
            style="width: auto; padding: 2px;"
            data-options="closable:false,
                collapsible:false,minimizable:false,maximizable:false">
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
                                            <asp:TemplateField HeaderText="当前状态">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="单位得分">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="领导班子得分">
                                                <ItemTemplate>
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
                            <tr>
                                <td style="width: 35%;vertical-align:top;">

                                    <!--当期考核数据统计-->
                                    <table style="width: 100%; text-align: center;" id="Table7">
                                        <%=GetEjdwKaoheInfo() %>
                                    </table>

                                </td>
                                <td>
                                    <!--图表-->
                                    <div id="chart1" style="width: 100%; height: 200px;"></div>
                                    <script>
                                        $(document).ready(function () {
                                            var s1 = [<%=GetChartData(1)%>];
                                            var s2 = [<%=GetChartData(2)%>];
                                            var ticks = [<%=GetChartData(3)%>];
                                            var plot1 = $.jqplot('chart1', [s1, s2], {
                                                seriesDefaults: {
                                                    renderer: $.jqplot.BarRenderer,
                                                    rendererOptions: {
                                                        barMargin: 30,
                                                        highlightMouseDown: true,
                                                        fillToZero: true
                                                    },
                                                    pointLabels: { show: true }
                                                },
                                                axes: {
                                                    xaxis: {
                                                        renderer: $.jqplot.CategoryAxisRenderer,
                                                        ticks: ticks
                                                    },
                                                    yaxis: {
                                                        padMin: 0,
                                                        tickOptions: { formatString: '%d' }
                                                    }
                                                }
                                            });
                                        });
                                    </script>
                                </td>
                            </tr>
                        </table>
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
</asp:Content>

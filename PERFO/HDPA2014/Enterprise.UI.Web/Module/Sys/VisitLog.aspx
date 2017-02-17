<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="VisitLog.aspx.cs" Inherits="Enterprise.UI.Web.Sys.VisitLog" %>

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
        function checkform() {
            return $("#form1").form('validate');
        }
        function aa() {
            if (confirm('您确定要删除数据？')) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'north',split:false,border:false" style="padding: 0px; overflow: hidden;">
        <%--导航条开始--%>
        <div class="vDaohangtiaoHolder module">
            <div class="vDaohangtiao">
                <ul>
                    <li class="first">
                        <a href="/Main" target="_parent">首页</a>
                    </li>
                    <li class="last">系统管理</li>
                </ul>
            </div>
        </div>
        <%--end--%>
        <%--权限按钮开始--%>
        <div id="main-tool">
        </div>
        <%--end--%>
    </div>
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>访问日志</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                <asp:TextBox ID="RQ_Start" runat="server" CssClass="easyui-datebox" editable="false"></asp:TextBox>
                &nbsp;—&nbsp;
                <asp:TextBox ID="RQ_End" runat="server" CssClass="easyui-datebox" editable="false"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Search" runat="server" class="easyui-linkbutton" iconCls="icon-search" OnClick="LnkBtn_Search_Click" >查询</asp:LinkButton>
            </div>
            <asp:GridView Width="100%" ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" ShowFooter="false" 
                 OnPageIndexChanging="GridView1_PageIndexChanging"
                DataKeyNames="ID" OnRowCommand="GridView1_RowCommand" AllowPaging="true" PageSize="20">
                <Columns>
                    <asp:BoundField HeaderText="序号">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="USERNAME" HeaderText="用户名">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="VISITURL" HeaderText="访问路径">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="OPERATIONTYPE" HeaderText="操作模块">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="OPERATIONDATE" HeaderText="操作时间">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="VISITIPADDR" HeaderText="来路IP">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="BROWSERTYPE" HeaderText="浏览器类型">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa()" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
            </asp:GridView>

            <%--<table style="width: 100%; text-align: center;" id="Table2">
                <tr>
                    <th style="width: 10%" class="td-bold">序号</th>
                    <th style="width: 15%" class="td-bold">用户名称</th>
                    <th style="width: 15%" class="td-bold">访问路径</th>
                    <th style="width: 15%" class="td-bold">操作类型</th>
                    <th style="width: 15%" class="td-bold">操作时间</th> 
                    <th style="width: 15%" class="td-bold">来路IP</th>
                    <th style="width: 15%" class="td-bold">浏览器类型</th>
                </tr>
                <tr style="text-align:left;">
                    <td>1</td>
                    <td>admin</td>
                    <td>/Default.aspx</td>
                    <td>读取</td>
                    <td>2014-07-04</td>
                    <td>127.0.0.1</td>
                    <td>IE10</td>
                </tr>
                <tr style="text-align:left;">
                    <td>2</td>
                    <td>admin</td>
                    <td>/Manage/VisitLog.aspx</td>
                    <td>读取</td>
                    <td>2014-07-04 13:13:13</td>
                    <td>127.0.0.1</td>
                    <td>IE10</td>
                </tr>
            </table>--%>
        </div>
        <div id="chart4" style="height: 350px; padding: 10px"></div>
        <script>
            $(document).ready(function () {
                var line1 = [<%=GetLineData()%>];
                var plot2 = $.jqplot('chart4', [line1], {
                    // Give the plot a title.
                    title: '访问量统计',
                    axes: {
                        // options for each axis are specified in seperate option objects.
                        xaxis: {
                            renderer: $.jqplot.DateAxisRenderer,
                            tickOptions: {
                                formatString: '%b&nbsp;%#d'
                            },
                            label: "日期"
                        },
                        yaxis: {
                            label: "次数",
                            tickOptions: {
                                formatString: '%d'
                            }
                        }
                    },
                    highlighter: {
                        show: true
                    },
                    cursor: {
                        show: false
                    }
                });
            });
        </script>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="JgbmWczShending.aspx.cs" Inherits="Enterprise.UI.Web.Kh.JgbmWczShending" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
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
            $('#dt').datagrid('options').url = "/Module/Kh/KhDataHandler.ashx?" + url;
            $('#dt').datagrid('reload');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>机关部门及负责人考核-结果汇总</span>
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
                <asp:LinkButton ID="LnkBtn_Calculate" runat="server" Text="统计得分" class="easyui-linkbutton" plain="true"
                    iconCls="icon-sum" OnClick="LnkBtn_Calculate_Click" OnClientClick="return c2();"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Edit" runat="server" Text="审定和发布结果" class="easyui-linkbutton" plain="true"
                    iconCls="icon-edit" OnClick="LnkBtn_Edit_Click" Visible="false"></asp:LinkButton>
            </div>
            <div id="tt" class="easyui-tabs" style="width: auto;">
                <div title="机关部门" data-options="iconCls:'icon-application'" style="padding: 4px; overflow-y: hidden;">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" 
                            CssClass="GridViewStyle"
                            OnRowDataBound="GridView1_RowDataBound" AllowPaging="false" ShowFooter="true"  DataKeyNames="KHID,JGBM">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="处室名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="管理类考核指标">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="专项考核指标">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="机关作风考核得分" Visible="false">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="机关作风建设加分" Visible="false">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="最终得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="排名">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="兑现系数" Visible="false">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                        <%--<table style="width: 100%; text-align: center;" id="Table4" runat="server">
                            <tr>
                                <th style="width: 6%" class="td-bold">序号</th>
                                <th style="width: 18%" class="td-bold">单位名称</th>
                                <th style="width: 12%" class="td-bold">年度重点工作</th>
                                <th style="width: 10%" class="td-bold">部门履职</th>
                                <th style="width: 12%" class="td-bold">机关作风建设</th>
                                <th style="width: 12%" class="td-bold">总部连带考核</th>
                                <th style="width: 10%" class="td-bold">总得分</th>
                                <th style="width: 10%" class="td-bold">得分排名</th>
                                <th style="width: 10%" class="td-bold">操作</th>
                            </tr>
                            <tr>
                                <td>1</td>
                                <td>财务一处</td>
                                <td>98</td>
                                <td>90</td>
                                <td>3</td>
                                <td>2</td>
                                <td>100</td>
                                <td>1</td>
                                 <td>1</td>
                            </tr>
                        </table>--%>
                    </div>
                </div>
                <%--<div title="负责人" data-options="iconCls:'icon-application'" style="padding: 4px;overflow-y: hidden;">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" 
                            CssClass="GridViewStyle"
                            OnRowDataBound="GridView2_RowDataBound" AllowPaging="false" ShowFooter="true" DataKeyNames="KHID,JGBM">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="处室名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="重点工作完成情况得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="部门履职考核得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="机关作风考核得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="机关作风建设加分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="部门得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="连带指标得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="原因说明">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="最终结果">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="排名">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="兑现倍数">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                    </div>
                </div>--%>
            </div>
            <div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                </div>
            </div>
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div class="btn-bgcolor">
                    发布选择：<asp:CheckBoxList ID="ChkBox_FBLX" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="部门考核结果" Value="BM" Selected="True"></asp:ListItem>
                        <%--<asp:ListItem Text="负责人考核结果" Value="FZR"></asp:ListItem>--%>
                    </asp:CheckBoxList><br/>
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
        <table id="dt"></table>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#tt').tabs('select', '<%=TabTitle%>');
            $('#dt').datagrid({
                url: '',
                width: 'auto',
                height: 'auto',
                fitColumns: true,
                singleSelect: true,
                columns: [[
					{ field: 'XH', title: '序号', width: 50 },
					{ field: 'DWMC', title: '打分单位', width: 150 },
					{ field: 'LD', title: '上级打分', width: 80 },
                    { field: 'TJ', title: '同级打分', width: 80 },
                    { field: 'ZP', title: '自评打分', width: 80 }
                ]]
            });
        });
    </script>
</asp:Content>

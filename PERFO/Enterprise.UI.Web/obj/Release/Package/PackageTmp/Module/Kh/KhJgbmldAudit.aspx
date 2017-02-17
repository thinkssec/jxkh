﻿<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KhJgbmldAudit.aspx.cs" Inherits="Enterprise.UI.Web.Kh.KhJgbmldAudit" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            var khzq = $('#<%=Ddl_KHZQ.ClientID%>').val();
            if (khzq == "季度") {
                var jd = prompt('请输入考核季度的数字！如：1', "");
                //alert(isNaN(jd) + "   " + jd);
                if (isNaN(jd) || jd == null || jd == "") {
                    alert("请输入考核季度对应的数字！");
                    return false;
                }
                else {
                    $('#<%=Hid_KHJD.ClientID%>').val(jd);
                }
            }
            else if (khzq == "半年") {
                var yd = prompt('请输入数字！如：1：上半年；2:下半年', "");
                if (isNaN(yd) || yd == null || yd == "") {
                    alert("请输入对应的数字！");
                    return false;
                }
                else {
                    $('#<%=Hid_KHYD.ClientID%>').val(yd);
                }
            }
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.alert(tt, msg);
        }
        function aa() {
            if (confirm('删除不可恢复!您确定要删除本次所有数据？')) {
                return true;
            }
            return false;
        }
        function tb() {
            if (confirm('您确定要重新进行考核指标和单位的数据同步？（只能追加）')) {
                return true;
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <!--IPHONE Style-->
    <link href='/Resources/charisma/css/jquery.iphone.toggle.css' rel='stylesheet' />
    <script src="/Resources/charisma/js/jquery.iphone.toggle.js"></script>
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>机关部门领导审核</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                <%--考核年度：<asp:DropDownList ID="Ddl_Niandu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Niandu_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:TextBox ID="Txt_Khmc_Search" runat="server"></asp:TextBox>
                &nbsp;&nbsp;--%>
                <%--<asp:LinkButton ID="LnkBtn_Find" runat="server" class="easyui-linkbutton" iconCls="icon-search" plain="true"
                    OnClick="LnkBtn_Find_Click">查询</asp:LinkButton>--%>
                <asp:LinkButton ID="Btn_Ok" runat="server" Text="提交被考核单位确认" class="easyui-linkbutton" plain="true"
                    iconCls="icon-ok" OnClick="Btn_Ok_Click"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LinkButton2" runat="server" Text="退回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-cancel" OnClick="Btn_No_Click"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LinkButton1" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="LnkBtn_Cancel_Click"></asp:LinkButton>
                <%--<asp:LinkButton ID="Btn_Add" runat="server" Text="新增" class="easyui-linkbutton" plain="true"
                    iconCls="icon-add" OnClick="Btn_Add_Click"></asp:LinkButton>--%>
                <asp:HiddenField ID="Hid_KHJD" runat="server" />
                <asp:HiddenField ID="Hid_KHYD" runat="server" />
            </div>
            <div id="openwin" class="easyui-window" style="width: 600px; height: 360px" title="附件信息" closed="true" modal="true"></div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" AllowPaging="True"
                 OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="ID"
                PageSize="15">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="KHMC" HeaderText="考核名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>

                    <asp:TemplateField HeaderText="单位名称">
                        <ItemTemplate>
                            <asp:CheckBox ID="Cb_ID" runat="server" /><a href="/Module/KhX/Khzbpf.aspx?khid=<%#Eval("KEY3") %>&mbjgid=<%#Eval("ID") %>&back=KhJgbmldAudit&audit=1" ><%#Eval("KEY2")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="KEY1" HeaderText="考核项目">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="得分">
                        <ItemTemplate>
                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="状态">
                        <ItemTemplate>
                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>

                    
                    <%--<asp:TemplateField HeaderText="附件">
                        <ItemTemplate>
                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="允许查询">
                        <ItemTemplate>
                            <img alt="否" src="<%#(Eval("SFKC").ToRequestString()=="1")?"/Resources/Images/right.gif":"/Resources/Images/wrong.gif" %>" border="0" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("KHID") %>'
                                CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" ToolTip="编辑" />
                            &nbsp;|&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("KHID") %>'
                                CommandName="gbzt" ImageUrl="/Resources/Styles/icon/b.gif" ToolTip="改变状态" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <PagerSettings FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PageButtonCount="5"
                    PreviousPageText="上一页" />
                <PagerStyle CssClass="GridViewPagerStyle" />
            </asp:GridView>
            <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <div id="contents" class="ssec-form">
                    <div class="ssec-group ssec-group-hasicon">
                        <div class="icon-form"></div>
                        <span>编辑</span>
                    </div>
                    <asp:HiddenField ID="Hid_KHID" Value="" runat="server" />
                    <table>
                        <tr>
                            <td>考核名称
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_KHMC" runat="server" class="easyui-validatebox" required="true"
                                    missingMessage="必填项" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>考核类型
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_LXID" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
<%--                        <tr>
                            <td>应用版本
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_BBMC" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>考核年度
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_KHND" runat="server" class="easyui-numberbox" required="true" min="2014"
                                    max="2100" precision="0" missingMessage="必须填写4位数字"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>考核周期
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_KHZQ" runat="server">
                                    <asp:ListItem>季度</asp:ListItem>
                                    <asp:ListItem>半年</asp:ListItem>
                                    <asp:ListItem>年度</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>考核状态
                            </td>
                            <td>
                                <asp:DropDownList ID="Ddl_KHZT" runat="server">
                                    <asp:ListItem Text="进行中" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="已完成" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>是否允许查询结果
                            </td>
                            <td>
                                <input id="Chk_SFKC" data-no-uniform="true" type="checkbox" class="iphone-toggle" runat="server" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td>结束时间
                            </td>
                            <td>
                                <asp:Calendar ID="Cal_KSSJ" runat="server" Font-Size="9pt" Height="190px" NextPrevFormat="FullMonth"
                                    Width="350px">
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="White" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                                    <TodayDayStyle BackColor="#CCCCCC" />
                                </asp:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td>考核项目
                            </td>
                            <td>
                                <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" 
                                    CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="DZBID" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="序号">
                                            <ItemTemplate>
                                                <%#(Container.DataItemIndex + 1) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                        </asp:TemplateField>
                    <%--                    <asp:TemplateField HeaderText="考核项目名称">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="负责部门">
                                            <ItemTemplate>
                                                <%#Enterprise.Service.Perfo.Sys.SysBmjgService.GetBmjgName((int)Eval("FZBM")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="ZBMC" HeaderText="考核项目名称">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="考核项目">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Cb_DZB" runat="server" /><%#Eval("ZBMC") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="排序">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_PX" runat="server" class="easyui-numberbox"  min="0"
                                    max="999"  missingMessage="必须填写数字" Width="30" precision="1"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                        OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton" iconCls="icon-remove"
                        OnClientClick="return aa();" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>
                    <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back" OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('.iphone-toggle').iphoneStyle();
        });
    </script>
</asp:Content>

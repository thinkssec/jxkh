<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="KhfjList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.KhfjList"  EnableEventValidation="false"%>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Register Src="~/Component/UserControl/PopWinUploadMuti.ascx" TagPrefix="uc1" TagName="PopWinUploadMuti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            
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
                <span><asp:Label ID="Lb_Title" runat="server"></asp:Label></span>
            </div>
        </div>
        <div class="main-gridview">

                    <table id="Tb_MENU" runat="server" width="100%">
                        <tr>
                            <td><uc1:PopWinUploadMuti runat="server" ID="Txt_SCFJ"  Ext="Custom" Required="false"  />
                            </td>
                            <td width="80px">
                                <asp:LinkButton ID="BtnSave" runat="server" ClientIDMode="Static" CssClass="easyui-linkbutton" iconCls="icon-save" OnClick="BtnSave_Click" OnClientClick="showLoading();">保存</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">

                                <%=fjhtml %>
                            </td>
                        </tr>
                            
                                <%--<td   width="100px">&nbsp;&nbsp;&nbsp;&nbsp;<a href='' class="easyui-linkbutton" id="H_Down" runat="server">下载</a>&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>--%>
                        
                    </table>


                            <asp:HiddenField ID="Hid_MBJGID"  runat="server" />
            <div id="openwin" class="easyui-window" style="width: 600px; height: 360px" title="附件信息" closed="true" modal="true"></div>
             
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

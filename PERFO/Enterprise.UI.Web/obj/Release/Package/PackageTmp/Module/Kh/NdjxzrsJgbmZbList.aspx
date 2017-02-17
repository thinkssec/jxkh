<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="NdjxzrsJgbmZbList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.NdjxzrsJgbmZbList" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GridViewAlternatingRowStyle {
            background-color: #fff;
        }
    </style>
    <script type="text/javascript">
        function checkForm() {
            saveData();
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.alert(tt, msg);
        }
        function aa() {
            if (confirm('删除不可恢复!您确定要删除数据？')) {
                return true;
            }
            return false;
        }
        function saveData() {
            var v;
            v= $('#<%=Txt_JGKHMB.ClientID%>').val();
            $('#<%=Hid_JGKHMB.ClientID%>').val(v);
            v = $('#<%=Txt_JGKHNR.ClientID%>').val();
            $('#<%=Hid_JGKHNR.ClientID%>').val(v);
            v = $('#<%=Txt_JGPFBZ.ClientID%>').val();
            $('#<%=Hid_JGPFBZ.ClientID%>').val(v);
            v = $('#<%=Txt_JGWCSJ.ClientID%>').val();
            $('#<%=Hid_JGWCSJ.ClientID%>').val(v);
            v = $('#<%=Txt_ZZBFZ.ClientID%>').val();
            $('#<%=Hid_ZZBFZ.ClientID%>').val(v);
        }
        function showWin() {
            $('#editWinDiv').window('open');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>年度绩效考核责任书-指标列表</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核年度：<asp:DropDownList ID="Ddl_Niandu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Niandu_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                单位：<Ent:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;
                应用版本：<asp:DropDownList ID="Ddl_Bbmc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Bbmc_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="维护" class="easyui-linkbutton" plain="true"
                    iconCls="icon-save" OnClick="Btn_Add_Click"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Ldsh" runat="server" Text="提请审核" class="easyui-linkbutton" plain="true"
                    iconCls="icon-ok" OnClientClick="return confirm('您需要将制定好的考核指标报领导审核吗？');" OnClick="LnkBtn_Ldsh_Click"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Xiada" runat="server" Text="下达" class="easyui-linkbutton" plain="true"
                    iconCls="icon-shenhe" OnClick="LnkBtn_Xiada_Click" Visible="false"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Back" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="Btn_Back_Click"></asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" AllowPaging="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%--#(Container.DataItemIndex + 1) --%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZRSID" HeaderText="责任书名称" Visible="false">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="考核指标类型与权重">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核主要内容">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZZBFZ" HeaderText="分值">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="考核目标">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="完成时间">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="评分标准">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="add" ImageUrl="/Resources/Styles/icon/new.gif" ToolTip="新增考核指标" />
                            |
                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="bianji" ImageUrl="/Resources/Styles/icon/application_edit.png" ToolTip="修改考核指标" />
                            |
                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="shanchu" ImageUrl="/Resources/Common/img/icon/delete.gif" OnClientClick="return aa();" ToolTip="删除指标数据" />
                            |
                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="up" ImageUrl="/Resources/OA/easyui1.32/themes/icons/up.gif" ToolTip="上调位置" />
                            |
                            <asp:ImageButton ID="ImageButton5" runat="server" CommandArgument='<%#Eval("ZRSZBID") %>' CommandName="down" ImageUrl="/Resources/OA/easyui1.32/themes/icons/down.gif" ToolTip="下调位置" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="160px" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
            </asp:GridView>
            <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView2_RowDataBound" AllowPaging="True" OnPageIndexChanging="GridView2_PageIndexChanging"
                PageSize="15" Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#(Container.DataItemIndex + 1) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="责任书名称">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="JGBM" HeaderText="单位名称">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="考核类型">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SZND" HeaderText="考核年度">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FZBM" HeaderText="负责部门">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="指标数量">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZRSZT" HeaderText="责任书状态">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="XDRQ" HeaderText="下达日期" DataFormatString="{0:yyyy-MM-dd}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <PagerSettings FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PageButtonCount="5"
                    PreviousPageText="上一页" />
                <PagerStyle CssClass="GridViewPagerStyle" />
            </asp:GridView>
            <div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                </div>
            </div>
            <asp:HiddenField ID="Hid_ZRSZBID" Value="" runat="server" />
            <asp:HiddenField ID="Hid_LHZBBM" Value="" runat="server" />
            <asp:HiddenField ID="Hid_JGKHNR" Value="" runat="server" />
            <asp:HiddenField ID="Hid_ZZBFZ" Value="" runat="server" />
            <asp:HiddenField ID="Hid_JGKHMB" Value="" runat="server" />
            <asp:HiddenField ID="Hid_JGWCSJ" Value="" runat="server" />
            <asp:HiddenField ID="Hid_JGPFBZ" Value="" runat="server" />
            <div id="editWinDiv" data-options="title:'编辑窗口',collapsible:false,minimizable:false,maximizable:true,draggable:true,resizable:true,inline:false" class="easyui-window" style="width: 575px; height: 410px; overflow: hidden" closed="true" modal="true">
                <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                    <div id="contents" class="ssec-form">
                        <div class="ssec-group ssec-group-hasicon">
                            <div class="icon-form"></div>
                            <span>编辑</span>
                        </div>
                        <table border="1">
                            <tr>
                                <td>考核指标类型</td>
                                <td>
                                    <asp:Label ID="Lbl_ZBMC" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>考核主要内容</td>
                                <td>
                                    <asp:TextBox ID="Txt_JGKHNR" runat="server" TextMode="MultiLine" Height="60px" Width="450px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>分值</td>
                                <td>
                                    <asp:TextBox ID="Txt_ZZBFZ" runat="server" Width="100px" class="easyui-numberbox" precision="1" max="200" min="0" missingMessage="只能填数字"></asp:TextBox>
                                    (<font color="red">注意：分值的合计值不应超过同一类型指标的总权重!</font>)
                                </td>
                            </tr>
                            <tr>
                                <td>考核目标</td>
                                <td>
                                    <asp:TextBox ID="Txt_JGKHMB" runat="server" TextMode="MultiLine" Height="60px" Width="450px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>完成时间</td>
                                <td>
                                    <asp:TextBox ID="Txt_JGWCSJ" runat="server" Width="450px" MaxLength="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>评分标准</td>
                                <td>
                                    <asp:TextBox ID="Txt_JGPFBZ" runat="server" TextMode="MultiLine" Height="60px" Width="450px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                            OnClientClick="return checkForm();" OnClick="LnkBtn_Upd_Click">保存</asp:LinkButton>
                        <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                            OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var sign = '<%=Pnl_Edit.Visible%>';
            if (sign == "True") showWin();
        });
    </script>
</asp:Content>

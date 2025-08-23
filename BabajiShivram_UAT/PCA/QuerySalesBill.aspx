<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuerySalesBill.aspx.cs" Inherits="PCA_QuerySalesBill" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb1" %>--%>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>

    <script type="text/javascript">
        function GridSelectAllColumn(spanChk) {
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++) {
                if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                    elm[i].click();
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function numeric(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode;
            if (unicode == 8 || unicode == 9 || (unicode >= 48 && unicode <= 57)) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>

            <div align="center">
                <asp:ValidationSummary ID="vsLRCopy" runat="server" ValidationGroup="vgLRCopy" ShowSummary="true" ShowMessageBox="false" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="false" ValidationGroup="RequiredField" CssClass="errorMsg" />
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:Label ID="Label2" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset id="mainfield" runat="server">
                <legend>View User Details Data</legend>
                <div>
                    <asp:Panel ID="pnlFilter" runat="server" Height="50%">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                </div>
                <asp:GridView ID="gvotherDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Style="white-space: normal"
                    PagerStyle-CssClass="pgr" DataKeyNames="invmstid" AllowPaging="True" AllowSorting="True" Width="100%"
                    PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="PCDSqlDataSource" OnRowCommand="gvotherDetail_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl"   ItemStyle-Width="3%">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="txt_InvoiceNo" HeaderText="Inovice No" Visible="false"/>
                        <asp:BoundField DataField="txt_Jobno" HeaderText="Job Number" ItemStyle-Width="8%"/>
                        <asp:BoundField DataField="ddl_consignname" HeaderText="Customer Name"  ItemStyle-Width="12%"/>
                        <asp:BoundField DataField="invdate" HeaderText="Billing Advise Date"   ItemStyle-Width="5%"/>
                        <asp:BoundField DataField="PSSSTATUS" HeaderText="PSSSTATUS" Visible="false"/>
                        <asp:BoundField DataField="PSSREMARKS" HeaderText="Bill Error"   ItemStyle-Width="12%"/>
                        <asp:BoundField DataField="PSSDRFNO" HeaderText="PSSDRFNO"  Visible="false"/>
                        <asp:BoundField DataField="PSSDRFDATE" HeaderText="PSSDRFDATE"  Visible="false"/>
                        <asp:TemplateField HeaderText="Sent" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdninvmstid" runat="server" Value='<%#Eval("invmstid")%>' />
                                <%--<asp:LinkButton ID="lnkSend" runat="server" Text="Bill Query Resolve" CommandName="Bill Query Resolve" CommandArgument='<%#Eval("invmstid") %>'
                                        Font-Size="Larger"></asp:LinkButton>--%>
                                <asp:Button ID="btnbillresolve" runat="server" Text="Bill Query Resolve" CommandName="Bill Query Resolve" CommandArgument='<%#Eval("invmstid") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <asp:Literal ID="PopupBox" runat="server"></asp:Literal>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetviewOtherDetails" SelectCommandType="StoredProcedure">
                    <%--<SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>--%>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="gvotherDetail" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>




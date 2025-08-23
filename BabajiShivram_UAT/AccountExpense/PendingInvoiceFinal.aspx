<%@ Page Title="Pending Final Invoice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingInvoiceFinal.aspx.cs" Inherits="AccountExpense_PendingInvoiceFinal" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <fieldset><legend>Final Invoice Submission</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
        </div>
        
        <div class="clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <div class="fleft" style="margin-left:30px; padding-top:3px;">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="InvoiceId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" OnRowDataBound="gvDetail_RowDataBound"
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Eval("InvoiceId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Proforma Invoice No" SortExpression="InvoiceNo"/>
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <%--<asp:BoundField DataField="VendorGSTNo" HeaderText="Supplier GSTIN No" SortExpression="VendorGSTNo" />--%>
                <%--<asp:BoundField DataField="PaymentTerms" HeaderText="Vendor Payment Terms In Days" SortExpression="PaymentTerms" />--%>
                <%--<asp:BoundField DataField="PaymentRequiredDate" HeaderText="Payment Required Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}"/>--%>
                <asp:BoundField DataField="InvoiceAmount" HeaderText="Total Value" SortExpression="InvoiceAmount" />
                <asp:BoundField DataField="KAM" HeaderText="KAM" SortExpression="KAM" />
                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                <asp:BoundField DataField="InstrumentDate" HeaderText="Payment Date" SortExpression="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:TemplateField HeaderText="Status" Visible="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkStatus" CommandName="status" runat="server" CommandArgument='<%#Eval("InvoiceId") +";"+ Eval("IsFinalInvoicePending") %>'
                            Text='<%#Eval("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>

         <%--  START : MODAL POP-UP FOR Cancel EXPENSE  --%>

            <div>
                <asp:LinkButton ID="lnkStatusExp" runat="server"></asp:LinkButton>
            </div>

            <div id="divOpExpense">
                <cc1:ModalPopupExtender ID="StatusModalPopupExtender" runat="server" CacheDynamicResults="false" PopupControlID="panStatusJob" 
                    CancelControlID="imgbtnStatusExp" TargetControlID="lnkStatusExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panStatusJob" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>

                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnStatusExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <div class="m">
                                    <center>
                                        <asp:Label ID="lblError_status" runat="server"></asp:Label>
                                    </center>
                                </div>
                                <tr>
                                    <td>BS Job Number</td>
                                    <td>
                                        <asp:Label ID="lblJobNumber" runat="server"></asp:Label>
                                    </td>
                                    <td>Branch</td>
                                    <td>
                                        <asp:Label ID="lblBranch1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Invoice No</td>
                                    <td>
                                        <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("IsFinalInvoicePending") %>' />
                                    </td>
                                    <%--<td>Expense Type</td>
                                    <td>
                                        <asp:Label ID="lblexpenseType1" runat="server"></asp:Label>
                                    </td>--%>
                                </tr>
                            </table>
                        </div>
                        <asp:RequiredFieldValidator ID="RFVReason" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Enter Cancel Remark" ValidationGroup="ValidateCancel" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Remark                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="4" MaxLength="200" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnUpdateJob" runat="server" Text="Update" OnClick="btnUpdateJob_Click" ValidationGroup="ValidateCancel"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>

        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingInvoiceForFinal" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>


<%@ Page Title="Edit Transport Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TransBillEdit.aspx.cs" Inherits="AccountTransport_TransBillEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:Scriptmanager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upFillDetails" runat="server">
    <ContentTemplate>
        <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Audit L2</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Submit/Send to Audit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required" />
                        
                        <asp:Button ID="btnBack" Text="Back" runat="server" CausesValidation="false" />
                    </div>
                    <fieldset id="fsGeneralTransportDetails" runat="server">
                    <legend>Transport Request Detail</legend>
                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Ref No.
                                </td>
                                <td>
                                    <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                                <td>Job No.
                                </td>
                                <td>
                                    <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Customer Name
                                </td>
                                <td>
                                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                                </td>
                                <td>Consignee Name
                                </td>
                                <td>
                                    <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Truck Request Date
                                </td>
                                <td>
                                    <asp:Label ID="lblTruckRequestDate" runat="server"></asp:Label>
                                </td>
                                <td>Vehicle Place Require Date
                                </td>
                                <td>
                                    <asp:Label ID="lblVehiclePlaceDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDispatch_Title" runat="server" Text="Number of vehicles required"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDispatch_Value" runat="server"></asp:Label>
                                </td>
                                <td>Location
                                </td>
                                <td>
                                    <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Destination
                                </td>
                                <td>
                                    <asp:Label ID="lblDestination" runat="server"></asp:Label>
                                </td>
                                <td>Dimension</td>
                                <td>
                                    <asp:Label ID="lblDimension" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Gross Weight (Kgs)
                                </td>
                                <td>
                                    <asp:Label ID="lblGrossWeight" runat="server"></asp:Label>
                                </td>
                                <td>Cont 20"
                                </td>
                                <td>
                                    <asp:Label ID="lblCon20" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Cont 40"
                                </td>
                                <td>
                                    <asp:Label ID="lblCon40" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDelExportType_Title" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDelExportType_Value" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                    <legend>Vehicle Rate Detail</legend>
                        <div style="width: 1100px;overflow-x: scroll">
                        <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                        DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                        PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                        <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" ReadOnly="true" />
                        <asp:BoundField DataField="LocationFrom" HeaderText="Delivery From" ReadOnly="true" />
                        <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" />
                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Advance" HeaderText="Advance(%)" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="TollCharges" HeaderText="Toll" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                        </asp:GridView>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Selling Rate Detail</legend>
                            <div>
                                <div style="width: 1100px;overflow-x: scroll">
                                    <asp:GridView ID="gvSellingDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Width="90%" 
                                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceSellingRate" 
                                         Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                                            <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                                            <%--<asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellTollCharges" HeaderText="Toll Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellOtherCharges" HeaderText="Other Amount" ReadOnly="true" />
                                            --%>          
                                            <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" ReadOnly="true" />--%>
                                            <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                    </fieldset>
                    <fieldset runat="server" id="fldVendor">
                    <legend>Vendor Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Vendor Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Credit Terms (Days)
                            </td>
                            <td>
                                <asp:Label ID="lblCreditTerms" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Payment Due Date
                            </td>
                            <td>
                                <asp:Label ID="lblPaymentDueDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                Payment Request Date
                            </td>
                            <td>
                                <asp:Label ID="lblPatymentRequestDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Account Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>                            
                            <td>
                                Account No
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountNo" runat="server"></asp:Label>
                            </td>
                            
                            <td>
                                IFSC
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountIFSC" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset>
                    <legend>Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing Party Name
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblBillingPartyName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceType" runat="server" Text="Proforma"></asp:Label>
                            </td> 
                            <td>
                                RIM/NON RIM
                            </td>
                            <td>
                                <asp:Label ID="lblRIM" runat="server" Text="RIM"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Invoice Value
                            </td>
                            <td>
                                <asp:Label ID="lblTotalInvoiceValue" runat="server" Font-Bold="true" Font-Size="12"></asp:Label>
                            </td>
                            <td>
                                Deduction
                            </td>
                            <td>
                                <asp:Label ID="lblDeduction" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Advance Paid
                            </td>
                            <td>
                                <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                            </td>
                            <td>
                                TDS
                            </td>
                            <td>
                                <asp:Label ID="lblTDSAmount" runat="server" Font-Bold="true" Font-Size="12"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Request Remark
                            </td>
                            <td>
                                <asp:Label ID="lblRequestRemark" runat="server"></asp:Label>
                            </td>
                            <td>
                                Request By
                            </td>
                            <td>
                                <asp:Label ID="lblRequestBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td>
                               Transport Dept Remark
                               <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required" InitialValue=""
                                   ControlToValidate="txtRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset id="fldInvoiceitem" runat="server">
                    <div class="clear">
                    </div>
                        <legend>Charge Detail</legend>
                        <div class="clear"></div>
                            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                                <asp:GridView ID="gvCharges" runat="server" CssClass="table" AutoGenerateColumns="false"
                                    AllowPaging="false" DataKeyNames="lid" DataSourceID="InvoiceDataSource" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HSN" HeaderText="HSN" />
                                        <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
                                        <asp:BoundField DataField="ChargeCode" HeaderText="Charge Code" />
                                        <asp:BoundField DataField="OtherDeduction" HeaderText="Deduction" />
                                        <asp:BoundField DataField="Amount" HeaderText="Total Value" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                    </fieldset>
                    
                    <fieldset runat="server" id="fldTDSItem" visible="false">
                    <legend>TDS</legend>
                    
                    TDS Applicable: <b><asp:Label ID="lblTDSApplicable" Text="No" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    TDS Rate Type:<b> <asp:Label ID="lblTDSRateType" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    TDS Rate:<b> <asp:Label ID="lblTDSRate" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    
                    Total TDS: &nbsp;&nbsp; <b><asp:Label ID="lblTotalTDS" runat="server"></asp:Label></b>
                        
                    <div class="clear"></div>
                    <div id="DivTDS2" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:GridView ID="GridViewTDS" runat="server" CssClass="table" AutoGenerateColumns="false"
                            AllowPaging="false" DataKeyNames="lid" DataSourceID="InvoiceDataSource" Visible="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField  DataField="TDSLedgerCode" HeaderText="TDS Ledger Code" />
                                <asp:BoundField  DataField="TDSRateTypeName" HeaderText="Rate Type" />
                                <asp:BoundField  DataField="NetTaxableValue" HeaderText="Net Taxable value" />
                                <asp:BoundField  DataField="TDSRate" HeaderText="TDS Rate" />
                                <asp:BoundField  DataField="TDSAmount" HeaderText="TDS Amount" />
                                <asp:BoundField  DataField="NetTDSPayable" HeaderText="Net Payable" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    
                </fieldset>
                    
                    <fieldset runat="server" id="fldPayment">
                    <legend>Payment Detail</legend>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Net Payable Amount Rs. <asp:Label ID="lblNetPayable" runat="server"></asp:Label>
                    <div class="clear">
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Payment
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                    <asp:ListItem Text="Full Payment" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Partial Payment" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Amount
                            </td>
                            <td>
                                <asp:Label ID="txtPayAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Payment Mode
                                <asp:RequiredFieldValidator ID="rfvPaymentType" runat="server" ValidationGroup="RequiredAudit" InitialValue="0"
                                     ControlToValidate="ddlPaymentType" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                    DataTextField="sName" DataValueField="lid" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Bank Name
                            </td>                        
                            <td>
                                <asp:DropDownList ID="ddBabajiBankName" runat="server" Enabled="false"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Account/Cash Book
                                <asp:RequiredFieldValidator ID="rfvBank" runat="server" ValidationGroup="RequiredAudit" InitialValue="0"
                                     ControlToValidate="ddBabajiBankAccount" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBankAccount" runat="server" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Fund Transfer From Live Tracking ?
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblFundTransferFromLiveTracking" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset>
                    <legend>Document</legend>
                    <asp:GridView ID="gvDocument" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="lID" AllowPaging="True" AllowSorting="True" PageSize="20"
                        DataSourceID="DataSourcePaymentDoument" OnRowCommand="gvDocument_RowCommand" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>                                 
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"/>
                            <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName"/>
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="View" 
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView> 
                </fieldset>
                    <fieldset>
                <legend>History</legend>
                <asp:GridView ID="gvInvoiceHistory" runat="server" AutoGenerateColumns="False"
                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                    DataKeyNames="lId" DataSourceID="DataSourceStatusHistory" CellPadding="4"
                    AllowPaging="True" AllowSorting="True" PageSize="40">
                    <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Status" DataField="StatusName" />
                <%--<asp:BoundField HeaderText="Remark" DataField="sRemark" />--%>
                <asp:TemplateField HeaderText="Remark">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width: 400px; white-space:normal;">
                        <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="User" DataField="UserName" />
                <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                </Columns>
                </asp:GridView>
                </fieldset>
                    <fieldset>
                    <legend>Previous Payment Request</legend>   
                        <asp:GridView ID="grdJobHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="RequestId" AllowPaging="True" AllowSorting="True" PageSize="20"
                        DataSourceID="DataSourceJobHistory" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" SortExpression="TRRefNo"/>
                            <%--<asp:BoundField DataField="FARefNo" HeaderText="BS Job No" SortExpression="FARefNo"/>--%>
                            <%--<asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName"/>--%>
                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                            <asp:BoundField DataField="Amount" HeaderText="Total Amt" SortExpression="TotalAmount" />
                            <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amt" SortExpression="PaidAmount" />
                            <asp:BoundField DataField="VendorName" HeaderText="Paid To" SortExpression="VendorName" ReadOnly="true"/>
                            <asp:BoundField DataField="RequestBy" HeaderText="Created By" SortExpression="CreatedBy"/>
                            <asp:BoundField DataField="RequestDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                </fieldset>
            </div>
        <div id="divDatasourc"> 
            <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetTransRateDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="PayRequestId" SessionField="TransPayId" />
                    </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetVehicleSellingDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="PayRequestId" SessionField="TransPayId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="InvoiceDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TRS_GetPaymentItem" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="RequestId" SessionField="TransPayId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetInvoiceDocument" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="PayRequestId" SessionField="TransPayId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TR_GetStatusHistory" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="RequestId" SessionField="TransPayId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceJobHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetInvoicePrevHistory" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="PayRequestId" SessionField="TransPayId" />
                </SelectParameters>
            </asp:SqlDataSource>
             <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


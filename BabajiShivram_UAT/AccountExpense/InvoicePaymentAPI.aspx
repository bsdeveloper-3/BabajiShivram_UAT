<%@ Page Title="Invoice Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoicePaymentAPI.aspx.cs" 
    Inherits="AccountExpense_InvoicePaymentAPI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </asp:Scriptmanager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                    Please do not press Refresh button
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ConfirmSubmit()
        {
            var objAmount = $get('<%=txtPayAmount.ClientID%>').value;

            return confirm('Are you sure for Payment of Rs. ' + objAmount);
        }

    </script>
    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnInvoiceType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNetPayableAmount" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Bold="true" Font-Size="Medium"></asp:Label>

                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Payment Confirmation</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            <asp:Button ID="btnHold" Text="Payment On Hold" OnClick="btnHold_Click" ValidationGroup="Required" runat="server" />
                            <asp:Button ID="btnReject" Text="Reject Payment" OnClick="btnReject_Click" ValidationGroup="Required" OnClientClick="return confirm('Are you sure to Reject Payment?')" runat="server" />
                            Hold/Reject Remark
                            <asp:TextBox ID="txtRejectRemark" runat="server" TextMode="MultiLine" MaxLength="200" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRejectRemark" runat="server" ValidationGroup="Required" InitialValue=""
                                ControlToValidate="txtRejectRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    </table>
                    <fieldset>
                    <fieldset>
                    <legend>Job Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblInvoiceMode" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                    <asp:ListItem Text="Tax Invoice" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Credit Note" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Debit Note" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Number
                            </td>
                            <td width="80%" colspan="3">
                                <asp:Label ID="lblJobNumber" Width="160px" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                            </td>
                            <td>
                                Consignee
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>BE No
                            </td>
                            <td>
                                <asp:Label ID="lblBENo" runat="server"></asp:Label>
                            </td>
                            <td>
                                BL No
                            </td>
                            <td>
                                <asp:Label ID="lblBLNo" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>Weight
                            </td>
                            <td>
                                <asp:Label ID="lblWeight" runat="server"></asp:Label>
                            </td>
                            <td>
                                Container Count
                            </td>
                            <td>
                                <asp:Label ID="lblContainerCount" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>
                                Assessable Value
                            </td>
                            <td>
                                <asp:Label ID="lblAssessableValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                IGST Amount
                            </td>
                            <td>
                                <asp:Label ID="lblIGSTAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Expense Type
                            </td>
                            <td>
                                <asp:Label ID="lblExpenseType" runat="server"></asp:Label>
                            </td>
                            <td>
                                Duty Amount
                            </td>
                            <td>
                                <asp:Label ID="lblDutyAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tdInterestAmt" runat="server" visible="false">
                            <td>Interest</td>
                            <td>
                                <asp:Label ID="lblInterestAmount" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    </fieldset>
                    
                     <fieldset>
                    <legend>Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing GSTN
                            </td>
                            <td>
                                <asp:Label ID="lblBillingGSTN" runat="server"></asp:Label>
                            </td>
                            <td>
                                Billing PAN
                            </td>
                            <td>
                                <asp:Label ID="lblBillingPAN" runat="server"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                             <td>
                                Billing Party Name
                            </td>
                            <td>
                                <asp:Label ID="lblBillingPartyName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Current O/S
                            </td>
                            <td>
                                <asp:Label ID="lblCurrentOutstanding" runat="server"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                             <td>
                                RIM/NON RIM
                            </td>
                            <td>
                                <asp:Label ID="lblRIM" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceType" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Invoice No
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                            </td>
                            <td>
                                Invoice Date
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Total Invoice Value
                            </td>
                            <td>
                                <asp:Label ID="lblTotalInvoiceValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:Label ID="lblTaxableValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Currency
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceCurrency" runat="server"></asp:Label>
                            </td>
                            <td>
                                Exhange Rate
                            </td>
                            <td>
                                <asp:Label ID="lblExchangeRate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                GST Amount
                            </td>
                            <td>
                                <asp:Label ID="lblGSTValue" runat="server"></asp:Label>
                            </td>
                            <td>Deduction</td>
                            <td>
                                <asp:Label ID="lblOtherDeduction" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Advance Received
                            </td>
                            <td>
                                <asp:Label ID="lblAdvanceReceived" runat="server"></asp:Label>
                            </td>
                            <td>
                                Advance Amount
                            </td>
                            <td>
                                <asp:Label ID="lblAdvanceAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </fieldset>       
                </fieldset>
                <fieldset runat="server" id="fldInvoiceItem" visible="true">
                    <div class="clear">
                    </div>
                        <legend>Charge Detail</legend>
                        Transaction Type <asp:Label ID="lblTransactionTypeName" runat="server"></asp:Label><br />
                        <asp:CheckBox ID="chkNoITC" runat="server" Text="No ITC" Enabled="false" />
                        <div class="clear"></div>
                            <div id="DivCharge" runat="server" style="max-height: 550px; overflow: auto;">
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
                                        <asp:BoundField DataField="TaxAmount" HeaderText="Taxable Value" />
                                        <%--<asp:BoundField DataField="TaxAmountINR" HeaderText="Taxable Value (INR)" DataFormatString="{0:0.00}" />--%>
                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency" />
                                        <asp:BoundField DataField="InvoiceCurrencyExchangeRate" HeaderText="Rate" />
                                        <asp:BoundField DataField="OtherDeduction" HeaderText="Deduction" />
                                        <asp:BoundField DataField="IGSTRate" HeaderText="IGST Rate" />
                                        <asp:BoundField DataField="IGSTAmount" HeaderText="IGST Amt" />
                                        <asp:BoundField DataField="CGSTRate" HeaderText="CGST Rate" />
                                        <asp:BoundField DataField="CGSTAmount" HeaderText="CGST Amt" />
                                        <asp:BoundField DataField="SGSTRate" HeaderText="SGST Rate" />
                                        <asp:BoundField DataField="SGSTAmount" HeaderText="SGST Amt" />
                                        <asp:BoundField DataField="Amount" HeaderText="Total Value" />
                                        <%--<asp:BoundField DataField="Remark" HeaderText="Remark" />--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                </fieldset>
                <fieldset runat="server" id="fldTDSItem" visible="false">
                    <legend>TDS</legend>
                    
                    TDS Applicable <asp:Label ID="lblTDSApplicable" Text="No" runat="server"></asp:Label>&nbsp;&nbsp;
                    TDS Rate Type <asp:Label ID="lblTDSRateType" runat="server"></asp:Label>&nbsp;&nbsp;
                    TDS Rate <asp:Label ID="lblTDSRate" runat="server"></asp:Label>&nbsp;&nbsp;
                    
                    Total TDS &nbsp;&nbsp; <asp:Label ID="lblTotalTDS" runat="server"></asp:Label>
                        
                    <div class="clear">
                    </div>
                    
                </fieldset>

                <fieldset runat="server" id="fldRCMItem" visible="false">
                    <legend>RCM</legend>
                    RCM Applicable &nbsp;&nbsp; <asp:Label ID="lblRCMYes" runat="server" Text="No" ></asp:Label>
                    RCM Rate &nbsp;&nbsp;<asp:Label ID="lblRCMRate" runat="server"></asp:Label>
                    GST : <asp:Label ID="lblRCMGstType" runat="server">  </asp:Label>
                    <div class="clear">
                    </div>
                    <div class="clear"></div>
                    
                </fieldset>
                <fieldset runat="server" id="fldVendor">
                    <legend>Vendor Detail/Paid To</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Vendor GSTIN
                            </td>
                            <td>
                                <asp:Label ID="lblVendorGSTIN" runat="server"></asp:Label>
                            </td>
                            <td>
                                PAN
                            </td>
                            <td>
                                <asp:Label ID="lblPAN" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Vendor Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Vendor Type
                            </td>
                            <td>
                                <asp:Label ID="lblVendorType" runat="server"></asp:Label>
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
                                Credit Terms (Days)
                            </td>
                            <td>
                                <asp:Label ID="lblCreditTerms" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Patyment Request Date
                            </td>
                            <td>
                                <asp:Label ID="lblPatymentRequestDate" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <tr>
                            <td>
                                Beneficiary Name
                            </td>
                            <td>
                                <asp:Label ID="lblBeneficiaryName" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Bank Account No
                            </td>
                            <td>
                                <asp:Label ID="lblBankAccountNo" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                            <td>
                                Bank Name
                            </td>
                            <td>
                                <asp:Label ID="lblBankName" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Bank IFSC
                            </td>
                            <td>
                                <asp:Label ID="lblBankIFSC" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        
                    </table>
                    </fieldset>
                    <fieldset>
                        <legend>Invoice History</legend>
                        <asp:GridView ID="gvInvoiceHistory" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lId" DataSourceID="DataSourceInvoiceHistory" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                <asp:BoundField HeaderText="Remark" DataField="sRemark" />
                                <asp:BoundField HeaderText="User" DataField="UserName" />
                                <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                </Columns>
                        </asp:GridView>
                    </fieldset>
                <fieldset runat="server" id="fldPayment">
                    <legend>Payment Detail</legend>
                    <asp:Button ID="btnSavePayment" runat="server" Text="Start Fund Transfer" OnClientClick='if(!ConfirmSubmit()) return false;'
                       ValidationGroup="RequiredPay" OnClick="btnSavePayment_Click"></asp:Button>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Net Payable Amount Rs. <asp:Label ID="lblNetPayable" runat="server" Font-Size="12" Font-Bold="true"></asp:Label>
                        <asp:HiddenField ID="hdnPaymentId" runat="server" Value="0" />
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
                                Amount (in Rs)
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayAmount" runat="server" MaxLength="12" TextMode="Number" Width="120px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Payment Mode
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" >
                                    <asp:ListItem Text="NEFT" Value="6" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="RTGS" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Currency
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency"
                                   Enabled="false"  AppendDataBoundItems="true" DataValueField="lId" DataTextField="Currency" TabIndex="10">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                 <asp:TextBox ID="txtExchangeRate" runat="server" Width="50px" Text="1" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Name
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBank" runat="server" Enabled="false">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Account No
                            </td>
                            <td>
                                <asp:Label ID="lblBabajiAccountNo" runat="server"></asp:Label>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                Payment Remark
                                <asp:RequiredFieldValidator ID="rfvPaymentRemark" runat="server" ValidationGroup="RequiredPay" InitialValue=""
                                     ControlToValidate="txtPaymentRemark" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPaymentRemark" runat="server" MaxLength="200" Width="200px" TextMode="MultiLine"></asp:TextBox>

                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                <legend>Previous Payment Request</legend>   
                    <asp:GridView ID="grdJobHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                    PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                    DataSourceID="DataSourceJobHistory" PagerSettings-Position="TopAndBottom" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"/>
                        <%--<asp:BoundField DataField="FARefNo" HeaderText="BS Job No" SortExpression="FARefNo"/>--%>
                        <%--<asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName"/>--%>
                        <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" />
                        <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amt" SortExpression="PaidAmount" />
                        <asp:BoundField DataField="VendorName" HeaderText="Paid To" SortExpression="VendorName" ReadOnly="true"/>
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"/>
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                    </Columns>
                </asp:GridView>
                </fieldset>
                <fieldset>
                    <legend>Invoice Document</legend>
                    <asp:GridView ID="gvDocument" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
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
            
        </div>
            
            <div id="divDatasourc"> 
            <asp:SqlDataSource ID="DataSourceJobHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceJobPrevHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="InvoiceDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceItem" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoicePayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceInvoiceHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>


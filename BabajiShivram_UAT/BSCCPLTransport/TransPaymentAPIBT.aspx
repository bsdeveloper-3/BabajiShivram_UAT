<%@ Page Title="Babaji - Transporter Payment" Language="C#" MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" CodeFile="TransPaymentAPIBT.aspx.cs" Inherits="BSCCPLTransport_TransPaymentAPI" %>
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
                    <asp:HiddenField ID="hdnPaymentId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

            <fieldset>
                    <legend>Payment Confirmation</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Remark
                            <asp:TextBox ID="txtRejectRemark" runat="server" TextMode="MultiLine" MaxLength="200" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRejectRemark" runat="server" ValidationGroup="Required" InitialValue=""
                                ControlToValidate="txtRejectRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:Button ID="btnHold" Text="Payment On Hold" OnClick="btnHold_Click" ValidationGroup="Required" runat="server" />
                            <asp:Button ID="btnReject" Text="Reject Payment" OnClick="btnReject_Click" ValidationGroup="Required" OnClientClick="return confirm('Are you sure to Reject Payment?')" runat="server" />
                            <asp:Button ID="btnPayFrom" Text="Move To Manual Payment" OnClick="btnPayFrom_Click" ValidationGroup="Required" OnClientClick="return confirm('Are you sure to Move To Manual Payment Tab?')" runat="server" />
                        </td>
                    </tr>
                    </table>
                    <fieldset id="fsGeneralTransportDetails" runat="server">
                    <legend>Transport Request Detail</legend>
                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Job No.
                                </td>
                                <td>
                                    <asp:Label ID="lblJobNo" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                                <td>Ref No.
                                </td>
                                <td>
                                    <asp:Label ID="lblTRRefNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Customer
                                </td>
                                <td>
                                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                                </td>
                                <td>Consignee
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
                            <td>Location
                            </td>
                            <td>
                                <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                            </td>
                            <td>Destination
                            </td>
                            <td>
                                <asp:Label ID="lblDestination" runat="server"></asp:Label>
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
                            <td>
                                <asp:Label ID="lblDelExportType_Title" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDelExportType_Value" runat="server"></asp:Label>
                            </td>
                            <td>Cont 40"
                            </td>
                            <td>
                                <asp:Label ID="lblCon40" runat="server"></asp:Label>
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
                        <asp:BoundField DataField="OtherCharges" HeaderText="Union" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <%--<asp:BoundField DataField="TotalAmount" HeaderText="Total" ItemStyle-Font-Bold="true" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />--%>
                        <%--<asp:BoundField DataField="LRNo" HeaderText="LR No" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" Visible="false" />
                        <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" Visible="false" />--%>
                        <%--<asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />--%>
                        <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                    </Columns>
                        </asp:GridView>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Selling Rate Detail</legend>
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
                                        <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                                        <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
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
                                Invoice No
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceNo" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
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
                                        <asp:BoundField DataField="TaxAmount" HeaderText="Taxable Value" />
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
                    <asp:Button ID="btnSavePayment" runat="server" Text="Start Fund Transfer" OnClientClick='if(!ConfirmSubmit()) return false;'
                       ValidationGroup="RequiredPay" OnClick="btnSavePayment_Click"></asp:Button>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Net Payable Amount <asp:Label ID="lblNetPayable" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                        

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
                                Paid Amount
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayAmount" runat="server" MaxLength="12" TextMode="Number" Width="120px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Payment Mode
                                <asp:RequiredFieldValidator ID="rfvPaymentType" runat="server" ValidationGroup="RequiredPay" InitialValue="0"
                                     ControlToValidate="ddlPaymentType" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                   Enabled="false"  DataTextField="sName" DataValueField="lid">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Bank Name
                            </td>                        
                            <td>
                                <asp:DropDownList ID="ddBabajiBankName" runat="server" Enabled="false">
                                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Name
                                <asp:RequiredFieldValidator ID="rfvBank" runat="server" ValidationGroup="RequiredPay" InitialValue="0"
                                     ControlToValidate="ddBabajiBankAccount" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBankAccount" runat="server" Enabled="false">
                                <%--<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Yes BANK LTD - 001790600004039" Value="Y20" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="AXIS BANK LTD - 912020005850066" Value="B20"></asp:ListItem>
                                    <asp:ListItem Text="H D F C BANK" Value="B11"></asp:ListItem>
                                    <asp:ListItem Text="IDBI BANK JNPT" Value="B18"></asp:ListItem>
                                    <asp:ListItem Text="YES BANK LTD. MUMBAI" Value="YBL000"></asp:ListItem>--%>
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
                <fieldset>
                    <legend>Bill Detail</legend>
                    <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJob" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvBillDetail_RowCommand"
                            OnRowDataBound="gvBillDetail_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompany" runat="server" Text='<%#Eval("Company")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BJV No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBJVNo" runat="server" Text='<%#Eval("BJVNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("INVNO")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />
                                <asp:BoundField HeaderText="Adjustment Amount" DataField="ADJAmount" />
                                <asp:BoundField HeaderText="Adjustment Date" DataField="ADJDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:TemplateField HeaderText="View Bill">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBillView" runat="server" Text="View" CommandName="View" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
            <asp:SqlDataSource ID="DataSourceBillJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetPendingBillDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" ConvertEmptyStringToNull="true"  />
                    <asp:Parameter Name="ModuleId" DefaultValue="1"/>
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



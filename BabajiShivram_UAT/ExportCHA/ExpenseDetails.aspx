<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpenseDetails.aspx.cs"
    Inherits="ExportCHA_ExpenseDetails" Title="Job Expense Detail" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
   
    <div align="center">
        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
        <asp:Label ID="lblResult" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="clear"></div>
    <fieldset>
        <legend>Job Expense Entry</legend>    
    
    <asp:FormView ID="fvExpense" runat="server" DefaultMode="Insert" Width="100%"
       DataKeyNames="lId" OnDataBound="fvExpense_DataBound">
     <EmptyDataTemplate>
        <div>
            <asp:Button ID="btnNew" runat="server" Text="New Expense" OnClick="btnNewExpense_Click" />
            <asp:Button ID="btnNewCancel" Text="Back To Expense Detail" runat="server" CausesValidation="false"
                OnClick="btnNewCancel_OnClick" />
        </div>
     </EmptyDataTemplate>
     <ItemTemplate>
        <div>
            <asp:Button ID="btnNew" runat="server" Text="New Expense" OnClick="btnNewExpense_Click" />
            <asp:Button ID="btnNewCancel" Text="Back To Expense Detail" runat="server" CausesValidation="false"
                OnClick="btnNewCancel_OnClick" />
        </div>
     </ItemTemplate>
    <InsertItemTemplate>
        <div>
            <asp:Button ID="btnAdd" runat="server" ValidationGroup="Required" Text="Save" OnClick="btnAdd_Click" />
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false"
                OnClick="btnCancel_OnClick" />
        </div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Charges / Expense Type
                    <asp:RequiredFieldValidator ID="RFVType" runat="server" InitialValue="0" ControlToValidate="ddlExpenseType" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Select Charges/Expense Type" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddlExpenseType" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    Expense Amount
                    <asp:RequiredFieldValidator ID="RFVEAmount" runat="server" ControlToValidate="txtEAmount" SetFocusOnError="true" 
                        Text="*" ErrorMessage="Please Enter Expense Amount" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompAmount" ControlToValidate="txtEAmount" runat="server" SetFocusOnError="true"
                     Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Valid Expense Amount" Text="Invalid Amount" ValidationGroup="Required"></asp:CompareValidator> 
                </td>
                <td>
                    <asp:TextBox ID="txtEAmount" runat="server" MaxLength="12"  Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Receiptable / Non Receiptable
                    <asp:RequiredFieldValidator ID="rfvReceipted" runat="server" ControlToValidate="ddReceipt" InitialValue="0"
                        ErrorMessage="Please Select Receiptable / Non Receiptable" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddReceipt" runat="server" >
                        <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Receipts"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Non-Receipted"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Location
                </td>
                <td>
                    <asp:TextBox ID="txtLocation" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Receipt No.
                </td>
                <td>
                    <asp:TextBox ID="txtReceiptNo" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    Receipt Date
                    <cc1:CalendarExtender ID="calReceiptDate" runat="server" PopupButtonID="imgReceiptDate" TargetControlID="txtReceiptDate"
                        Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight"
                       EndDate='<%#DateTime.Now %>' ></cc1:CalendarExtender>
                    <asp:CompareValidator ID="ComValETADate" runat="server" ControlToValidate="txtReceiptDate" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid Receipt Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                
                    <cc1:MaskedEditExtender ID="MEReceiptDate" TargetControlID="txtReceiptDate" Mask="99/99/9999" MessageValidatorTip="true" 
                        MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEValReceiptDate" ControlExtender="MEReceiptDate" ControlToValidate="txtReceiptDate" IsValidEmpty="true" 
                        InvalidValueMessage="Invalid Date" SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                        MinimumValue="01/01/2014" MaximumValue='<%#DateTime.Now %>' Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtReceiptDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgReceiptDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
           </tr>
            <tr>         
                <td>
                    Payment Type
                </td>
                <td>
                    <asp:DropDownList ID="ddPaymentType" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Receipt Amount
                    <asp:CompareValidator ID="CompValRecptAmnt" ControlToValidate="txtReceiptAmount" runat="server" SetFocusOnError="true"
                     Operator="DataTypeCheck" Type="Double" ErrorMessage="*" ValidationGroup="Required"></asp:CompareValidator> 
                </td>
                <td>
                    <asp:TextBox ID="txtReceiptAmount" runat="server" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
            <!-- -->
            <tr>
                <td>
                    Billable / Non Billable
                    <asp:RequiredFieldValidator ID="RFVBillable" runat="server" ControlToValidate="ddBillable" InitialValue="-1"
                        ErrorMessage="Please Select Billable / Non Billable" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddBillable" runat="server">
                        <asp:ListItem Text="-Select-" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Billable" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Non Billable" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Paid To
                </td>
                <td>
                    <asp:TextBox ID="txtPaidTo" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>   
            <tr>
                <td>
                    Cheque No
                </td>
                <td>
                    <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="100"></asp:TextBox>
                </td>             
                <td>
                    Cheque Date
                    <cc1:CalendarExtender ID="calChequeDate" runat="server" PopupButtonID="imgChequeDate" TargetControlID="txtChequeDate"
                        Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight"></cc1:CalendarExtender>
                    <asp:CompareValidator ID="CompValCheque" runat="server" ControlToValidate="txtChequeDate" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid Cheque Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtChequeDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Remark
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" TextMode="MultiLine" Columns="3" Width="400px"></asp:TextBox>
                </td>
            </tr>
    </table>
    </InsertItemTemplate>
    <EditItemTemplate>
        <div>
            <asp:Button ID="btnUpdate" runat="server" ValidationGroup="updRequired"
                Text="Update" OnClick="btnUpdate_Click"/>
            <asp:Button ID="btnUpdCancel" Text="Cancel" runat="server" CausesValidation="false"
                OnClick="btnUpdCancel_OnClick" />
            <asp:HiddenField ID="hdnExpenseType" Value='<%#Bind("ExpenseTypeId") %>'  runat="server" />
            <asp:HiddenField ID="hdnReceiptable" Value='<%#Bind("ReceiptTypeId") %>' runat="server" />
            <asp:HiddenField ID="hdnPaymentType" Value='<%#Bind("PaymentType") %>' runat="server" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Charges For/Expense Type
                    <asp:RequiredFieldValidator ID="RFVUpdType" runat="server" InitialValue="0" ControlToValidate="ddlUPDExpenseType" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Select Charges For/Expense Type" Display="Dynamic" ValidationGroup="updRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUPDExpenseType" runat="server" Enabled="false"></asp:DropDownList>
                </td>
                <td>
                    Expense Amount
                    <asp:RequiredFieldValidator ID="RFVEUPDAmount" runat="server" ControlToValidate="txtUPDExAmount" SetFocusOnError="true" 
                        Text="*" ErrorMessage="Please Enter Expense Amount" Display="Dynamic" ValidationGroup="updRequired"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompAmount" ControlToValidate="txtUPDExAmount" runat="server" SetFocusOnError="true"
                     Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Valid Expense Amount" Text="Invalid Amount" ValidationGroup="updRequired"></asp:CompareValidator> 
                </td>
                <td>
                    <asp:TextBox ID="txtUPDExAmount" Text='<%#Bind("Amount") %>' runat="server" MaxLength="12" Width="120px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Receipt No.
                    <cc1:CalendarExtender ID="calUPDReceiptDate" runat="server" PopupButtonID="imgUPDReceiptDate" TargetControlID="txtUPDReceiptDate"
                        Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight"></cc1:CalendarExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtUPDReceiptNo" Text='<%#Bind("ReceiptNo") %>' runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    Receipt Date
                    <cc1:CalendarExtender ID="calUPDChequeDate" runat="server" PopupButtonID="imgUPDChequeDate" TargetControlID="txtUPDChequeDate"
                        Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight"></cc1:CalendarExtender>
                    <asp:CompareValidator ID="ComValUPDETADate" runat="server" ControlToValidate="txtUPDReceiptDate" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid Receipt Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtUPDReceiptDate" Text='<%#Bind("ReceiptDate","{0:dd/MM/yyyy}") %>' runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgUPDReceiptDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>     
                <td>
                    Receiptable / Non Receiptable
                    <asp:RequiredFieldValidator ID="rfvUpdReceipted" runat="server" ControlToValidate="ddUPDReceipt" InitialValue="0"
                     ErrorMessage="Please Select Receiptable / Non Receiptable" Text="*" ValidationGroup="updRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddUPDReceipt" runat="server">
                        <asp:ListItem Value="0" Text="--Select--" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Receipts"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Non-Receipted"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Location
                </td>
                <td>
                    <asp:TextBox ID="txtUPDLocation" Text='<%#Bind("Location") %>' runat="server" MaxLength="100" ></asp:TextBox>
                </td>
            </tr>
            <tr>         
                <td>
                   Payment  Type 
                </td>
                <td>
                    <asp:DropDownList ID="ddUPDPaymentType" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Receipt Amount
                    <asp:CompareValidator ID="CompValUPDRecptAmnt" ControlToValidate="txtUPDReceiptAmount" runat="server" SetFocusOnError="true"
                     Operator="DataTypeCheck" Type="Double" ErrorMessage="*" ValidationGroup="Required"></asp:CompareValidator> 
                </td>
                <td>
                    <asp:TextBox ID="txtUPDReceiptAmount" Text='<%#Bind("ReceiptAmount") %>' runat="server" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Billable / Non Billable
                </td>
                <td>
                    <asp:DropDownList ID="ddUPDBillable" runat="server">
                        <asp:ListItem Text="Billable" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Non Billable" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Paid To
                </td>
                <td>
                    <asp:TextBox ID="txtUPDPaidTo" Text='<%#Bind("PaidTo") %>' runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>   
            <tr>
                <td>
                    Cheque No
                </td>
                <td>
                    <asp:TextBox ID="txtUPDChequeNo" Text='<%#Bind("ChequeNo") %>' runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    Cheque Date
                    <asp:CompareValidator ID="CompValUPDCheque" runat="server" ControlToValidate="txtUPDChequeDate" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid Cheque Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtUPDChequeDate" Text='<%#Bind("ChequeDate","{0:dd/MM/yyyy}") %>' runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Remark
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtUPDRemark" runat="server" Text='<%#Bind("Remark") %>' MaxLength="200" TextMode="MultiLine" Columns="3" Width="400px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
    </asp:FormView>
    </fieldset>
    <fieldset>
        <legend>Job Expense Detail</legend>    
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" Visible="false"  ToolTip="Print To PDF" CausesValidation="false" />
    </div>
    <div id="content" style="overflow:scroll;">
        <asp:GridView ID="gvExpenseDetails" runat="server" AutoGenerateColumns="False" CssClass="table"
            Width="98%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" PageSize="40" 
            PagerSettings-Position="TopAndBottom" AllowPaging="true" 
            DataSourceID="DataJobExpenseDetails" OnRowCommand="gvExpenseDetails_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl" >
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" Text="Edit" CommandName="Select" CommandArgument='<%#Eval("lId") %>'
                            runat="server" ToolTip="Edit Expense">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExpenseName" HeaderText="Expense Type" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                <asp:BoundField DataField="PaidTo" HeaderText="Paid To" />
                <asp:BoundField DataField="ReceiptType" HeaderText="Receipt" />
                <asp:BoundField DataField="Location" HeaderText="Location" />
                <asp:BoundField DataField="PaymentTypeName" HeaderText="Type of Payment" />
                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" />
                <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amount" />
                <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="BillableNoBillable" HeaderText="Billable/NonBillable" />
                <%--<asp:BoundField DataField="ChequeNo" HeaderText="Cheque No" />
                <asp:BoundField DataField="ChequeDate" HeaderText="Cheque Date" DataFormatString="{0:dd/MM/yyyy}" />--%>
                <asp:BoundField DataField="UserName" HeaderText="User Name" />
                <asp:BoundField DataField="dtDate" HeaderText="Activity Date" DataFormatString="{0:dd/MM/yyyy}" />
                <%-- <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" Text="Delete" CommandName="remove" CommandArgument='<%#Eval("lId") %>'
                            class="noprint" runat="server" ToolTip="Delete Expense" OnClientClick="return confirm('Sure to delete?');">
                            </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="DataJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="EX_GetExpensesDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="JobId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>


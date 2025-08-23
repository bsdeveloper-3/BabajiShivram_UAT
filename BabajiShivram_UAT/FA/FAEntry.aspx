<%@ Page Title="Invoice Booking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="FAEntry.aspx.cs" Inherits="FA_FAEntry" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="updUpdatePanlel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="RequiredJob" CssClass="errorMsg" />
    </div>
    <fieldset style="width: 90%;">
        <legend>Invoice Booking</legend>
        <asp:Wizard ID="wzDelivery" runat="server" DisplaySideBar="false" OnNextButtonClick="wzDelivery_NextButtonClick"
            OnFinishButtonClick="wzDelivery_FinishButtonClick">
            <WizardSteps>
                <asp:WizardStep ID="StepMain" StepType="Start" Title="Vehicle Detail">
                    <cc1:CalendarExtender ID="CalChequeDate" runat="server" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChequeDate" PopupPosition="BottomRight"
                        TargetControlID="txtChequeDate">
                    </cc1:CalendarExtender>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Company
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdlCompany" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdlCompany_SelectedIndexChanged">
                                    <asp:ListItem Text="Babaji Shivram" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="NAVBHARAT" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="NAVRAJ" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                GSTIN
                            </td>
                            <td>
                                <asp:Label ID="lblGSTN" runat="server" Text="27AAACB0466A1ZB"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Cheque No
                                <asp:RequiredFieldValidator ID="RFVChequeNo" runat="server" ControlToValidate="txtChequeNo" SetFocusOnError="true"
                                    InitialValue="" Text="*" ErrorMessage="Please Enter Cheque No." Display="Static" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                Cheque Date
                                <cc1:MaskedEditExtender ID="MEditChequeDate" TargetControlID="txtChequeDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValChequeDate" ControlExtender="MEditChequeDate" ControlToValidate="txtChequeDate" IsValidEmpty="false"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Cheque Date" InvalidValueBlurredMessage="Invalid Cheque Date"
                                    InvalidValueMessage="Cheque Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Future date for Cheque Issue not allowed"
                                    MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2019" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Name
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBankName" runat="server">
                                    <asp:ListItem Value="YBL000" Text="YES BANK LTD. MUMBAI"></asp:ListItem>
                                    <asp:ListItem Value="B11" Text="H D F C BANK"></asp:ListItem>
                                    <asp:ListItem Value="B20" Text="AXIS BANK LTD"></asp:ListItem>
                                    <asp:ListItem Value="BOBA00" Text="BANK OF BARODA"></asp:ListItem>
                                    <asp:ListItem Value="IB0000" Text="I.C.I.C.I BANK LTD"></asp:ListItem>
                                    <asp:ListItem Value="B9" Text="IDBI BANK LTD"></asp:ListItem>
                                    <asp:ListItem Value="KMBL00" Text="KOTAK MAHINDRA BANK LTD"></asp:ListItem>
                                    <asp:ListItem Value="RBL001" Text="RBL BANK LTD"></asp:ListItem>
                                    <asp:ListItem Value="B1" Text="SOUTH INDIAN BANK LTD.[O/D]"></asp:ListItem>
                                    <asp:ListItem Value="B4" Text="STATE BANK OF INDIA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Amount
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeAmount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:WizardStep>
                <asp:WizardStep ID="StepOne" StepType="Step" Title="Booking Detail">
                    <cc1:CalendarExtender ID="CalInvoiceDate" runat="server" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvoiceDate" PopupPosition="BottomRight"
                        TargetControlID="txtInvoiceDate">
                        </cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CalReceiptDate" runat="server" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgReceiptDate" PopupPosition="BottomRight"
                        TargetControlID="txtReceiptDate">
                        </cc1:CalendarExtender>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Vendor
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                GST Credit
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblGSTCredit" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice No
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Invoice Date
                                <cc1:MaskedEditExtender ID="MEditInvoiceDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValInvoiceDate" ControlExtender="MEditInvoiceDate" ControlToValidate="txtInvoiceDate" IsValidEmpty="false"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Invoice Date" InvalidValueBlurredMessage="Invalid Invoice Date"
                                    InvalidValueMessage="Invoice Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Future date for Invoice not allowed"
                                    MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2019" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice GSTIN
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceGSTIN" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Invoice Amount
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceAmount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Receipt No
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptNo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Receipt Date
                                <cc1:MaskedEditExtender ID="MEditReceiptDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValReceiptDate" ControlExtender="MEditReceiptDate" ControlToValidate="txtReceiptDate" IsValidEmpty="false"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Receipt Date" InvalidValueBlurredMessage="Invalid Receipt Date"
                                    InvalidValueMessage="Receipt Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Future date for Receipt not allowed"
                                    MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2019" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgReceiptDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Job No
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceRemark" runat="server" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                            </td>
                    </table>
                    
                    <fieldset>
                        <legend>Charge Code Booking</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Charge Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtchargeCode" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                HSN/SAC
                            </td>
                            <td>
                                <asp:TextBox ID="txtHSN" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Sub-Total
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeAmount" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                CGST
                            </td>
                            <td>
                                <asp:TextBox ID="txtCGST" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SGST
                            </td>
                            <td>
                                <asp:TextBox ID="txtSGST" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                IGST
                            </td>
                            <td>
                                <asp:TextBox ID="txtIGST" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total
                            </td>
                            <td>
                                <asp:Label ID="lblChargeTotal" runat="server"></asp:Label>
                            </td>
                            <td>
                                Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <br /><br />
                        <asp:Button ID="btnAddCharges" runat="server" Text="Add Charges" OnClick="btnAddCharges_Click" />
                    </div>
                        <legend>Charge Detail</legend>
                        <div class="clear"></div>
                            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                                <asp:GridView ID="gvCharges" runat="server" CssClass="table" AutoGenerateColumns="true"
                                    AllowPaging="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                    </fieldset>
                    
                </asp:WizardStep>
                <asp:WizardStep ID="wzFinish" StepType="Finish" Title="Update">
                    <fieldset>
                        <legend>Booking Review & Save</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Cheque Number
                                </td>
                                <td>
                                    <asp:Label ID="lblVehicleNo" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Cheque Date
                                </td>
                                <td>
                                    <asp:Label ID="lblVehicleType" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Bank Name
                                </td>
                                <td>
                                    <asp:Label ID="lblTransporterName" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Amount
                                </td>
                                <td>
                                    <asp:Label ID="lblDispatchDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:WizardStep>
                <asp:WizardStep ID="wzComplete" StepType="Complete" Title="Close" AllowReturn="false">
                    <fieldset>
                        <legend>Status</legend>
                        <div class="success" align="center" style="width: 600px; text-align: center;">
                            <asp:Label ID="lblSuccessMessage" Text="Dispatch details updated successfully!" runat="server"></asp:Label>
                        </div>
                        <div class="m" style="float: left;">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" OnClick="btnCancel_OnClick" />
                        </div>
                    </fieldset>
                </asp:WizardStep>
            </WizardSteps>
            <StartNavigationTemplate>
                <br></br>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_OnClick" />
                &nbsp;&nbsp;<asp:Button ID="btnNext" runat="server" CommandName="MoveNext" Text="Next" ValidationGroup="Required" />
            </StartNavigationTemplate>
            <StepNavigationTemplate>
                <br></br>
                <asp:Button ID="btnStepPrevious" runat="server" Text="Previous" CommandName="MovePrevious" CausesValidation="false" />
                &nbsp;&nbsp;<asp:Button ID="btnStepNext" runat="server" CommandName="MoveNext" Text="Next" CausesValidation="true" ValidationGroup="RequiredJob" />

                &nbsp;&nbsp;<asp:Button ID="btnFinish" runat="server" CommandName="Finish" Text="Finish" CausesValidation="true" ValidationGroup="RequiredJob" />
            </StepNavigationTemplate>
            <FinishNavigationTemplate>
                <br></br>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                    OnClientClick="return confirm('Are you sure you want to cancel');" OnClick="btnCancel_OnClick" />
                <asp:Button ID="btnFinPrevious" runat="server" Text="Previous" CommandName="MovePrevious" CausesValidation="false" />
                &nbsp;&nbsp;<asp:Button ID="btnFinish" runat="server" Text="Save Detail" CausesValidation="true" CommandName="MoveComplete" />
            </FinishNavigationTemplate>
        </asp:Wizard>
    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
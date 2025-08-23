<%@ Page Title="Bill Submission" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillSubmission.aspx.cs" Inherits="AccountTransport_BillSubmission" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="vgJobNo" />
        <cc1:CalendarExtender ID="calBillDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBillDate"
            PopupPosition="BottomRight" TargetControlID="txtBillDate">
        </cc1:CalendarExtender>
    </div>
    <fieldset style="min-height: 380px; margin-top: 0px">
        <fieldset>
            <legend>Job Detail</legend>
            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                <tr>
                    <td>BS Job No
                    </td>
                    <td>
                        <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                    </td>
                    <td>Customer
                    </td>
                    <td>
                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Destination
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblDestination" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
        <legend>Billing Detail</legend>
        <div>
            <asp:HiddenField ID="hdnPageValid" runat="server" Value="0" />
            <asp:HiddenField ID="hdnFreightAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDetentionAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnVaraiAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnEmptyContReturnAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTollCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOtherCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAdvanceAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTotalAmount" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSavingAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarketRate" runat="server" Value="0" />
        </div>
        <div align="center">
            
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="BillRequired" CssClass="errorMsg" />
        </div>
        <div class="m clear">
            <asp:Button ID="btnBillSubmit" Text="Submit Bill" runat="server" ValidationGroup="BillRequired"
                OnClick="btnBillSubmit_Click" />
            <asp:Button ID="btnCancelBill" runat="server" Text="Cancel" OnClick="btnCancelBill_Click" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
           <%-- <tr>
                <td>Transporter
                </td>
                <td>
                    <asp:Label ID="lblTransporter" runat="server"></asp:Label>
                </td>
            </tr>--%>
            <tr>
                <td>Bill No
                    <asp:RequiredFieldValidator ID="RFVBillNo" runat="server" ControlToValidate="txtBillNo" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Bill No" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillNo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>Bill Date
                    <cc1:maskededitextender id="MEditBillDate" targetcontrolid="txtBillDate" mask="99/99/9999" messagevalidatortip="true"
                        masktype="Date" autocomplete="false" runat="server">
                    </cc1:maskededitextender>
                    <cc1:maskededitvalidator id="MEditValBillDate" controlextender="MEditBillDate" controltovalidate="txtBillDate" isvalidempty="false"
                        invalidvalueblurredmessage="Invalid Bill Date" invalidvaluemessage="Bill Date is invalid" minimumvaluemessage="Invalid Date" maximumvaluemessage="Invalid Date"
                        emptyvalueblurredtext="*" emptyvaluemessage="Please Enter Bill Date"
                        minimumvalue="01/04/2024" setfocusonerror="true" runat="Server" validationgroup="BillRequired"></cc1:maskededitvalidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Total Amount
                </td>
                <td>
                    <asp:TextBox ID="txtTotalAmount" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>Advance Paid</td>
                <td>
                    <asp:Label ID="lblAdvanceReceived" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Bill Copy (PDF)
                <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument"
                    Display="Dynamic" ValidationGroup="BillRequired" SetFocusOnError="true" Text="*"
                    ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:FileUpload ID="fuDocument" runat="server" />
                    <asp:HiddenField ID="hdnUploadPath" runat="server" />
                </td>
                <td>Justification
                    <asp:RequiredFieldValidator ID="rfvJustification" runat="server" ControlToValidate="txtJustification" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter Justification." Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtJustification" runat="server" TextMode="MultiLine" Rows="2" Width="350px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="m clear">
            <asp:GridView ID="GridViewBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" DataKeyNames="lId" DataSourceID="DataSourceBillDetail" CellPadding="4">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" />
                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                    <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                    <%--<asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />--%>
                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                    <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                    <asp:BoundField HeaderText="Empty" DataField="EmptyContRcptCharges" />
                    <asp:BoundField HeaderText="Toll" DataField="TollCharges" />
                    <asp:BoundField HeaderText="Union" DataField="OtherCharges" />
                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                    <asp:BoundField HeaderText="Remark" DataField="Justification" />
                    <asp:BoundField HeaderText="Date" DataField="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="User" DataField="UserName"/>
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <fieldset>
        <legend>Vehicle Detail</legend>
        <div style="overflow:scroll">
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="700px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" ShowFooter="true"
                PageSize="200" OnRowDataBound="GridViewVehicle_RowDataBound" 
                FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle No">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%# Bind("VehicleNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Freight Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="lblRate" runat="server" Text='<%# Bind("Rate") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" SortExpression="AdvanceAmount" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Detention">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDetention" runat="server" Text='<%# Bind("DetentionAmount") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Varai">
                        <ItemTemplate>
                            <asp:TextBox ID="txtVarai" runat="server" Text='<%# Bind("VaraiExpense") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Empty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtEmptyContRecpt" runat="server" Text='<%# Bind("EmptyContRecptCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Toll">
                        <ItemTemplate>
                            <asp:TextBox ID="txtToll" runat="server" Text='<%# Bind("TollCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Union">
                        <ItemTemplate>
                            <asp:TextBox ID="txtOtherCharges" runat="server" Text='<%# Bind("OtherCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LR Copy">
                        <ItemTemplate>
                            <asp:FileUpload ID="updLRCopy" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Challan Copy">
                        <ItemTemplate>
                            <asp:FileUpload ID="updChallanCopy" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Empty Receipt">
                        <ItemTemplate>
                            <asp:FileUpload ID="updEptyReceipt" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    </fieldset>
    <div id="divDatasource">
    <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="TR_GetTransBillByJobid" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="TRS_GetTransRateByRequestId" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
            <asp:SessionParameter SessionField="TransporterId" Name="TransporterId" />
        </SelectParameters>
    </asp:SqlDataSource>
    </div>
</asp:Content>


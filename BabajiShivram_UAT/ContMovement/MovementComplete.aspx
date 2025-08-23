<%@ Page Title="Movement Complete" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MovementComplete.aspx.cs"
    Inherits="ContMovement_MovementComplete" ValidateRequest="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upMovementComplete" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upMovementComplete" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div>
                <br />
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:Label ID="lblFlashError" runat="server" EnableViewState="false" Font-Bold="true" Font-Size="16px"></asp:Label>
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsMovementComplete" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
                </div>
                <div class="clear">
                </div>
                <fieldset>
                    <legend>Movement Complete</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click" ValidationGroup="vgRequired" TabIndex="6" />
                        <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server" OnClick="btnCancel_Click" TabIndex="7" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>BS Job No.
                            </td>
                            <td>
                                <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                            </td>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignee
                            </td>
                            <td>
                                <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                            </td>
                            <td>ETA</td>
                            <td>
                                <asp:Label ID="lblETADate" runat="server">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Branch</td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                            </td>
                            <td>Sum of 20</td>
                            <td>
                                <asp:Label ID="lblSumof20" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Sum of 40</td>
                            <td>
                                <asp:Label ID="lblSumof40" runat="server"></asp:Label>
                            </td>
                            <td>Container Type</td>
                            <td>
                                <asp:Label ID="lblContType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Creation Date</td>
                            <td>
                                <asp:Label ID="lblJobCreationDate" runat="server"></asp:Label>
                            </td>
                            <td>Job Created By</td>
                            <td>
                                <asp:Label ID="lblJobCreatedBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Shipping Name</td>
                            <td>
                                <asp:Label ID="lblShippingName" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnShipperId" runat="server" Value="0" />
                            </td>
                            <td>Delivery Type
                            </td>
                            <td>
                                <asp:Label ID="lblDeliveryType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>CFS Name
                            </td>
                            <td>
                                <asp:Label ID="lblCFSName" runat="server"></asp:Label>
                            </td>
                            <td>Nominated CFS Name</td>
                            <td>
                                <asp:DropDownList ID="ddlCFSName" runat="server" Width="300px">
                                    <asp:ListItem Text="--Select--" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnCFSName" runat="server" Value='<%#Eval("CFSId") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>Shipping Line Date
                                <cc1:CalendarExtender ID="calShippingLineDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgShippingLineDate"
                                    PopupPosition="BottomRight" TargetControlID="txtShippingLineDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeShippingLineDate" TargetControlID="txtShippingLineDate" Mask="99/99/9999"
                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevShippingLineDate" ControlExtender="meeShippingLineDate" ControlToValidate="txtShippingLineDate" IsValidEmpty="true"
                                    EmptyValueMessage="Enter Shipping Line Date" EmptyValueBlurredText="*" InvalidValueMessage="Shipping Line Date is invalid" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Shipping Line Date" MaximumValueMessage="Invalid Shipping Line Date" InvalidValueBlurredMessage="Invalid"
                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtShippingLineDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgShippingLineDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>Movement Confirmed By Line Date
                                <cc1:CalendarExtender ID="calConfirmedByLineDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgConfirmedByLineDate"
                                    PopupPosition="BottomRight" TargetControlID="txtConfirmedByLineDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeConfirmedByLineDate" TargetControlID="txtConfirmedByLineDate" Mask="99/99/9999"
                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevConfirmedByLineDate" ControlExtender="meeConfirmedByLineDate" ControlToValidate="txtConfirmedByLineDate" IsValidEmpty="true"
                                    EmptyValueMessage="Enter Movement Confirmed By Line Date" EmptyValueBlurredText="*" InvalidValueMessage="Movement Confirmed By Line Date is invalid" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfirmedByLineDate" runat="server" Width="100px" MaxLength="10" TabIndex="3" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgConfirmedByLineDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <asp:CompareValidator ID="cvConfirmedByLineDate" runat="server" ControlToValidate="txtConfirmedByLineDate" ControlToCompare="txtShippingLineDate"
                                    Display="Dynamic" ErrorMessage="Movement Confirmed By Line Date should be greater than Shipping Line Date." Text="*" Type="Date"
                                    Operator="GreaterThanEqual" SetFocusOnError="true" ValidationGroup="vgRequired"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Movement Complete Date
                                <cc1:CalendarExtender ID="calMovementComplete" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCompleteDate"
                                    PopupPosition="BottomRight" TargetControlID="txtCompleteDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeCompleteDate" TargetControlID="txtCompleteDate" Mask="99/99/9999"
                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevCompleteDate" ControlExtender="meeCompleteDate" ControlToValidate="txtCompleteDate" IsValidEmpty="true"
                                    EmptyValueMessage="Enter Movement Complete Date" EmptyValueBlurredText="*" InvalidValueMessage="Movement Complete Date is invalid" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCompleteDate" runat="server" Width="100px" MaxLength="10" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgCompleteDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <asp:CompareValidator ID="cvCompleteDate" runat="server" ControlToValidate="txtCompleteDate" ControlToCompare="txtConfirmedByLineDate"
                                    Display="Dynamic" ErrorMessage="Movement Complete Date should be greater than Confirmed By Line Date." Text="*" Type="Date"
                                    Operator="GreaterThanEqual" SetFocusOnError="true" ValidationGroup="vgRequired"></asp:CompareValidator>
                            </td>
                            <td>Container Received at CFS Date
                                <cc1:CalendarExtender ID="calEmptyContReturnDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEmptyContReturnDate"
                                    PopupPosition="BottomRight" TargetControlID="txtEmptyContReturnDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeEmptyContReturnDate" TargetControlID="txtEmptyContReturnDate" Mask="99/99/9999"
                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevEmptyContReturnDate" ControlExtender="meeEmptyContReturnDate" ControlToValidate="txtEmptyContReturnDate" IsValidEmpty="true"
                                    EmptyValueMessage="Enter Empty Container Return Date" EmptyValueBlurredText="*" InvalidValueMessage="Empty Container Return Date is invalid" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmptyContReturnDate" runat="server" Width="100px" MaxLength="10" TabIndex="5" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgEmptyContReturnDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <asp:CompareValidator ID="cvEmptyContReturnDate" runat="server" ControlToValidate="txtEmptyContReturnDate" ControlToCompare="txtCompleteDate"
                                    Display="Dynamic" ErrorMessage="Empty Container Return Date should be greater than Movement Complete Date." Text="*" Type="Date"
                                    Operator="GreaterThanEqual" SetFocusOnError="true" ValidationGroup="vgRequired"></asp:CompareValidator>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Documents</legend>
                    <div>
                        <asp:FileUpload ID="fuUploadFile" runat="server" />
                        &nbsp;
                        <asp:Button ID="btnSaveDocument" runat="server" Text="Upload Document" TabIndex="2" OnClick="btnSaveDocument_Click" />
                    </div>
                    <div>
                        <asp:GridView ID="gvDocuments" runat="server" CssClass="table" AutoGenerateColumns="false"
                            PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                            PagerSettings-Position="TopAndBottom" OnRowCommand="gvDocuments_RowCommand" Width="100%"
                            DataSourceID="DataSourceDocuments">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownloadDoc" CommandName="download" ToolTip="Download document" runat="server"
                                            Text='<%#Eval("DocPath") %>' CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" CommandName="deletedoc" ToolTip="Delete document" runat="server"
                                            Text="Delete" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </fieldset>
                <fieldset style="visibility: hidden">
                    <legend>Generate Letter</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Select Letter</td>
                            <td>
                                <asp:DropDownList ID="ddlLetters" runat="server" AppendDataBoundItems="true" Width="350px" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <p>
                                <asp:Table ID="CustomUITable" runat="server" border="0" CellPadding="0" CellSpacing="0" Width="100%" bgcolor="white">
                                </asp:Table>
                            </p>
                        </tr>
                        <tr>
                            <div>
                                <asp:Panel ID="pnlGrids" runat="server" Visible="true">
                                </asp:Panel>
                            </div>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnLetterPDF" runat="server" OnClick="btnLetterPDF_Click" Text="Letter to PDF" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hfGridHtml" runat="server" />
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="SqlDataSourceLetter" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetShippingLettersById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnShipperId" Name="ShippingId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetDocuments" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


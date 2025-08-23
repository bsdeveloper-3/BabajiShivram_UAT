<%@ Page Title="Existing Customer Enquiry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustEnquiry.aspx.cs"
    Inherits="CRM_CustEnquiry" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <style type="text/css">
        table.table th, table.table th a {
            background-color: white;
        }
    </style>
    <script type="text/javascript">
        function OnCustomerSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCustId').value = results.ClientId;

        }
        $addHandler
       (
           $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_txtCustomer'), 'keyup',
           function () {
               $get('ctl00_ContentPlaceHolder1_hdnCustId').value = '0';
           }
       );
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnEnquiryNo" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
            </div>
            <div class="m clear">
                <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgRequired" TabIndex="4" />
                <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="5" runat="server" />
                <asp:Button ID="btnBack" Text="Back" OnClick="btnBack_Click" CausesValidation="false" TabIndex="6" runat="server" />
            </div>
            <fieldset>
                <legend>Enquiry</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <colgroup>
                        <col width="20%" />
                        <col width="80%" />
                    </colgroup>
                    <tr>
                        <td>Customer
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="txtCustomer" SetFocusOnError="true" Display="Dynamic"
                                ErrorMessage="Please Enter Customer Name" Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomer" Width="30%" runat="server" ToolTip="Enter Customer Name." AutoPostBack="true" style='text-transform:uppercase' 
                                CssClass="SearchTextbox" placeholder="Search" TabIndex="1"></asp:TextBox>
                            <div id="divwidthCust" runat="server">
                            </div>
                            <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                CompletionListElementID="divwidthCust" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Payment Terms</td>
                        <td>
                            <asp:TextBox ID="txtPaymentTerms" runat="server" TabIndex="2" Width="30%" placeholder="Payment terms ..."></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Notes 
                            <asp:RequiredFieldValidator ID="rfvNotes" runat="server" ControlToValidate="txtNotes" SetFocusOnError="true"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please Enter Notes" Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" runat="server" TabIndex="3" TextMode="MultiLine" Rows="5" placeholder="Enter notes ..."></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Services</legend>
                <asp:GridView ID="gvService" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="90%" TabIndex="21" class="table"
                    OnRowCreated="gvService_RowCreated" Style="border-collapse: initial; border: 1px solid #5D7B9D;">
                    <Columns>
                        <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Service" ItemStyle-Width="45%">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlService" runat="server" DataSourceID="DataSourceService" DataTextField="sName" DataValueField="ServicesId"
                                    AppendDataBoundItems="true" TabIndex="21" Width="95%">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvService" runat="server" ControlToValidate="ddlService" Display="Dynamic" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select service" ValidationGroup="vgService" InitialValue="0"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Location" ItemStyle-Width="45%">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlLocation" runat="server" DataSourceID="DataSourceServiceLocation" DataTextField="BranchName" DataValueField="lid"
                                    AppendDataBoundItems="true" TabIndex="22" Width="95%">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" Display="Dynamic" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select service location" ValidationGroup="vgService" InitialValue="0"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Expected Close Date" ItemStyle-Width="45%">
                            <ItemTemplate>
                                <cc1:CalendarExtender ID="calCloseDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="txtCloseDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtCloseDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeCloseDate" TargetControlID="txtCloseDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevCloseDate" ControlExtender="meeCloseDate" ControlToValidate="txtCloseDate" IsValidEmpty="true"
                                    InvalidValueMessage="Expected Close Date is invalid" InvalidValueBlurredMessage="Invalid Expected Close Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Expected Close Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>
                                <asp:TextBox ID="txtCloseDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="23" ToolTip="Enter Expected Close Date."></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Volume Expected" ItemStyle-Width="45%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVolumeExp" runat="server" TabIndex="24"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requirement" ItemStyle-Width="45%">
                            <ItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvRequirement" ControlToValidate="txtRequirement" ValidationGroup="vgService"
                                    Text="*" ErrorMessage="Please Enter Requirement" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtRequirement" runat="server" TextMode="MultiLine" TabIndex="25" Rows="3" Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteRow" runat="server" CausesValidation="false" TabIndex="27" OnClick="lnkDeleteRow_Click" Text="Delete" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="btnAddTransportCharges" ValidationGroup="vgService" TabIndex="26" runat="server" Text="Add Service" OnClick="btnAddTransportCharges_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <fieldset>
                <legend>Documents (if any)</legend>
                <div class="m clear">
                    <asp:FileUpload ID="fuDocument2" runat="server" ViewStateMode="Enabled" TabIndex="8" />
                    <asp:Button ID="btnSaveDocument2" Text="Add" runat="server" OnClick="btnSaveDocument2_Click" />
                </div>
                <div>
                    <asp:Repeater ID="rptDocument2" runat="server" OnItemCommand="rptDocument2_ItemCommand">
                        <HeaderTemplate>
                            <table class="table" border="0" cellpadding="0" cellspacing="0" style="width: 50%">
                                <tr bgcolor="#FF781E">
                                    <th>Sl
                                    </th>
                                    <th>Document Name
                                    </th>
                                    <th>Action
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Container.ItemIndex +1%>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkDownload" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                        CommandArgument='<%# Eval("DocPath") %>' CausesValidation="false" runat="server"
                                        Width="200px" CommandName="DownloadFile"></asp:LinkButton>
                                    &nbsp;
                                    <asp:HiddenField ID="hdnDocLid" Value='<%#DataBinder.Eval(Container.DataItem,"PkId") %>'
                                        runat="server"></asp:HiddenField>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                        runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceBabajiCustomers" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCustomerMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceServiceLocation" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetBranchByUser" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


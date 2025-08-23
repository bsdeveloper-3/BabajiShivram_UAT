<%@ Page Title="Add Quote" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RfqQuote.aspx.cs"
    Inherits="CRM_RfqQuote" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <%--<asp:Label ID="lblGeneralMsg" runat="server" Font-Underline="true" Font-Bold="true" ForeColor="Red" Text="Note: KYC mail will be forwarded once approved!"></asp:Label>--%>
            <asp:Label ID="lblMessage" runat="server" Font-Underline="true" Font-Bold="true" ForeColor="Red"></asp:Label>
    <style type="text/css">
        table.table th, table.table th a {
            background-color: white;
        }
    </style>
    <script type="text/javascript">
        function OnSalesPersonSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnSalesPersonId.ClientID %>').value = results.Userid;
        }

        function OnKAMSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnKAMId.ClientID %>').value = results.Userid;
        }
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                    <asp:ValidationSummary ID="vs_Service" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="vgService" CssClass="errorMsg" EnableViewState="false" />
                    <asp:HiddenField ID="hdnTurnover" runat="server" />
                    <asp:HiddenField ID="hdnCompanyType" runat="server" />
                    <asp:HiddenField ID="hdnEmployeeCount" runat="server" />
                    <asp:HiddenField ID="hdnEnquiryNo" runat="server" />
                </div>
                <div class="m clear">
                    <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                        TabIndex="39" />
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="40"
                        runat="server" />
                    <asp:Button ID="btnBack" Text="Back" OnClick="btnBack_Click" CausesValidation="false" TabIndex="40"
                        runat="server" />
                </div>

                <fieldset>
                    <legend>Lead</legend>
                    <asp:FormView ID="FormView1" runat="server" Width="100%" DataSourceID="FormviewSqlDataSource" DataKeyNames="lid">
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Lead Ref No</td>
                                    <td>
                                        <asp:Label ID="lblLeadRefNo" runat="server" Text='<%#Eval("LeadRefNo") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>Company Name</td>
                                    <td>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Lead Status</td>
                                    <td>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("LeadStage") %>'></asp:Label>
                                    </td>
                                    <td>Lead Source</td>
                                    <td>
                                        <asp:Label ID="lblLeadSource" runat="server" Text='<%#Eval("LeadSource") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Source Description</td>
                                    <td>
                                        <asp:Label ID="lblSourceDescription" runat="server" Text='<%#Eval("SourceDescription") %>'></asp:Label>
                                    </td>
                                    <td>Company Type</td>
                                    <td>
                                        <asp:Label ID="lblCompanyType" runat="server" Text='<%#Eval("CompanyType") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Business Sector</td>
                                    <td>
                                        <asp:Label ID="lblBusinessSector" runat="server" Text='<%#Eval("BusinessSector") %>'></asp:Label>
                                    </td>
                                    <td>Business Category</td>
                                    <td>
                                        <asp:Label ID="lblBusinessCatg" runat="server" Text='<%#Eval("BusinessCategory") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Turnover</td>
                                    <td>
                                        <asp:Label ID="lblTurnover" runat="server" Text='<%#Eval("Turnover") %>'></asp:Label>
                                    </td>
                                    <td>Employee Count</td>
                                    <td>
                                        <asp:Label ID="lblEmployeeCount" runat="server" Text='<%#Eval("EmployeeCount") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Contact Name</td>
                                    <td>
                                        <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("ContactName") %>'></asp:Label>
                                    </td>
                                    <td>Desgination</td>
                                    <td>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Role</td>
                                    <td>
                                        <asp:Label ID="lblRole" runat="server" Text='<%#Eval("Role") %>'></asp:Label>
                                    </td>
                                    <td>Email</td>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Mobile No</td>
                                    <td>
                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                    </td>
                                    <td>Contact No</td>
                                    <td>
                                        <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("ContactNo") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Created By</td>
                                    <td>
                                        <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                    </td>
                                    <td>Created Date</td>
                                    <td>
                                        <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Office Location</td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOfficeLocation" runat="server" Text='<%#Eval("OfficeLocation") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Address Line 1</td>
                                    <td colspan="3">
                                        <asp:Label ID="lblAddressLine1" runat="server" Width="1020px" Text='<%#Eval("AddressLine1") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Address Line 2</td>
                                    <td colspan="3">
                                        <asp:Label ID="lblAddressLine2" runat="server" Width="1020px" Text='<%#Eval("AddressLine2") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Address Line 3</td>
                                    <td colspan="3">
                                        <asp:Label ID="lblAddressLine3" runat="server" Width="1020px" Text='<%#Eval("AddressLine3") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Description</td>
                                    <td colspan="3">
                                        <asp:Label ID="lblDescription" runat="server" Width="1020px" Text='<%#Eval("Description") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                    <div id="divDataSource">
                        <asp:SqlDataSource ID="FormviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="CRM_GetLeadByLid" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="lId" SessionField="LeadId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
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
                    <br />
                    <asp:GridView ID="GridView1" runat="server" ShowFooter="false" AutoGenerateColumns="False" Width="80%" TabIndex="21" class="table"
                        DataSourceID="DataSourceServices" DataKeyNames="lid" Style="border-collapse: initial; border: 1px solid #5D7B9D;">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="edit service" Text="Edit"
                                        CommandName="editrow" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="delete service" Text="Delete"
                                        CommandName="deleterow" CommandArgument='<%#Eval("lid")%>' OnClientClick="return confirm('Are you sure to delete service?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ServiceName" HeaderText="Service" ReadOnly="true" />
                            <asp:BoundField DataField="ServiceLocation" HeaderText="Location" ReadOnly="true" />
                            <asp:BoundField DataField="VolumeExpected" HeaderText="Volume Expected" ReadOnly="true" />
                            <asp:BoundField DataField="Requirement" HeaderText="Requirement" ReadOnly="true" />
                            <asp:BoundField DataField="ExpectedCloseDate" HeaderText="Expected Close Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>

                    <asp:SqlDataSource ID="DataSourceServices" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetServices" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </fieldset>
                <fieldset>
                    <legend>Enquiry/RFQ Received</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Payment Terms <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvPaymentTerms" runat="server" ControlToValidate="txtPaymentTerms" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Payment Terms" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTerms" runat="server" TabIndex="1" Width="260px"></asp:TextBox>
                            </td>
                            <td>Branch <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBabajiBranch" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Branch" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="270px" TabIndex="4">
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <%--<tr>
                           <td>Customer Reference</td>
                            <td>
                                <asp:TextBox ID="txtCustRef" runat="server" TabIndex="2" Width="260px"></asp:TextBox>
                            </td>
                            <td>Years In Service</td>
                            <td>
                                <asp:TextBox ID="txtYearsInService" runat="server" TabIndex="3" Width="260px"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>Sales Person Name <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvSalesPerson" runat="server" ControlToValidate="txtSalesPerson" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Sales Person Name." Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="SearchTextbox" Width="260px" placeholder=" Search" TabIndex="5"></asp:TextBox>
                                <asp:HiddenField ID="hdnSalesPersonId" runat="server" Value="0" />
                                <div id="divwidthSalesRep">
                                </div>
                                <cc1:AutoCompleteExtender ID="SalesRepExtender" runat="server" TargetControlID="txtSalesPerson"
                                    CompletionListElementID="divwidthSalesRep" ServicePath="../WebService/UserAutoComplete.asmx"
                                    ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthSalesRep"
                                    ContextKey="7164" UseContextKey="True" OnClientItemSelected="OnSalesPersonSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>KAM</td>
                            <td>
                                <asp:TextBox ID="txtKAM" runat="server" CssClass="SearchTextbox" Width="260px" placeholder="Search" TabIndex="6"></asp:TextBox>
                                <asp:HiddenField ID="hdnKAMId" runat="server" Value="0" />
                                <div id="divwidthKAM">
                                </div>
                                <cc1:AutoCompleteExtender ID="KAMExtender" runat="server" TargetControlID="txtKAM"
                                    CompletionListElementID="divwidthKAM" ServicePath="../WebService/UserAutoComplete.asmx"
                                    ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthKAM"
                                    ContextKey="7164" UseContextKey="True" OnClientItemSelected="OnKAMSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>Notes <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvNotes" runat="server" ControlToValidate="txtNotes" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Notes." Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3" Width="85%" TabIndex="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Add documents (if any)</td>
                            <td colspan="3">
                                <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                                    <asp:FileUpload ID="fuDocument2" runat="server" ViewStateMode="Enabled" TabIndex="8" />
                                    <asp:Button ID="btnSaveDocument2" Text="Add" runat="server" OnClick="btnSaveDocument2_Click" />
                                </div>
                                <br />
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
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
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


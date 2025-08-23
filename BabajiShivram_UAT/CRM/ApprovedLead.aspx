<%@ Page Title="Approved Lead" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovedLead.aspx.cs"
    Inherits="CRM_ApprovedLead" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <style type="text/css">
        .modal-header {
            padding: 5px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <script type="text/javascript">
        function OnUserSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnAttendeeUserId.ClientID %>').value = results.Userid;
        }

        function OnKAMSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnKAMId.ClientID %>').value = results.Userid;
        }
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div id="divInbond" class="info" runat="server" align="center">
                <asp:Label ID="lblInbondJobNo" runat="server"></asp:Label>
                <asp:HiddenField ID="hdnSalesPersonId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnKAMId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnEnquiryId" runat="server" />
                <asp:HiddenField ID="hdnLid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCompanyId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAttendeeUserId" runat="server" Value="0" />
            </div>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnDeliveryType" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsService" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgService" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsAddStatus" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgAddStatus" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsContact" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgContact" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsVisitReport" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgVisitReport" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsMOM" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgMOM" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsAttendee" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgAttendee" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelLead" TabIndex="1" HeaderText="Lead">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Lead</legend>
                            <asp:FormView ID="FormViewLead" runat="server" Width="100%" DataSourceID="DataSourceLead" DataKeyNames="lid" OnItemUpdating="FormViewLead_ItemUpdating"
                                OnItemDeleted="FormViewLead_ItemDeleted" OnItemUpdated="FormViewLead_ItemUpdated" OnItemInserted="FormViewLead_ItemInserted"
                                OnDataBound="FormViewLead_DataBound">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnEditLead" runat="server" OnClick="btnEditLead_Click" Text="Edit" />
                                        <asp:Button ID="btnDeleteLead" runat="server" OnClick="btnDeleteLead_Click" Text="Delete" Visible="false"
                                            OnClientClick="return confirm('Sure to delete Lead Detail? All lead related detail will be removed from system.');" />
                                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
                                    </div>
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
                                            <td>Lead Source</td>
                                            <td>
                                                <asp:Label ID="lblLeadSource" runat="server" Text='<%#Eval("LeadSource") %>'></asp:Label>
                                            </td>
                                            <td>Source Description</td>
                                            <td>
                                                <asp:Label ID="lblSourceDescription" runat="server" Text='<%#Eval("SourceDescription") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Company Type</td>
                                            <td>
                                                <asp:Label ID="lblCompanyType" runat="server" Text='<%#Eval("CompanyType") %>'></asp:Label>
                                            </td>
                                            <td>Business Sector</td>
                                            <td>
                                                <asp:Label ID="lblBusinessSector" runat="server" Text='<%#Eval("BusinessSector") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Business Category</td>
                                            <td>
                                                <asp:Label ID="lblBusinessCatg" runat="server" Text='<%#Eval("BusinessCategory") %>'></asp:Label>
                                            </td>
                                            <td>Contact Name</td>
                                            <td>
                                                <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("ContactName") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Designation</td>
                                            <td>
                                                <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                            </td>
                                            <td>Role</td>
                                            <td>
                                                <asp:Label ID="lblRole" runat="server" Text='<%#Eval("Role") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Email</td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                            </td>
                                            <td>Mobile No</td>
                                            <td>
                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contact No</td>
                                            <td>
                                                <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("ContactNo") %>'></asp:Label>
                                            </td>
                                            <td>Lead Owner</td>
                                            <td>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Created Date</td>
                                            <td>
                                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>
                                            </td>
                                            <td></td>
                                            <td></td>
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
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateLead" runat="server" CommandName="Update" Text="Update" ValidationGroup="Required" />
                                        <asp:Button ID="btnCancelLead" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                                    </div>
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
                                            <td>Lead Source
                                                <asp:RequiredFieldValidator ID="rfvLeadSource" runat="server" ControlToValidate="ddlSource" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Select Lead Source" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSource" runat="server" DataSourceID="DataSourceSource" DataTextField="sName"
                                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="1" Width="310px">
                                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Source Description
                                                <asp:RequiredFieldValidator ID="rfvSourceDesc" runat="server" ControlToValidate="txtSourceDescription" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Enter Source Description" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSourceDescription" runat="server" TabIndex="2" Width="300px" Text='<%#Bind("SourceDescription") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Company Type
                                                <asp:RequiredFieldValidator ID="rfvCompanyType" runat="server" ControlToValidate="ddlCompanyType" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Select Company Type" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCompanyType" runat="server" DataSourceID="DataSourceCompanyType" DataTextField="sName"
                                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="3" Width="310px">
                                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Business Sector
                                                <asp:RequiredFieldValidator ID="rfvSector" runat="server" ControlToValidate="ddlBusinessSector" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Select Business Sector" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBusinessSector" runat="server" DataSourceID="DataSourceBusinessSector" DataTextField="sName"
                                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="4" Width="310px">
                                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Business Category
                                                <asp:RequiredFieldValidator ID="rfvCatg" runat="server" ControlToValidate="ddlBusinessCatg" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Select Business Category" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBusinessCatg" runat="server" DataSourceID="DataSourceBusinessCatg" DataTextField="sName"
                                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="5" Width="310px">
                                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Contact Name
                                                <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ControlToValidate="txtContactName" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Enter Contact Name" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactName" runat="server" TabIndex="6" Width="300px" Text='<%#Bind("ContactName") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Desgination</td>
                                            <td>
                                                <asp:TextBox ID="txtDesignation" runat="server" TabIndex="7" Width="300px" Text='<%#Bind("Designation") %>'></asp:TextBox>
                                            </td>
                                            <td>Role</td>
                                            <td>
                                                <asp:DropDownList ID="ddlRole" runat="server" DataSourceID="DataSourceRole" DataTextField="sName"
                                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="8" Width="310px">
                                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Email</td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" TabIndex="9" Width="300px" Text='<%#Bind("Email") %>'></asp:TextBox>
                                            </td>
                                            <td>Mobile No</td>
                                            <td>
                                                <asp:TextBox ID="txtMobileNo" runat="server" TabIndex="10" Width="300px" Text='<%#Bind("MobileNo") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contact No</td>
                                            <td>
                                                <asp:TextBox ID="txtContactNo" runat="server" TabIndex="11" Width="300px" MaxLength="25" Text='<%#Bind("ContactNo") %>'></asp:TextBox>
                                            </td>
                                            <td>Lead Owner</td>
                                            <td>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Created Date</td>
                                            <td>
                                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>Office Location</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtOfficeLocation" runat="server" TabIndex="12" Width="300px" MaxLength="80" Text='<%#Bind("OfficeLocation") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address 1
                                                <asp:RequiredFieldValidator ID="rfvAddressLine1" runat="server" ControlToValidate="txtAddressLine1" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Please Enter Address" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAddressLine1" runat="server" TabIndex="22" MaxLength="50" Width="980px" Text='<%#Bind("AddressLine1") %>' placeholder="eg :- Street name"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address 2</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAddressLine2" runat="server" TabIndex="23" Rows="2" MaxLength="50" Width="980px" Text='<%#Bind("AddressLine2") %>' placeholder="eg :- Landmark, City"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address 3</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAddressLine3" runat="server" TabIndex="24" Rows="2" MaxLength="50" Width="980px" Text='<%#Bind("AddressLine3") %>' placeholder="eg :- Zipcode, State, Country"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Description</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtDescription" runat="server" Rows="2" TextMode="MultiLine" TabIndex="14" Width="1020px" Text='<%#Bind("Description") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelContact" TabIndex="2" HeaderText="Contacts">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Add Contact
                            </legend>
                            <div class="m clear">
                                <asp:Button ID="btnAddContact" runat="server" Text="Save" OnClick="btnAddContact_Click" TabIndex="20" CausesValidation="true" ValidationGroup="vgContact" />
                                <asp:Button ID="btnCancelContact" runat="server" Text="Cancel" TabIndex="21" OnClick="btnCancelContact_Click" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <colgroup>
                                    <col width="10%" />
                                    <col width="40%" />
                                    <col width="10%" />
                                    <col width="40%" />
                                </colgroup>
                                <tr>
                                    <td>Contact Name
                                        <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ControlToValidate="txtContactName" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Contact Name" Text="*" ValidationGroup="vgContact" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactName" runat="server" TabIndex="11" Width="300px"></asp:TextBox>
                                    </td>
                                    <td>Designation
                                        <asp:RequiredFieldValidator ID="rfvDesignation" runat="server" ControlToValidate="txtDesignation" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Designation" Text="*" ValidationGroup="vgContact" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDesignation" runat="server" TabIndex="12" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Role</td>
                                    <td>
                                        <asp:DropDownList ID="ddlRole" runat="server" DataSourceID="DataSourceRole" DataTextField="sName"
                                            DataValueField="lid" AppendDataBoundItems="true" TabIndex="13" Width="310px">
                                            <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Email
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Email" Text="*" ValidationGroup="vgContact" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                            SetFocusOnError="true" Display="Dynamic" Text="*" ErrorMessage="Invalid Email" ForeColor="Red"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Required"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" TabIndex="14" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Mobile No
                                        <asp:RequiredFieldValidator ID="rfvMobileNo" runat="server" ControlToValidate="txtMobileNo" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Mobile No" Text="*" ValidationGroup="vgContact" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revMobileNo" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtMobileNo"
                                            ErrorMessage="Enter valid mobile no" ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMobileNo" runat="server" TabIndex="15" Width="300px"></asp:TextBox>
                                    </td>
                                    <td>Contact No
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactNo" runat="server" TabIndex="16" Width="300px" MaxLength="25"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Office Location</td>
                                    <td>
                                        <asp:TextBox ID="txtOfficeLocation" runat="server" TabIndex="17" Width="300px" MaxLength="80"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Address
                                        <asp:RequiredFieldValidator ID="rfvAddressLine1" runat="server" ControlToValidate="txtContactAddress" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Address" Text="*" ValidationGroup="vgContact" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtContactAddress" runat="server" TabIndex="18" MaxLength="50" Width="980px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Description</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescription" runat="server" Rows="2" TextMode="MultiLine" TabIndex="19" Width="980px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvContacts" runat="server" ShowFooter="false" AutoGenerateColumns="False" Width="80%" TabIndex="21" class="table"
                                DataSourceID="DataSourceContacts" DataKeyNames="lid" Style="border-collapse: initial; border: 1px solid #5D7B9D;">
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="edit service" Text="Edit"
                                                CommandName="editrow" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="delete service" Text="Delete"
                                                CommandName="deleterow" CommandArgument='<%#Eval("lid")%>' OnClientClick="return confirm('Are you sure to delete service?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContactName" HeaderText="Contact" ReadOnly="true" />
                                    <asp:BoundField DataField="Designation" HeaderText="Designation" ReadOnly="true" />
                                    <asp:BoundField DataField="RoleName" HeaderText="Role" ReadOnly="true" />
                                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" ReadOnly="true" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
                                    <asp:BoundField DataField="AlternatePhone" HeaderText="Alternate Phone" ReadOnly="true" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelService" TabIndex="3" HeaderText="Product">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Add Service
                            </legend>
                            <div class="m clear">
                                <asp:Button ID="btnAddService" runat="server" Text="Save" OnClick="btnAddService_Click" TabIndex="16" CausesValidation="true" ValidationGroup="vgService" />
                                <asp:Button ID="btnCancelService" runat="server" Text="Cancel" TabIndex="17" OnClick="btnCancelService_Click" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white">
                                <colgroup>
                                    <col width="25%" />
                                    <col width="75%" />
                                </colgroup>
                                <tr>
                                    <td>Product
                                        <asp:RequiredFieldValidator ID="rfvService" runat="server" ControlToValidate="ddlService" Display="Dynamic" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please Select Service" ValidationGroup="vgService" InitialValue="0"></asp:RequiredFieldValidator>
                                        <td>
                                            <asp:DropDownList ID="ddlService" runat="server" DataSourceID="DataSourceService" DataTextField="sName" DataValueField="ServicesId"
                                                AppendDataBoundItems="true" TabIndex="11" Width="210px">
                                                <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Service Location <span style="color: red">(Babaji Branches)</span>
                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" Display="Dynamic" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please Select Service Location" ValidationGroup="vgService" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLocation" runat="server" DataSourceID="DataSourceServiceLocation" DataTextField="BranchName" DataValueField="lid"
                                            AppendDataBoundItems="true" TabIndex="12" Width="210px">
                                            <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Volume Expected
                                        <asp:RequiredFieldValidator ID="rfvVolumeExp" runat="server" ControlToValidate="txtVolumeExp" Display="Dynamic" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please enter volume expected" ValidationGroup="vgService"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolumeExp" runat="server" TabIndex="13" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Expected Close Date
                                        <AjaxToolkit:CalendarExtender ID="calCloseDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="txtCloseDate"
                                            Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtCloseDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <AjaxToolkit:MaskedEditExtender ID="meeCloseDate" TargetControlID="txtCloseDate" Mask="99/99/9999" MessageValidatorTip="true"
                                            MaskType="Date" AutoComplete="false" runat="server">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="mevCloseDate" ControlExtender="meeCloseDate" ControlToValidate="txtCloseDate" IsValidEmpty="true"
                                            InvalidValueMessage="Expected Close Date is invalid" InvalidValueBlurredMessage="Invalid Expected Close Date" SetFocusOnError="true"
                                            MinimumValueMessage="Invalid Expected Close Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                            runat="Server"></AjaxToolkit:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCloseDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="14" ToolTip="Enter Expected Close Date."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Requirement
                                        <asp:RequiredFieldValidator ID="rfvRequirement" ControlToValidate="txtRequirement" ValidationGroup="vgService"
                                            Text="*" ErrorMessage="Please enter requirement" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRequirement" runat="server" TextMode="MultiLine" TabIndex="15" Rows="8" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvService" runat="server" ShowFooter="false" AutoGenerateColumns="False" Width="80%" TabIndex="21" class="table"
                                DataSourceID="DataSourceServices" DataKeyNames="lid" Style="border-collapse: initial; border: 1px solid #5D7B9D;" OnRowCommand="gvService_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
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
                                    <asp:BoundField DataField="ServiceName" HeaderText="Product" ReadOnly="true" />
                                    <asp:BoundField DataField="ServiceLocation" HeaderText="Location" ReadOnly="true" />
                                    <asp:BoundField DataField="VolumeExpected" HeaderText="Volume Expected" ReadOnly="true" />
                                    <asp:BoundField DataField="Requirement" HeaderText="Requirement" ReadOnly="true" />
                                    <asp:BoundField DataField="ExpectedCloseDate" HeaderText="Expected Close Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelVisitReport" TabIndex="4" HeaderText="Visit Report">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Add Report</legend>
                            <div class="m clear">
                                <asp:Button ID="btnAddVisitReport" runat="server" Text="Save" OnClick="btnAddVisitReport_Click" TabIndex="35" CausesValidation="true" ValidationGroup="vgVisitReport" />
                                <asp:Button ID="btnCancelVisitReport" runat="server" Text="Cancel" TabIndex="36" OnClick="btnCancelVisitReport_Click" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white">
                                <colgroup>
                                    <col width="10%" />
                                    <col width="90%" />
                                </colgroup>
                                <tr>
                                    <td>Visit Date
                                        <AjaxToolkit:CalendarExtender ID="calVisitDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgVisitDate"
                                            Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtVisitDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <AjaxToolkit:MaskedEditExtender ID="meeVisitDate" TargetControlID="txtVisitDate" Mask="99/99/9999" MessageValidatorTip="true"
                                            MaskType="Date" AutoComplete="false" runat="server">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="mevVisitDate" ControlExtender="meeVisitDate" ControlToValidate="txtVisitDate" IsValidEmpty="false"
                                            InvalidValueMessage="Visit Date is invalid" InvalidValueBlurredMessage="Invalid Visit Date" SetFocusOnError="true"
                                            MinimumValueMessage="Invalid Visit Date" MaximumValueMessage="Invalid Date" 
                                            runat="Server" ValidationGroup="vgVisitReport"></AjaxToolkit:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVisitDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="31" ToolTip="Enter Visit Date."></asp:TextBox>
                                        <asp:Image ID="imgVisitDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Visit Category
                                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddCategory" SetFocusOnError="true" Display="Dynamic"
                                            InitialValue="0" ErrorMessage="Please Select Customer" Text="*" ValidationGroup="vgVisitReport"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="DataSourceVisitCategory" DataTextField="CategoryName"
                                            DataValueField="lid" CssClass="form-control dropdown" AppendDataBoundItems="true" Width="370px" TabIndex="1">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Visit Remark
                                        <asp:RequiredFieldValidator ID="rfvVisitRemark" runat="server" ControlToValidate="txtVisitRemark" SetFocusOnError="true"
                                            Display="Dynamic" ErrorMessage="Please Enter Visit Remark" Text="*" ForeColor="Red" ValidationGroup="vgVisitReport"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVisitRemark" runat="server" TextMode="MultiLine" Rows="3" Width="99%" TabIndex="32"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvVisitReport" runat="server" ShowFooter="false" AutoGenerateColumns="False" Width="90%" TabIndex="21" class="table"
                                DataSourceID="DataSourceVisitReport" DataKeyNames="lid" Style="border-collapse: initial; border: 1px solid #5D7B9D;" OnRowCommand="gvVisitReport_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="delete report" Text="Delete"
                                                CommandName="deleterow" CommandArgument='<%#Eval("lid")%>' OnClientClick="return confirm('Are you sure to delete report?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="VisitDate" HeaderText="Visit Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 400px; white-space:normal;">
                                            <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="Remark" HeaderText="Remark" ReadOnly="true" />--%>
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelMOM" TabIndex="5" HeaderText="MOM">
                    <ContentTemplate>
                        <div class="clear" style="text-align: center">
                            <asp:Button ID="btnSaveMOM" runat="server" ValidationGroup="vgMOM" Text="Save" OnClick="btnSaveMOM_Click" TabIndex="50" />
                            <asp:Button ID="btnCancelMOM" runat="server" Text="Cancel" OnClick="btnCancelMOM_Click" TabIndex="51" />
                        </div>
                        <fieldset style="margin: 0px">
                            <legend>MOM Info</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white">
                                <colgroup>
                                    <col width="15%" />
                                    <col width="35%" />
                                    <col width="15%" />
                                    <col width="35%" />
                                </colgroup>
                                <tr>
                                    <td>Meeting Title
                                    <asp:RequiredFieldValidator ID="rfvMeetingTitle" runat="server" ControlToValidate="txtMeetingTitle" SetFocusOnError="true"
                                        Display="Dynamic" ForeColor="Red" ErrorMessage="Meeting Title is required" Text="*" ValidationGroup="vgMOM"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMeetingTitle" runat="server" CssClass="form-control" TabIndex="40" Width="260px"></asp:TextBox>
                                    </td>
                                    <td>Meeting Date
                                    <AjaxToolkit:CalendarExtender ID="calMeetingDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgMeetingDate"
                                        Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtMeetingDate">
                                    </AjaxToolkit:CalendarExtender>
                                        <AjaxToolkit:MaskedEditExtender ID="meeMeetingDate" TargetControlID="txtMeetingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                            MaskType="Date" AutoComplete="false" runat="server">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="mevMeetingDate" ControlExtender="meeMeetingDate" ControlToValidate="txtMeetingDate" IsValidEmpty="true"
                                            InvalidValueMessage="Meeting Date is invalid" InvalidValueBlurredMessage="Invalid Meeting Date" SetFocusOnError="true"
                                            MinimumValueMessage="Invalid Meeting Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                            runat="Server"></AjaxToolkit:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMeetingDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="41" ToolTip="Enter Visit Date."></asp:TextBox>
                                        <asp:Image ID="imgMeetingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Start Time
                                        <AjaxToolkit:MaskedEditExtender ID="meeStartTime" runat="server" AcceptAMPM="false" MaskType="Time"
                                            Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                            ErrorTooltipEnabled="true" UserTimeFormat="TwentyFourHour" TargetControlID="txtStartTime"
                                            InputDirection="LeftToRight" AcceptNegative="Left">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="mevStartTime" runat="server" ControlExtender="meeStartTime"
                                            ControlToValidate="txtStartTime" IsValidEmpty="False" EmptyValueMessage="Start Time is required"
                                            InvalidValueMessage="Start Time is invalid" Display="Dynamic" EmptyValueBlurredText="*"
                                            InvalidValueBlurredMessage="Invalid Start Time" ValidationGroup="vgMOM" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartTime" runat="server" Text='<%# Bind("StartTime", "{0:t}") %>' Width="60px" TabIndex="42"></asp:TextBox>
                                        (24 Hours Format)
                                    </td>
                                    <td>End Time
                                        <AjaxToolkit:MaskedEditExtender ID="meeEndTime" runat="server" AcceptAMPM="false" MaskType="Time"
                                            Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                            ErrorTooltipEnabled="true" UserTimeFormat="TwentyFourHour" TargetControlID="txtEndTime"
                                            InputDirection="LeftToRight" AcceptNegative="Left">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="mevEndTime" runat="server" ControlExtender="meeEndTime"
                                            ControlToValidate="txtEndTime" IsValidEmpty="False" EmptyValueMessage="End Time is required"
                                            InvalidValueMessage="End Time is invalid" Display="Dynamic" EmptyValueBlurredText="*"
                                            InvalidValueBlurredMessage="Invalid End Time" ValidationGroup="vgMOM" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndTime" runat="server" Text='<%# Bind("EndTime", "{0:t}") %>' Width="60px" TabIndex="43"></asp:TextBox>
                                        (24 Hours Format)
                                    </td>
                                </tr>
                                <tr>
                                    <td>Resources</td>
                                    <td>
                                        <asp:TextBox ID="txtResources" TextMode="MultiLine" runat="server" TabIndex="44" Rows="2"></asp:TextBox>
                                    </td>
                                    <td>Observers</td>
                                    <td>
                                        <asp:TextBox ID="txtObservers" TextMode="MultiLine" runat="server" TabIndex="45" Rows="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Special Notes</td>
                                    <td>
                                        <asp:TextBox ID="txtSpecialNotes" TextMode="MultiLine" runat="server" TabIndex="46" Rows="2"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Attendee Info</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white">
                                <colgroup>
                                    <col width="15%" />
                                    <col width="35%" />
                                    <col width="15%" />
                                    <col width="35%" />
                                </colgroup>
                                <tr>
                                    <td>Name
                                        <asp:RequiredFieldValidator ID="rfvAttendeeName" runat="server" ControlToValidate="txtName" SetFocusOnError="true" Display="Dynamic"
                                            ForeColor="Red" ErrorMessage="Attendee Name is required" Text="*" ValidationGroup="vgAttendee"></asp:RequiredFieldValidator>
                                        <AjaxToolkit:AutoCompleteExtender ID="UserExtender" runat="server" TargetControlID="txtName"
                                            CompletionListElementID="divwidthName" ServicePath="~/WebService/UserAutoComplete.asmx"
                                            ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthName"
                                            ContextKey="4567" UseContextKey="True" OnClientItemSelected="OnUserSelected"
                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                        </AjaxToolkit:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName" Width="250px" runat="server" ToolTip="Select OR Enter Attendee Name"
                                            CssClass="SearchTextbox" placeholder="Search" TabIndex="61" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                        <div id="divwidthName" runat="server">
                                        </div>
                                    </td>
                                    <td>Email
                                        <asp:RequiredFieldValidator ID="rfvAttendeeEmail" runat="server" ControlToValidate="txtAttendeeEmail" SetFocusOnError="true" Display="Dynamic"
                                            ForeColor="Red" ErrorMessage="Attendee Email is required" Text="*" ValidationGroup="vgAttendee"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAttendeeEmail" runat="server" TabIndex="62" placeholder="Attendee Email" Width="250px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revAttendeeEmail" runat="server" ControlToValidate="txtAttendeeEmail"
                                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="(Invalid Email)" ForeColor="Red"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnAddAttendee" runat="server" TabIndex="63" Text="Add Attendee" ValidationGroup="vgAttendee" OnClick="btnAddAttendee_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvExistingAttendee" runat="server" AutoGenerateColumns="false" CssClass="table" Width="90%" OnRowCommand="gvExistingAttendee_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPkId" runat="server" Text='<%#Bind("PkId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UserId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%#Bind("UserId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name" ItemStyle-Width="45%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%#Bind("UserName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmailId" runat="server" Text='<%#Bind("EmailId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField HeaderText="Remove" CommandName="Remove" ButtonType="Button" Text="Remove" ItemStyle-Width="10%" ControlStyle-CssClass="btn btn-xs btn-primary" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset>
                            <legend>Agenda Info</legend>
                            <asp:GridView ID="gvAgenda" runat="server" Width="90%" AutoGenerateColumns="false" ShowFooter="true" CssClass="table table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="PkId" HeaderText="SN" />
                                    <asp:TemplateField HeaderText="Topic" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTopic" runat="server" TabIndex="51" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTopic" runat="server" ControlToValidate="txtTopic" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" ErrorMessage="Required" ValidationGroup="vgAgenda"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="55%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="52" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDescription" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" ErrorMessage="Required" ValidationGroup="vgAgenda"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Person Name" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPersonName" runat="server" TabIndex="53" CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Button ID="btnAddAgenda" runat="server" TabIndex="54" Text="Add New Row" ValidationGroup="vgAgenda" CssClass="btn btn-xs btn-primary" OnClick="btnAddAgenda_Click" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset>
                            <legend>MOMs</legend>
                            <div>
                                <asp:GridView ID="gvMOMs" runat="server" ShowFooter="false" AutoGenerateColumns="False" Width="80%" TabIndex="21" class="table"
                                    DataSourceID="DataSourceMOMs" DataKeyNames="lid" Style="border-collapse: initial; border: 1px solid #5D7B9D;"
                                    PageSize="20" PagerSettings-Position="Bottom" OnRowCommand="gvMOMs_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkShowMOM" runat="server" ToolTip="View MOM" Text="View MOM"
                                                    CommandName="showmom" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Title" HeaderText="Meeting Title" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                                        <asp:BoundField DataField="dtDate" HeaderText="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <%-- POPUP -- > MOM Draft Mail--%>
                        <AjaxToolkit:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
                            DropShadow="False" PopupControlID="pnlMOM" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground">
                        </AjaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlMOM" runat="server" CssClass="ModalPopupPanel">
                            <div style="background-color: #8ab933">
                                <div>
                                    <span style="font-weight: 600; font-size: 18px; font-family: Calibri; color: white; padding-left: 10px">MINUTES OF MEETINGS MAIL
                                    </span>
                                </div>
                            </div>
                            <div class="m"></div>
                            <div id="DivABC" runat="server" style="max-height: 600px; max-width: 900px; overflow: auto;">
                                <div style="padding: 10px; font-size: 14px; margin-left: 10px; margin-right: 10px; margin-bottom: 20px;">
                                    <div id="divMsg_Popup" runat="server"></div>
                                    <asp:HiddenField ID="hdnMomId_Popup" runat="server" Value="0" />
                                    <div style="padding-left: 2px; padding-right: 2px">
                                        <label>Email To :</label>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:TextBox ID="lblCustomerEmail" runat="server" Width="400px"></asp:TextBox>
                                    </div>
                                    <div style="padding-left: 2px; padding-right: 2px">
                                        <label>Email CC :</label>
                                        &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtMailCC" runat="server" Width="400px" AutoPostBack="true" OnTextChanged="txtMailCC_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="padding-left: 2px; padding-right: 2px">
                                        <label>Subject :</label>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtSubject" runat="server" Width="400px" Enabled="false" AutoPostBack="true" OnTextChanged="txtSubject_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="padding-left: 2px; padding-right: 2px">
                                        <label>Participants :</label>
                                        &nbsp;&nbsp;
                                        <asp:TextBox ID="txtParticipants" runat="server" Width="400px" AutoPostBack="true" OnTextChanged="txtParticipants_TextChanged"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="divPreviewEmail" runat="server" style="margin-left: 10px; margin-right: 10px; margin-bottom: 20px;">
                                </div>
                            </div>
                            <div style="text-align: center">
                                <asp:Button ID="btnSendMail" runat="server" TabIndex="2" OnClick="btnSendMail_OnClick" Text="SEND MAIL" CssClass="btn btn-3d btn-primary" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelPopup" runat="server" TabIndex="3" OnClick="btnCancelPopup_OnClick" Text="CANCEL" CssClass="btn btn-3d btn-default" />
                            </div>
                        </asp:Panel>
                        <asp:HiddenField ID="lnkDummy" runat="server"></asp:HiddenField>
                        <!--Customer Email Draft End -->
                        <%-- POPUP -- > MOM Draft Mail--%>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelStatusHistory" TabIndex="6" HeaderText="Status History">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Lead Status</legend>
                            <%--<table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Select Status
                                        <asp:RequiredFieldValidator ID="rfvstatus" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Select Status." Text="*" ValidationGroup="vgAddStatus" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="150px" TabIndex="31">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="Quote"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="Lost"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Remark
                                        <asp:RequiredFieldValidator ID="rfvremark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgAddStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="800px" TabIndex="32">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSaveStatus" runat="server" OnClick="btnSaveStatus_Click" CausesValidation="true" ValidationGroup="vgAddStatus"
                                            Text="Save Status" TabIndex="33" />
                                    </td>
                                </tr>
                            </table>--%>
                            <div>
                                <asp:GridView ID="gvStatusHistory" runat="server" CssClass="table" PagerStyle-CssClass="pgr"
                                    AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" DataSourceID="DataSourceStatusHistory"
                                    PageSize="30" PagerSettings-Position="TopAndBottom" Style="white-space: normal"
                                    DataKeyNames="lid" Width="99%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="LeadStageName" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Modified By" DataField="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Modified Date" DataField="UpdateOn" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CRM_GetLeadStageHistory" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </AjaxToolkit:TabContainer>
            <div id="divDataSource">
                <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetLeadStageMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceSource" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetLeadSourceMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceBusinessSector" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="KYC_GetVarientMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceCompanyType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetCompanyTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceBusinessCatg" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetBusinessCategoryMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceRole" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetRoleMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceLead" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetLeadByLid" SelectCommandType="StoredProcedure" DeleteCommand="CRM_delLeads"
                    DeleteCommandType="StoredProcedure" UpdateCommand="CRM_updLead" UpdateCommandType="StoredProcedure"
                    OnUpdated="DataSourceLead_Updated" OnSelected="DataSourceLead_Selected" OnDeleted="DataSourceLead_Deleted">
                    <SelectParameters>
                        <asp:SessionParameter Name="lId" SessionField="LeadId" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="lId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="lId" />
                        <asp:Parameter Name="LeadSourceId" Type="Int32" />
                        <asp:Parameter Name="SourceDescription" Type="String" />
                        <asp:Parameter Name="SectorId" Type="Int32" />
                        <asp:Parameter Name="CompanyTypeId" Type="Int32" />
                        <asp:Parameter Name="ContactName" Type="String" />
                        <asp:Parameter Name="CatgId" Type="Int32" />
                        <asp:Parameter Name="Designation" Type="String" />
                        <asp:Parameter Name="RoleId" Type="Int32" />
                        <asp:Parameter Name="Email" Type="String" />
                        <asp:Parameter Name="MobileNo" Type="String" />
                        <asp:Parameter Name="ContactNo" Type="String" />
                        <asp:Parameter Name="OfficeLocation" Type="String" />
                        <asp:Parameter Name="AddressLine1" Type="String" />
                        <asp:Parameter Name="AddressLine2" Type="String" />
                        <asp:Parameter Name="AddressLine3" Type="String" />
                        <asp:Parameter Name="Description" Type="String" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceAllAttendees" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetAllAttendees" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceVisitReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetVisitReport" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceServices" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetServices" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceContacts" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetContactsByLead" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceServiceLocation" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetAllBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceMOMs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetMomByLeadId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceVisitCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_GetVisitCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


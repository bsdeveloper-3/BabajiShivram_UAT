<%@ Page Title="Lead Status" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LeadStatus.aspx.cs"
    Inherits="CRM_LeadStatus" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
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
            <div id="divInbond" class="info" runat="server" align="center">
                <asp:Label ID="lblInbondJobNo" runat="server"></asp:Label>
                <asp:HiddenField ID="hdnSalesPersonId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnKAMId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnEnquiryId" runat="server" />
                <asp:HiddenField ID="hdnLid" runat="server" Value="0" />
            </div>
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
                </div>
            </fieldset>
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
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Lead Status</legend>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Select Status <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvstatus" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Status." Text="*" ValidationGroup="vgAddStatus" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                                    <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                    <%--<asp:ListItem Value="2" Text="First Attempt"></asp:ListItem>--%>
                                    <asp:ListItem Value="3" Text="Under Progress"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Converted"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Send lead for approval?"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Under Follow Up"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="Cold"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="Contact Established"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Remark <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvremark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgAddStatus"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="800px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSaveStatus" runat="server" OnClick="btnSaveStatus_Click" CausesValidation="true" ValidationGroup="vgAddStatus"
                                    Text="Save Status" />
                            </td>
                        </tr>
                    </table>
                    <br />
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
            <div>
                <asp:SqlDataSource ID="DataSourceServices" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetServices" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
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


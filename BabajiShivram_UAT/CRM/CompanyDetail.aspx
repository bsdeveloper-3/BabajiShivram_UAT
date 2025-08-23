<%@ Page Title="Company Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CompanyDetail.aspx.cs"
    Inherits="CRM_CompanyDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlComapnyDetail"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ActiveTabChangedCustomer() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <asp:UpdatePanel ID="upnlComapnyDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:HiddenField ID="hdnUserCountryId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCustFilePath" runat="server" Value="" />
                <asp:Label ID="lblError" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
            <cc1:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" CssClass="Tab" CssTheme="None" OnClientActiveTabChanged="ActiveTabChangedCustomer">
                <cc1:TabPanel ID="TabCustomer" runat="server" HeaderText="Company">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Company Detail</legend>
                            <asp:FormView ID="FormView1" runat="server" Width="100%" OnItemDeleted="FormView1_ItemDeleted"
                                OnItemUpdated="FormView1_ItemUpdated" OnItemInserted="FormView1_ItemInserted"
                                DataSourceID="FormviewSqlDataSource" DataKeyNames="lid" OnDataBound="FormView1_DataBound">
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" Text="Update"
                                            ValidationGroup="Required" TabIndex="15" />
                                        <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="Cancel" TabIndex="16" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>Company Name
                                            </td>
                                            <td>
                                                <%--<asp:Label ID="txtCustomerName" runat="server" Text='<%# Eval("sName") %>'
                                                    MaxLength="100" TabIndex="1" Width="400px"></asp:Label>--%>

                                                <asp:TextBox ID="txtCustName" runat="server" Text='<%# Bind("sName") %>'
                                                    MaxLength="100" TabIndex="1" Width="400px"></asp:TextBox>

                                            </td>
                                            <td>Corporate Contact Person
                                                <asp:RequiredFieldValidator ID="RFVEditContactPerson" runat="server" ControlToValidate="txtContactPerson"
                                                    Text="*" ErrorMessage="Please Enter Contact Person Name" ValidationGroup="Required"
                                                    SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactPerson" runat="server" Text='<%# Bind("ContactPerson") %>'
                                                    MaxLength="200" TabIndex="2" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Email
                                                <asp:RequiredFieldValidator ID="RFVeditEmail" runat="server" ControlToValidate="txtEmail"
                                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Email" ValidationGroup="Required"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegExeditEmail" runat="server" ControlToValidate="txtEmail"
                                                    ErrorMessage="Please Enter Valid Email. Enter Comma-Separated For Multiple Email."
                                                    Text="*" ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*"
                                                    ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>'
                                                    Rows="2" MaxLength="200" TabIndex="3" Width="400px"></asp:TextBox>&nbsp;
                                            </td>
                                            <td>Mobile No.
                                                <asp:RequiredFieldValidator ID="RFVeditMobNo" runat="server" ControlToValidate="txtMobileNo"
                                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Mobile No." ValidationGroup="Required"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobileNo" runat="server" Text='<%# Bind("MobileNo") %>' MaxLength="200" Width="400px"
                                                    TabIndex="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contact No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactNo" runat="server" Text='<%# Bind("ContactNo") %>' MaxLength="200" Width="400px"
                                                    TabIndex="5"></asp:TextBox>
                                            </td>
                                            <td>Address Line 1
                                                <asp:RequiredFieldValidator ID="rfvCorporateAddress" runat="server" ControlToValidate="txtAddressLine1"
                                                    Text="*" ErrorMessage="Please Enter Corporate Address" ValidationGroup="Required"
                                                    Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddressLine1" runat="server" MaxLength="400" Text='<%# Bind("AddressLine1") %>'
                                                    TextMode="MultiLine" TabIndex="6" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address Line 2
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddressLine2" runat="server" MaxLength="400" Text='<%# Bind("AddressLine2") %>'
                                                    TextMode="MultiLine" TabIndex="7" Width="400px"></asp:TextBox>
                                            </td>
                                            <td>Address Line 3
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddressLine3" runat="server" MaxLength="400" Text='<%# Bind("AddressLine3") %>'
                                                    TextMode="MultiLine" TabIndex="8" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Office Location
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOfficeLocation" runat="server" MaxLength="100" Text='<%# Bind("OfficeLocation") %>'
                                                    TabIndex="9" Width="400px"></asp:TextBox>
                                            </td>
                                            <td>Website</td>
                                            <td>
                                                <asp:TextBox ID="txtWebsite" runat="server" Text='<%# Bind("Website") %>' MaxLength="200" Width="400px"
                                                    TabIndex="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');"
                                            Text="Delete" />
                                        <asp:Button ID="btnCancelCust" runat="server" Text="Back" OnClick="btnCancel_Click" />
                                    </div>
                                    <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>Company Name
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("sName") %>'></asp:Label>
                                            </td>
                                            <td>Corporate Contact Person
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContactPerson" runat="server" Text='<%# Eval("ContactPerson") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Email
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                            </td>
                                            <td>Mobile No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contact No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContactNo" runat="server" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                            </td>
                                            <td>Address Line 1
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lbaddress" runat="server" Text='<%# Eval("AddressLine1") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address Line 2
                                            </td>
                                            <td>
                                                <asp:Label ID="lbladdress2" runat="server" Text='<%# Eval("AddressLine2") %>'></asp:Label>
                                            </td>
                                            <td>Address Line 3
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lbladdress3" runat="server" Text='<%# Eval("AddressLine3") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Office Location
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOfficeLocation" runat="server" Text='<%# Eval("OfficeLocation") %>'></asp:Label>
                                            </td>
                                            <td>Website</td>
                                            <td>
                                                <asp:Label ID="lblWebsite" runat="server" Text='<%# Eval("Website") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                            <div id="divDataSource">
                                <asp:SqlDataSource ID="FormviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CRM_GetCompanyAsPerLid" SelectCommandType="StoredProcedure" DeleteCommand="CRM_delCompanyMS"
                                    DeleteCommandType="StoredProcedure" UpdateCommand="CRM_updCompanyMS" UpdateCommandType="StoredProcedure"
                                    OnUpdated="FormviewSqlDataSource_Updated" OnSelected="FormviewSqlDataSource_Selected"
                                    OnDeleted="FormviewSqlDataSource_Deleted">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="lId" SessionField="CompanyId" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="lId" />
                                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                                    </DeleteParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="lId" />
                                        <asp:Parameter Name="sName" Type="String" />
                                        <asp:Parameter Name="ContactPerson" Type="String" />
                                        <asp:Parameter Name="Email" Type="String" />
                                        <asp:Parameter Name="MobileNo" Type="String" />
                                        <asp:Parameter Name="ContactNo" Type="String" />
                                        <asp:Parameter Name="AddressLine1" Type="String" />
                                        <asp:Parameter Name="AddressLine2" Type="String" />
                                        <asp:Parameter Name="AddressLine3" Type="String" />
                                        <asp:Parameter Name="Website" Type="String" />
                                        <asp:Parameter Name="Description" Type="String" />
                                        <asp:Parameter Name="OfficeLocation" Type="String" />
                                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


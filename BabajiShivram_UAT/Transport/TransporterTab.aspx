<%@ Page Title="Transporter Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransporterTab.aspx.cs" 
    Inherits="Transport_TransporterTab" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVpager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div align="center">
        <asp:HiddenField ID="hdnUserCountryId" runat="server" Value="10" />
        <asp:HiddenField ID="hdnCustFilePath" runat="server" Value="" />
        <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
    </div>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>

            <script type="text/javascript">

<%--                function OnCustomerSelected(source, eventArgs) {
                    var results = eval('(' + eventArgs.get_value() + ')');
                    $get('<%=hdnCustId.ClientID%>').value = results.ClientId;           
                    }
                    $addHandler
                    (
                        $get('txtCustomerName'), 'keyup',

                        function () {
                            $get('<%=txtCustomer.ClientID%>').value = '0';
                    }
                );--%>

                function OnCountrySelected(source, eventArgs) {
                    var results = eval('(' + eventArgs.get_value() + ')');
                    $get('<%=hdnUserCountryId.ClientID%>').value = results.CountryId;
                }
                function disableEnterKey(e) {
                    //create a variable to hold the number of the key that was pressed
                    var key;
                    //if the users browser is internet explorer
                    if (window.event) {
                        //store the key code (Key number) of the pressed key
                        key = window.event.keyCode;
                        //otherwise, it is firefox 
                    } else {
                        //store the key code (Key number) of the pressed key 
                        key = e.which;
                    }
                    //if key 13 is pressed (the enter key) 
                    if (key == 13) {
                        //do nothing
                        return false;
                        //otherwise
                    } else {
                        //continue as normal (allow the key press for keys other than "enter") 
                        return true;
                    }
                    //and don't forget to close the function   
                }
            </script>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsBankReq" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgBankReq" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="ConsigneeRequired" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="RequiredField" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="valPlantAddress" runat="server" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="PlantAddressRequired" EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary4" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="CreditRequiredField" CssClass="errorMsg"
                    EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary7" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="updCreditRequiredField" CssClass="errorMsg"
                    EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary6" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="RequiredFieldforDate" CssClass="errorMsg"
                    EnableViewState="false" />
                <asp:ValidationSummary ID="ValidationSummary5" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="RequiredFieldforweeks" CssClass="errorMsg"
                    EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="Tab"
                CssTheme="None" >
                <cc1:TabPanel ID="TabCustomer" runat="server" HeaderText="Transporter">
                    <ContentTemplate>
                        <div style="overflow: auto; width: 100%;">
                            <fieldset>
                                <legend>Transporter Detail</legend>
                                <asp:FormView ID="FormView1" runat="server" Width="100%" OnItemDeleted="FormView1_ItemDeleted"
                                    OnItemUpdated="FormView1_ItemUpdated" 
                                    DataSourceID="FormviewSqlDataSource" DataKeyNames="lid" OnDataBound="FormView1_DataBound">
                                    <EditItemTemplate>
                                        <div class="m clear">
                                            <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" Text="Update"
                                                ValidationGroup="Required" TabIndex="12" />
                                            <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                                Text="Cancel" TabIndex="13" />
                                        </div>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>Transporter Name
                                                    <asp:RequiredFieldValidator ID="RFVEditName" runat="server" Text="*" Display="Dynamic"
                                                        ErrorMessage="Please Enter Customer Name" ControlToValidate="txtUpdCustomerName"
                                                        ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUpdCustomerName" runat="server" Text='<%# Bind("CustName") %>'
                                                        MaxLength="100" TabIndex="1" Width="80%" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>Corporate Contact Person
                                                    <asp:RequiredFieldValidator ID="RFVEditContactPerson" runat="server" ControlToValidate="txtUpdContactPerson"
                                                        Text="*" ErrorMessage="Please Enter Contact Person Name" ValidationGroup="Required"
                                                        SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUpdContactPerson" runat="server" Text='<%# Bind("ContactPerson") %>'
                                                        MaxLength="200" TabIndex="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Email
                                                    <asp:RequiredFieldValidator ID="RFVeditEmail" runat="server" ControlToValidate="txtUpdEmail"
                                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Email" ValidationGroup="Required"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegExeditEmail" runat="server" ControlToValidate="txtUpdEmail"
                                                        ErrorMessage="Please Enter Valid Email. Enter Comma-Separated For Multiple Email."
                                                        Text="*" ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*"
                                                        ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUpdEmail" runat="server" Text='<%# Bind("email") %>' TextMode="MultiLine"
                                                        Rows="2" MaxLength="200" TabIndex="3"></asp:TextBox>&nbsp;
                                                </td>
                                                <td>Mobile No.
                                                    <asp:RequiredFieldValidator ID="RFVeditMobNo" runat="server" ControlToValidate="txtUpdMobile"
                                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Mobile No." ValidationGroup="Required"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUpdMobile" runat="server" Text='<%# Bind("MobileNo") %>' MaxLength="200"
                                                        TabIndex="4"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Contact No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContactNo" runat="server" Text='<%# Bind("ContactNo") %>' MaxLength="200"
                                                        TabIndex="5"></asp:TextBox>
                                                </td>
                                                <td>Transporter Code
                                                    <asp:RequiredFieldValidator ID="RFVCode" runat="server" ControlToValidate="txtCustCode"
                                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Code" ValidationGroup="Required"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustCode" runat="server" Text='<%# Bind("CustCode") %>' MaxLength="100"
                                                        TabIndex="6" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Corporate Address
                                                    <asp:RequiredFieldValidator ID="RFV9" runat="server" ControlToValidate="txtUpdAddress"
                                                        Text="*" ErrorMessage="Please Enter Corporate Address" ValidationGroup="Required"
                                                        Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtUpdAddress" runat="server" MaxLength="400" Text='<%# Bind("Address") %>'
                                                        TextMode="MultiLine" TabIndex="7"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td>Referred by
                                                </td>
                                                <td>
                                                    <div id="divwidthCust" runat="server"></div>
                                                    <asp:TextBox ID="txtUpdReferredBy" runat="server" MaxLength="100" Text='<%# Bind("ReferredBy") %>'
                                                        TabIndex="11" onKeyPress="return disableEnterKey(event)" onkeyUp="Javascript:ResetUser();"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="UserExtender" runat="server" BehaviorID="divwidthCust"
                                                        CompletionListCssClass="AutoExtender" CompletionListElementID="divwidthCust"
                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"
                                                        FirstRowSelected="true"  ServiceMethod="GetUserCompletionList"
                                                        OnClientItemSelected="OnUserSelected" ServicePath="~/WebService/UserAutoComplete.asmx"
                                                        TargetControlID="txtUpdReferredBy" UseContextKey="True">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    
                                    <ItemTemplate>
                                        <div class="m clear">
                                            <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                                            <asp:Button ID="btnCancelCust" runat="server" Text="Back" OnClick="btnCancel_Click" />
                                        </div>
                                        <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td>Transporter Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbcustomername" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%# Bind("lId") %>' />
                                                </td>
                                                <td>Corporate Contact Person
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbcontactPerson" runat="server" Text='<%# Bind("ContactPerson") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Email
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbemail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                    <td>Mobile No
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbmobileno" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td>Contact No
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblContactNO" runat="server" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                                </td>
                                                <td>Transporter Code
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustCode" runat="server" Text='<%# Eval("CustCode") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Corporate Address
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label ID="lbaddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td>Referred By
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReferredBy" runat="server" Text='<%# Bind("ReferredBy") %>'></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:FormView>
                            </fieldset>
                        </div>
                        <div id="divDataSource">
                            <asp:SqlDataSource ID="FormviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerBylid" SelectCommandType="StoredProcedure" DeleteCommand="DelCustomer"
                                DeleteCommandType="StoredProcedure" InsertCommand="insCustomerMS" InsertCommandType="StoredProcedure"
                                UpdateCommand="updCustomerMS" UpdateCommandType="StoredProcedure" 
                                OnUpdated="FormviewSqlDataSource_Updated" OnSelected="FormviewSqlDataSource_Selected" >
                                <SelectParameters>
                                    <asp:SessionParameter Name="lId" SessionField="TR_TransporterID" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="lid" />
                                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="CustName" Type="String" />
                                    <asp:Parameter Name="CustCode" Type="String" />
                                    <asp:Parameter Name="ContactPerson" Type="String" />
                                    <asp:Parameter Name="Email" Type="String" />
                                    <asp:Parameter Name="MobileNo" Type="String" />
                                    <asp:Parameter Name="ContactNo" Type="String" />
                                    <asp:Parameter Name="Address" Type="String" />
                                    <asp:Parameter Name="PCDRequired" Type="Boolean" DefaultValue="0" />
                                    <asp:Parameter Name="ReferredBy" Type="String" />
                                    <asp:Parameter Name="TransportationRequired" Type="Boolean" DefaultValue="0"/>
                                    <asp:Parameter Name="SVBApplicable" Type="Boolean" DefaultValue="0"/>
                                    <asp:Parameter Name="IECNo" Type="String" DefaultValue="" />
                                    <asp:Parameter Name="IncomeTaxNo" Type="String" DefaultValue="" />
                                    <asp:Parameter Name="MovementRequired" Type="Boolean" DefaultValue="0"/>
                                    <asp:Parameter Name="CustChecklistApproval" Type="Boolean" DefaultValue="0"/>
                                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                                    <asp:Parameter Name="lid" />
                                </UpdateParameters>
                                
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <!-- Customer User Tab -->
                <cc1:TabPanel ID="TabUser" runat="server" HeaderText="Contact">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Transporter Contact</legend>
                            <div>
                                <asp:Button ID="btnNewUser" runat="server" Text="New Contact" OnClick="btnNewUser_Click" />
                            </div>
                            <asp:GridView ID="gvCustomerUser" runat="server" DataSourceID="GridViewSqlDataSource"
                                DataKeyNames="lid" AllowPaging="True" PageSize="20" AllowSorting="true"
                                AutoGenerateColumns="False" CssClass="table" AlternatingRowStyle-CssClass="alt"
                                PagerStyle-CssClass="pgr" OnSelectedIndexChanged="gvCustomerUser_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField DataTextField="EmpName" HeaderText="Name" CommandName="select" SortExpression="EmpName" />
                                    <%--<asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />--%>
                                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" />
                                    <asp:BoundField DataField="LoginRequired" HeaderText="Login Required" SortExpression="LoginRequired" />
                                    <asp:BoundField DataField="LastLoginDate" HeaderText="Login Date" SortExpression="LastLoginDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerId" Text='<%#Bind("CustomerId") %>' runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbluserId" Text='<%#Bind("lid") %>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div id="divDataDource11">
                            <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerUser" SelectCommandType="StoredProcedure" InsertCommand="insCustomerUser"
                                InsertCommandType="StoredProcedure" UpdateCommand="updCustomerUserMS" UpdateCommandType="StoredProcedure"
                                DeleteCommandType="StoredProcedure" DeleteCommand="delCustomerUserDetail">
                                <SelectParameters>
                                    <asp:SessionParameter SessionField="TR_TransporterID" Name="CustomerId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <asp:FormView ID="FormViewUser" runat="server" DataKeyNames="lId" OnItemDeleted="FormViewUser_ItemDeleted"
                            OnItemUpdated="FormViewUser_ItemUpdated" OnItemUpdating="FormViewUser_Updating"
                            OnItemInserted="FormViewUser_ItemInserted" OnItemInserting="FormViewUser_ItemInserting"
                            DataSourceID="SqlDataSourceUser" OnItemCommand="FormViewUser_ItemCommand" Width="100%"
                            OnDataBound="FormViewUser_DataBound">
                            <EditItemTemplate>
                                <fieldset>
                                    <legend>Update Contact Detail</legend>
                                    <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" ValidationGroup="Required"
                                        TabIndex="9" />
                                    <asp:Button ID="btnUpdateCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel" TabIndex="10" />
                                    <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>Name
                                                <asp:RequiredFieldValidator ID="RFVupdEmpName" runat="server" ControlToValidate="txtEmpName"
                                                    Text="*" SetFocusOnError="true" ErrorMessage="Please Enter Employee Name" Display="Dynamic"
                                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmpName" runat="server" Text='<%# Bind("EmpName") %>' TabIndex="1"></asp:TextBox>
                                            </td>
                                            <td>Email
                                                <asp:RequiredFieldValidator ID="RFVupdEmail" runat="server" ControlToValidate="txtEmail"
                                                    Text="*" ErrorMessage="Please Enter Email" Display="Dynamic" ValidationGroup="Required"
                                                    SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="REVupdEmail" runat="server" ControlToValidate="txtEmail"
                                                    SetFocusOnError="true" Display="Dynamic" Text="Invalid Email" ErrorMessage="Please Enter Valid Email"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Required"></asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' TabIndex="2"
                                                    Width="90%"> </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Designation
                                                <asp:RequiredFieldValidator ID="RFVupdDesig" runat="server" ControlToValidate="txtDesignation"
                                                    Text="*" Display="Dynamic" ErrorMessage="Please Enter Designation" ValidationGroup="Required"
                                                    SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDesignation" runat="server" Text='<%# Bind("Designation") %>'
                                                    TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td>Login Required
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblupdLogin" runat="server" SelectedValue='<%#Eval("LoginActive") %>'
                                                    RepeatDirection="Horizontal" TabIndex="4">
                                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Date Of Birth
                                                <asp:CompareValidator ID="CompareValidator233" runat="server" ControlToValidate="txtDateOfBirth"
                                                    Display="Dynamic" ErrorMessage="Invalid Date Of Birth" Type="Date" Text="Invalid Date"
                                                    CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required"
                                                    SetFocusOnError="true"></asp:CompareValidator>
                                                <cc1:CalendarExtender ID="calDOBUpd" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateOfBirth" PopupPosition="BottomRight"
                                                    TargetControlID="txtDateOfBirth">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateOfBirth" runat="server" Text='<%# Bind("dtBirthDate","{0:dd/MM/yyyy}") %>'
                                                    Width="100px" TabIndex="5"></asp:TextBox>
                                                <asp:Image ID="imgDateOfBirth" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                            </td>
                                            <td>Mobile No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobileNo" runat="server" Text='<%# Bind("MobileNo") %>' MaxLength="50"
                                                    TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Fax No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFaxNo" runat="server" Text='<%# Bind("FaxNo") %>' MaxLength="50"
                                                    TabIndex="7"></asp:TextBox>
                                            </td>
                                            <td>Office No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOfficeNo" runat="server" Text='<%# Bind("OfficeNo") %>' MaxLength="50"
                                                    TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>City
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("CityName") %>' TabIndex="8"></asp:TextBox>
                                            </td>
                                            <td>Country
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="SearchTextbox" Text='<%#Eval("CountryName") %>'
                                                    TabIndex="8"></asp:TextBox>
                                                <div id="divwidthCountry">
                                                </div>
                                                <cc1:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountry"
                                                    CompletionListElementID="divwidthCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                                                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                                                    ContextKey="4244" UseContextKey="True" OnClientItemSelected="OnCountrySelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                </cc1:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Password Reset Required
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdPasswordReset" runat="server" RepeatDirection="Horizontal"
                                                    TabIndex="9">
                                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>Password Reset Days
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPasswdDaysEdit" runat="server" Width="50px" MaxLength="3" Text='<%# Bind("ResetDays") %>'
                                                    TabIndex="10"></asp:TextBox>
                                                <asp:CompareValidator ID="CompValDays" runat="server" ControlToValidate="txtPasswdDaysEdit"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Number Of Days."
                                                    Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>Reset Login Password</legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>Password
                                                <asp:RequiredFieldValidator ID="RFVPass" runat="server" ControlToValidate="txtPassCode"
                                                    Text="Required" Display="Dynamic" ErrorMessage="*" ValidationGroup="PassRequired"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassCode" runat="server" TextMode="Password" MaxLength="50" TabIndex="11"></asp:TextBox>
                                                <asp:Button ID="btnPasswrd" OnClick="btnPasswrd_Click" runat="server" Text="Reset Password"
                                                    ValidationGroup="PassRequired" TabIndex="12" />
                                            </td>
                                            <td colspan="2"></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <fieldset>
                                    <legend>Add Contact Detail</legend>
                                    <div class="m clear">
                                        <asp:Button ID="btnSave" runat="server" CommandName="Insert" ValidationGroup="Required"
                                            Text="Save" TabIndex="13" />
                                        <asp:Button ID="btnSaveCancel" runat="server" Text="Cancel" CausesValidation="False"
                                            OnClick="btnCancel_Click" TabIndex="14" />
                                    </div>
                                    <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>Name
                                                <asp:RequiredFieldValidator ID="RFVinsEmpName" runat="server" ControlToValidate="txtEmpName"
                                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Employee Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmpName" runat="server" Text='<%# Bind("EmpName") %>'
                                                    TabIndex="1" Width="90%"></asp:TextBox>
                                            </td>
                                            <td>Email
                                                <asp:RequiredFieldValidator ID="RFVinsEmail" runat="server" ControlToValidate="txtEmail"
                                                    Text="*" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Email"
                                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="REVEmail" runat="server" ControlToValidate="txtEmail"
                                                    SetFocusOnError="true" Display="Dynamic" Text="Invalid Email" ErrorMessage="Please Enter Valid Email"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Required"></asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' TabIndex="2"
                                                    Width="90%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Designation
                                                <asp:RequiredFieldValidator ID="RFVDesig" runat="server" ControlToValidate="txtDesignation"
                                                    Text="*" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Designation"
                                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDesignation" runat="server" Text='<%# Bind("Designation") %>'
                                                    TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td>Mobile No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'
                                                    TabIndex="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Login Required
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblLogin" runat="server" SelectedValue='<%#Bind("LoginActive") %>'
                                                    RepeatDirection="Horizontal" TabIndex="5">
                                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>Password
                                                <asp:RequiredFieldValidator ID="RFVinsPass" runat="server" ControlToValidate="txtPassword"
                                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Password" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassword" runat="server" Text='<%# Bind("Password") %>' MaxLength="50"
                                                    TextMode="Password" TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Fax No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFaxNo" runat="server" Text='<%# Bind("FaxNo") %>' TabIndex="7"></asp:TextBox>
                                            </td>
                                            <td>Office No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOfficeNo" runat="server" Text='<%# Bind("OfficeNo") %>' TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>City
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("CityName") %>' TabIndex="9"></asp:TextBox>
                                            </td>
                                            <td>Country
                                                <asp:RequiredFieldValidator ID="RFVCountryNew" runat="server" ControlToValidate="txtCountry"
                                                    Text="*" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Select Country Name"
                                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="SearchTextbox" Text='<%#Eval("CountryName") %>'
                                                    TabIndex="10"></asp:TextBox>
                                                <div id="divwidthCountry">
                                                </div>
                                                <cc1:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountry"
                                                    CompletionListElementID="divwidthCountry" ServicePath="WebService/CountryAutoComplete.asmx"
                                                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                                                    ContextKey="4104" UseContextKey="True" OnClientItemSelected="OnCountrySelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                </cc1:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Password Reset Required
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdPasswordReset" runat="server" RepeatDirection="Horizontal"
                                                    TabIndex="11">
                                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>Password Reset Days
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPasswdDays" runat="server" Width="50px" MaxLength="3" Text='<%# Bind("ResetDays") %>'
                                                    TabIndex="12"></asp:TextBox>
                                                <asp:CompareValidator ID="CompValDays" runat="server" ControlToValidate="txtPasswdDays"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Number Of Days."
                                                    Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <fieldset>
                                    <legend>Contact Detail</legend>
                                    <div class="m clear">
                                        <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');"
                                            Text="Delete" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                    </div>
                                    <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>Employee Name
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmployee" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                            </td>
                                            <td>Email
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Designation
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                            </td>
                                            <td>Login Required
                                            </td>
                                            <td>
                                                <%# (Boolean.Parse(Eval("LoginActive").ToString())) ? "Yes" : "No"%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Date Of Birth
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Eval("dtBirthdate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>Mobile No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>City
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCityName" runat="server" Text='<%# Eval("CityName") %>'></asp:Label>
                                            </td>
                                            <td>Country
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCountryName" runat="server" Text='<%# Eval("CountryName") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Fax No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFaxNo" runat="server" Text='<%# Eval("FaxNo") %>'></asp:Label>
                                            </td>
                                            <td>Office No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOfficeNo" runat="server" Text='<%# Eval("OfficeNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Password Reset Required
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDailyPasswordReset" runat="server" Text='<%# Bind("ResetCodeRequired") %>'></asp:Label>
                                            </td>
                                            <td>Password Reset Days
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPasswordDays" runat="server" Text='<%# Bind("ResetDays") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ItemTemplate>
                        </asp:FormView>
                        <div id="div5">
                            <asp:SqlDataSource ID="SqlDataSourceUser" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerUserBylid" SelectCommandType="StoredProcedure" InsertCommand="insCustomerUser"
                                InsertCommandType="StoredProcedure" UpdateCommand="updCustomerUserMS" UpdateCommandType="StoredProcedure"
                                DeleteCommand="delCustomerUserDetail" DeleteCommandType="StoredProcedure" OnInserted="SqlDataSourceUser_Inserted"
                                OnUpdated="SqlDataSourceUser_Updated" OnDeleted="SqlDataSourceUser_Deleted" OnUpdating="SqlDataSourceUser_Updating"
                                OnDeleting="SqlDataSourceUser_Deleting">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="gvCustomerUser" Name="lid" PropertyName="SelectedValue" />
                                    <%--<asp:SessionParameter Name="lId" SessionField="UserIdCustomer" />--%>
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="lid" />
                                    <asp:SessionParameter Name="lUserId" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <%--<asp:SessionParameter Name="lId" SessionField="UserIdCustomer" />--%>
                                    <asp:Parameter Name="lId" />
                                    <asp:Parameter Name="EmpName" />
                                    <asp:Parameter Name="Email" />
                                    <asp:Parameter Name="Designation" Type="String" />
                                    <asp:ControlParameter ControlID="FormViewUser$rblupdLogin" Name="LoginActive" PropertyName="SelectedValue"
                                        Type="string" />
                                    <asp:Parameter Name="dtBirthDate" Type="DateTime" />
                                    <asp:Parameter Name="MobileNo" Type="String" />
                                    <asp:Parameter Name="FaxNo" Type="String" />
                                    <asp:Parameter Name="OfficeNo" Type="String" />
                                    <asp:Parameter Name="CityName" Type="String" />
                                    <asp:Parameter Name="CountryID" Type="String" />
                                    <asp:ControlParameter ControlID="FormViewUser$rdPasswordReset" Name="ResetCode" PropertyName="SelectedValue"
                                        Type="Boolean" />
                                    <asp:Parameter Name="ResetDays" Type="int32" DefaultValue="0" />
                                    <asp:SessionParameter Name="lUserId" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:SessionParameter Name="CustomerId" SessionField="TR_TransporterID" Type="Int32" />
                                    <asp:Parameter Name="EmpName" Type="String" />
                                    <asp:Parameter Name="Designation" Type="String" />
                                    <asp:ControlParameter ControlID="FormViewUser$rblLogin" Name="LoginActive" PropertyName="SelectedValue"
                                        Type="string" />
                                    <asp:Parameter Name="Email" Type="String" />
                                    <asp:Parameter Name="dtBirthDate" Type="DateTime" />
                                    <asp:Parameter Name="MobileNo" Type="String" />
                                    <asp:Parameter Name="FaxNo" Type="String" />
                                    <asp:Parameter Name="OfficeNo" Type="String" />
                                    <asp:Parameter Name="CityName" Type="String" />
                                    <asp:Parameter Name="CountryID" Type="String" />
                                    <asp:Parameter Name="Password" Type="String" />
                                    <asp:ControlParameter ControlID="FormViewUser$rdPasswordReset" Name="ResetCode" PropertyName="SelectedValue"
                                        Type="Boolean" />
                                    <asp:Parameter Name="ResetDays" Type="int32" DefaultValue="0" />
                                    <asp:SessionParameter Name="lUserId" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                                </InsertParameters>
                            </asp:SqlDataSource>
                        </div>
                        <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width:50%;">
                                    <fieldset style="min-height: 185px;">
                                        <legend>Contact Notification Setting</legend>
                                        
                                        <table>
                                            <tr>
                                                <td>Notification Type
                                                    <asp:RequiredFieldValidator ID="RFVNotification" runat="server" ControlToValidate="ddNotificationType"
                                                        InitialValue="0" ErrorMessage="*" ValidationGroup="NotifyRequired"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddNotificationType" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                           
                                                <td>Notification Mode
                                                    <asp:RequiredFieldValidator ID="RFVNotifiMode" runat="server" ControlToValidate="ddNotificationMode"
                                                        InitialValue="0" ErrorMessage="*" ValidationGroup="NotifyRequired"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddNotificationMode" runat="server" Width="120px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Button ID="btnAddNofication" runat="server" Text="Add" OnClick="btnAddNofication_Click"
                                                        ValidationGroup="NotifyRequired" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="m">
                                        </div>
                                        <asp:GridView ID="gvNotification" runat="server" DataKeyNames="lid" Width="30%" PageSize="40"
                                            AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceNotification"
                                            AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex +1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NotificationType" HeaderText="Notification Type" />
                                                <asp:BoundField DataField="NotificationMode" HeaderText="Notification Mode" />
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkNotifyRemove" runat="server" CommandName="Delete" Text="Remove"
                                                            OnClientClick="return confirm('Are you sure you want to remove this Notification?')" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        </fieldset>
                                </td>
                                <td>
                                    <fieldset style="min-height: 185px;">
                                        <legend>Contact Sub Branch/Plant</legend>
                                        <table id="tblLocation">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddUserPlant" DataSourceID="DataSourcePlant" runat="server"
                                                        EnableViewState="false" DataTextField="PlantDisplayName" DataValueField="lId" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0" Text="-Sub Branch/Plant Name-"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnAddUserPlant" Text="Add Contact Sub Branch/Plant" OnClick="btnAddUserPlant_Click"
                                                        runat="server" />
                                                </td>
                                               <%-- <td style="vertical-align: top;">
                                                    <asp:ListBox ID="listPlant" DataSourceID="DataSourceUserPlant" SelectionMode="Single"
                                                        DataTextField="PlantName" DataValueField="lId" runat="server" Height="50px"></asp:ListBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnRemoveUserPlant" Text="Remove Plant" OnClick="btnRemoveUserPlant_Click"
                                                        runat="server" />
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="grdPlant" runat="server" CssClass="table" AutoGenerateColumns="false" DataSourceID="DataSourceUserPlant" DataKeyNames="lid">
                                                        <Columns>
                                                             <asp:TemplateField HeaderText="Sl">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex +1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Division" DataField="Division"/>
                                                            <asp:BoundField  HeaderText="Plant" DataField="PlantName"/>
                                                            <asp:TemplateField HeaderText="Remove">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPlantRemove" runat="server" CommandName="Delete" Text="Remove"
                                                                        OnClientClick="return confirm('Are you sure you want to remove this plant?')" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <div id="Div2">
                                        <asp:SqlDataSource ID="DataSourceNotification" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                            SelectCommand="GetCustomerUserNotification" SelectCommandType="StoredProcedure"
                                            DeleteCommand="DelCustomerUserNotification" DeleteCommandType="StoredProcedure"
                                            OnDeleted="DataSourceNotification_Deleted">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="FormViewUser" Name="CustUserId" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                            <DeleteParameters>
                                                <asp:ControlParameter ControlID="gvNotification" Name="lId" PropertyName="SelectedValue" />
                                                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                                            </DeleteParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        </fieldset>
                        <div id="divUserPlantDataSource">
                            <asp:SqlDataSource ID="DataSourcePlant" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerPlant" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="CustomerId" SessionField="TR_TransporterID" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceUserPlant" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerUserPlant" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="FormViewUser" Name="CustomerUserId" PropertyName="SelectedValue" />
                                    <%--<asp:SessionParameter Name="CustomerUserId" SessionField="UserIdCustomer" />--%>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabNotes" runat="server" HeaderText="Contract / Notes">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <fieldset>
                                <legend>Transporter - Contract / Notes</legend>
                                <div>
                                    <div class="fleft">
                                        <asp:RadioButtonList ID="rdlNotType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Document" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Contract" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
<%--                                    <asp:TextBox ID="txtCustomer" Width="50%" runat="server" MaxLength="100" 
                                     Placeholder="Customer Name" OnTextChanged="txtCustomer_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:HiddenField ID="hdnCustId" runat="server" />

                                    <div id="divwidthCust">
                                    </div>
                                    <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                        CompletionListElementID="divwidthCust" ServicePath="../WebService/CustomerAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                        OnClientItemSelected="OnCustomerSelected"
                                        ContextKey="3629" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                                    </cc1:AutoCompleteExtender>--%>
                                    <div>
                                        <asp:Button ID="btnNotes" ValidationGroup="NotesRequired" Text="Add Note/Contract" runat="server"
                                            OnClick="btnNotesAdd_Click" />
                                    </div>
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <th style="color: #FFFFFF; background-color: #5A7FA5;" colspan="2">Notes/Contract</th>
                                    <th style="color: #FFFFFF; background-color: #5A7FA5">Start Date</th>
                                    <th style="color: #FFFFFF; background-color: #5A7FA5">Valid Till</th>
                                    <th style="color: #FFFFFF; background-color: #5A7FA5">Document</th>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RequiredFieldValidator ID="RVFNote" ControlToValidate="txtNotes" ValidationGroup="NotesRequired"
                                                Text="Required" ErrorMessage="Please Enter Note!" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3" MaxLength="8000"></asp:TextBox>
                                        </td>
                                        <td>
                                            <cc1:CalendarExtender ID="CalExtStartDate" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateStart" PopupPosition="BottomRight"
                                                TargetControlID="txtStartDate">
                                            </cc1:CalendarExtender>
                                            <asp:TextBox ID="txtStartDate" runat="server" Width="80px"></asp:TextBox>
                                            <asp:Image ID="imgDateStart" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                        </td>
                                        <td>
                                            <cc1:CalendarExtender ID="CalExtValidDate" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgValidDate" PopupPosition="BottomRight"
                                                TargetControlID="txtValidDate">
                                            </cc1:CalendarExtender>
                                            <asp:TextBox ID="txtValidDate" runat="server" Width="80px"></asp:TextBox>
                                            <asp:Image ID="imgValidDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuNotesDoc" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear">
                                </div>
                                <div>
                                    <asp:GridView ID="gvNotes" runat="server" DataKeyNames="lid" Width="99%" AllowPaging="True"
                                        PageSize="50" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceNotes"
                                        OnRowCommand="gvNotes_RowCommand" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex +1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="Notes" HeaderText="Notes" />--%>
                                            <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                                                <ItemTemplate>
                                                    <div style="word-wrap: break-word; width: 99%; white-space:normal;">
                                                        <asp:Label ID="lblNotes" runat="server" 
                                                           Text='<%# Eval("Notes")%>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="ValidTillDate" HeaderText="Valid Till" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:TemplateField HeaderText="Document">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNotesDownload" Text="Download" CommandName="Download" runat="server"
                                                        CommandArgument='<%#Eval("FilePath") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ContractCustomerName" HeaderText="Customer" SortExpression="ContractCustomerName" />
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                            <asp:BoundField DataField="dtDate" HeaderText="Activity Date" SortExpression="dtDate"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                            <%--<asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNotesDelete" Text="Remove" CommandName="Delete" runat="server"
                                                        OnClientClick="return confirm('Sure to Remove Customer Note?');"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </div>
                        <div id="Div4">
                            <asp:SqlDataSource ID="DataSourceNotes" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerNotes" SelectCommandType="StoredProcedure" DeleteCommand="delCustomerNotes"
                                DeleteCommandType="StoredProcedure" OnDeleted="DataSourceNotes_Deleted">
                                <SelectParameters>
                                    <asp:SessionParameter Name="CustId" SessionField="TR_TransporterID" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:ControlParameter ControlID="gvNotes" Name="lId" PropertyName="SelectedValue" />
                                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                                
                <cc1:TabPanel ID="TabKYC" runat="server" HeaderText="KYC">
                    <ContentTemplate>
                        <div style="overflow: auto; width: 100%;">
                            <fieldset>
                                <legend>KYC Details</legend>
                                <asp:GridView ID="gvKYCCopys" runat="server" DataKeyNames="lid" Width="99%" AllowPaging="True"
                                    PageSize="50" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceKYC"
                                    OnRowCommand="gvKYCCopys_RowCommand" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CatgName" HeaderText="Description" SortExpression="CatgName" />
                                        <asp:TemplateField HeaderText="Document">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkCopyDownload" Text="Download" CommandName="Download" runat="server"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="dtDate" HeaderText="Created Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                                <div>
                                    <asp:SqlDataSource ID="DataSourceKYC" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="KYC_GetAllKYCCopys" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="CustID" SessionField="TR_TransporterID" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </fieldset>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                
                 <%--Added new status tab for active,inactive and hold status--%>
                <cc1:TabPanel ID="TabStatus" runat="server" HeaderText="Status">
                <ContentTemplate>
                    <div style="overflow: auto; width: 100%;">
                        <fieldset>
                            <legend>Activity Status</legend>
                            <div class="m clear">
                                <asp:Button ID="btnSaveStatus" runat="server" ValidationGroup="RequiredStatus" OnClick="btnSaveStatus_Click" Text="Save" />
                                <asp:Button ID="btnBack" Text="Back" runat="server" CausesValidation="false" OnClick="btnBack_OnClick" TabIndex="6" Visible="false" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Current Status
                                        <asp:RequiredFieldValidator ID="RFVStatus" runat="server" ErrorMessage="Please Select Activity Status." Text="*" SetFocusOnError="true"
                                        ControlToValidate="ddlStatus" Display="Dynamic" InitialValue="0" ValidationGroup="RequiredStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="DropDownBox">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td>Remark
                                         <asp:RequiredFieldValidator ID="RFVRemark" runat="server" ErrorMessage="Please Enter Remark." Text="*" SetFocusOnError="true"
                                         ControlToValidate="txtRemark" Display="Dynamic" ValidationGroup="RequiredStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="TextBox" Width="300px" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </td>
                                </tr>--%>
                            </table>
                        </fieldset>
                         <fieldset>
                         <legend>Current Status Activity</legend>
                         <asp:GridView ID="gvStatusActivity" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false" PagerStyle-CssClass="pgr"
                             CssClass="table" Width="100%" DataSourceID="DataSourceStatusActivity" PageSize="20" OnRowDataBound="gvStatusActivity_RowDataBound">
                             <Columns>
                                 <asp:TemplateField HeaderText="Sl">
                                     <ItemTemplate>
                                         <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="dtDate" HeaderText="Date & Time" SortExpression="dtDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                 <asp:BoundField DataField="Status" HeaderText="Current Status" />
                              <%--   <asp:BoundField DataField="Remark" HeaderText="Remark" />--%>
                                 <asp:BoundField DataField="UserName" HeaderText="UserName" />
                             </Columns>
                         </asp:GridView>
                     </fieldset>
                      <asp:SqlDataSource ID="DataSourceStatusActivity" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                         SelectCommand="GetStatusActivity" SelectCommandType="StoredProcedure">
                         <SelectParameters>
                             <asp:SessionParameter Name="CustomerId" SessionField="TR_TransporterID" />
                         </SelectParameters>
                     </asp:SqlDataSource>
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>


            </cc1:TabContainer>
            <div id="DivDataSrouce222">
                <asp:SqlDataSource ID="DataSourceDivision" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCustomerDivision" SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="CustomerId" SessionField="TR_TransporterID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



<%@ Page Title="Add Lead" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddLead.aspx.cs"
    Inherits="CRM_AddLead" EnableEventValidation="false" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    
   
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>
    <script type="text/javascript">
        function OnCompanySelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCompanyId').value = results.CompanyId;
        }

        function OnSalesPersonSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnSalesPersonId.ClientID %>').value = results.Userid;
        }

        function OnKAMSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnKAMId.ClientID %>').value = results.Userid;
        }
        
    </script>

    <%-- ----------------------------------------------------------------------------------------------- --%>

    <%--<script type="text/javascript" language="javascript">
        //function preventBack() { window.history.forward(); }
        //setTimeout("preventBack()", 0);
        //window.onunload = function () { null };

       
        function back_block() {
            window.history.foward(1)
        }

    </script>

    <asp:UpdateProgress ID="updProgressTab"  AssociatedUpdatePanelID="upJobDetail" runat="server" >
      <ProgressTemplate>
            <div class="WaitDisplay">
                  <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
            </div>
      </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <%-- ----------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="upJobDetail" runat="server" >
        <ContentTemplate>
            
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnCompanyId" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                    <asp:ValidationSummary ID="vsService" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="vgService" CssClass="errorMsg" EnableViewState="false" />
                    <asp:HiddenField ID="hdnTurnover" runat="server" />
                    <asp:HiddenField ID="hdnCompanyType" runat="server" />
                    <asp:HiddenField ID="hdnEmployeeCount" runat="server" />
                    <asp:HiddenField ID="hdnEnquiryNo" runat="server" />
                </div>
                <div class="m clear">
                    <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                        TabIndex="39"  /><%--OnClientClick="javascript:return Validate()"--%>
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="40"
                        runat="server" />
                    <asp:Button ID="btnBack" Text="Back" OnClick="btnBack_Click" CausesValidation="false" TabIndex="41"
                        runat="server" />
                </div>
                <fieldset>
                    <legend>Company Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col width="10%" />
                            <col width="40%" />
                            <col width="10%" />
                            <col width="40%" />
                        </colgroup>
                        <tr>
                            <td>Company Name <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="txtCompanyName" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Company Name" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCompanyName" runat="server" TabIndex="1" Width="300px" AutoPostBack="true" OnTextChanged="txtCompanyName_TextChanged"></asp:TextBox>
                                <div id="divwidthCust" runat="server"></div>
                                <cc1:AutoCompleteExtender ID="CompanyExtender" runat="server" TargetControlID="txtCompanyName"
                                    CompletionListElementID="divwidthCust" ServicePath="~/WebService/LeadAutoCompany.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                    ContextKey="4567" UseContextKey="True" OnClientItemSelected="OnCompanySelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>Company Type <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvCompanyType" runat="server" ControlToValidate="ddlCompanyType" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Company Type" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompanyType" runat="server" DataSourceID="DataSourceCompanyType" DataTextField="sName"
                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="310px">
                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Business Sector <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvSector" runat="server" ControlToValidate="ddlBusinessSector" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Business Sector" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBusinessSector" runat="server" DataSourceID="DataSourceBusinessSector" DataTextField="sName"
                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="3" Width="310px">
                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Business Category <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvCatg" runat="server" ControlToValidate="ddlBusinessCatg" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Business Category" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBusinessCatg" runat="server" DataSourceID="DataSourceBusinessCatg" DataTextField="sName"
                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="4" Width="310px">
                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Turnover <%--<span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvTurnover" runat="server" ControlToValidate="txtTurnover" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Turnover" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTurnover" runat="server" TabIndex="5" Width="300px"></asp:TextBox>
                            </td>
                            <td>Employee Count</td>
                            <td>
                                <asp:TextBox ID="txtEmployeeCount" runat="server" TabIndex="6" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer Reference</td>
                            <td>
                                <asp:TextBox ID="txtCustRef" runat="server" TabIndex="7" Width="300px"></asp:TextBox>
                            </td>
                            <td>Years In Service</td>
                            <td>
                                <asp:TextBox ID="txtYearsInService" runat="server" TabIndex="8" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Website</td>
                            <td>
                                <asp:TextBox ID="txtWebsite" runat="server" TabIndex="9" Width="300px"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Contact Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col width="10%" />
                            <col width="40%" />
                            <col width="10%" />
                            <col width="40%" />
                        </colgroup>
                        <tr>
                            <td>Contact Name <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ControlToValidate="txtContactName" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Contact Name" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactName" runat="server" TabIndex="15" Width="300px"></asp:TextBox>
                            </td>
                            <td>Designation</td>
                            <td>
                                <asp:TextBox ID="txtDesignation" runat="server" TabIndex="16" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Role <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Role" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRole" runat="server" DataSourceID="DataSourceRole" DataTextField="sName"
                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="17" Width="310px">
                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Email <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Email" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    SetFocusOnError="true" Display="Dynamic" Text="*" ErrorMessage="Invalid Email" ForeColor="Red"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Required"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" TabIndex="18" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mobile No <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvMobileNo" runat="server" ControlToValidate="txtMobileNo" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Mobile No" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revMobileNo" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtMobileNo"
                                    ErrorMessage="Enter valid mobile no" ValidationExpression="^(\d{10}|\d{11})$"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobileNo" runat="server" TabIndex="19" Width="300px"></asp:TextBox>
                            </td>
                            <td>Contact No
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactNo" runat="server" TabIndex="20" Width="300px" MaxLength="25"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Office Location</td>
                            <td>
                                <asp:TextBox ID="txtOfficeLocation" runat="server" TabIndex="21" Width="300px" MaxLength="80"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Address <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvAddressLine1" runat="server" ControlToValidate="txtAddressLine1" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Address" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAddressLine1" runat="server" TabIndex="22" MaxLength="50" Width="980px" placeholder="eg :- Street name"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAddressLine2" runat="server" TabIndex="23" Rows="2" MaxLength="50" Width="980px" placeholder="eg :- Landmark, City"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAddressLine3" runat="server" TabIndex="24" Rows="2" MaxLength="50" Width="980px" placeholder="eg :- Zipcode, State, Country"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Description</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDescription" runat="server" Rows="2" TextMode="MultiLine" TabIndex="25" Width="980px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Lead Status</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col width="10%" />
                            <col width="40%" />
                            <col width="10%" />
                            <col width="40%" />
                        </colgroup>
                        <tr>
                            <td>Lead Source <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvLeadSource" runat="server" ControlToValidate="ddlSource" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Lead Source" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSource" runat="server" DataSourceID="DataSourceSource" DataTextField="sName"
                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="30" Width="310px" >
                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Source Description <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvSourceDesc" runat="server" ControlToValidate="txtSourceDescription" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Source Description" Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceDescription" runat="server" TabIndex="31" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>RFQ Received? <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfv_RFQReceived" runat="server" ControlToValidate="ddlRFQReceived" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select RFQ Received?" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRFQReceived" runat="server" TabIndex="32" Width="80" AutoPostBack="true" OnSelectedIndexChanged="ddlRFQReceived_SelectedIndexChanged1">
                                    <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>--%>
                    </table>
                </fieldset>
                <div id="dvRFQ" runat="server">
                    <fieldset id="lblRFQ" runat="server">
                    <legend>Enquiry/RFQ Received</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Payment Terms<span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvPaymentTerms" runat="server" ControlToValidate="txtPaymentTerms" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Payment Terms" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTerms" runat="server" TabIndex="41" Width="260px"></asp:TextBox>
                            </td>
                            <td>Branch <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBabajiBranch" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Branch" Text="*" ValidationGroup="Required" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="270px" TabIndex="44">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Sales Person Name <span style="color: red">*</span>
                                <asp:RequiredFieldValidator ID="rfvSalesPerson" runat="server" ControlToValidate="txtSalesPerson" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Sales Person Name." Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="SearchTextbox" Width="260px" placeholder=" Search" TabIndex="45"></asp:TextBox>
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
                                <asp:TextBox ID="txtKAM" runat="server" CssClass="SearchTextbox" Width="260px" placeholder="Search" TabIndex="46"></asp:TextBox>
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
                                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3" Width="85%" TabIndex="47"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Attach quote/other document</td>
                            <td colspan="3">
                                <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                                    <asp:FileUpload ID="fuDocument2" runat="server" ViewStateMode="Enabled" TabIndex="48" />
                                    <asp:Button ID="btnSaveDocument2" Text="Add" runat="server" OnClick="btnSaveDocument2_Click"  />
                                   
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
                                                    <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="49" CausesValidation="false"
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

                <fieldset>
                    <legend>Product</legend> <span style="color: red">Atleast one product compulsory *</span>
                    <asp:GridView ID="gvService" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="90%" TabIndex="21" class="table"
                        OnRowCreated="gvService_RowCreated" Style="border-collapse: initial; border: 1px solid #5D7B9D;">
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Product" ItemStyle-Width="45%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlService" runat="server" DataSourceID="DataSourceService" DataTextField="sName" DataValueField="ServicesId"
                                        AppendDataBoundItems="true" TabIndex="32" Width="95%">
                                        <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                    </asp:DropDownList><span style="color: red">*</span>
                                    <asp:RequiredFieldValidator ID="rfvService" runat="server" ControlToValidate="ddlService" Display="Dynamic" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please select service" ValidationGroup="vgService" InitialValue="0"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Location" ItemStyle-Width="45%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlLocation" runat="server" DataSourceID="DataSourceServiceLocation" DataTextField="BranchName" DataValueField="lid"
                                        AppendDataBoundItems="true" TabIndex="33" Width="95%">
                                        <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                    </asp:DropDownList><span style="color: red">*</span>
                                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" Display="Dynamic" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please select service location" ValidationGroup="vgService" InitialValue="0"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Volume Expected" ItemStyle-Width="45%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtVolumeExp" runat="server" TabIndex="34"></asp:TextBox><span style="color: red">*</span>
                                    <asp:RequiredFieldValidator ID="rfvVolumeExp" runat="server" ControlToValidate="txtVolumeExp" Display="Dynamic" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please enter volume expected" ValidationGroup="vgService"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requirement" ItemStyle-Width="45%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRequirement" runat="server" TextMode="MultiLine" TabIndex="35" Rows="3" Width="200px"></asp:TextBox><span style="color: red">*</span>
                                    <asp:RequiredFieldValidator ID="rfvRequirement" ControlToValidate="txtRequirement" ValidationGroup="vgService"
                                        Text="*" ErrorMessage="Please enter requirement" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDeleteRow" runat="server" CausesValidation="false" TabIndex="59" OnClick="lnkDeleteRow_Click" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <asp:Button ID="btnAddTransportCharges" ValidationGroup="vgService" TabIndex="60" runat="server" Text="Add Service" OnClick="btnAddTransportCharges_Click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                
               <%-- <fieldset >
                    <legend>Add Service</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col width="10%" />
                            <col width="90%" />
                           
                        </colgroup>
                        <tr>
                            <td>Service  <span style="color: red">*</span>                                     
                                        <asp:RequiredFieldValidator ID="rfvService" runat="server" ControlToValidate="ddlService" Display="Dynamic" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please select service" ValidationGroup="Required" ForeColor="Red"  InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlService" runat="server" DataSourceID="DataSourceService" DataTextField="sName" DataValueField="ServicesId"
                                    AppendDataBoundItems="true" TabIndex="91" Width="50%">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Service Location   <span style="color: red">*</span>                                    
                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" Display="Dynamic" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please select service location" ValidationGroup="Required" ForeColor="Red"  InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLocation" runat="server" DataSourceID="DataSourceServiceLocation" DataTextField="BranchName" DataValueField="lid"
                                    AppendDataBoundItems="true" TabIndex="92" Width="50%">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>Volume Expected</td>
                            <td>
                                <asp:TextBox ID="txtVolumeExp" runat="server" TabIndex="94"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Requirement <span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="rfvRequirement" ControlToValidate="txtRequirement" ValidationGroup="Required"
                                            Text="*" ErrorMessage="Please Enter Requirement" Display="Dynamic" ForeColor="Red"  runat="server"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRequirement" runat="server" TextMode="MultiLine" TabIndex="95" Rows="3" Width="350px"></asp:TextBox>
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
                            <asp:TemplateField HeaderText="Volume Expected" ItemStyle-Width="45%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtVolumeExp" runat="server" TabIndex="24"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvVolumeExp" runat="server" ControlToValidate="txtVolumeExp" Display="Dynamic" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please enter volume expected" ValidationGroup="vgService"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requirement" ItemStyle-Width="45%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRequirement" runat="server" TextMode="MultiLine" TabIndex="25" Rows="3" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRequirement" ControlToValidate="txtRequirement" ValidationGroup="vgService"
                                        Text="*" ErrorMessage="Please enter requirement" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
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
                </fieldset>--%>
            </div>
            <div>
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
                <asp:SqlDataSource ID="DataSourceServices" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetServices" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="LeadId" SessionField="LeadId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceServiceLocation" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetAllBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


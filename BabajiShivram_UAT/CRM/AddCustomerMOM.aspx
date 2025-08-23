<%@ Page Title="Add Customer MOM" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AddCustomerMOM.aspx.cs" Inherits="CRM_AddCustomerMOM" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVPager" %>
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
        function OnUserSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnAttendeeUserId.ClientID %>').value = results.Userid;
        }
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnCompanyId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAttendeeUserId" runat="server" Value="0" />
                <asp:ValidationSummary ID="vsMOM" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgMOM" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear">
                <asp:Button ID="btnSaveMOM" runat="server" ValidationGroup="vgMOM" Text="Save" OnClick="btnSaveMOM_Click" TabIndex="50" />
                <asp:Button ID="btnCancelMOM" runat="server" Text="Cancel" OnClick="btnCancelMOM_Click" TabIndex="51" />
                <asp:Button ID="btnGoBack" runat="server" Text="Go Back" OnClick="btnGoBack_Click" TabIndex="51" />
            </div>
            <br />
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
                        <td>Customer
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddlCustomer" SetFocusOnError="true" Display="Dynamic"
                                InitialValue="0" ErrorMessage="Customer is required" Text="*" ValidationGroup="vgMOM"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlCustomer" runat="server" DataSourceID="DataSourceBabajiCustomers" DataTextField="CustName"
                                DataValueField="lid" CssClass="form-control dropdown" AppendDataBoundItems="true" Width="370px" TabIndex="39">
                                <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Meeting Title
                            <asp:RequiredFieldValidator ID="rfvMeetingTitle" runat="server" ControlToValidate="txtMeetingTitle" SetFocusOnError="true"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Meeting Title is required" Text="*" ValidationGroup="vgMOM"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMeetingTitle" runat="server" CssClass="form-control" TabIndex="40" Width="360px"></asp:TextBox>
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
                <asp:SqlDataSource ID="DataSourceBabajiCustomers" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCustomerMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
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
                <asp:GridView ID="gvAgenda" runat="server" Width="90%" AutoGenerateColumns="false" ShowFooter="true" CssClass="table table-bordered" 
                   > 
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
                                <asp:TextBox ID="txtDescription" runat="server" TabIndex="52" Text='<%#Eval("Description") %>'
                                     CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
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

            <%-- POPUP --> MOM Draft Mail--%>
            <AjaxToolkit:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
                DropShadow="False" PopupControlID="pnlMOM" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground">
            </AjaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMOM" runat="server" CssClass="ModalPopupPanel">
                <div style="background-color: #8ab933">
                    <div>
                        <span style="font-weight: 600; font-size: 18px; font-family: Calibri; color: white; padding-left: 10px">MINUTES OF MEETINGS MAIL
                        </span>
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/Close.gif" ImageAlign="Right" runat="server" OnClick="imgClose_Click" />
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
                            <asp:TextBox ID="txtMailCC" runat="server" Width="400px"></asp:TextBox>
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
            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
            <%-- POPUP --> MOM Draft Mail--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


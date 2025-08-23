<%@ Page Title="Task Reminder" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TaskReminder.aspx.cs"
    Inherits="Service_TaskReminder" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }

        .AdjustUserCell {
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        function OnUserSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnUserId.ClientID%>').value = results.Userid;
        }

        function OnUserSelected2(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnUserId2.ClientID%>').value = results.Userid;
        }

        function ValidateCheckList(source, args) {
            var chkListMode = document.getElementById('<%= chkRemindMode.ClientID %>');
            var chkListinputs = chkListMode.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanelDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:ValidationSummary ID="Valsummary" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="validateReminder" />
                <asp:ValidationSummary ID="ValSummary2" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="validateReminder2" />
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <asp:HiddenField ID="hdnEnquiryRefNo" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnCustomerId" runat="server"></asp:HiddenField>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Add Reminder</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnReminder" runat="server" Text="Add Reminder" ValidationGroup="validateReminder"
                                OnClick="btnAddReminder_Click" />
                            &nbsp; 
                             <asp:Button ID="btnCancelReminder" runat="server" Text="Cancel" CausesValidation="false"
                                 OnClick="btnCancelReminder_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>Type
                            <asp:CustomValidator runat="server" ID="CValRemindMode" ClientValidationFunction="ValidateCheckList" Display="Dynamic"
                                Text="*" SetFocusOnError="true" ErrorMessage="Please Select Atleast One Type" ValidationGroup="validateReminder"></asp:CustomValidator>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chkRemindMode" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                <asp:ListItem Text="Email" Value="1" Selected="True"></asp:ListItem>
                                <%--   <asp:ListItem Text="SMS" Value="2" Enabled="false"></asp:ListItem>--%>
                            </asp:CheckBoxList>
                        </td>
                        <td>Category
                             <asp:RequiredFieldValidator ID="rfvCatg" runat="server" ControlToValidate="ddlCategory" Display="Dynamic"
                                 InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Category." ValidationGroup="validateReminder"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server" AppendDataBoundItems="true" DataSourceID="CatgDataSource"
                                DataTextField="sName" DataValueField="lid" ToolTip="Select Category." >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Reminder Date
                            <AjaxToolkit:CalendarExtender ID="calRemindDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRmdate" PopupPosition="BottomRight"
                                TargetControlID="txtRemindDate">
                            </AjaxToolkit:CalendarExtender>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtRemindDate" TargetControlID="txtRemindDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValRemindDate" ControlExtender="MskExtRemindDate" ControlToValidate="txtRemindDate" IsValidEmpty="false"
                                InvalidValueMessage="Reminder Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Reminder Date." EmptyValueBlurredText="*" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/08/2017" MaximumValue="31/12/2040"
                                runat="Server" ValidationGroup="validateReminder"></AjaxToolkit:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemindDate" Width="100px" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgRmdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                        <td>Reminder Note
                            <asp:RequiredFieldValidator ID="rfvnotes" runat="server" ControlToValidate="txtRemindNote" Display="Dynamic"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Reminder Note." ValidationGroup="validateReminder"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemindNote" runat="server" TextMode="MultiLine" MaxLength="800"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Repeat Month
                        </td>
                        <td>
                            <asp:TextBox ID="txtRepeatMonth" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revCycleMonth" runat="server" ControlToValidate="txtRepeatMonth" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Enter Valid Month (e.g.: 1 or 2)" ValidationExpression="^[0-9]\d{0,4}$"></asp:RegularExpressionValidator>
                        </td>
                        <td>Share With 
                        </td>
                        <td>
                            <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
                            <asp:TextBox ID="txtUser" runat="server" MaxLength="100"
                                placeholder="User Name" Width="30%"></asp:TextBox>
                            &nbsp;
                            <asp:Button ID="btnAddProjectUsers" runat="server" OnClick="btnAddProjectUsers_OnClick" CausesValidation="false" Text=" + " />
                            <div id="divwidthUser" runat="server"></div>
                            <AjaxToolkit:AutoCompleteExtender ID="UserExtender" runat="server" BehaviorID="divwidthUser"
                                CompletionListCssClass="AutoExtender" CompletionListElementID="divwidthUser"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"
                                ContextKey="4317" FirstRowSelected="true" MinimumPrefixLength="2" OnClientItemSelected="OnUserSelected"
                                ServiceMethod="GetUserCompletionList" ServicePath="~/WebService/UserAutoComplete.asmx"
                                TargetControlID="txtUser" UseContextKey="True">
                            </AjaxToolkit:AutoCompleteExtender>
                            <br />
                            <br />
                            <asp:GridView ID="gvUserList" ShowHeader="true" Width="60%" runat="server" CssClass="table" ShowFooter="false" AutoGenerateColumns="False" Style="border-collapse: initial; border: 1px solid #5D7B9D">
                                <Columns>
                                    <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:TemplateField HeaderText="Shared User List">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UserId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDeleteRow" runat="server" CausesValidation="false" OnClick="lnkDeleteRow_Click" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

            </fieldset>

            <fieldset>
                <legend>Reminder Details</legend>
                <div>
                    <div class="clear">
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style=" padding-top: 3px;">
                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                    <asp:GridView ID="gvReminder" runat="server" AutoGenerateColumns="False" CssClass="table" Width="100%" Style="white-space: normal"
                        PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" DataKeyNames="lId"
                        DataSourceID="ReminderDetailSqlDataSource" PagerSettings-Position="TopAndBottom" OnRowCommand="gvReminder_RowCommand"
                        OnRowDataBound="gvReminder_RowDataBound" OnRowUpdating="gvReminder_RowUpdating" OnPageIndexChanging="gvReminder_PageIndexChanging"
                        OnRowEditing="gvReminder_RowEditing" OnRowCancelingEdit="gvReminder_RowCancelingEdit" OnPreRender="gvReminder_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" Font-Underline="true" ValidationGroup="validateReminder2"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label ID="lblCatgName" runat="server" Text='<%#Eval("CatgName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCategory" runat="server" AppendDataBoundItems="true" DataSourceID="CatgDataSource"
                                        DataTextField="sName" DataValueField="lid" ToolTip="Select Category." SelectedValue='<%#Eval("CategoryId") %>'>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCatg2" runat="server" ControlToValidate="ddlCategory" Display="Dynamic" ValidationGroup="validateReminder2"
                                        InitialValue="0" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CatgName" HeaderText="Category" Visible="false" ReadOnly="true" />
                            <asp:BoundField DataField="ReminderType" HeaderText="Type" ReadOnly="true" />
                            <asp:BoundField DataField="RemindDate" HeaderText="Reminder Date" Visible="false" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Notes" ItemStyle-Width="400px">
                                <ItemTemplate>
                                    <asp:Label ID="lblNotes" runat="server" Text='<%#Eval("ReminderNotes") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNotes" runat="server" Text='<%#Eval("ReminderNotes") %>' 
                                        TextMode="MultiLine" Rows="3" MaxLength="800"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemStyle Width="400px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reminder Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemindDate" runat="server" Text='<%#Eval("RemindDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemindDate" runat="server" Text='<%#Eval("RemindDate","{0:dd/MM/yyyy}") %>' Width="60px"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="calRemindDate" runat="server" Enabled="True" EnableViewState="False"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight"
                                        TargetControlID="txtRemindDate">
                                    </AjaxToolkit:CalendarExtender>
                                    <AjaxToolkit:MaskedEditExtender ID="MskExtRemindDate" TargetControlID="txtRemindDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="MskValRemindDate" ControlExtender="MskExtRemindDate" ControlToValidate="txtRemindDate" IsValidEmpty="false"
                                        InvalidValueMessage="Reminder Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                        EmptyValueMessage="Please Enter Reminder Date." EmptyValueBlurredText="*" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/08/2017" MaximumValue="31/12/2040"
                                        runat="Server" ValidationGroup="validateReminder2"></AjaxToolkit:MaskedEditValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Repeat Month">
                                <ItemTemplate>
                                    <asp:Label ID="lblRepeatMonth" runat="server" Text='<%#Eval("RepeatMonth") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRepeatMonth" runat="server" Text='<%#Eval("RepeatMonth") %>' Width="60px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revCycleMonth2" runat="server" ControlToValidate="txtRepeatMonth" SetFocusOnError="true"
                                        Display="Dynamic" ErrorMessage="Enter Valid Month (e.g.: 1 or 2)" ValidationExpression="^[0-9]\d{0,4}$"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Shared Users">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblUsersList" runat="server" Text="Show" CommandName="UserList"
                                        CommandArgument='<%#Eval("lId") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblRemindStatus" runat="server" Text="Stop" CommandName="RemindStatus"
                                        CommandArgument='<%#Eval("lId") %>' OnClientClick="return confirm('Are you sure to stop reminder?');"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnlRemoveReminder" runat="server" Text="Remove" CommandName="RemoveRemind"
                                        CommandArgument='<%#Eval("lId") %>' OnClientClick="return confirm('Are you sure to remove reminder?');"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SharedUserCommaSep" HeaderText="Shared Users List" Visible="false" ReadOnly="true" />
                        </Columns>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle CssClass="pgr" />
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </fieldset>

            <!--Document for Doc Upload-->
            <div id="divApproval">
                <AjaxToolkit:ModalPopupExtender ID="mpeUserList" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlUserList" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground" DropShadow="true">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlUserList" runat="server" CssClass="ModalPopupPanel" Width="400px" Style="border-radius: 10px">
                    <div class="header">
                        <div class="fleft">
                            Shared With
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnApproval" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnApproval_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                        <asp:HiddenField ID="hdnReminderId" runat="server" />
                        <div>
                            <asp:Label ID="lblErrorPopup" runat="server" EnableViewState="false"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="Please Enter UserName."
                                Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgUserPopup" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="background-color: beige; border-radius: 5px; border-collapse: initial">
                                <tr>
                                    <td>Share With 
                                        &nbsp;
                                        <asp:HiddenField ID="hdnUserId2" runat="server" Value="0" />
                                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="100" placeholder="User Name" Width="30%"></asp:TextBox>
                                        &nbsp;
                                        <asp:Button ID="btnAddProjectUsers2" runat="server" OnClick="btnAddProjectUsers2_OnClick" ValidationGroup="vgUserPopup"
                                            CssClass="buttonSmall" Text="Add User" />
                                        <div id="divwidthUser2" runat="server"></div>
                                        <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="divwidthUser2"
                                            CompletionListCssClass="AutoExtender" CompletionListElementID="divwidthUser2"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"
                                            ContextKey="4317" FirstRowSelected="true" MinimumPrefixLength="2" OnClientItemSelected="OnUserSelected2"
                                            ServiceMethod="GetUserCompletionList" ServicePath="~/WebService/UserAutoComplete.asmx"
                                            TargetControlID="txtUserName" UseContextKey="True">
                                        </AjaxToolkit:AutoCompleteExtender>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center; padding-top: 10px">
                                <asp:GridView ID="gvSharedWithUsersList" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true"
                                    CssClass="table" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                    DataSourceID="GridViewSqlDataSource" CellPadding="4" PageSize="20" PagerSettings-Position="Top" Width="100%"
                                    OnRowCommand="gvSharedWithUsersList_RowCommand" OnPreRender="gvSharedWithUsersList_PreRender">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl" ItemStyle-CssClass="AdjustUserCell">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" ItemStyle-CssClass="AdjustUserCell" />
                                        <asp:TemplateField HeaderText="Remove" ItemStyle-CssClass="AdjustUserCell">
                                            <ItemTemplate>
                                                <asp:LinkButton Text="Remove" CommandName="Remove" OnClientClick="return confirm('Are you sure to delete this user?');"
                                                    runat="server" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" />
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="m clear">
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="LinkButton1" runat="server"></asp:HiddenField>
            </div>
            <!--Document for Doc Upload - END -->

            <div>
                <asp:SqlDataSource ID="ReminderDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TS_GetTaskReminder" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TS_GetReminderUser" SelectCommandType="StoredProcedure" DeleteCommand="TS_delReminderUser" DeleteCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnReminderId" PropertyName="Value" Name="ReminderId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="CatgDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TS_GetReminderCategoriesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


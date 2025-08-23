<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerPreAlert.aspx.cs" 
    Inherits="FreightOperation_CustomerPreAlert" Title="Customer PreAlert Details" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValSummaryFreightDetail" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
    </div>
    <div class="clear"></div>
    <fieldset><legend>Customer PreAlert Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnSubmit_Click" ValidationGroup="Required" />
            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False"
                Text="Cancel" TabIndex="20" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
            <tr>
                <td>
                    Job No
                </td>
                <td>
                    <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                </td>
                <td>
                    Booking Month
                </td>
                <td>
                    <asp:Label ID="lblBookingMonth" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Shipped on Board
                    <AjaxToolkit:CalendarExtender ID="calShipDate" runat="server" EnableViewState="False" FirstDayOfWeek="Sunday" 
                        Format="dd/MM/yyyy" PopupButtonID="imgShipDate" PopupPosition="BottomRight" TargetControlID="txtShippedDate">
                    </AjaxToolkit:CalendarExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtShippedDate" runat="server" Width="100px"></asp:TextBox>
                    <asp:Image ID="imgShipDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                    <AjaxToolkit:MaskedEditExtender ID="MskExtShipDate" TargetControlID="txtShippedDate" Mask="99/99/9999" MessageValidatorTip="true" 
                        MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                    <AjaxToolkit:MaskedEditValidator ID="MskValShipDate" ControlExtender="MskExtShipDate" ControlToValidate="txtShippedDate" IsValidEmpty="false" 
                        InvalidValueMessage="Shipped on Board Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                        EmptyValueMessage="Please Enter Shipped on Board Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" 
                        MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025" Runat="Server" ValidationGroup="Required">
                    </AjaxToolkit:MaskedEditValidator>
                </td>
                <td>
                    PreAlert Date
                    <AjaxToolkit:CalendarExtender ID="calAlertDate" runat="server" EnableViewState="False" FirstDayOfWeek="Sunday" 
                        Format="dd/MM/yyyy" PopupButtonID="imgAlertDate" PopupPosition="BottomRight" TargetControlID="txtPreAlertDate">
                    </AjaxToolkit:CalendarExtender>
                    <AjaxToolkit:MaskedEditExtender ID="MskExtAlertDate" TargetControlID="txtPreAlertDate" Mask="99/99/9999" MessageValidatorTip="true" 
                        MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                    <AjaxToolkit:MaskedEditValidator ID="MskValAlertDate" ControlExtender="MskExtAlertDate" ControlToValidate="txtPreAlertDate" IsValidEmpty="false" 
                        InvalidValueMessage="PreAlert Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                        EmptyValueMessage="Please Enter PreAlert Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" 
                        MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025" 
                        Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtPreAlertDate" runat="server" Width="100px"></asp:TextBox>
                    <asp:Image ID="imgAlertDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                </td>
            </tr>
            <tr>
                <td>
                    Customer Email
                    <asp:RequiredFieldValidator ID="rfvCustomerEmail" runat="server" InitialValue="" ValidationGroup="Required"
                        ControlToValidate="txtCustEmail" ErrorMessage="Please Enter Customer Email" Text="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="REVEmail" runat="server" ControlToValidate="txtCustEmail" Text="Invalid Email" Display="Dynamic" 
                        ErrorMessage="Please Enter Valid Email. Enter Comma-Separated Multiple Email." ValidationGroup="Required" SetFocusOnError="true"
                        ValidationExpression="^[\W]*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4}[\W]*,{1}[\W]*)*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4})[\W]*$"></asp:RegularExpressionValidator>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCustEmail" TextMode="MultiLine" runat="server" Height="50px" content="email" Width="95%"></asp:TextBox>
			<asp:HiddenField ID="hdnBranchEmail" runat="server" />
                </td>
                <td>
                    <asp:LinkButton ID="lnkPreAlertEmailDraft" runat="server" Text="View & Send Customer Email" OnClick="lnkPreAlertEmailDraft_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <!--Customer Email Draft Start -->
        <div id="divPreAlertEmail">
        <AjaxToolkit:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
            DropShadow="False" PopupControlID="Panel2Email" TargetControlID="lnkDummy">
        </AjaxToolkit:ModalPopupExtender>
                
        <asp:Panel ID="Panel2Email" runat="server" CssClass="ModalPopupPanel">
            <div class="header">
                <div class="fleft">
                    Customer PreAlert Email Draft
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgEmailClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnEMailCancel_Click" ToolTip="Close"  />
                </div>
            </div>
            <div class="m"></div>    
            <div id="DivABC" runat="server" style="max-height: 500px; max-width:780px; overflow: auto;">
             <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" ValidationGroup="mailRequired"
		        OnClientClick="if (!Page_ClientValidate('mailRequired')){ return false; } this.disabled = true; this.value = 'Email Sending...';" UseSubmitBehavior="false"/><br />
            <div class="m">
                <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                Email To:&nbsp;<asp:Label ID="lblCustomerEmail" runat="server"></asp:Label><br />
                Email CC&nbsp;<asp:TextBox ID="txtMailCC" runat="server" Width="85%"></asp:TextBox>
                Subject&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSubject" runat="server" Width="85%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true"
                 Text="*" ErrorMessage="Subject Required" ValidationGroup="mailRequired"></asp:RequiredFieldValidator>
                <hr style="border-top: 1px solid #8c8b8b" />        
            </div>
            <div id="divPreviewEmail" runat="server" style="margin-left:10px;">
                    
            </div>
            <fieldset><legend>Document Attachment</legend>
                <asp:GridView ID="gvFreightAttach" runat="server" AutoGenerateColumns="False" Width="100%"  
                    DataKeyNames="DocId" DataSourceID="FreightAttachSqlDataSource" CssClass="table"
                    CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAttach" runat="server" />
                                <asp:HiddenField ID="hdnDocPath" runat="server" Value='<%#Eval("DocPath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                        <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                        <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                </asp:GridView>
            </fieldset>        
            </div>
        </asp:Panel>
        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
        <!--Customer Email Draft End -->
    </div>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="FreightAttachSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div> 
</asp:Content>

<%@ Page Title="Export Operation" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpOperation.aspx.cs"
    Inherits="FreightExport_ExpOperation" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryFreightDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Operation Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" OnClick="btnSubmit_Click"/>
                    
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click" />
                   
                    <asp:HiddenField ID="hdnModeId" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td style="width:15%">Job No.
                        </td>
                        <td style="width:35%">
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                        <td style="width:15%">
                            Enquiry No
                        </td>
                        <td style="width:35%">
                             <asp:Label ID="lblEnquiryNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Shipper
                        </td>
                        <td>
                            <asp:Label ID="lblShipper" runat="server"></asp:Label>
                        </td>
                        <td>Shipper Address</td>
                        <td>
                            <asp:Label ID="lblShipperAddr" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Branch</td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server"></asp:Label>
                        </td>
                        <td>Country</td>
                        <td>
                            <asp:Label ID="lblCountry" runat="server"></asp:Label>
                        </td>
                     
                    </tr>
                    <tr>
                        <td>
                            Consignee
                        </td>
                        <td>
                            <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                        </td>
                        <td>
                            Consignee Address
                        </td>
                        <td>
                            <asp:Label ID="lblConsignAddr" runat="server" ></asp:Label>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            Type Of Export
                        </td>
                        <td>
                            <asp:Label ID="lblTypeofExport" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Port Of Loading</td>
                        <td>
                            <asp:Label ID="lblPortLoading" runat="server"></asp:Label>
                        </td>
                        <td>Port Of Discharge</td>
                        <td>
                            <asp:Label ID="lblPortDischarge" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Customer
                        </td>
                        <td>
                            <asp:Label id="lblCustomer" runat="server"></asp:Label>
                        </td>
                        <td>
                            Terms
                        </td>
                        <td>
                            <asp:Label ID="lblTerms" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Container20"
                        </td>
                        <td>
                            <asp:Label ID="lblContainer20" runat="server"></asp:Label>
                        </td>
                        <td>
                            Container40"
                        </td>
                        <td>
                            <asp:Label ID="lblContainer40" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            LCL (CBM)
                        </td>
                        <td>
                            <asp:Label ID="lblLCLCBL" runat="server"></asp:Label>
                        </td>
                        <td>LCL/FCL</td>
                        <td>
                            <asp:Label ID="lblLCLFCL" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>No.of Packages</td>
                        <td>
                            <asp:Label ID="lblNoPackages" runat="server"></asp:Label>
                        </td>
                        <td>Type of Packing</td>
                        <td>
                            <asp:Label ID="lblTypePacking" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Gross Weight(kgs)
                        </td>
                        <td>
                            <asp:Label ID="lblGrossWt" runat="server"></asp:Label>
                        </td>
                        <td>Chargeable Weight(kgs)</td>
                        <td>
                            <asp:Label ID="lblChargeable" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SB NO.
                        </td>
                        <td>
                            <asp:TextBox ID="txtSBNo" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSBNO" runat="server" ControlToValidate="txtSBNo" ValidationGroup="Required" ErrorMessage="Enter the SB NO." >*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            SB Date
                            
                             <AjaxToolkit:CalendarExtender ID="calChequeDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSBDate" PopupPosition="BottomRight"
                                TargetControlID="txtSBDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSBDate" runat="server"></asp:TextBox>
                            
                             <asp:Image ID="imgSBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtSBDate" TargetControlID="txtSBDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValSBDate" ControlExtender="MskExtSBDate" ControlToValidate="txtSBDate" IsValidEmpty="true" 
                                InvalidValueMessage="SB Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                            <asp:RequiredFieldValidator ID="rfvSBDate" runat="server" ControlToValidate="txtSBDate" ValidationGroup="Required" ErrorMessage="Enter the SB Date" >*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>                            
                            <asp:Label ID="lblContainerPick" runat="server" Text="Container Pickup Date"></asp:Label>
                         <%--   <asp:Label ID="lblCargoPick" runat="server" Text="Cargo Pickup Date"></asp:Label>--%>
                                 
                            <AjaxToolkit:CalendarExtender ID="calContPickDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContPickDate" PopupPosition="BottomRight"
                                TargetControlID="txtContPickDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtContPickDate" runat="server" ></asp:TextBox>
                            
                             <asp:Image ID="imgContPickDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskContPickDate" TargetControlID="txtContPickDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValContPickDate" ControlExtender="MskContPickDate" ControlToValidate="txtContPickDate" IsValidEmpty="true" 
                                InvalidValueMessage="Container Pickup Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                             <asp:RequiredFieldValidator ID="rfvContainerDate" runat="server" ControlToValidate="txtContPickDate" ValidationGroup="Required" ErrorMessage="Enter the Container Pickup date" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            
                            <asp:Label ID="lblCustomDate" runat="server" Text="Customs Permission Date"></asp:Label>
                            
                             <AjaxToolkit:CalendarExtender ID="calCustPerDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCustPermDate" PopupPosition="BottomRight"
                                TargetControlID="txtCustomPermiDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomPermiDate" runat="server" OnTextChanged="txtCustomPermiDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            
                             <asp:Image ID="imgCustPermDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskCustPerDate" TargetControlID="txtCustomPermiDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValCustPerDate" ControlExtender="MskCustPerDate" ControlToValidate="txtCustomPermiDate" IsValidEmpty="true" 
                                InvalidValueMessage="Customs Permission Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                             <asp:RequiredFieldValidator ID="rfvCustomDate" runat="server" ControlToValidate="txtCustomPermiDate" ValidationGroup="Required" ErrorMessage="Enter Custom Permission date" >*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trCarting" runat="server">
                        <td>
                            Carting Date
                        </td>
                        <td>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCartingDate" PopupPosition="BottomRight"
                                TargetControlID="txtCartingDate">
                            </AjaxToolkit:CalendarExtender>

                            <asp:TextBox ID="txtCartingDate" runat="server" ></asp:TextBox>
                           
                             <asp:Image ID="imgCartingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskCartingDate" TargetControlID="txtCartingDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" ControlExtender="MskCartingDate" ControlToValidate="txtCartingDate" IsValidEmpty="true" 
                                InvalidValueMessage="Customs Permission Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCartingDate" ValidationGroup="Required" ErrorMessage="Enter Stuffing date" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr id="trStuffing" runat="server">
                        <td>
                            Stuffing Date
                            
                             <AjaxToolkit:CalendarExtender ID="calStuffingDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgStuffingDate" PopupPosition="BottomRight"
                                TargetControlID="txtStuffingDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStuffingDate" runat="server" OnTextChanged="txtStuffingDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                           
                             <asp:Image ID="imgStuffingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskStuffingDate" TargetControlID="txtStuffingDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValStuffingDate" ControlExtender="MskStuffingDate" ControlToValidate="txtStuffingDate" IsValidEmpty="true" 
                                InvalidValueMessage="Customs Permission Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                              <asp:RequiredFieldValidator ID="rfvStuffingDate" runat="server" ControlToValidate="txtStuffingDate" ValidationGroup="Required" ErrorMessage="Enter Stuffing date" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            CLP Date
                            
                       <AjaxToolkit:CalendarExtender ID="calCLPDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCLPDate" PopupPosition="BottomRight"
                                TargetControlID="txtCLPDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCLPDate" runat="server" OnTextChanged="txtCLPDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            
                    <asp:Image ID="imgCLPDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskCLPDate" TargetControlID="txtCLPDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValCLPDate" ControlExtender="MskCLPDate" ControlToValidate="txtCLPDate" IsValidEmpty="true" 
                                InvalidValueMessage="Customs Permission Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>

                             <asp:RequiredFieldValidator ID="rfvCLPDate" runat="server" ControlToValidate="txtCLPDate" ValidationGroup="Required" ErrorMessage="Enter CLP date">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr1" runat="server">
                        <td>
                            Shipping line Booking No.
                        </td>
                        <td>
                            <asp:TextBox ID="txtShipBookingNo" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvShipBookingNo" runat="server" ControlToValidate="txtShipBookingNo" ValidationGroup="Required" ErrorMessage="Enter the Shipping line Booking No" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Vessel
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvVessel" runat="server" ControlToValidate="txtVessel" ValidationGroup="Required" ErrorMessage="Enter the Vessel" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        </tr>
                    <tr id="tr2" runat="server">
                        <td>
                            Place Of Delivery
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlaceOfDelivery" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPlaceOfDelivery" runat="server" ControlToValidate="txtPlaceOfDelivery" ValidationGroup="Required" ErrorMessage="Enter the Place Of Delivery" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            CHA Job No
                        </td>
                        <td>
                            <asp:TextBox ID="txtCHAJobNo" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCHAJobNo" runat="server" ControlToValidate="txtCHAJobNo" ValidationGroup="Required" ErrorMessage="Enter the CHA Job No" >*
                            </asp:RequiredFieldValidator>
                        </td>
                        </tr>
                       <tr id="trAirMode" runat="server">
                        <td>
                            ASI By
                        </td>
                        <td>
                            <asp:TextBox ID="txtASIBy" runat="server" TextMode="MultiLine"> </asp:TextBox>
                        </td>
                        <td>
                           ASI Date
                        </td>
                        <td>
                             <asp:TextBox ID="txtASIDate" runat="server"></asp:TextBox>

                             <AjaxToolkit:CalendarExtender ID="calASIDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgASIDate" PopupPosition="BottomRight"
                                TargetControlID="txtASIDate">
                            </AjaxToolkit:CalendarExtender>
                            
                              <asp:Image ID="imgASIDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                             <AjaxToolkit:MaskedEditExtender ID="MskASIDate" TargetControlID="txtASIDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                  MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                             <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="MskCLPDate" ControlToValidate="txtASIDate" IsValidEmpty="true" 
                                InvalidValueMessage="ASI Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkStuffingEmailDraft" runat="server" Text="View & Send Operation Email" OnClick="lnkPreAlertEmailDraft_Click"></asp:LinkButton>  
                        </td>
                        <td>
                            <%--<asp:LinkButton ID="lnkContainerPickupEmail" runat="server" Text="Container Pick Up Email" ></asp:LinkButton>--%>
                        </td>
                        <td></td>
                        <td></td>
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
                   Export Operation Email Draft
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
                Email To:&nbsp;<asp:TextBox ID="txtCustomerEmail" runat="server" Width="85%"></asp:TextBox>
                <%--<asp:Label ID="lblCustomerEmail" runat="server"></asp:Label>--%><br />
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


            <fieldset>
                <legend>Upload Document</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                    <tr>
                        <td width="110px" align="center">Document Name
                                            <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="ddl_DocumentType"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
                        </td>
                        <td width="50px" align="center">
                            <%--<asp:TextBox ID="txtDocName" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddl_DocumentType" runat="server" DataSourceID="FrDocTypeSqlDataSource"
                                                DataTextField="Sname" DataValueField="lid">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </td>
                        <td align="center">Attachment
                                            <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuDocument" runat="server" />
                            <asp:Button ID="btnUpload" runat="server"
                                Text="Upload" OnClick="btnFileUpload_Click" ValidationGroup="validateDocument"/>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Download</legend>
                <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="99%"
                    DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                    CellPadding="4" PagerStyle-CssClass="pgr"
                    AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom" OnRowCommand="gvFreightDocument_RowCommand">  <%-- OnRowCommand="gvFreightDocument_RowCommand"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                        <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                        <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Download">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                    CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div>
                <asp:SqlDataSource ID="FrDocTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="Get_FRDocumentType" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                        <asp:QueryStringParameter Name="JobType" DbType="String" DefaultValue='Export' />
                        <asp:SessionParameter Name="JobMode" SessionField="JobMode" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


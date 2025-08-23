<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpCustPreAlert.aspx.cs"
    Inherits="FreightExport_ExpCustPreAlert" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); opacity: .8;">
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
                <asp:HiddenField ID="hdnCHAById" runat="server" />
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>PreAlert Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" OnClick="btnSubmit_Click" />

                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click" />

                    <asp:HiddenField ID="hdnModeId" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td style="width: 15%">Job No
                        </td>
                        <td style="width: 35%">
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                         <td>Enquiry No</td>
                        <td>
                            <asp:Label ID="lblEnquiryNo" runat="server"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>Shipper
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
                        <td style="width: 15%">Branch
                        </td>
                        <td style="width: 35%">
                            <asp:Label ID="lblBranch" runat="server"></asp:Label>
                        </td>
                        <td>Country</td>
                        <td>
                            <asp:Label ID="lblCountry" runat="server"></asp:Label>
                        </td>
                       
                    </tr>
                    <tr>
                        <td>Consignee
                        </td>
                        <td>
                            <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                        </td>
                        <td>Consignee Address
                        </td>
                        <td>
                            <asp:Label ID="lblConsignAddr" runat="server"></asp:Label>
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
                        <td>Customer
                        </td>
                        <td>
                            <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                        </td>
                        <td>Terms
                        </td>
                        <td>
                            <asp:Label ID="lblTerms" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Container20"
                        </td>
                        <td>
                            <asp:Label ID="lblContainer20" runat="server"></asp:Label>
                        </td>
                        <td>Container40"
                        </td>
                        <td>
                            <asp:Label ID="lblContainer40" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>LCL (CBM)
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
                        <td>Gross Weight(kgs)
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
                        <td>SB NO.
                        </td>
                        <td>
                            <asp:Label ID="lblSBNo" runat="server"></asp:Label>
                        </td>
                        <td>SB Date
                        </td>
                        <td>
                            <asp:Label ID="lblSBDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Container Pickup Date</td>
                        <td>
                            <asp:Label ID="lblContPickDate" runat="server"></asp:Label>
                        </td>
                        <td>Customs Permission Date</td>
                        <td>
                            <asp:Label ID="lblCustomPermiDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Stuffing Date
                        </td>
                        <td>
                            <asp:Label ID="lblStuffingDate" runat="server"></asp:Label>
                        </td>
                        <td>CLP Date
                        </td>
                        <td>
                            <asp:Label ID="lblCLPDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>VGM Date</td>
                        <td>
                            <asp:Label ID="lblVGMDate" runat="server"></asp:Label>
                        </td>
                        <td>Form 13 Date</td>
                        <td>
                            <asp:Label ID="lblForm13Date" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>MBL/MAWBL No</td>
                        <td>
                            <asp:TextBox ID="txtMBLNo" runat="server"></asp:TextBox>
                            <asp:regularexpressionvalidator id="regExRFVMBLNo" runat="server" controltovalidate="txtMBLNo" Display="Dynamic"
                               SetFocusOnError="true" ErrorMessage="Please enter proper MBL No" ValidationExpression="[a-zA-Z0-9]+$" ValidationGroup="Required"/>
                            <asp:RequiredFieldValidator ID="rfvMBLNo" runat="server" ControlToValidate="txtMBLNo" ValidationGroup="Required" ErrorMessage="Enter the MBL No">*
                            </asp:RequiredFieldValidator>

                           <%-- <asp:RegularExpressionValidator ID="RegExp1" runat="server"    
                                ErrorMessage="Enter 8-10 digit MBL Number" ControlToValidate="txtMBLNo"    
                                ValidationExpression="^[a-zA-Z0-9\s]{8,10}$"/>--%>

                    <%--        <asp:RangeValidator ID="rngvMBLNo" runat="server"  ControlToValidate="txtMBLNo" ValidationGroup="Required"
                                  ErrorMessage="Enter the MBL No">*</asp:RangeValidator>--%>

                           <%--  <asp:RegularExpressionValidator ID="revMBLNo" runat="server" ErrorMessage="Please Enter 7 digit MBL Number."
                                    Text="*" ValidationExpression="^[A-Z]{10}$" ControlToValidate="txtMBLNo" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td>MBL Date
                            <AjaxToolkit:CalendarExtender ID="calMBLDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgMBLDate" PopupPosition="BottomRight"
                                TargetControlID="txtMBLDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMBLDate" runat="server"></asp:TextBox>
                            <asp:Image ID="imgMBLDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            <AjaxToolkit:MaskedEditExtender ID="MskMBLDate" TargetControlID="txtMBLDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskvalMBLDate" ControlExtender="MskMBLDate" ControlToValidate="txtMBLDate" IsValidEmpty="true"
                                InvalidValueMessage="MBL Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026"
                                runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                            <asp:RequiredFieldValidator ID="rfvMBLDate" runat="server" ControlToValidate="txtMBLDate" ValidationGroup="Required" ErrorMessage="Enter the MBL date">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>HBL/MAWBL No
                        </td>
                        <td>
                            <asp:TextBox ID="txtHBLNo" runat="server" OnTextChanged="txtHBLNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvHBLNo" runat="server" ControlToValidate="txtHBLNo" ValidationGroup="Required" ErrorMessage="Enter the HBL No">*
                            </asp:RequiredFieldValidator>
                             

                          <%--  <asp:RegularExpressionValidator ID="REVHBLNo" runat="server" ErrorMessage="Please Enter 7 digit HBL Numeric Number."
                                    ValidationExpression="^[0-9]{7}$" ControlToValidate="txtHBLNo" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>

                        <%--     <asp:RegularExpressionValidator ID="REVHBLNo" runat="server" ErrorMessage="Please Enter 7 digit HBL Number."
                                    Text="*" ValidationExpression="^[0-9]{7}$" ControlToValidate="txtHBLNo" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>

                                <%--<asp:RequiredFieldValidator ID="rfvHBLNo" runat="server" Text="*" ControlToValidate="txtHBLNo" SetFocusOnError="true"
                                    ErrorMessage="Please Enter HBL No." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>--%>

                        </td>
                        <td>HBL Date
                              <AjaxToolkit:CalendarExtender ID="calHBLDate" runat="server" Enabled="True" EnableViewState="False"
                                  FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgHBLDate" PopupPosition="BottomRight"
                                  TargetControlID="txtHBLDate">
                              </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHBLDate" runat="server"></asp:TextBox>

                            <asp:Image ID="imgHBLDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                            <AjaxToolkit:MaskedEditExtender ID="MskHBLDate" TargetControlID="txtHBLDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskvalHBLDate" ControlExtender="MskHBLDate" ControlToValidate="txtHBLDate" IsValidEmpty="true"
                                InvalidValueMessage="HBL Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026"
                                runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                            <asp:RequiredFieldValidator ID="rfvHBLDate" runat="server" ControlToValidate="txtHBLDate" ValidationGroup="Required" ErrorMessage="Enter the HBL date">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trAirMode" runat="server">
                        <td>
                            Flight Schedule Details
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlightSchedule" runat="server" TextMode="MultiLine"> </asp:TextBox>
                        </td>
                        <td>
                            LEO Date
                        </td>
                        <td>
                             <asp:TextBox ID="txtLeoDate" runat="server"></asp:TextBox>

                             <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                  FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLeoDate" PopupPosition="BottomRight"
                                  TargetControlID="txtLeoDate">
                              </AjaxToolkit:CalendarExtender>

                            <asp:Image ID="imgLeoDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                            <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtHBLDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="MskHBLDate" ControlToValidate="txtLeoDate" IsValidEmpty="true"
                                InvalidValueMessage="Leo Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026"
                                runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            ETA at Transhipment
                        </td>
                        <td>
                            <asp:TextBox ID="txtETA" runat="server" ></asp:TextBox>

                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                                  FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgETA" PopupPosition="BottomRight"
                                  TargetControlID="txtETA">
                              </AjaxToolkit:CalendarExtender>

                            <asp:Image ID="imgETA" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                             <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtETA" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                     
                           
                        </td>
                        <td>
                            ETD from Transhipment 
                        </td>
                        <td>
                            <asp:TextBox ID="txtETD" runat="server"></asp:TextBox>

                            <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" EnableViewState="False"
                                  FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgETD" PopupPosition="BottomRight"
                                  TargetControlID="txtETD">
                              </AjaxToolkit:CalendarExtender>

                            <asp:Image ID="imgETD" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                             <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtETD" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                        </td>
                        </tr>
                    
                    <tr>
                        <td>
                            ATA at Destination
                        </td>
                         <td>
                             <asp:TextBox ID="txtATA" runat="server"></asp:TextBox>
                            
                             <AjaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" EnableViewState="False"
                                  FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgATA" PopupPosition="BottomRight"
                                  TargetControlID="txtATA">
                              </AjaxToolkit:CalendarExtender>

                            <asp:Image ID="imgATA" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                             <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txtATA" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                        </td>

                        </tr>
                    <tr>
                        <%--<td>Container No</td>
                        <td>
                            <asp:TextBox ID="txtContainerNo1" runat="server"></asp:TextBox>
                        </td>--%>
                        <td>PreAlert Email</td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="90%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="REVEmailId"
                                runat="server" ErrorMessage="Please Enter Valid Email ID"
                                ValidationGroup="vgSubmit" ControlToValidate="txtEmail"
                                ForeColor="Red"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*
                            </asp:RegularExpressionValidator>
                        </td>
                        <td></td>
                        <td>
                            <asp:LinkButton ID="lnkPreAlertEmailDraft" runat="server" Text="View & Send Cust Pre-Alert Email"  OnClick="lnkPreAlertEmailDraft_Click"></asp:LinkButton>
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
                    Customer Pre-Alert Email Draft
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
                    DataKeyNames="DocId" CssClass="table" DataSourceID="FreightAttachSqlDataSource"
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

            </fieldset>
             <div>
                <asp:SqlDataSource ID="FreightAttachSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>
             </div> 
            <div id="ContainerDiv" runat="server">


                <fieldset>
                    <legend>Add Container</legend>
                    <asp:Label ID="lblContainerMessage" runat="server" CssClass="errorMsg"></asp:Label>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Container 20</td>
                            <td>
                                <asp:Label ID="lblCont20" runat="server"></asp:Label>
                            </td>
                            <td>Container 40</td>
                            <td>
                                <asp:Label ID="lblCont40" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Container No
                                        <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="true" ErrorMessage="Enter Container No"
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                    ValidationGroup="valContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                    Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                            </td>
                            <td>Container Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddContainerType" runat="server">
                                    <%-- AutoPostBack="true" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged"--%>
                                    <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Container Size
                            </td>
                            <td>
                                <asp:DropDownList ID="ddContainerSize" runat="server">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnAddContainer" Text="Add Container" OnClick="btnAddContainer_Click"
                                    ValidationGroup="valContainer" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Container Detail</legend>
                    <div>
                        <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                            PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                            PageSize="40" DataSourceID="DataSourceContainer"
                            AllowSorting="true">
                            <%--OnRowDataBound="gvContainer_RowDataBound" OnRowUpdating="gvContainer_RowUpdating" OnRowDeleting="gvContainer_RowDeleting"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Container No" SortExpression="ContainerNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'
                                            MaxLength="11" Width="100px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                            ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                            Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RFVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                            ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="*" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" Text='<%#Eval("ContainerTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddEditContainerType" runat="server" SelectedValue='<%#Eval("ContainerType") %>'
                                            Width="80px">
                                            <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Size">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSize" runat="server" Text='<%#Eval("ContainerSizeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddEditContainerSize" runat="server" SelectedValue='<%#Eval("ContainerSize") %>'
                                            Width="80px">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContrUser" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContrDate" runat="server" Text='<%#Eval("updDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:CommandField ButtonType="Link" ShowEditButton="false" ShowDeleteButton="false"
                                            ValidationGroup="valGridContainer" HeaderText="Edit" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <div>
                    <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FOP_GetContainerDetailExport" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
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
                                Text="Upload" OnClick="btnFileUpload_Click" ValidationGroup="validateDocument" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Download</legend>
                <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="99%"
                    DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                    OnRowCommand="gvFreightDocument_RowCommand" CellPadding="4" PagerStyle-CssClass="pgr"
                    AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom">
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


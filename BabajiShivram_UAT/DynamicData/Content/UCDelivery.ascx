<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCDelivery.ascx.cs" Inherits="DynamicData_Content_UCDelivery" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ucc1" %>
<div>
    <ucc1:CalendarExtender ID="CalLRDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLR" PopupPosition="BottomRight"
        TargetControlID="txtLRDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalRoadPermitDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPermitDate" PopupPosition="BottomRight"
        TargetControlID="txtRoadPermitDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalNFormDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNFormDate" PopupPosition="BottomRight"
        TargetControlID="txtNFormDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalNClosingDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNClosingDate" PopupPosition="BottomRight"
        TargetControlID="txtNClosingDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalSFormDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSFormDate" PopupPosition="BottomRight"
        TargetControlID="txtSFormDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalSClosingDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSClosingDate" PopupPosition="BottomRight"
        TargetControlID="txtSClosingDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalOctroiPaidDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOctroiPaidDate" PopupPosition="BottomRight"
        TargetControlID="txtOctroiPaidDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalChallahDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChallanDate" PopupPosition="BottomRight"
        TargetControlID="txtBabajiChallanDate">
    </ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalExamineDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgExamineDt" PopupPosition="BottomRight"
        TargetControlID="txtExamineDate"></ucc1:CalendarExtender>
    <ucc1:CalendarExtender ID="CalOutOfChargeDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOutOfChargeDt"
        PopupPosition="BottomRight" TargetControlID="txtOutOfChargeDate"></ucc1:CalendarExtender>
</div>
<div>
    <fieldset><legend>Delivery Detail</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Job Ref No
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdnJobId" Value="0" runat="server" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" />
                    <asp:HiddenField ID="hdnDeliveryId" Value="0" runat="server" />
                    <asp:HiddenField ID="hdnUploadPath" runat="server" />
                </td>
                <td>
                    Cargo Move To
                    <asp:RequiredFieldValidator ID="RFVCargoMoveTo" runat="server" Text="*" InitialValue="0" 
                        SetFocusOnError="true" ControlToValidate="ddTransitType" ErrorMessage="Please Select Cargo Move To"
                        ValidationGroup="RequiredJob"> </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddTransitType" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddTransitType_SelectedIndexChanged" TabIndex="12">
                        <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Move to Customer Place"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Move To General Warehouse"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Move To In Bonded Warehouse"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddWarehouse" runat="server" Visible="false" TabIndex="10"> </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFVBonded" runat="server" ControlToValidate="ddWarehouse"
                    SetFocusOnError="true" InitialValue="0" ErrorMessage="Please Select Warehouse Name"
                    Text="*" ValidationGroup="RequiredJob" Enabled="false"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Examine Date
                </td>
                <td>
                    <asp:TextBox ID="txtExamineDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgExamineDt" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                        runat="server" />
                    <ucc1:MaskedEditExtender ID="MEditExtExaminDate" TargetControlID="txtExamineDate"
                        Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                        runat="server"></ucc1:MaskedEditExtender>
                    <ucc1:MaskedEditValidator ID="MEditValExaminDate" ControlExtender="MEditExtExaminDate" 
                        ControlToValidate="txtExamineDate" InvalidValueMessage="Examine Date is invalid" IsValidEmpty="false" 
                        SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                        EmptyValueBlurredText="*" EmptyValueMessage="Examine Date Required"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="RequiredJob"></ucc1:MaskedEditValidator>
                </td>
                <td>
                    Out Of Charge Date
                </td>
                <td>
                    <asp:TextBox ID="txtOutOfChargeDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgOutOfChargeDt" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                        runat="server" />
                    <ucc1:MaskedEditExtender ID="MEditExtOutDate" TargetControlID="txtOutOfChargeDate"
                        Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                        runat="server"></ucc1:MaskedEditExtender>
                    <ucc1:MaskedEditValidator ID="MEditValOutDate" ControlExtender="MEditExtOutDate" ControlToValidate="txtOutOfChargeDate"
                        InvalidValueMessage="Out Of Charge Date is invalid" SetFocusOnError="true" IsValidEmpty="false"
                        EmptyValueBlurredText="*" EmptyValueMessage="Out Of Charge Date Required"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014"
                        MaximumValue="31/12/2025" runat="Server" ValidationGroup="RequiredJob"></ucc1:MaskedEditValidator>
                </td>
            </tr>
            <tr>
                <td>
                    No of Packages
                    <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages" Operator="DataTypeCheck" SetFocusOnError="true" 
                        Type="Integer" Text="Invalid Packages" ErrorMessage="Invalid No of Packages" Display="Dynamic" ValidationGroup="RequiredJob"></asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtNoOfPackages" SetFocusOnError="true"   
                        InitialValue="" Text="*" ErrorMessage="Please Enter No of Packages" Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtNoOfPackages" MaxLength="8" runat="server" Width="100px" TabIndex="1"></asp:TextBox>
                    <ucc1:MaskedEditExtender ID="MEditPackage" TargetControlID="txtNoOfPackages" Mask="999999" MessageValidatorTip="true" 
                        MaskType="Number" AutoComplete="false" runat="server"></ucc1:MaskedEditExtender>
                    <ucc1:MaskedEditValidator ID="MEditValPackage" ControlExtender="MEditPackage" ControlToValidate="txtNoOfPackages" IsValidEmpty="false" 
                        EmptyValueBlurredText="*" EmptyValueMessage="Invalid Number Of Packages" InvalidValueBlurredMessage="Invalid Package"
                        InvalidValueMessage="Package is invalid" MinimumValueMessage="Invalid Packages" MaximumValueMessage="Invalid Package"
                        MinimumValue="1" SetFocusOnError="true" Runat="Server" ValidationGroup="RequiredJob"></ucc1:MaskedEditValidator>
                </td>
                <td>
                    Balance Packages
                </td>
                <td>
                    <asp:Label ID="lblBalancePackage" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdnBalPackage" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Delivery Destination
                    <asp:RequiredFieldValidator ID="RFVDeliveryDestination" runat="server" ControlToValidate="txtDestination" SetFocusOnError="true"   
                        InitialValue="" Text="*" ErrorMessage="Please Enter Delivery Destination" Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtDestination" runat="server" MaxLength="100" TabIndex="6"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <asp:Panel ID="pnlLRDetail" runat="server" Visible="false">
            <tr>
                <td>
                    LR No
                </td>
                <td>
                    <asp:TextBox ID="txtLRNo" runat="server" MaxLength="50" TabIndex="7"></asp:TextBox>
                </td>
                <td>
                LR Date
                        
                </td>
                <td>
                    <asp:TextBox ID="txtLRDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgLR" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    LR Attachment
                </td>
                <td>
                    <asp:FileUpload ID="fuLRCopy" runat="server" TabIndex="9" />
                    <asp:HiddenField ID="hdnLRPath" runat="server" />
                </td>
            </tr>
            </asp:Panel>
            <%--<tr>
                <td>
                    Cargo Delivered Date
                </td>
                <td>
                    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" TabIndex="10" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgDelivery" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
                <td>
                    Cargo Recvd Person Name
                </td>
                <td>
                    <asp:TextBox ID="txtCargoPersonName" runat="server" MaxLength="50" TabIndex="11"></asp:TextBox>
                </td>
            </tr>--%>
            <div id="divExpenseAir" runat="server" visible="false">
            <tr>
                <td>
                    Runway Delivery ?
                    <asp:RequiredFieldValidator ID="RFVRunway" Text="*" runat="server" ControlToValidate="rdlRunwayDelivery" 
                        ValidationGroup="RequiredJob" ErrorMessage="Please Check Shipment is Runway Delivery - Yes/No" 
                        Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rdlRunwayDelivery" runat="server" RepeatDirection="Horizontal" TabIndex="12">
                        <asp:ListItem Text="NO" Value="false"></asp:ListItem>
                        <asp:ListItem Text="YES" Value="true"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            </div>
            <div id="divExpenseSea" runat="server" visible="false">
            <tr>
                <td>
                    Labour Type
                    <asp:RequiredFieldValidator ID="RFVLabourType" runat="server" ControlToValidate="ddLabourType" SetFocusOnError="true"   
                        Text="*" InitialValue="0" ErrorMessage="Please Select Labour Type" Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddLabourType" runat="server" Width="100px" TabIndex="12">
                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Forklift" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Labour" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Crane" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </div>
            <asp:Panel ID="pnlOctroiApplicable" runat="server" Visible="false">
            <tr>
                <td>
                    Octroi Amount
                    <asp:RequiredFieldValidator ID="RFVOctroiAmt" runat="server" ControlToValidate="txtOctroiAmount" SetFocusOnError="true"   
                        Text="*" ErrorMessage="Please Enter Octroi Amount." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CVOctroiAmount" runat="server" ControlToValidate="txtOctroiAmount" Display="Dynamic" SetFocusOnError="true"
                        Type="Double" Operator="DataTypeCheck" ErrorMessage="Invalid Octroi Amount" ValidationGroup="RequiredJob"></asp:CompareValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtOctroiAmount" runat="server" MaxLength="12" Width="100px" TabIndex="12"></asp:TextBox>
                </td>
                <td>
                    Octroi Receipt No
                    <asp:RequiredFieldValidator ID="RFVOctroiReceipt" runat="server" ControlToValidate="txtOctroiReceiptNo" SetFocusOnError="true"   
                        Text="*" ErrorMessage="Please Enter Octroi Receipt No." Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtOctroiReceiptNo" runat="server" MaxLength="100" TabIndex="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Octroi Paid Date
                </td>
                <td>
                    <asp:TextBox ID="txtOctroiPaidDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgOctroiPaidDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
                <td>
                </td>
                <td></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="pnlSFormApplicable" runat="server" Visible="false">
                <tr>
                <td>
                    S Form No
                    <asp:RequiredFieldValidator ID="RFVSFormNo" runat="server" ControlToValidate="txtSFormNo" SetFocusOnError="true"   
                        Text="*" ErrorMessage="Please Enter S Form No." Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtSFormNo" runat="server" MaxLength="100" TabIndex="12"></asp:TextBox>
                </td>
                <td>
                    S Form Date
                </td>
                <td>
                    <asp:TextBox ID="txtSFormDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgSFormDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td> 
            </tr>
                <tr>
                <td>
                    S Form Closing Date
                </td>
                <td>
                    <asp:TextBox ID="txtSClosingDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgSClosingDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
                <td></td>
                <td></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="pnlNFormApplicable" runat="server" Visible="false">
                <tr>
                    <td>
                        N Form No
                        <asp:RequiredFieldValidator ID="RFVNFormNo" runat="server" ControlToValidate="txtNFormNo" SetFocusOnError="true"   
                        Text="*" ErrorMessage="Please Enter N Form No." Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNFormNo" runat="server" MaxLength="100" TabIndex="12"></asp:TextBox>
                    </td>
                    <td>
                        N Form Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtNFormDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgNFormDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        N Form Closing Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtNClosingDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgNClosingDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlRoadPermitApplicable" runat="server" Visible="false">
                <tr>
                    <td>
                        Road Permit No
                        <asp:RequiredFieldValidator ID="RFVRoadPermitNo" runat="server" ControlToValidate="txtRoadPermitNo" SetFocusOnError="true"   
                        Text="*" ErrorMessage="Please Enter Road Permit No." Display="Dynamic" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRoadPermitNo" runat="server" MaxLength="50" TabIndex="12"></asp:TextBox>
                    </td>
                    <td>
                        Road Permit Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtRoadPermitDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="ImgPermitDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td>
                    Babaji Challan No
                </td>
                <td>   
                    <asp:TextBox ID="txtBabajiChallanNo" runat="server" MaxLength="50" TabIndex="13"></asp:TextBox>&nbsp;
                </td>
                <td>
                    Babaji Challan Date
                </td>
                <td>
                    <asp:TextBox ID="txtBabajiChallanDate" runat="server" Width="100px" TabIndex="14" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgChallanDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>    
            <tr>
                <td>
                    Babaji Challan Copy
                </td>
                <td>
                    <asp:FileUpload ID="fuChallanCopy" runat="server" />
                    <asp:HiddenField ID="hdnChallanPath" runat="server" />
                </td>
                <td>
                    Damage Image
                </td>
                <td>
                    <asp:FileUpload ID="fuDamageCopy" runat="server" />
                    <asp:HiddenField ID="hdnDamagePath" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    RMS/NonRMS
                </td>
                <td>
                    <asp:Label ID="lblRMSNonRms" runat="server"></asp:Label>
                </td>
                <td>
                    Container Re-Examine
                </td>
                <td>
                    <asp:Label ID="lblConReExamine" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    CFS Name
                </td>
                <td>
                    <asp:Label ID="lblCFSName" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset><legend>Delivery History</legend>
        <div>
        <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
            DataSourceID="DataSourceBalance" CellPadding="4" AllowPaging="True" AllowSorting="True">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" />
                <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                <asp:BoundField HeaderText="Transporter" DataField="TransporterName" />
                <asp:BoundField HeaderText="LR No" DataField="LRNo" />
                <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="User" DataField="UserName" />
            </Columns>
        </asp:GridView>
    </div>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="DataSourceBalance" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetDeliveryDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="JobId" Type="Int32" />
                <asp:ControlParameter Name="TransitType" ControlID="ddTransitType" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>            
</div>

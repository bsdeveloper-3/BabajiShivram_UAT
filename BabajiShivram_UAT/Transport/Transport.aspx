<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Transport.aspx.cs" Inherits="Test_Transport" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    
    <div>
        <asp:UpdateProgress ID="updProgress1" AssociatedUpdatePanelID="updTransport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div>
    <fieldset>
    <legend>Transport Detail</legend>
        <asp:UpdatePanel ID="updTransport" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="validateTransport" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="m">
                <asp:Button ID="btnSave" Text="Save" ValidationGroup="validateTransport"
                    runat="server" TabIndex="9"/>
                <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server"
                    CausesValidation="false" TabIndex="10"/>
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>
                            Ref No
                        </td>
                        <td>
                            <asp:Label ID="lblRefNo" runat="server" ></asp:Label>
                        </td>
                        <td>
                            Date
                        </td>
                        <td>
                           <span> <%=DateTime.Now.ToString("dd/MM/yyyy") %> </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Transporter
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransPortName" runat="server" TabIndex="1" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            Delivery Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDeliveryType" runat="server">
                                <asp:ListItem Text="Loaded" Value="1"></asp:ListItem>
                                <asp:ListItem Text="DeStuff" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Delivery From
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrom" runat="server" TabIndex="2" Width="80%"></asp:TextBox>
                         </td>
                        <td>
                            Destination
                        </td>
                        <td>
                            <asp:TextBox ID="txtTo" runat="server" TabIndex="3" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No Of Packages
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoOfPkgs" runat="server" TabIndex="6" Width="20%" type="Number"></asp:TextBox>
                            <asp:CompareValidator ID="CompValPackgs" runat="server" ControlToValidate="txtNoOfPkgs"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid No Of Packages"
                                Display="Dynamic" ValidationGroup="validateTransport"></asp:CompareValidator>
                        </td>
                        <td>
                            Weight (Kgs)
                        </td>
                        <td>
                            <asp:TextBox ID="txtGrossWeight" runat="server" TabIndex="7" Width="20%"></asp:TextBox>
                            <asp:CompareValidator ID="ComValGrossWT" runat="server" ControlToValidate="txtGrossWeight"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Gross Weight"
                                Display="Dynamic" ValidationGroup="validateTransport"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Container 20"
                        </td>
                        <td>
                            <asp:TextBox ID="txtCont20" runat="server" TabIndex="4" Width="20%" type="Number"></asp:TextBox>
                            <asp:CompareValidator ID="CompValCon20" runat="server" ControlToValidate="txtCont20"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 20"
                                Display="Dynamic" ValidationGroup="validateTransport"></asp:CompareValidator>
                        </td>
                        <td>
                            Container 40"
                        </td>
                        <td>
                            <asp:TextBox ID="txtCont40" runat="server" TabIndex="5" Width="20%" type="Number"></asp:TextBox>
                            <asp:CompareValidator ID="CompValCon40" runat="server" ControlToValidate="txtCont40"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 40"
                                Display="Dynamic" ValidationGroup="validateTransport"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Remarks</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" runat="server" TabIndex="8" TextMode="MultiLine" Width="70%" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <fieldset>
        <legend>Vehicle Detail</legend>
    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Vehicle No & Type
                <asp:RequiredFieldValidator ID="RFVVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true"   
                    Text="*" ErrorMessage="Please Enter Vehicle No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
            </td>
            <td>   
                <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" Width="100px" TabIndex="2"></asp:TextBox>&nbsp;
                <asp:DropDownList ID="ddVehicleType" runat="server" Width="100px" TabIndex="3"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RFVVehicleType" runat="server" ControlToValidate="ddVehicleType" SetFocusOnError="true"   
                    Text="*" InitialValue="0" ErrorMessage="Please Select Vehicle Type" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
            </td>
            <td>
                
            </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td>
                No Of Packages
                <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages" Operator="DataTypeCheck" SetFocusOnError="true" 
                   Type="Integer" Text="Invalid Packages" ErrorMessage="Invalid No Of Packages" Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtNoOfPackages" SetFocusOnError="true"   
                    Text="*" ErrorMessage="Please Enter No Of Packages" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="txtNoOfPackages" MaxLength="8" runat="server" Width="100px" TabIndex="1" type="number"></asp:TextBox>
            </td>
            <td>
                Container
            </td>
            <td>
                20"
                <asp:TextBox ID="txtCount20" MaxLength="8" runat="server" Width="50px" TabIndex="1" type="number"></asp:TextBox>
                
                <asp:TextBox ID="txtCount40" MaxLength="8" runat="server" Width="50px" TabIndex="1" type="number"></asp:TextBox>
                40"
            </td>
        </tr>
        <tr>
            <td>
                Location From
            </td>
            <td>
                <asp:TextBox ID="txtLocationFrom" runat="server" MaxLength="100" TabIndex="5"></asp:TextBox>
            </td>
            <td>
                Destination
            </td>
            <td>
                <asp:TextBox ID="txtDestination" runat="server" MaxLength="100" TabIndex="6"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td>
                Dispatch Date
                <cc1:MaskedEditExtender ID="MEditDispatchDate" TargetControlID="txtDispatchDate" Mask="99/99/9999" MessageValidatorTip="true" 
                    MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="MEditValDispatchDate" ControlExtender="MEditDispatchDate" ControlToValidate="txtDispatchDate" IsValidEmpty="false" 
                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Dispatch Date" InvalidValueBlurredMessage="Invalid Date" 
                    InvalidValueMessage="Dispatch Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                    MinimumValue="01/01/2015" SetFocusOnError="true" Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
            </td>
            <td>
                <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" TabIndex="7" placeholder="dd/mm/yyyy"></asp:TextBox>
                <asp:Image ID="imgClearance" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                    runat="server" />
            </td>
            <td>
                Delivered Date
                <cc1:MaskedEditExtender ID="MEditDeliveredDate" TargetControlID="txtDeliveryDate" Mask="99/99/9999" MessageValidatorTip="true" 
                    MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="MEditValDeliveredDate" ControlExtender="MEditDeliveredDate" ControlToValidate="txtDeliveryDate" IsValidEmpty="true"
                    InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Cargo Delivered Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" 
                    MinimumValue="01/01/2015" SetFocusOnError="true" Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
            </td>
            <td>
                <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                <asp:Image ID="imgDelivery" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
            </td>
        </tr>
                    
    </table>
    </fieldset>
    </div>
</asp:Content>
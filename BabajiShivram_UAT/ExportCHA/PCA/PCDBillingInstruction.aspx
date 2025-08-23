<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCDBillingInstruction.aspx.cs"
    Inherits="PCA_PCDBillingInstruction" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </cc1:ToolkitScriptManager>

    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;            
            $get('ctl00_ContentPlaceHolder1_hdnModuleId').value = results.ModuleId;
            Alert(results.ModuleId);
        }
    </script>

    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>

                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnJobType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>

                <fieldset>
                    <legend>Billing Instruction</legend>
                    <div id="divInstruction" class="info" runat="server">
                    </div>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" TabIndex="7" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" TabIndex="8" OnClick="btnCancel_Click" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job Number
                            <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"
                                ControlToValidate="txtJobNumber" Text="*" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:autocompleteextender id="JobDetailExtender" runat="server" targetcontrolid="txtJobNumber"
                                    completionlistelementid="divwidthCust_Loc" servicepath="~/WebService/JobNumberAutoComplete.asmx"
                                    servicemethod="GetCompletionList" minimumprefixlength="2" behaviorid="divwidthCust_Loc"
                                    contextkey="4567" usecontextkey="True" onclientitemselected="OnJobSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCust" runat="server" Enabled="false" Width="290px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblConsigShipper" runat="server" Text="Consignee"></asp:Label>
                                </td>
                            <td>
                                <asp:Label ID="lblConsignee" runat="server" Enabled="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Shipment Type
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblShipmentType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Shipment Category
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblShipmentCate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Allied Service
                            </td>
                            <td>
                                <asp:CheckBoxList ID="chkAgencyService" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="ADC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="FSSAI" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="PQ" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="AQ" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Texttile Committee" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Sample Testing" Value="6"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                            <td style="border-right: hidden;">Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtAgencyServiceRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Other Service</td>
                            <td>
                                <asp:CheckBoxList ID="chkOtherService" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Licence" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Chartered" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="SVB" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="SVB Registration" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Sampling" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="CE Certificate" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Labelling" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Re-Packing" Value="8"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                            <td style="border-right: hidden;">Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherServiceRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Other Instruction
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>Attachment Copy</td>
                            <td>
                                <asp:FileUpload ID="FuInstructionCopy" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Other Instruction
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherRemark1" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>Attachment Copy</td>
                            <td>
                                <asp:FileUpload ID="FuInstructionCopy1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Other Instruction
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherRemark2" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>Attachment Copy</td>
                            <td>
                                <asp:FileUpload ID="FuInstructionCopy2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Other Instruction
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherRemark3" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>Attachment Copy</td>
                            <td>
                                <asp:FileUpload ID="FuInstructionCopy3" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


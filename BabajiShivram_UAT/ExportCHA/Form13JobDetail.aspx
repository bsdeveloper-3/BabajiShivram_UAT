<%@ Page Title="Form 13 Job Detaikldl" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Form13JobDetail.aspx.cs"
    Inherits="ExportCHA_Form13JobDetail" MaintainScrollPositionOnPostback="true" Culture="en-GB"
    EnableSessionState="True" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />

    <div class="clear">
    </div>
    <fieldset>
        <legend>Job Detail</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>BS Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>Cust Ref No
                </td>
                <td>
                    <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Customer
                </td>
                <td>
                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                </td>
                <td>Consignee
                </td>
                <td>
                    <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Shipper
                </td>
                <td>
                    <asp:Label ID="lblShipper" runat="server"></asp:Label>
                </td>
                <td>Mode
                </td>
                <td>
                    <asp:Label ID="lblMode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Port Of Loading
                </td>
                <td>
                    <asp:Label ID="lblPOL" runat="server"></asp:Label>
                </td>
                <td>Port Of Discharge
                </td>
                <td>
                    <asp:Label ID="lblPOD" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Country Consignment
                </td>
                <td>
                    <asp:Label ID="lblCountryConsgn" runat="server"></asp:Label>
                </td>
                <td>Destination Country  
                </td>
                <td>
                    <asp:Label ID="lblDestCountry" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Forwarder Name</td>
                <td>
                    <asp:Label ID="lblForwarderName" runat="server"></asp:Label>
                </td>
                <td>No Of Packages
                </td>
                <td>
                    <asp:Label ID="lblNoOfPkg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Gross Weight
                </td>
                <td>
                    <asp:Label ID="lblGrossWT" runat="server"></asp:Label>
                </td>
                <td>Net WT
                </td>
                <td>
                    <asp:Label ID="lblNetWT" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>SB No
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
                <td>LEO Date
                </td>
                <td>
                    <asp:Label ID="lblLEODate" runat="server"></asp:Label>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </fieldset>
</asp:Content>


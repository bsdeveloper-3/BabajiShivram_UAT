<%@ Page Title="PCA To Customer Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCDDocDetail.aspx.cs"
    Inherits="ExportPCA_PCDDocDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />

    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>

    <fieldset>
        <legend>Job Shipping Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnBack" runat="server" OnClientClick="javascript:history.back(); return false;" CausesValidation="false" Text="Back" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>BS Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>Cust Ref No.
                </td>
                <td>
                    <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Customer
                </td>
                <td>
                    <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                </td>
                <td>Shipper Name
                </td>
                <td>
                    <asp:Label ID="lblShipper" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Consignee
                </td>
                <td>
                    <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                </td>
                <td>Trans Mode
                </td>
                <td>
                    <asp:Label ID="lblTransMode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Buyer Name
                </td>
                <td>
                    <asp:Label ID="lblBuyerName" runat="server"></asp:Label>
                </td>
                <td>Type of Export</td>
                <td>
                    <asp:Label ID="lblExportType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Product Desc
                </td>
                <td colspan="3">
                    <asp:Label ID="lblProductDesc" runat="server" Width="90%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Port Of Loading
                </td>
                <td>
                    <asp:Label ID="lblPortOfLoading" runat="server"></asp:Label>
                </td>
                <td>Port Of Discharge
                </td>
                <td>
                    <asp:Label ID="lblPortOfDischarge" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Consignment Country
                </td>
                <td>
                    <asp:Label ID="lblConsignmentCountry" runat="server"></asp:Label>
                </td>
                <td>Destination Country
                </td>
                <td>
                    <asp:Label ID="lblDestCountry" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>No Of Packages
                </td>
                <td>
                    <asp:Label ID="lblNoOfPkg" runat="server"></asp:Label>
                </td>
                <td>Shipping Bill Type
                </td>
                <td>
                    <asp:Label ID="lblShippingBillType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Forwarder Name
                </td>
                <td>
                    <asp:Label ID="lblForwarderName" runat="server"></asp:Label>
                </td>
                <td>Container Loaded
                </td>
                <td>
                    <asp:Label ID="lblContainerLoaded" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Gross Weight
                </td>
                <td>
                    <asp:Label ID="lblGrossWT" runat="server"></asp:Label>
                </td>
                <td>Net Weight
                </td>
                <td>
                    <asp:Label ID="lblNetWT" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Transport By
                </td>
                <td>
                    <asp:Label ID="lblTransportBy" runat="server"></asp:Label>
                </td>
                <td>SB No</td>
                <td>
                    <asp:Label ID="lblSBNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>SB Date
                </td>
                <td>
                    <asp:Label ID="lblSBDate" runat="server"></asp:Label>
                </td>
                <td>Supretendent LEO Date</td>
                <td>
                    <asp:Label ID="lblLEODate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Document Hand Over To Shipping Line Date
                </td>
                <td>
                    <asp:Label ID="lblShippingLineDate" runat="server"></asp:Label>
                </td>
                <td>Exporter Copy</td>
                <td>
                    <asp:LinkButton ID="lnkbtnDownloadExpCopy" runat="server" OnClick="lnkbtnDownloadExpCopy_OnClick"
                        Font-Underline="true" ToolTip="Download Exporter Copy"></asp:LinkButton>
                    <asp:HiddenField ID="hdnExporterCopyPath" runat="server" />

                </td>
            </tr>
            <tr>
                <td>VGM Copy
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnDwnloadVGMCopy" runat="server" OnClick="lnkbtnDwnloadVGMCopy_OnClick"
                        Font-Underline="true" ToolTip="Download VGM Copy"></asp:LinkButton>
                    <asp:HiddenField ID="hdnVGMCopyPath" runat="server" />
                </td>
                <td>Informed To Freight Forwarded Date</td>
                <td>
                    <asp:Label ID="FreightForDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Forwarder Person Name</td>
                <td>
                    <asp:Label ID="lblForwarderPersonName" runat="server"></asp:Label>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </fieldset>
</asp:Content>


<%@ Page Title="Quote Success" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuoteSuccess.aspx.cs" 
    Inherits="CRM_QuoteSuccess" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css">
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <style type="text/css">
        .ui-tooltip {
            font-size: small;
            font-family: Arial;
        }

        label {
            display: inline-block;
            width: 5em;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).tooltip({
                track: true
            });
        });
    </script>

    <fieldset style="width: 95%">
        <div align="center" id="dvErrorSection" runat="server">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="Valsummary" runat="server" ShowMessageBox="true" ShowSummary="false"
                ValidationGroup="vgRequired" />
            <asp:ValidationSummary ID="valDraftQuote" runat="server" ShowMessageBox="true" ShowSummary="false"
                ValidationGroup="vgDraftQuote" />
            <asp:ValidationSummary ID="vsLumpSumRange" runat="server" ShowMessageBox="true" ShowSummary="false"
                ValidationGroup="vgLumpSumRange" />
            <asp:HiddenField ID="hdnPath" runat="server" />
        </div>
        <br />
        <br />

        <div align="center" class="m clear">
            <asp:ImageButton ID="imgbtnDownloadCopy" runat="server" ToolTip="Download PDF for Quotation." ImageUrl="~/Images/pdf2.png" OnClick="imgbtnDownloadCopy_OnClick" Width="25" Height="25" />
        </div>
    </fieldset>


</asp:Content>


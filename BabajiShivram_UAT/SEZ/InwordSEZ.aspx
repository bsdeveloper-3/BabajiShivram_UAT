<%@ Page Title="SEZ" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InwordSEZ.aspx.cs"
    Inherits="SEZ_InwordSEZ" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            background-color: #000032;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 1px;
            padding-left: 0px;
            /*width: 400px;
            height: 155px;*/
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <asp:Panel ID="panFirstcheck" runat="server" Visible="false">

        <fieldset>
            <legend>SEZ Detail</legend>

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
            </table>


        </fieldset>
    </asp:Panel>

</asp:Content>


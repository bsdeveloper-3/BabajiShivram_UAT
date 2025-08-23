<%@ Page Title="MOM/Visit Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Others.aspx.cs"
    Inherits="CRM_Others" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }

        .tbl_td {
            height: 30px;
            background-color: #2196f363;
            padding: 5px;
            border-radius: 2px;
            font-size: 13px;
            border: 1px solid #2196f38a;
        }

        .dvListing {
            padding-left: 10px;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlLeads" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlLeads" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset>
                <legend>Listings</legend>
                <div>
                    <table border="1" cellpadding="0" cellspacing="0" width="50%" bgcolor="white">
                        <tr>
                            <td class="tbl_td" style="border: 1px solid #2196f38a;">
                                <div class="dvListing">
                                    <b>(1) &nbsp;<a href="CustomerMom.aspx">Minutes of Meeting For Babaji Customer</a></b>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table border="1" cellpadding="0" cellspacing="0" width="50%" bgcolor="white">
                        <tr>
                            <td class="tbl_td" style="border: 1px solid #2196f38a;">
                                <div class="dvListing">
                                    <b>(2) &nbsp;<a href="CustomerVisit.aspx">Visit Report For Babaji Customer</a></b>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


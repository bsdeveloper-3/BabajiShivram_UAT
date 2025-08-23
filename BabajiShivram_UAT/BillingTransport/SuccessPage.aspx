<%@ Page Title="Success" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true" CodeFile="SuccessPage.aspx.cs" Inherits="BillingTransport_SuccessPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Status</legend>
        <div class="success" align="center">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
        <div style="height: 15px">
            &nbsp;&nbsp;
        </div>
    </fieldset>

</asp:Content>
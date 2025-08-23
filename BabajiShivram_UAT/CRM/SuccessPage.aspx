<%@ Page Title="Success Page" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SuccessPage.aspx.cs"
    Inherits="CRM_SuccessPage" EnableEventValidation="false" Culture="en-GB" %>

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


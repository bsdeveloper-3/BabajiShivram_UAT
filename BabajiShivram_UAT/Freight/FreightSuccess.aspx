<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightSuccess.aspx.cs" Inherits="Freight_FreightSuccess" 
    MasterPageFile="~/MasterPage.master" Title="Success" Culture="en-GB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<fieldset>
    <legend>Freight Status</legend>
    <div class="success" align="center">
        <asp:Label ID="lblFreightMessage" runat="server"></asp:Label>
    </div>
    <div style="height:15px">
        &nbsp;&nbsp;
    </div>
</fieldset>
</asp:Content>
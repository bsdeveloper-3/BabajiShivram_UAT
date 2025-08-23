<%@ Page Title="Success" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AccountSuccess.aspx.cs" 
    Inherits="AccountExpense_AccountSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <fieldset style="width:70%; height:300px">
    <legend>Status</legend>
    <div align="center">
        <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="Medium" class="successImg"></asp:Label>
    </div>
    <div style="height:15px">
        &nbsp;&nbsp;
    </div>
</fieldset>
</asp:Content>


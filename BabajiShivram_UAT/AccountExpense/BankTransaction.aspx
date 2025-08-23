<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BankTransaction.aspx.cs" 
    Inherits="AccountExpense_BankTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:DropDownList ID="ddAccoutNo" runat="server">
        <asp:ListItem Text="--Account No --" Value="0"></asp:ListItem>
        <asp:ListItem Text="000000000000002" Value="000000000000002"></asp:ListItem>
        <asp:ListItem Text="000000000000005" Value="000000000000005"></asp:ListItem>
    </asp:DropDownList> 
    <asp:TextBox ID="txtReferenceNo" runat="server"></asp:TextBox>
    <br /><br /> 

<%--    <asp:Button ID="btnGetToken" runat="server" Text="Generation Token" OnClick="btnGetToken_Click" />--%>
    
    <asp:Button ID="btnGetBalance" runat="server" Text="Check Balance" OnClick="btnGetBalance_Click" />
    <asp:Button ID="btnGetTransferStatus" runat="server" Text="Check Transfer Status" OnClick="btnUATStatus_Click" />
    <asp:Button ID="btnTransferFund" runat="server" Text="Transfer Fund" OnClick="btnTransferFund_Click" />


    <%--<asp:Button ID="btnGetLiveBalance" runat="server" Text="Check Live Balance" OnClick="btnGetLiveBalance_Click" />

    <asp:Button ID="btnGetLiveStatus" runat="server" Text="Check Live Status" OnClick="btnGetLiveStatus_Click" />

    <asp:Button ID="btnGetLiveTransfer" runat="server" Text="Live Transfer" OnClick="btnGetLiveTransfer_Click" />--%>



    <br /><br />

    <asp:Label ID="lblReqest" runat="server" Text=""></asp:Label>
            <br /><br />
    <asp:Label ID="lblToken" runat="server" Text=""></asp:Label>
</asp:Content>


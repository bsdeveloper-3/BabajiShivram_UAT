<%@ Page Title="Reject E-Way Bill" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="RejectEWayBill.aspx.cs" Inherits="EWayBill_RejectEWayBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="m clear"></div>
    <fieldset><legend>Search & Reject E-Way Bill</legend>
    <div style="margin-left:10px;">
    <span><b>E-Way Bill No </b></span>
    <asp:RequiredFieldValidator ID="rfvBillNo" runat="server" ControlToValidate="txtEwayBillNo"
       Text="Required" InitialValue="" ValidationGroup="Required" Display="Dynamic" ></asp:RequiredFieldValidator>
    <asp:TextBox ID="txtEwayBillNo" runat="server" Text=""></asp:TextBox>
    
    <asp:DropDownList ID="ddTrasnporter" runat="server">
        <asp:ListItem Value="27AAACN1163G1ZR" Text="NAVBHARAT CLEARING AGENTS PVT.LTD." Selected="True"></asp:ListItem>
        <asp:ListItem Value="27AAFFN5296A1ZA" Text="NAV JEEVAN AGENCY"></asp:ListItem>
        <asp:ListItem Value="27AAACB0466A1ZB" Text="MAHARASHTRA"></asp:ListItem>
    </asp:DropDownList>

    &nbsp;&nbsp;<asp:Button ID="btnGetewayBill" runat="server" Text="Get Bill Detail" OnClick="btnGetewayBill_Click" ValidationGroup="Required"/>
    <br /><br />
    <asp:RegularExpressionValidator ID="RegExBill" Display="Dynamic" ControlToValidate="txtEwayBillNo" 
        ValidationExpression = "^[\s\S]{12,12}$" runat="server" ErrorMessage="Invalid Bill No! 12 Digit Required."></asp:RegularExpressionValidator>
    </div>
    </fieldset>
</asp:Content>


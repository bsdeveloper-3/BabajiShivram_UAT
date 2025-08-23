<%@ Page Title="Multi Vehicle" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MultiVehicle.aspx.cs" Inherits="EWayBill_MultiVehicle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="m clear"></div>
    <fieldset><legend>Multi Vehicle E-Way Bill</legend>
    
    <span><b>E-Way Bill No </b></span>
    <asp:RequiredFieldValidator ID="rfvBillNo" runat="server" ControlToValidate="txtEwayBillNo"
       Text="Required" InitialValue="" ValidationGroup="Required" Display="Dynamic" ></asp:RequiredFieldValidator>
    <asp:TextBox ID="txtEwayBillNo" runat="server" Text="" TextMode="Number"  Width="110px"></asp:TextBox>

    <asp:DropDownList ID="ddTrasnporter" runat="server">
        <asp:ListItem Value="27AAACN1163G1ZR" Text="NAVBHARAT CLEARING AGENTS" Selected="True"></asp:ListItem>
        <asp:ListItem Value="27AAFFN5296A1ZA" Text="NAV JEEVAN AGENCY"></asp:ListItem>
        <asp:ListItem Value="27AAACB0466A1ZB" Text="MAHARASHTRA"></asp:ListItem>
    </asp:DropDownList>

    <asp:Button ID="btnGetewayBill" runat="server" Text="Get Bill Detail" OnClick="btnGetewayBill_Click" ValidationGroup="Required"/>
    <br />
    <asp:RegularExpressionValidator ID="RegExBill" Display="Dynamic" ControlToValidate="txtEwayBillNo"  ValidationGroup="Required"
        ValidationExpression = "^[\s\S]{12,12}$" runat="server" ErrorMessage="Invalid Bill No! 12 Digit Required."></asp:RegularExpressionValidator>
    <br /><br />
    </fieldset>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text='Note *** After submission enter the vehicle details in "Update PART-B/Vehicle"' Font-Size="Large"></asp:Label>
    
</asp:Content>


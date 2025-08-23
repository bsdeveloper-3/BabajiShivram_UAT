<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestTransaction.aspx.cs" Inherits="AccountExpense_TestTransaction" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:Button ID="btnGetLiveBalance" runat="server" Text="Check Live Balance" OnClick="btnGetLiveBalance_Click" />
    <br /><br /><br /><br /><br /><br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblToken" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>

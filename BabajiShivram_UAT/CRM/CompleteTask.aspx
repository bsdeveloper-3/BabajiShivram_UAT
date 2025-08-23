<%@ Page Title="Complete Task" Language="C#" AutoEventWireup="true" CodeFile="CompleteTask.aspx.cs"
    Inherits="CRM_CompleteTask" Culture="en-GB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Complete Task</title>
</head>
<body>
    <form id="frmComplete" runat="server" class="formApproval">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <fieldset>
            <div class="success" align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div style="height: 15px">
                &nbsp;&nbsp;
            </div>
        </fieldset>
    </form>
</body>
</html>



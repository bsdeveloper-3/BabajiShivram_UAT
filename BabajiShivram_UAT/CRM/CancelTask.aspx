<%@ Page Title="Cancel Task" Language="C#" AutoEventWireup="true" CodeFile="CancelTask.aspx.cs" Inherits="CRM_CancelTask" Culture="en-GB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cancel Task</title>
</head>
<body>
    <form id="frmCancel" runat="server" class="formApproval">
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

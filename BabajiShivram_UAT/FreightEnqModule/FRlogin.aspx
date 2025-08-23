<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FRlogin.aspx.cs" Inherits="FreightEnqModule_FRlogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <link href="../CSS/babaji-shivram.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .container
        {
            position: relative;
            padding-right: 15px;
            padding-left: 15px;
            margin-right: auto;
            margin-left: auto;
        }
        .row
        {
            margin-right: -15px;
            margin-left: -15px;
        }
        .col-md-offset-3
        {
            margin-left: 25%;
            width: 50%;
            float: left;
            padding-right: 15px;
            padding-left: 15px;
        }
        .alert
        {
            padding: 6px 10px;
            border-left: 0;
            color: #a94442;
            background-color: #f2dede;
            border-color: #ebccd1;
            margin-bottom: 10px;
            border: 1px solid transparent;
        }
        .box-static
        {
            border-color: #8ab933;
            background-color: rgba(0,0,0,0.05);
            padding: 30px !important;
        }
        .h2_style
        {
            font-size: 20px !important;
            line-height: 26px !important;
            font-family: 'Raleway' , 'Open Sans' ,Arial,Helvetica,sans-serif;
            font-weight: 600;
            color: #414141;
            margin: 0 0 15px 0;
        }
        
        .form-control
        {
            border: #ddd 2px solid;
            box-shadow: none;
            display: block;
            width: 85%;
            height: 20px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
        }
        
        .btn
        {
            padding: 6px 12px;
            font-size: 14px;
            font-weight: 400;
            background-color: #8ab933;
            border: 1px solid transparent;
        }
    </style>
</head>
<body class="hidden" style="font-family: sans-serif;">
    <form id="frmBillTranLogin" runat="server">
    <div style="min-height: 765px;">
        <div class="container" style="padding: 100px">
            <div class="row">
                <div class="col-md-offset-3">
                    <!-- ALERT -->
                    <div id="lblErrorMsg" runat="server" class="alert" style="font-weight: 600">
                        Error
                    </div>
                    <!-- /ALERT -->
                    <div class="box-static" style="border: #ddd 2px solid; background: url(../Images/wall2.jpg);
                        background-size: cover;">
                        <div style="margin-bottom: 20px; border-bottom: rgba(0,0,0,0.1) 1px solid;">
                            <h2 class="h2_style">
                                Login
                            </h2>
                        </div>
                        <div style="margin: 0 !important;">
                            <table width="100%">
                                <tr>
                                    <td style="float: left;">
                                        <img id="imgLogo" src="../Images/BS_logo_Login.jpg" style="border: 2px solid darkgray"
                                            runat="server" alt="" width="250" height="150" />
                                    </td>
                                    <td>
                                        <div>
                                            <!-- Email -->
                                            <div style="margin-bottom: 15px;">
                                                <asp:TextBox ID="txtEmail" runat="server" required="" class="form-control" placeholder="Email"
                                                    ToolTip="Enter Email ID." TabIndex="1"></asp:TextBox>
                                            </div>
                                            <!-- Password -->
                                            <div class="form-group">
                                                <asp:TextBox ID="txtPassword" runat="server" class="form-control" placeholder="Password"
                                                    required="" TextMode="Password" ToolTip="Enter Password" TabIndex="2"></asp:TextBox>
                                            </div>
                                            <div class="row">
                                                <div style="padding-left: 15px; padding-top: 20px;">
                                                    <asp:Button ID="btnSubmit" class="btn" BackColor="YellowGreen" runat="server" Text="OK, LOG IN"
                                                        OnClick="btnSubmit_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuccessKYC.aspx.cs" Inherits="KYC_New_SuccessKYC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Success</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- CSS -->
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Roboto:400,100,300,500">
    <!-- CORE CSS -->
    <link href="assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- THEME CSS -->
    <link href="assets/css/essentials.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/layout.css" rel="stylesheet" type="text/css" />
    <!-- PAGE LEVEL SCRIPTS -->
    <link href="assets/css/header-1.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/color_scheme/red.css" rel="stylesheet" type="text/css" id="color_scheme" />
    <style type="text/css">
        .SuccessMsg {
            background: #10bf1075;
            border: 2px solid darkgreen;
            text-align: center;
            width: fit-content;
            padding-left: 25px;
            padding-right: 25px;
            border-radius: 5px;
        }
        hr{
            border: 1px solid #ec2500;
        }
    </style>
</head>
<body class="smoothscroll enable-animation" style="text-align: center">
    <form id="frmSuccessKYC" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <!-- Top menu -->
            <nav class="navbar navbar-inverse navbar-no-bg" role="navigation">
                <div class="container">
                    <div class="navbar-header" style="padding-top: 15px">
                        <a href="#" style="color: #f35b3f; font-family: serif; font-family: 'Raleway','Open Sans',Book Antiqua,Helvetica,sans-serif; font-weight: 900; font-size: 28px;">Babaji Shivram Clearing &amp; Carriers Pvt Ltd</a>
                    </div>
                </div>
            </nav>
            <!-- Top content -->
            <div class="top-content" style="padding-top: 2px; padding-bottom: 50px">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-10 col-md-8 col-md-offset-2 col-lg-10 col-lg-offset-1 form-box">
                            <div class="f1">
                                <h3 style="font-weight: 500; font-family: serif">Know Your Customer
                                </h3>
                                <hr />
                                <fieldset id="fsGST">
                                    <div class="row">
                                        <div class="form-group"></div>
                                    </div>
                                    <div class="row" style="padding-top: 2px;">
                                        <div class="form-group">
                                            <div class="col-sm-12 col-md-12" style="text-align: -webkit-center">
                                                <label class="SuccessMsg">Successfully saved your KYC copy!</label>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        var plugin_path = 'assets/plugins/';
    </script>
    <script type="text/javascript" src="assets/plugins/jquery/jquery-2.1.4.min.js"></script>
    <script src="assets/js/jquery.backstretch.min.js" type="text/javascript"></script>
    <script src="assets/js/retina-1.1.0.min.js" type="text/javascript"></script>
    <script src="assets/js/scripts.js" type="text/javascript"></script>
    <script type="text/javascript" src="assets/plugins/select2/js/select2.full.min.js"></script>
</body>
</html>

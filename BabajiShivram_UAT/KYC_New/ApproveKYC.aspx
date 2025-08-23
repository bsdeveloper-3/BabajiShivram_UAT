<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApproveKYC.aspx.cs" Inherits="KYC_New_ApproveKYC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approve KYC</title>
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
        .form-control[disabled], .form-control[readonly], fieldset[disabled] .form-control {
            background-color: #ffffff75;
        }

        hr {
            border: 1px solid #ec2500;
        }
    </style>
</head>
<body class="smoothscroll enable-animation" style="text-align: center">
    <form id="frmSaveKYC" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:HiddenField ID="hdnVendorId" runat="server" Value="0" />
            <!-- Top menu -->
            <nav class="navbar navbar-inverse navbar-no-bg" role="navigation">
                <div class="container">
                    <div class="navbar-header" style="padding-top: 15px">
                        <a href="#" style="color: #f35b3f; font-family: serif; font-family: 'Raleway','Open Sans',Book Antiqua,Helvetica,sans-serif; font-weight: 900; font-size: 28px;">Babaji Shivram Clearing &amp; Carriers Pvt Ltd</a>
                    </div>
                </div>
            </nav>
            <!-- Top content -->
            <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="true" ValidationGroup="vgReject" />
            <div class="top-content" style="padding-top: 2px; padding-bottom: 5px">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-10 col-md-8 col-md-offset-2 col-lg-10 col-lg-offset-1 form-box" style="padding-top: 10px">
                            <div class="f1" style="padding-bottom: 2px">
                                <h3 style="font-weight: 500; font-family: serif">Know Your Customer
                                </h3>
                                <hr />

                                <fieldset id="fsGeneral">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-5 col-md-5"></div>
                                            <div class="col-sm-1 col-md-1" style="padding-left: 60px">
                                                <label>
                                                    <asp:ImageButton ID="imgbtnKYCCopy" runat="server" ToolTip="Download KYC Form Copy Here" ImageUrl="~/Images/file.gif" Width="28px" Height="28px" OnClick="imgbtnKYCCopy_Click" />
                                                    <asp:HiddenField ID="hdnKYCCopyPath" runat="server" />
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>Company Name</label>
                                                <div>
                                                    <asp:TextBox ID="txtGeneral_CompanyName" runat="server" class="form-control required" TabIndex="1"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Type</label>
                                                <div>
                                                    <asp:DropDownList ID="ddlGeneral_KYCType" runat="server" class="form-control" TabIndex="2">
                                                        <asp:ListItem Value="1" Text="Customer"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="Vendor"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>Corporate Address 1</label>
                                                <div class="fancy-form">
                                                    <asp:TextBox ID="txtGeneral_CorporateAddress1" TextMode="MultiLine" runat="server" TabIndex="3" CssClass="form-control required"
                                                        Rows="2" ToolTip="Enter corporate address 1."></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Corporate Address 2</label>
                                                <div class="fancy-form">
                                                    <asp:TextBox ID="txtGeneral_CorporateAddress2" TextMode="MultiLine" runat="server" TabIndex="4" CssClass="form-control"
                                                        Rows="2" ToolTip="Enter corporate address 2."></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>City</label>
                                                <div>
                                                    <asp:TextBox ID="txtGeneral_City" runat="server" class="form-control required" TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>State</label>
                                                <div class="fancy-form fancy-form-select">
                                                    <asp:DropDownList ID="ddlGeneral_State" runat="server" class="form-control required" DataSourceID="DataSourceStateMS" TabIndex="6"
                                                        DataTextField="StateName" DataValueField="StateId">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>Country</label>
                                                <div class="fancy-form fancy-form-select">
                                                    <asp:DropDownList ID="ddlCountry" runat="server" DataSourceID="DataSourceCountryMS" TabIndex="7" CssClass="form-control required" DataTextField="CountryName" DataValueField="CountryId"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>
                                                    Telephone No
                                                      <asp:RegularExpressionValidator ID="revTelephone" runat="server" ControlToValidate="txtTelephoneNo" SetFocusOnError="true"
                                                          Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid telephone no)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtTelephoneNo" runat="server" TabIndex="9" MaxLength="20" CssClass="form-control required"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-12 col-md-12">
                                                <label>
                                                    Rejection Reason
                                                    <asp:RequiredFieldValidator ID="rfvReject" runat="server" ControlToValidate="txtRemark" ValidationGroup="vgReject" SetFocusOnError="true"
                                                        Display="Dynamic" ErrorMessage="Please Enter Rejection Reason!" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </label>
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-5 col-md-5"></div>
                                            <div class="col-sm-2 col-md-2 pull-right">
                                                <asp:Button ID="btnApprove" runat="server" TabIndex="2" CssClass="btn btn-primary" BackColor="#f35b3f" ForeColor="White" Text="&nbsp; &nbsp; Approve &nbsp; &nbsp;"
                                                    OnClick="btnApprove_Click" OnClientClick="return confirm('Are you sure to approve this KYC?');" />
                                            </div>
                                            <div class="col-sm-1 col-md-1 pull-right" style="width: 12%">
                                                <asp:Button ID="btnReject" runat="server" TabIndex="2" CssClass="btn btn-primary" BackColor="#f35b3f" ForeColor="White" Text="&nbsp; &nbsp; Reject &nbsp; &nbsp;"
                                                    ValidationGroup="vgReject" OnClick="btnReject_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="DataSourceStateMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetStateMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceCountryMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetCountryMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KYC.aspx.cs" Inherits="KYC_New_KYC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KYC</title>
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
        .select2-container--default .select2-selection--single {
            border: 1px solid #ddd;
        }

        input#chkBillingOperation {
            height: 15px;
            width: 18px;
            margin-top: 5px;
        }

        input#chkBillingFinance {
            height: 15px;
            width: 18px;
            margin-top: 5px;
        }

        input#chkBillingOther {
            height: 15px;
            width: 18px;
            margin-top: 5px;
        }

        table#gvMaterial, table#gvServices {
            background-color: rgba(2, 2, 2, 0.54);
        }

        .thheader {
            font-weight: 500;
        }

        input#txtGeneral_CompanyName {
            background-color: #ffffff75;
        }
    </style>
</head>
<body class="smoothscroll enable-animation" style="text-align: center">
    <form id="frmKYC" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:HiddenField ID="ChangeFieldset" runat="server" />
            <asp:HiddenField ID="hdnUploadFile" runat="server" />

            <!-- Top menu -->
            <nav class="navbar navbar-inverse navbar-no-bg" role="navigation">
                <div class="container">
                    <div class="navbar-header" style="padding-top: 15px">
                        <a href="#" style="color: #f35b3f; font-family: serif; font-family: 'Raleway','Open Sans',Book Antiqua,Helvetica,sans-serif; font-weight: 900; font-size: 28px;">Babaji Shivram Clearing &amp; Carriers Pvt Ltd</a>
                    </div>
                </div>
            </nav>
            <!-- Top content -->
            <asp:ValidationSummary ID="vsGeneralDetails" runat="server" ShowMessageBox="true" ValidationGroup="vgGeneralDetails" CssClass="modal" />
            <asp:ValidationSummary ID="vsBillingAddress" runat="server" ShowMessageBox="true" ValidationGroup="vgBillingAddress" />
            <asp:HiddenField ID="hdnEnquiryId" runat="server" Value="0" />
            <div class="top-content" style="padding-top: 2px; padding-bottom: 50px">
                <div class="container">
                    <div class="row">
                        <%-- <div class="col-sm-8 col-sm-offset-1 col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2 form-box">--%>
                        <div class="col-sm-10 col-md-8 col-md-offset-2 col-lg-10 col-lg-offset-1 form-box">
                            <div class="f1">
                                <h3 style="font-weight: 500; font-family: serif">Know Your Customer
                                </h3>
                                <div class="f1-steps">
                                    <div class="f1-progress">
                                        <div class="f1-progress-line" data-now-value="10.66" data-number-of-steps="4" style="width: 10.66%;">
                                        </div>
                                    </div>
                                    <div class="f1-step active">
                                        <div class="f1-step-icon">
                                            <i class="fa fa-user"></i>
                                        </div>
                                        <p>
                                            General
                                        </p>
                                    </div>
                                    <div class="f1-step">
                                        <div class="f1-step-icon">
                                            <i class="fa fa-book"></i>
                                        </div>
                                        <p>
                                            Company
                                        </p>
                                    </div>
                                    <div class="f1-step">
                                        <div>
                                            <div class="f1-step-icon">
                                                <i class="fa fa-star"></i>
                                            </div>
                                            <p id="title_GST">
                                                GST
                                            </p>
                                        </div>
                                    </div>
                                    <div class="f1-step">
                                        <div id="step_BillingContact">
                                            <div class="f1-step-icon">
                                                <i class="fa fa-pencil"></i>
                                            </div>
                                            <p>
                                                Billing/Contact
                                            </p>
                                        </div>
                                    </div>
                                    <div class="f1-step">
                                        <div id="step_Other">
                                            <div class="f1-step-icon">
                                                <i class="fa fa-upload"></i>
                                            </div>
                                            <p>
                                                Other
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <fieldset id="fsGeneral">
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
                                                    <asp:DropDownList ID="ddlGeneral_KYCType" runat="server" class="form-control" TabIndex="2" onchange="return AddKYCType();">
                                                        <asp:ListItem Value="1" Text="Customer"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="Vendor"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Overseas Customer"></asp:ListItem>
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
                                                        Rows="2" ToolTip="Enter corporate address 1." MaxLength="75"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Corporate Address 2</label>
                                                <div class="fancy-form">
                                                    <asp:TextBox ID="txtGeneral_CorporateAddress2" TextMode="MultiLine" runat="server" TabIndex="4" CssClass="form-control"
                                                        Rows="2" ToolTip="Enter corporate address 2." MaxLength="75"></asp:TextBox>
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
                                                    <asp:DropDownList ID="ddlGeneral_State" runat="server" class="form-control required" DataSourceID="DataSourceStateMS" TabIndex="6" AppendDataBoundItems="true"
                                                        DataTextField="StateName" DataValueField="StateId" AutoPostBack="true" OnSelectedIndexChanged="ddlGeneral_State_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Value="0" Text="- select -"></asp:ListItem>
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
                                            <div class="col-sm-2 col-md-2" style="padding-right: 2px">
                                                <label>Pin Code</label>
                                                <div class="fancy-form">
                                                    <asp:TextBox ID="txtPinCode" runat="server" TabIndex="8" CssClass="form-control masked" data-format="999999"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4" style="padding-left: 2px">
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
                                            <%--<div class="col-sm-6 col-md-6">
                                                <label>
                                                    Fax No
                                                    <asp:RegularExpressionValidator ID="revFaxNo" runat="server" ControlToValidate="txtFaxNo" SetFocusOnError="true"
                                                        Display="Dynamic" ValidationExpression="\+[0-9]{1,3}\([0-9]{3}\)[0-9]{7,}" ErrorMessage="(Invalid fax no)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtFaxNo" runat="server" TabIndex="10" CssClass="form-control" MaxLength="35"></asp:TextBox>
                                                </div>
                                            </div>--%>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Constitution</label>
                                                <div class="fancy-form fancy-form-select">
                                                    <asp:DropDownList ID="ddlConstitution" runat="server" DataSourceID="DataSourceConstitutionMS" TabIndex="11" CssClass="form-control required" DataTextField="sName" DataValueField="lId"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>Sector</label>
                                                <div class="fancy-form fancy-form-select">
                                                    <asp:DropDownList ID="ddlSector" runat="server" DataSourceID="DataSourceSectorMS" TabIndex="12" CssClass="form-control" DataTextField="sName" DataValueField="lid"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Nature Of Business</label>
                                                <div class="fancy-form fancy-form-select">
                                                    <asp:DropDownList ID="ddlNatureOfBusiness" runat="server" DataSourceID="DataSourceNatureOfBusinessMS" TabIndex="13" CssClass="form-control required" DataTextField="sName" DataValueField="lId"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>Website Address</label>
                                                <div>
                                                    <asp:TextBox ID="txtGeneral_WebsiteAdd" runat="server" TabIndex="14" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>
                                                    Email id
                                                    <asp:RegularExpressionValidator ID="revGeneral_Email" runat="server" ErrorMessage="(Invalid email id)"
                                                        ValidationGroup="vgGeneral" ForeColor="#ff3300" SetFocusOnError="true" Display="Dynamic"
                                                        ControlToValidate="txtGeneral_Email" ValidationExpression="^[a-zA-Z0-9][-a-zA-Z0-9._]+@([-a-zA-Z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtGeneral_Email" runat="server" TabIndex="15" CssClass="form-control" MaxLength="300"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="f1-buttons">
                                        <button type="button" class="btn btn-next" id="btnGeneralNext" tabindex="16" style="background-color: #f35b3f; color: white" onclick="return ValidateGeneral();">Next</button>
                                    </div>
                                </fieldset>
                                <fieldset id="fsCompany">
                                    <div id="dvCompany_Overseas">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        PAN No
                                                        <asp:RegularExpressionValidator ID="revPanNo" runat="server" ControlToValidate="txtPANNo"
                                                            Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                            ValidationGroup="rec" ValidationExpression="[A-Z a-z]{5}\d{4}[A-Z a-z]{1}">
                                                        </asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtPANNo" runat="server" class="form-control required" TabIndex="25"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        VAT No
                                                    <asp:RegularExpressionValidator ID="revVATNo" runat="server" ControlToValidate="txtVATNo"
                                                        Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                        ValidationGroup="rec" ValidationExpression="[A-Z]{2}[A-Z a-z 0-9]{2,13}">
                                                    </asp:RegularExpressionValidator></label>
                                                    <div>
                                                        <asp:TextBox ID="txtVATNo" runat="server" TabIndex="26" CssClass="form-control" MaxLength="13"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        Service Tax No
                                                     <asp:RegularExpressionValidator ID="revServiceTaxNo" runat="server" ControlToValidate="txtServiceTaxNo"
                                                         Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                         ValidationGroup="rec" ValidationExpression="^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$">
                                                     </asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtServiceTaxNo" runat="server" class="form-control" TabIndex="27" MaxLength="15"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        Excise No
                                                    <asp:RegularExpressionValidator ID="revExciseNo" runat="server" ControlToValidate="txtExciseNo"
                                                        Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                        ValidationGroup="rec" ValidationExpression="[A-Z a-z]{5}\d{4}[A-Z a-z]{1}[EM ED]{2}[0-9]{3}">
                                                    </asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtExciseNo" runat="server" TabIndex="28" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        CST No
                                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCSTNo"
                                                         Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                         ValidationGroup="rec" ValidationExpression="[0-9 ]{11}">
                                                     </asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtCSTNo" runat="server" class="form-control" TabIndex="29" MaxLength="11"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label>
                                                        TAN No
                                                     <asp:RegularExpressionValidator ID="revTANNo" runat="server" ControlToValidate="txtTANNo"
                                                         Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                         ValidationGroup="rec" ValidationExpression="[A-Z]{4}[0-9]{5}[A-Z]{1}">
                                                     </asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtTANNo" runat="server" TabIndex="30" class="form-control required" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px">
                                                    <label>
                                                        SSI No
                                                     <asp:RegularExpressionValidator ID="revSSINo" runat="server" ControlToValidate="txtSSINo"
                                                         Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                         ValidationGroup="rec" ValidationExpression="[0-9]{3}[-][0-9]{2}[-][0-9]{4}">
                                                     </asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtSSINo" runat="server" TabIndex="32" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>Bank Name</label>
                                                <div>
                                                    <asp:TextBox ID="txtCompany_BankName" runat="server" class="form-control" TabIndex="33"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Account No</label>
                                                <div>
                                                    <asp:TextBox ID="txtCompany_AccountNo" runat="server" TabIndex="34" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>
                                                    IFSC Code
                                                     <asp:RegularExpressionValidator ID="revIFSCCode" runat="server" ControlToValidate="txtIFSCCode"
                                                         Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                         ValidationGroup="rec" ValidationExpression="[A-Z]{4}[0][A-Z 0-9]{6}">
                                                     </asp:RegularExpressionValidator>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtIFSCCode" runat="server" class="form-control" TabIndex="35" MaxLength="11"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>
                                                    MICR Code
                                                     <asp:RegularExpressionValidator ID="revMICRCode" runat="server" ControlToValidate="txtMICRCode"
                                                         Display="Dynamic" ErrorMessage="(Invalid)" SetFocusOnError="true" ForeColor="#ff3300"
                                                         ValidationGroup="rec" ValidationExpression="[0-9]{9}">
                                                     </asp:RegularExpressionValidator>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtMICRCode" runat="server" TabIndex="36" CssClass="form-control" MaxLength="9"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-6 col-md-6">
                                                <label>IEC Code</label>
                                                <div>
                                                    <asp:TextBox ID="txtCompany_IECCode" runat="server" class="form-control" TabIndex="37" MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6">
                                                <label>Payment Terms (In days)</label>
                                                <div>
                                                    <asp:TextBox ID="txtCompany_PaymentTerms" runat="server" TabIndex="38" class="form-control required"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="f1-buttons">
                                        <button type="button" tabindex="39" class="btn btn-previous" style="background-color: #f35b3f; color: white">
                                            Previous</button>
                                        <button type="button" tabindex="40" id="btnCompanyNext" class="btn btn-next" style="background-color: #f35b3f; color: white" onclick="return ValidateCompany();">
                                            Next</button>
                                    </div>
                                </fieldset>
                                <fieldset id="fsGST">
                                    <div id="dvGSTDetails">
                                        <div class="row" style="padding-top: 2px;">
                                            <div class="form-group">
                                                <div class="col-sm-12 col-md-12">
                                                    <asp:UpdatePanel ID="upnlGSTDetail" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-sm-2 col-md-2">
                                                                        <asp:Button ID="btnAddGSTDetail" runat="server" Text="ADD MORE ROWS" OnClick="btnAddGSTDetail_OnClick"
                                                                            CssClass="btn btn-xs btn-3d btn-primary" />
                                                                    </div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                   <%-- <div class="col-sm-2 col-md-2"></div>--%>
                                                                </div>
                                                            </div>
                                                            <asp:ListView ID="lvGSTDetails" runat="server" AutoGenerateColumns="false" OnItemDataBound="lvGSTDetails_ItemDataBound" OnItemCommand="lvGSTDetails_ItemCommand">
                                                                <ItemTemplate>
                                                                    <div class="row" style="margin-bottom: 3px">
                                                                        <div class="form-group">
                                                                            <div class="col-sm-3 col-md-3" style="padding-right: 2px; padding-left: 2px">
                                                                                <label>Location Of Branch</label>
                                                                                <div>
                                                                                    <asp:TextBox ID="txtGST_CompanyName" runat="server" class="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-md-3" style="padding-right: 2px; padding-left: 2px">
                                                                                <label>State Branch</label>
                                                                                <div>
                                                                                    <asp:DropDownList ID="ddlGST_Branch" runat="server" class="form-control" DataSourceID="DataSourceStateBranchGst"
                                                                                        DataTextField="sName" DataValueField="lId" AppendDataBoundItems="true" Style="margin-bottom: 0px">
                                                                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-md-3" style="padding-right: 2px; padding-left: 2px">
                                                                                <label>Address</label>
                                                                                <div class="fancy-form">
                                                                                    <asp:TextBox ID="txtGST_Address" TextMode="MultiLine" Height="40px" runat="server" CssClass="form-control"
                                                                                        Rows="1" ToolTip="Enter address."></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                                                <label>Contact Person</label>
                                                                                <div class="fancy-form">
                                                                                    <asp:TextBox ID="txtGST_ContactPerson" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-bottom: 3px">
                                                                        <div class="form-group">
                                                                            <div class="col-sm-3 col-md-3" style="padding-right: 2px; padding-left: 2px">
                                                                                <label>Person Number</label>
                                                                                <div>
                                                                                    <asp:TextBox ID="txtGST_ContactNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-md-3" style="padding-right: 2px; padding-left: 2px">
                                                                                <label>
                                                                                    Person Email
                                                                                 <asp:RegularExpressionValidator ID="revGST_Email" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true" Display="Dynamic"
                                                                                     ControlToValidate="txtGST_Email" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                                                </label>
                                                                                <div>
                                                                                    <asp:TextBox ID="txtGST_Email" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-md-3" style="padding-right: 2px; padding-left: 2px">
                                                                                <label>
                                                                                    GST Provision ID No
                                                                                   <asp:RegularExpressionValidator ID="revGST_GSTNo" runat="server" Display="Dynamic" ControlToValidate="txtGST_ProvisionId" ForeColor="#ff3300"
                                                                                       ErrorMessage="(Invalid)" SetFocusOnError="true" ValidationExpression="[0-9]{2}[A-Z a-z]{5}\d{4}[A-Z a-z]{1}\d{1}[Z]{1}[A-Z a-z 0-9]{1}">
                                                                                   </asp:RegularExpressionValidator>
                                                                                </label>
                                                                                <div class="fancy-form">
                                                                                    <asp:TextBox ID="txtGST_ProvisionId" runat="server" CssClass="form-control"
                                                                                        ToolTip="Enter address." MaxLength="15"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                                                <label>
                                                                                    ARN No
                                                                                 <asp:RegularExpressionValidator ID="revGST_ArnNo" runat="server" Display="Dynamic" ControlToValidate="txtGST_ArnNo" ForeColor="#ff3300"
                                                                                     ErrorMessage="(Invalid)" SetFocusOnError="true" ValidationExpression="[0-9]{15}">
                                                                                 </asp:RegularExpressionValidator>
                                                                                </label>
                                                                                <div class="fancy-form">
                                                                                    <asp:TextBox ID="txtGST_ArnNo" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-bottom: 0px">
                                                                        <div class="form-group">
                                                                            <div class="col-sm-4 col-md-4"></div>
                                                                            <div class="col-sm-8 col-md-8">
                                                                                <div class="fancy-file-upload fancy-file-primary">
                                                                                    <asp:FileUpload ID="fuDocument" runat="server" CssClass="form-control" Width="50%" />
                                                                                    <span class="button">Upload GSTN/ARN Copy</span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div style="border-top: 1px solid #de2727; padding-bottom: 20px"></div>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="f1-buttons">
                                            <button type="button" tabindex="39" class="btn btn-previous" style="background-color: #f35b3f; color: white">
                                                Previous</button>
                                            <button type="button" title="GST Next" tabindex="40" id="btnGSTNext" class="btn btn-next" style="background-color: #f35b3f; color: white">
                                                Next</button>
                                        </div>
                                    </div>
                                    <div id="dvOverseas">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_BillingName" required runat="server" class="form-control" TabIndex="41"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        Email id
                                                    <asp:RegularExpressionValidator ID="revOversea_BillingEmail" runat="server" ErrorMessage="(Invalid email id)"
                                                        ValidationGroup="vgBilling" ForeColor="#ff3300" SetFocusOnError="true" Display="Dynamic"
                                                        ControlToValidate="txtOversea_BillingEmail" ValidationExpression="^[a-zA-Z0-9][-a-zA-Z0-9._]+@([-a-zA-Z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_BillingEmail" runat="server" TabIndex="42" CssClass="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        Mobile No
                                                     <asp:RegularExpressionValidator ID="revOversea_Mobile" runat="server" ControlToValidate="txtBilling_MobileNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid mobile no)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_BillingMobile" runat="server" class="form-control" TabIndex="43"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>Address</label>
                                                    <div class="fancy-form">
                                                        <asp:TextBox ID="txtOversea_BillingAddress" TextMode="MultiLine" runat="server" Height="40px" TabIndex="44" CssClass="form-control" required
                                                            Rows="1" ToolTip="Enter billing address."></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>Pin Code</label>
                                                    <div class="fancy-form">
                                                        <asp:TextBox ID="txtOversea_BillingPinCode" runat="server" class="form-control masked" data-format="999999" TabIndex="45"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>City</label>
                                                    <div class="fancy-form">
                                                        <asp:TextBox ID="txtOversea_BillingCity" runat="server" TabIndex="46" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr />
                                        <div class="row" style="margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <asp:CheckBox ID="chkOversea_BillingOperation" runat="server" ToolTip="Include same as above billing details" onchange="return GetOversea_BillingDetails(1);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 5px; margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label><span class="label label-danger">OPERATION contact</span> Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OperationContact" runat="server" class="form-control" required TabIndex="47"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Email id
                                                    <%--<asp:RegularExpressionValidator ID="revOversea_OperationEmail" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true"
                                                        Display="Dynamic" ControlToValidate="txtOversea_OperationEmail" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>--%>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OperationEmail" runat="server" TabIndex="48" CssClass="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Mobile No
                                                     <%--<asp:RegularExpressionValidator ID="revOversea_OperationMobile" runat="server" ControlToValidate="txtOversea_OperationMobile" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>--%>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OperationMobile" runat="server" class="form-control" required TabIndex="49"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px;">
                                                    <label>
                                                        Landline No
                                                     <asp:RegularExpressionValidator ID="revOversea_OperationLandline" runat="server" ControlToValidate="txtOversea_OperationLandline" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OperationLandline" runat="server" class="form-control" TabIndex="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <asp:CheckBox ID="chkOversea_BillingFinance" runat="server" ToolTip="Include same as above billing details" onchange="return GetOversea_BillingDetails(2);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 5px; margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label><span class="label label-danger">FINANCE contact</span> Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_FinanceContact" runat="server" class="form-control" required TabIndex="51"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Email id
                                                    <%--<asp:RegularExpressionValidator ID="revOversea_FinanceEmail" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true"
                                                        Display="Dynamic" ControlToValidate="txtOversea_FinanceEmail" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>--%>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_FinanceEmail" runat="server" TabIndex="52" CssClass="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Mobile No
                                                     <%--<asp:RegularExpressionValidator ID="revOversea_FinanceMobile" runat="server" ControlToValidate="txtOversea_FinanceMobile" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>--%>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_FinanceMobile" runat="server" class="form-control" TabIndex="53"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px;">
                                                    <label>
                                                        Landline No
                                                     <asp:RegularExpressionValidator ID="revOversea_FinanceLandline" runat="server" ControlToValidate="txtOversea_FinanceLandline" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_FinanceLandline" runat="server" class="form-control" TabIndex="54"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <asp:CheckBox ID="chkOversea_Other" runat="server" ToolTip="Include same as above billing details" onchange="return GetOversea_BillingDetails(3);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 5px;">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label><span class="label label-danger">OTHERS contact</span> Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OtherContact" runat="server" class="form-control" TabIndex="55"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Email id
                                                    <%--<asp:RegularExpressionValidator ID="revOversea_OtherEmail" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true"
                                                        Display="Dynamic" ControlToValidate="txtOversea_OtherEmail" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>--%>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OtherEmail" runat="server" TabIndex="56" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Mobile No
                                                     <%--<asp:RegularExpressionValidator ID="revOversea_OtherMobile" runat="server" ControlToValidate="txtOversea_OtherMobile" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>--%>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OtherMobile" runat="server" class="form-control" TabIndex="57"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px;">
                                                    <label>
                                                        Landline No
                                                     <asp:RegularExpressionValidator ID="revOversea_OtherLandline" runat="server" ControlToValidate="txtOversea_OtherLandline" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOversea_OtherLandline" runat="server" class="form-control" TabIndex="58"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="background-color: rgba(2, 2, 2, 0.54)">
                                            <div class="form-group">
                                                <div class="col-sm-12 col-md-12">
                                                    <label>Other Copy</label>
                                                    <div class="fancy-file-upload fancy-file-primary">
                                                        <asp:FileUpload ID="fuOversea_OtherCopy" runat="server" CssClass="form-control" TabIndex="65" Width="50%" onchange="jQuery(this).next('input').val(this.value);" />
                                                        <input type="text" class="form-control" placeholder="no file selected" readonly="" style="color: white; background-color: #ccccc; width: 50%" />
                                                        <span class="button">Upload Copy</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="f1-buttons">
                                            <button type="button" tabindex="66" class="btn btn-previous" style="background-color: #f35b3f; color: white">
                                                Previous</button>
                                            <asp:Button ID="btnOversea_SaveKYC" runat="server" TabIndex="67" CssClass="btn btn-primary" BackColor="#f35b3f" ForeColor="White" Text="Print KYC"
                                                OnClick="btnOversea_SaveKYC_Click" />
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset id="fsBillingContact">
                                    <div id="dvBillingContact">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtBilling_Name" runat="server" class="form-control required" TabIndex="41"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        Email id
                                                    <asp:RegularExpressionValidator ID="revBilling_Email" runat="server" ErrorMessage="(Invalid email id)"
                                                        ValidationGroup="vgBilling" ForeColor="#ff3300" SetFocusOnError="true" Display="Dynamic"
                                                        ControlToValidate="txtBilling_Email" ValidationExpression="^[a-zA-Z0-9][-a-zA-Z0-9._]+@([-a-zA-Z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtBilling_Email" runat="server" TabIndex="42" CssClass="form-control required"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>
                                                        Mobile No
                                                     <asp:RegularExpressionValidator ID="revBilling_MobileNo" runat="server" ControlToValidate="txtBilling_MobileNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid mobile no)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtBilling_MobileNo" runat="server" class="form-control" TabIndex="43"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>Address</label>
                                                    <div class="fancy-form">
                                                        <asp:TextBox ID="txtBilling_Address" TextMode="MultiLine" runat="server" Height="40px" TabIndex="44" CssClass="form-control required"
                                                            Rows="1" ToolTip="Enter billing address."></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-sm-6 col-md-6">
                                                    <label>Pin Code</label>
                                                    <div class="fancy-form">
                                                        <asp:TextBox ID="txtBilling_PinCode" runat="server" class="form-control masked" data-format="999999" TabIndex="45"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6">
                                                    <label>City</label>
                                                    <div class="fancy-form">
                                                        <asp:TextBox ID="txtBilling_City" runat="server" TabIndex="46" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr />
                                        <div class="row" style="margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <asp:CheckBox ID="chkBillingOperation" runat="server" ToolTip="Include same as above billing details" onchange="return GetBillingDetails(1);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 5px; margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label><span class="label label-danger">OPERATION contact</span> Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtOperation_ContactName" runat="server" class="form-control required" TabIndex="47"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Email id
                                                    <asp:RegularExpressionValidator ID="revOperation_Email" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true"
                                                        Display="Dynamic" ControlToValidate="txtOperation_Email" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOperation_Email" runat="server" TabIndex="48" CssClass="form-control required"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Mobile No
                                                     <asp:RegularExpressionValidator ID="revOperation_MobileNo" runat="server" ControlToValidate="txtOperation_MobileNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOperation_MobileNo" runat="server" class="form-control required" TabIndex="49"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px;">
                                                    <label>
                                                        Landline No
                                                     <asp:RegularExpressionValidator ID="revOperation_Landline" runat="server" ControlToValidate="txtOperation_LandlineNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOperation_LandlineNo" runat="server" class="form-control" TabIndex="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <asp:CheckBox ID="chkBillingFinance" runat="server" ToolTip="Include same as above billing details" onchange="return GetBillingDetails(2);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 5px; margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label><span class="label label-danger">FINANCE contact</span> Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtFinance_ContactName" runat="server" class="form-control required" TabIndex="51"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Email id
                                                    <asp:RegularExpressionValidator ID="revFinance_Email" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true"
                                                        Display="Dynamic" ControlToValidate="txtFinance_Email" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtFinance_Email" runat="server" TabIndex="52" CssClass="form-control required"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Mobile No
                                                     <asp:RegularExpressionValidator ID="revFinance_MobileNo" runat="server" ControlToValidate="txtFinance_MobileNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtFinance_MobileNo" runat="server" class="form-control" TabIndex="53"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px;">
                                                    <label>
                                                        Landline No
                                                     <asp:RegularExpressionValidator ID="revFinance_Landline" runat="server" ControlToValidate="txtFinance_LandlineNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtFinance_LandlineNo" runat="server" class="form-control" TabIndex="54"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 2px">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <asp:CheckBox ID="chkBillingOther" runat="server" ToolTip="Include same as above billing details" onchange="return GetBillingDetails(3);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 5px;">
                                            <div class="form-group">
                                                <div class="col-sm-3 col-md-3" style="padding-right: 2px">
                                                    <label><span class="label label-danger">OTHERS contact</span> Name</label>
                                                    <div>
                                                        <asp:TextBox ID="txtOther_ContactName" runat="server" class="form-control" TabIndex="55"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Email id
                                                    <asp:RegularExpressionValidator ID="revOther_Email" runat="server" ErrorMessage="(Invalid)" ForeColor="#ff3300" SetFocusOnError="true"
                                                        Display="Dynamic" ControlToValidate="txtOther_Email" ValidationExpression="^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOther_Email" runat="server" TabIndex="56" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px; padding-right: 2px">
                                                    <label>
                                                        Mobile No
                                                     <asp:RegularExpressionValidator ID="revOther_MobileNo" runat="server" ControlToValidate="txtOther_MobileNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOther_MobileNo" runat="server" class="form-control" TabIndex="57"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3 col-md-3" style="padding-left: 2px;">
                                                    <label>
                                                        Landline No
                                                     <asp:RegularExpressionValidator ID="revOther_Landline" runat="server" ControlToValidate="txtOther_LandlineNo" SetFocusOnError="true"
                                                         Display="Dynamic" ValidationExpression="^\d{5,}$" ErrorMessage="(Invalid)" ForeColor="#ff3300"></asp:RegularExpressionValidator>
                                                    </label>
                                                    <div>
                                                        <asp:TextBox ID="txtOther_LandlineNo" runat="server" class="form-control" TabIndex="58"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="f1-buttons">
                                            <button type="button" tabindex="59" class="btn btn-previous" style="background-color: #f35b3f; color: white">
                                                Previous</button>
                                            <button type="button" tabindex="60" id="btnBillingNext" class="btn btn-next" style="background-color: #f35b3f; color: white" onclick="return ValidateBilling();">
                                                Next</button>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset id="Others">
                                    <div id="dvOthers">
                                        <div class="row" style="padding-top: 2px;" id="dvMaterialService">
                                            <div class="form-group">
                                                <div class="col-sm-12 col-md-12">
                                                    <asp:UpdatePanel ID="upnlMaterialService" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-sm-2 col-md-2">
                                                                        <asp:Button ID="btnAddMaterial" runat="server" Text="ADD MORE MATERIAL ROWS" OnClick="btnAddMaterial_OnClick" CssClass="btn btn-xs btn-3d btn-primary" />
                                                                    </div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                </div>
                                                            </div>
                                                            <asp:GridView ID="gvMaterial" CssClass="table" BorderStyle="None" TabIndex="61" runat="server" AutoGenerateColumns="false" HeaderStyle-ForeColor="White" RowStyle-ForeColor="White">
                                                                <Columns>
                                                                    <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="thheader" />
                                                                    <asp:TemplateField HeaderText="Material Supplied" ItemStyle-Width="40%" HeaderStyle-CssClass="thheader">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtMaterialSupplied" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Commodity Name" ItemStyle-Width="40%" HeaderStyle-CssClass="thheader">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCommodityName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HSN Code" ItemStyle-Width="15%" HeaderStyle-CssClass="thheader">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtHSNCode" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-sm-2 col-md-2">
                                                                        <asp:Button ID="btnAddServices" runat="server" Text="ADD MORE SERVICE ROWS" OnClick="btnAddServices_OnClick" CssClass="btn btn-xs btn-3d btn-primary" />
                                                                    </div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                    <div class="col-sm-2 col-md-2"></div>
                                                                </div>
                                                            </div>
                                                            <asp:GridView ID="gvServices" CssClass="table" TabIndex="62" BorderStyle="None" runat="server" AutoGenerateColumns="false" HeaderStyle-ForeColor="White" RowStyle-ForeColor="White">
                                                                <Columns>
                                                                    <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="thheader" />
                                                                    <asp:TemplateField HeaderText="Service Provided" ItemStyle-Width="40%" HeaderStyle-CssClass="thheader">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtServiceProvided" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Service Category" ItemStyle-Width="40%" HeaderStyle-CssClass="thheader">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtServiceCatg" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC Code" ItemStyle-Width="15%" HeaderStyle-CssClass="thheader">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtSACCode" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="background-color: rgba(2, 2, 2, 0.54)">
                                            <div class="form-group">
                                                <div class="col-sm-12 col-md-12">
                                                    <label>PAN Copy</label>
                                                    <div class="fancy-file-upload fancy-file-primary">
                                                        <asp:FileUpload ID="fuPanCopy" runat="server" CssClass="form-control" TabIndex="63" Width="50%" onchange="jQuery(this).next('input').val(this.value);" />
                                                        <input type="text" class="form-control" placeholder="no file selected" readonly="" style="color: white; background-color: #ccccc; width: 50%" />
                                                        <span class="button">Upload Copy</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="background-color: rgba(2, 2, 2, 0.54)">
                                            <div class="form-group">
                                                <div class="col-sm-12 col-md-12">
                                                    <label>IEC Copy</label>
                                                    <div class="fancy-file-upload fancy-file-primary">
                                                        <asp:FileUpload ID="fuIECCopy" runat="server" CssClass="form-control" TabIndex="64" Width="50%" onchange="jQuery(this).next('input').val(this.value);" />
                                                        <input type="text" class="form-control" placeholder="no file selected" readonly="" style="color: white; background-color: #ccccc; width: 50%" />
                                                        <span class="button">Upload Copy</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="background-color: rgba(2, 2, 2, 0.54)">
                                            <div class="form-group">
                                                <div class="col-sm-12 col-md-12">
                                                    <label>Other Copy</label>
                                                    <div class="fancy-file-upload fancy-file-primary">
                                                        <asp:FileUpload ID="fuOtherCopy" runat="server" CssClass="form-control" TabIndex="65" Width="50%" onchange="jQuery(this).next('input').val(this.value);" />
                                                        <input type="text" class="form-control" placeholder="no file selected" readonly="" style="color: white; background-color: #ccccc; width: 50%" />
                                                        <span class="button">Upload Copy</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="f1-buttons">
                                            <button type="button" tabindex="66" class="btn btn-previous" style="background-color: #f35b3f; color: white">
                                                Previous</button>
                                            <asp:Button ID="btnSaveKYC" runat="server" TabIndex="67" CssClass="btn btn-primary" BackColor="#f35b3f" ForeColor="White" Text="Print KYC"
                                                OnClientClick="javascript: return ValidateForm();" OnClick="btnSaveKYC_Click" />
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
                            <asp:SqlDataSource ID="DataSourceConstitutionMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetConstitutionMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceSectorMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetVarientMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceNatureOfBusinessMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetNatureofBusinessMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceManagersMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetUserByManagers" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceStateBranchGst" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="KYC_GetStateBranchOfGst" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
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

    <script type="text/javascript" lang="javascript">
        function AddKYCType() {
            var KYCType = document.getElementById("ddlGeneral_KYCType").value;
            if (KYCType == "2") // overseas customer
            {
                document.getElementById("ddlGeneral_State").value = "468";  // selected other countries row
                document.getElementById("ddlGeneral_State").disabled = true;
            }
            else {
                document.getElementById("ddlGeneral_State").value = "0";  // by default row
                document.getElementById("ddlGeneral_State").disabled = false;
            }
        }

        function ValidateGeneral() {
            document.getElementById("ChangeFieldset").value = "2";
            emailExp = /^[a-zA-Z0-9][-a-zA-Z0-9._]+@([-a-zA-Z0-9]+[.])+[a-z]{2,5}$/; // to validate email id
            var EmailId = document.getElementById("txtGeneral_Email").value;
            if (EmailId != '') {
                if (!EmailId.match(emailExp)) {
                    alert("Invalid Email Id");
                    document.getElementById("txtGeneral_Email").focus();
                    document.getElementById("ChangeFieldset").value = "1";
                    return false;
                }
            }

            var Type = document.getElementById("ddlGeneral_KYCType").value;
            if (Type == "2") // overseas customer
            {
                document.getElementById("dvCompany_Overseas").style.display = "none";
                document.getElementById("title_GST").innerHTML = "Billing/Contact";
                document.getElementById("txtPANNo").setAttribute("class", "form-control");
                document.getElementById("txtIFSCCode").setAttribute("class", "form-control");
                document.getElementById("txtTANNo").setAttribute("class", "form-control");
            }
            else {
                document.getElementById("dvCompany_Overseas").style.display = "block";
                document.getElementById("title_GST").innerHTML = "GST";
                document.getElementById("txtPANNo").setAttribute("class", "form-control required");
                document.getElementById("txtIFSCCode").setAttribute("class", "form-control");
                document.getElementById("txtTANNo").setAttribute("class", "form-control required");
            }
        }
        function ValidateCompany() {
            document.getElementById("ChangeFieldset").value = "2";
            var PANNo = document.getElementById("txtPANNo").value;
            var VATNo = document.getElementById("txtVATNo").value;
            if (PANNo != '') {
                var PANNoExp = /[A-Z a-z]{5}\d{4}[A-Z a-z]{1}/;// to validate PAN No
                if (!PANNo.match(PANNoExp)) {
                    alert("Invalid PAN No.");
                    document.getElementById("txtPANNo").focus();
                    document.getElementById("ChangeFieldset").value = "1";
                    return false;
                }
            }
            if (VATNo != '') {
                var VATNoExp = /[A-Z]{2}[A-Z a-z 0-9]{2,13}/; // to validate VAT No        
                if (!VATNo.match(VATNoExp)) {
                    alert("Invalid VAT No.");
                    document.getElementById("txtVATNo").focus();
                    document.getElementById("ChangeFieldset").value = "1";
                    return false;
                }
            }
            var Type = document.getElementById("ddlGeneral_KYCType").value;
            if (Type == "2") // overseas customer
            {
                document.getElementById("dvCompany_Overseas").style.display = "none";
                document.getElementById("dvOverseas").style.display = "block";
                document.getElementById("dvGSTDetails").style.display = "none";         // remove gst section
                document.getElementById("dvBillingContact").style.display = "none";     // remove billing operation section
                document.getElementById("dvOthers").style.display = "none";             // remove other section
                document.getElementById("step_BillingContact").style.display = "none";  // remove billing/contact step
                document.getElementById("step_Other").style.display = "none";           // remove other step
            }
            else {
                document.getElementById("txtOversea_BillingName").removeAttribute("required");
                document.getElementById("txtOversea_BillingEmail").removeAttribute("required");
                document.getElementById("txtOversea_BillingAddress").removeAttribute("required");
                document.getElementById("txtOversea_OperationContact").removeAttribute("required");
                document.getElementById("txtOversea_OperationEmail").removeAttribute("required");
                document.getElementById("txtOversea_OperationMobile").removeAttribute("required");
                document.getElementById("txtOversea_FinanceContact").removeAttribute("required");
                document.getElementById("txtOversea_FinanceEmail").removeAttribute("required");

                document.getElementById("dvCompany_Overseas").style.display = "block";
                document.getElementById("dvOverseas").style.display = "none";
                document.getElementById("dvGSTDetails").style.display = "block";         // remove gst section
                document.getElementById("dvBillingContact").style.display = "block";     // remove billing operation section
                document.getElementById("dvOthers").style.display = "block";             // remove other section
                document.getElementById("step_BillingContact").style.display = "block";  // remove billing/contact step
                document.getElementById("step_Other").style.display = "block";           // remove other step
            }
        }
        function ValidateBilling() {
            document.getElementById("ChangeFieldset").value = "2";
            emailExp = /^[a-zA-Z0-9][-a-zA-Z0-9._]+@([-a-zA-Z0-9]+[.])+[a-z]{2,5}$/; // to validate email id
            var EmailId = document.getElementById("txtBilling_Email").value;
            if (EmailId != '') {
                if (!EmailId.match(emailExp)) {
                    alert("Invalid Email Id");
                    document.getElementById("txtBilling_Email").focus();
                    document.getElementById("ChangeFieldset").value = "1";
                    return false;
                }
                else {
                    var KYCType = document.getElementById("ddlGeneral_KYCType").value;
                    var MaterialService = document.getElementById("dvMaterialService");
                    if (KYCType == 0) {
                        MaterialService.style.display = "block";
                    }
                    else {
                        MaterialService.style.display = "none";
                    }
                }
            }
        }
        function ValidateGST(txtControl, number) {
            var Control = document.getElementById(txtControl).value;
            document.getElementById("btnGSTNext").disabled = false;
            document.getElementById("ChangeFieldset").value = "2";
            if (number != '') {
                if (number == 1) {
                    document.getElementById(txtControl).setAttribute("title", "Enter valid email id.");
                    MatchExp = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([com\co\.\in\net])+$/; // to validate email id
                }
                else if (number == 2) {
                    document.getElementById(txtControl).setAttribute("title", "Enter valid GST Provision ID No.");
                    MatchExp = /[0-9]{2}[A-Z a-z]{5}\d{4}[A-Z a-z]{1}\d{1}[Z]{1}[A-Z a-z 0-9]{1}/; // to validate gst provision id no
                }
                else if (number == 3) {
                    document.getElementById(txtControl).setAttribute("title", "Enter valid 15 digit ARN No.");
                    MatchExp = /[0-9]{15}/; // to validate gst arn no
                }
                if (Control != '') {
                    if (!Control.match(MatchExp)) {
                        document.getElementById("ChangeFieldset").value = "1";
                        document.getElementById("btnGSTNext").disabled = true;
                        return false;
                    }
                }
            }
        }
        function GSTRequireFields(txtGSTName, ddlBranch, txtAddress, txtPerson, txtNumber, txtGSTNo) {
            var GSTName = document.getElementById(txtGSTName).value;
            if (GSTName != '') {
                document.getElementById(ddlBranch).setAttribute("class", "form-control required");
                document.getElementById(txtAddress).setAttribute("class", "form-control required");
                document.getElementById(txtPerson).setAttribute("class", "form-control required");
                document.getElementById(txtNumber).setAttribute("class", "form-control required");
                document.getElementById(txtGSTNo).setAttribute("class", "form-control required");
            }
        }
        function GetBillingDetails(value) {
            var name = document.getElementById("txtBilling_Name").value;
            var email = document.getElementById("txtBilling_Email").value;
            var mobileno = document.getElementById("txtBilling_MobileNo").value;

            if (value == 1) {
                if (document.getElementById('chkBillingOperation').checked) {
                    document.getElementById("txtOperation_ContactName").value = name;
                    document.getElementById("txtOperation_Email").value = email;
                    document.getElementById("txtOperation_MobileNo").value = mobileno;
                }
            }
            else if (value == 2) {
                if (document.getElementById('chkBillingFinance').checked) {
                    document.getElementById("txtFinance_ContactName").value = name;
                    document.getElementById("txtFinance_Email").value = email;
                    document.getElementById("txtFinance_MobileNo").value = mobileno;
                }
            }
            else if (value == 3) {
                if (document.getElementById('chkBillingOther').checked) {
                    document.getElementById("txtOther_ContactName").value = name;
                    document.getElementById("txtOther_Email").value = email;
                    document.getElementById("txtOther_MobileNo").value = mobileno;
                }
            }
        }
        function GetOversea_BillingDetails(value) {
            var name = document.getElementById("txtOversea_BillingName").value;
            var email = document.getElementById("txtOversea_BillingEmail").value;
            var mobileno = document.getElementById("txtOversea_BillingMobile").value;

            if (value == 1) {
                if (document.getElementById('chkOversea_BillingOperation').checked) {
                    document.getElementById("txtOversea_OperationContact").value = name;
                    document.getElementById("txtOversea_OperationEmail").value = email;
                    document.getElementById("txtOversea_OperationMobile").value = mobileno;
                }
            }
            else if (value == 2) {
                if (document.getElementById('chkOversea_BillingFinance').checked) {
                    document.getElementById("txtOversea_FinanceContact").value = name;
                    document.getElementById("txtOversea_FinanceEmail").value = email;
                    document.getElementById("txtOversea_FinanceMobile").value = mobileno;
                }
            }
            else if (value == 3) {
                if (document.getElementById('chkOversea_Other').checked) {
                    document.getElementById("txtOversea_OtherContact").value = name;
                    document.getElementById("txtOversea_OtherEmail").value = email;
                    document.getElementById("txtOversea_OtherMobile").value = mobileno;
                }
            }
        }
        function ValidateForm() {
            var KYCType = document.getElementById("ddlGeneral_KYCType").value;
            var MaterialRow1, MaterialRow2, MaterialRow3, ServiceRow1, ServiceRow2, ServiceRow3, count;
            var tblMaterial = document.getElementById('<%=gvMaterial.ClientID %>');
            var tblServices = document.getElementById('<%=gvServices.ClientID %>');
            MaterialRow1 = tblMaterial.rows[1].cells[1].getElementsByTagName("input")[0].value;
            MaterialRow2 = tblMaterial.rows[1].cells[2].getElementsByTagName("input")[0].value;
            MaterialRow3 = tblMaterial.rows[1].cells[3].getElementsByTagName("input")[0].value;
            ServiceRow1 = tblServices.rows[1].cells[1].getElementsByTagName("input")[0].value;
            ServiceRow2 = tblServices.rows[1].cells[2].getElementsByTagName("input")[0].value;
            ServiceRow3 = tblServices.rows[1].cells[3].getElementsByTagName("input")[0].value;

            if (KYCType == 0) {
                if (MaterialRow1 == '' || MaterialRow2 == '' || MaterialRow3 == '') {
                    alert('Enter alteast one record for material.');
                    document.getElementById(tblMaterial.rows[1].cells[1].getElementsByTagName("input")[0].id).setAttribute("class", "form-control required");
                    document.getElementById(tblMaterial.rows[1].cells[2].getElementsByTagName("input")[0].id).setAttribute("class", "form-control required");
                    document.getElementById(tblMaterial.rows[1].cells[3].getElementsByTagName("input")[0].id).setAttribute("class", "form-control required");
                    return false;
                }

                if (ServiceRow1 == '' || ServiceRow2 == '' || ServiceRow3 == '') {
                    alert('Enter alteast one record for service.');
                    document.getElementById(tblServices.rows[1].cells[1].getElementsByTagName("input")[0].id).setAttribute("class", "form-control required");
                    document.getElementById(tblServices.rows[1].cells[2].getElementsByTagName("input")[0].id).setAttribute("class", "form-control required");
                    document.getElementById(tblServices.rows[1].cells[3].getElementsByTagName("input")[0].id).setAttribute("class", "form-control required");
                    return false;
                }
            }

            if (document.getElementById('<%=fuPanCopy.ClientID %>').value == '') {
                alert('Please upload PAN Copy!!');
                return false;
            }

            if (document.getElementById('<%=fuIECCopy.ClientID %>').value == '') {
                alert('Please upload IEC Copy!!');
                return false;
            }
        }
    </script>
</body>
</html>

<%@ Page Title="SEZ Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SEZDetail.aspx.cs"
    Inherits="SEZ_SEZDetail" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            background-color: #000032;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 1px;
            padding-left: 0px;
            width: 700px;
            /*height: 155px;*/
        }

        .RBL label {
            display: block;
        }

        .RBL td {
            text-align: center;
            /*width: 200px;*/
        }

        .text {
            font-weight: bold;
        }

        .text1 {
            font-weight: bold;
        }

        textarea, input, select {
            background-color: #FFF1E0;
        }

        .fieldset {
            /*background-color:#F5DFE2;*/
        }

        .grid th {
            background-color: darkslateblue;
            color: #ffffff;
            height: 5px;
        }

        .grid tr {
            height: 20px;
            border: 2px solid #ccc;
        }

        .grid td {
            padding-left: 10px;
            border: 2px solid #ccc;
        }


        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.5;
            filter: alpha(opacity=80);
            moz-opacity: 0.5;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            /*width: 180px;
            height: 70px;*/
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }

        .loading1 {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">

        function OnCustomerSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnCustId.ClientID%>').value = results.ClientId;
        }
        $addHandler
        (
            $get('txtCustomerName'), 'keyup',

            function () {
                $get('<%=txtClientName.ClientID%>').value = '0';
            }
        );

    </script>
    <script type="text/javascript">

        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function () {
            //Page_ClientValidate();
            if (Page_IsValid) {

                ShowProgress();
            }

        });

    </script>

    <script type="text/javascript">
        function SetButtonStatus(sender, target) {
            Page_ClientValidate();
            if (!Page_IsValid)


                document.getElementById(target).disabled = true;

            else
                document.getElementById(target).disabled = false;

        }
    </script>


    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="ContainerReq" CssClass="errorMsg" EnableViewState="false" />


    <asp:Panel ID="panSez" runat="server">
        <fieldset class="fieldset">
            <legend>SEZ Type</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        <center>
                            <asp:RadioButtonList ID="rdbSEZtype" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table" Width="400px" AutoPostBack="true" class="text" OnSelectedIndexChanged="rdbSEZtype_SelectedIndexChanged">
                                <asp:ListItem Text="Inward" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Outward" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>

                        </center>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="ImportDSRExcel" runat="server">
            <legend>Import Excel File</legend>

            <table border="0" cellpadding="0" cellspacing="0" width="65%" bgcolor="white">
                <tr>
                    <td><b>Select Customer : </b></td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlCustomer" runat="server" Width="550px" DataSourceID="CustomerSqlDataSource" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                            DataTextField="CustName" DataValueField="lid" AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <%-- <asp:ListItem Text="USD" Value="1"></asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("CustomerId") %>' />

                      <%--  
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Select Customer" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlCustomer" ValidationGroup="Required">*</asp:RequiredFieldValidator>--%>

                        <asp:SqlDataSource ID="CustomerSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetCustomerDetail" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>

                </tr>
                <tr>
                    <td>
                        <b>Division : </b>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDivision" runat="Server" Width="200px" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"
                            AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Select Division" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlDivision" ValidationGroup="Required">*</asp:RequiredFieldValidator>--%>

                        <asp:HiddenField ID="hdnDivisionId" runat="server" Value='<%#Eval("DivisionId") %>' />

                    </td>
                    <td>
                        <b>Plant : </b>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPlant" runat="Server" Width="200px" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please Select Plant" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlPlant" ValidationGroup="Required">*</asp:RequiredFieldValidator>--%>

                        <asp:HiddenField ID="hdnPlantId" runat="server" Value='<%#Eval("PlantId") %>' />
                    </td>
                </tr>

                <tr>
                    <td>
                        <b>Request Type : </b>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRequestType" runat="Server" Width="200px" AppendDataBoundItems="true" DataSourceID="RequestTypeSqlDataSource"
                            DataTextField="ReqTypeName" DataValueField="lid">
                            <%--<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="RequestTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetRequestType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Select Division" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlDivision" ValidationGroup="Required">*</asp:RequiredFieldValidator>--%>

                        <asp:HiddenField ID="hdnRequestTypeId" runat="server" Value='<%#Eval("DivisionId") %>' />

                    </td>
                    <td>
                      
                    </td>
                    <td>
                       
                    </td>
                </tr>

                <tr>
                    <td>
                        <b>Upload DSR File :  </b>
                    </td>
                    <td>
                        <asp:FileUpload ID="FUDSRImport" runat="server" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnUploadDSR" runat="server" Text="Upload" OnClick="btnUploadDSR_Click" />
                    </td>

                </tr>
            </table>
        </fieldset>


        <fieldset id="filldsetbasic" runat="server" class="fieldset">
            <legend>
                <asp:Label ID="lblTitle" runat="server"></asp:Label></legend>
            <div class="m clear">
                <asp:Button ID="btnSubmit" Text="Save" runat="server" ValidationGroup="Required" OnClick="btnSave_Click" />
                <asp:Button ID="btnNew" Text="NEW" CausesValidation="false" runat="server" OnClick="btnNew_Click" Visible="false" />
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">

                <tr>
                     <td>
                        <b>Request Type : </b>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIndiviReqType" runat="Server" Width="200px" AppendDataBoundItems="true" DataSourceID="IndiviRequestTypeSqlDataSource"
                            DataTextField="ReqTypeName" DataValueField="lid" OnSelectedIndexChanged = "ddlRequestType_OnSelectedIndexChanged" AutoPostBack = "true">
                            <%--<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator ID="RFVJobType" ValidationGroup="Required" runat="server" SetFocusOnError="true"
                            Display="dynamic" ControlToValidate="ddlIndiviReqType" InitialValue="0" ErrorMessage="Please Select Request Type"
                            Text="*"></asp:RequiredFieldValidator>

                        <asp:SqlDataSource ID="IndiviRequestTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetRequestType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Select Division" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlDivision" ValidationGroup="Required">*</asp:RequiredFieldValidator>--%>                       
                    </td>
                </tr>
                <tr>
                    <td>BS Job Number
                                          
                       <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ControlToValidate="txtJobNo"
                           SetFocusOnError="true" ErrorMessage="Please Enter BS Job Number" Display="Dynamic"
                           Text="*" ValidationGroup="Required" InitialValue=""></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtJobNo" runat="server" MaxLength="18" placeholder="SZ00001/DSIB/17-18" ReadOnly="true"></asp:TextBox>
                    </td>

                    <td>Customer Name
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtClientName" runat="server" placeholder="Customer Name" Width="300px"
                            OnTextChanged="txtClientName_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:HiddenField ID="hdnCustId" runat="server" />

                        <asp:RequiredFieldValidator ID="RFVCustomer" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtClientName" ErrorMessage="Please Select Customer" ForeColor="red">*
                        </asp:RequiredFieldValidator>

                        <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" TargetControlID="txtClientName"
                            CompletionListElementID="divwidthCust1" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust1"
                            ContextKey="4329" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td><%--Babaji Branch

                        <asp:RequiredFieldValidator ID="RFVBabajiBranch" ValidationGroup="Required" runat="server"
                            Display="Dynamic" ControlToValidate="ddBabajiBranch" InitialValue="0" ErrorMessage="Please Select Babaji Branch"
                            Text="*"></asp:RequiredFieldValidator>--%>
                    </td>
                    <%-- <td>
                        <asp:DropDownList ID="ddBabajiBranch" runat="server">
                        </asp:DropDownList>
                    </td>--%>
                </tr>

                <tr>

                    <td>
                        Division
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDivisionInd" runat="Server" Width="200px" OnSelectedIndexChanged="ddlDivisionInd_SelectedIndexChanged"
                            AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                         <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please Select Division" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlDivisionInd" ValidationGroup="Required">*</asp:RequiredFieldValidator>

                        <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("DivisionId") %>' />

                    </td>
                    <td>
                        Plant
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPlantInd" runat="Server" Width="200px" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                          <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please Select Plant" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlPlantInd" ValidationGroup="Required">*

                          </asp:RequiredFieldValidator>

                        <asp:HiddenField ID="HiddenField3" runat="server" Value='<%#Eval("PlantId") %>' />
                    </td>
                   <td>Mode
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true"></asp:DropDownList>--%>
                        <%--OnSelectedIndexChanged="ddlMode_SelectedIndexChanged"--%>

                        <asp:DropDownList ID="ddlMode" runat="server" Width="100px" DataSourceID="ModeSqlDataSource"
                            DataTextField="TransMode" DataValueField="lid" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>                        
                        </asp:DropDownList>

                    <%--    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Please Select Mode" Text="*" ForeColor="red"
                            InitialValue="0" ControlToValidate="ddlMode" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>


                        <asp:SqlDataSource ID="ModeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetTransModeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                    </td>

                </tr>


                    <tr>
                     <td>Request ID
                    </td>
                    <td>
                        <asp:TextBox ID="txtRequestId" runat="server"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtRequestId" ErrorMessage="Enter The RequestId" ForeColor="red">*
                        </asp:RequiredFieldValidator>
                    </td>

                    <td>BE No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtBeNo" runat="server"></asp:TextBox>
                    </td>
                    <td>BE Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtBEDate" runat="server"></asp:TextBox>
                        <asp:Image ID="imgBEDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        <asp:CalendarExtender ID="CalBEDate" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBEDate" PopupPosition="BottomRight"
                            TargetControlID="txtBEDate">
                        </asp:CalendarExtender>

                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtBEDate" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>

                        <%--<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtBEDate" ErrorMessage="Invalid BE Date."
                              Display="Dynamic" Type="Date" Text="*" CultureInvariantValues="false" Operator="DataTypeCheck"
                              ValidationGroup="JobRequired"></asp:CompareValidator>--%>
                    </td>
                   
                </tr>


                       <tr>
                    <%--  <td>Invoice No</td>
                    <td>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" ></asp:TextBox>
                    </td>
                    <td>
                        Invoice Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtInvoiceDate" runat="server"></asp:TextBox>
                        <asp:Image ID="imgInvoice" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                          <asp:CalendarExtender ID="calInvoice" runat="server" Enabled="True" EnableViewState="False"
                              FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvoice" PopupPosition="BottomRight"
                              TargetControlID="txtInvoiceDate">
                          </asp:CalendarExtender>
                       
                    </td>--%>
                    <td>Assesable Value
                    </td>
                    <td>
                        <asp:TextBox ID="txtAssesValue" runat="server"></asp:TextBox>

                        <asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                            ErrorMessage="Please enter Assesable Value in integer or decimal number with 4 decimal places." ForeColor="Red"
                            ControlToValidate="txtAssesValue" ValidationGroup="Required">*
                        </asp:RegularExpressionValidator>

<%--                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtAssesValue" ErrorMessage="Enter the Assessable Value" ForeColor="red">*
                        </asp:RequiredFieldValidator>--%>

                    </td>



                    <td>Ex Rate
                    </td>
                    <td>
                        <asp:TextBox ID="txtExRate" runat="server"></asp:TextBox>

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                            ErrorMessage="Please enter EX Rate in integer or decimal number with 4 decimal places." ForeColor="Red"
                            ControlToValidate="txtExRate" ValidationGroup="Required">*
                        </asp:RegularExpressionValidator>

<%--                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtExRate" ErrorMessage="Invalid EX-Rate" ForeColor="red">*
                        </asp:RequiredFieldValidator>--%>
                    </td>   

                    <td>Currency
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlCurrency" runat="server" ></asp:DropDownList>--%>
                        <%--OnSelectedIndexChanged="ddDivision_SelectedIndexChanged"--%>

                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="200px" DataSourceID="CurrencySqlDataSource"
                            DataTextField="Currency" DataValueField="lid" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <%-- <asp:ListItem Text="USD" Value="1"></asp:ListItem>--%>
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="CurrencySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                </tr>



                <%--<tr>

                    <td>Customer Name

                        <asp:RequiredFieldValidator ID="RFVCustomer" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="ddCustomer" InitialValue="0" ErrorMessage="Please Select Customer"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddCustomer" runat="server" AutoPostBack="true">
                           
                        </asp:DropDownList>
                    </td>

                    <td>Job Type

                        <asp:RequiredFieldValidator ID="RFVJobType" ValidationGroup="Required" runat="server" SetFocusOnError="true"
                            Display="dynamic" ControlToValidate="ddJobType" InitialValue="0" ErrorMessage="Please Select Job Type"
                            Text="*"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddJobType" runat="server" AutoPostBack="true">
                          
                        </asp:DropDownList>
                    </td>


                </tr>
                <tr>
                    <td>Consignee Name
                    </td>
                    <td>
                        <asp:Label ID="txtConsignee" runat="server" Text=""></asp:Label>


                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Customer Division

                         <asp:RequiredFieldValidator ID="RFVCustDivis" ValidationGroup="Required" runat="server" Display="Dynamic"
                             Text="*" ControlToValidate="ddDivision" InitialValue="0" ErrorMessage="Please Select Customer Division">
                         </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true">
                          
                        </asp:DropDownList>

                    </td>
                    <td>Customer Plant

                        <asp:RequiredFieldValidator ID="RFVPlant" ValidationGroup="Required" runat="server"
                            Display="Dynamic" ControlToValidate="ddPlant" InitialValue="0" ErrorMessage="Please Select Customer Plant"
                            Text="*"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddPlant" runat="server"></asp:DropDownList>

                    </td>
                </tr>--%>
              <%--  <tr>
                   
                   
                    <td>Person
                    </td>
                    <td>
                        <asp:TextBox ID="txtPerson" runat="server" placeholder="Person Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtPerson" ErrorMessage="Person Name Missing" ForeColor="red">*
                        </asp:RequiredFieldValidator>

                    </td>                 

                </tr>--%>

         

                <%-- <tr>

                    <td>
                        Description & Qty
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtDescription" runat="server" textmode="multiline" Width="495px"></asp:TextBox>
                    </td> 

                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>

                   
                   
                </tr>--%>

                <%-- <tr>
                    <td>
                        Invoice Value
                    </td>    
                    <td>
                        <asp:TextBox ID="txtInvoiceValue" runat="server"></asp:TextBox>
                    </td> 
                    <td>
                        Term
                    </td>
                    <td>
                       
                        <asp:DropDownList ID="ddlTerm" runat="server" Width="100px">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="CIF" Value="1"></asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>

                </tr>--%>

                <%---------  Outward ----------%>

                <%--  <tr>
                    <td colspan="4">
                        
                      <table id="tblOutward" width="90%">--%>
                <tr id="trOutwardBE" runat="server">

                    <td>
                        <asp:Label ID="lblInwardBENo" runat="Server" Text="Inward BE NO."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInwardBENo" runat="server"></asp:TextBox>
                    </td>
                    <%-- <td>                                    
                                    <asp:label ID="lblInwardJobNumber" runat="Server" Text="Inward Job NO."></asp:label>                                  
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInwardJobNo" runat="server"></asp:TextBox>
                                </td>--%>

                    <td>
                        <asp:Label ID="lblInJobNo" runat="Server" Text="Inward Job No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInwardJobNo" runat="Server" OnTextChanged="txtInwardJobNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblInwardBEDate" runat="Server" Text="Inward BE Date."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInwardBEDate" runat="server"></asp:TextBox>
                        <asp:Image ID="imgInwardBEDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInwardBEDate" PopupPosition="BottomRight"
                            TargetControlID="txtInwardBEDate">
                        </asp:CalendarExtender>

                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtInwardBEDate" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>

                    </td>
                </tr>
             <%--   <tr id="trOutwardDays" runat="server">

                    <td>
                        <asp:Label ID="lblDaysStore" runat="Server" Text="Days Store"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDaysStore" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ControlToValidate="txtDaysStore" ErrorMessage=" Days Only numeric allowed." ForeColor="Red"
                            ValidationExpression="^[0-9]*$" ValidationGroup="Required">Invalid Number
                        </asp:RegularExpressionValidator>

                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>

                </tr>--%>
                <%--      </table>
                    </td>

                </tr>--%>

                <%------- END  Outward --------%>
            
                <%--<tr>
                    <td>Duty Amount(INR)
                    </td>
                    <td>
                        <asp:TextBox ID="txtDutyAmnt" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                            ErrorMessage="Please enter Duty in integer or decimal number with 4 decimal places." ForeColor="Red"
                            ControlToValidate="txtDutyAmnt" ValidationGroup="Required">*
                        </asp:RegularExpressionValidator>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtDutyAmnt" ErrorMessage="Enter The Duty Amount" ForeColor="red">*
                        </asp:RequiredFieldValidator>

                    </td>
                    <td>Duty As Per Customs</td>
                    <td>
                        <asp:DropDownList ID="ddlDutyCustom" runat="server" Width="160px">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Forgone(F)" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Payable(P)" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td>Inward Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtInwardDate" runat="server"></asp:TextBox>
                        <asp:Image ID="imgInward" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        <asp:CalendarExtender ID="CalInward" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInward" PopupPosition="BottomRight"
                            TargetControlID="txtInwardDate">
                        </asp:CalendarExtender>

                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtInwardDate" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>
                    
                    </td>
                </tr>--%>

                  <%--  <tr>
                <td>No. of Packages</td>
                    <td>
                        <asp:TextBox ID="txtPackages" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ControlToValidate="txtPackages" ErrorMessage=" Packages numeric allowed." ForeColor="Red"
                            ValidationExpression="^[0-9]*$" ValidationGroup="Required">*
                        </asp:RegularExpressionValidator>

                    </td>

                    <td>Packages Unit</td>
                    <td>
                        <asp:DropDownList ID="ddlPackagesUnit" runat="server" Width="160px" DataSourceID="PackagesSqlDataSource"
                            DataTextField="sName" DataValueField="lid" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="PackagesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPackageType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>                  

                </tr>--%>

          <%--      <tr>
                    <td>Gross Weight (kgs)
                    </td>
                    <td>
                        <asp:TextBox ID="txtGrossWt" runat="server"></asp:TextBox>                    

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                            ErrorMessage="Please enter Duty in integer or decimal number with 4 decimal places." ForeColor="Red"
                            ControlToValidate="txtGrossWt" ValidationGroup="Required">*
                        </asp:RegularExpressionValidator>


                    </td>
                    <td>No.of Vehicles
                    </td>
                    <td>
                        <asp:TextBox ID="txtVehicles" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                            ControlToValidate="txtVehicles" ErrorMessage=" Vehicle numeric allowed." ForeColor="Red"
                            ValidationExpression="^[0-9]*$" ValidationGroup="Required">*
                        </asp:RegularExpressionValidator>
                    </td>
                    <td>BE Type
                    </td>
                    <td>                 

                        <asp:DropDownList ID="ddlBEType" runat="server" Width="160px">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Import" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Re-Export" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Home Consumption" Value="3"></asp:ListItem>
                        </asp:DropDownList>

                    </td>
                </tr>

                <tr>

                    <td>Add Buyer</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAddBuyer" runat="server" Width="520px"></asp:TextBox>
                    </td>
                    <td>CHA Code</td>
                    <td>
                        <asp:TextBox ID="txtCHACode" runat="server" Text="11/262"></asp:TextBox></td>

                </tr>--%>

                <tr id="trCIF" runat="server">
                    <td>CIF Value
                    </td>
                    <td>
                        <asp:TextBox ID="txtCIFValue" runat="server"></asp:TextBox>

                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="Required" runat="server" Display="Dynamic"
                                              Text="*" ControlToValidate="txtCIFValue"  ErrorMessage="Enter The RequestId" ForeColor="red">*
                         </asp:RequiredFieldValidator>   --%>
                    </td>         
                    <td>Goods Measurement Unit</td>
                    <td>
                        <asp:DropDownList ID="ddlGrossWt" runat="server" Width="160px" DataSourceID="GrossWtSqlDataSource"
                            DataTextField="UName" DataValueField="Gid" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="GrossWtSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetGrossWtUnitMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                 <%--   <td>Destination</td>
                    <td>
                        <asp:DropDownList ID="ddlDestination" runat="server" Width="160px" DataSourceID="DestinationSqlDataSource"
                            DataTextField="DName" DataValueField="Did" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="DestinationSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetDestinationMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                    <td>Country of Origin</td>
                    <td>
                        <asp:DropDownList ID="ddlCountyOrigin" runat="server" Width="160px" DataSourceID="CountyOriginSqlDataSource"
                            DataTextField="CName" DataValueField="Cid" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="CountyOriginSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetCountryOriginMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>--%>
              
                        
                   <td></td>
                    <td></td>
                </tr>
                <tr id="trDutyAmnt" runat="server">
                      <td>
                          Duty Amount
                         
                      </td>
                      <td>
                          <asp:TextBox ID="txtDutyAmount" runat="server" ></asp:TextBox>

                   
                           <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                              ErrorMessage="Please enter Duty Amount in integer or decimal number with 4 decimal places." ForeColor="Red"
                              ControlToValidate="txtDutyAmount" ValidationGroup="JobRequired">*
                              </asp:RegularExpressionValidator>

                      </td>
                </tr>

                 <tr id="trBOE" runat="server">
                    <td>Supplier Name</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSupplierName" runat="server" placeholder="Supplier Name" width="300px"></asp:TextBox>                      
                    </td>
                </tr>

               <tr id="trDiscountAppl" runat="server">
                    <td>Discount Applicable</td>
                    <td>
                         <asp:RadioButtonList ID="rdlDiscountAppli" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table" Width="150px" class="text" >
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="2" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                    </td>
                    <td>Re-Import</td>
                    <td>
                        <asp:RadioButtonList ID="rdlReImport" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table" Width="150px" class="text" >
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="2" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                    </td>
                    <td>Previous Import</td>
                    <td>
                        <asp:RadioButtonList ID="rdlPreviousImport" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table" Width="150px" class="text" >
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="NO" Value="2" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                    </td>
                </tr>

              
               <tr id="trSB2" runat="server">
                   <td>
                      Buyer Name
                   </td>
                   <td colspan="2">
                        <asp:TextBox ID="txtBuyerName" runat="server" Text='<%# Bind("BuyerName") %>' Width="300px"></asp:TextBox> 
                   </td>
                   <td>
                      Scheme Code
                   </td>
                   <td colspan="2">
                      <asp:TextBox ID="txtSchemeCode" runat="server" Text='<%# Bind("SchemeCode") %>' Width="300px"></asp:TextBox> 
                   </td>                                                    
               </tr>


                <tr id="trSB" runat="server">
                   <td>
                      Previous Export Goods
                   </td>
                   <td>
                        <asp:RadioButtonList ID="rdlPrevExpGoods" runat="server" RepeatDirection="Horizontal"
                               RepeatLayout="Table" Width="150px" class="text" >
                               <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                               <asp:ListItem Text="No" Value="2" ></asp:ListItem>
                           </asp:RadioButtonList>
                       <asp:HiddenField ID="hdnPrevExpGoods" runat="server" Value='<%#Eval("PrevExpGoods") %>' />
                   </td>
                   <td>
                       Cess Details
                   </td>
                   <td>
                       <asp:RadioButtonList ID="rdlCessDetail" runat="server" RepeatDirection="Horizontal"
                               RepeatLayout="Table" Width="150px" class="text" >
                               <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                               <asp:ListItem Text="No" Value="2" ></asp:ListItem>
                           </asp:RadioButtonList>
                       <asp:HiddenField ID="hdnCessDetail" runat="server" Value='<%#Eval("CessDetail") %>' />
                   </td>
                   <td>
                       Licence Registration No
                   </td>
                   <td>
                       <asp:RadioButtonList ID="rdlLicenceRegNo" runat="server" RepeatDirection="Horizontal"
                               RepeatLayout="Table" Width="150px" class="text" >
                               <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                               <asp:ListItem Text="NO" Value="2" ></asp:ListItem>
                           </asp:RadioButtonList>
                       <asp:HiddenField ID="hdnLicenceRegNo" runat="server" Value='<%#Eval("LicenceRegNo") %>' />
                   </td>
               </tr>
                  <tr id="trSB1" runat="server">
                   <td>
                      Re-Export
                   </td>
                   <td>
                        <asp:RadioButtonList ID="rdlReExport" runat="server" RepeatDirection="Horizontal"
                               RepeatLayout="Table" Width="150px" class="text" >
                               <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                               <asp:ListItem Text="No" Value="2" ></asp:ListItem>
                           </asp:RadioButtonList>
                       <asp:HiddenField ID="hdnReExport" runat="server" Value='<%#Eval("ReExport") %>' />
                   </td>
                   <td>
                       Previous Export
                   </td>
                   <td>
                       <asp:RadioButtonList ID="rdlPrevExport" runat="server" RepeatDirection="Horizontal"
                               RepeatLayout="Table" Width="150px" class="text">
                               <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                               <asp:ListItem Text="No" Value="2" ></asp:ListItem>
                           </asp:RadioButtonList>
                       <asp:HiddenField ID="hdnPrevExport" runat="server" Value='<%#Eval("PrevExport") %>' />
                   </td>
                   <td>
                      
                   </td>
                   <td>
                      
                   </td>
               </tr>

                <%--<tr>
                    <td>Place of Origin</td>
                    <td>
                        <asp:DropDownList ID="ddlPlaceOrigin" runat="server" Width="160px" DataSourceID="PlaceOriginSqlDataSource"
                            DataTextField="PName" DataValueField="Pid" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="PlaceOriginSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetPlaceOriginMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>--%>

                <tr>
                    <td>Dispatch Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtOutwardDate" runat="server" onkeyup="SetButtonStatus(this, 'btnSubmit')"></asp:TextBox>
                        <asp:Image ID="imgOutwardDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOutwardDate" PopupPosition="BottomRight"
                            TargetControlID="txtOutwardDate">
                        </asp:CalendarExtender>

                      <%--  <asp:RegularExpressionValidator runat="server" ControlToValidate="txtOutwardDate" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Required" runat="server" Display="Dynamic"
                            Text="*" ControlToValidate="txtOutwardDate" ErrorMessage="Enter The Dispatch Date" ForeColor="red">*
                        </asp:RequiredFieldValidator>--%>


                        <%--<asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtOutwardDate" ErrorMessage="Invalid Inward Date."
                              Display="Dynamic" Type="Date" Text="*" CultureInvariantValues="false" Operator="DataTypeCheck"
                              ValidationGroup="JobRequired"></asp:CompareValidator>--%>
                    </td>
                    <td>PCD From Dahej</td>
                    <td>
                        <asp:TextBox ID="txtPCDDahej" runat="server"></asp:TextBox>
                        <asp:Image ID="imgPCDDahej" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPCDDahej" PopupPosition="BottomRight"
                            TargetControlID="txtPCDDahej">
                        </asp:CalendarExtender>

                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPCDDahej" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>

                        <%-- <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="txtInwardDate" ErrorMessage="Invalid Inward Date."
                              Display="Dynamic" Type="Date" Text="*" CultureInvariantValues="false" Operator="DataTypeCheck"
                              ValidationGroup="JobRequired"></asp:CompareValidator>--%>
                    </td>
                    <td>PCD Sent Client</td>
                    <td>
                        <asp:TextBox ID="txtPCDSentClient" runat="server"></asp:TextBox>
                        <asp:Image ID="imgPCDSentClient" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPCDSentClient" PopupPosition="BottomRight"
                            TargetControlID="txtPCDSentClient">
                        </asp:CalendarExtender>

                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPCDSentClient" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <td>File Sent to Billing</td>
                    <td>
                        <asp:TextBox ID="txtFileSentBilling" runat="server"></asp:TextBox>
                        <asp:Image ID="imgFileSentBilling" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFileSentBilling" PopupPosition="BottomRight"
                            TargetControlID="txtFileSentBilling">
                        </asp:CalendarExtender>

                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFileSentBilling" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                            ErrorMessage="Invalid date format." ValidationGroup="Required" ForeColor="Red">*
                        </asp:RegularExpressionValidator>

                    </td>
                    <td>Billing Status</td>
                    <td>
                        <asp:TextBox ID="txtBillingStatus" runat="server"></asp:TextBox>

                    </td>
                    <td><%--R.N.Logistics--%></td>
                    <td>
                        <asp:TextBox ID="txtRNLogistics" runat="server" Width="200px" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="multiline" Width="515px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnContainerDetail" runat="server" Text="Container Details" OnClick="btnContainerDetail_Click" />
                    </td>
                    <td></td>
                </tr>

            </table>

            </fieldset>

        <fieldset id="filldsetInvoice" runat="server" class="fieldset">
                <legend>Invoice & Product Details</legend>

                <table border="0" cellpadding="0" cellspacing="0" width="60%" bgcolor="white">
                    <tr>
                        <td>
                            <b>Upload Invoice File :  </b>
                        </td>
                        <td>
                            <asp:FileUpload ID="FuInvoice" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnUploadInvoice" runat="server" Text="Upload" OnClick="btnUploadInvoice_Click" />
                        </td>

                    </tr>
                </table>

                <br />

                <asp:GridView ID="GrdInvoiceDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false" Width="100%"
                    CssClass="grid" OnRowDataBound="GrdInvoiceDetail_OnRowDataBound" >
                    <Columns>
                        <%--  <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." />--%>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No.">
                            <ItemTemplate>
                                <asp:TextBox ID="txtInvoiceNum" runat="server" Width="150px"></asp:TextBox>
                            </ItemTemplate>

                            <FooterStyle HorizontalAlign="Left" />
                            <FooterTemplate>
                                <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" OnClick="ButtonAdd_Click" />
                            </FooterTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:TextBox ID="txtInvoiceDt" runat="server" Width="110px" placeholder="DD/MM/YYYY"></asp:TextBox>

                                <asp:CalendarExtender ID="CalendarExtender40" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtInvoiceDt" PopupPosition="BottomRight"
                                    TargetControlID="txtInvoiceDt">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Value">
                            <ItemTemplate>
                                <asp:TextBox ID="txtValueInvoice" runat="server" Width="110px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Term">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlTermInvoice" runat="server" DataSourceID="InvoiceSqlDataSource"
                                    DataTextField="sName" DataValueField="lid" AppendDataBoundItems="true" Width="100px">
                                    <%--<asp:ListItem Value="0" Text="-Select-"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescriptionProd" runat="server" Width="340px"></asp:TextBox>
                                <%--Textmode="Multiline"--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText=" Total Quantity">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server" Width="80px"></asp:TextBox>
                            </ItemTemplate>                           
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Quantity">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRemainingQty" runat="server" Width="80px" OnTextChanged="txtRemainingQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Item Price">
                            <ItemTemplate>
                                <asp:TextBox ID="txtItemPrice" runat="server" Width="80px" OnTextChanged="txtItemPrice_TextChanged" AutoPostBack="true"></asp:TextBox>                                
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Product Value">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProductVal" runat="server" Width="80px"></asp:TextBox>                                
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CTH">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCTH" runat="server" Width="80px"></asp:TextBox>                                
                            </ItemTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField HeaderText="Item Type">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlItemType" runat="server" DataSourceID="ItemTypeSqlDataSource"
                                    DataTextField="ItemName" DataValueField="lid" AppendDataBoundItems="true" Width="150px">
                                    <%--<asp:ListItem Value="0" Text="-Select-"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>


                    </Columns>
                </asp:GridView>

                <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetShipmentTerms" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                <asp:SqlDataSource ID="ItemTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="SEZ_GetItemTypeInvoice" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <%--  <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                 <tr>

                                <td>Invoice No.
                                </td>
                                <td>
                                    
                                        <%# Eval("InvoiceNo")%>
                                    
                                </td>
                                <td>Invoice Date
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("InvoiceDate", "{0:dd/MM/yyyy  hh:mm tt}")%>
                                    </span>
                                </td>

                                <td>Invoice Value
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("InvoiceValue")%>
                                    </span>
                                </td>
                            </tr>
                                 <tr>

                                <td>Term
                                </td>
                                <td>
                                    
                                        <%# Eval("Term")%>
                                    
                                </td>
                                <td>Quantity
                                </td>
                                <td>
                                    <%--<span>
                                        <%# Eval("InvoiceDate", "{0:dd/MM/yyyy  hh:mm tt}")%>
                                    </span>
                                </td>

                                <td>
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                                 <tr>

                                <td>Description
                                </td>
                                <td colspan="4">
                                    
                                        <%# Eval("DescriptionQty")%>
                                    
                                </td>                               

                                <td>
                                </td>
                               
                            </tr>
                             </table>
                --%>
            </fieldset>

        <fieldset id="filldsetUploadDoc" runat="server" class="fieldset">
                <legend>Upload Document</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="60%" bgcolor="white">
                    <tr>
                        <td>
                            <b>Document Type :</b>
                            <asp:DropDownList ID="ddDocument" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuDocument" runat="server" />
                        </td>


                        <td>
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                        </td>

                    </tr>
                </table>
            </fieldset>

        <div class="m clear">
        </div>
        <fieldset id="filldsetDownloadDoc" runat="server" class="fieldset">
                <legend>Download Document</legend>
                <asp:GridView ID="grdDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                    OnRowCommand="GridViewDocument_RowCommand" OnRowDeleting="OnRowDeleting"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" OnRowDataBound="grdDocument_OnRowDataBound" >
                    <%--DataSourceID="DocumentSqlDataSource" DataKeyNames="DocId"--%>
                    <Columns>
                        <%-- <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Doc Id">
                            <ItemTemplate>
                                <asp:Label ID="lblDocId" runat="server" Text='<%#Eval("PkId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Doc Type" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDocType" runat="server" Text='<%#Eval("DocType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Doc Type">
                            <ItemTemplate>
                                <asp:Label ID="lblDocTypeName" runat="server" Text='<%#Eval("DocTypeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Doc Path">
                            <ItemTemplate>
                                <asp:Label ID="lblDocPath" runat="server" Text='<%#Eval("DocPath") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Document Name">
                            <ItemTemplate>
                                <asp:Label ID="lblDocName" runat="server" Text='<%#Eval("DocumentName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Id">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("UserId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Download">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                <%--<asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" />

                        <%--                            <asp:BoundField DataField="PkId" HeaderText="Doc Id" />
                                <asp:BoundField DataField="DocPath" HeaderText="Doc Path" />
                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                                <asp:BoundField DataField="UserId" HeaderText="UserId" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <%-- <div>
                        <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetUploadedDocument" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>--%>
    

    </asp:Panel>

    <div class="loading" align="center">
        <%--<img src="loader.gif" alt="" />--%>
        <%--<img src="../Images/SEZload.gif" />--%>
        <img src="../Images/processing1.gif" />
        <%--<img src="../Images/SEZload.gif" />
        <img src="../Images/SEZload.gif" />
        <img src="../Images/SEZload.gif" />--%>
    </div>



    <div id="divHold1">

        <asp:ModalPopupExtender ID="ModalPopupContainer" runat="server" PopupControlID="PanelContainer" TargetControlID="btnContainerDetail" CacheDynamicResults="false"
            CancelControlID="imgContainerClose" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="PanelContainer" runat="server" CssClass="modalPopup" align="center" Style="display: none" BackColor="#EBF4FA">
            <center>
                <asp:Label ID="lblPopMessageContainer" runat="server" Visible="false"></asp:Label></center>
            <table width="100%" style="background-color: #c7a879">
                <tr>
                    <td style="width: 33%"></td>
                    <td style="width: 34%">
                        <div>
                            <center>
                                <b>Add Container Details </b>
                            </center>
                        </div>

                    </td>
                    <td style="width: 33%">
                        <div class="fright">
                            <asp:ImageButton ID="imgContainerClose" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                        </div>
                    </td>
                </tr>
            </table>

            <center>
                <asp:GridView ID="GrvCantainerDetail" runat="server" ShowFooter="true"
                    AutoGenerateColumns="false" class="table table-bordered" Width="95%" >
                    <Columns>
                        <asp:BoundField DataField="RowNumber" HeaderText="Sr" />
                        <asp:TemplateField HeaderText="Container NO">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtCantainerNO" runat="server" class="form-control"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="requiredValidator1" runat="server" ControlToValidate="TxtCantainerNO" Text="*"
                                    ValidationGroup="ContainerReq" ForeColor="Red" SetFocusOnError="true" ErrorMessage="Container No. Can't be blank"
                                    Display="Dynamic">                                
                                </asp:RequiredFieldValidator>


                                <%--  <asp:RegularExpressionValidator ID="RegularExpresphone1" ValidationGroup="ContainerReq" Display="Dynamic"
                                    ControlToValidate="TxtCantainerNO" runat="server" ErrorMessage="Container No. must be 11 Digit"
                                    SetFocusOnError="True" ValidationExpression="^\d{11}$"></asp:RegularExpressionValidator>--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ValidationGroup="ContainerReq" Display="Dynamic"
                                    ControlToValidate="TxtCantainerNO" runat="server" ErrorMessage="Alpha Numeric Combination + 11 Character Only"
                                    Text="*" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9]{11}$">
                                </asp:RegularExpressionValidator>

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Container Type">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlCantainerType" runat="server" DataSourceID="ContainerTypeSqlDataSource1"
                                    DataTextField="sName" DataValueField="lId" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Container Size">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlSize" runat="server" DataSourceID="ContainerSizeSqlDataSource"
                                    DataTextField="sName" DataValueField="lId" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="REQhsnCod" runat="server" ErrorMessage="*" SetFocusOnError="true"
                                    ForeColor="red" ControlToValidate="ddlSize" ValidationGroup="ContainerReq"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="btnContainer" runat="server" Text="Add New Row" class="btn btn-primary"
                                    OnClick="btnContainer_Click" ValidationGroup="ContainerReq" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#E6E9ED" ForeColor="#73879C" />
                </asp:GridView>
            </center>

            <asp:SqlDataSource ID="ContainerSizeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="SEZ_GetContainerSize" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <asp:SqlDataSource ID="ContainerTypeSqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="SEZ_GetContainerType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

        </asp:Panel>
    </div>

    <div id="divErrror">

        <asp:HiddenField ID="hdnPopUpError" runat="server" />     

        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="PanelError" TargetControlID="hdnPopUpError" CacheDynamicResults="false"
            CancelControlID="imgClose" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="PanelError" runat="server" CssClass="modalPopup" align="center" Style="display: none; overflow-y: scroll; height: 400px; width: 700px" BackColor="#EBF4FA" ScrollBar="auto" >
            <center>
                <asp:Label ID="lblExcelError" runat="server"></asp:Label></center>
            <table width="100%" style="background-color: #c7a879">
                <tr>
                    <td style="width: 33%"></td>
                    <td style="width: 34%">
                        <div>
                            <center>
                                <b>Error Message </b>
                            </center>
                        </div>

                    </td>
                    <td style="width: 33%">
                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                        </div>
                    </td>
                </tr>
            </table>

            <center>
                <asp:GridView ID="GrdError" runat="server"
                    AutoGenerateColumns="false" class="table table-bordered" Width="95%" AlternatingRowStyle-BackColor = "#C2D69B">
                    <Columns>

                        <%--<asp:BoundField DataField=""  HeaderText="Thoka No" />--%>
                        <asp:TemplateField HeaderText="Sr No." ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RequestID" HeaderText="Request ID" ItemStyle-Width="40%" />
                        <asp:BoundField DataField="ThokaNo" HeaderText="Thoka No" ItemStyle-Width="40%"/>

                    </Columns>
                    <HeaderStyle BackColor="#E6E9ED" ForeColor="#73879C" />
                </asp:GridView>
            </center>
        </asp:Panel>
    </div>


</asp:content>


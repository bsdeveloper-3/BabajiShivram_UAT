<%@ Page Title="Contract Billing" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContractBilling.aspx.cs"
    Inherits="Master_ContractBilling" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="Gvpager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="../scripts/jquery.sumoselect.min.js"></script>
    <link href="../CSS/sumoselect.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=dgvDetailData_1.ClientID %>').Scrollable({
                ScrollHeight: 300,
                IsInUpdatePanel: true
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
        function numeric(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode;
            if (unicode == 8 || unicode == 9 || (unicode >= 48 && unicode <= 57)) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <style type="text/css">
        .FixedHeader {
            position: absolute:static;
            font-weight: bold;
        }

        .scrolling {
            position: absolute;
        }

        .gvWidthHight {
            overflow: scroll;
            height: 300px;
            width: 100%;
        }
    </style>
    <style>
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            width: 72%;
            height: 100%;
            overflow: auto;
        }

        .CPH {
            height: 100%;
        }

        .modalPopupField {
            width: 100%;
            height: 100%;
        }

        .column {
            float: left;
            width: 49%;
            padding-left: 10px;
        }

        .linkColor {
            color: #00f;
        }
        /* Clear floats after the columns */
        .row:after {
            content: "";
            display: table;
            clear: both;
        }

        a {
            color: #0d0d0e;
        }

        myGridStylePreview {
            border-collapse: collapse;
            font-size: 7px;
        }

        .myGridStylePreview tr th {
            padding: 8px;
            color: #0a0a0a;
            border: 1px solid black;
        }

        .myGridStylePreview td {
            border: 1px solid black;
            padding: 8px;
        }


        .myGridStyle {
            border-collapse: collapse;
        }

            .myGridStyle tr th {
                padding: 8px;
                color: #0a0a0a;
                border: 1px solid black;
            }


            .myGridStyle tr:nth-child(even) {
                background-color: #bcbcbc;
            }

            .myGridStyle tr:nth-child(odd) {
                background-color: #eeeeee;
            }

            .myGridStyle td {
                border: 1px solid black;
                padding: 8px;
            }

            .myGridStyle tr:last-child td {
            }

        .buttonstyle {
            width: 80px;
            height: 50px;
            font-size: medium;
        }

        .auto-style8 {
            width: 110px;
        }

        .auto-style9 {
            width: 120px;
        }

        .auto-style10 {
            width: 100px;
        }

        .auto-style11 {
            width: 108px;
        }

        .auto-style12 {
            width: 117px;
        }

        .auto-style13 {
            width: 139px;
        }
    </style>

    <ajaxtoolkit:toolkitscriptmanager runat="server" asyncpostbacktimeout="36000" id="ScriptManager1" />

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" />
    <asp:HiddenField ID="hdnCRM_UserId" runat="server" Value="0" />


    <asp:UpdatePanel runat="server" ID="UpdMain" UpdateMode="Conditional">

        <ContentTemplate>
            <div>
                &nbsp;
            </div>
            <div align="center">
                <asp:Label ID="lblCbMaster" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
            </div>
            <asp:Button ID="btnNew" runat="server" Text="New Contract" OnClick="btnNew_Click" />
            <fieldset id="Fieldset1" runat="server">
                <legend>CB Billing Detail</legend>
                <!-- Filter Content Start-->
                <%-- <div class="m clear">
                    <div class="fleft">
                        <td>Search By:
        <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="True">
            <asp:ListItem Value="0" Text="All"></asp:ListItem>
            <asp:ListItem Value="1" Text="ContractName"></asp:ListItem>
            <asp:ListItem Value="2" Text="CustomerName"></asp:ListItem>
        </asp:DropDownList>
                            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
width: 100%; height: 100%; 
                </div>--%>
                <div class="fleft">
            <asp:LinkButton ID="lnkexport1" runat="server" OnClick="lnkexport1_Click">
                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            &nbsp;
        </div>
                <div class="clear">
                </div>
                <div style="overflow: scroll">
                    <%-- <uc1:DataFilter ID="DataFilter1" runat="server" />--%>
                    <!-- Filter Content END-->
                    <uc1:datafilter id="DataFilter1" runat="server" />
                    <asp:Panel ID="Panel6" runat="server" CssClass="ModalPopupPanel" Width="100%" BorderStyle="None" ScrollBars="Auto">
                        <asp:GridView ID="dgvDetailData_1" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" DataSourceID="GridviewSqlDataSource" DataKeyNames="lId"
                            Width="100%" AllowPaging="True" PageSize="20" CssClass="table" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom"
                            runat="server" OnRowCommand="dgvDetailData_1_RowCommand" OnPreRender="gvUser_PreRender" OnSelectedIndexChanged="gvUser_SelectedIndexChanged">

                            <%--OnSelectedIndexChanged="OnSelectedIndexChanged">--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" SortExpression="CustName">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="Server" Text='<%#Eval("CustomerName") %>' CommandName="Select"
                                            CommandArgument='<%#Eval("lId") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustomerName" HeaderText="Company Name" visible="false"/>
                                <asp:BoundField DataField="ContractName" HeaderText="Contract Name" />
                                <asp:BoundField DataField="ContractStartDate" HeaderText="Contract StartDate" DataFormatString="{0:dd/MM/yyy}" />
                                <asp:BoundField DataField="ContractEndDate" HeaderText="Contract EndDate" DataFormatString="{0:dd/MM/yyy}" />
                                <asp:BoundField DataField="ContractUID" HeaderText="Contract UID" />
                            </Columns>

                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </div>
                <%--<PagerTemplate>
                        <Gvpager:GridViewPager runat="server" /> </PagerTemplate>--%>
                <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="Get_ContratcBilling" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="updAll" UpdateMode="Conditional">
        <ContentTemplate>


            <div id="divContractBilling">
                <%-- <AjaxToolkit:ModalPopupExtender ID="ModalPopupContractBilling" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="PanelContractBilling" TargetControlID="lnkDummyContractBilling">
                </AjaxToolkit:ModalPopupExtender>--%>

                <div>
                    <!--remove code-->
                    <div align="center">
                        <asp:Label ID="lberror" runat="server" Font-Size="Medium" Font-Bold="True" CssClass="errorMsg" EnableViewState="false"></asp:Label>

                    </div>
                </div>
                <asp:Panel ID="PanelContractBilling" runat="server" CssClass="ModalPopupPanel" Width="99%" Height="100%" BorderStyle="Solid" ScrollBars="vertical">
                    <div id="AddedPanel" runat="server">
                        <fieldset id="Fieldset2" runat="server">
                            <legend>Customer Details</legend>
                            <table border="1" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                               <tr>
            <td class="auto-style8">Customer Name
<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cboCustomerName"
    InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Customer"
    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
            <td colspan="3">
                <asp:DropDownList ID="cboCustomerName" AppendDataBoundItems="true" AutoPostBack="false"
                    DataTextField="CustName" DataValueField="CustID" runat="server" Style="width: 100%;"
                    TabIndex="1">
                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                </asp:DropDownList></td>
            <td class="auto-style10">Contract Name
<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtContractName"
    Text="*" InitialValue="0" Display="Dynamic" ErrorMessage="Please Enter Contract Name"
    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
            <td colspan="3">
                <asp:TextBox ID="txtContractName" runat="server" TabIndex="2" MaxLength="250" TextMode="MultiLine" Width="94%" Text='<%# Bind("ContractName") %>' /></td>
            </tr>
        <tr>
            <td class="auto-style9">Contract Start Date
<asp:RequiredFieldValidator ID="RFVStartDate" runat="server" ControlToValidate="dtStartDate"
    InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Start Date"
    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
            <td class="auto-style12">
                <ajaxtoolkit:calendarextender id="CalExtStartDate" runat="server" enabled="True" enableviewstate="False"
                    firstdayofweek="Sunday" format="dd/MMM/yyyy" popupbuttonid="imgDateStart" popupposition="BottomRight"
                    targetcontrolid="dtStartDate">
                </ajaxtoolkit:calendarextender>
                <asp:TextBox ID="dtStartDate" runat="server" TabIndex="4" Text='<%# Bind("ContractStartDate","{0:dd-MMM-yyyy}") %>' Style="width: 70px"></asp:TextBox>
                <asp:Image ID="imgDateStart" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" /></td>
            <td class="auto-style11">Contract End Date
<asp:RequiredFieldValidator ID="RFVEndDate" runat="server" ControlToValidate="dtEndDate"
    InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select End Date"
    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cmp_edate" ValidationGroup="diff_graph" ControlToValidate="dtEndDate" ControlToCompare="dtStartDate"
                    Text="greater than start date!" ForeColor="Red" Display="Dynamic" runat="server" ErrorMessage="CompareValidator" Operator="GreaterThan"></asp:CompareValidator>
            </td>
            <td class="auto-style13">
                <ajaxtoolkit:calendarextender id="CalExtEndDate" runat="server" enabled="True" enableviewstate="False"
                    firstdayofweek="Sunday" format="dd/MMM/yyyy" popupbuttonid="imgDateEnd" popupposition="BottomRight"
                    targetcontrolid="dtEndDate">
                </ajaxtoolkit:calendarextender>
                <asp:TextBox ID="dtEndDate" runat="server" CssClass="date form-control" TabIndex="5" Text='<%# Bind("ContractEndDate","{0:dd-MMM-yyyy}") %>' Style="width: 70px" />
                <asp:Image ID="imgDateEnd" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" /></td>
            <td>
                Contract Copy
            </td>
            <td>
                <asp:LinkButton ID="lnbContractCopy" runat="server" OnClick="lnbContractCopy_Click" Text="Upload Contract Copy" autopostback="true" ></asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click" Text='<%# Bind("FileName") %>'></asp:LinkButton>
            </td>
           <%-- <td>Contract Copy</td>
             <td colspan="3"> 
                 <asp:FileUpload id="fuContractFile" runat="server" />
             </td>--%>
        </tr>
        <%-- <tr>
            <td colspan="8">
                 <asp:Button ID="btnSave" align="center" runat="server" Text="Upload" OnClick="btnSave_Click" autopostback="true" />
            </td>
        </tr>--%>
                            </table>
                        </fieldset>
                        <fieldset id="Fieldset3" runat="server">
                            <legend>Add Billing Details</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Division
                        <asp:RequiredFieldValidator ID="valid_Division" runat="server" ControlToValidate="cboDivision"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Customer"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cboDivision" Width="150 px" AppendDataBoundItems="false" AutoPostBack="true"
                                            DataTextField="Division" DataValueField="divisionID" runat="server" Style="width: 100%;"
                                            TabIndex="1" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Heading</td>
                                    <td>
                                        <asp:TextBox ID="txtHeading" runat="server" TabIndex="2" MaxLength="250" Width="94%" Text='<%# Bind("ContractName") %>' /></td>
                                    <td>Job Type
                        <asp:RequiredFieldValidator ID="valid_job" runat="server" ControlToValidate="cboJobType"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Job Type"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboJobType" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="Job" DataValueField="JobID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Type of BE/SB
                        <asp:RequiredFieldValidator ID="valid_typeofbe" runat="server" ControlToValidate="cboTypeOfBE"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Type of BEe"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboTypeOfBE" AppendDataBoundItems="false" AutoPostBack="true" OnSelectedIndexChanged="cboTypeOfBE_SelectedIndexChanged"
                                            DataTextField="BETypeName" DataValueField="JobID" runat="server" Style="width: 100%;"
                                            TabIndex="1" Enabled="False">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Charage Name
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="cboChargeName"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Charge Name"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="cboChargeName" AppendDataBoundItems="false" AutoPostBack="true"
                                            DataTextField="ChargeName" DataValueField="chargeID" runat="server" Style="width: 100%;"
                                            TabIndex="1" OnSelectedIndexChanged="cboChargeName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>

                                    <td>RMS/Non RMS
                        <asp:RequiredFieldValidator ID="valid_RSMNONRSM" runat="server" ControlToValidate="cboRMSNonRMS"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Job Type"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboRMSNonRMS" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>Delivery Type
                        <asp:RequiredFieldValidator ID="valid_deliverytype" runat="server" ControlToValidate="cboLoadedDeStuff"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Type of BEe"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboLoadedDeStuff" AppendDataBoundItems="false" AutoPostBack="True"
                                            DataTextField="Name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1" OnSelectedIndexChanged="cboLoadedDeStuff_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Charge Code</td>
                                    <td>
                                        <asp:TextBox ID="txtChargeCode" runat="server" TabIndex="2" MaxLength="250" Text='<%# Bind("ContractName") %>' Width="94%" ReadOnly="True" /></td>
                                    <td>UOM
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboUOM"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Customer"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboUOM" AppendDataBoundItems="false" AutoPostBack="true"
                                            DataTextField="sname" DataValueField="lid" runat="server" Style="width: 100%;"
                                            TabIndex="1" OnSelectedIndexChanged="cboUOM_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>Currency
                        <asp:RequiredFieldValidator ID="valid_currency" runat="server" ControlToValidate="cboCurrency"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Job Type"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboCurrency" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">Range Criteria
                        <asp:RequiredFieldValidator ID="valid_range" runat="server" ControlToValidate="cboRangeCriteria"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Customer"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td colspan="3" style="border-top-style: solid; border-top-width: medium; border-top-color: black;">
                                        <asp:DropDownList ID="cboRangeCriteria" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>

                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">Cap Criteria
                        <asp:RequiredFieldValidator ID="valid_capcriteria" runat="server" ControlToValidate="cboCAPCriteria"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Customer"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td colspan="3" style="border-top-style: solid; border-top-width: medium; border-top-color: black;">
                                        <asp:DropDownList ID="cboCAPCriteria" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Range From:</td>
                                    <td>
                                        <asp:TextBox ID="txtRangeFrom" runat="server" Width="94%" TabIndex="2" MaxLength="250" Text='<%# Bind("ContractName") %>' /></td>
                                    <td>Range To</td>
                                    <td>
                                        <asp:TextBox ID="txtRangeto" runat="server" TabIndex="2" MaxLength="250" Text='<%# Bind("ContractName") %>' Width="94%" /></td>
                                    <td>Cap Min</td>
                                    <td>
                                        <asp:TextBox ID="txtCapMin" runat="server" Width="94%" TabIndex="2" MaxLength="250" Text='<%# Bind("ContractName") %>' /></td>
                                    <td>Cap Max</td>
                                    <td>
                                        <asp:TextBox ID="txtCapMax" runat="server" TabIndex="2" MaxLength="250" Text='<%# Bind("ContractName") %>' Width="94%" /></td>
                                </tr>
                                <tr>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">Mode
                        <asp:RequiredFieldValidator ID="valid_mode" runat="server" ControlToValidate="cboMode"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Mode"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">

                                        <asp:DropDownList ID="cboMode" AppendDataBoundItems="false" AutoPostBack="True"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1" OnSelectedIndexChanged="cboMode_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">Port
                        <asp:RequiredFieldValidator ID="valid_port" runat="server" ControlToValidate="cboPort"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Mode"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">
                                        <asp:DropDownList ID="cboPort" AppendDataBoundItems="false" AutoPostBack="true" OnSelectedIndexChanged="cboPort_SelectedIndexChanged"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">User/System         
                        <asp:RequiredFieldValidator ID="valid_usersystem" runat="server" ControlToValidate="cboUserSystem"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Customer"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">
                                        <asp:DropDownList ID="cboUserSystem" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">Rate</td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">
                                        <asp:TextBox ID="txtRate" runat="server" TabIndex="2" MaxLength="250" Text='<%# Bind("ContractName") %>' Width="94%" />
                                        <asp:RegularExpressionValidator ID="revRate" runat="server" ValidationExpression="^[0-9]\d*(\.\d+)?$" ControlToValidate="txtRate"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="(Invalid rate)" ForeColor="Red"></asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Container Type
                        <asp:RequiredFieldValidator ID="valid_containertype" runat="server" ControlToValidate="cboContainerType"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Container Type"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboContainerType" AppendDataBoundItems="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>Container Size
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboTypeOfShipment"
                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select Container Size"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td>
                                        <asp:DropDownList ID="cboTypeOfShipment" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="name" DataValueField="ID" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>Condition</td>
                                    <td><asp:TextBox ID="txtcondition" runat="server" TabIndex="2" TextMode="MultiLine" Text='<%# Bind("co") %>' Width="94%" /></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">FA Book         
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="cboFABook"
                                            InitialValue="0" Display="Dynamic" Text="*" ErrorMessage="Please Select FA Book"
                                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                    <td style="border-top-style: solid; border-top-width: medium; border-top-color: black;">
                                        <asp:DropDownList ID="cboFABook" AppendDataBoundItems="false" AutoPostBack="false"
                                            DataTextField="BookName" DataValueField="BookCode" runat="server" Style="width: 100%;"
                                            TabIndex="1">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                 <tr>
                                
                             </tr>
                            </table>
                            <table>
                               
                            </table>
                        </fieldset>

                    </div>
                    <div style="float: none">
                        <center>
                            <%-- <uc1:DataFilter ID="DataFilter1" runat="server" />--%>
                            <asp:Button ID="cmdAddLine" runat="server" Text="Add Line Item" OnClick="cmdAddLine_Click" BorderStyle="Solid" />
                            <asp:Button ID="cmdEditContractLine" runat="server" Text="Update Line Item" OnClick="cmdEditContractLine_Click" BorderStyle="Solid" />
                            <%--<asp:Button ID="cmdClearline" runat="server" Text="Billing Details Clear" OnClick="cmdClearline_Click" Height="30px" Width="182px" Font-Size="Large" BorderStyle="Solid" />--%>
                        </center>
                        <div align="center">
                            <asp:Label ID="lblerror1" runat="server" Font-Size="Medium" Font-Bold="True" CssClass="errorMsg" EnableViewState="false"></asp:Label>

                        </div>
                    </div>
                    <br />
                    <div>
                        <asp:Panel ID="Panel1" runat="server" CssClass="ModalPopupPanel" Width="99%" Height="30%" BorderStyle="Solid" ScrollBars="Auto">
                            <div class="gvWidthHight" runat="server">
                                <br />
                                PageSize:
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="40" Value="40" />
                                    <asp:ListItem Text="80" Value="80" />
                                    <asp:ListItem Text="Show All" Value="0" Selected="True" />
                                </asp:DropDownList>

                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>

                                <hr />

                                <asp:GridView ID="dgvDetailData1" Height="25%" runat="server"
                                    runat="server" AutoGenerateColumns="false" CssClass="table" PageSize="20" AllowPaging="false"
                                    PagerSettings-Position="TopAndBottom" HeaderStyle-CssClass="FixedHeader"
                                    HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                    DataKeyNames="fabookid,ChargeCode,UOMID,ModeID,CurrencyID,PortID,ContainerTypeID,TypeOfShipmentID,RangeCriteriaID,
                                                            CapCriteriaID,UserSystemID,JobTypeID,TypeOfBEID,RMSNonRMSID,LoadedDeStuffID,DivisionId,lid"
                                    OnRowCommand="dgvDetailData1_RowCommand"
                                    OnPageIndexChanging="dgvDetailData1_PageIndexChanging" OnRowDeleting="dgvDetailData1_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex +1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DivisionName" HeaderText="Division Name" SortExpression="DivisionName" />
                                        <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" SortExpression="ChargeName" />
                                        <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />
                                        <asp:BoundField DataField="UOM" HeaderText="UOM" SortExpression="UOM" />
                                        <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                                        <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                                        <asp:BoundField DataField="ContainerType" HeaderText="Container Type" SortExpression="ContainerType" />
                                        <asp:BoundField DataField="TypeOfShipment" HeaderText="Type Of Shipment" SortExpression="TypeOfShipment" />
                                        <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                        <asp:BoundField DataField="ChargeCode" HeaderText="Charge Code" SortExpression="ChargeCode" />
                                        <asp:BoundField DataField="Heading" HeaderText="Heading" SortExpression="Heading" />
                                        <asp:BoundField DataField="UserSystem" HeaderText="User/System" SortExpression="UserSystem" />
                                        <asp:BoundField DataField="JobType" HeaderText="Job Type" SortExpression="JobType" />
                                        <asp:BoundField DataField="TypeOfBE" HeaderText="Type Of BE" SortExpression="TypeOfBE" />
                                        <asp:BoundField DataField="RMSNonRMSName" HeaderText="RMS/NonRMS" SortExpression="RMSNonRMSName" />
                                        <asp:BoundField DataField="LoadedDeStuff" HeaderText="Loaded/De-Stuff" SortExpression="LoadedDeStuff" />
                                        <asp:BoundField DataField="RangeCriteria" HeaderText="Range Criteria" SortExpression="RangeCriteria" />
                                        <asp:BoundField DataField="RangeFrom" HeaderText="Range From" SortExpression="RangeFrom" />
                                        <asp:BoundField DataField="RangeTo" HeaderText="Range To" SortExpression="RangeTo" />
                                        <asp:BoundField DataField="CapCriteria" HeaderText="Cap Criteria" SortExpression="CapCriteria" />
                                        <asp:BoundField DataField="CapMin" HeaderText="Cap Min" SortExpression="CapMin" />
                                        <asp:BoundField DataField="CapMax" HeaderText="Cap Max" SortExpression="CapMax" />

                                        <asp:BoundField DataField="UOMID" HeaderText="UOMID" SortExpression="UOMID" Visible="false" />
                                        <asp:BoundField DataField="ModeID" HeaderText="ModeID" SortExpression="ModeID" Visible="false" />
                                        <asp:BoundField DataField="CurrencyID" HeaderText="CurrencyID" SortExpression="CurrencyID" Visible="false" />
                                        <asp:BoundField DataField="PortID" HeaderText="PortID" SortExpression="PortID" Visible="false" />
                                        <asp:BoundField DataField="ContainerTypeID" HeaderText="ContainerTypeID" SortExpression="ContainerTypeID" Visible="false" />
                                        <asp:BoundField DataField="TypeOfShipmentID" HeaderText="TypeOfShipmentID" SortExpression="TypeOfShipmentID" Visible="false" />
                                        <asp:BoundField DataField="RangeCriteriaID" HeaderText="RangeCriteriaID" SortExpression="RangeCriteriaID" Visible="false" />
                                        <asp:BoundField DataField="CapCriteriaID" HeaderText="CapCriteriaID" SortExpression="CapCriteriaID" Visible="false" />
                                        <asp:BoundField DataField="UserSystemID" HeaderText="UserSystemID" SortExpression="UserSystemID" Visible="false" />
                                        <asp:BoundField DataField="JobTypeID" HeaderText="JobTypeID" SortExpression="JobTypeID" Visible="false" />
                                        <asp:BoundField DataField="TypeOfBEID" HeaderText="TypeOfBEID" SortExpression="TypeOfBEID" Visible="false" />
                                        <asp:BoundField DataField="RMSNonRMSID" HeaderText="RMSNonRMSID" SortExpression="RMSNonRMSID" Visible="false" />
                                        <asp:BoundField DataField="LoadedDeStuffID" HeaderText="LoadedDeStuffID" SortExpression="LoadedDeStuffID" Visible="false" />
                                        <asp:BoundField DataField="DivisionId" HeaderText="DivisionId" SortExpression="DivisionId" Visible="false" />
                                        <asp:BoundField DataField="Condition" HeaderText="Condition" SortExpression="Condition" />
                                        <asp:BoundField DataField="lid" HeaderText="lid" SortExpression="lid" Visible="false" />
                                        <asp:BoundField DataField="fabookid" HeaderText="fabookid" SortExpression="fabookid"/>  <%--fabookid--%>
                                        <%--<asp:BoundField DataField="FABookname" HeaderText="FA Book Name" SortExpression="FABookname" />--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hflid" runat="server" Value='<%# Bind("lid") %>' EnableViewState="true" />
                                                <asp:LinkButton ID="lnkEdit" runat="Server" CssClass="linkColor" Text='Edit'
                                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditItem"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdelete" runat="Server" CssClass="linkColor" Text='Remove'
                                                    CommandArgument="<%# Container.DataItemIndex %>" OnClick="deletecontractline_Click"
                                                    CommandName="Delete"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                            </div>
                        </asp:Panel>
                    </div>
                    <br />
                    <div style="float: none">
                        <center>
                            <asp:SqlDataSource ID="GridviewSqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="Usp_GetContractbillingrData" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="lid" SessionField="lid" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Button ID="cmdsave" runat="server" Text="Contract Save" OnClick="cmdsave_Click" BorderStyle="Solid" />
                            <%--<asp:Button ID="cmdclear" runat="server" Text="Clear" OnClick="cmdclear_Click" BorderStyle="Solid" />--%>
                            &nbsp;<asp:Button ID="btncloseContractBilling" runat="server" Text="Close" CausesValidation="false" OnClick="btncloseContractBilling_Click" />
                            <%--<asp:Button ID="cmdexporttoexcel" runat="server" Text="Export to Excel"  BorderStyle="Solid" OnClick="cmdexporttoexcel_Click" />--%>
                            <%--<asp:Button ID="cmdexit" runat="server" Text="Exit" Visible="False" Height="30px" Width="80px" Font-Size="Large" BorderStyle="Solid" />--%>
                        </center>
                    </div>

                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyContractBilling" runat="server" Text=""></asp:LinkButton>
            </div>

            <asp:Button ID="modelPopup1" runat="server" Style="display: none" />
            <ajaxtoolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup1" PopupControlID="Panel2"></ajaxtoolkit:ModalPopupExtender>
            <asp:Panel ID="Panel2" Style="display: none" runat="server">
                <fieldset class="ModalPopupPanel">
                    <div title="ContractCopy Details" class="header">
                        <textbox>ContractCopy Details</textbox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="imgClose" align="center" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                    </div>
                    <div class="AutoExtenderList">
                        <td>
                            <asp:Label ID="lblContractCopyerror" runat="server"></asp:Label>
                        </td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblContractCopy" runat="server" Text="Contract Copy:  " Font-Bold="true" ForeColor="Black" Font-Size="9"></asp:Label>
                                </td>
                                <td> 
                                     <asp:FileUpload id="fuContractFile" runat="server" />
                                    <asp:HiddenField ID="hdContractFileName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                   <asp:Button ID="btnSave" align="center" runat="server" Text="Upload" OnClick="btnSave_Click" autopostback="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </asp:Panel>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NFormDetail.aspx.cs" Inherits="NForm_NFormDetail"
    Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>N Form Detail - Babaji Shivram</title>
    <link href="../CSS/babaji-shivram.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table.table
        {
            white-space: normal;
        }
    </style>
    <script type="text/javascript">
        function imgDelNformDoc_OnClick(fileupload) {
            var fileUpload = document.getElementById(fileupload);
            var id = fileUpload.id;
            var name = fileUpload.name;

            //Create a new FileUpload element.
            var newFileUpload = document.createElement("input");
            newFileUpload.type = "file";

            //Append it next to the original FileUpload.
            fileUpload.parentNode.insertBefore(newFileUpload, fileUpload.nextSibling);

            //Remove the original FileUpload.
            fileUpload.parentNode.removeChild(fileUpload);

            //Set the Id and Name to the new FileUpload.
            newFileUpload.id = id;
            newFileUpload.name = name;
            newFileUpload.style = 'width: 165px';
            return false;
        }

       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <input type="hidden" id="scrollPos" name="scrollPos" value="0" runat="server" />
    <div class="logout">
        <asp:LinkButton ID="btnLogout" Text="Logout" runat="server" OnClick="btnLogout_Click"
            ToolTip="Logout" CausesValidation="false" TabIndex="1000">
                <img src="../Images/logout.png" width="48" height="43" alt="Logout" /></asp:LinkButton></div>
    <div id="header" style="background: url(Images/banner-bg-logo.png) right top no-repeat #1e3062;">
        <img src='../Images/Babji-Logo.png' width="592" height="131" alt="Babaji Shivram" />
        <div class="clear">
        </div>
    </div>
    <div id="divUpdPanel">
        <div>
            <div class="heading">
                <div class="UserName">
                </div>
                <div class="pageHeading">
                    N Form
                </div>
                <div class="clear">
                </div>
            </div>
            <div id="divCPH" class="CPH" onscroll="saveScrollPos();" runat="server" style="padding: 10px;">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="UpdateReq" />
                <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlNForm" runat="server">
                    <ProgressTemplate>
                        <img alt="progress" src="../Images/processing.gif" />
                        Processing...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <fieldset>
                    <legend>Upload N Form Detail</legend>
                    <table cellpadding="0" cellspacing="0" width="50%" bgcolor="white">
                        <tr>
                            <td>
                                &nbsp;Enter Babaji Job Number / BOE
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" runat="server" TabIndex="1" placeholder="Job Number / BOE"
                                    ToolTip="Enter Babaji Job Number."></asp:TextBox>
                                &nbsp;&nbsp; &nbsp;
                                <asp:Button ID="btnSearchJobNformDet" runat="server" OnClick="btnSearchJobNformDet_OnClick"
                                    Text="Search" />
                                <asp:Button ID="btnCancelJobNo" runat="server" Text="Cancel" OnClick="btnCancelJobNo_OnClick" />
                            </td>
                        </tr>
                    </table>
                    <fieldset id="fsDeliveryDetails" runat="server">
                        <legend>Delivery Detail</legend>
                        <div>
                            <asp:GridView ID="GridViewDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                                ToolTip="Delivery Details" Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                                OnRowCommand="GridViewDelivery_RowCommand" DataKeyNames="lid" CellPadding="4"
                                OnRowDataBound="GridViewDelivery_RowDataBound" AllowPaging="True" OnRowEditing="GridViewDelivery_RowEditing"
                                OnRowCancelingEdit="GridViewDelivery_RowCancelingEdit" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                                PageSize="7" OnPageIndexChanging="GridViewDelivery_PageIndexChanging" OnRowUpdating="GridViewDelivery_RowUpdating">
                                <Columns>
                                    <%--  <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="BS Job No" ItemStyle-Width="130px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobNoNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BOE" ItemStyle-Width="35px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBOE" runat="server" Text='<%#Eval("BOE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="150px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustName" runat="server" Text='<%#Eval("CustName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transporter Name" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransporter" runat="server" Text='<%#Eval("TransporterName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LR No" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLRNo" runat="server" Text='<%#Eval("LRNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LR Date" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLRDate" runat="server" Text='<%#Eval("LRDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="N Form No" ItemStyle-Width="50px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:TextBox ID="txtNFormNo" runat="server" Width="50px" Text='<%# Eval("NFormNo")%>'
                                                Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqvNFormNo1" runat="server" Display="Dynamic" Text="*"
                                                ErrorMessage="Please Enter N Form No" ValidationGroup="GridDeliveryRequired"
                                                ControlToValidate="txtNFormNo"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="N Form Date" ItemStyle-Width="50px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>'
                                                placeholder="dd/mm/yyyy"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtNFormDate" runat="server" Width="65px" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>'
                                                placeholder="dd/mm/yyyy" Enabled="false" Style="border: none; background-color: White;
                                                color: Black"></asp:TextBox>
                                        </EditItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:TextBox ID="txtNFormDate" runat="server" Width="65px" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>'
                                                Enabled="true" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calNFormDate" runat="server" Enabled="true" EnableViewState="false"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtNFormDate">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEditNFormDate" TargetControlID="txtNFormDate" Mask="99/99/9999"
                                                MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValNFormDate" ControlExtender="MEditNFormDate"
                                                ControlToValidate="txtNFormDate" IsValidEmpty="false" EmptyValueBlurredText="*"
                                                EmptyValueMessage="Please Enter N Form Date" InvalidValueBlurredMessage="Invalid Date"
                                                InvalidValueMessage="N Form Date is invalid" MinimumValueMessage="Invalid Date"
                                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" SetFocusOnError="true"
                                                runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </EditItemTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="N Closing Date" ItemStyle-Width="50px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'
                                                Enabled="true"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'
                                                Width="65px" Enabled="true" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calNClosingDate" runat="server" Enabled="true" EnableViewState="false"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtNClosingDate">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEditNFormCloseDate" TargetControlID="txtNClosingDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                                runat="server">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValNFormCloseDate" ControlExtender="MEditNFormCloseDate"
                                                ControlToValidate="txtNClosingDate" IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date"
                                                InvalidValueMessage="N Form Closing Date is invalid" MinimumValueMessage="Invalid Date"
                                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                                                EmptyValueMessage="Enter N Closing Date" SetFocusOnError="true" runat="Server" EmptyValueBlurredText="*"
                                                ValidationGroup="UpdateReq"></cc1:MaskedEditValidator>
                                            <asp:CompareValidator ID="cvNClosingNFormDate" runat="server" SetFocusOnError="true"
                                                Display="Dynamic" ControlToValidate="txtNClosingDate" Type="Date" ControlToCompare="txtNFormDate"
                                                Text="*" ErrorMessage="N Closing Date should be greater than N Form Date." Operator="GreaterThanEqual"
                                                ForeColor="Red" ValidationGroup="UpdateReq"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-Width="50px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>' Enabled="true"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" Width="50px" Text='<%#Eval("Amount") %>'
                                                Enabled="true"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="txtAmount"
                                                ValidationExpression="^[0-9]\d{0,9}(\.\d{1,2})?%?$" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" ValidationGroup="UpdateReq" Text="*" ErrorMessage="Invalid amount."></asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NForm Document" ItemStyle-Width="265px" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadNForm" runat="server" Text='<%#Eval("NformDocName") %>'
                                                CommandName="DownloadDoc" CommandArgument='<%#Eval("NformDocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:FileUpload ID="fuDocument_NForm" runat="server" Width="165px" />
                                            <asp:ImageButton ID="imgbtnDelNformDoc" runat="server" ImageUrl="../Images/Close.gif"
                                                Width="10px" Height="10px" Style="height: 10px; width: 10px; float: right; padding: 0px;" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Handwritten NForm Document" ItemStyle-Width="265px"
                                        ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadHandNform" runat="server" Text='<%#Eval("HandNformDocName") %>'
                                                CommandName="DownloadDoc" CommandArgument='<%#Eval("HandNformDocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:FileUpload ID="fuDocument_HandNForm" runat="server" Width="165px" />
                                            <asp:ImageButton ID="imgbtnDelHandNformDoc" runat="server" ImageUrl="../Images/Close.gif"
                                                Width="10px" Height="10px" Style="height: 10px; width: 10px; float: right; padding: 0px;" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                                Text="Edit" Font-Underline="true"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ValidationGroup="UpdateReq" ToolTip="Update"
                                                Width="45" runat="server" Text="Update" Font-Underline="true">
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                                                runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hdnUploadPath" runat="server" />
                            <div>
                                <asp:LinkButton ID="lnkDummy" runat="server" Text="" CausesValidation="false"></asp:LinkButton>
                            </div>
                        </div>
                    </fieldset>
                </fieldset>
                <asp:UpdatePanel ID="upnlNForm" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Generate Report</legend>
                            <div>
                                <div class="fleft">
                                    <asp:Button ID="btnGenReport" runat="server" Text="Show Report" OnClick="btnGenReport_OnClick"
                                        ValidationGroup="Required" />
                                    <asp:Button ID="btnCancelReport" runat="server" Text="Cancel" OnClick="btnCancelReport_OnClick" />
                                </div>
                                <div class="fleft" style="margin-left: 10px;">
                                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                                    </asp:LinkButton>
                                </div>
                                <div class="fleft" style="margin-left: 10px;">
                                    <asp:LinkButton ID="lnkPdf" runat="server" OnClick="lnkPdf_Click" ValidationGroup="Required">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/pdf2.png" ToolTip="Export To PDF"
                                            Height="25px" />
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="m clear">
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>
                                        Closing Date From
                                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                                            Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtDateFrom"
                                            Display="Dynamic" ValidationGroup="Required" ErrorMessage="Invalid From Date."
                                            Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateFrom" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                            runat="server" />
                                        <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateFrom" PopupPosition="BottomRight"
                                            TargetControlID="txtDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                        Closing Date To
                                        <asp:RequiredFieldValidator ID="RFVDateTo" ValidationGroup="Required" runat="server"
                                            Text="*" ControlToValidate="txtDateTo" ErrorMessage="Please Enter To Date" SetFocusOnError="true"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="ComValDateTo" runat="server" ControlToValidate="txtDateTo"
                                            Display="Dynamic" ValidationGroup="Required" ErrorMessage="Invalid To Date."
                                            Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>
                                        <asp:CompareValidator ID="cvDateFromDateTo" runat="server" ControlToValidate="txtDateTo"
                                            ControlToCompare="txtDateFrom" Display="Dynamic" ValidationGroup="Required" Text="*"
                                            ErrorMessage="Closing Date To should be greater than Closing Date From." Type="Date"
                                            Operator="GreaterThanEqual"></asp:CompareValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateTo" runat="server" Width="100px" placeholder="dd/mm/yyy"></asp:TextBox>
                                        <asp:Image ID="imgDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                            runat="server" />
                                        <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateTo" PopupPosition="BottomRight"
                                            TargetControlID="txtDateTo">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Bill No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillNo_Report" runat="server" MaxLength="50" Width="200px" placeholder="Bill No"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <asp:GridView ID="gvNformDet_Report" runat="server" Width="100%" AutoGenerateColumns="false"
                                    CssClass="table" ToolTip="N Form Delivery Details">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BS Job No" ItemStyle-Width="130px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBsJobNo" runat="server" Text='<%#Eval("BS Job No") %>' ToolTip="BS Job No"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="250px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvNo" runat="server" Text='<%#Eval("Invoice No") %>' ToolTip="Invoice No"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BOE" ItemStyle-Width="50px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBOE" runat="server" Text='<%#Eval("BOE") %>' ToolTip="BOE"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="300px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Customer Name") %>'
                                                    ToolTip="Customer Name"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No of Pkg" ItemStyle-Width="30px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoofPKG" runat="server" Text='<%#Eval("No of PKG") %>' ToolTip="No Of Pkg"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weight" ItemStyle-Width="40px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWeight" runat="server" Text='<%#Eval("Weight") %>' ToolTip="Weight"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="90px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("Vehicle No") %>' ToolTip="Vehicle No"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="N Form No" ItemStyle-Width="60px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("N Form No") %>' ToolTip="N Form No"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="N Form Opening Date" ItemStyle-Width="80px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNFormOpeningDate" runat="server" ToolTip="N Form Opening Date"
                                                    Text='<%#Eval("N Form Opening Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="N Form Closing Date" ItemStyle-Width="80px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNFormClosingDate" runat="server" ToolTip="N Form Closing Date"
                                                    Text='<%#Eval("N Form Closing Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="50px" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" ToolTip="Amount" Text='<%#Eval("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="copyPower">
        <div class="copy fleft">
            Copyright © 2016 Babaji Shivram. All rights reserved.</div>
        <div class="poweredby fright">
        </div>
    </div>
    </form>
    <script type="text/javascript" language="javascript">

        // get total height 
        var totalHeight = document.body.offsetHeight;
        // get height of top and bottom div 
        //var topDivHeight = document.getElementById('1').offsetHeight; 
        //var bottomDivHeight = document.getElementById('3').offsetHeight; 
        // calculate height for center div and apply it 
        var centerDivHeight = totalHeight - 171;
        // document.getElementById('ctl00_LeftNavigation1_trMktMenu').style.height = centerDivHeight + 'px';
        document.getElementById('<%=divCPH.ClientID%>').style.height = centerDivHeight - 40 + 'px';  
    </script>
    <script type="text/javascript" language="javascript">

        Sys.Application.add_init(AppInit);

        function AppInit(sender) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler(sender, args) {
            setScrollPos();
        }

        function saveScrollPos() {

            document.getElementById('<%=scrollPos.ClientID%>').value =
                document.getElementById('<%=divCPH.ClientID%>').scrollTop;

        }
        function setScrollPos() {
            document.getElementById('<%=divCPH.ClientID%>').scrollTop =
                document.getElementById('<%=scrollPos.ClientID%>').value;

        }
    </script>
</body>
</html>

<%@ Page Title="Edit SEZ Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SEZEditJobDetail.aspx.cs" Inherits="SEZ_SEZEditJobDetail" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

<AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
<div>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
        <ProgressTemplate>
            <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                <img alt="progress" src="images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>

<style type="text/css">
    /*table.table th, table.table th a {
        text-align:center
    }*/

    .accordionHeader, .accordionHeaderSelected {
        background-position-x: 4px;
    }

    .accordionHeader {
        width: 50%;
    }
</style>
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
         </style>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="JobRequired" CssClass="errorMsg" EnableViewState="false" />
<asp:UpdatePanel ID="upJobDetail" runat="server">
   <ContentTemplate>
    <div id="divInbond" class="info" runat="server" align="center">
        <asp:Label ID="lblInbondJobNo" runat="server"></asp:Label>
    </div>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="clear"></div>

    <div style="padding: 5px">

    <AjaxToolkit:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" Width="95%"
        ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true"
        SuppressHeaderPostbacks="true" TransitionDuration="250" FramesPerSecond="40"
        RequireOpenedPane="false" AutoSize="None">
        <Panes>
            <AjaxToolkit:AccordionPane ID="accJobDetail" runat="server">
                <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Job Detail
                </Header>
                <Content>                  
                    <fieldset>
                        <legend>Job Detail</legend>
                        <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="lid"
                            Width="100%" OnDataBound="FVJobDetail_DataBound">
                            <ItemTemplate>
                                <div class="m clear">

                                    <asp:Button ID="btnEditJobDetail" runat="server" OnClick="btnEditJobDetail_Click" Text="Edit" />
                                    <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" Text="Back" />
                                </div>

                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td><b>SEZ Type</b>
                                        </td>
                                        <td>
                                            <b><%# Eval("SEZType") %></b>

                                        </td>
                                        <td><b>Request Type</b></td>
                                        <td> 
                                                <b><%# Eval("ReqTypeName") %></b>
                                        </td>

                                        <td colspan="2">
                                            <span></span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>BS Job Number.
                                        </td>
                                        <td>
                                            <%--  <%# Eval("JobRefNo") %>--%>
                                            <asp:Label ID="lblSEZJobNo" Text='<%# Bind("JobRefNo") %>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />

                                        </td>
                                        <td>Customer Name
                                        </td>
                                        <td colspan="3">
                                            <span>
                                                <%# Eval("CustName") %>
                                            </span>
                                        </td>
                                    </tr
                                    <tr>
                                            <td>Division
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Division") %>
                                            </span>
                                        </td>
                                            <td>Plant
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Plant") %>
                                            </span>
                                        </td>
                                        <td>Mode
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Mode")%>
                                            </span>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>Request ID
                                        </td>
                                        <td>
                                            <%# Eval("RequestId")%>
                                        </td>

                                            <td>BE No
                                        </td>
                                        <td>
                                            <%# Eval("BENo")%>
                                        </td>
                                        <td>BE Date
                                        </td>
                                        <td>
                                            <%--<%# Eval("BEDate","{0:dd/MM/yyyy}") %> --%><%--, "{0:dd/MM/yyyy  hh:mm tt}")%>--%>
                                            <%# (Eval("BEDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("BEDate", "{0:dd/MM/yyyy}"))%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            Assesable Value
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("AssesableValue")%>
                                            </span>
                                        </td>
                                        <td>Ex Rate
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("ExRate")%>
                                            </span>
                                        </td>

                                        <td>Currency
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Currency")%>
                                            </span>
                                        </td>
                                    </tr>

                                    <tr id="trinwardfalse" runat="server">
                                        <td>
                                            <asp:Label ID="lbl1InwardBE" runat="server" Text="Inward BE No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl2InwardBE" runat="server" Text='<%# Eval("InwardBENo")%>'></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="lbl1InwardDate" runat="server" Text="Inward BE Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl2InwardDate" runat="server" Text='<%# Eval("InwardBEDate", "{0:dd/MM/yyyy}")%>'></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="lbl1InwardJobNo" runat="server" Text="Inward Job No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl2InwardJobNo" runat="server" Text='<%# Eval("InwardJobNo")%>'></asp:Label>

                                        </td>
                                    </tr>                                   
                                    <tr id="trDTASale" runat="Server">
                                                   
                                        <td>
                                            <asp:Label ID="lblCIFVal1" runat="server" Text="CIF Value"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCIFVal" runat="server" Text='<%# Eval("CIFValue")%>'></asp:Label>
                                        </td>
                                                  
                                        <td>
                                            <asp:Label ID="lblGrossUnit1" runat="server" Text="Goods Measurement Unit"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGrossUnit" runat="server" Text='<%# Eval("GrossWtUnit")%>'></asp:Label>
                                        </td>
                                            <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="trDTAP" runat="server">
                                        <td>
                                            <asp:Label ID="lblDutyAmnt1" runat="server" Text="Duty Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDutyAmnt" runat="server" Text='<%# Eval("DutyAmount")%>'></asp:Label>                                                        
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="trBOE" runat="server">
                                        <td>
                                            <asp:Label ID="lblSupplierName1" runat="server" Text="Supplier Name"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblSupplierName" runat="server" Text='<%# Eval("SupplierName")%>'></asp:Label>                                                        
                                        </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                     </tr> 
                                      <tr id="trDtASale1" runat="Server">
                                        <td>
                                            <asp:Label ID="lblDiscount1" runat="server" Text="Discount Applicable"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("Discount1")%>'></asp:Label>                                                      
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReImport1" runat="server" Text="Re-Import"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReImport" runat="server" Text='<%# Eval("ReImport1")%>'></asp:Label>                                                       
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevImport1" runat="server" Text="Previous Import"></asp:Label>                                                        
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevImport" runat="server" Text='<%# Eval("PrevImport1")%>'></asp:Label>                                                       
                                        </td>
                                    </tr>
                                        <tr id="trSBill2" runat="server">
                                        <td>
                                            <asp:Label ID="lblBuyerName1" runat="server" Text="Buyer Name"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblBuyerName" runat="server" Text='<%# Eval("BuyerName")%>'></asp:Label>                                                        
                                        </td>
                                            <td>
                                            <asp:Label ID="lblSchemeCode1" runat="server" Text="Scheme Code"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("SchemeCode")%>'></asp:Label>                                                        
                                        </td>
                                    </tr>
                                    <tr id="trSBill" runat="Server">
                                        <td>
                                            <asp:Label ID="lblPrevExpGoods1" runat="server" Text="Previous Export Goods"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevExpGoods" runat="server" Text='<%# Eval("PrevExpGoods1")%>'></asp:Label>                                                      
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCessDetail1" runat="server" Text="Cess Details"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCessDetail" runat="server" Text='<%# Eval("CessDetail1")%>'></asp:Label>                                                       
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLicenceRegNo1" runat="server" Text="Licence Registration No"></asp:Label>                                                        
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLicenceRegNo" runat="server" Text='<%# Eval("LicenceRegNo1")%>'></asp:Label>                                                       
                                        </td>
                                    </tr>
                                    <tr id="trSBill1" runat="Server">
                                        <td>
                                            <asp:Label ID="lblReExport1" runat="server" Text="Re-Export"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReExport" runat="server" Text='<%# Eval("ReExport1")%>'></asp:Label>                                                      
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevExport1" runat="server" Text="Previous Export"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevExport" runat="server" Text='<%# Eval("PrevExport1")%>'></asp:Label>                                                       
                                        </td>
                                        <td>
                                                       
                                        </td>
                                        <td>
                                                                                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Dispatch Date
                                        </td>
                                        <td>
                                            <%# (Eval("OutwardDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("OutwardDate", "{0:dd/MM/yyyy}"))%>
                                            
                                        </td>
                                        <td>PCD From Dahej
                                        </td>
                                        <td>
                                            <%# (Eval("PCDFrDahej", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PCDFrDahej", "{0:dd/MM/yyyy}"))%>
                                            
                                    
                                        </td>
                                        <td>PCD Sent Client
                                        </td>
                                        <td>
                                            <%# (Eval("PCDSentClient", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PCDSentClient", "{0:dd/MM/yyyy}"))%>
                                            <%--<%# Eval("PCDSentClient", "{0:dd/MM/yyyy}")%>--%>
                                    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>File Sent To Billing
                                        </td>
                                        <td>
                                            <%# (Eval("FileSentToBilling", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("FileSentToBilling", "{0:dd/MM/yyyy}"))%>
                                            <%--<%# Eval("FileSentToBilling", "{0:dd/MM/yyyy}")%>--%>
                                    
                                        </td>
                                        <td>Billing Status
                                        </td>
                                        <td>
                                            <%# Eval("BillingStatus")%>
                                        </td>
                                        <td></td>
                                        <td></td>                                      
                                    </tr>
                                    <tr>
                                        <td>Remark
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("Remark")%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                </table>
                              
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="m clear">
                                    <asp:Button ID="btnUpdateJobDetail" runat="server"
                                        Text="Update" ValidationGroup="JobRequired" OnClick="btnUpdateJobDetail_Click" />
                                    <asp:Button ID="btnCancelJobDetail" runat="server"
                                        CausesValidation="False" Text="Cancel" OnClick="btnCancelJobDetail_Click" />
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td><b>SEZ Type</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSEZType" runat="server" Text='<%# Bind("SEZType") %>' Font-Bold="true"></asp:Label>

                                        </td>
                                        <td><b>Request Type</b></td>
                                        <td> 
                                                <asp:Label ID="lblReqType" runat="server" Text='<%# Bind("ReqTypeName") %>' Font-Bold="true"></asp:Label>                                                         
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td>BS Job Number
                                    <%-- <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ControlToValidate="txtJobRefNo"
                                        SetFocusOnError="true" ErrorMessage="Please Enter BS Job Number" Display="Dynamic"
                                        Text="*" ValidationGroup="JobRequired"></asp:RequiredFieldValidator>--%>
                                            <%--<asp:RegularExpressionValidator ID="regJobRefNo" runat="server" ControlToValidate="txtJobRefNo"
                                        ValidationGroup="JobRequired" ValidationExpression="\d{5}/[A-Z]{3}/\d{2}-\d{2}"
                                        Text="*" Display="Dynamic" ErrorMessage="Please Enter Valid BS Job Number- 00001/BOI/14-15"
                                        SetFocusOnError="true">
                                        </asp:RegularExpressionValidator>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtJobNo" Text='<%# Bind("JobRefNo") %>' MaxLength="18" runat="server" Enabled="false" />
                                        </td>
                                        <td>Customer Name
                                    <%--<asp:RequiredFieldValidator ID="RFVCustomer" ValidationGroup="JobRequired" runat="server"
                                        Display="Dynamic" ControlToValidate="ddCustomer" InitialValue="0" ErrorMessage="Please Select Customer"
                                        Text="*"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td colspan="3">
                                            <%--<asp:DropDownList ID="ddCustomer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtClientName" runat="server" placeholder="Customer Name" Width="300px"
                                                Text='<%# Bind("CustName") %>' Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%#Eval("ClientName") %>' />
                                            <asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />
                                        </td>
                                    </tr>

                                        <td>Division
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDivision" runat="server" placeholder="Division" 
                                                Text='<%# Bind("Division") %>' Enabled="false"></asp:TextBox>
                                        </td>
                                            <td>Plant
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPlant" runat="server" placeholder="Plant" 
                                                Text='<%# Bind("Plant") %>' Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>Mode
                                    <%-- <asp:RequiredFieldValidator ID="RFVMode" ValidationGroup="JobRequired" runat="server"
                                        Display="Dynamic" ControlToValidate="ddMode" InitialValue="0" ErrorMessage="Please Select Mode"
                                        Text="*"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddMode" runat="server" AutoPostBack="true" Width="100px" DataSourceID="ModeSqlDataSource"
                                                    DataTextField="TransMode" DataValueField="lid" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnMode" runat="server" Value='<%#Eval("SEZMode") %>' />

                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Please Select Mode" Text="*" ForeColor="red"
                                                            InitialValue="0" ControlToValidate="ddMode" ValidationGroup="JobRequired"></asp:RequiredFieldValidator>--%>

                                                <asp:SqlDataSource ID="ModeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                        SelectCommand="GetTransModeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                                        </td>
                                    </tr>

                                        <%--<tr>
                                        <td>Person                                              
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPerson" runat="server" placeholder="Person Name"
                                                Text='<%# Bind("Person") %>' Enabled="false"></asp:TextBox>
                                        </td>                                                    
                                    </tr>--%>
                                    <tr>
                                        <td>Request ID
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRequestId" runat="server" Text='<%# Bind("RequestId") %>' ></asp:TextBox>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="JobRequired" runat="server" Display="Dynamic"
                                                    Text="*" ControlToValidate="txtRequestId"  ErrorMessage="Enter The RequestId" ForeColor="red">*
                                                </asp:RequiredFieldValidator>
                                        </td>

                                        <td>BE No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBENo" runat="server" Text='<%# Bind("BENo") %>'></asp:TextBox>
                                        </td>
                                        <td>BE Date
                                        </td>
                                        <td>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBEDate" PopupPosition="BottomRight"
                                                TargetControlID="txtBEDate">
                                            </AjaxToolkit:CalendarExtender>

                                            <asp:TextBox ID="txtBEDate" Text='<%# (Eval("BEDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("BEDate", "{0:dd/MM/yyyy}"))%>' Width="100px" runat="server" placeholder="dd/mm/yyyy" />
                                            <asp:Image ID="imgBEDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                        </td>
                                    </tr>

                                    <tr>

                                        <td>Assesable Value
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAssesableValue" runat="server" Text='<%# Bind("AssesableValue") %>'></asp:TextBox>                                                         

                                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="JobRequired" runat="server" Display="Dynamic"
                                                    Text="*" ControlToValidate="txtAssesableValue"  ErrorMessage="Enter The Assessable Value" ForeColor="red">*
                                                    </asp:RequiredFieldValidator>--%>
                                                         
                                                <asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                                                ErrorMessage="Please enter Assesable Value in integer or decimal number with 4 decimal places." ForeColor="Red"
                                                ControlToValidate="txtAssesableValue" ValidationGroup="JobRequired">*
                                                </asp:RegularExpressionValidator>
                                        </td>

                                        <td>Ex Rate
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExRate" runat="server" Text='<%# Bind("ExRate") %>'></asp:TextBox>

                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="JobRequired" runat="server" Display="Dynamic"
                                                    Text="*" ControlToValidate="txtExRate"  ErrorMessage="Enter The Ex Rate" ForeColor="red">*
                                                    </asp:RequiredFieldValidator>--%>
                                                         
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                                                ErrorMessage="Please enter Ex Rate in integer or decimal number with 4 decimal places." ForeColor="Red"
                                                ControlToValidate="txtExRate" ValidationGroup="JobRequired">*
                                                </asp:RegularExpressionValidator>
                                        </td>

                                        <td>Currency
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" Width="200px">

                                                <%--<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                                <asp:ListItem Text="USD" Value="1"></asp:ListItem>
                                                <%--<asp:ListItem Text="Sea" Value="2"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnCurrency" runat="server" Value='<%#Eval("SEZCurrency") %>' />
                                        </td>
                                    </tr>

                                    <tr id="aaa" runat="server">
                                        <td>
                                            <asp:Label ID="lblInwardBeno" runat="server" Text="Inward BE No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInwardBENo" runat="server" Text='<%# Bind("InwardBENo") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInwardBeDate" runat="server" Text="Inward BE Date"></asp:Label>
                                        </td>
                                        <td>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtInwardBEDate" PopupPosition="BottomRight"
                                                TargetControlID="txtInwardBEDate">
                                            </AjaxToolkit:CalendarExtender>

                                            <asp:TextBox ID="txtInwardBEDate" Text='<%# Bind("InwardBEDate", "{0:dd/MM/yyyy}") %>' Width="100px"
                                                runat="server" placeholder="dd/mm/yyyy" />
                                            <%-- <asp:Image ID="imgInwardBEDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />--%>

                                        </td>
                                        <td>
                                            <asp:Label ID="lblInwardJobNo" runat="server" Text="Inward Job No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInwardJobNo" runat="server" Text='<%# Bind("InwardJobNo") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--   <tr id="BBB" runat="server">

                                        <td>
                                            <asp:Label ID="lblDaysStore" runat="server" Text="Days Store"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDaysStore" runat="server" Text='<%# Bind("DaysStore") %>'></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <tr id="trDTA" runat="server">
                                            <td>
                                                <asp:Label ID="lblCIFValue" runat="server" Text="CIF Value"></asp:Label>
                                            </td>
                                            <td>
                                                    <asp:TextBox ID="txtCIFValue" runat="server" Text='<%# Bind("CIFValue") %>'></asp:TextBox>                                                         
                                            </td>
                                            <td>
                                            <asp:Label ID="lblGrossUnit" runat="server" Text="Goods Measurement Unit"></asp:Label> 
                                            </td>                                                   
                                                    
                                        <td>
                                            <asp:DropDownList ID="ddlGrossUnit" runat="server" AutoPostBack="true" Width="150px" >
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>                                                           
                                                      
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnGrossUnit" runat="server" Value='<%#Eval("UName") %>' />    
                                                        
                                            </td>  
                                                   
                                        <td></td>
                                        <td></td>
                                                    
                                    </tr>
                                    <tr id="trDTAPedit" runat="server">
                                        <td>
                                            <asp:Label ID="lblDutyAmount" runat="server" Text="Duty Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDutyAmount" runat="server" Text='<%# Bind("DutyAmount") %>'></asp:TextBox>

                                                 
                                            <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="((\d+)((\.\d{1,4})?))$"
                                                ErrorMessage="Please enter Duty Amount in integer or decimal number with 4 decimal places." ForeColor="Red"
                                                ControlToValidate="txtDutyAmount" ValidationGroup="JobRequired">*
                                                </asp:RegularExpressionValidator>--%>

                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>                                               
                                    </tr>
                                    <tr id="trBOEntry" runat="server">
                                        <td>
                                            <asp:Label ID="lblSupplyName" runat="server" Text="Supplier Name"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSupplierName" runat="server" Text='<%# Bind("SupplierName") %>' width="250px"></asp:TextBox>                                               
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                        <%-- 
                                        <td>Inward Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInwardDate" runat="server"  Text='<%# (Eval("InwardDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("InwardDate", "{0:dd/MM/yyyy}"))%>' placeholder="dd/mm/yyyy"></asp:TextBox>

                                                <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtInwardDate" PopupPosition="BottomRight"
                                                TargetControlID="txtInwardDate">
                                            </AjaxToolkit:CalendarExtender>

                                        </td>
                                        <td>No. Of Packages
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNoOfPackage" runat="server" Text='<%# Bind("NoOfPackages") %>'></asp:TextBox>

                                        </td>
                                    </tr>--%>


                                    <%--  <tr>
                                            <td>Duty As Per Customs
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDutyCustom" runat="server" Width="160px">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Forgone(F)" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Payable(P)" Value="2"></asp:ListItem>                              
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnDutyCustom" runat="server" Value='<%#Eval("DutyPF") %>' />
                                            </td>
                                            <td>Packages Unit
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPackagesUnit" runat="server" AutoPostBack="true" Width="100px" >
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>                                                           
                                                         
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnPackagesUnit" runat="server" Value='<%#Eval("PackageUnit") %>' />                                                          
                                                            
                                            </td>
                                                       
                                        </tr>--%>



                                    <%--   <tr>
                                        <td>Gross Weight (kgs)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGrossWeight" runat="server" Text='<%# Bind("GrossWeight") %>'></asp:TextBox>

                                        </td>
                                        <td>No. Of Vehicles
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNoOfVehicles" runat="server" Text='<%# Bind("NoOfVehicles") %>'></asp:TextBox>

                                        </td>
                                        <td>BE Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlServicesProvide" runat="server" AutoPostBack="true" Width="150px">

                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Import" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Re-Export" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Home Consumption" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnServicesProvide" runat="server" Value='<%#Eval("SEZServicesProvide") %>' />

                                        </td>
                                    </tr>--%>


                                                
                                <%--     <tr>
                                        <td>
                                            Buyer Name
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtBuyerName" runat="server" Text='<%# Bind("BuyerName") %>'></asp:TextBox>                                                         
                                        </td>
                                        <td> 
                                            CHA Code
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCHACode" runat="server" Text='<%# Bind("CHACode")%>'></asp:Label>
                                                        
                                        </td>
                                    </tr>--%>
                                    <%-- <tr>
                                                   
                                        <td>Destination</td>
                                        <td>
                                            <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="true" Width="150px" >
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>                                                           
                                                      
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnDestination" runat="server" Value='<%#Eval("DName") %>' />                                                        
                                                      
                                        </td>
                                        <td>Country of Origin</td>
                                        <td>
                                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="150px" >
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>                                                           
                                                      
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnCountry" runat="server" Value='<%#Eval("CName") %>' />                                                        
                                                      
                                        </td>
                                    </tr>--%>
                                    <%--  <tr>
                                        <td>Place of Origin</td>
                                            <td>
                                            <asp:DropDownList ID="ddlPlace" runat="server" AutoPostBack="true" Width="150px" >
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>                                                           
                                                      
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnPlace" runat="server" Value='<%#Eval("PName") %>' />                                                        
                                                      
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>--%>
                                    <tr id="trDTA1" runat="server">
                                        <td>
                                            <asp:Label ID="lblDiscoutAppli" runat="server" Text="Discount Applicable"></asp:Label> 
                                        </td>
                                        <td>
                                                <asp:RadioButtonList ID="rdlDiscountAppli" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table" Width="150px" class="text" >
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2" ></asp:ListItem>
                                                </asp:RadioButtonList>
                                            <asp:HiddenField ID="hdndiscount" runat="server" Value='<%#Eval("Discount") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReImport1" runat="server" Text="Re-Import"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdlReImport" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table" Width="150px" class="text" >
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2" ></asp:ListItem>
                                                </asp:RadioButtonList>
                                            <asp:HiddenField ID="hdnReImport" runat="server" Value='<%#Eval("ReImport") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPreviousImport" runat="server" Text="Previous Import"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdlPreviousImport" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table" Width="150px" class="text" >
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="2" ></asp:ListItem>
                                                </asp:RadioButtonList>
                                            <asp:HiddenField ID="hdnPrevImport" runat="server" Value='<%#Eval("PrevImport") %>' />
                                        </td>
                                    </tr>


                                            <tr id="trSB2" runat="server">
                                        <td>
                                            <asp:Label ID="lblBuyerName" runat="server" Text="Buyer Name"></asp:Label> 
                                        </td>
                                        <td colspan="2">
                                                <asp:TextBox ID="txtBuyerName" runat="server" Text='<%# Bind("BuyerName") %>' Width="300px"></asp:TextBox> 
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSchemeCode" runat="server" Text="Re-Import"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSchemeCode" runat="server" Text='<%# Bind("SchemeCode") %>' Width="300px"></asp:TextBox> 
                                        </td>                                                    
                                    </tr>


                                        <tr id="trSB" runat="server">
                                        <td>
                                            <asp:Label ID="lblPrevExpGoods" runat="server" Text="Previous Export Goods"></asp:Label> 
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
                                            <asp:Label ID="lblCessDetail" runat="server" Text="Cess Details"></asp:Label>
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
                                            <asp:Label ID="lblLicenceRegNo" runat="server" Text="Licence Registration No"></asp:Label>
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
                                            <asp:Label ID="lblReExport" runat="server" Text="Re-Export"></asp:Label> 
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
                                            <asp:Label ID="lblPrevExport" runat="server" Text="Previous Export"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdlPrevExport" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table" Width="150px" class="text" >
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

                                    <tr>
                                        <td>Dispatch Date
                                        </td>
                                        <td>                                                      

                                            <asp:TextBox ID="txtOutwardDate" Text='<%# (Eval("OutwardDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("OutwardDate", "{0:dd/MM/yyyy}"))%>' Width="100px"
                                                runat="server" placeholder="dd/mm/yyyy" />

                                                <asp:Image ID="imgOutwardDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOutwardDate" PopupPosition="BottomRight"
                                                TargetControlID="txtOutwardDate">
                                            </AjaxToolkit:CalendarExtender>

<%--                                                         <asp:RegularExpressionValidator runat="server" ControlToValidate="txtOutwardDate" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                                                                ErrorMessage="Invalid date format" ValidationGroup="JobRequired" ForeColor="Red">*
                                                            </asp:RegularExpressionValidator>


                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="JobRequired" runat="server" Display="Dynamic"
                                                                Text="*" ControlToValidate="txtOutwardDate"  ErrorMessage="Enter The Dispatch Date" ForeColor="red">*
                                                            </asp:RequiredFieldValidator>--%>


                                            <%-- <asp:Image ID="imgOutwardDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />--%>

                                        </td>
                                        <td>PCD From Dahej
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txtPCDFrDahej" runat="server" Text='<%# Bind("PCDFrDahej", "{0:dd/MM/yyyy}") %>'></asp:TextBox>--%>
                                            <asp:TextBox ID="txtPCDFrDahej" runat="server" Text='<%# (Eval("PCDFrDahej", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PCDFrDahej", "{0:dd/MM/yyyy}"))%>'></asp:TextBox>

                                                            

                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgpcdfrDahej" PopupPosition="BottomRight"
                                                TargetControlID="txtPCDFrDahej">
                                            </AjaxToolkit:CalendarExtender>

                                            <asp:Image ID="imgpcdfrDahej" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                        </td>
                                        <td>PCD Sent Client
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPCDSentClient" runat="server" Text='<%# (Eval("PCDSentClient", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PCDSentClient", "{0:dd/MM/yyyy}"))%>'></asp:TextBox>

                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPCDSentClient" PopupPosition="BottomRight"
                                                TargetControlID="txtPCDSentClient">
                                            </AjaxToolkit:CalendarExtender>

                                            <asp:Image ID="imgPCDSentClient" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>File Sent To Billing
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFileSentToBilling" runat="server" Text='<%# (Eval("FileSentToBilling", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("FileSentToBilling", "{0:dd/MM/yyyy}"))%>' 
                                                ToolTip="After File Sent To Billing, You Can Not Change Any Kind of Information" OnTextChanged="txtFileSentToBilling_TextChanged" AutoPostBack="true"></asp:TextBox>

                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFileSentToBilling" PopupPosition="BottomRight"
                                                TargetControlID="txtFileSentToBilling">
                                            </AjaxToolkit:CalendarExtender>

                                            <asp:Image ID="imgFileSentToBilling" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />


                                        </td>
                                        <td>Billing Status
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillingStatus" runat="server" Text='<%# Bind("BillingStatus") %>'></asp:TextBox>

                                        </td>
                                        <td><%--R.N.Logistics--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRNLogistics" runat="server" Text='<%# Bind("RNLogistics") %>' Visible="false"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Remark
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="500px" Text='<%# Bind("Remark") %>'></asp:TextBox>

                                        </td>
                                    </tr>
                                </table>

                            </EditItemTemplate>
                        </asp:FormView>
                    </fieldset>
                </Content>
            </AjaxToolkit:AccordionPane>

            <AjaxToolkit:AccordionPane ID="AccINVDetails" runat="server">
                <Header>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Invoice & Product Details
                </Header>
                <Content>
                   <fieldset>
                        <legend>Add Invoice Details</legend>

                        <asp:Label ID="lblTitle" runat="server"></asp:Label></legend>
                        <div class="m clear">
                            <asp:Button ID="btnSubmit" Text="Save" runat="server" ValidationGroup="Required" OnClick="btnSave_Click" />
                            <%--<asp:Button ID="btnNew" Text="NEW" CausesValidation="false" runat="server" OnClick="btnNew_Click" Visible="false" />--%>
                        </div>
                        <asp:Panel ID="pnlInvoice" runat="server" ScrollBars="Auto">
                          <asp:GridView ID="GrdInvoiceDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false" Width="100%"
                                CssClass="grid">
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
                        </asp:Panel>

                                 <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="EX_GetShipmentTerms" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                                 <asp:SqlDataSource ID="ItemTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                     SelectCommand="SEZ_GetItemTypeInvoice" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                   </fieldset>
                   <fieldset>
                        <legend>Inoice & Product Detail</legend>

                        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" 
                            DataKeyNames="JobId"  AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                            PagerSettings-Position="TopAndBottom" DataSourceID="SqlDataSourceINV" OnPreRender="gvJobDetail_PreRender"
                            OnRowEditing="gvJobDetail_RowEditing" OnRowCancelingEdit="gvJobDetail_RowCancelingEdit"
                            OnRowUpdating="gvJobDetail_RowUpdating" OnRowCommand="gvJobDetail_RowCommand">  
                            <%-- OnRowDataBound="gvJobDetail_RowDataBound"  --%>
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" 
                                            ToolTip="Click To Change Invoice Details"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Invoice & Product Detail" runat="server"
                                            Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Invoice & Product Detail" CausesValidation="false"
                                            runat="server" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>                                             

                                <asp:BoundField DataField="InvNo" HeaderText="Invoice No" SortExpression="InvNo" ReadOnly="true" /> 

                                <%--  <asp:BoundField DataField="InvId" HeaderText="InvId" SortExpression="InvId" ReadOnly="true" Visible="false"/> 
                                <asp:BoundField DataField="JobId" HeaderText="Job Id" SortExpression="JobId" ReadOnly="true" Visible="false"/> --%>

                                <asp:TemplateField HeaderText="Invoice Date" SortExpression="InvDate">
                                        <ItemTemplate>                        
                                                        
                                            <asp:Label ID="lblInvDate" Text='<%# Bind("InvDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                                <asp:HiddenField ID="hdnInvId" Value='<%# Bind("InvId")%>' runat="server"/>
                                            <asp:HiddenField ID="hdnJobId" Value='<%# Bind("JobId")%>' runat="server"/>     
                                            <asp:TextBox ID="txtInvDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("InvDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calInvDate" runat="server" Enabled="True" 
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvDate" PopupPosition="BottomRight"
                                                TargetControlID="txtInvDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgInvDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                            <%-- <AjaxToolkit:MaskedEditExtender ID="MskExtINVDate" TargetControlID="txtInvDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                                            <AjaxToolkit:MaskedEditValidator ID="MskValBOEDate" ControlExtender="MskExtINVDate" ControlToValidate="txtInvDate" IsValidEmpty="false" 
                                                EmptyValueMessage="Please Enter Invoice Date" EmptyValueBlurredText="*" InvalidValueMessage="Invoice Date is invalid" 
                                                InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Invoice Date" MaximumValueMessage="Invalid Invoice Date"                                     
                                                Runat="Server" SetFocusOnError="true" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>--%>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Value" SortExpression="InvValue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceValue" Text='<%# Bind("InvValue")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtInvVal" runat="server" Width="70px" MaxLength="10" Text='<%# Bind("InvValue")%>'></asp:TextBox>                                                                                     
                                        </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Term" HeaderText="Term" SortExpression="Term" ReadOnly="true" /> 
                                <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" Text='<%# Bind("Description")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Width="500px" Text='<%# Bind("Description")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ControlToValidate="txtInvVal" SetFocusOnError="true"
                                                ErrorMessage="Please Enter the Description." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Total Quantity" SortExpression="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" Text='<%# Bind("Quantity")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" Width="70px"  Text='<%# Bind("Quantity")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ControlToValidate="txtQuantity" SetFocusOnError="true"
                                                ErrorMessage="Please Enter the Quantity." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remaining Quantity" SortExpression="RemainingQty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremQuantity" Text='<%# Bind("RemainingQty")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>                                                       
                                            <asp:TextBox ID="txtremQuantity" runat="server" Width="70px" Text='<%# Bind("RemainingQty")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Text="*" ControlToValidate="txtremQuantity" SetFocusOnError="true"
                                                ErrorMessage="Please Enter the Remaining Quantity." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>               
                                <asp:TemplateField HeaderText="Item Price" SortExpression="ItemPrice">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemPrice" Text='<%# Bind("ItemPrice")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtItemPrice" runat="server" Width="70px" Text='<%# Bind("ItemPrice")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="*" ControlToValidate="txtItemPrice" SetFocusOnError="true"
                                                ErrorMessage="Please Enter the Item Price." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Product Value" SortExpression="ProductValue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductValue" Text='<%# Bind("ProductValue")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProductValue" runat="server" Width="70px" Text='<%# Bind("ProductValue")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Text="*" ControlToValidate="txtProductValue" SetFocusOnError="true"
                                                ErrorMessage="Please Enter The Product Value." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField> 
                                <asp:TemplateField HeaderText="CTH" SortExpression="CTH">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCTH" Text='<%# Bind("CTH")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCTH" runat="server" Width="70px" Text='<%# Bind("CTH")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Text="*" ControlToValidate="txtCTH" SetFocusOnError="true"
                                                ErrorMessage="Please Enter The CTH." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Item Name" SortExpression="ItemName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" Text='<%# Bind("ItemName")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <%-- <EditItemTemplate>
                                            <asp:TextBox ID="txtItemName" runat="server" Width="70px" MaxLength="7" Text='<%# Bind("ItemName")%>'></asp:TextBox>                              
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Text="*" ControlToValidate="txtItemName" SetFocusOnError="true"
                                                ErrorMessage="Please Enter The Item Value." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>--%>
                                    </asp:TemplateField>
                                                                      
                                </Columns>
                            </asp:GridView>
                              <asp:SqlDataSource ID="SqlDataSourceINV" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                     SelectCommand="SEZ_GetInvoiceDetail" SelectCommandType="StoredProcedure">
                                 <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                    </fieldset>
                </Content>
            </AjaxToolkit:AccordionPane>

            <AjaxToolkit:AccordionPane ID="accDocuments" runat="server">
                <Header>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Document Detail</Header>
                <Content>

                    <fieldset>
                        <legend>Download Document</legend>
                        <fieldset>
                            <legend>Upload Document</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="70%" bgcolor="white">
                                <tr>
                                    <td><b>Document Type : </b>
                                            <asp:DropDownList ID="ddDocument" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuDocumentSEZ" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                                           
                            </table>
                        </fieldset>

                        <asp:GridView ID="grdDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                            OnRowCommand="GrdDocument_RowCommand" DataSourceID="DocumentSqlDataSource" DataKeyNames="JobId"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                            <%--DataSourceID="DocumentSqlDataSource" DataKeyNames="DocId"--%>
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                                <asp:BoundField DataField="JobId" HeaderText="Doc Id" />

                                <asp:BoundField DataField="DocType" HeaderText="Doc TypeId" Visible="false" />
                                <asp:BoundField DataField="DocTypeName" HeaderText="Doc Type" />

                                <asp:BoundField DataField="DocPath" HeaderText="Doc Path" />
                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                                <asp:BoundField DataField="sName" HeaderText="User Name" />
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileDirName" runat="server" Text='<%#Eval("FileDirName") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Download">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                            CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        <%--<asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetUploadedDocument" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </div>
                </Content>
            </AjaxToolkit:AccordionPane>      
                        
            <AjaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Billing Details
                </Header>
                    <Content>                               
                        <fieldset id="BillingScrutiny" runat="server">
                            <legend>Billing Scrutiny</legend>                                        
                        
                            <asp:Label ID="lblfreight" runat="server"></asp:Label>
                            <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" ><%--OnRowDataBound="gvbillingscrutiny_RowDataBound">--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Billing Advice" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Billing Advice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Billing Scrutiny" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Scrutiny Completed Date" DataField="ScrutinyDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Scrutiny Completed By" DataField="ScrutinyCompletedBy" />
                                    <asp:BoundField HeaderText="FreightJob" DataField="FreightjobNo" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                        <fieldset id="DraftInvoice" runat="server">
                                <legend>Draft Invoice</legend>
                                <asp:Label ID="lblConsolidated" runat="server"></asp:Label>
                                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" >
                                    <Columns>  <%-- OnRowCommand="gvDraftInvoice_RowCommand" OnRowDataBound="gvDraftInvoice_RowDataBound" --%>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Scrutiny" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Invoice" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Consolidated Job No" DataField="ConsolidatedjobNo" />
                                        <asp:ButtonField Text="Next" ButtonType="Button" CommandName="DraftInvoiceNext" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                        <fieldset id="DraftCheck" runat="server">
                             <legend>Draft Check</legend>
                             <asp:GridView ID="gvDraftcheck" runat="server" AutoGenerateColumns="False"
                                 CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftCheck"
                                 CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                 <Columns>
                                     <asp:TemplateField HeaderText="Sl">
                                         <ItemTemplate>
                                             <%#Container.DataItemIndex + 1 %>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:BoundField HeaderText="Status" DataField="Status" />
                                     <asp:BoundField HeaderText="Sent By Draft Invoice" DataField="SentBy" />
                                     <asp:BoundField HeaderText="Draft Invoice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                     <asp:BoundField HeaderText="Received By Draft Check" DataField="ReceivedBy" />
                                     <asp:BoundField HeaderText="Draft Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                     <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                 </Columns>
                             </asp:GridView>
                          </fieldset>

                        <fieldset id="FinalInvoiceTyping" runat="server">
                                <legend>Final Invoice Typing</legend>
                                <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40"> <%-- OnRowCommand="gvFinaltyping_RowCommand"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Typing" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Typing Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed Date" DataField="FinalTypingDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed by" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Comment" DataField="Comment" />
                                        <asp:ButtonField Text="Next" ButtonType="Button" CommandName="FinalTypingNext" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                        <fieldset id="FinalInvoiceCheck" runat="server">
                            <legend>Final Invoice Check</legend>
                            <asp:GridView ID="gvfinalcheck" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalCheck"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Final Typing" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Final Typing Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Final Check" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Final Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                        <fieldset id="Billdispatch" runat="server">
                            <legend>Bill Dispatch</legend>
                            <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Final Check" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Final Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Bill Dispatch" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                        <fieldset id="BillRejection" runat="server">
                            <legend>Bill Rejection</legend>
                            <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillRejection"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Stage" DataField="Stage" />
                                    <asp:BoundField HeaderText="Rejected by" DataField="RejectedBy" />
                                    <asp:BoundField HeaderText="Bill Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Reason" DataField="Reason" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                    <asp:BoundField HeaderText="Followup Date" DataField="FollowupDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Followup Remark" DataField="FollowupRemark" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                     <%--   <fieldset id="fsRepository" runat="server">
                         <legend>Billing Repository</legend>
                            <asp:Label ID="lblBillReportMsg" runat="server"></asp:Label>
                                <asp:GridView ID="gvBillingRepository" runat="server" AutoGenerateColumns="False" CssClass="table"
                                  Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                  OnRowCommand="gvBillingRepository_RowCommand" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SI">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Document" />
                                        <asp:BoundField DataField="CreationTime" HeaderText="Date" />
                                        <asp:TemplateField HeaderText="Download">    
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadRepo" runat="server" Text="Download" CommandName="downloadrepo"
                                                CommandArgument='<%#Eval("FullName") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                        </fieldset>--%>

                        <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId"/>
                            </SelectParameters>
                         </asp:SqlDataSource>
                        
                        <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                         </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                         </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                               
                   </Content>
               </AjaxToolkit:AccordionPane>
                      
            <AjaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
            <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Container Details
            </Header>
            <Content>

            <fieldset>
            <legend>Container Detail</legend>

        <fieldset>
            <legend>Add Container</legend>
        <asp:Label ID="lblContainerMessage" runat="server" CssClass="errorMsg"></asp:Label>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>Container No
                            <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                ValidationGroup="valContainer" SetFocusOnError="true" ErrorMessage="Enter Container No"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                        ValidationGroup="valContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                        Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                </td>
                <td>Container Type
                </td>
                <td>
                    <asp:DropDownList ID="ddContainerType" runat="server">
                        <%-- AutoPostBack="true" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged"--%>
                        <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                        <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                        <asp:ListItem Text="ISO" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Container Size
                </td>
                <td>
                    <asp:DropDownList ID="ddContainerSize" runat="server">
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                        <asp:ListItem Text="20" Value="1"></asp:ListItem>
                        <asp:ListItem Text="40" Value="2"></asp:ListItem>
                        <asp:ListItem Text="45" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btnAddContainer" Text="Add Container" OnClick="btnAddContainer_Click"
                        ValidationGroup="valContainer" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>

        <div>
            <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="JobId" Width="100%"
                PageSize="40" DataSourceID="DataSourceContainer" AllowSorting="true"
                OnPreRender="gvContainer_PreRender" OnRowEditing="gvContainer_RowEditing" OnRowCancelingEdit="gvContainer_RowCancelingEdit" 
                OnRowUpdating="gvContainer_RowUpdating" OnRowCommand="gvContainer_RowCommand">
                <%--OnRowDataBound="gvContainer_RowDataBound" OnRowUpdating="gvContainer_RowUpdating" 
                    OnRowDeleting="gvContainer_RowDeleting"--%>

                <%--OnPreRender="gvJobDetail_PreRender"
                                OnRowEditing="gvJobDetail_RowEditing" OnRowCancelingEdit="gvJobDetail_RowCancelingEdit"
                            OnRowUpdating="gvJobDetail_RowUpdating" OnRowCommand="gvJobDetail_RowCommand"--%>

                <Columns>
                        <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" 
                                            ToolTip="Click To Change Container Details"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Container Detail" runat="server"
                                            Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Container Detail" CausesValidation="false"
                                            runat="server" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>                                       



                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Container No" SortExpression="ContainerNo">
                        <ItemTemplate>
                            <asp:Label ID="lblContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'
                                MaxLength="11" Width="100px" ReadOnly="true"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="REVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RFVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="*" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("ContainerTypeName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEditContainerType" runat="server" SelectedValue='<%#Eval("ContainerType") %>'
                                Width="80px">
                                <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Size">
                        <ItemTemplate>
                            <asp:Label ID="lblSize" runat="server" Text='<%#Eval("ContainerSizeName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEditContainerSize" runat="server" SelectedValue='<%#Eval("ContainerSize") %>'
                                Width="80px">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                <asp:ListItem Text="45" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User">
                        <ItemTemplate>
                            <asp:Label ID="lblContrUser" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblContrDate" runat="server" Text='<%#Eval("lUser","{0:dd/MM/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                              
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="SEZ_GetContainerDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="JobId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</div>

                             
    </Content>
        </AjaxToolkit:AccordionPane> 

       </Panes>
  </AjaxToolkit:Accordion>


   </div>
 </ContentTemplate>
</asp:UpdatePanel>

</asp:content>


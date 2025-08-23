<%@ Page Title="SEZ Information" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SEZInfo.aspx.cs"
    Inherits="SEZ_SEZInfo" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
            width: 600px;
            height: 200px;
        }
    </style>


    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" enablepartialrendering="true" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <%-- <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>--%>
    <%--<asp:Label ID="lblError" runat="server" EnableViewState="false">--%>

    <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="JobRequired" CssClass="errorMsg" EnableViewState="false" />

    <asp:Panel ID="panSez" runat="server">

     <%--   <fieldset>
            <legend>SEZ Type</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        <center>
                            <asp:RadioButtonList ID="rdbSEZtype" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="rdbSEZtype_SelectedIndexChanged">
                                <asp:ListItem Text="Inword" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Outword" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>

                        </center>
                    </td>
                </tr>
            </table>
        </fieldset>--%>

       <%-- <fieldset>

            <legend>
                <asp:Label ID="lblTitle" runat="server"></asp:Label></legend>--%>

            <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
                RenderMode="Inline">
                <ContentTemplate>
                    <div align="center">
                        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
                    </div>
                    <div class="clear"></div>
                    <fieldset>
                        <legend><asp:Label ID="lblTitle" runat="server" Text="SEZ Job Detail"></asp:Label></legend>
                        <div class="clear">
                            <asp:Panel ID="pnlFilter" runat="server">
                                <div class="fleft">
                                    <uc1:datafilter id="DataFilter1" runat="server" />
                                </div>
                                <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                    </asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="clear">
                        </div>
                        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                            OnRowCommand="gvJobDetail_RowCommand" AllowPaging="True" AllowSorting="True" Width="100%"
                            PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="PendingSEZDataSource"
                            OnRowDataBound="gvJobDetail_RowDataBound" OnPreRender="gvJobDetail_PreRender" >
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SEZ Job No" SortExpression="JobRefNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                            CommandArgument='<%#Eval("lid")%>' />

                                        <asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("JobRefNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false"  />

                                <asp:TemplateField HeaderText="SEZ Type" SortExpression="JobRefNo">
                                    <ItemTemplate>
                                       <%-- <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                            CommandArgument='<%#Eval("lid") %>' />--%>
                                        <asp:Label ID="lblSEZType" runat="server" Text='<%#Eval("SEZType") %>'  CommandArgument='<%#Eval("JobRefNo") %>'> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ReqTypeName" HeaderText="Request Type" SortExpression="ReqTypeName" />
                               <%-- <asp:BoundField DataField="SEZType" HeaderText="SEZ Type" SortExpression="SEZType" />--%>
                                <asp:BoundField DataField="RequestId" HeaderText="Request Id" SortExpression="RequestId" />
                                <asp:BoundField DataField="BENo" HeaderText="BE No" SortExpression="BENo" />
                                <asp:BoundField DataField="ClientName" HeaderText="Customer Name" SortExpression="ClientName" />
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                                <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode"/>
                                <asp:BoundField DataField="FileSentToBilling" HeaderText="File Sent To Billing" SortExpression="FileSentToBilling" DataFormatString = "{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="AssesableValue" HeaderText="Assesable Value" SortExpression="AssesableValue" />
                                <asp:BoundField DataField="CIFValue" HeaderText="CIF Value" SortExpression="CIFValue" />

                              <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkStatus" CommandName="QueryPopup" Text='<%#Eval("JStatus") %>' CommandArgument='<%#Eval("lid") + ";" + Eval("JobRefNo") %>' runat="server" TabIndex="6"></asp:LinkButton>
                                </ItemTemplate>
                             </asp:TemplateField>

                               <%-- <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}"--%>
                                   
                              <%--  <asp:BoundField DataField="ATADate" HeaderText="ATA" DataFormatString="{0:dd/MM/yyyy}"
                                    SortExpression="ATADate" />
                                <asp:BoundField DataField="BETypeName" HeaderText="BOE" SortExpression="BETypeName" />
                                <asp:BoundField DataField="JobCreatedBy" HeaderText="Job User" SortExpression="JobCreatedBy" />
                                <asp:BoundField DataField="JobDate" HeaderText="Job Date" SortExpression="JobDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CheckListStatus" HeaderText="Status" SortExpression="CheckListStatus" />
                             
                                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                                <asp:TemplateField HeaderText="Hold">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkHold" CommandName="HoldPopup" CommandArgument='<%#Eval("JobId")+";" + Eval("JobRefNo")%>' Text="Hold" runat="server"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </fieldset>
                    <!--Status Detail Start -->
                    <div id="divHold">

                         <cc1:ModalPopupExtender ID="ModalPopupStatus" runat="server" PopupControlID="PanelSEZStatus" TargetControlID="lnkDummy" CacheDynamicResults="false"
                                CancelControlID="imgStatusClose" BackgroundCssClass="modalBackground">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="PanelSEZStatus" runat="server" CssClass="modalPopup" align="center" Style="display: none" BackColor="#EBF4FA" >
                                                        <center>  <asp:Label ID="lblPopMessageQuery" runat="server" Visible="false"></asp:Label></center>                                                
                             <table width="100%" style="background-color: #c7a879">
                                <tr>
                                    <td style="width: 33%"></td>
                                    <td style="width: 34%">
                                        <div>
                                            <center>
                                                <b>Add SEZ Status </b>
                                            </center>
                                        </div>

                                    </td>
                                    <td style="width: 33%">
                                        <div class="fright">
                                            <asp:ImageButton ID="imgStatusClose" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="m"></div>
                            <div>
                                <asp:Label ID="lblPopMessageHold" runat="server" EnableViewState="false"></asp:Label>
                                <asp:HiddenField ID="hdnJobId" runat="server" />   
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td><b>Job Ref No</b></td>
                                    <td>
                                        <asp:Label ID="lblJobRefNo" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b> Select Status </b></td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="200px">
                                            <asp:ListItem Text="--- SELECT ---" Value="0"></asp:ListItem>
                                            <%--<asp:ListItem Text="DO Pending" Value="1"></asp:ListItem>--%>
                                            <asp:ListItem Text="Noting Pending" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Custom Formalities Pending" Value="2"></asp:ListItem>                                           
                                            <asp:ListItem Text="Delivery Pending" Value="3"></asp:ListItem>      
                                            <asp:ListItem Text="File Sent To Billing" Value="4"></asp:ListItem> 
                                            <asp:ListItem Text="Checklist under preparation" Value="5"></asp:ListItem>      
                                            <asp:ListItem Text="Checklist approval pending" Value="6"></asp:ListItem>      
                                            <asp:ListItem Text="Under noting" Value="7"></asp:ListItem>      
                                            <asp:ListItem Text="Under Query" Value="8"></asp:ListItem>      
                                            <asp:ListItem Text="Under assessments" Value="9"></asp:ListItem>      
                                            <asp:ListItem Text="Under DC" Value="10"></asp:ListItem>      
                                            <asp:ListItem Text="Under Examination" Value="11"></asp:ListItem>      
                                            <asp:ListItem Text="Cargo Part inward/Delivery" Value="12"></asp:ListItem>      
                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%"><b> Status Remark </b>
                                    <asp:RequiredFieldValidator ID="RFVReason" runat="server" ControlToValidate="txtSEZStatus"
                                    Text="Required" ErrorMessage="Please Enter The Remark" ValidationGroup="JobRequired" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtSEZStatus" runat="server" TextMode="MultiLine" MaxLength="200" Rows="5"></asp:TextBox>
                                    </td>
                                    
                                </tr>  
                                 <tr>
                                    <td>
                                        <asp:Button ID="BtnSaveStatus" Text="Add Status" runat="server" ValidationGroup="JobRequired" OnClick="BtnSaveStatus_Click" /> <%--OnClick="BtnSaveQuery_Click"--%>
                                           
                                    </td>
                                    <td >
                                       
                                           
                                    </td>                                    
                                 </tr>                            
                                </table>                           
                        </asp:Panel>

                    </div>
                    <div>
                        <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                    </div>

                    <div>
                        <asp:SqlDataSource ID="PendingSEZDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetSEZJobDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>  
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

      <%--  </fieldset>--%>

        <%--<fieldset>
            <legend>
                <asp:Label ID="lblTitle" runat="server"></asp:Label></legend>

            <asp:GridView ID="gridSEZDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false" CssClass="grid" OnRowDataBound="gridSEZDetail_OnRowDataBound"
                 OnRowCommand="gridSEZDetail_RowCommand">
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." />
                    <asp:TemplateField HeaderText="BS Job No">
                        <ItemTemplate>
                            <asp:TextBox ID="txtJobNo" runat="server" placeholder="SZ00001/DSIB/17-18"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mode">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlMode" runat="server" Width="100px">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Land" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtClientName" runat="server" Width="200px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Person">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPerson" runat="server" Width="200px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSupplierName" runat="server" Width="200px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Invoice No">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Invoice Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInvoiceDate" runat="server"></asp:TextBox>
                            

                            <asp:CalendarExtender ID="calInvoice" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtInvoiceDate" PopupPosition="BottomRight"
                                TargetControlID="txtInvoiceDate">
                            </asp:CalendarExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description & Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Width="300px" TextMode="Multiline" Height="30px" Class="css_input"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Invoice Value">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInvoiceValue" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Currency">
                        <ItemTemplate>
                            
                            <asp:DropDownList ID="ddlCurrency" runat="server" Width="100px">
                                
                                <asp:ListItem Text="USD" Value="1"></asp:ListItem>

                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Term">
                        <ItemTemplate>
                            
                            <asp:DropDownList ID="ddlTerm" runat="server" Width="100px">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="CIF" Value="1"></asp:ListItem>

                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ex-Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtExRate" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Assesable Value (INR)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAssesable" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    

                    <asp:TemplateField HeaderText="Inward BE No.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInwardBENo" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inward Job No.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInwardJobNo" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inward BE Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInwardBEDate" runat="server"></asp:TextBox>

                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtBEDate" PopupPosition="BottomRight"
                                TargetControlID="txtInwardBEDate">
                            </asp:CalendarExtender>
                        </ItemTemplate>
                    </asp:TemplateField>

                    

                    <asp:TemplateField HeaderText="BE No.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBENo" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BE Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBEDate" runat="server"></asp:TextBox>
                            

                            <asp:CalendarExtender ID="CalBEDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtBEDate" PopupPosition="BottomRight"
                                TargetControlID="txtBEDate">
                            </asp:CalendarExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request ID">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRequestID" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Duty Amount (INR)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDutyAmnt" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inward Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtInwardDate" runat="server"></asp:TextBox>

                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtBEDate" PopupPosition="BottomRight"
                                TargetControlID="txtInwardDate">
                            </asp:CalendarExtender>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No of Packages">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPackages" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gross Weight (Kgs)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGrossWt" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No. of Vehicles">
                        <ItemTemplate>
                            <asp:TextBox ID="txtVehicles" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Services Provide">
                        <ItemTemplate>
                            
                            <asp:DropDownList ID="ddlServices" runat="server" Width="100px">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Import" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Re-Export" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Outward Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtOutwardDate" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    

                    <asp:TemplateField HeaderText="Days Store">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDaysStore" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    

                    <asp:TemplateField HeaderText="PCD From Dahej">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPCDDahej" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PCD Sent Client">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPCDClient" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Sent To Billing">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFileSentBilling" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Billing Status">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBillingStatus" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remark">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Width="300px" TextMode="Multiline" Height="30px" Class="css_input"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                          
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="R.N.Logistics">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLogistics" runat="server" Width="200px"></asp:TextBox>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <asp:Button ID="btnAdd" runat="server" Text="Add New Row" OnClick="btnAdd_OnClick" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </fieldset>--%>
    </asp:Panel>

    <%--<asp:Button ID="modelPopup" ValidationGroup="Required" runat="server" />
    <asp:modalpopupextender id="ModalPopupExtender1" runat="server" targetcontrolid="modelPopup"
        popupcontrolid="panDocument">
   </asp:modalpopupextender>
    <asp:Panel ID="panDocument" runat="server">
        <div style="border: 1px solid #C0C0C0; width: auto; height: 500px; overflow: scroll; position: static;">
            <table>
                <tr class="heading">
                    <td>
                        <b>Documents Upload</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" 
                                            ToolTip="Close" />
                     
                    </td>
                </tr>

                <tr>
                    <td>
                          <fieldset>
                        <legend>Upload Document</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="50%" bgcolor="white">
                            <tr>
                                <td>
                                    Document Type <asp:DropDownList ID="ddDocument" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                 <asp:FileUpload ID="fuDocument" runat="server" />   
                                </td>
                            </tr>    
                            <tr>
                                <td>
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload"  />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </fieldset>
                    <div class="m clear">
                    </div>
                    <fieldset>
                        <legend>Download Document</legend>
                        <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="DocId"
                            DataSourceID="DocumentSqlDataSource" 
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DocumentName" HeaderText="Document" />
                                <asp:BoundField DataField="sName" HeaderText="Uploaded By" />
                                <asp:TemplateField HeaderText="Download">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                            CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetUploadedDocument" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:GridView ID="gvShipmentdetails" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                            AllowPaging="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobrefNo" HeaderText="Job No" SortExpression="JobrefNo" />
                                <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>--%>
</asp:Content>


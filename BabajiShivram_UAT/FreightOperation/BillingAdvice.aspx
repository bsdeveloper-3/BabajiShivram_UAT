<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillingAdvice.aspx.cs"
    Inherits="FreightOperation_BillingAdvice" Title="Freight Billing Advice" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnTypeId" runat="server" Value="0" />
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Billing Advice Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnSaveAdvice" runat="server" Text="Save" OnClick="btnSaveAdvice_Click" ValidationGroup="Required" />
                    <asp:Button ID="btnCancelAdvice" runat="server" OnClick="btnCancelAdvice_Click" CausesValidation="False"
                        Text="Cancel" TabIndex="20" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>Job No
                        </td>
                        <td>
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAgentField" runat="server" Text="Agent Invoice Received?"></asp:Label>

                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlAgentInvoice" RepeatDirection="Horizontal" runat="server" Enabled="true"
                                OnSelectedIndexChanged="rdlAgentInvoice_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="NO" Value="false" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="YES" Value="true"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Sent To Billing Dept
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlSentToBilling" RepeatDirection="Horizontal" runat="server"
                                OnSelectedIndexChanged="rdlSentToBilling_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="NO" Value="false" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="YES" Value="true"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>Remark
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtBillingRemark" TextMode="MultiLine" Width="90%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                
                <div id="dvAgentInvoice" runat="server">
                    <fieldset id="Agentfield" runat="server">
                        <legend>Agent Invoice Detail</legend>
                        <div>
                            <%--<asp:Button ID="btnSaveAgentInvoice" runat="server" Text="Save" OnClick="btnSaveAgentInvoice_Click" ValidationGroup="RequiredInvoice" />--%>
                            <%--<asp:Button ID="btnCancelInvoice" runat="server" OnClick="btnCancelInvoice_Click" CausesValidation="False"
                                Text="Cancel" TabIndex="20" />--%>
                        </div>
                        <table id="tblAgentDetails" runat="server" visible="true" border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                            <tr>
                                <td>Agent Name
                            <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="ddAgent" Display="Dynamic" Enabled="true"
                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Agent Name." ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddAgent" runat="server"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtAgentName" runat="server"></asp:TextBox>--%>
                                </td>
                                <td>Invoice Received Date
                            <AjaxToolkit:CalendarExtender ID="CalRcvdDate" runat="server" Enabled="True" EnableViewState="False" 
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRcvdDate" PopupPosition="BottomRight"
                                TargetControlID="txtReceivedDate">
                            </AjaxToolkit:CalendarExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgRcvdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <%--<AjaxToolkit:MaskedEditExtender ID="MskExtRcvdDate" TargetControlID="txtReceivedDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="MskValRcvdDate" ControlExtender="MskExtRcvdDate" ControlToValidate="txtReceivedDate" IsValidEmpty="false"
                                        InvalidValueMessage="Bill Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                        EmptyValueMessage="Please Enter Invoice Rcvd Date" EmptyValueBlurredText="Required" MinimumValueBlurredText="Invalid Date" MinimumValueMessage="Invalid Received Date"
                                        MaximumValueBlurredMessage="Invalid Date" MaximumValueMessage="Invalid Received Date" MinimumValue="01/01/2016" MaximumValue="01/01/2020"
                                        runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Agent/Vendor Invoice No
                            <asp:RequiredFieldValidator ID="RFVInvNo" runat="server" ControlToValidate="txtInvoiceNo" Display="Dynamic" Enabled="true"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice No." ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                                </td>
                                <td>Invoice Date
                            <AjaxToolkit:CalendarExtender ID="calInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvoiceDate" PopupPosition="BottomRight"
                                TargetControlID="txtInvoiceDate">
                            </AjaxToolkit:CalendarExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" Width="100px" Enabled="true"></asp:TextBox>
                                    <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <%--<AjaxToolkit:MaskedEditExtender ID="MskExtInvoiceDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="MskValInvoiceDate" ControlExtender="MskExtInvoiceDate" ControlToValidate="txtInvoiceDate" IsValidEmpty="false"
                                        InvalidValueMessage="Invoice Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                        EmptyValueMessage="Please Enter Invoice Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date"
                                        MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2020"
                                        runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Invoice Amount
                            <asp:RequiredFieldValidator ID="RFVInvoiceAmount" runat="server" ControlToValidate="txtInvoiceAmount" Display="Dynamic" InitialValue="" Enabled="true"
                                SetFocusOnError="true" Text="*" ErrorMessage="Required" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompValAmount" runat="server" ControlToValidate="txtInvoiceAmount"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Amount"
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceAmount" runat="server" MaxLength="15"></asp:TextBox>
                                </td>
                                <td>Invoice Currency
                                    <asp:RequiredFieldValidator ID="RFVCurrency" runat="server" ControlToValidate="ddCurrency" Display="Dynamic" InitialValue="0" Enabled="true"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Select Invoice Currency." ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency" AppendDataBoundItems="true" Enabled="true"
                                        DataValueField="lId" DataTextField="Currency">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>Agent Invoice
                                <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuAgentInvoice" Display="Dynamic" ErrorMessage="Attach Agent Invoice Copy For Upload." SetFocusOnError="true" Text="*" ValidationGroup="Required"
                                    Enabled="true"
                                    ></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:FileUpload ID="fuAgentInvoice" runat="server" ViewStateMode="Enabled"  Enabled="true"/>
                            </td>
                            </tr>
                            <tr>
                                <td>Remark
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtInvoiceRemark" TextMode="MultiLine" Width="90%" runat="server" Enabled="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:GridView ID="GridViewInvoiceDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" DataKeyNames="lId" DataSourceID="DataSourceInvoiceDetail" CellPadding="4" 
                                OnRowCommand="GridViewInvoiceDetail_RowCommand" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Agent" DataField="AgentInvoiceName" />
                                    <asp:BoundField HeaderText="Invoice No" DataField="AgentInvoiceNo" />
                                    <asp:BoundField HeaderText="Invoice Date" DataField="AgentInvoiceDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Invoice Receive Date" DataField="InvoiceReceivedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Invoice Amount" DataField="AgentInvoiceAmount" />
                                    <asp:BoundField HeaderText="Currency" DataField="Currency" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("DocName") %>' CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Agent Created By" DataField="AgentInvoiceCreatedBy" />
                                    <asp:BoundField HeaderText="Agent Created Date" DataField="AgentInvoicedtDate" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                
                <!--Document for BIlling Advice Start-->
                <div id="divDocument" class="clear" runat="server">
                    <fieldset id="DocumentType" runat="server">
                        <legend>Document Type</legend>
                        <div class="m clear">
                           <%-- &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" CausesValidation="false" />--%>
                            &nbsp;&nbsp;&nbsp;&nbsp; 
                            <%--<asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" ValidationGroup="Required" />--%>
                        </div>
                        <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                            <asp:HiddenField ID="hdnJobId" runat="server" />
                            <%--<asp:Label ID="lblMsg" runat="server" ForeColor="Red" style="align-content:center" >All Documents are Mandatory</asp:Label>--%>
                            <asp:HiddenField ID="hdnBranchId" runat="server" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <!--Document for BIlling Advice Start-->
                            
                                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptDocument" runat="server" DataSourceID="FrDocTypeSqlDataSource"
                                                OnItemDataBound="rpDocument_ItemDataBound">
                                                <%----%>
                                                <HeaderTemplate>
                                                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr bgcolor="#FF781E">
                                                            <th>Sl
                                                            </th>
                                                            <th>Name
                                                            </th>
                                                            <%--<th>Type
                                            </th>--%>
                                                            <th>Browse
                                                            </th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Container.ItemIndex +1%>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                                                runat="server"/>&nbsp;
                                                            <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                                                runat="server"></asp:HiddenField>
                                                            <asp:Label id="lblCheck" runat="server" visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="fuDocument"
                                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                                            <asp:FileUpload ID="fuDocument" runat="server" Enabled="true" ViewStateMode="Enabled"/>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>

                        </div>
                        <!--Document for BIlling Advice- END -->
                    </fieldset>

                    <fieldset>
                                <legend>Download</legend>
                                <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="99%"
                                    DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                                     CellPadding="4" PagerStyle-CssClass="pgr" OnRowCommand="gvFreightDocument_RowCommand"
                                    AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom">
                                    <%--OnRowCommand="gvFreightDocument_RowCommand"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                                        <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                        <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                                    CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                </div>
                <div>
                    <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                </div>
                <!--Document for BIlling Advice- END -->

                <div>
                    <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                
                <div>
                    <asp:SqlDataSource ID="FrDocTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="Get_FRDocumentType" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <%--<asp:ControlParameter ControlID="hdnTypeId" Name="TYPE" PropertyName="Value" />--%>
                            <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                            <asp:SessionParameter Name="JobType" SessionField="JobType" />
                            <asp:SessionParameter Name="JobMode" SessionField="JobMode" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceInvoiceDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FOP_GetAgentInvoiceDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </fieldset>
            <!--Popup for Proforma Invoice - Start -->
            <div id="divProformaInvoice">
                <AjaxToolkit:ModalPopupExtender ID="ModalPopupProforma" runat="server" CacheDynamicResults="false"
                    DropShadow="True" PopupControlID="pnlProforma" TargetControlID="lnkDummyProforma">
                </AjaxToolkit:ModalPopupExtender>

                <asp:Panel ID="pnlProforma" runat="server" CssClass="ModalPopupPanel">

                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID='lblProformaMeessage' runat="server" align="center" Font-Bold="true"></asp:Label>
                            Please Provide Final Invoice against Proforma to Accounts Department   
                        </div>
                        <div class="fright">
                            &nbsp;<asp:Button ID="Button1" runat="server" OnClick="btnCancelBJVdetails_Click" Text="Close" CausesValidation="false" />
                        </div>
                    </div>

                    <div id="Div41" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:GridView ID="gvProformaDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Type" SortExpression="ExpenseTypeName" />
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Proforma Invoice No" SortExpression="InvoiceNo" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                                <asp:BoundField DataField="InvoiceAmount" HeaderText="Total Value" SortExpression="InvoiceAmount" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <%--<asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:dd/MM/yyyy}" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyProforma" runat="server" Text="" Enabled="false"></asp:LinkButton>
            </div>
            <!--Popup for Proforma Invoice - END -->
             <!--Popup for BJV details - Start -->
            <div id="divBJVDetails">
                <AjaxToolkit:ModalPopupExtender ID="mpeBJVDetails" runat="server" CacheDynamicResults="false"
                    DropShadow="True" PopupControlID="pnlBJVDetails" TargetControlID="lnkDummy2">
                </AjaxToolkit:ModalPopupExtender>

                <asp:Panel ID="pnlBJVDetails" runat="server" CssClass="ModalPopupPanel">

                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID='lbldiv' runat="server" align="center" Font-Bold="true"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   
                        </div>
                        <div class="fright">
                            &nbsp;<asp:Button ID="btnCancelBJVdetails" runat="server" OnClick="btnCancelBJVdetails_Click" Text="Close" CausesValidation="false" />
                        </div>
                    </div>

                    <div id="Div2" runat="server" style="max-height: 550px; overflow: auto;">
                        <div>
                            <asp:Repeater ID="rptBJVDetails" runat="server" OnItemDataBound="rptBJVDetails_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table" cellpadding="0" cellspacing="0" width="100%" valign="top">

                                        <tr bgcolor="#FF781E">
                                            <th>SI</th>
                                            <th>VCHDATE</th>
                                            <th>VCHNO</th>
                                            <th>CONTRANAME</th>
                                            <th>CHQNO</th>
                                            <th>CHQDATE</th>
                                            <th>DEBITAMT</th>
                                            <%--<th>CREDITAMT</th><th>AMOUNT</th>--%><th>NARRATION</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="10">
                                            <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><%#Container.ItemIndex +1%></td>
                                        <td>
                                            <asp:Label ID="lblVCHDATE" runat="server" Text='<%#Eval("VCHDATE")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblVCHNO" runat="server" Text='<%#Eval("VCHNO")%>'></asp:Label></td>
                                        <td width="200px" style="white-space: normal">
                                            <asp:Label ID="lblCONTRANAME" runat="server" Text='<%#Eval("CONTRANAME")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblCHQNO" runat="server" Text='<%#Eval("CHQNO")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblCHQDATE" runat="server" Text='<%#Eval("CHQDATE")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblDEBITAMT" runat="server" Text='<%#Eval("DEBITAMT")%>'></asp:Label></td>
                                        <%--<td><asp:Label ID="lblCREDITAMT" runat="server" Text='<%#Eval("CREDITAMT")%>'></asp:Label></td>
			                                <td><asp:Label ID="lblAMOUNT" runat="server" Text='<%#Eval("AMOUNT")%>'></asp:Label></td>--%>
                                        <td width="200px" style="white-space: normal">
                                            <asp:Label ID="lblNARRATION" runat="server" Text='<%#Eval("NARRATION")%>'></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="6" align="right"><b>Total</b></td>
                                        <td>
                                            <asp:Label ID="lbltotDebitamt" runat="server"></asp:Label></td>
                                        <%--<td> <asp:Label ID="lbltotCREDITAMT" runat="server"></asp:Label></td>
                                            <td> <asp:Label ID="lbltotAMOUNT" runat="server"></asp:Label></td>--%>
                                        <td></td>
                                    </tr>

                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy2" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Popup for BJV details - END -->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

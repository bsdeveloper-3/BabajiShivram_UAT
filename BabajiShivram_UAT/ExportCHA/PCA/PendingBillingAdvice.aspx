<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingBillingAdvice.aspx.cs" Inherits="PCA_PendingBillingAdvice"
    MasterPageFile="~/MasterPage.master" Title="Pending Billing Advice" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <%--<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />--%>
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
    <script type="text/javascript">
        function GridSelectAllColumn(spanChk) {
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++) {
                if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                    elm[i].click();
            }
        }

    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <progresstemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </progresstemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <contenttemplate>
            <div align="center">
                <asp:ValidationSummary ID="vsLRCopy" runat="server" ValidationGroup="vgLRCopy" ShowSummary="true" ShowMessageBox="false" />
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Pending Billing Advice</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 40px;">
                            <asp:Button ID="btnPrintExpense" runat="server" Text="Print BJV" OnClick="btnPrintExpense_Click" ToolTip="Print BJV" />
                            &nbsp;
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Style="white-space: normal"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="1650px"
                    PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender" DataSourceID="PCDSqlDataSource"
                    OnRowCommand="gvJobDetail_RowCommand" OnRowDataBound="gvJobDetail_RowDataBound">
                    <Columns>
                        <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False" />
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All To Print BJV" runat="server" onclick="GridSelectAllColumn(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkPrint" runat="server" ToolTip="Check To Print BJV"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LR Pending">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlLrPending" runat="server" Width="80px">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hold">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/UnlockImg.png"
                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("Amount") + ";" + Eval("JobRefNo")%>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Job."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:ImageButton ID="imgbtnUnholdJob" runat="server" ImageUrl="~/Images/LockImg.png"
                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("Amount") + ";" + Eval("JobRefNo")%>' CommandName="Unhold" Width="18px" Height="18px" ToolTip="Unhold Job."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billing Instruction">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkInstruction" runat="server" Text="Billing Instruction" CommandName="InstructionPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Instr_status" HeaderText="Inst Status"/>
                        <asp:BoundField DataField="Doc_status" HeaderText="Doc Status"/>
                         <asp:TemplateField HeaderText="Rejection Type" SortExpression="ReasonforPendency">
                            <ItemTemplate>
                                <asp:Label ID="lblReasonPendency" runat="server" Text='<%# Eval("ReasonforPendency") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hold Reason" ItemStyle-Width="8%" SortExpression="HoldRemark">
                            <ItemTemplate>
                                <asp:Label ID="labHoldJobRemark" runat="server" Text='<%# Eval("HoldRemark") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="BJV Details" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnBJVDetails" runat="server" Text="Show" CommandName="showBJV" CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                        <%--<asp:TemplateField HeaderText="Instructions" SortExpression="Instructions" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="labInstructions" runat="server" Text='<%# Eval("Instructions") %>'></asp:Label>
                                <asp:Button ID="btnInstructions" runat="server" Text="" OnCommand="txtInstructions_Changed" CommandArgument='<%# Bind("JobID") %>' Style="display: none" />
                                <asp:TextBox ID="txtInstructions" runat="server" Text='<%# Eval("Instructions") %>' Width="175px"
                                    Style="display: none" TextMode="MultiLine"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="PCA To Customer" SortExpression="PCDRequired">
                <ItemTemplate>
                    <%# (Boolean.Parse(Eval("PCDRequired").ToString())) ? "Yes" : "No"%>
                </ItemTemplate>
            </asp:TemplateField>--%>
                        <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out Of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
                        <asp:TemplateField HeaderText="Billing Documents">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDocument" runat="server" Text="Advice Documents" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aging I" SortExpression="Aging">
                            <ItemTemplate>
                                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Today – Documents sent to Back Office"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BackOfficeDate" HeaderText="Date on Job Forwarded For Billing Scrutiny" Visible="false" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BackOfficeDate" />
                        <asp:BoundField HeaderText="Billing Scrutiny Rejection Date" DataField="RejectedDate" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Rejection Remark" DataField="RejectRemark" Visible="false" />
                        <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy" Visible="false" />
                        <asp:BoundField HeaderText="Remark" ItemStyle-Width="8%" />
                        <asp:TemplateField HeaderText="Sent To Scrutiny" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkScrutiny" runat="server" Text="Send" CommandName="SendForScrutiny" CommandArgument='<%#Eval("JobId") %>'
                                    OnClientClick="return confirm('Are you sure wants to Move the Job To Scrutiny ?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ReasonforPendency" HeaderText="Rejection Type" Visible="false" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <!--Document for BIlling Advice Start-->
            <div id="divDocument">
                <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel">
                    <div class="header">
                        <div class="fleft">
                            &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" CausesValidation="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;    
                            <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" ValidationGroup="Required" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m"></div>
                    <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:HiddenField ID="hdnJobId" runat="server" />
                        <asp:HiddenField ID="hdnBranchId" runat="server" />
                        <asp:HiddenField ID="hdnUploadPath" runat="server" />
                        <asp:Label ID="lblDocMsg" runat="server"></asp:Label>
                        <!--Document for BIlling Advice Start-->
                        <div>
                            <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rpDocument_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr bgcolor="#FF781E">
                                            <th>Sl
                                            </th>
                                            <th>Name
                                            </th>
                                            <th>Type
                                            </th>
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
                                                runat="server" />&nbsp;
                                        <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                            runat="server"></asp:HiddenField>
                                        </td>
                                        <td>
                                            <asp:CustomValidator ID="CVCheckBoxList" runat="server" ClientValidationFunction="ValidateCheckBoxList"
                                                Enabled="false" ErrorMessage="Please Select Type" ValidationGroup="Required" Display="Dynamic"></asp:CustomValidator>
                                            <asp:CheckBoxList ID="chkDuplicate" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Original" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Copy" Value="2"></asp:ListItem>
                                            </asp:CheckBoxList>

                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="fuDocument"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                            <asp:FileUpload ID="fuDocument" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <fieldset>
                            <legend>Download</legend>
                            <asp:GridView ID="gvPCDDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20"  OnRowDataBound="gvPCDDocument_RowDataBound"
                                OnRowCommand="gvPCDDocument_RowCommand">  <%--DataSourceID="PCDDocumentSqlDataSource"--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="All Document" />
                                    <%--<asp:BoundField DataField="PCDToCustomer" HeaderText="PCD To Customer" />
                                    <asp:BoundField DataField="PCDToScrutiny" HeaderText="Scrutiny" Visible="false" />--%>
                                    <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </fieldset>
                        </div>
                    </div>
                    <!--Document for BIlling Advice- END -->
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Document for BIlling Advice- END -->

            <!--Popup for BJV details - Start -->
            <div id="divBJVDetails">
                <cc1:ModalPopupExtender ID="mpeBJVDetails" runat="server" CacheDynamicResults="false"
                    DropShadow="True" PopupControlID="pnlBJVDetails" TargetControlID="lnkDummy2">
                </cc1:ModalPopupExtender>

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

            <%--  START : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <div id="divHoldExpense">
                <cc1:ModalPopupExtender ID="mpeHoldExpense" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlHoldExpense" CancelControlID="imgbtnHoldExp" TargetControlID="hdnHoldExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlHoldExpense" runat="server" CssClass="ModalPopupPanel" Width="600px" Height="350px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblHoldPopupName" runat="server"></asp:Label>
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnHoldExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                        <asp:HiddenField ID="hdnJobRefNo" runat="server" />
                        <asp:HiddenField ID="hdnJobId_hold" runat="server" />
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 300px; overflow: auto; padding: 5px">
                        <center>
                            <asp:Label ID="lblError_HoldExp" runat="server" EnableViewState="false"></asp:Label>
                            <div style="width:100%">
                                <asp:FormView ID="fvHoldJobDetail" runat="server" DataSourceID="DataSourceHoldJob" DataKeyNames="JobId">
                                    <ItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>BS Job Number</td>
                                                <td>
                                                    <asp:Label ID="lblBSJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                                </td>
                                                <td>Customer</td>
                                                <td>
                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Consignee</td>
                                                <td>
                                                    <asp:Label ID="lblConsignee" runat="server" Text='<%#Eval("Consignee") %>'></asp:Label>
                                                </td>
                                                <td>Amount</td>
                                                <td>
                                                    <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Out of Charge Date</td>
                                                <td colspan="3">
                                                    <asp:Label ID="lblOutOfChargeDt" runat="server" Text='<%#Eval("OutOfChargeDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </td>
                                           
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReasonHold" runat="server" Text="Rejection Type" ForeColor="Black" Font-Size="9"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlReasonHold" runat="server" DataSourceID="DsReasonforpendency" ForeColor="Black" Font-Size="9" DataTextField="ReasonforPendency" DataValueField="Id">
                                                        <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>

                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlReasonHold"
                                                        ErrorMessage="Please Select Rejection Type" InitialValue="0" ValidationGroup="vgAddHoldExpense"></asp:RequiredFieldValidator>

                                                     <asp:SqlDataSource ID="DsReasonforpendency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                            SelectCommand="GetpendencyReason" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                </td>
                                          
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:FormView>
                            </div>
                            <br />
                            &nbsp;
                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" SetFocusOnError="true" Display="Dynamic"
                                ForeColor="Red" ErrorMessage="* Enter Reason" ValidationGroup="vgAddHoldExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                            <div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Reason                                      
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnHoldJob" runat="server" ValidationGroup="vgAddHoldExpense" Text=""
                                                OnClientClick="return confirm('Are you sure to hold this job?');" OnClick="btnHoldJob_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="hdnHoldExp" runat="server"></asp:LinkButton>
            </div>

            <%--  END   : MODAL POP-UP FOR HOLD EXPENSE  --%>
            <!--Popup for Proforma Invoice - Start -->
            <div id="divProformaInvoice">
                <cc1:ModalPopupExtender ID="ModalPopupProforma" runat="server" CacheDynamicResults="false"
                    DropShadow="True" PopupControlID="pnlProforma" TargetControlID="lnkDummyProforma">
                </cc1:ModalPopupExtender>

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
                            Width="100%" >
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Type" SortExpression="ExpenseTypeName"/>
                            <asp:BoundField DataField="InvoiceNo" HeaderText="Proforma Invoice No" SortExpression="InvoiceNo"/>
                            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                            <asp:BoundField DataField="InvoiceAmount" HeaderText="Total Value" SortExpression="InvoiceAmount" />
                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:dd/MM/yyyy}"/>
                        </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyProforma" runat="server" Text="" Enabled="false"></asp:LinkButton>
            </div>
            <!--Popup for Proforma Invoice - END -->

            <!--Document for BIlling Instruction Start-->
            <div id="divInstruction">
                <cc1:ModalPopupExtender ID="ModalPopupInstruction" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="PanelInstruction" TargetControlID="lnkDummyInstruction">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="PanelInstruction" runat="server" CssClass="ModalPopupPanel" Width="70%" Height="70%">
                    
                        <div class="header">
                            <div class="fleft">
                                &nbsp;<asp:Button ID="btnCanInstruction" runat="server" OnClick="btnCanInstruction_Click" Text="Close" CausesValidation="false" />
                                &nbsp;&nbsp;&nbsp;&nbsp;    
                                <asp:Button ID="btnSaveInstruction" Text="Save Instruction" runat="server" OnClick="btnSaveInstruction_Click" CausesValidation="true" ValidationGroup="RequiredField" />
                            </div>
                            <%--<div class="right">
                                <asp:ImageButton ID="ImgInstructionClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="ImgInstructionClose_Click" ToolTip="Close" />
                            </div>--%>
                        </div>
                    <div id="dvBillInstruction" runat="server">
                        <div class="m"></div>
                        <div id="Div4" runat="server" style="max-height: 550px; overflow: auto;">
                            <!--Document for BIlling Instruction Start-->
                            <asp:Label ID="lblBillMsg" runat="server"></asp:Label>
                            <div>
                                <table class="table" border="1" width="100%">
                                    <tr>
                                        <td>Job Ref No</td>
                                        <td colspan="3">
                                            <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Shipment Type
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblShipmentType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Shipment Category
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblShipmentCate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>Allied Agency Service Apply
                                            <asp:RequiredFieldValidator ID="RFVAgencyService" runat="server" ControlToValidate="rdbAgencyServiceApply" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Select Allied Agency Service Apply" ValidationGroup="RequiredField"></asp:RequiredFieldValidator>
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButtonList ID="rdbAgencyServiceApply" runat="server" RepeatDirection="Horizontal" >
                                                <asp:ListItem value="1" Text="Yes"></asp:ListItem>
                                                <asp:ListItem value="2" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>--%>
                                    <tr style="border-top: hidden;">
                                        <td>Allied Service
                                        </td>
                                        <td style="border-right: hidden;">
                                            <asp:CheckBoxList ID="chkAgencyService" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="ADC" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="FSSAI" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="PQ" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="AQ" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Texttile Committee" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Sample Testing" Value="6"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td style="border-right: hidden;">Remark
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgencyServiceRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <%--<tr>
                                        <td>Delivery Related Apply
                                            <asp:RequiredFieldValidator ID="RFVDeliveryRelatedApply" runat="server" ControlToValidate="rdbDeliveryRelatedApply" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Select Delivery Related Apply" ValidationGroup="RequiredField"></asp:RequiredFieldValidator>
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButtonList ID="rdbDeliveryRelatedApply" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem value="1" Text="Yes"></asp:ListItem>
                                                <asp:ListItem value="2" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            
                                        </td>
                                    </tr>
                                    <tr style="border-top:hidden;">
                                        <td>
                                            Delivery Related
                                            <span id="spDeliveryRelated" runat="server" style="color:red" visible="false">*</span>
                                        </td>
                                        <td style="border-right:hidden;">
                                            <asp:DropDownList ID="ddlDeliveryRelated" runat="server">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="De-Stuffing" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Loading" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Thappi" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" colspan="2">
                                            <asp:LinkButton ID="lnkBillingDocument" runat="server" Text="Billing Document" OnClick="lnkBillingDocument_Click" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Value Added Services Apply
                                            <asp:RequiredFieldValidator ID="RFVVASApply" runat="server" ControlToValidate="rdblVASApply" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Select Value Added Service Apply" ValidationGroup="RequiredField"></asp:RequiredFieldValidator>
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButtonList ID="rdblVASApply" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem value="1" Text="Yes"></asp:ListItem>
                                                <asp:ListItem value="2" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr style="border-top:hidden;">
                                        <td>
                                            Value Added Services (VAS)
                                            <span id="spVas" runat="server" style="color:red" visible="false">*</span>
                                        </td>
                                        <td style="border-right:hidden;">
                                            <asp:CheckBoxList ID="chkVas" runat="server"  RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Labelling" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Repacking" Value="2"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td style="border-right:hidden;">Remark
                                            <span id="spvasRemark" runat="server" style="color:red" visible="false">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" ID="txtVASRemark" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>CE Certificate Apply
                                            <asp:RequiredFieldValidator ID="RFVCECertificateApply" runat="server" ControlToValidate="rdbCECertificate" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Select CE Certificate Apply" ValidationGroup="RequiredField"></asp:RequiredFieldValidator>
                                        </td>
                                        <td >
                                            <asp:RadioButtonList ID="rdbCECertificate" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem value="1" Text="Yes"></asp:ListItem>
                                                <asp:ListItem value="2" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="border-right:hidden;"> Remark
                                            <span id="spCECertificateRemark" runat="server" style="color:red" visible="false">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCERemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="border-top:hidden;">
                                        <%--<td>
                                            CE Certificate
                                            <span id="spCECertificate" runat="server" style="color:red" visible="false">*</span>
                                        </td>
                                        <td style="border-right:hidden;">
                                            <asp:DropDownList ID="ddlCertificate" runat="server">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                 <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            Transportation
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransportation" runat="server"></asp:Label>
                                        </td>
                                        <td>LR upload</td>
                                        <td><asp:Label ID="lblLRUpload" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Send Billing Without LR</td>
                                        <td>
                                            <asp:DropDownList ID="ddlSendBillingOpt" runat="server">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                 <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>Email Approval Copy</td>
                                        <td>
                                            <asp:FileUpload ID="fuEmailDocument" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Type Of  consignment
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlConsignee" runat="server">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                 <asp:ListItem Text="Machinery" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Raw Material" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Old & Used " Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>Other Service</td>
                                        <td>
                                            <asp:CheckBoxList ID="chkOtherService" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Licence" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Chartered" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="SVB" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="SVB Registration" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sampling" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="CE Certificate" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Labelling" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="Re-Packing" Value="8"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td style="border-right: hidden;">Remark
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherServiceRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Other Instruction
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>Attachment Copy</td>
                                        <td>
                                            <asp:FileUpload ID="FuInstructionCopy" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Other Instruction
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherRemark1" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>Attachment Copy</td>
                                        <td>
                                            <asp:FileUpload ID="FuInstructionCopy1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Other Instruction
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherRemark2" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>Attachment Copy</td>
                                        <td>
                                            <asp:FileUpload ID="FuInstructionCopy2" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Other Instruction
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherRemark3" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>Attachment Copy</td>
                                        <td>
                                            <asp:FileUpload ID="FuInstructionCopy3" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id="dvResult" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">
                        <br />
                        <br />
                        <asp:Label ID="lblRemark" runat="server" Style="text-align: center; text-decoration: underline;">Bill Instruction already exist</asp:Label>
                        <br />
                        <br />
                        <div align="center">
                            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" align="center" style="text-align: left;">
                                <tr>
                                    <td><b>Job Ref No</b></td>
                                    <td colspan="3">
                                        <asp:Label ID="lblRefNo" runat="server"></asp:Label></td>
                                </tr>
                                <%--<tr>
                                <td>Allied Agency Apply</td>
                                <td colspan="3"><asp:Label ID="lblAgencyApply" runat="server" Text='<%# Bind("AlliedAgencyApply") %>'></asp:Label> </td>
                            </tr>--%>
                                <tr>
                                    <td><b>Allied Service</b></td>
                                    <td>
                                        <asp:Label ID="lblAlliedAgencyService" runat="server" Text='<%# Bind("AlliedAgencyService") %>'></asp:Label>
                                    </td>
                                    <td><b>Allied Remark</b></td>
                                    <td>
                                        <asp:Label ID="lblAlliedAgencyRemark" runat="server" Text='<%# Bind("AlliedAgencyRemark") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Other Service</b></td>
                                    <td>
                                        <asp:Label ID="lblOtherService" runat="server"></asp:Label>
                                    </td>
                                    <td><b>Other Service remark</b></td>
                                    <td>
                                        <asp:Label ID="lblOtherServiceRemark" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Other Instruction</b></td>
                                    <td>
                                        <asp:Label ID="lblInstruction" runat="server"></asp:Label></td>
                                    <td><b>Instruction Copy</b></td>
                                    <td>
                                        <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td><b>Other Instruction</b></td>
                                    <td>
                                        <asp:Label ID="lblInstruction1" runat="server"></asp:Label></td>
                                    <td><b>Instruction Copy</b></td>
                                    <td>
                                        <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td><b>Other Instruction</b></td>
                                    <td>
                                        <asp:Label ID="lblInstruction2" runat="server"></asp:Label></td>
                                    <td><b>Instruction Copy</b></td>
                                    <td>
                                        <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td><b>Other Instruction</b></td>
                                    <td>
                                        <asp:Label ID="lblInstruction3" runat="server"></asp:Label></td>
                                    <td><b>Instruction Copy</b></td>
                                    <td>
                                        <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <!--Document for BIlling Instruction- END -->
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyInstruction" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Document for BIlling Instruction- END -->

            <div>
                <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure" >  <%--OnSelected="PCDDocumentSqlDataSource_Selected"--%>
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillingInstruction" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="Get_BillingInstructionDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                        <%--<asp:ControlParameter ControlID="hdnJobId_hold" Name="JobId" PropertyName="Value" />--%>
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetPendingPCABillingAdvice" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceHoldJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnJobId_hold" Name="JobId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

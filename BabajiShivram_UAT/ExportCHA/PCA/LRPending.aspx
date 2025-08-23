<%@ Page Title="LR Pending" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LRPending.aspx.cs"
    Inherits="PCA_LRPending" Culture="en-GB" EnableEventValidation="false" %>

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
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:ValidationSummary ID="vsLRCopy" runat="server" ValidationGroup="vgLRCopy" ShowSummary="true" ShowMessageBox="false" />
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>LR Pending</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 5px;">
                            <asp:Button ID="btnPrintExpense" runat="server" Text="Print BJV" OnClick="btnPrintExpense_Click" ToolTip="Print BJV" />
                            &nbsp;
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            &nbsp;
                            <asp:TextBox ID="txtAging" runat="server" Text="AGING I : Billing Advice – Job Received in LR Pending" Width="320px" Enabled="false" Style="border-radius: 4px; background-color: white; color: darkblue; font-weight: 600"></asp:TextBox>
                            <asp:TextBox ID="txtAging2" runat="server" Text="AGING II : Today – Dispatch Date" Width="200px" Enabled="false" Style="border-radius: 4px; background-color: white; color: darkblue; font-weight: 600"></asp:TextBox>
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
                        <asp:TemplateField HeaderText="LR Copy" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%--<asp:RequiredFieldValidator ID="rfvLRCopy" runat="server" ControlToValidate="fuLRCopy" SetFocusOnError="true"
                                    Display="Dynamic" ForeColor="Red" Text="*" ErrorMessage="Please upload LR Copy!" ValidationGroup="vgLRCopy"></asp:RequiredFieldValidator>--%>
                                <asp:FileUpload ID="fuLRCopy" runat="server" />
                                <%--<asp:ImageButton ID="imgbtnLRCopy" runat="server" ImageUrl="~/Images/UnlockImg.png" CommandName="LRCopy" Width="18px"
                                    CommandArgument='<%#Eval("JobId")%>' Height="18px" ToolTip="Upload LR Copy."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>--%>
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
                        <asp:TemplateField HeaderText="Rejection Type" SortExpression="ReasonforPendency" Visible="false">
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
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-Width="8%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                        <asp:TemplateField HeaderText="Remark" SortExpression="Instructions" ItemStyle-Width="10%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="labInstructions" runat="server" Text='<%# Eval("Instructions") %>'></asp:Label>
                                <asp:Button ID="btnInstructions" runat="server" Text="" OnCommand="txtInstructions_Changed" CommandArgument='<%# Bind("JobID") %>' Style="display: none" />
                                <asp:TextBox ID="txtInstructions" runat="server" Text='<%# Eval("Instructions") %>' Width="175px"
                                    Style="display: none" TextMode="MultiLine"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out Of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
                        <asp:BoundField DataField="RejectedRemark" HeaderText="Rejection Remark" />
                        <asp:TemplateField HeaderText="Aging I" SortExpression="Aging">
                            <ItemTemplate>
                                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Billing Advice - Job Received in LR Pending"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aging II" SortExpression="Aging2">
                            <ItemTemplate>
                                <asp:Label ID="lblAging2" runat="server" Text='<%#Eval("Aging2") %>' ToolTip="Today – Dispatch Date"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BackOfficeDate" HeaderText="Date on Job Forwarded For Billing Scrutiny" Visible="false" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BackOfficeDate" />
                        <asp:BoundField HeaderText="Billing Scrutiny Rejection Date" DataField="RejectedDate" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Rejection Remark" DataField="RejectRemark" Visible="false" />
                        <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy" Visible="false" />
                        <asp:BoundField HeaderText="Remark" ItemStyle-Width="8%" Visible="false" />
                        <asp:TemplateField HeaderText="Sent To" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkScrutiny" runat="server" Text="Send" CommandName="SendForScrutiny"
                                    CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")+";"+ Eval("ReceivedId")+";"+Eval("JobRefNo")%>' ValidationGroup="vgLRCopy"
                                    OnClientClick="return confirm('Are you sure wants to Move the Job Ahead?');"></asp:LinkButton>
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
                        <asp:HiddenField ID="hdnUploadPath" runat="server" />
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
                            <div style="width: 100%">
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
                                                    <asp:Label ID="lblRejectionType" runat="server" Text="LR/DC"></asp:Label>
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
                <asp:LinkButton ID="hdnHoldExp" runat="server" Text=""></asp:LinkButton>
            </div>

            <%--  END   : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <div>
                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetLRPending" SelectCommandType="StoredProcedure">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


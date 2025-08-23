<%@ Page Title="Pending Billing Advice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingBillingAdvice.aspx.cs"
    Inherits="ExportPCA_PendingBillingAdvice" EnableEventValidation="false" Culture="en-GB" %>


<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <%--<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />--%>
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
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
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset id="mainfield" runat="server" class="fieldset-AutoWidth">
                <legend>Pending Billing Advice</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 40px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="99%"
                    PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender" DataSourceID="PCDSqlDataSource"
                    OnRowCommand="gvJobDetail_RowCommand" OnRowDataBound="gvJobDetail_RowDataBound">
                    <Columns>
                        <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False" />
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Hold" >
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/UnlockImg.png"
                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("Amount") + ";" + Eval("JobRefNo")%>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Job."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:ImageButton ID="imgbtnUnholdJob" runat="server" ImageUrl="~/Images/LockImg.png"
                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("Amount") + ";" + Eval("JobRefNo")%>' CommandName="Unhold" Width="18px" Height="18px" ToolTip="Unhold Job."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:HiddenField ID="hdnJobId" runat="server" value='<%#Eval("JobId")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billing Instruction">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkInstruction" runat="server" Text="Billing Instruction" CommandName="InstructionPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Instr_status" HeaderText="Inst Status" />
                        <asp:BoundField DataField="Doc_status" HeaderText="Doc Status" />
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

                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="ShipperName" HeaderText="Shipper" SortExpression="ShipperName" />

                        <%--<asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />--%>
                        <%--<asp:TemplateField HeaderText="Instructions" SortExpression="Instructions">
                            <ItemTemplate>
                                <asp:Label ID="labInstructions" runat="server" Text='<%# Eval("Instructions") %>'></asp:Label>
                                <asp:Button ID="btnInstructions" runat="server" Text="" OnCommand="txtInstructions_Changed" CommandArgument='<%# Bind("JobID") %>' Style="display: none" />
                                <asp:TextBox ID="txtInstructions" runat="server" Text='<%# Eval("Instructions") %>' Width="175px"
                                    Style="display: none" TextMode="MultiLine"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="PCA To Customer" SortExpression="PCDRequired">
                            <ItemTemplate>
                                <%# (Boolean.Parse(Eval("PCDRequired").ToString())) ? "Yes" : "No"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OutOfChargeDate" HeaderText="Document Hand Over To Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" />
                        <asp:BoundField DataField="LastDispatchDate" HtmlEncode="false" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" Visible="false" />
                        <asp:TemplateField HeaderText="Billing Documents">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDocument" runat="server" Text="List of Billing Advice Doc" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aging I">
                            <ItemTemplate>
                                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Today – Document Hand Over To Shipping Line Date."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BackOfficeDate" HeaderText="Date on Job Forwarded For Billing Advice" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BackOfficeDate" />
                        <asp:BoundField HeaderText="Billing Scrutiny Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Rejection Remark" DataField="RejectRemark" />
                        <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy" />
                        <asp:BoundField HeaderText="Remark" />
                        <asp:TemplateField HeaderText="Contractual Billing Instruction" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnot" runat="server" Value='<%#Eval("statuscontract")%>' />
                                <%--<asp:HiddenField ID="hdnJOBID" runat="server" Value='<%#Eval("JobId") %>' />--%>
                                <asp:LinkButton ID="lnkotherinstru" runat="server" Text='<%#Eval("billing")%>' CommandName="ContractualBillingInstruction" CommandArgument='<%#Eval("JobId")  + ";" + Eval("JobRefNo") + ";" + Eval("ConsigneeName") %>'></asp:LinkButton>
                                <%--<asp:LinkButton ID="lnkInstruction" runat="server" Text="Billing Instruction" CommandName="InstructionPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billing Scrutiny">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkScrutiny" runat="server" Text="Send Document Scrutiny" CommandName="SendForScrutiny" CommandArgument='<%#Eval("JobId") %>'
                                    OnClientClick="return confirm('Are you sure to Move the Job To Scrutiny ?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                        <asp:HiddenField ID="hdnBranchId" runat="server" />
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
                                    AllowSorting="True" PageSize="20" OnRowDataBound="gvPCDDocument_RowDataBound"
                                    OnRowCommand="gvPCDDocument_RowCommand">
                                    <%--DataSourceID="PCDDocumentSqlDataSource"--%>
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
                                                    <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction1" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction2" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy2" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction3" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy3" runat="server" />
                                                </td>
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

            <!--Contract Billing Start-->
            <div id="divContractBillingPrint" runat="server" visible="False">
               
            </div>
            <div id="divContractBilling" runat="server">
                <cc1:modalpopupextender id="ModalPopupContractBilling" runat="server" cachedynamicresults="false"
                    dropshadow="False" popupcontrolid="PanelContractBilling" targetcontrolid="lnkDummyContractBilling">
                </cc1:modalpopupextender>

                <%-- <cc1:ModalPopupExtender ID="ModalPopupContractBillingprint" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="PanelContractBillingPrint" TargetControlID="lnkDummyContractBillingprint">
                </cc1:ModalPopupExtender>--%>

                <asp:Panel ID="PanelContractBilling" runat="server" CssClass="ModalPopupPanel" Width="70%" Height="80%">
                    <div class="header">
                        <div class="fleft">
                            &nbsp;<asp:Button ID="btncloseContractBilling" runat="server" Text="Close" CausesValidation="false" OnClick="btncloseContractBilling_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;    
                                <asp:Button ID="btnsaveContractBilling" Text="Save Billing" runat="server" CausesValidation="true" ValidationGroup="RequiredField" OnClick="btnsaveContractBilling_Click" />
                        </div>
                    </div>
                    <div id="divContractBilling1" runat="server">
                        <div class="m"></div>
                        <div id="Div7" runat="server" style="max-height: 550px; overflow: auto;">
                            <!--Document for BIlling Instruction Start-->
                            <center>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                            </center>
                            <fieldset runat="server">
                                <legend>Billing Details</legend>
                                <table border="1" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr style="display: none;">
                                        <td>From Date</td>
                                        <td>
                                            <asp:TextBox ID="dtStartDate" runat="server" TabIndex="4" Text='' ReadOnly="true"></asp:TextBox></td>
                                        <td>To Date</td>
                                        <td>
                                            <asp:TextBox ID="dtEndDate" runat="server" TabIndex="4" Text='' ReadOnly="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Customer Name</td>
                                        <td>
                                            <asp:TextBox ID="txtcustname" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>Consignee Name</td>
                                        <td>
                                            <asp:TextBox ID="txtconsigneename" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Branch</td>
                                        <td>
                                            <asp:TextBox ID="txtbranch" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>Port</td>
                                        <td>
                                            <asp:TextBox ID="txtport" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total No of Containers</td>
                                        <td>
                                            <asp:TextBox ID="txtcontainer" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>No of Packages</td>
                                        <td>
                                            <asp:TextBox ID="txtpackages" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Sum of 20</td>
                                        <td>
                                            <asp:TextBox ID="txtsum20" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>Sum of 40</td>
                                        <td>
                                            <asp:TextBox ID="txtsum40" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Gross Weight</td>
                                        <td>
                                            <asp:TextBox ID="txtgrossweight" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>Type of BOE</td>
                                        <td>
                                            <asp:TextBox ID="txttypeboe" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Delivery Type</td>
                                        <td>
                                            <asp:TextBox ID="txtdeliverytype" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>Job Type</td>
                                        <td>
                                            <asp:TextBox ID="txtjobtype" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Job No.</td>
                                        <td>
                                            <asp:TextBox ID="txtjobno" runat="server" TabIndex="4" Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td>Container Type</td>
                                        <td>
                                            <asp:TextBox ID="txtContainerType" runat="server"  Text='' ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>

                        </div>
                    </div>
                    <div style="overflow:auto;Height:300px;" >

                        <asp:GridView ID="grdcontract_User" runat="server" AutoGenerateColumns="False" CssClass="table" Style="white-space: normal;"
                            PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True" HeaderStyle-HorizontalAlign="Center" width="98%" 
                            PageSize="20" PagerSettings-Position="TopAndBottom" OnRowDataBound="grdcontract_User_RowDataBound"  >
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="Selected">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All To Add user charges" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelection" runat="server" Checked='<%# bool.Parse(Eval("Selection").ToString()) %>' ToolTip="Check To Add user Charges" OnCheckedChanged="CheckBox_Changed" AutoPostBack="true"></asp:CheckBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>--%>

                                <%-- <asp:BoundField DataField="lid" ItemStyle-Width="0">
                                    <HeaderStyle Width="0px" />
                                    <ItemStyle Font-Size="1px" Width="1px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ChargeCode" ItemStyle-Width="0" >
                                    <HeaderStyle Width="0px" />
                                    <ItemStyle Font-Size="1px" Width="1px" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="UOM" HeaderText="UOM" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Condition" HeaderText="Condition" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Rate" HeaderText="Rate" Visible="false" />
                                <asp:TemplateField HeaderText="Q-Selection" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnlid" runat="server" Value='<%#Eval("lid")%>' />
                                        <asp:HiddenField ID="hdnchargecode" runat="server" Value='<%#Eval("ChargeCode")%>' />
                                        <asp:RadioButtonList ID="rdselect" runat="server" Width="70px" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="CheckBox_Changed" OnDataBound="CheckBox1_Changed">
                                            <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtqty" runat="server" Text='<%# Bind("Quantity") %>' onkeypress="return numeric(event);" ReadOnly="true" Width="30px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="username" HeaderText="User Name" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="userdate" HeaderText="Date" ItemStyle-Width="150px" />
                            </Columns>
                            <PagerSettings Position="TopAndBottom" />
                            <PagerStyle CssClass="pgr" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <center>
                            <asp:Button ID="btnAdduser" Text="Add User Charges" runat="server" OnClick="btnAdduser_Click" />
                        </center>
                    </div>
                    <div id="Div8" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">
                        <asp:GridView ID="grdbillinglinedetails" runat="server" AutoGenerateColumns="False" CssClass="table"
                            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Particulars" HeaderText="Particulars" SortExpression="Particulars" />
                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                <asp:BoundField DataField="AmountWithOutGST" HeaderText="AmountWithOutGST" SortExpression="AmountWithOutGST" />
                                <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" Visible="false" />
                                <asp:BoundField DataField="GST" HeaderText="GST" SortExpression="GST" />
                                <asp:BoundField DataField="Amt" HeaderText="Amt" SortExpression="Amt" />
                            </Columns>
                        </asp:GridView>

                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyContractBilling" runat="server" Text=""></asp:LinkButton>
                <asp:LinkButton ID="lnkDummyContractBilling2" runat="server" Text=""></asp:LinkButton>
                <asp:LinkButton ID="lnkDummyContractBillingprint" runat="server" Text=""></asp:LinkButton>
                <%--<asp:LinkButton ID="lnkDummyContractBilling2" runat="server" Text=""></asp:LinkButton>--%>
            </div>

            <!--Document for Contract Billing- END -->


            <div>
                <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure"><%--OnSelected="PCDDocumentSqlDataSource_Selected"--%>
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="BillingAdviceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" />
                        <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                    </SelectParameters>
                </asp:SqlDataSource>
                
                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetPendingBillingAdvice" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceHoldJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

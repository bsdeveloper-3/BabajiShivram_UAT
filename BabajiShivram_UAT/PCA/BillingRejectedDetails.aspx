<%@ Page Title="Rejected Job" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillingRejectedDetails.aspx.cs"
    Inherits="PCA_BillingRejectedDetails" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" />
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
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
                <asp:Label ID="lblChk" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryPassingDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="PassingRequired" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <asp:HiddenField ID="hdnCurrentId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnPrevId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnNxtId" runat="server" Value="0" />

            <fieldset>
                <legend>Rejected Job</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <table>
                                <tr style="width: auto">
                                    <asp:LinkButton ID="lnk" runat="server" OnClick="lnkExcel_Click" data-tooltip="&nbsp; Export To Excel">
                                        <asp:Image ID="img1" runat="server" ImageUrl="~/Images/Excel.jpg" />
                                    </asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;   
                                    <td valign="top">
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="BtnCompleted" runat="server" Text="Completed" ToolTip="Billing Rejection Completed"
                                            OnClick="BtnComplete_Click" />
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="BtnSendmail" runat="server" Text="Send Mail Notification" ToolTip="Billing Rejection Mail Notification" OnClick="BtnSendmail_Click" />
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtfollowupdone" runat="server" Width="3%" Height="10%" BackColor="Green"></asp:TextBox>
                                        <asp:Label ID="lblfollowupdone" runat="server" Text="Followup Completed" Font-Bold="true"></asp:Label>
                                        <asp:TextBox ID="txtfollowuppending" runat="server" Width="3%" Height="10%" BackColor="Red"></asp:TextBox>
                                        <asp:Label ID="lblfollowuppending" runat="server" Text="Followup Pending" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </asp:Panel>
                </div>
                <asp:Button ID="modelPopup" ValidationGroup="Required" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup" PopupControlID="Panel1"></cc1:ModalPopupExtender>
                <asp:Panel ID="Panel1" Style="display: none" runat="server">
                    <fieldset class="ModalPopupPanel">
                        <div title="Followup Details" class="header">
                            <textbox>Followup Details</textbox>
                        </div>

                        <table class="table">
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblmessage1" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnPrevious" runat="server" Text="<" Font-Size="10" OnClick="btnPrevious_click" /></td>
                                <td>

                                    <asp:Repeater ID="RptBillingfollowup" runat="server" DataSourceID="sqldatasourcebillingfollowup" OnItemDataBound="Repeater1_ItemDataBound">
                                        <HeaderTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl1" runat="server" Text="JobNo." Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblrefno" runat="server" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:Label ID="lbljobid" runat="server"></asp:Label>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table border="0">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblreason" runat="server" Text='<%#Eval("ReasonforPendency")%>' Font-Bold="true"></asp:Label>
                                                        <asp:Label ID="lblreasonId" runat="server" Text='<%#Eval("ReasonId")%>' Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Chk1" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                                <td>
                                    <asp:Button ID="btnNext" runat="server" Text=">" Font-Size="10" OnClick="btnNext_click" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnok" runat="server" Text="Ok" OnClick="btnok_click" CommandArgument='<%# Eval("JobId") %>' /><%----%>
                                    <asp:Button ID="btncancel" runat="server" Text="Cancel" OnClick="btncancel_click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>

                <asp:SqlDataSource ID="sqldatasourcelrdctype" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="Getlrdctype" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                <asp:SqlDataSource ID="sqldatasourcebillingfollowup" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetfollowupbillingRejected" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:Parameter Name="JobId" Type="int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <div class="fright">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" Visible="false">
                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="100%"
                    PageSize="20" PagerSettings-Position="Bottom" OnPreRender="gvJobDetail_PreRender" OnRowCreated="gvJobDetail_RowCreated"
                    DataSourceID="PCDRejectedSqlDataSource" OnRowCommand="gvJobDetail_RowCommand" OnRowDataBound="gvJobDetail_RowDataBound" OnPageIndexChanging="gvJobDetail_PageIndexChanging">
                    <HeaderStyle Font-Bold="true" BackColor="#CBCBDC" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server"
                                    Text="Update" Font-Underline="true" CommandArgument='<%#Eval("JobId") %>' ValidationGroup="PassingRequired"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                                    runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkboxSelectAll" ToolTip="Check All" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" ToolTip="Check" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" ReadOnly="true" SortExpression="JobRefNo" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                        <asp:BoundField DataField="KAM" HeaderText="KAM" SortExpression="KAM" ReadOnly="true" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" ReadOnly="true" />
                        <asp:TemplateField HeaderText="BJV Details">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnBJVDetails" runat="server" Text="Show" CommandName="showBJV" CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobType" HeaderText="Job Type" ReadOnly="true" SortExpression="JobType"/>
                        <asp:BoundField DataField="Aging1" HeaderText="Aging I" SortExpression="Aging1" ReadOnly="true" />

                        <%-- <asp:BoundField DataField="RejectedDate" HeaderText="Rejected Date"  DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" SortExpression="RejectedDate">           
                                </asp:BoundField>--%>

                        <asp:TemplateField HeaderText="Followup date" SortExpression="Followupdate">
                            <ItemTemplate>
                                <asp:Label ID="lblfollowupdate" runat="server" Text='<%# Eval("Followupdate","{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtfollowupdate" runat="server" Width="100px" Text='<%# Eval("Followupdate","{0:dd/MM/yyyy}")%>' TabIndex="1" placeholder="dd/mm/yyyy" ToolTip="Enter Followup Date"></asp:TextBox>
                                <asp:Image ID="imgfollowupDt" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                <asp:RequiredFieldValidator ID="RefvFollowupdate" runat="server" ControlToValidate="txtfollowupdate"
                                    Display="Dynamic" ErrorMessage="Please Enter Followup Date" ValidationGroup="PassingRequired"></asp:RequiredFieldValidator>
                                <cc1:CalendarExtender ID="calfollowupDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgfollowupDt" PopupPosition="BottomRight"
                                    TargetControlID="txtfollowupdate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskExtfollowupDate" TargetControlID="txtfollowupdate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValfollowupDate" ControlExtender="MskExtfollowupDate" ControlToValidate="txtfollowupdate" IsValidEmpty="true"
                                    InvalidValueMessage="followup Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                    runat="Server" ValidationGroup="PassingRequired"></cc1:MaskedEditValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Followup Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Eval("FollowupRemark")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtremarks" runat="server" ToolTip="Enter Followup Remark" Text='<%# Eval("FollowupRemark")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Refv" runat="server" ControlToValidate="txtremarks" Display="Dynamic" ErrorMessage="Please Enter Remarks" ValidationGroup="PassingRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rejected Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblReceipts" runat="server" Text='<%#Eval("Receipts") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblLRDC" runat="server"  Text='<%#Eval("LRDC") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblPCDACK" runat="server" Text='<%#Eval("PCDACK") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblMailApproval" runat="server" Text='<%#Eval("MailApproval") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblOthers" runat="server" Text='<%#Eval("Others") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected by">
                            <ItemTemplate>
                                <asp:Label ID="lblRejectedbyName" runat="server" Text='<%# Eval("Rejectedby")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected Type" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRejectedby" runat="server" Text='<%# Eval("ReceivedId")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="rulename">
                            <ItemTemplate>
                                <asp:Label ID="rulename" runat="server" Text='<%#Eval("rulename")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Followupdate" HeaderText="Followup date" Visible="false" />
                        <asp:TemplateField HeaderText="Jobid" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblJobID" runat="server" Text='<%# Eval("JobId")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Followup History">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkviewhistory" runat="server" OnClick="lnkviewhistory_Click" Text="View Followup History" ForeColor="Blue"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
<%--                        <asp:TemplateField HeaderText="Consolidate Bill">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkConsolidateBill" runat="server" OnClick="lnkConsolidateBill_Click" Text="Consolidate Bill" ForeColor="Blue"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>

                </asp:GridView>
                <asp:Button ID="btnFollowuphistory" ValidationGroup="Required" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderFollowuphistory" runat="server" TargetControlID="btnFollowuphistory" PopupControlID="PanelFollowuphistory"></cc1:ModalPopupExtender>
                <asp:Panel ID="PanelFollowuphistory" Style="display: none" runat="server">
                    <fieldset class="ModalPopupPanel">
                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                        </div>
                        <div title="Followup History" class="header">
                            <textbox>Followup History</textbox>
                            <asp:GridView ID="grdfollowuphistory" runat="server" DataSourceID="SqlDataSourcefollowuphistory" CssClass="gridview" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="SR.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("Row")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JobNo.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("JobRefNo")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ReasonforPendency">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("ReasonforPendency")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Followup Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("Followupdate","{0:dd/MM/yyyy}")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Followup Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("FollowupRemark")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Followup Completed Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("Followupcomplete_date","{0:dd/MM/yyyy}")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>
                    </fieldset>
                </asp:Panel>

                <!-- Consolidate Bill --->
                <asp:LinkButton ID="lnkDummyConsoledate" runat="server"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderConsolidate" runat="server" TargetControlID="lnkDummyConsoledate" PopupControlID="PanelConsolidate"></cc1:ModalPopupExtender>
                <asp:Panel ID="PanelConsolidate" Style="display: none" runat="server">
                    <fieldset class="ModalPopupPanel">
                        <div class="fright">
                            <asp:ImageButton ID="ImageClose11" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                        </div>
                        <div title="Consoldiate Job No" class="header">
                            <asp:GridView ID="GridViewConsolidate" runat="server" DataSourceID="SqlDataSourcefollowuphistory" CssClass="gridview" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="SR.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("Row")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JobNo.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("JobRefNo")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ReasonforPendency">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("ReasonforPendency")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Followup Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("Followupdate","{0:dd/MM/yyyy}")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Followup Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("FollowupRemark")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Followup Completed Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" runat="server" Text='<%#Eval("Followupcomplete_date","{0:dd/MM/yyyy}")%>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>
                    </fieldset>
                </asp:Panel>
            </fieldset>
            <div>

                <asp:SqlDataSource ID="SqlDataSourcefollowuphistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BillingRejectedfollowuphistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="0" ConvertEmptyStringToNull="true" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:Parameter Name="JobId" Type="int32" />
                    </SelectParameters>

                </asp:SqlDataSource>

                <asp:SqlDataSource ID="PCDRejectedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBillingRejectedRemark" SelectCommandType="StoredProcedure" UpdateCommand="updBillingRejected"
                    UpdateCommandType="StoredProcedure" OnUpdated="PCDRejectedSqlDataSource_Updated">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="0" ConvertEmptyStringToNull="true" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="JobId" Type="int32" />
                        <asp:Parameter Name="FollowupDate" Type="datetime" />
                        <asp:Parameter Name="FollowupRemark" Type="string" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Type="int32" Direction="Output" Size="4" />
                    </UpdateParameters>

                </asp:SqlDataSource>
            </div>

            <div style="text-align: right">
                <br />
                <asp:Repeater ID="Rptpriorities" runat="server" DataSourceID="SqlDataSourcepriorities" OnItemDataBound="Rptpriorities_ItemDataBound">
                    <ItemTemplate>
                        <asp:TextBox ID="txtpriorities" runat="server" Width="0.5%"></asp:TextBox>
                        <asp:Label ID="lblpriorities" runat="server" Text='<%#Eval("prioritiesName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!--Popup for BJV details - Start -->
            <div id="divBJVDetails">
                <cc1:ModalPopupExtender ID="mpeBJVDetails" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="pnlBJVDetails" TargetControlID="lnkDummy2">
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

            <asp:SqlDataSource ID="SqlDataSourcepriorities" runat="server"
                ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="Getpriorities" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


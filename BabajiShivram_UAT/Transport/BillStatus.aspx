<%@ Page Title="Bill Status" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillStatus.aspx.cs"
    Inherits="Transport_BillStatus" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlBillStatus" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlBillStatus" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnTransporterId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnStatusId" runat="server" Value="0" />
            <fieldset>
                <legend>Bill Status Summary - Transporter Wise</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <br />
                <asp:GridView ID="gvTransBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="TransporterId"
                    PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnRowDataBound="gvTransBillDetail_RowDataBound" OnRowCommand="gvTransBillDetail_RowCommand"
                    OnPreRender="gvTransBillDetail_PreRender" DataSourceID="SqlDataSourceBillAmt">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustName" HeaderText="Transporter" SortExpression="CustName" />
                        <asp:BoundField DataField="NoofJobs" HeaderText="No of Jobs" SortExpression="NoofJobs" />
                        <asp:BoundField DataField="NoofBills" HeaderText="No of Bills" SortExpression="NoofBills" />
                        <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" SortExpression="BillAmount" />
                        <asp:TemplateField HeaderText="Bill Pending">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnBillPending" CommandName="NoBill" runat="server" Text='<%#Eval("BillPending") %>'
                                    CommandArgument='<%#Eval("TransporterId") + ";" + Eval("CustName")%>' Font-Bold="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Memos">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnMemos" CommandName="Memo" runat="server" Text='<%#Eval("TotalMemos") %>'
                                    CommandArgument='<%#Eval("TransporterId") + ";" + Eval("CustName")%>' Font-Bold="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cheques">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnCheques" CommandName="Cheque" runat="server" Text='<%#Eval("TotalCheques") %>'
                                    CommandArgument='<%#Eval("TransporterId") + ";" + Eval("CustName")%>' Font-Bold="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill Hold">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnHoldBill" CommandName="HoldBill" runat="server" Text='<%#Eval("TotalBillHold") %>'
                                    CommandArgument='<%#Eval("TransporterId") + ";" + Eval("CustName")%>' Font-Bold="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Released">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnRelease" CommandName="Release" runat="server" Text='<%#Eval("PaymentRelease") %>'
                                    CommandArgument='<%#Eval("TransporterId") + ";" + Eval("CustName")%>' Font-Bold="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>

            <%-- START  :- No of Memos Status Report--%>
            <asp:HiddenField ID="hdnMemo" runat="server"></asp:HiddenField>
            <AjaxToolkit:ModalPopupExtender ID="mpeMemoStatus" runat="server" TargetControlID="hdnMemo" PopupControlID="pnlMemoStatus" BackgroundCssClass="modalBackground">
            </AjaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMemoStatus" runat="server" CssClass="ModalPopupPanel" BackColor="#F5F5DC" Width="1120px" Height="420px" Style="border: 1px solid #cccc; border-radius: 5px">
                <div class="header">
                    <div class="fleft">
                        Total Number of Memo's - 
                        <asp:Label ID="lblTransporter_Memo" runat="server" Font-Underline="true"></asp:Label>
                        &nbsp;                    
                        <asp:LinkButton ID="lnkbtnExportMemo" runat="server" OnClick="lnkbtnExportMemo_Click" data-tooltip="&nbsp; Export To Excel">
                            <asp:Image ID="imgExportMemo" runat="server" ImageUrl="~/images/Excel.jpg" />
                        </asp:LinkButton>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgbtnCancelMemo" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnCancelMemo_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="clear">
                    <asp:Label ID="lblErrorMemo" runat="server"></asp:Label>
                </div>
                <asp:Panel ID="pnlMemoStatusBody" runat="server" Width="1100px" Height="370px" ScrollBars="Vertical" Style="padding: 3px">
                    <asp:GridView ID="gvMemoStatusDetail" runat="server" CssClass="table" Style="white-space: initial" Width="99%" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true"
                        PagerSettings-Position="TopAndBottom" PageSize="80" ShowFooter="true" DataSourceID="DataSourceMemoStatusDetail" OnRowCommand="gvMemoStatusDetail_RowCommand"
                         OnRowDataBound="gvMemoStatusDetail_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TRRefNo" HeaderText="TR Ref No" SortExpression="TRRefNo" />
                            <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" SortExpression="PartyName" />
                            <asp:BoundField DataField="BillNumber" HeaderText="Bill No" SortExpression="BillNumber" />
                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amt" SortExpression="BillAmount" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="VaraiAmount" HeaderText="Varai Amt" SortExpression="VaraiAmount"  ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="EmptyContRcptCharges" HeaderText="Empty Cont Receipt Chrg" SortExpression="EmptyContRcptCharges" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TollCharges" HeaderText="Toll Chrg" SortExpression="TollCharges" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="OtherCharges" HeaderText="Other Chrg" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="BillCreatedBy" HeaderText="Bill Created By" SortExpression="BillCreatedBy" Visible="false" />
                            <asp:BoundField DataField="BillCreatedOn" HeaderText="Bill Created On" SortExpression="BillCreatedOn" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Sent User" HeaderText="Sent User" SortExpression="SentUser" Visible="false" />
                            <asp:BoundField DataField="SentDate" HeaderText="Sent Date" SortExpression="SentDate" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Priority">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnTransBillId" runat="server" Value='<%#Bind("TransBillId") %>' />
                                    <asp:DropDownList ID="ddlPriority" runat="server" Width="100px" DataSourceID="DataSourcePriority" DataTextField="sName" DataValueField="lid" 
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="ddlPriority" InitialValue="0" SetFocusOnError="true"
                                        Display="Dynamic" ForeColor="Red" ErrorMessage="Required" ValidationGroup="vgPriorityReq"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="Priority" Text="Save" ButtonType="Button" ValidationGroup="vgPriorityReq" ItemStyle-Font-Bold="true" />
                            <asp:BoundField DataField="Priority" HeaderText="Priority" Visible="false" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>

            <%-- END    :- No of Memos Status Report--%>

            <%-- START  :- Other Bill Status Report--%>
            <asp:HiddenField ID="hdnPopup1" runat="server"></asp:HiddenField>
            <AjaxToolkit:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="hdnPopup1" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground">
            </AjaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlPopup" runat="server" CssClass="ModalPopupPanel" BackColor="#F5F5DC" Width="1020px" Height="420px" Style="border: 1px solid #cccc; border-radius: 5px">
                <div class="header">
                    <div class="fleft">
                        <asp:Label ID="lblPopup_Title" runat="server"></asp:Label>
                        <asp:Label ID="lblTransporter_OtherStatus" runat="server" Font-Underline="true"></asp:Label>
                        &nbsp;                    
                        <asp:LinkButton ID="lnkbtnExportExcel" runat="server" OnClick="lnkbtnExportExcel_Click" data-tooltip="&nbsp; Export To Excel">
                            <asp:Image ID="imgExportExcel" runat="server" ImageUrl="~/images/Excel.jpg" />
                        </asp:LinkButton>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="clear">
                    <asp:Label ID="lblError_Popup" runat="server"></asp:Label>
                </div>
                <asp:Panel ID="pnlInnerPopup" runat="server" Width="1000px" Height="370px" ScrollBars="Vertical" Style="padding: 3px">
                    <asp:GridView ID="gvBillStatusDetail" runat="server" CssClass="table" Style="white-space: initial" OnRowDataBound="gvBillStatusDetail_RowDataBound"
                        Width="99%" AutoGenerateColumns="false" AllowSorting="true" ShowFooter="true" AllowPaging="true" PagerSettings-Position="TopAndBottom" PageSize="80">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TRRefNo" HeaderText="TR Ref No" SortExpression="TRRefNo" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" SortExpression="JobRefNo" ItemStyle-Width="26%" />
                            <asp:BoundField DataField="BillNumber" HeaderText="Bill No" SortExpression="BillNumber" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amt" SortExpression="BillAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="VaraiAmount" HeaderText="Varai Amt" SortExpression="VaraiAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="EmptyContRcptCharges" HeaderText="Empty Cont Receipt Chrg" SortExpression="EmptyContRcptCharges" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="TollCharges" HeaderText="Toll Chrg" SortExpression="TollCharges" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="OtherCharges" HeaderText="Other Chrg" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>
            <%-- END    :- Other Bill Status Report--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div>
        <asp:SqlDataSource ID="SqlDataSourceBillAmt" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_rptBillAmtTransporter" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourcePriority" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="CRM_GetPriorityMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetNoofBillPending" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTransporterId" PropertyName="Value" Name="TransporterId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceMemoStatusDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetMemoBillStatus" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTransporterId" PropertyName="Value" Name="TransporterId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceChequeStatusDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetChequeBillStatus" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTransporterId" PropertyName="Value" Name="TransporterId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceReleaseStatusDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetReleaseBillStatus" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTransporterId" PropertyName="Value" Name="TransporterId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillHoldStatusDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetHoldBillStatus" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTransporterId" PropertyName="Value" Name="TransporterId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>


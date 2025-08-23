<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillingReport.aspx.cs" Inherits="Reports_BillingReport" Debug="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <table border="0" cellpadding="0" cellspacing="0" width="80%" bgcolor="white" style="Width:100%; align-content:center;">
                <tr>
                    <td>
                        Billing Advise From
                        <cc1:CalendarExtender ID="CalExtJobFromDate" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd-MMM-yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
                            TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"  ></asp:TextBox>
                        <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                            runat="server"/>
                        <%--<asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date" 
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                        </asp:CompareValidator>--%>
                    </td>
                    <td>
                        Billing Advise To
                        <cc1:CalendarExtender ID="CalExtJobFromTo" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd-MMM-yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
                            TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"  ></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server"/>
                        <%--<asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>    --%>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server" visible="false">
                            <%--<asp:ListItem Value="" Text="All Job"></asp:ListItem>--%>
                            <asp:ListItem Value="1" Text="Cleared"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Un-Cleared" Selected="True" ></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit"  />
                    </td>
                </tr>
            </table>
            <div class="clear"></div>
            <div class="clear"></div>

            <fieldset><legend>Billing Report </legend>
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click" >  
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>

                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="true" DataSourceID="DataSourceReport" Visible="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="BranchName" DataField="BranchName" />
                        <asp:BoundField HeaderText="Consignee" DataField="ConsigneeName" />
                        <asp:BoundField HeaderText="Customer" DataField="CustName" />
                        <asp:BoundField HeaderText="Mode" DataField="TransMode" />
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                        <%--<asp:BoundField HeaderText="Billing Advise date" DataField="Billing Advise date" />--%>
                        <asp:BoundField HeaderText="Billing Scrutiny Completed By" DataField="BillingScrutinyCompleted" />
                       <asp:BoundField HeaderText="Billing Scrutiny Completed Date" DataField="BillingScrutinyCompletedDate" />
                        <asp:BoundField HeaderText="Billing Scrutiny Received By" DataField="BillingScrutinyReceivedBy" />
                        <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="BillingScrutinyReceivedDate" />
                       <asp:BoundField HeaderText="Draft Check Completed By" DataField="DraftCheckCompletedBy" />
                        <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckCompletedDate" />
                        <asp:BoundField HeaderText="Draft Check Received By" DataField="DraftCheckReceivedBy" />
                        <asp:BoundField HeaderText="Draft Check Received Date" DataField="DraftCheckReceivedDate" />
                        <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="DraftInvoiceCompletedBy" />
                        <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceCompletedDate" />
                        <asp:BoundField HeaderText="Draft Invoice Received By" DataField="DraftInvoiceReceivedBy" />
                        <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="DraftInvoiceReceivedDate" />
                        <asp:BoundField HeaderText="Final Check Completed By" DataField="FinalCheckCompletedBy" />
                        <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckCompletedDate" />
                        <asp:BoundField HeaderText="Final Check Received By" DataField="FinalCheckReceivedBy" />
                        <asp:BoundField HeaderText="Final Check Received Date" DataField="FinalCheckReceivedDate" />
                        <asp:BoundField HeaderText="Final Check Tick/Cross" DataField="Final Check Tick/Cross" />
                        <asp:BoundField HeaderText="Final Type Completed By" DataField="FinalTypeCompletedBy" />
                        <asp:BoundField HeaderText="Final Type Completed Date" DataField="FinalTypeCompletedDate" />
                        <asp:BoundField HeaderText="Final Type Received By" DataField="FinalTypeReceivedBy" />
                        <asp:BoundField HeaderText="Final Type Received Date" DataField="FinalTypeReceivedDate" />
                        <asp:BoundField HeaderText="Bill Dispatch Received By" DataField="BillDispatchReceivedBy" />
                        <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="BillDispatchReceivedDate" />
                        <asp:BoundField HeaderText="Bill Dispatch Completed By" DataField="BillDispatchCompletedBy" />
                        <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchCompletedDate" />
                        <asp:BoundField HeaderText="Final Type InvoNo" DataField="Final Type InvoNo" />
                        <asp:BoundField HeaderText="Final Type InvoDate" DataField="Final Type InvoDate" />
                        <asp:BoundField HeaderText="Billing Adv Completed By" DataField="Billing Adv Completed By" />
                        <asp:BoundField HeaderText="Billing Adv Completed Date" DataField="Billing Adv Completed Date" />
                        <asp:BoundField HeaderText="Dispatch Date" DataField="Dispatch Date" />
                        <asp:BoundField HeaderText="Reject Reason" DataField="Reject Reason" />
                        <asp:BoundField HeaderText="Rejection Completed By" DataField="Rejection Completed By" />
                        <asp:BoundField HeaderText="Rejection Completed Date" DataField="Rejection Completed Date" />
                        <asp:BoundField HeaderText="Rejected Stage" DataField="Rejected Stage" />
                        <%--<asp:BoundField HeaderText="Created Date" DataField="Created Date" />--%>
                    </Columns>
                </asp:GridView>

                <div>
                    <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                     SelectCommand="GET_BillingReport" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="FINYEAR" SessionField="FinYearId" />
                        <asp:ControlParameter Name="DateFrom" ControlID="txtFromDate" PropertyName="Text" Type="datetime" />
                        <asp:ControlParameter Name="DateTo" ControlID="txtToDate" PropertyName="Text" Type="datetime"/>
                        <%--<asp:ControlParameter ControlID="ddClearedStatus" Name="Status" PropertyName="SelectedValue" />--%>
                    </SelectParameters>
                </asp:SqlDataSource>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


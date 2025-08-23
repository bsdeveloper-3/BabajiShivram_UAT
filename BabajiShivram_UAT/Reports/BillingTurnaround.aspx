<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillingTurnaround.aspx.cs" Inherits="Reports_BillingTurnaround" %>

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
                        Job Date From
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
                        Job Date To
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
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
            <div class="clear"></div>
            <div class="clear"></div>

            <fieldset><legend>Billing TurnAround Report </legend>
                <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click" >  
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>

                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="true"  Visible="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                        <asp:BoundField HeaderText="Bill Dispatch Completed By" DataField="Bill Dispatch Completed By" />
                        <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="Bill Dispatch Completed Date" />
                        <asp:BoundField HeaderText="Bill Dispatch Received By" DataField="Bill Dispatch Received By" />
                        <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="Bill Dispatch Received Date" />
                        <asp:BoundField HeaderText="Billing Adv Completed By" DataField="Billing Adv Completed By" />
                        <asp:BoundField HeaderText="Billing Adv Completed Date" DataField="Billing Adv Completed Date" />
                        <asp:BoundField HeaderText="Billing Instruction" DataField="Billing Instruction" />
                        <asp:BoundField HeaderText="Billing Scrutiny Completed By" DataField="Billing Scrutiny Completed By" />
                        <asp:BoundField HeaderText="Billing Scrutiny Completed Date" DataField="Billing Scrutiny Completed Date" />
                        <asp:BoundField HeaderText="Billing Scrutiny Received By" DataField="Billing Scrutiny Received By" />
                        <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="Billing Scrutiny Received Date" />
                        <%--<asp:BoundField HeaderText="Delivery Date" DataField="Delivery Date" />--%>
                        <asp:BoundField HeaderText="Dispatch Date" DataField="Dispatch Date" />
                        <asp:BoundField HeaderText="Dispatch Type" DataField="Dispatch Type" />
                        <asp:BoundField HeaderText="Draft Check Completed By" DataField="Draft Check Completed By" />
                        <asp:BoundField HeaderText="Draft Check Completed Date" DataField="Draft Check Completed Date" />
                        <asp:BoundField HeaderText="Draft Check Received By" DataField="Draft Check Received By" />
                        <asp:BoundField HeaderText="Draft Check Received Date" DataField="Draft Check Received Date" />
                        <asp:BoundField HeaderText="Draft InvoDate" DataField="Draft InvoDate" />
                        <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="Draft Invoice Completed By" />
                        <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="Draft Invoice Completed Date" />
                        <asp:BoundField HeaderText="Draft Invoice Received By" DataField="Draft Invoice Received By" />
                        <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="Draft Invoice Received Date" />
                        <asp:BoundField HeaderText="Draft InvoNo" DataField="Draft InvoNo" />
                        <asp:BoundField HeaderText="Final Check Completed By" DataField="Final Check Completed By" />
                        <asp:BoundField HeaderText="Final Check Completed Date" DataField="Final Check Completed Date" />
                        <asp:BoundField HeaderText="Final Check Received By" DataField="Final Check Received By" />
                        <asp:BoundField HeaderText="Final Check Received Date" DataField="Final Check Received Date" />
                        <asp:BoundField HeaderText="Final Check Tick/Cross" DataField="Final Check Tick/Cross" />
                        <asp:BoundField HeaderText="Final Type Completed By" DataField="Final Type Completed By" />
                        <asp:BoundField HeaderText="Final Type Completed Date" DataField="Final Type Completed Date" />
                        <asp:BoundField HeaderText="Final Type InvoNo" DataField="Final Type InvoNo" />
                        <asp:BoundField HeaderText="Final Type InvoDate" DataField="Final Type InvoDate" />
                        <asp:BoundField HeaderText="Final Type Received By" DataField="Final Type Received By" />
                        <asp:BoundField HeaderText="Final Type Received Date" DataField="Final Type Received Date" />
                        <asp:BoundField HeaderText="Follouwup remark" DataField="Follouwup remark" />
                        <asp:BoundField HeaderText="Reject Reason" DataField="Reject Reason" />
                        <asp:BoundField HeaderText="Rejected By" DataField="Rejected By" />
                        <asp:BoundField HeaderText="Rejected Date" DataField="Rejected date" />
                        <asp:BoundField HeaderText="Rejected Stage" DataField="Rejected Stage" />
                        <asp:BoundField HeaderText="Rejection Completed By" DataField="Rejection Completed By" />
                        <asp:BoundField HeaderText="Rejection Completed Date" DataField="Rejection Completed Date" />
                       
                        
                         <%--<asp:BoundField HeaderText="Consoildated Jobno" DataField="Consoildated Jobno" />
                        <asp:BoundField HeaderText="Courier Name" DataField="Courier Name" />
                        <asp:BoundField HeaderText="Dispatch Docket No" DataField="Dispatch Docket No" />
                        <asp:BoundField HeaderText="Documents Carrying Person Name" DataField="Documents Carrying Person Name" />
                        <asp:BoundField HeaderText="Documents Received Person Name" DataField="Documents Received Person Name" />
                        <asp:BoundField HeaderText="Draft Check Tick/Cross" DataField="Draft Check Tick/Cross" />
                        <asp:BoundField HeaderText="Freight Job No" DataField="Freight Job No" />
                        <asp:BoundField HeaderText="LR/DC Type" DataField="LR/DC Type" />
                        <asp:BoundField HeaderText="MasterInvoiceDate" DataField="MasterInvoiceDate" />
                        <asp:BoundField HeaderText="MasterInvoiceNo" DataField="MasterInvoiceNo" />
                        <asp:BoundField HeaderText="Customer" DataField="Customer" />
                        <asp:BoundField HeaderText="Date of Dispatch From CFS" DataField="Date of Dispatch From CFS" />
                        <asp:BoundField HeaderText="Last Dispatch Date" DataField="Last Dispatch Date" />
                        <asp:BoundField HeaderText="Out of Charge Date" DataField="Out of Charge Date" />--%>

                        <%--<asp:BoundField HeaderText="Created Date" DataField="Created Date" />--%>
                    </Columns>
                </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BillReturnDetailBS.aspx.cs" Inherits="PCA_BillReturnDetailBS" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />

    <div>
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
    </div>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>Bill Return Detail</legend>
                    <asp:FormView ID="FVBillReturn" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId"
                        Width="100%" OnDataBound="FVBillReturn_DataBound">
                        <ItemTemplate>
                            <div class="m clear">
                                <asp:Button ID="btnEditJob" runat="server" OnClick="btnEditJob_Click" CssClass="btn"
                                    Text="Edit" />
                                <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" UseSubmitBehavior="false"
                                    Text="Back" CommandArgument="BillReturnBS.aspx" CausesValidation="false" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>BS Job No.
                                    </td>
                                    <td>
                                        <%# Eval("JobRefNo") %>                                    
                                    </td>
                                    <td>Cust Ref No.
                                    </td>
                                    <td>
                                        <%# Eval("CustRefNo") %>                                                
                                    </td>
                                </tr>
                                <tr>
                                    <td>Customer</td>
                                    <td>
                                        <%#Eval("CustName") %>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Bill No.</td>
                                    <td>
                                        <%#Eval("INVNO") %>
                                    </td>
                                    <td>Bill Amount</td>
                                    <td>
                                        <%#Eval("INVAMOUNT") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reason</td>
                                    <td>
                                        <asp:Label ID="lblRetutnReason" runat="server" Text='<%#Bind("BillReturnReason") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlReason" runat="server" Enabled="false" ForeColor="Black" Visible="false">
                                            <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="GST Invoice Related"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Teriff/Contract"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="SOP Contract"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Dispatch/Wrong Attension"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Warehouse"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Shipping Line"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="CFS"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblReason" runat="server"></asp:Label>
                                    </td>
                                    <td>Return Remark /Error</td>
                                    <td>
                                        <%#Eval("BillReturnRemark") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Return Date</td>
                                    <td>
                                        <%#Eval("BillRetDate") %>
                                    </td>
                                    <td>Bill Return By</td>
                                    <td>
                                        <%#Eval("BillReturnBy") %>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="m clear">
                                <asp:Button ID="btnUpdateJob" runat="server" OnClick="btnUpdateJob_Click" CssClass="update"
                                    Text="Update" ValidationGroup="Required" />
                                <asp:Button ID="btnCancelButton" runat="server" OnClick="btnCancelButton_Click" CausesValidation="False"
                                    CssClass="cancel" Text="Cancel" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>BS Job No.
                                    </td>
                                    <td>
                                        <%# Eval("JobRefNo") %>
                                        <asp:Label ID="lblBillReturnLid" runat="server" Text='<%#Bind("BillRetLid") %>' Visible="false"></asp:Label>
                                    </td>
                                    <td>Cust Ref No.
                                    </td>
                                    <td>
                                        <%# Eval("CustRefNo") %>                                                
                                    </td>
                                </tr>
                                <tr>
                                    <td>Customer</td>
                                    <td>
                                        <%#Eval("CustName") %>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Bill No.</td>
                                    <td>
                                        <%#Eval("INVNO") %>
                                    </td>
                                    <td>Bill Amount</td>
                                    <td>
                                        <%#Eval("INVAMOUNT") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reason</td>
                                    <td>
                                        <asp:Label ID="lblRetutnReason" runat="server" Text='<%#Bind("BillReturnReason") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlReason" runat="server" Enabled="false" ForeColor="Black" Visible="false">
                                            <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="GST Invoice Related"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Teriff/Contract"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="SOP Contract"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Dispatch/Wrong Attension"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Warehouse"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Shipping Line"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="CFS"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblReason" runat="server"></asp:Label>
                                    </td>
                                    <td>Return Remark / Error</td>
                                    <td>
                                        <%#Eval("BillReturnRemark") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Return Date</td>
                                    <td>
                                        <%#Eval("BillRetDate") %>
                                    </td>
                                    <td>Bill Return By</td>
                                    <td>
                                        <%#Eval("BillReturnBy") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Changes Date</td>
                                    <td>
                                        <asp:TextBox ID="txtchangeDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ControlToValidate="txtchangeDate"
                                            SetFocusOnError="true" ErrorMessage="Please Enter Changes Date" Display="Dynamic"
                                            Text="*" ValidationGroup="Required" InitialValue=""></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgBill" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                        <AjaxToolkit:CalendarExtender ID="calBillReturn" runat="server" Enabled="True" EnableViewState="False"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBill" PopupPosition="BottomRight"
                                            TargetControlID="txtchangeDate">
                                        </AjaxToolkit:CalendarExtender>
                                    </td>
                                    <td>New Dispatch Date</td>
                                    <td>
                                        <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDispatchDate"
                                            SetFocusOnError="true" ErrorMessage="Please Enter New Dispatch Date" Display="Dynamic"
                                            Text="*" ValidationGroup="Required" InitialValue=""></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgDispatch" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                        <AjaxToolkit:CalendarExtender ID="calBillDispatch" runat="server" Enabled="True" EnableViewState="False"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatch" PopupPosition="BottomRight"
                                            TargetControlID="txtDispatchDate">
                                        </AjaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Action Taken</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtRemark" runat="server" Width="400px" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDispatchDate"
                                            SetFocusOnError="true" ErrorMessage="Enter The Remark" Display="Dynamic"
                                            Text="*" ValidationGroup="Required" InitialValue=""></asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>

                        </EditItemTemplate>

                    </asp:FormView>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


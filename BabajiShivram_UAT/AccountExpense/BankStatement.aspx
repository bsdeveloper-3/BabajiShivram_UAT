<%@ Page Title="Bank Account Statement" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BankStatement.aspx.cs" Inherits="AccountExpense_BankStatement" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div>
                <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yy" PopupButtonID="imgDateFrom" PopupPosition="BottomRight"
                    TargetControlID="txtDateFrom">
                </cc1:CalendarExtender>
                <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yy" PopupButtonID="imgDateTo" PopupPosition="BottomRight"
                    TargetControlID="txtDateTo">
                </cc1:CalendarExtender>
            </div>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>Bank Statement</legend>    
            <div>
                <div class="fleft">
                </div>
                <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            </div>
            <div class="m clear"></div>
            <table border="0" cellpadding="0" cellspacing="0" width="80%" bgcolor="white">
                <tr>
                    <td> Date From
                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                            Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="Required"
                            ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="90px" Text=""></asp:TextBox>
                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td> Date To
                        <asp:RequiredFieldValidator ID="RFVDateTo" ValidationGroup="Required" runat="server"
                            Text="*" ControlToValidate="txtDateTo" ErrorMessage="Please Enter TO Date"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValDateTo" runat="server" ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="Required"
                            ErrorMessage="Invalid To Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Width="90px" Text=""></asp:TextBox>
                        <asp:Image ID="imgDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddAccount" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td><asp:Button ID="btnShowReport" Text="View Statement" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" /></td>
                </tr>
                <tr>
                    <td>
                        Cheque No
                        <asp:RequiredFieldValidator ID="RFVCheque" ValidationGroup="RequiredCheque" runat="server"
                            Text="*" ControlToValidate="txtChequeNo" ErrorMessage="Please Enter UTR/Cheque NO"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                    </td>
                    <td><asp:Button ID="btnSearchCheque" Text="Search Cheque" OnClick="btnSearchCheque_Click" runat="server" ValidationGroup="RequiredCheque" /></td>
                </tr>
            </table>
            </fieldset>
            <div class="clear"></div>
            <fieldset><legend>Statement</legend>
            <asp:GridView ID="gvStatement" runat="server" AutoGenerateColumns="false" CssClass="table">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="txn_date" HeaderText="Date" SortExpression="txn_date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
                    <asp:BoundField DataField="amt_withdrawal" HeaderText="Withdrawal" SortExpression="amt_withdrawal" />
                    <asp:BoundField DataField="amt_deposit" HeaderText="Deposit" SortExpression="amt_deposit" />
                    <asp:BoundField DataField="ref_usr_no" HeaderText="UTR" SortExpression="ref_usr_no" />
                    <asp:BoundField DataField="ref_chq_num" HeaderText="Cheque No" SortExpression="ref_chq_num" />
                    <asp:BoundField DataField="cod_txn_literal" HeaderText="Txn_Code" SortExpression="cod_txn_literal" />
                    <asp:BoundField DataField="txn_desc" HeaderText="Narration" SortExpression="txn_desc" />
                    <asp:BoundField DataField="balance" HeaderText="Balance" SortExpression="balance" />
                </Columns>
            </asp:GridView>
            </fieldset>
        </ContentTemplate> 
    </asp:UpdatePanel>
</asp:Content>
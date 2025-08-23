<%@ Page Title="Additional Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="AddtnlExpense.aspx.cs" Inherits="AccountExpense_AddtnlExpense" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">
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
            <div>
                <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateFrom" PopupPosition="BottomRight"
                    TargetControlID="txtDateFrom">
                </cc1:CalendarExtender>
            </div>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>Job Expense Report</legend>
            <div>
                <div class="fleft">
                    <asp:Button ID="btnShowReport" Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
                </div>
                <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            </div>
            <div class="m clear"></div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>Expense Date
                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                            Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                     </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td>Branch</td>
                    <td>
                        <asp:DropDownList ID="ddBranch" runat="server"></asp:DropDownList>
                    </td>
                </tr>    
            </table>
            <div class="m clear"></div>
            <div>
                <asp:GridView ID="gvJobExpensesReport" runat="server" AutoGenerateColumns="false" CssClass="table"
                    AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job Ref No" DataField="JobRefNo" SortExpression="JobRefNo" />
                        <asp:BoundField HeaderText="Customer" DataField="Customer" SortExpression="Customer" />
                        <asp:BoundField HeaderText="Expense Type" DataField="ExpenseType" SortExpression="ExpenseType" />
                        <asp:BoundField HeaderText="Billable ?" DataField="BillableNoBillable" SortExpression="BillableNoBillable" />
                        <asp:BoundField HeaderText="Location" DataField="Location" SortExpression="Location" />
                        <asp:BoundField HeaderText="Amount" DataField="Amount" SortExpression="Amount" />
                    </Columns>
                </asp:GridView>
            </div>
       </fieldset> 
        <asp:SqlDataSource ID="datasrcJobExpenses" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommandType="StoredProcedure" SelectCommand="GetAddtnlExpense" >
            <SelectParameters>
                <asp:ControlParameter Name="ExpenseDate" ControlID="txtDateFrom" PropertyName="Text" Type="datetime" />
                <asp:ControlParameter Name="BranchId" ControlID="ddBranch" PropertyName="SelectedValue" />        
            </SelectParameters>
            </asp:SqlDataSource> 
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>
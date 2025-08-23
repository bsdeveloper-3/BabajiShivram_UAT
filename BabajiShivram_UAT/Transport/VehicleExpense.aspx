<%@ Page Title="Maintenance Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="VehicleExpense.aspx.cs" Inherits="Transport_VehicleExpense" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <fieldset><legend>Maintenance Expense </legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="fleft">
                    <asp:Button ID="btnNewWork" runat="server" Text="New Maintenance Work" OnClick="btnNewWork_Click" UseSubmitBehavior="false" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="imgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                </div>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
                </div>
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <asp:Panel ID="pnlPDF" runat="server" DefaultButton="btnPrintVoucher">
                <div class="fleft">
                    &nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtRefNo" runat="server" Width="100px" placeholder="Type Ref No"></asp:TextBox>
                    <asp:Button ID="btnPrintVoucher" runat="server" Text="Print ED Voucher" AssociatedControlID="txtRefNo" 
                        OnClick="btnPrintVoucher_Click" UseSubmitBehavior="false" />
                 </div>
                 </asp:Panel>
                <div>
                <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="MaintanceId,ExpenseId" 
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="20" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="false" DataSourceID="SqlDataSourceExp" OnRowCommand="gvMaintenance_RowCommand"
                    OnRowDataBound="gvMaintenance_RowDataBound" OnRowEditing="gvMaintenance_RowEditing" OnRowUpdating="gvMaintenance_RowUpdating" OnPreRender="gvMaintenance_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" Text="Edit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Expense Detail" runat="server"
                                Text="Save" ValidationGroup="Required"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Update" CausesValidation="false"
                                runat="server" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref No" SortExpression="RefNo">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkRefNo" Text='<%#Eval("RefNo") %>' CommandName="Select" CommandArgument='<%#Eval("MaintanceId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RefNo" HeaderText="Ref No" SortExpression="RefNo" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="WorkDate" HeaderText="Work Date" SortExpression="WorkDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                        <%--<asp:BoundField DataField="sType" HeaderText="Type" SortExpression="Type" ReadOnly="true" />--%>
                        <%--<asp:BoundField DataField="ApprovalStatus" HeaderText="Approved" SortExpression="ApprovalStatus" ReadOnly="true" />--%>
                        <%--<asp:BoundField DataField="ApprovedAmount" HeaderText="Approved Amount" SortExpression="ApprovedAmount" ReadOnly="true" />--%>
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="BillNumber" HeaderText="Bill No" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="SupportBillPaidTo" HeaderText="Support Bill To" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="PayType" HeaderText="Pay Type" ReadOnly="true" Visible="false" />
                        <asp:TemplateField HeaderText="Category" SortExpression="CategoryName">
                            <ItemTemplate>
                                <asp:Label ID="lblCategory" Text='<%#Eval("CategoryName") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddCategory" runat="server" SelectedValue='<%#BIND("CategoryID") %>'>
                                <asp:ListItem Text="Battery" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Bike Petrol Expenses" Value="29"></asp:ListItem>
                                <asp:ListItem Text="Body Repairing" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Courier Expenses" Value="25"></asp:ListItem>
                                <asp:ListItem Text="Electricals" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Engine" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Gear & Clutch" Value="5"></asp:ListItem>
                                <asp:ListItem Text="General" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Labour Charges" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Mechanical" Value="8"></asp:ListItem>
                                <asp:ListItem Text="RTO Annual Passing" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Service" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Spare Parts" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Tyres" Value="12"></asp:ListItem>
                                <asp:ListItem Text="Patta Repairing" Value="14"></asp:ListItem>
                                <asp:ListItem Text="Fuel Injection Pump" Value="15"></asp:ListItem>
                                <asp:ListItem Text="Glass Repairing" Value="16"></asp:ListItem>
                                <asp:ListItem Text="Rope (Rassi)" Value="17"></asp:ListItem>
                                <asp:ListItem Text="Schedule Service" Value="18"></asp:ListItem>
                                <asp:ListItem Text="Tarpaulins" Value="19"></asp:ListItem>
                                <asp:ListItem Text="Washing" Value="20"></asp:ListItem>
                                <asp:ListItem Text="MPI/TPI" Value="21"></asp:ListItem>
                                <asp:ListItem Text="Barmer Misc" Value="22"></asp:ListItem>
                                <asp:ListItem Text="Brake System" Value="27"></asp:ListItem>
                                <asp:ListItem Text="Slings/Wire Rope/D-Shackles Test Certifications" Value="26"></asp:ListItem>
                                <asp:ListItem Text="Documents Renewals Expenses" Value="30"></asp:ListItem>
                                <asp:ListItem Text="Operator Certifications" Value="23"></asp:ListItem>
                                <asp:ListItem Text="Crew Medical Test" Value="24"></asp:ListItem>
                                <asp:ListItem Text="GPS Tracking Device" Value="31"></asp:ListItem>
                                <asp:ListItem Text="Safety Compliances" Value="28"></asp:ListItem>
                                <asp:ListItem Text="Xerox, Scanning & Prinout Charges" Value="32"></asp:ListItem>
                                <asp:ListItem Text="Diesel" Value="33"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="13"></asp:ListItem>
                                <asp:ListItem Text="PUC Charges" Value="220"></asp:ListItem>
                                <asp:ListItem Text="Border Tax Charges" Value="221"></asp:ListItem>
                                <asp:ListItem Text="Petrol/CNG Charges" Value="222"></asp:ListItem>
                                <asp:ListItem Text="Toll/Fas Tag Charges" Value="223"></asp:ListItem>
                                <asp:ListItem Text="Labour Charges" Value="224"></asp:ListItem>
                                <asp:ListItem Text="Permit Charges" Value="225"></asp:ListItem>
                                <asp:ListItem Text="AC Service Charges" Value="226"></asp:ListItem>
                                <asp:ListItem Text="Insurance Charges" Value="227"></asp:ListItem>
                                <asp:ListItem Text="Conveyance For Repairs Charges" Value="228"></asp:ListItem>
                                <asp:ListItem Text="Police/RTO Fine Charges" Value="229"></asp:ListItem>
                                <asp:ListItem Text="Conveyance Expenses" Value="230"></asp:ListItem>
                                <asp:ListItem Text="Staff Welfare Expense" Value="231"></asp:ListItem>
                                <asp:ListItem Text="Food Expense" Value="232"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" Text='<%#BIND("Amount") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAmount" Text='<%#BIND("Amount") %>' runat="server" Width="100px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ExpenseDesc" HeaderText="Description" ReadOnly="true" Visible="false" />
                        <asp:TemplateField HeaderText="Description" SortExpression="ExpenseDesc">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 310px; white-space:normal;">
                                <asp:Label ID="lblExpenseDesc" Text='<%#BIND("ExpenseDesc") %>' runat="server" ></asp:Label>
                                </div>
                                <%--<asp:LinkButton ID="lnkMore" runat="server" Text="...More" ToolTip='<%#BIND("ExpenseDesc") %>'></asp:LinkButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="RfvDesc" runat="server" Text="*" ControlToValidate="txtExpenseDisc" SetFocusOnError="true"
                                InitialValue="" ErrorMessage="Please Enter Description." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtExpenseDisc" runat="server" Text='<%#BIND("ExpenseDesc") %>' Columns="2"  Width="300px" TextMode="MultiLine"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill No" SortExpression="BillNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblBillNo" Text='<%#BIND("BillNumber") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtBillNo" Text='<%#BIND("BillNumber") %>' runat="server" Width="100px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paid To" SortExpression="PaidTo">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidTo" Text='<%#BIND("PaidTo") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPaidTo" Text='<%#BIND("PaidTo") %>' runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Support Bill Paid To" SortExpression="SupportBillPaidTo">
                            <ItemTemplate>
                                <asp:Label ID="lblSupportBillPaidTo" Text='<%#BIND("SupportBillPaidTo") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSupportBillPaidTo" Text='<%#BIND("SupportBillPaidTo") %>' runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pay Type" SortExpression="PayType">
                            <ItemTemplate>
                                <asp:Label ID="lblPayType" Text='<%#BIND("PayType") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="RfvPayType" runat="server" Text="*" ControlToValidate="ddPayType" SetFocusOnError="true"
                                    InitialValue="0" ErrorMessage="Please Select Pay Type." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:DropDownList ID="ddPayType" runat="server" SelectedValue='<%#BIND("PayTypeID") %>' Width="100px">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Cash" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Cheque" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="NEFT" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="RTGS" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                         </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceExp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetWorkExpense" SelectCommandType="StoredProcedure" UpdateCommand="TR_updWorkExpense"
                        UpdateCommandType="StoredProcedure" OnUpdated="SqlDataSourceExp_Updated">
                        <UpdateParameters>
                            <asp:Parameter Name="MaintanceId" Type="Int32" />
                            <asp:Parameter Name="ExpenseId"  Type="Int32" />
                            <asp:Parameter Name="Amount" Type="String" />
                            <asp:Parameter Name="ExpenseDesc" Type="String" />
                            <asp:Parameter Name="CategoryId" Type="Int32" />
                            <asp:Parameter Name="BillNumber" Type="String" />
                            <asp:Parameter Name="PaidTo" Type="String" />
                            <asp:Parameter Name="SupportBillPaidTo" Type="String" />
                            <asp:Parameter Name="PayTypeId" Type="Int32" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
<%@ Page Title="Vessel Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VesselExpense.aspx.cs" 
        Inherits="Transport_VesselExpense" EnableEventValidation="false" Culture="en-GB" %>

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
    <fieldset><legend>Vessel Expense </legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="fleft">
                    <asp:Button ID="btnNewWork" runat="server" Text="New Vessel Expense" OnClick="btnNewWork_Click" UseSubmitBehavior="false" />
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
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vessel No" SortExpression="VehicleNo" ReadOnly="true" />
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
                                <asp:ListItem Text="ANALYSIS" Value="201"></asp:ListItem>
                                <asp:ListItem Text="BUNKER ( HF HSD )" Value="202"></asp:ListItem>
                                <asp:ListItem Text="CREW WAGES & MANAGEMENT" Value="203"></asp:ListItem>
                                <asp:ListItem Text="FRESH WATER" Value="204"></asp:ListItem>
                                <asp:ListItem Text="INDIAN REGISTER OF SHIPPING" Value="205"></asp:ListItem>
                                <asp:ListItem Text="INSURANCE" Value="206"></asp:ListItem>
                                <asp:ListItem Text="LAUNCH HIRE" Value="207"></asp:ListItem>
                                <asp:ListItem Text="MEDICINE" Value="208"></asp:ListItem>
                                <asp:ListItem Text="NAVIGATION" Value="209"></asp:ListItem>
                                <asp:ListItem Text="OIL & GREASE" Value="210"></asp:ListItem>
                                <asp:ListItem Text="PPE" Value="211"></asp:ListItem>
                                <asp:ListItem Text="PROVISITON" Value="212"></asp:ListItem>
                                <asp:ListItem Text="RENTAL EQUIPMENT" Value="213"></asp:ListItem>
                                <asp:ListItem Text="REPAIR & MAINTANCE" Value="214"></asp:ListItem>
                                <asp:ListItem Text="SALOON ITEMS" Value="215"></asp:ListItem>
                                <asp:ListItem Text="STATIONERY ITEMS" Value="216"></asp:ListItem>
                                <asp:ListItem Text="STATUTORY FEES" Value="217"></asp:ListItem>
                                <asp:ListItem Text="GENERAL" Value="218"></asp:ListItem>
                                <asp:ListItem Text="LSA/FFA" Value="219"></asp:ListItem>
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
                        SelectCommand="TR_GetWorkExpenseVessel" SelectCommandType="StoredProcedure" UpdateCommand="TR_updWorkExpense"
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
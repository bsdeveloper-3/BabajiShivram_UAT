<%@ Page Title="Bank Cheque" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BankChequeBook.aspx.cs" Inherits="AccountExpense_BankChequeBook" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:ScriptManager>
       <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div> 
    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <fieldset>
                    <legend>Cheque Book Entry</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Bank Name
                                <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ValidationGroup="Required" InitialValue="0"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddBabajiBankName"
                                    Text="*" ErrorMessage="Please Select Bank Name"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBankName" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddBabajiBankName_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                Account No
                                    <asp:RequiredFieldValidator ID="RFVAcountNo" runat="server" ValidationGroup="Required" InitialValue="0"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddBabajiBankAccount"
                                    Text="*" ErrorMessage="Please Select Bank Account"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBankAccount" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Start Cheque No
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartChequNo" runat="server" MaxLength="6"></asp:TextBox>
                            </td>
                            <td>
                                End Cheque No
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndChequNo" runat="server" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Branch
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBranch" runat="server" AutoPostBack="true">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnSaveCheque" Text="Add Cheque Detail" runat="server" OnClick="btnSaveCheque_Click" ValidationGroup="Required" />
                            </td>
                        </tr>
                    </table>
                                                          
                </fieldset>
                
                <fieldset>
                    <legend>Cheque Detail </legend>
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    <asp:GridView ID="gvChequeDetail" runat="server" AutoGenerateColumns="False"
                        CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                        DataKeyNames="lId" DataSourceID="DataSourceChequeDetail" CellPadding="4"
                        AllowPaging="True" AllowSorting="True" PageSize="40">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Account Name" DataField="BankAccountName" />
                            <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" />
                            <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChequeDate"/>
                            <asp:BoundField HeaderText="Cheque Amount" DataField="ChequeAmount" SortExpression="ChequeAmount" />
                            <asp:BoundField HeaderText="Job No" DataField="JobNo" SortExpression="JobNo"/>
                            <asp:BoundField HeaderText="User" DataField="UserName" />
                            <asp:BoundField HeaderText="Date" DataField="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                    </asp:GridView>
                </fieldset>
                <div>
                <div>
                    <asp:SqlDataSource ID="DataSourceChequeDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="AC_GetBankChequeByAccount" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddBabajiBankName" Name="BankID" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddBabajiBankAccount" Name="BankAccountId" PropertyName="SelectedValue" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddBranch" Name="BranchID" PropertyName="SelectedValue" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>

                    </asp:SqlDataSource>
                    
                 </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


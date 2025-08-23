<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpenseRuleAir.aspx.cs" 
    Inherits="ExpenseRuleAir" Culture="en-GB" Title="AIR Expense Rule Setup" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <fieldset>
        <legend>Air - Expense Rule </legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                
                <div class="clear">
                </div>
                <asp:GridView ID="gvAirRule" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="lid" 
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="20" AllowPaging="True"
                    AllowSorting="True" PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="false" DataSourceID="SqlDataSourceAirRule" 
                    OnPreRender="gvAirRule_PreRender" OnRowEditing="gvAirRule_RowEditing" OnRowUpdating="gvAirRule_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" SortExpression="sName">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Text='<%#Eval("sName") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVName" runat="server" Display="Dynamic" ControlToValidate="txtName"
                                    Text="*" ValidationGroup="Required" ErrorMessage="Please Enter Rule Name." SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="sDescription">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Text='<%# Eval("Description")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVDesc" runat="server" Display="Dynamic" SetFocusOnError="true"
                                    ControlToValidate="txtDescription" Text="*" ValidationGroup="Required"
                                    ErrorMessage="Please Enter Description."> </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Charges") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Charges") %>' MaxLength="8" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVAmount" runat="server" Display="Dynamic" ControlToValidate="txtAmount"
                                    Text="*" ValidationGroup="Required" ErrorMessage="Please Enter Amount" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValAmount" runat="server" ControlToValidate="txtAmount"
                                    Text="Invalid Amount!" Operator="DataTypeCheck" Type="Double" ValidationGroup="Required"
                                    Display="Dynamic" ErrorMessage="Invalid Amount." SetFocusOnError="true"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Max Charges" SortExpression="MaxValue">
                            <ItemTemplate>
                                <asp:Label ID="lblMaxValue" runat="server" Text='<%#Eval("MaxPayable") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMaxValue" runat="server" Text='<%#Eval("MaxPayable") %>' MaxLength="8" Width="100px" ></asp:TextBox>
                                <asp:CompareValidator ID="ComValMax" runat="server" ControlToValidate="txtMaxValue"
                                    Text="Invalid Max Charges!" Operator="DataTypeCheck" Type="Double" ValidationGroup="Required"
                                    Display="Dynamic" ErrorMessage="Invalid Max Charges." SetFocusOnError="true"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceAirRule" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetExpenseRuleAir" SelectCommandType="StoredProcedure" UpdateCommand="updExpenseRuleAir"
                        OnUpdated="SqlDataSourceRule_Updated" UpdateCommandType="StoredProcedure">
                        <UpdateParameters>
                            <asp:Parameter Name="lId" Type="int32" />
                            <asp:Parameter Name="sName" Type="string" />
                            <asp:Parameter Name="Description" Type="string" />
                            <asp:Parameter Name="Charges" Type="string" />
                            <asp:Parameter Name="MaxPayable" Type="string" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:Parameter Name="Output" Type="int32" Direction="Output" Size="4" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpenseRuleSea.aspx.cs" 
    Inherits="Master_ExpenseRuleSea" Culture="en-GB" Title="SEA Expense Rule Setup" EnableEventValidation="false" %>

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
        <legend>Sea Expense Rule </legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                
                <div class="clear">
                </div>
                <asp:GridView ID="gvSeaRule" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="lid" 
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="40" AllowPaging="True"
                    AllowSorting="True" PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="false" DataSourceID="SqlDataSourceRule" 
                    OnPreRender="gvSeaRule_PreRender" OnRowEditing="gvSeaRule_RowEditing" OnRowUpdating="gvSeaRule_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sName" HeaderText="Name" SortExpression="sName" ReadOnly="true" />
                        <asp:BoundField DataField="ReportHeading" HeaderText="Heading" SortExpression="ReportHeading" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Con20" SortExpression="Con20">
                            <ItemTemplate>
                                <asp:Label ID="lblCon20" runat="server" Text='<%#Eval("Con20") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCon20" runat="server" Text='<%#Eval("Con20") %>' MaxLength="8" Width="70px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVCon20" runat="server" Display="Dynamic" ControlToValidate="txtCon20"
                                    Text="*" ValidationGroup="Required" ErrorMessage="Please Enter Con20 Amount" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValCon20" runat="server" ControlToValidate="txtCon20"
                                    Text="Invalid Amount!" Operator="DataTypeCheck" Type="Integer" ValidationGroup="Required"
                                    Display="Dynamic" ErrorMessage="Invalid Amount." SetFocusOnError="true"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Con40" SortExpression="Con40">
                            <ItemTemplate>
                                <asp:Label ID="lblCon40" runat="server" Text='<%#Eval("Con40") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCon40" runat="server" Text='<%#Eval("Con40") %>' MaxLength="8" Width="70px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVCon40" runat="server" Display="Dynamic" ControlToValidate="txtCon40"
                                    Text="*" ValidationGroup="Required" ErrorMessage="Please Enter Con40 Amount" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValCon40" runat="server" ControlToValidate="txtCon40"
                                    Text="Invalid Amount!" Operator="DataTypeCheck" Type="Integer" ValidationGroup="Required"
                                    Display="Dynamic" ErrorMessage="Invalid Amount." SetFocusOnError="true"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LCL" SortExpression="LCL">
                            <ItemTemplate>
                                <asp:Label ID="lblLCL" runat="server" Text='<%#Eval("LCL") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLCL" runat="server" Text='<%#Eval("LCL") %>' MaxLength="8" Width="70px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVLCL" runat="server" Display="Dynamic" ControlToValidate="txtLCL"
                                    Text="*" ValidationGroup="Required" ErrorMessage="Please Enter LCL Amount" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValLCL" runat="server" ControlToValidate="txtLCL"
                                    Text="Invalid Amount!" Operator="DataTypeCheck" Type="Integer" ValidationGroup="Required"
                                    Display="Dynamic" ErrorMessage="Invalid Amount." SetFocusOnError="true"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName" ReadOnly="true"/>
                        <asp:BoundField DataField="StandardRequired" HeaderText="Is Standard Required" SortExpression="StandardRequired" ReadOnly="true"/>
                        <asp:BoundField DataField="IsRMS" HeaderText="RMS" SortExpression="IsRMS" ReadOnly="true"/>
                        <asp:BoundField DataField="IsNonRMS" HeaderText="NonRMS" SortExpression="IsNonRMS" ReadOnly="true"/>
                        <asp:BoundField DataField="IsPayParCont" HeaderText="Pay Par Cont" SortExpression="IsPayParCont" ReadOnly="true"/>
                        <asp:BoundField DataField="LabourTypeName" HeaderText="Labour Type" SortExpression="LabourTypeName" ReadOnly="true"/>
                        <asp:BoundField DataField="DeliveryTypeName" HeaderText="Delivery Type" SortExpression="DeliveryTypeName" ReadOnly="true"/>

                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceRule" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetExpenseRuleSea" SelectCommandType="StoredProcedure" UpdateCommand="updExpenseRuleSea"
                        OnUpdated="SqlDataSourceRule_Updated" UpdateCommandType="StoredProcedure">
                        <UpdateParameters>
                            <asp:Parameter Name="lId" Type="int32" />
                            <asp:Parameter Name="Con20" Type="string" />
                            <asp:Parameter Name="Con40" Type="string" />
                            <asp:Parameter Name="LCL" Type="string" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:Parameter Name="Output" Type="int32" Direction="Output" Size="4" />
                        </UpdateParameters>
                        
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>



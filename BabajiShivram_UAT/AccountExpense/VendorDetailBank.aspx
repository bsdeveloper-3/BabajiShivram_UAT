<%@ Page Title="Vendor Bank Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="VendorDetailBank.aspx.cs" Inherits="AccountExpense_VendorDetailBank" Culture="en-GB"%>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
     </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" />
    <asp:HiddenField ID="hdnCRM_UserId" runat="server" Value="0" />

    <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid"
        Width="100%" OnDataBound="FormView1_DataBound"
        OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated"
        OnItemCommand="FormView1_ItemCommand">

        <InsertItemTemplate>
            <fieldset>
                <legend>Add New Bank Vendor Account</legend>
                <div class="m clear">
                    <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                        Text="Save" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Bank Name
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBankName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Bank Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                       <%-- <asp:RegularExpressionValidator ID="RFVExpBankName" runat="server" ControlToValidate="txtBankName" ValidationExpression="^[a-zA-Z]+\.[a-zA-Z]{4,100}^"
                            ErrorMessage="Invalid Bank Name" Text="Invalid Bannk Name - Special characters not allowed" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankName" runat="server" Text='<%# Bind("BankNAME") %>' MaxLength="100" ></asp:TextBox>
                        </td>
                        <td>Payment For 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddAccountType" DataSourceID="DataSourceExpense" AppendDataBoundItems="true"
                                DataTextField="sName" DataValueField="lid" runat="server" Width="185"
                                SelectedValue='<%# Bind("AccountType") %>'>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Account Name
                        <asp:RequiredFieldValidator ID="RFV04" runat="server" ControlToValidate="txtAccountName" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Account Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="RFVExpAccountName" runat="server" ControlToValidate="txtAccountName" ValidationExpression="^[a-zA-Z]+\.[a-zA-Z]{4,100}^"
                            Text="*" ErrorMessage="Invalid Account Name - Special characters not allowed" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
                        
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountName" runat="server" Text='<%# Bind("AccountName") %>' MaxLength="80" />
                        </td>
                        <td>Account No
                        <asp:RequiredFieldValidator ID="RFV05" runat="server" ControlToValidate="txtAccountNo" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Account No" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="RFVExpAccountNo" runat="server" ControlToValidate="txtAccountName" ValidationExpression="^[a-zA-Z0-9- / -]*$"
                            Text="Invalid Account No" ErrorMessage="9 digits to 20 digits Only" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountNo" runat="server" Text='<%# Bind("AccountNo") %>' MaxLength="25" />
                        </td>
                    </tr>
                    <tr>
                        <td>IFSC Code
                        <asp:RequiredFieldValidator ID="rfvIFSCCode" runat="server" ControlToValidate="txtIFSCCode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter IFSC Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RFVExpIFSCCode" runat="server" ControlToValidate="txtIFSCCode" ValidationExpression="^[A-Z]{4}0[A-Z0-9]{6}$"
                            ErrorMessage="Invalid IFSC Code " Text="Invalid IFSC Code" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                        
                        </td>
                        <td>
                            <asp:TextBox ID="txtIFSCCode" runat="server" Text='<%# Bind("IFSCCode") %>' MaxLength="11" />
                        </td>
                        <td>Remark
                        <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Remark" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%# Bind("Remark") %>' TextMode="MultiLine" MaxLength="200" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
        <EditItemTemplate>
            <fieldset>
                <legend>Update Bank Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" 
                        Text="Update" ValidationGroup="Required" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Bank Name
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBankName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Bank Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankName" runat="server" Text='<%# Bind("BankNAME") %>' ></asp:TextBox>
                        </td>
                        <td>Payment For 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddAccountType" DataSourceID="DataSourceExpense" AppendDataBoundItems="true"
                                DataTextField="sName" DataValueField="lid" runat="server" Width="185"
                                SelectedValue='<%# Bind("AccountType") %>' >
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Account Name
                        <asp:RequiredFieldValidator ID="RFV04" runat="server" ControlToValidate="txtAccountName" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Account Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountName" runat="server" Text='<%# Bind("AccountName") %>' MaxLength="80" />
                        </td>
                        <td>Account No
                        <asp:RequiredFieldValidator ID="RFV05" runat="server" ControlToValidate="txtAccountNo" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Account No" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountNo" runat="server" Text='<%# Bind("AccountNo") %>' MaxLength="25" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>IFSC Code
                        <asp:RequiredFieldValidator ID="rfvIFSCCode" runat="server" ControlToValidate="txtIFSCCode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter IFSC Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RFVExpIFSCCode1" runat="server" ControlToValidate="txtIFSCCode" ValidationExpression="^[A-Z]{4}0[A-Z0-9]{6}$"
                            ErrorMessage="Invalid IFSC Code " Text="Invalid IFSC Code" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIFSCCode" runat="server" Text='<%# Bind("IFSCCode") %>' MaxLength="11" />
                        </td>
                        <td>Remark
                        <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Account No" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%# Bind("Remark") %>' />
                        </td>
                    </tr>
                </table>
                <div class="m clear">
                </div>
                
            </fieldset>
        </EditItemTemplate>
        <ItemTemplate>
            <fieldset>
                <legend>Bank Account Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                    &nbsp;&nbsp;<asp:Button ID="btnNew2" runat="server" Text="Add New Account" CommandName="New" CausesValidation="false" />
                    &nbsp;&nbsp;<asp:Button ID="btnBack2" runat="server" Text="Back" OnClick="btnBack_Click" CausesValidation="false" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Bank Name
                        </td>
                        <td>
                            <asp:Label ID="lblBankName" runat="server" Text='<%# Bind("BankNAME") %>' ></asp:Label>
                        </td>
                        <td>Payment For 
                          </td>
                        <td>
                            <asp:Label ID="lblAccountTypeName" runat="server" Text='<%# Bind("AccountTypeName") %>' ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Account Name
                        </td>
                        <td>
                            <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("AccountName") %>' />
                        </td>
                        <td>Account No
                        </td>
                        <td>
                            <asp:Label ID="lblAccountNo" runat="server" Text='<%# Bind("AccountNo") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>IFSC Code
                        </td>
                        <td>
                            <asp:Label ID="lblIFSCCode" runat="server" Text='<%# Bind("IFSCCode") %>' />
                        </td>
                        <td>
                            Remark
                        </td>
                        <td>
                            <asp:TextBox ID="lblRemark" runat="server" Text='<%# Bind("Remark") %>' />
                        </td>
                    </tr>
                </table>
            </fieldset>
            
        </ItemTemplate>
        <EmptyDataTemplate>
            &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="Add New Account" CommandName="New" CausesValidation="false" />
            &nbsp;&nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CausesValidation="false" />
        </EmptyDataTemplate>
    </asp:FormView>

    <fieldset>
        <legend>Bank Account Detail</legend>
        <!-- Filter Content Start-->
        <div class="m clear">
            <div class="fleft">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>&nbsp;&nbsp;
            </div>
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
        </div>
        <div class="clear">
        </div>
        <!-- Filter Content END-->
        <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="False" DataSourceID="GridViewSqlDataSource"
            AllowSorting="true" AllowPaging="True" CssClass="table" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="lId" PageSize="20" PagerStyle-CssClass="pgr" OnSelectedIndexChanged="gvUser_SelectedIndexChanged"
            OnPreRender="gvUser_PreRender" PagerSettings-Position="TopAndBottom">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bank Name" SortExpression="BankName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBankName" runat="Server" Text='<%#Eval("BankName") %>' CommandName="select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BankName" HeaderText="Bank Name" Visible="false" />
                <asp:BoundField DataField="AccountTypeName" HeaderText="Type" SortExpression="AccountTypeName" />
                <asp:BoundField DataField="AccountName" HeaderText="Account Name" SortExpression="AccountName" />
                <asp:BoundField DataField="AccountNo" HeaderText="AccountNo" SortExpression="AccountNo" />
                <asp:BoundField DataField="IFSCCode" HeaderText="IFSCCode" SortExpression="IFSCCode" />
                <%--<asp:BoundField DataField="DivisionName" HeaderText="BS Group" SortExpression="DivisionName" />--%>
                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" />
                <asp:BoundField DataField="dtDate" HeaderText="Date" SortExpression="dtDate" />
                <asp:BoundField DataField="updUserName" HeaderText="Updated By" SortExpression="updUserName" />
                <asp:BoundField DataField="updDate" HeaderText="Update Date" SortExpression="updDate" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="BJV_GetVendorbankDetailById" SelectCommandType="StoredProcedure"
            InsertCommand="BJV_insVendorBankDetail" InsertCommandType="StoredProcedure" 
            UpdateCommand="BJV_updVendorBankDetail" UpdateCommandType="StoredProcedure" 
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnInserting="FormViewDataSource_Inserting" OnUpdating="FormViewDataSource_Updating">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvUser" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="VendorId" SessionField="VendorId" />
                <asp:ControlParameter ControlID="ctl00$ContentPlaceHolder1$FormView1$ddAccountType" Name="AccountType"
                    PropertyName="SelectedValue" Type="int32" />
                <asp:Parameter Name="BankName" Type="string" />
                <asp:Parameter Name="AccountName" Type="string" />
                <asp:Parameter Name="AccountNo" Type="string" />
                <asp:Parameter Name="IFSCCode" Type="string" />
                <asp:Parameter Name="Remark" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="VendorId" SessionField="VendorId" />
                <asp:Parameter Name="lId" />
                <asp:ControlParameter ControlID="ctl00$ContentPlaceHolder1$FormView1$ddAccountType" Name="AccountType"
                    PropertyName="SelectedValue" Type="int32" />
                <asp:Parameter Name="BankName" Type="string" />
                <asp:Parameter Name="AccountName" Type="string" />
                <asp:Parameter Name="AccountNo" Type="string" />
                <asp:Parameter Name="IFSCCode" Type="string" />
                <asp:Parameter Name="Remark" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />

            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="BJV_GetVendorbankDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="VendorId" SessionField="VendorID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    </div>

</asp:Content>


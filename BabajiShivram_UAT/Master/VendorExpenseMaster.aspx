<%@ Page Title="Vendor Expense Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VendorExpenseMaster.aspx.cs" 
    Inherits="Master_VendorExpenseMaster" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%--    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />--%>
     <div align="center">
        <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" />

    <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid"
        Width="100%" OnDataBound="FormView1_DataBound"
        OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated"
        OnItemDeleted="FormView1_ItemDeleted" OnItemCommand="FormView1_ItemCommand">

        <InsertItemTemplate>
            <fieldset>
                <legend>Add Expense Type</legend>
                <div class="m clear">
                    <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" 
                        Text="Save" TabIndex="11" OnClick="btnSubmit_Click" ValidationGroup="Required"/> <%----%>
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" TabIndex="12" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Expense Name
                        <asp:RequiredFieldValidator ID="RFVExpenseName" runat="server" ControlToValidate="txtExpenseName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Expense Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExpenseName" runat="server" Text='<%# Bind("sName") %>' TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Charge Code
                        <asp:RequiredFieldValidator ID="RFVChargecode" runat="server" ControlToValidate="txtChargecode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Charge Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChargecode" runat="server" TabIndex="4" Text='<%# Bind("ChargeCode") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>Charge Name</td>
                        <td>
                          <asp:TextBox ID="txtChargeName" runat="server" TabIndex="4" Text='<%# Bind("ChargeName") %>' />
                        </td>
                        <td>HSN Code</td>
                        <td>
                           <asp:TextBox ID="txtHSNCode" runat="server" TabIndex="4" Text='<%# Bind("ChargeHSN") %>' />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
        <EditItemTemplate>
            <fieldset>
                <legend>Update Expense Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" TabIndex="11"
                        Text="Update" OnClick="btnUpdate_Click" ValidationGroup="Required"/> <%----%>
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        TabIndex="12" Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                     <tr>
                        <td>Expense Name
                        <asp:RequiredFieldValidator ID="RFVExpenseName" runat="server" ControlToValidate="txtExpenseName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Expense Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExpenseName" runat="server" Text='<%# Bind("sName") %>' TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Charge Code
                        <asp:RequiredFieldValidator ID="RFVChargecode" runat="server" ControlToValidate="txtChargecode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Charge Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChargecode" runat="server" TabIndex="4" Text='<%# Bind("ChargeCode") %>' />
                        </td>
                    </tr>                  
                    <tr>
                        <td>Charge Name</td>
                        <td>
                          <asp:TextBox ID="txtChargeName" runat="server" TabIndex="4" Text='<%# Bind("ChargeName") %>' />
                        </td>
                        <td>Charge HSN </td>
                         <td>
                            <asp:TextBox ID="txtHSNCode" runat="server" TabIndex="4" Text='<%# Bind("ChargeHSN") %>' />
                         </td>
                    </tr>
                </table>
                <div class="m clear">
                </div>
            </fieldset>
        </EditItemTemplate>
        <ItemTemplate>
            <fieldset>
                <legend>Expense Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this record?');" Text="Delete" />
                    <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Expense Name
                        </td>
                        <td>
                            <asp:Label ID="lblExpenseName" runat="server" Text='<%# Bind("sName") %>'></asp:Label>
                        </td>
                        <td>Charge Code</td>
                        <td>
                            <asp:Label ID="lblChargeCode" runat="server" Text='<%# Bind("ChargeCode") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>Charge Name
                        </td>
                        <td>
                            <asp:Label ID="lblChargeName" runat="server" Text='<%# Bind("ChargeName") %>'></asp:Label>
                        </td>
                        <td>Charge HSN 
                        </td>
                        <td>
                            <asp:Label ID="lblHSNCode" runat="server" Text='<%# Bind("ChargeHSN") %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnUserId" runat="server" Value='<%# Bind("lId") %>' Visible="true" />
            </fieldset>
                  </ItemTemplate>
        <EmptyDataTemplate>
            &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="Add New Expense Type" CommandName="New" />
        </EmptyDataTemplate>
    </asp:FormView>

    <fieldset id="fsMainBorder" runat="server">
        <legend>Expense Type Detail</legend>
        <!-- Filter Content Start-->
        <div class="m clear">
            <div class="fleft">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>&nbsp;&nbsp;
            </div>
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
        </div>
        <div class="clear">
        </div>
        <!-- Filter Content END-->
        <asp:GridView ID="gvExpense" runat="server" AutoGenerateColumns="False" DataSourceID="GridViewSqlDataSource"
            AllowSorting="true" AllowPaging="True" CssClass="table" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="lId" PageSize="20" PagerStyle-CssClass="pgr" OnSelectedIndexChanged="gvExpesne_SelectedIndexChanged"
            OnPreRender="gvExpesne_PreRender" PagerSettings-Position="TopAndBottom" OnRowCommand="gvExpense_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expense Name" SortExpression="sName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkexpesnename" runat="Server" Text='<%#Eval("sName") %>' CommandArgument='<%# Eval("lId") %>' CommandName="select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="sName" HeaderText="Expense Name" Visible="false" />
                <asp:BoundField DataField="ChargeCode" HeaderText="Charge Code" SortExpression="ChargeCode" />
                <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" SortExpression="ChargeName" />
                <asp:BoundField DataField="ChargeHSN" HeaderText="Charge HSN" SortExpression="ChargeHSN" />
                <asp:BoundField DataField="dtDate" HeaderText="Created Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}"/>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="AC_GetRequestExpenseType" SelectCommandType="StoredProcedure"
            InsertCommand="AC_insExpenseType" InsertCommandType="StoredProcedure" UpdateCommand="AC_updExpenseType"
            UpdateCommandType="StoredProcedure" DeleteCommand="AC_DeleteExpenseDetail" DeleteCommandType="StoredProcedure"
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvExpense" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
           <%-- <InsertParameters>
                <asp:Parameter Name="sName" Type="string" />
                <asp:Parameter Name="ChargeCode" Type="string" />
                <asp:Parameter Name="ChargeName" Type="String" />
                 <asp:Parameter Name="ChargeHSNCode" Type="String" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="lId" />
               <asp:Parameter Name="sName" Type="string" />
                <asp:Parameter Name="ChargeCode" Type="string" />
                <asp:Parameter Name="ChargeName" Type="String" />
                <asp:Parameter Name="ChargeHSNCode" Type="String" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lId" />--%>
            <%--    <%--<asp:SessionParameter Name="lUser" SessionField="UserId" />--%>
              <%--  <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </DeleteParameters>--%>
        </asp:SqlDataSource>
              <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="AC_GetRequestExpenseType" SelectCommandType="StoredProcedure">
             <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

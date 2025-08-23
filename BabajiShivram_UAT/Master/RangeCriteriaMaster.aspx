<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RangeCriteriaMaster.aspx.cs" Inherits="Master_RangeCriteriaMaster" 
    Title="Range Criteria Master Setup" EnableEventValidation="false" Culture="en-GB" %>

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
    <%--<asp:HiddenField ID="hdnDiv_UserId" runat="server" Value="0" />--%>

    <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid"
        Width="100%" OnDataBound="FormView1_DataBound"
        OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated"
        OnItemDeleted="FormView1_ItemDeleted" OnItemCommand="FormView1_ItemCommand">

        <InsertItemTemplate>
            <fieldset>
                <legend>Add New RangeCriteria</legend>
                <div class="m clear">
                    <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                        Text="Save" TabIndex="11" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" TabIndex="12" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Range Criteria Master
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtRangeName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Range Criteria Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRangeName" runat="server" Text='<%# Bind("RangeCriteriaName") %>' TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Code
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRangeCode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Range Criteria Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRangeCode" runat="server" TabIndex="4" Text='<%# Bind("RCCode") %>' />
                        </td>
                    </tr>
                   
                  
                    <tr>
                         
                        <td>Remarks</td>
                        <td>
                          <asp:TextBox ID="txtRemarks" runat="server" TabIndex="4" Text='<%# Bind("Remarks") %>' />
                        </td>
                        <td></td>
                        <td></td>
                       
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
        <EditItemTemplate>
            <fieldset>
                <legend>Update Range Criteria Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" TabIndex="11"
                        Text="Update" ValidationGroup="Required" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        TabIndex="12" Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                     <tr>
                        <td>Range Criteria Master
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtRangeName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Range Criteria Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRangeName" runat="server" Text='<%# Bind("RangeCriteriaName") %>' TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Code
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRangeCode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Range Criteria Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRangeCode" runat="server" TabIndex="4" Text='<%# Bind("RCCode") %>' />
                        </td>
                    </tr>
                   
                  
                    <tr>
                         
                        <td>Remarks</td>
                        <td>
                          <asp:TextBox ID="txtRemarks" runat="server" TabIndex="4" Text='<%# Bind("Remarks") %>' />
                        </td>
                       <td>Is Active?
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblActive" runat="server" SelectedValue='<%#Bind("IsActive") %>'
                                RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                       
                    </tr>
                </table>
                <div class="m clear">
                </div>
                
            </fieldset>
        </EditItemTemplate>
        <ItemTemplate>
            <fieldset>
                <legend>Range Criteria Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');" Text="Delete" />
                    <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Range Criteria Name
                        </td>
                        <td>
                            <asp:Label ID="lblRcName" runat="server" Text='<%# Bind("RangeCriteriaName") %>'></asp:Label>
                        </td>
                        <td>Range Code</td>
                        <td>
                            <asp:Label ID="lblRccode" runat="server" Text='<%# Bind("RCCode") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>Remarks
                        </td>
                        <td>
                            <asp:Label ID="lblRcRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                        </td>
                         <td>Is Active?
                        </td>
                        <td>
                            <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%>
                        </td>
                       
                    </tr>
                   
                   
                    
                </table>
                <asp:HiddenField ID="hdnUserId" runat="server" Value='<%# Bind("lId") %>' Visible="true" />
            </fieldset>
          
          
        </ItemTemplate>
        <EmptyDataTemplate>
            &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="Add New Range Criteria" CommandName="New" />

        </EmptyDataTemplate>
    </asp:FormView>

    <fieldset id="fsMainBorder" runat="server">
        <legend>Range Criteria Detail</legend>
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
        <asp:GridView ID="gvRange" runat="server" AutoGenerateColumns="False" DataSourceID="GridViewSqlDataSource"
            AllowSorting="true" AllowPaging="True" CssClass="table" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="lId" PageSize="20" PagerStyle-CssClass="pgr" OnSelectedIndexChanged="gvRange_SelectedIndexChanged"
            OnPreRender="gvRange_PreRender" PagerSettings-Position="TopAndBottom">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Range Criteria Name" SortExpression="RangeCriteriaName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkempname" runat="Server" Text='<%#Eval("RangeCriteriaName") %>' CommandName="select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RangeCriteriaName" HeaderText="Range Criteria Name" Visible="false" />
                
                <asp:BoundField DataField="RCCode" HeaderText="RCCode" SortExpression="RCCode" />
              
               
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
             
                <asp:BoundField DataField="ActiveStatus" HeaderText="ActiveStatus" SortExpression="ActiveStatus" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" />
               
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="GetRangeCriteriaDetail" SelectCommandType="StoredProcedure"
            InsertCommand="insertRangeCriteria" InsertCommandType="StoredProcedure" UpdateCommand="UpdateRangeCriteriaDetail"
            UpdateCommandType="StoredProcedure" DeleteCommand="DeleteRangeDetail" DeleteCommandType="StoredProcedure"
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvRange" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="RangeCriteriaName" Type="string" />
                <asp:Parameter Name="RCCode" Type="string" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="lId" />
               <asp:Parameter Name="RangeCriteriaName" Type="string" />
                <asp:Parameter Name="RCCode" Type="string" />
                <asp:Parameter Name="Remarks" Type="String" />
                 <asp:ControlParameter ControlID="ctl00$ContentPlaceHolder1$FormView1$rblActive" Name="IsActive"
                    PropertyName="SelectedValue" Type="string" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lId" />
                <%--<asp:SessionParameter Name="lUser" SessionField="UserId" />--%>
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetRangeCriteriaDetail" SelectCommandType="StoredProcedure">
             <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>

        </asp:SqlDataSource>
       
    </div>

</asp:Content>


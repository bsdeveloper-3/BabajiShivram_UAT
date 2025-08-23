<%@ Page Title="Product Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ProductMaster.aspx.cs" Inherits="EWayBill_ProductMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
       <div class="clear"></div>     
    </div>
    <div>
        <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid"
            OnItemDeleted="FormView1_ItemDeleted" OnItemUpdated="FormView1_ItemUpdated" OnItemInserted="FormView1_ItemInserted"
            OnItemCommand="FormView1_ItemCommand" Width="100%" OnDataBound="FormView1_DataBound">
            <EditItemTemplate>
                <fieldset><legend>Update Product Detail</legend>
                <div class="m clear">
                <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" TabIndex="8"
                    Text="Update" ValidationGroup="Required" />
                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    TabIndex="9" Text="Cancel" />
                </div>    
                <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            Product Name
                            <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBranchNameED" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Enter Product Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchNameED" Text='<%# Bind("sName") %>' TabIndex="1" runat="server" />
                        </td>
                        <td>
                            Product Code
                            <asp:RequiredFieldValidator ID="RFV02" runat="server" ErrorMessage="Please Enter Product Code" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtBranchCodeED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchCodeED" Text='<%# Bind("sCode") %>' TabIndex="2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            HSN Code
                            <asp:RequiredFieldValidator ID="RFVinsCity" runat="server" ControlToValidate="txtHSN"
                                Display="Dynamic" ErrorMessage="Please Enter HSN" InitialValue="" Text="*"
                                ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHSN" Text='<%# Bind("HSNCode") %>' TabIndex="2" runat="server" />
                            
                        </td>
                        <td>
                            Description
                            
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" Text='<%# Bind("sDescription") %>' TabIndex="2" runat="server" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            IGST Rate
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddressED" Text='<%#Bind("IGSTRate") %>' runat="server" TabIndex="5"
                                />
                        </td>
                        <td>
                            CGST Rate
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailED" Text='<%#Bind("CGSTRate") %>' runat="server" TabIndex="6" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SGST Rate
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchPrefixED" Text='<%#Bind("SGSTRate") %>' runat="server" TabIndex="7" MaxLength="50" />
                        </td>
                        <td>
                         
                        </td>
                        <td>
                         
                        </td>
                    </tr>
                </table>
                </fieldset>
            </EditItemTemplate>
            <InsertItemTemplate>
                <fieldset><legend>Add Product Detail</legend>
                <div class="m clear">
                <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                    TabIndex="8" Text="Save" />
                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    TabIndex="9" Text="Cancel" />
                </div>
                <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            Product Name
                            <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBranchNameED" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Enter Product Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchNameED" Text='<%# Bind("sName") %>' TabIndex="1" runat="server" />
                        </td>
                        <td>
                            Product Code
                            <asp:RequiredFieldValidator ID="RFV02" runat="server" ErrorMessage="Please Enter Product Code" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtBranchCodeED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchCodeED" Text='<%# Bind("sCode") %>' TabIndex="2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            HSN Code
                            <asp:RequiredFieldValidator ID="RFVinsCity" runat="server" ControlToValidate="txtHSN"
                                Display="Dynamic" ErrorMessage="Please Enter HSN" InitialValue="" Text="*"
                                ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHSN" Text='<%# Bind("HSNCode") %>' TabIndex="2" runat="server" />
                            
                        </td>
                        <td>
                            Description
                            
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" Text='<%# Bind("sDescription") %>' TabIndex="2" runat="server" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            IGST Rate
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddressED" Text='<%# Bind("IGSTRate") %>' runat="server" TabIndex="5"
                                />
                        </td>
                        <td>
                            CGST Rate
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailED" Text='<%# Bind("CGSTRate") %>' runat="server" TabIndex="6" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SGST Rate
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchPrefixED" Text='<%# Bind("SGSTRate") %>' runat="server" TabIndex="7" MaxLength="50" />
                        </td>
                        <td>
                         
                        </td>
                        <td>
                         
                        </td>
                    </tr>
                </table>
                </fieldset>
            </InsertItemTemplate>
            <ItemTemplate>
            <fieldset><legend>Product Detail</legend>
                <div class="m clear">
                
                <asp:HiddenField ID="hdnBranchId" runat="server" Value='<%# Bind("lId") %>' />
                </div>
                <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            Product Name
                        </td>
                        <td>
                            <asp:Label ID="lblBranchName" Text='<%# Eval("sName") %>' runat="server" />
                        </td>
                        <td>
                            Product Code
                        </td>
                        <td>
                            <asp:Label ID="lblBranchCode" Text='<%# Eval("sCode") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            HSN
                        </td>
                        <td>
                            <asp:Label ID="txtCity" Text='<%# Eval("HSNCode") %>' runat="server" />
                        </td>
                        <td>
                            Description
                        </td>
                        <td>
                            <asp:Label ID="lblTelephone" Text='<%# Eval("sDescription") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            IGST 
                        </td>
                        <td>
                            <asp:Label ID="lblAddress" Text='<%# Eval("IGSTRate") %>' runat="server" />
                        </td>
                        <td>
                            CGST Rate
                        </td>
                        <td>
                            <asp:Label ID="lblBranchEmail" Text='<%# Eval("CGSTRate") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SGST Rate
                        </td>
                        <td>
                            <asp:Label ID="lblPrefix" Text='<%# Eval("SGSTRate") %>' runat="server" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                </fieldset>
                                       
                
            </ItemTemplate>
            <EmptyDataTemplate>
                &nbsp;&nbsp;<asp:Button ID="btnnew" runat="server" Text="New Product" CommandName="New" CssClass="buttonTest" />
            </EmptyDataTemplate>
        </asp:FormView>
        
    </div>
   <fieldset id="fsMainBorder" runat="server">
    <legend>Product Detail</legend>    
    <div>
        <asp:GridView ID="gvBranch" runat="server" Width="100%" AutoGenerateColumns="False"
             CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="lId" PageSize="20" AllowPaging="true"
             DataSourceID="GridviewSqlDataSource" AllowSorting="true" PagerSettings-Position="TopAndBottom" 
             OnSelectedIndexChanged="gvBranch_SelectedIndexChanged" OnPreRender="gvBranch_PreRender" >
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Name" SortExpression="sName">
                    <ItemTemplate>
                        <asp:LinkButton CausesValidation="false" ID="lnkBranchName" Text='<%#Eval("sName") %>'
                            runat="server" CommandName="Select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="lId" HeaderText="lId" InsertVisible="False" ReadOnly="True"
                    SortExpression="lId" Visible="False" />
                <asp:BoundField DataField="sCode" HeaderText="Code" SortExpression="sCode" />
                <asp:BoundField DataField="HSNCode" HeaderText="HSNCode" SortExpression="HSNCode" />
                <asp:BoundField DataField="sDescription" HeaderText="Description" SortExpression="sDescription" />
                <asp:BoundField DataField="IGSTRate" HeaderText="IGST" SortExpression="IGSTRate" />
                <asp:BoundField DataField="SGSTRate" HeaderText="SGST" SortExpression="SGSTRate" />
                <asp:BoundField DataField="CGSTRate" HeaderText="CGST" SortExpression="CGSTRate" />

                <%--<asp:TemplateField HeaderText="Add Port">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkportdetail" runat="server" CommandArgument='<%#Eval("lid") %>'
                                    CommandName="Navigate" CausesValidation="False">Add Port</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </div>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="TR_GetEWayProductById" SelectCommandType="StoredProcedure"
            InsertCommand="TR_insEWayProduct" InsertCommandType="StoredProcedure" UpdateCommand="TR_updEWayProduct"
            UpdateCommandType="StoredProcedure" 
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvBranch" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="sName" Type="string" />
                <asp:Parameter Name="sCode" Type="string" />
                <asp:Parameter Name="HSNCode" Type="int32" />
                <asp:Parameter Name="sDescription" Type="string" />
                <asp:Parameter Name="IGSTRate" Type="string" />
                <asp:Parameter Name="CGSTRate" Type="string" />
                <asp:Parameter Name="SGSTRate" Type="string" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="gvBranch" Name="lid" PropertyName="SelectedValue" />
                <asp:Parameter Name="sName" Type="string" />
                <asp:Parameter Name="sCode" Type="string" />
                <asp:Parameter Name="HSNCode" Type="int32" />
                <asp:Parameter Name="sDescription" Type="string" />
                <asp:Parameter Name="IGSTRate" Type="string" />
                <asp:Parameter Name="CGSTRate" Type="string" />
                <asp:Parameter Name="SGSTRate" Type="string" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </UpdateParameters>
            
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetEWayProduct" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    </div>
</asp:Content>



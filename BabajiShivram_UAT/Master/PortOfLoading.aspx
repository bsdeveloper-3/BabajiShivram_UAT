<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortOfLoading.aspx.cs" Inherits="Master_PortOfLoading" 
     MasterPageFile="~/MasterPage.master" Title="Port of Loading Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />

        <div align="center">
            <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" />
        </div>
        <div class="clear"></div>     
        <div>
            <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewSqlDataSource" 
                OnDataBound="FormView1_DataBound" OnItemInserted="FormView1_ItemInserted" OnItemDeleted="FormView1_ItemDeleted"
                OnItemUpdated="FormView1_ItemUpdated" OnItemCommand="FormView1_ItemCommand" Width="100%">
                <EditItemTemplate>
                    <fieldset><legend>Update Port Detail</legend>
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" 
                       Text="Update" ValidationGroup="Required" TabIndex="4"/>
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" TabIndex="5"/>
                    <div class="m"></div>             
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                               Loading Port Name
                                <asp:RequiredFieldValidator ID="RFVinsPortName" runat="server" ControlToValidate="PortNameTextBox" Text="*" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Loading Port Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>    
                            </td>
                            <td>
                                <asp:TextBox ID="PortNameTextBox" Text='<%# Bind("LoadingPortName") %>' MaxLength="100" TabIndex="1" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Port Code
                                <asp:RequiredFieldValidator ID="RFVinsPortCode" runat="server" ControlToValidate="PortCodeTextBox" Text="*"
                                    ErrorMessage="Please Enter Port Code" ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="PortCodeTextBox" Text='<%# Bind("PortCode") %>'
                                    MaxLength="100" TabIndex="2" runat="server"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                City
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityName" Text='<%# Bind("CityName") %>'
                                    MaxLength="100" TabIndex="3" runat="server"> </asp:TextBox>
                            </td>
                            <td>
                                Mode
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("TransMode") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </EditItemTemplate>
                <InsertItemTemplate>
                <fieldset><legend>Add New Port</legend>
                    <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                        Text="Save" TabIndex="5"/>
                    <asp:Button ID="btnInsertCancelButton" runat="server" CommandName="Cancel"
                        CausesValidation="false" Text="Cancel" TabIndex="6"/>
                    <div class="m"></div>          
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Loading Port Name
                             <asp:RequiredFieldValidator ID="RFVinsPortName" runat="server" ErrorMessage="Please Enter Loading Port Name"
                              SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="Required" ControlToValidate="PortNameTextBox"></asp:RequiredFieldValidator>   
                            </td>
                            <td>
                                <asp:TextBox ID="PortNameTextBox" Text='<%# Bind("LoadingPortName") %>'
                                    MaxLength="100" TabIndex="1" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Port Code
                                <asp:RequiredFieldValidator ID="RFVinsPortCode" runat="server" ErrorMessage="Please Enter Loading Port Code" Text="*"
                                    SetFocusOnError="true" ControlToValidate="PortCodeTextBox" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>    
                            </td>
                            <td>
                                <asp:TextBox ID="PortCodeTextBox" Text='<%# Bind("PortCode") %>'
                                    MaxLength="50" TabIndex="2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                City
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityName" Text='<%# Bind("CityName") %>'
                                    MaxLength="100" TabIndex="3" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Mode
                                <asp:RequiredFieldValidator ID="RFVinsMode" runat="server" ControlToValidate="ddmode"
                                    Display="Dynamic" ErrorMessage="Please Select Mode" InitialValue="0" Text="*"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>
                            <td>
                                <asp:DropDownList ID="ddmode" runat="server" SelectedValue='<%# Bind("lMode") %>' TabIndex="4">
                                    <asp:ListItem Value="0" Text="--Select--" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Air"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Sea"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </InsertItemTemplate>
                <ItemTemplate>
                    <fieldset><legend>Loading Port Detail</legend>
                    <asp:Button ID="btnEditButton" Text="Edit" CommandName="Edit" runat="server"/>
                    <asp:Button ID="btnDeleteButton" Text="Delete" OnClientClick="return confirm('Sure to delete Loading Port?');"
                        runat="server" CommandName="Delete" />
                    <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server" />
                    <div class="m"></div>     
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                               Loading Port Name</td>
                            <td>
                                <asp:Label ID="PortNameLabel" runat="server" Text='<%# Bind("LoadingPortName") %>'></asp:Label></td>
                            <td>
                                Port Code
                            </td>
                            <td>
                                <asp:Label ID="PortCodeLabel" runat="server" Text='<%# Bind("PortCode") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                City
                            </td>
                            <td>
                                <asp:Label ID="CityLabel" runat="server" Text='<%# Eval("CityName") %>'></asp:Label></td>
                            <td>
                                Mode</td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("TransMode") %>'></asp:Label></td>
                        </tr>
                    </table>
                   </fieldset>
                </ItemTemplate>
                <EmptyDataTemplate>
                    &nbsp;&nbsp;<asp:Button ID="btnNewButton" Text="New Loading Port" runat="server" CssClass="buttonTest" OnClick="btnNewButton_Click" CommandName="New" />
                </EmptyDataTemplate>
            </asp:FormView>
        </div>
        <div>
        <fieldset id="fsMainBorder" runat="server">
        <legend>Port of Loading Detail</legend>   
        <!-- Filter Content Start-->
            <div class="m clear">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true"
                CssClass="table" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid" 
                DataSourceID="GridViewSqlDataSource" CellPadding="4" PageSize="20" PagerSettings-Position="TopAndBottom" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPreRender="GridView1_PreRender">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LoadingPortName" HeaderText="Port Name" SortExpression="LoadingPortName" Visible="false" />
                    <asp:ButtonField DataTextField="LoadingPortName" HeaderText="Loading Port Name" CommandName="Select"
                        SortExpression="LoadingPortName" CausesValidation="false" />
                    <asp:BoundField DataField="PortCode" HeaderText="Port Code" SortExpression="PortCode" />
                    <asp:BoundField DataField="CityName" HeaderText="City" SortExpression="CityName" />
                    <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" />
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
               
            </asp:GridView>
        </fieldset>
            <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetLoadingPort" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <asp:SqlDataSource ID="FormViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                DeleteCommand="delLoadingPort" DeleteCommandType="StoredProcedure" InsertCommand="InsLoadingPort"
                InsertCommandType="StoredProcedure" SelectCommand="GetLoadingPortBylid" SelectCommandType="StoredProcedure"
                UpdateCommand="updLoadingPort" UpdateCommandType="StoredProcedure"
                OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
                OnDeleted="FormviewSqlDataSource_Deleted">
                <SelectParameters>
                    <asp:ControlParameter ControlID="GridView1" Name="lid" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:ControlParameter ControlID="GridView1" Name="lid" PropertyName="SelectedValue" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:ControlParameter ControlID="GridView1" Name="lid" PropertyName="SelectedValue" />
                    <asp:Parameter Name="LoadingPortName" Type="String" />
                    <asp:Parameter Name="PortCode" Type="String" />
                    <asp:Parameter Name="CityName" Type="String" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="LoadingPortName" Type="String" />
                    <asp:Parameter Name="PortCode" Type="String" />
                    <asp:Parameter Name="CityName" Type="String" />
                    <asp:Parameter Name="lMode" Type="Int32" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </InsertParameters>
            </asp:SqlDataSource>
        </div>

</asp:Content>
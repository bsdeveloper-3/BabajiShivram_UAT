<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Location.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Location" Title="Location Setup"  %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
<asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
    
        <div align="center">
            <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" />
        </div>
        <div class="clear"></div>
        <fieldset>
        <legend>Location Detail</legend>
        <div>
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="lId" DataSourceID="SDSFormView" 
            Width="99%" OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated"
                OnItemDeleted="FormView1_ItemDeleted" OnDataBound="FormView1_DataBound">
                <EmptyDataTemplate>
                <asp:Button ID="btnNew" Text="New Location" OnClick="btnNewButton_Click" runat="server"/>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnEditButton" Text="Edit" CommandName="Edit" runat="server" />
                    <asp:Button ID="btnDeleteButton" Text="Delete" OnClientClick="return confirm('Sure to delete?');"
                        runat="server" CommandName="Delete" />
                    <asp:Button ID="btnNewButton" Text="New" runat="server" OnClick="btnNewButton_Click"/>
                
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                City Name
                            </td>
                            <td>
                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("CityName") %>'></asp:Label>
                            </td>
                            <td>
                                City Code
                            </td>
                            <td>
                               <asp:Label ID="lblCityCode" runat="server" Text='<%# Bind("CityCode") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                State
                            </td>
                            <td>
                                <asp:Label ID="lblStateName" runat="server" Text='<%# Bind("StateName") %>'></asp:Label></td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                           
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdateButton" Text="Update" runat="server"
                        CommandName="Update" CssClass="btn" ValidationGroup="Required"  />
                    <asp:Button ID="btnUpdateCancelButton" Text="Cancel" CssClass="btn" runat="server"
                        CausesValidation="False" CommandName="Cancel" />
                
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                City Name
                                <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="txtCityName" 
                                Text="*" ErrorMessage="Please Enter City Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityName" MaxLength="100"  runat="server" Text='<%# Bind("CityName") %>'></asp:TextBox>
                            </td>
                            <td>
                                City Code
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCityCode" 
                                Text="*" ErrorMessage="Please Enter City Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityCode" runat="server" Text='<%# Bind("CityCode") %>' 
                                 MaxLength="50" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                State
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddState" Text="*"
                                    ErrorMessage="Please Select State" ValidationGroup="Required" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddState" DataSourceID="DataSourceStateName" runat="server" CssClass="DropDownBox"
                                DataTextField="StateName" DataValueField="StateId" AppendDataBoundItems="true"
                                Width="185px" SelectedValue='<%# Bind("StateId") %>'>
                                <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                </asp:DropDownList> 
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                            
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:Button ID="btnInsertButton" Text="Save" ValidationGroup="Required"
                        runat="server" CommandName="Insert" TabIndex="4" />
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false" CommandName="Cancel" TabIndex="5" />
              
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                City Name
                                <span style="color:Red">*</span>
                                <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="txtCityName" Display="None"
                                 InitialValue="" ErrorMessage="Please Enter City Name" ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityName" Text='<%# Bind("CityName") %>' runat="server" MaxLength="100" TabIndex="1" /></td>
                            <td>
                                City Code
                                <span style="color:Red">*</span>
                                <asp:RequiredFieldValidator ID="RFVCodr" runat="server" ControlToValidate="txtCityCode" Display="None"
                                 InitialValue="" ErrorMessage="Please Enter City Code" ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityCode" Text='<%# Bind("CityCode") %>' runat="server" MaxLength="50" TabIndex="2" /></td>
                        </tr>
                        <tr>
                            <td>
                                State
                                <span style="color:Red">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddState" Display="None"
                                 ErrorMessage="Please Select State" ValidationGroup="Required" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>   
                            </td>
                            <td>
                                <asp:DropDownList ID="ddState" DataSourceID="DataSourceStateName" runat="server" 
                                DataTextField="StateName" DataValueField="StateId" AppendDataBoundItems="true"
                                Width="185px" SelectedValue='<%# Bind("StateId") %>' TabIndex="3">
                                <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                </asp:DropDownList> 
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                          
                </InsertItemTemplate>
            </asp:FormView>
        </div>
        <div>
            <asp:SqlDataSource ID="SDSFormView" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetLocationById" SelectCommandType="StoredProcedure"
                InsertCommand="insLocation" InsertCommandType="StoredProcedure"
                UpdateCommand="updLocation" UpdateCommandType="StoredProcedure"
                DeleteCommand="delLocation" DeleteCommandType="StoredProcedure"
                OnInserted="DataSourceFormView_Inserted" OnUpdated="DataSourceFormView_Updated"
                OnDeleted="DataSourceFormView_Deleted" >
                <SelectParameters>
                    <asp:ControlParameter Name="lId" ControlID="gvLocaton" PropertyName="SelectedValue" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="CityName" />
                    <asp:Parameter Name="CityCode" />
                    <asp:ControlParameter ControlID="ctl00$ContentPlaceHolder1$FormView1$ddState"
                        Name="StateId" PropertyName="SelectedValue" Type="int32" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="lId" />
                    <asp:Parameter Name="CityName" />
                    <asp:Parameter Name="CityCode" />
                    <asp:ControlParameter ControlID="ctl00$ContentPlaceHolder1$FormView1$ddState"
                        Name="StateId" PropertyName="SelectedValue" Type="int32" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="lId" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4"/>
                </DeleteParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceStateName" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            SelectCommand="GetStateName" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </div>
        
        <div>
            <asp:GridView ID="gvLocaton" runat="server" AutoGenerateColumns="False" Width="99%" 
                CssClass="table" PageSize="10" PagerSettings-Position="TopAndBottom"  
                DataKeyNames="CityId" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                AllowPaging="true" AllowSorting="true" DataSourceID="GridviewSqlDataSource" OnPreRender="gvLocaton_PreRender">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City Name" SortExpression="CityName">
                        <ItemTemplate>
                            <asp:LinkButton CausesValidation="false" ID="lnkCityName" Text='<%#Eval("CityName") %>'
                                runat="server" CommandName="Select" OnClick="lnkCityName_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CityCode" HeaderText="City Code" SortExpression="CityCode" />
                    <asp:BoundField DataField="StateName" HeaderText="State" SortExpression="StateName" />
                </Columns>
             
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
                 
            </asp:GridView>
            <br />
        </div>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetLocation" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
    
    </ContentTemplate>
</asp:UpdatePanel> 
</asp:Content>

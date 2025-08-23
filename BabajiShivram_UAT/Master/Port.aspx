<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Port.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Port" Title="Port Setup" %>
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
            <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewSqlDataSource" 
                OnDataBound="FormView1_DataBound" OnItemInserted="FormView1_ItemInserted" OnItemDeleted="FormView1_ItemDeleted"
                OnItemUpdated="FormView1_ItemUpdated" OnItemCommand="FormView1_ItemCommand" Width="100%">
                <EditItemTemplate>
                    <fieldset><legend>Update Port Detail</legend>
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" 
                       Text="Update" ValidationGroup="Required" TabIndex="5"/>
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" TabIndex="6"/>
                    <div class="m"></div>        
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Port Name
                                <asp:RequiredFieldValidator ID="RFVupdPortName" runat="server" ControlToValidate="PortNameTextBox" Text="*"
                                    ErrorMessage="Please Enter Port Name" Display="Dynamic" ValidationGroup="Required" SetFocusOnError=true></asp:RequiredFieldValidator>    
                            </td>
                            <td>
                                <asp:TextBox ID="PortNameTextBox" Text='<%# Bind("PortName") %>' MaxLength="100" TabIndex="1" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Port Code
                                <asp:RequiredFieldValidator ID="RFVupdPortCode" runat="server" ControlToValidate="PortCodeTextBox" Text="*"
                                    ErrorMessage="Please Enter Port Code" ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="PortCodeTextBox" Text='<%# Bind("PortCode") %>'
                                    MaxLength="50" TabIndex="2" runat="server"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                City
                                <asp:RequiredFieldValidator ID="RFVinsCity" runat="server" ControlToValidate="ddcity"
                                    Display="Dynamic" ErrorMessage="Please Select City" InitialValue="0" Text="*"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>
                            <td>
                                <asp:DropDownList ID="ddcity" runat="server" DataSourceID="CitySqlDataSource" DataTextField="CityName"
                                    DataValueField="CityId" AppendDataBoundItems="True" TabIndex="3">
                                    <asp:ListItem Value="0" Text="Select City"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnCityId" Value='<%# Eval("CityId") %>' runat="server" />
                            </td>
                            <td>
                                Mode
                            </td>
                            <td>
                                <%--<asp:Label ID="Label1" runat="server" Text='<%# Eval("transmode") %>'></asp:Label>--%>
                                <asp:DropDownList ID="ddModeEdit" runat="server" SelectedValue='<%# Bind("lMode") %>' TabIndex="4">
                                    <asp:ListItem Value="0" Text="--Select--" Selected="True" ></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Air"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Sea"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Land"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" Text='<%# Bind("PortHeadEmail") %>'
                                   TextMode="MultiLine" MaxLength="100" runat="server" TabIndex="4"> </asp:TextBox>
                            </td>
                            <td>
                               Duty Account Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtDutyAccName" Text='<%# Bind("DutyAccountName") %>'
                                MaxLength="100" runat="server" TabIndex="4"> </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <fieldset><legend>Add New Port</legend>
                    <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                        TabIndex="6" Text="Save" />
                    <asp:Button ID="btnInsertCancelButton" runat="server" CommandName="Cancel"
                        CausesValidation="false" Text="Cancel" TabIndex="7" />
                    <div class="m"></div>      
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Port Name
                                <asp:RequiredFieldValidator ID="RFVinsPortName" runat="server" ErrorMessage="Please Enter Port Name" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="Required" ControlToValidate="PortNameTextBox"></asp:RequiredFieldValidator>   
                            </td>
                            <td>
                                <asp:TextBox ID="PortNameTextBox" Text='<%# Bind("PortName") %>'
                                MaxLength="100" TabIndex="1" runat="server"></asp:TextBox>
                                
                            </td>
                            <td>
                                Port Code
                                <asp:RequiredFieldValidator ID="RFVinsPortCode" runat="server" ErrorMessage="Please Enter Port Code" Text="*"
                                    ControlToValidate="PortCodeTextBox" Display="Dynamic" ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>    
                            </td>
                            <td>
                                <asp:TextBox ID="PortCodeTextBox" Text='<%# Bind("PortCode") %>'
                                MaxLength="50" TabIndex="2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                City
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddcity"
                                    Display="Dynamic" ErrorMessage="Please Select City" InitialValue="0" Text="*"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>  
                            </td>
                            <td>
                                <asp:DropDownList ID="ddcity" runat="server" DataSourceID="CitySqlDataSource" DataTextField="CityName"
                                    DataValueField="CityId" SelectedValue='<%# Bind("CityId") %>'
                                    AppendDataBoundItems="True" TabIndex="3">
                                    <asp:ListItem Selected="True" Value="0" Text="Select City"></asp:ListItem>
                                </asp:DropDownList>
                                </td>
                            <td>
                                Mode
                                <asp:RequiredFieldValidator ID="RFVinsMode" runat="server" ControlToValidate="ddMode"
                                    Display="Dynamic" ErrorMessage="Please Select Mode" InitialValue="0" Text="*"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>
                            <td>
                                <asp:DropDownList ID="ddMode" runat="server" SelectedValue='<%# Bind("lMode") %>' TabIndex="4">
                                    <asp:ListItem Value="0" Text="--Select--" Selected="True" ></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Air"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Sea"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Land"></asp:ListItem>
                                </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" Text='<%# Bind("PortHeadEmail") %>'
                                   TextMode="MultiLine" MaxLength="100" runat="server" TabIndex="4"> </asp:TextBox>
                            </td>
                            <td>
                               Duty Account Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtDutyAccName" Text='<%# Bind("DutyAccountName") %>'
                                MaxLength="100" runat="server" TabIndex="5"> </asp:TextBox>
                            </td>
                            
                        </tr>
                    </table>
                    </fieldset>        
                </InsertItemTemplate>
                <ItemTemplate>
                    <fieldset><legend>Port Detail</legend>
                    <asp:Button ID="btnEditButton" CssClass="btn" Text="Edit" CommandName="Edit" runat="server"/>
                    <asp:Button ID="btnDeleteButton" Text="Delete" OnClientClick="return confirm('Sure to delete?');"
                        runat="server" CssClass="btn" CommandName="Delete" />
                    <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server" CssClass="btn" />
                    <div class="m"></div>     
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Port Name</td>
                            <td>
                                <asp:Label ID="PortNameLabel" runat="server" Text='<%# Bind("PortName") %>'></asp:Label></td>
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
                                <asp:Label ID="CityLabel" runat="server" Text='<%# Eval("sName") %>'></asp:Label></td>
                            <td>
                                Mode</td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("transmode") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                Email
                            </td>
                            <td>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("PortHeadEmail") %>'></asp:Label>
                            </td>
                            <td>
                               Duty Account Name
                            </td>
                            <td>
                                <asp:Label ID="lblDutyAccName" runat="server" Text='<%# Eval("DutyAccountName") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </ItemTemplate>
                <EmptyDataTemplate>
                    &nbsp;&nbsp;<asp:Button ID="btnNewButton" Text="New Port" runat="server" CssClass="buttonTest" OnClick="btnNewButton_Click" CommandName="New" />
                </EmptyDataTemplate>
            </asp:FormView>
        </div>
        <div>
        <fieldset id="fsMainBorder" runat="server">
        <legend>Port Detail</legend>   
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
                    <asp:ButtonField DataTextField="PortName" HeaderText="Port Name" CommandName="Select"
                        SortExpression="PortName" CausesValidation="false" />
                    <asp:BoundField DataField="portcode" HeaderText="Port Code" SortExpression="PortCode" />
                    <asp:BoundField DataField="CityName" HeaderText="City" SortExpression="CityName" />
                    <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" />
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
               
            </asp:GridView>
        </fieldset>                
            <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetAllPort" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <asp:SqlDataSource ID="FormViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                DeleteCommand="delPortMS" DeleteCommandType="StoredProcedure" InsertCommand="InsPortMS"
                InsertCommandType="StoredProcedure" SelectCommand="getportbylid" SelectCommandType="StoredProcedure"
                UpdateCommand="updPortMS" UpdateCommandType="StoredProcedure"
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
                    <asp:Parameter Name="PortName" Type="String" />
                    <asp:Parameter Name="PortCode" Type="String" />
                    <asp:ControlParameter ControlID="FormView1$ddcity" Name="CityId" PropertyName="SelectedValue" />
                    <asp:Parameter Name="DutyAccountName" Type="String" />
                    <asp:Parameter Name="PortHeadEmail" Type="String"/>
                    <asp:Parameter Name="lMode" Type="Int32" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="PortName" Type="String" />
                    <asp:Parameter Name="PortCode" Type="String" />
                    <asp:Parameter Name="CityId" Type="Int32" />
                    <asp:Parameter Name="lMode" Type="Int32" />
                    <asp:Parameter Name="DutyAccountName" Type="String"/>
                    <asp:Parameter Name="PortHeadEmail" Type="String"/>
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="CitySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetLocation">
            </asp:SqlDataSource>
        </div>

</asp:Content>

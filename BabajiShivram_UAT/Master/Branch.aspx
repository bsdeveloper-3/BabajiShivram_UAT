<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Branch.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Branch" Title="Branch Setup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div id="divJavaScript">

        <script type="text/javascript">
       
            function OnUserSelected(source, eventArgs) {
                var results = eval('(' + eventArgs.get_value() + ')');
                $get('ctl00_ContentPlaceHolder1_FormView1_txtUserName').value = results.Username;
                $get('<%=hdnUserId.ClientID %>').value = results.Userid;
                //document.getElementById('ctl00_ContentPlaceHolder1_FormView1_txtUserName').focus();
            }

        </script>

    </div>

    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
       <div class="clear"></div>     
        <asp:HiddenField ID="hdnUserId" runat="server" />
    </div>
    <div>
        <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid"
            OnItemDeleted="FormView1_ItemDeleted" OnItemUpdated="FormView1_ItemUpdated" OnItemInserted="FormView1_ItemInserted"
            OnItemCommand="FormView1_ItemCommand" Width="100%" OnDataBound="FormView1_DataBound">
            <EditItemTemplate>
                <fieldset><legend>Update Branch Detail</legend>
                <div class="m clear">
                <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" TabIndex="8"
                    Text="Update" ValidationGroup="Required" />
                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    TabIndex="9" Text="Cancel" />
                </div>    
                <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            Branch Name
                            <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBranchNameED" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Enter Branch Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchNameED" Text='<%# Bind("BranchName") %>' TabIndex="1" runat="server" />
                        </td>
                        <td>
                            Branch Code
                            <asp:RequiredFieldValidator ID="RFV02" runat="server" ErrorMessage="Please Enter Branch Code" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtBranchCodeED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchCodeED" Text='<%# Bind("BranchCode") %>' TabIndex="2" runat="server" />
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
                                DataValueField="CityId" TabIndex="3" SelectedValue='<%# Bind("CityId") %>'
                                AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="0" Text="-Select City-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Telephone
                            <asp:RequiredFieldValidator ID="RFV04" runat="server" ErrorMessage="Please Enter Telephone No" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtTelephoneED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTelephoneED" Text='<%#Bind("ContactNo") %>' runat="server" TabIndex="4" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address
                            <asp:RequiredFieldValidator ID="RFV05" runat="server" ErrorMessage="Please Enter Address" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtAddressED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAddressED" Text='<%#Bind("Address") %>' runat="server" TabIndex="5"
                                TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Branch Job Prefix
                            <asp:RequiredFieldValidator ID="RFVJobPrefixED" runat="server" ErrorMessage="Please Enter Job Prefix" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtBranchPrefixED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchPrefixED" Text='<%#Bind("BranchPreFix") %>' runat="server" TabIndex="7" MaxLength="50" />
                        </td>
                        <td>
                         Email
                            <asp:RequiredFieldValidator ID="RFVEmailED" runat="server" ErrorMessage="Please Enter Job Prefix" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtEmailED" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                         <asp:TextBox ID="txtEmailED" Text='<%#Bind("BranchEmail") %>' runat="server" TabIndex="6" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
                </fieldset>
            </EditItemTemplate>
            <InsertItemTemplate>
                <fieldset><legend>Add Branch Detail</legend>
                <div class="m clear">
                <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                    TabIndex="8" Text="Save" />
                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    TabIndex="9" Text="Cancel" />
                </div>
                <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            Branch Name
                            <asp:RequiredFieldValidator ID="RFV010" runat="server" ControlToValidate="txtBranchName" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Enter Branch Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchName" Text='<%# Bind("BranchName") %>' TabIndex="1"
                                runat="server" />
                        </td>
                        <td>
                            Branch Code
                            <asp:RequiredFieldValidator ID="RFV020" runat="server" ErrorMessage="Please Enter Branch Code" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtBranchCode" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchCode" Text='<%# Bind("BranchCode") %>' TabIndex="3"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            City
                            <asp:RequiredFieldValidator ID="RFV030" runat="server" ControlToValidate="ddcity"
                                Display="Dynamic" ErrorMessage="Please Select City" InitialValue="0" Text="*"
                                ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddcity" runat="server" DataSourceID="CitySqlDataSource" DataTextField="CityName"
                                DataValueField="CityId" TabIndex="3" SelectedValue='<%# Bind("CityId") %>'
                                AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="0" Text="-Select City-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Telephone
                            <asp:RequiredFieldValidator ID="RFV040" runat="server" ErrorMessage="Please Enter Telephone No" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtTelephone" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTelephone" Text='<%#Bind("ContactNo") %>' runat="server" TabIndex="4" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address
                            <asp:RequiredFieldValidator ID="RFV050" runat="server" ErrorMessage="Please Enter Address" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtAddress" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAddress" runat="server" TabIndex="5" TextMode="MultiLine"
                                Text='<%#Bind("Address") %>' />
                        </td>
                      
                    </tr>
                    <tr>
                        <td>
                            Job Prefix
                            <asp:RequiredFieldValidator ID="RFVPrefix" runat="server" ErrorMessage="Please Enter Branch Job Prefix" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtBranchPrefix" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBranchPrefix" runat="server" TabIndex="7" Text='<%#Bind("BranchPrefix") %>' MaxLength="50" />
                        </td>
                          <td>
                            Email
                            <asp:RequiredFieldValidator ID="RFVEmail" runat="server" ErrorMessage="Please Enter Branch Email" SetFocusOnError="true"
                                Text="*" ControlToValidate="txtEMail" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" TabIndex="6" TextMode="MultiLine"
                                Text='<%#Bind("BranchEmail") %>' />
                        </td>
                    </tr>
                </table>
                </fieldset>
            </InsertItemTemplate>
            <ItemTemplate>
            <fieldset><legend>Branch Detail</legend>
                <div class="m clear">
                <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" CssClass="edit"
                    Text="Edit" />
                <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" CssClass="delete"
                    OnClientClick="return confirm('Sure to delete?');" Text="Delete" />
                <asp:Button ID="btnCancelButton" runat="server" CssClass="cancel" Text="Cancel" CommandName="Cancel" />
                <asp:HiddenField ID="hdnBranchId" runat="server" Value='<%# Bind("lId") %>' />
                </div>
                <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            Branch Name
                        </td>
                        <td>
                            <asp:Label ID="lblBranchName" Text='<%# Eval("BranchName") %>' runat="server" />
                        </td>
                        <td>
                            Branch Code
                        </td>
                        <td>
                            <asp:Label ID="lblBranchCode" Text='<%# Eval("BranchCode") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            City
                        </td>
                        <td>
                            <asp:Label ID="txtCity" Text='<%# Eval("CityName") %>' runat="server" />
                        </td>
                        <td>
                            Phone No
                        </td>
                        <td>
                            <asp:Label ID="lblTelephone" Text='<%# Eval("ContactNo") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblAddress" Text='<%# Eval("Address") %>' runat="server" />
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            Job Prefix
                        </td>
                        <td>
                            <asp:Label ID="lblPrefix" Text='<%# Eval("BranchPrefix") %>' runat="server" />
                        </td>
                        <td>
                            Email
                        </td>
                        <td>
                            <asp:Label ID="lblBranchEmail" Text='<%# Eval("BranchEmail") %>' runat="server" />
                        </td>
                    </tr>
                </table>
                </fieldset>
            <fieldset><legend>Add/Remove Port</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlPort" runat="server" DefaultButton="btnAddPort">
                        <asp:HiddenField ID="hdnPortId" runat="server" Value="0" />
                        <asp:TextBox ID="txtPortName" runat="server" TabIndex="1"/>
                        <asp:Button ID="btnAddPort" Text="Add Port" CssClass="buttonSmall" runat="server"
                            OnClick="btnAddPort_Cick" TabIndex="2"/>
                        <div id="divwidth">
                        </div>
                    </asp:Panel>
                    <asp:GridView ID="gvPort" runat="server" DataKeyNames="lId" Width="100%" AllowPaging="True"
                        PageSize="20" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourcePort"
                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PortName" HeaderText="Port Name" />
                            <asp:BoundField DataField="Mode" HeaderText="Mode" />
                            <asp:BoundField DataField="CityName" HeaderText="Port City" />
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:LinkButton Text="Remove" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');"
                                        runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <AjaxToolkit:AutoCompleteExtender ID="portExtender" runat="server" TargetControlID="txtPortName"
                        CompletionListElementID="divwidth" ServicePath="../WebService/PortAutoComplete.asmx"
                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidth"
                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortSelected"
                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                    </AjaxToolkit:AutoCompleteExtender>
                </div>
                <div id="DivDataSource">
                    <asp:SqlDataSource ID="DataSourcePort" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetPortByBranch" SelectCommandType="StoredProcedure" DeleteCommand="delBranchPort"
                        DeleteCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="gvBranch" Name="BranchId" PropertyName="SelectedValue" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:ControlParameter ControlID="gvPort" Name="lid" PropertyName="SelectedValue" />
                            <asp:SessionParameter Name="lUser" SessionField="UserId" />    
                        </DeleteParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>    
            <fieldset><legend>Add/Remove Warehouse</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlWareHouse" runat="server" DefaultButton="btnAddWareHouse">
                        <asp:HiddenField ID="hdnWareHouseId" runat="server" Value="0" />
                        <asp:TextBox ID="txtWareHouseName" runat="server" TabIndex="3"/>
                        <asp:Button ID="btnAddWareHouse" Text="Add Warehouse" CssClass="buttonSmall" runat="server"
                            OnClick="btnAddWareHouse_Cick" TabIndex="4"/>
                        <div id="divwidth11">
                        </div>
                    </asp:Panel>
                    <asp:GridView ID="gvWareHouse" runat="server" DataKeyNames="lId" Width="100%" AllowPaging="True"
                        PageSize="20" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceWareHouse"
                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="WareHouseName" HeaderText="Warehouse Name" />
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" Text="Remove" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');"
                                        runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtWareHouseName"
                        CompletionListElementID="divwidth11" ServicePath="../WebService/WareHouseAutoComplete.asmx"
                        ServiceMethod="GetWareHouseCompletionList" MinimumPrefixLength="2" BehaviorID="divwidth11"
                        ContextKey="1231" UseContextKey="True" OnClientItemSelected="OnWareHouseSelected"
                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                    </AjaxToolkit:AutoCompleteExtender>
                </div>
                <div id="DivDataSource2">
                    <asp:SqlDataSource ID="DataSourceWareHouse" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetWareHouseByBranch" SelectCommandType="StoredProcedure" DeleteCommand="delBranchWarehouse"
                        DeleteCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="gvBranch" Name="BranchId" PropertyName="SelectedValue" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:ControlParameter ControlID="gvWareHouse" Name="lid" PropertyName="SelectedValue" />
                            <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        </DeleteParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
            <fieldset><legend>Add/Remove CFS</legend>
                <div class="m clear">
                    <asp:Panel ID="PanelCFS" runat="server" DefaultButton="btnAddCFS">
                        <asp:HiddenField ID="hdnCFSId" runat="server" Value="0" />
                        <asp:Label ID="lblCFSName" runat="server">CFS Name</asp:Label>
                        <asp:TextBox ID="txtCFSName" runat="server" Width="250px" TabIndex="5"/>
                        <asp:Label ID="lblUserName" runat="server">User Name</asp:Label>
                        <asp:TextBox ID="txtUserName" Width="250px" runat="server" ToolTip="Select User"
                            CssClass="SearchTextbox" placeholder="Search" ></asp:TextBox>
                        <asp:Button ID="btnAddCFS" Text="Add CFS" runat="server"
                            OnClick="btnAddCFS_Cick" TabIndex="6"/>
                        <div id="divCFS">
                        </div>
                        <div id="divwidthUser" runat="server">
                        </div>
                    </asp:Panel>
                    <asp:GridView ID="gvCFS" runat="server" DataKeyNames="lId" Width="100%" AllowPaging="True"
                        PageSize="20" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceCFS"
                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" OnRowCommand="gvCFS_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="CFSName" HeaderText="CFS Name" />--%>
                            <asp:TemplateField HeaderText="CFS Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCFSName" runat="server" Text='<%# Eval("CFSName") %>' CommandName="select" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CFSUserName" HeaderText="CFS User Name" />
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRemoveCFS" Text="Remove" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');"
                                        runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteCFS" runat="server" TargetControlID="txtCFSName"
                        CompletionListElementID="divCFS" ServicePath="../WebService/CFSAutoComplete.asmx"
                        ServiceMethod="GetCFSCompletionList" MinimumPrefixLength="2" BehaviorID="divCFS"
                        ContextKey="1231" UseContextKey="True" OnClientItemSelected="OnCFSSelected"
                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                    </AjaxToolkit:AutoCompleteExtender>

                    <AjaxToolkit:AutoCompleteExtender ID="UserExtender" runat="server" TargetControlID="txtUserName"
                        CompletionListElementID="divwidthUser" ServicePath="~/WebService/UserAutoComplete.asmx"
                        ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthUser"
                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnUserSelected"
                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                    </AjaxToolkit:AutoCompleteExtender>
                </div>
                <div>
                    <asp:SqlDataSource ID="DataSourceCFS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetCFSByBranch" SelectCommandType="StoredProcedure" DeleteCommand="delBranchCFS"
                        DeleteCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="gvBranch" Name="BranchId" PropertyName="SelectedValue" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:ControlParameter ControlID="gvCFS" Name="lid" PropertyName="SelectedValue" />
                            <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        </DeleteParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
                <div id="DivJS">

                <script type="text/javascript">
                function OnPortSelected(source, eventArgs)
                {
                   // alert(eventArgs.get_value());
                    var results = eval('('  + eventArgs.get_value() + ')');
                  //  document.getElementById('txtConsigneeId').value =   results.ConsigneeId; 
                  //  alert(results.ConsigneeId);
                    $get('<%=FormView1.FindControl("hdnPortId").ClientID%>').value = results.PortId;            
                }
                
                $addHandler
                (
                   $get('<%=FormView1.FindControl("txtPortName").ClientID%>'), 'keypress', 
                       function()
                       {
                           $get('<%=FormView1.FindControl("hdnPortId").ClientID %>').value ='0';
                       }
               );
             </script>
        
                <script type="text/javascript">
                function OnWareHouseSelected(source, eventArgs)
                {
                    var results = eval('('  + eventArgs.get_value() + ')');
                    $get('<%=FormView1.FindControl("hdnWareHouseId").ClientID%>').value = results.WarehouseId;            
                }
        
                $addHandler
                (
                   $get('<%=FormView1.FindControl("txtWareHouseName").ClientID%>'), 'keypress', 
                       function()
                       {
                           $get('<%=FormView1.FindControl("hdnWareHouseId").ClientID %>').value ='0';
                       }
               );
            </script>
                <script type="text/javascript">
                function OnCFSSelected(source, eventArgs)
                {
                    var results = eval('('  + eventArgs.get_value() + ')');
                    $get('<%=FormView1.FindControl("hdnCFSId").ClientID%>').value = results.CFSId;            
                }
                    
                $addHandler
                (
                    $get('<%=FormView1.FindControl("txtCFSName").ClientID%>'), 'keypress', 
                        function()
                        {
                            $get('<%=FormView1.FindControl("hdnCFSId").ClientID %>').value ='0';
                        }
                );
            </script>
            </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                &nbsp;&nbsp;<asp:Button ID="btnnew" runat="server" Text="New Branch" CommandName="New" CssClass="buttonTest" />
            </EmptyDataTemplate>
        </asp:FormView>
        
    </div>
   <fieldset id="fsMainBorder" runat="server">
    <legend>Branch Detail</legend>    
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
                <asp:TemplateField HeaderText="Branch Name" SortExpression="BranchName">
                    <ItemTemplate>
                        <asp:LinkButton CausesValidation="false" ID="lnkBranchName" Text='<%#Eval("BranchName") %>'
                            runat="server" CommandName="Select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="lId" HeaderText="lId" InsertVisible="False" ReadOnly="True"
                    SortExpression="lId" Visible="False" />
                <asp:BoundField DataField="BranchCode" HeaderText="Branch Code" SortExpression="BranchCode" />
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
            runat="server" SelectCommand="GetBranchBylId" SelectCommandType="StoredProcedure"
            InsertCommand="insBranch" InsertCommandType="StoredProcedure" UpdateCommand="updBranch"
            UpdateCommandType="StoredProcedure" DeleteCommand="delBranch" DeleteCommandType="StoredProcedure"
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvBranch" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="BranchName" Type="string" />
                <asp:Parameter Name="BranchCode" Type="string" />
                <asp:Parameter Name="CityId" Type="int32" />
                <asp:Parameter Name="ContactNo" Type="string" />
                <asp:Parameter Name="Address" Type="string" />
                <asp:Parameter Name="BranchEmail" Type="string" />
                <asp:Parameter Name="BranchPrefix" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="gvBranch" Name="lid" PropertyName="SelectedValue" />
                <asp:Parameter Name="BranchName" Type="string" />
                <asp:Parameter Name="BranchCode" Type="string" />
                <asp:Parameter Name="CityId" Type="int32" />
                <asp:Parameter Name="ContactNo" Type="string" />
                <asp:Parameter Name="Address" Type="string" />
                <asp:Parameter Name="BranchEmail" Type="string" />
                <asp:Parameter Name="BranchPrefix" Type="string" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lId" />
                <asp:SessionParameter Name="lUserId" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetAllBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="CitySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetLocation" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    </div>
</asp:Content>

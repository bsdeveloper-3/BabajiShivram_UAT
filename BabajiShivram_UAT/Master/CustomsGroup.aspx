<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomsGroup.aspx.cs" Inherits="Master_CustomsGroup" Title="Customs Group" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="Gvpager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <%--   <asp:UpdatePanel ID="updAdhocReport" runat="server">
        <ContentTemplate>--%>
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
        <div class="clear">
        </div>
    </div>
    <div align="left">
        &nbsp;&nbsp;<asp:Button ID="btnNewCustomGroup" runat="server" Text="New Custom Group" OnClick="btnNewCustomGroup_Click"
            Visible="false" />
    </div>
    <div>
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="lId" Width="100%" DataSourceID="FormViewDataSource"
            OnItemCommand="FormView1_ItemCommand" OnItemDeleted="FormView1_ItemDeleted" OnItemInserted="FormView1_ItemInserted"
            OnItemUpdated="FormView1_ItemUpdated">
            <EditItemTemplate>
                <asp:Panel ID="pnlUpdateCustomGroup" runat="server">
                    <fieldset>
                        <legend>Update Customs Group Detail</legend>
                        <div class="m clear">
                            <asp:Button ID="btnUpdateCustomGroup" runat="server" OnClick="btnUpdateCustomGroup_Click"
                                Text="Update" ValidationGroup="Required" TabIndex="5" />
                            <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel" TabIndex="6" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Port Name
                                    <asp:RequiredFieldValidator ID="RFVPortName" runat="server" ControlToValidate="txtPortName"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Port Name" Display="Dynamic"
                                        ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hiddenPortId" runat="server" Value='<%#Bind("PortId") %>' />
                                    <asp:TextBox ID="txtPortName" Text='<%# Bind("PortName") %>' TabIndex="1" runat="server" />
                                    <div id="divwidth345">
                                    </div>
                                    <AjaxToolkit:AutoCompleteExtender ID="portExtender" runat="server" TargetControlID="txtPortName"
                                        CompletionListElementID="divwidth345" ServicePath="../WebService/PortAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidth345"
                                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </td>
                                <td>
                                    Person Name
                                    <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="txtName"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Person Name" Display="Dynamic"
                                        ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" Text='<%# Bind("PersonName") %>' runat="server" TabIndex="3" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mobile No
                                    <asp:RequiredFieldValidator ID="RFVMobileNo" runat="server" ControlToValidate="txtTelephone"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Mobile No" Display="Dynamic"
                                        ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelephone" Text='<%#Bind("MobileNo") %>' runat="server" TabIndex="4" />
                                </td>
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdlCustomGroupStatus" runat="server" RepeatDirection="horizontal"
                                        SelectedValue='<%#Eval("isActive")%>'>
                                        <asp:ListItem Text="Active" Selected="True" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="False"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <div class="clear"></div>    
                        
                <table id="tblAuto" bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="80%">
                    <tr>
                        <th style="width:50%;padding-left:150px;">
                            <h4>
                                Custom Group
                            </h4>
                        </th>
                        <th style="width:50%">
                            <h4>
                                Alloted Custom Group
                            </h4>
                        </th>
                    </tr>
                </table>
                <div id="divEditGroupField">
                    <div style="margin-left:150px;margin-right:30px; float: left;">
                        <asp:ListBox ID="listupdCustomGroup" runat="server" SelectionMode="Multiple" DataSourceID="AvailableGroupSqlDataSource"
                            DataTextField="GroupName" DataValueField="CustomGroupId" Height="150px" Width="250px"></asp:ListBox>
                    </div>
                    <div style="float: left;">
                        <br />
                        <br />
                        <asp:ImageButton ID="btnupdInsert" runat="server" OnClick="btnupdInsert_Click" ImageUrl="~/Images/right-arrow-key.png"
                            Width="30px" Height="30px" ToolTip="Add Custom Group" TabIndex="6" CommandName="GroupInsert" />
                        <br />
                        <br />
                        <asp:ImageButton ID="btnupdRemove" runat="server" OnClick="btnupdRemove_Click" ImageUrl="~/Images/left-arrow-key.png"
                            Width="30px" Height="30px" ToolTip="Remove Custom Group" TabIndex="7" CommandName="GroupRemove" />
                    </div>
                    <div style="float: left; margin-left:20px;margin-right:20px">
                        <asp:ListBox ID="listupdGroup" runat="server" SelectionMode="Multiple" Height="150px"
                            Width="250px" DataSourceID="AllottedGroupSqlDataSource" DataTextField="GroupName"
                            DataValueField="CustomGroupId"></asp:ListBox>
                    </div>
                </div>    
                    </fieldset>
                    <div id="DivJS">

                        <script type="text/javascript">
          
        function OnPortSelected(source, eventArgs)
        {
       
            var results = eval('('  + eventArgs.get_value() + ')');
            $get('<%=FormView1.FindControl("hiddenPortId").ClientID %>').value = results.PortId;
        }
        $addHandler
        (
            $get('ctl00$ContentPlaceHolder1$FormView1$txtPortName'), 'keyup', 
        
            function()
            {
                $get('<%=FormView1.FindControl("hiddenPortId").ClientID %>').value = '0';
     
            }
        );
                        </script>

                    </div>
                </asp:Panel>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:Panel ID="pnlAddCustomGroups" runat="server">
                    <fieldset>
                        <legend>Add Custom Group Detail</legend>
                        <div class="m clear">
                            <asp:Button ID="btnAddCustomGroup" runat="server" ValidationGroup="Required" OnClick="btnAddCustomGroup_Click"
                                Text="Save" TabIndex="5" />
                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel" TabIndex="6" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Port Name
                                    <asp:RequiredFieldValidator ID="rfvPortName" runat="server" ControlToValidate="txtPortName"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Port Name" Display="Dynamic"
                                        ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdnPortId" runat="server" Value='<%#Bind("PortId") %>' />
                                    <asp:TextBox ID="txtPortName" runat="server" AutoPostBack="true" Text='<%# Bind("PortName") %>'
                                        TabIndex="1" />
                                    <div id="divwidth123">
                                    </div>
                                    <AjaxToolkit:AutoCompleteExtender ID="portExtender" runat="server" TargetControlID="txtPortName"
                                        CompletionListElementID="divwidth123" ServicePath="../WebService/PortAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidth123"
                                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </td>
                                <td>
                                    Person Name
                                    <asp:RequiredFieldValidator ID="rfvPersonName" runat="server" ControlToValidate="txtPersonName"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Person Name" Display="Dynamic"
                                        ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPersonName" runat="server" Text='<%# Bind("PersonName") %>' TabIndex="3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mobile No
                                    <asp:RequiredFieldValidator ID="rfvMobileNo" runat="server" ControlToValidate="txtMobile"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Mobile No" Display="Dynamic"
                                        ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMobile" Text='<%#Bind("MobileNo") %>' runat="server" TabIndex="4" MaxLength="400" />
                                </td>
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdlStatus" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Active" Selected="True" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="False"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <div class="clear"></div>    
                        
                <table id="tblAuto" bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="80%">
                    <tr>
                        <th style="width:50%;padding-left:150px;">
                            <h4>
                                Custom Group
                            </h4>
                        </th>
                        <th style="width:50%">
                            <h4>
                                Alloted Custom Group
                            </h4>
                        </th>
                    </tr>
                </table>
                <div id="divGroupField">
                   <div style="margin-left:150px;margin-right:30px; float: left;">
                        <asp:ListBox ID="listCustomGroup" runat="server" SelectionMode="Multiple" DataSourceID="GroupSqlDataSource"
                            DataTextField="sName" DataValueField="lId" Height="150px" Width="250px"></asp:ListBox>
                    </div>
                    <div style="float: left;">
                        <br />
                        <br />
                        <asp:ImageButton ID="btnInsert" runat="server" OnClick="btnInsert_Click" ImageUrl="~/Images/right-arrow-key.png"
                            Width="30px" Height="30px" ToolTip="Add Custom Group" TabIndex="6" CommandName="GroupInsert" />
                        <br />
                        <br />
                        <asp:ImageButton ID="btnRemove" runat="server" OnClick="btnRemove_Click" ImageUrl="~/Images/left-arrow-key.png"
                            Width="30px" Height="30px" ToolTip="Remove Custom Group" TabIndex="7" CommandName="GroupRemove" />
                    </div>
                    <div style="float: left; margin-left:20px;margin-right:20px">
                        <asp:ListBox ID="listGroup" runat="server" SelectionMode="Multiple" Height="150px"
                            Width="250px"></asp:ListBox>
                    </div>
               </div>      
                    </fieldset>
                    <div id="DivJS">
                        <script type="text/javascript">
                            function OnPortSelected(source, eventArgs)
                            {                            
                                var results = eval('('  + eventArgs.get_value() + ')');
                                $get('<%=FormView1.FindControl("hdnPortId").ClientID %>').value = results.PortId;
                            }
                            $addHandler
                            (
                                $get('ctl00_ContentPlaceHolder1_FormView1_txtPortName'), 'keyup', 
                              
                                function()
                                {
                                    $get('<%=FormView1.FindControl("hdnPortId").ClientID %>').value = '0';
                         
                                }
                            );
                        </script>
                    </div>
                </asp:Panel>
            </InsertItemTemplate>
            <ItemTemplate>
                <fieldset>
                    <legend>Customs Group Detail</legend>
                    <div class="m clear">
                        <asp:Button ID="btnEditCustomGroup" runat="server" CommandName="Edit" CssClass="edit"
                            Text="Edit" />
                        <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" CssClass="delete"
                            OnClientClick="return confirm('Are you sure to delete?');" Text="Delete" />
                        <asp:Button ID="btnCancelButton" runat="server" CssClass="cancel" Text="Cancel" CommandName="Cancel" />
                    </div>
                    <table  bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                Port Name
                            </td>
                            <td>
                                <asp:Label ID="lblLid" runat="server" Text='<%# Eval("lId") %>' Visible="false" />
                                <asp:Label ID="lblPortName" runat="server" Text='<%# Eval("PortName") %>' />
                            </td>
                            <td>
                                Person Name
                            </td>
                            <td>
                                <asp:Label ID="txtPerson" Text='<%# Eval("PersonName") %>' runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mobile No
                            </td>
                            <td>
                                <asp:Label ID="lblMobileNo" Text='<%# Eval("MobileNo") %>' runat="server" />
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:Label ID="lblStatus" Text='<%#(Boolean.Parse(Eval("IsActive").ToString())? "Active" : "Inactive") %>'
                                        runat="server"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                Custom Group
                            </td>
                            <td class="label" colspan="3">
                                <asp:BulletedList ID="blViewGroup" DataSourceID="AllottedGroupSqlDataSource" DataTextField="GroupName"
                                    DataValueField="CustomGroupId" runat="server" DisplayMode="Text" CausesValidation="false"
                                    CssClass="ulList" TabIndex="35">
                                    <asp:ListItem Text="No Custom Group Assigned" Value="0" Enabled="false"></asp:ListItem>
                                </asp:BulletedList>
                                
                        </tr>
                    </table>
                </fieldset>
            </ItemTemplate>
        </asp:FormView>
    </div>
    <fieldset id="fsMainBorder" runat="server">
        <legend>Customs Group Detail</legend>
        <div>
            <asp:GridView ID="gvCustomGroup" runat="server" Width="100%" AutoGenerateColumns="False"
                CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="lId" PageSize="20" AllowPaging="true"
                OnSelectedIndexChanged="gvCustomGroup_SelectedIndexChanged" DataSourceID="GridviewSqlDataSource"
                AllowSorting="true" PagerSettings-Position="TopAndBottom" OnPreRender="gvCustomGroup_PreRender"
                OnRowDataBound="gvCustomGroup_OnRowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Person Name" SortExpression="PersonName">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#Eval("lId") %>'>
                            </asp:Label>
                            <asp:LinkButton CausesValidation="false" ID="lnkPersonName" Text='<%#Eval("PersonName") %>'
                                runat="server" CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PortName" HeaderText="Port Name" SortExpression="PortName" />
                    <asp:BoundField DataField="MobileNo" HeaderText="Person Mobile No." SortExpression="MobileNo" />
                    <asp:TemplateField HeaderText="Group Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkGroupName" runat="server" Enabled="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" Text='<%#(Boolean.Parse(Eval("IsActive").ToString())? "Active" : "Inactive") %>'
                                runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <Gvpager:GridViewPager runat="server" />
                </PagerTemplate>
            </asp:GridView>
        </div>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="GetCustomsGroupdetailBylId" SelectCommandType="StoredProcedure"
            DeleteCommand="delCustomsGroup" DeleteCommandType="StoredProcedure" OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvCustomGroup" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="lId" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetCustomsGroupdetail" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="GroupSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetCustomGroup" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="AvailableGroupSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetAvailableCustomGroup" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvCustomGroup" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="AllottedGroupSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetAllottedCustomGroup" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvCustomGroup" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

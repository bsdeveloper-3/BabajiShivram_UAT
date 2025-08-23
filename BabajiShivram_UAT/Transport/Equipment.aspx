<%@ Page Title="Transport Equipment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="Equipment.aspx.cs" Inherits="Transport_Equipment" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" />
    <asp:FormView ID="FormView1" runat="server" DataKeyNames="lid" DataSourceID="FormViewDataSource"
        Width="97%" OnDataBound="FormView1_DataBound" 
        OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated"
        OnItemDeleted="FormView1_ItemDeleted" OnItemCommand="FormView1_ItemCommand">
        <InsertItemTemplate>
            <fieldset><legend>Add New Vehicle/Equipment</legend>
            <div class="m clear">
                <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                    Text="Save" TabIndex="11"/>
                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" TabIndex="12"/>
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Vehicle No
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Vehicle No" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVehicleNo" runat="server" Text='<%# Bind("sName") %>' TabIndex="1" ></asp:TextBox>
                    </td>
                    <td>
                        Company Name
                        <asp:RequiredFieldValidator ID="RFV02" runat="server" Display="Dynamic" Text="*" SetFocusOnError="true"
                            ErrorMessage="Please Enter Company Name" ControlToValidate="txtCompanyName" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" Text='<%# Bind("CompanyName") %>' TabIndex="2" Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Vehilce Type
                        <asp:RequiredFieldValidator ID="RFV03" runat="server" ControlToValidate="txtVehicleType"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Type"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVehicleType" runat="server" Text='<%# Bind("sType") %>' TabIndex="2" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        Model
                        <asp:RequiredFieldValidator ID="RFV04" runat="server" ControlToValidate="txtModel" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Vehicle Model" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtModel" runat="server" TabIndex="4" Text='<%# Bind("Model") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        Reg Year
                    </td>
                    <td>
                        <asp:TextBox ID="txtRegYear" runat="server" TabIndex="5" Text='<%# Bind("RegYear") %>' />
                    </td>
                    <td>
                        Capacity
                    </td>
                    <td>
                        <asp:TextBox ID="txtCapacity" runat="server"  Text='<%# Bind("Capacity") %>' TabIndex="6"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fuel Average
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuelAverage" runat="server"  Text='<%# Bind("FuelAverage") %>' TabIndex="6"/>
                    </td>
                    <td>
                        Work Site
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtWorksite" runat="server"  Text='<%# Bind("WorkSite") %>' TabIndex="7"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Chassis Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtChassisNo" runat="server"  Text='<%# Bind("ChassisNo") %>' TabIndex="6"/>
                    </td>
                    <td>
                        Engine Number
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtEngineNo" runat="server"  Text='<%# Bind("EngineNo") %>' TabIndex="7"/>
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server"  Text='<%# Bind("Remark") %>' MaxLength="400" TabIndex="8"/>    
                    </td>
                </tr>
            </table>
        </fieldset>    
        </InsertItemTemplate>
        <EditItemTemplate>
            <fieldset><legend>Update Equipment Detail</legend>
            <div class="m clear">
                <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" TabIndex="11"
                    Text="Update" ValidationGroup="Required" />
                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    TabIndex="12" Text="Cancel" />
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Vehicle No
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtEdtVehicleNo" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Vehicle No" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtVehicleNo" runat="server" Text='<%# Bind("sName") %>' TabIndex="1" ></asp:TextBox>
                    </td>
                    <td>
                        Company Name
                        <asp:RequiredFieldValidator ID="RFV02" runat="server" Display="Dynamic" Text="*" SetFocusOnError="true"
                            ErrorMessage="Please Enter Company Name" ControlToValidate="txtEdtVehicleNo" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" Text='<%# Bind("CompanyName") %>' TabIndex="2" Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Vehilce Type
                        <asp:RequiredFieldValidator ID="RFV03" runat="server" ControlToValidate="txtEdtVehicleType"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Type"
                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtVehicleType" runat="server" Text='<%# Bind("sType") %>' TabIndex="2" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        Model
                        <asp:RequiredFieldValidator ID="RFV04" runat="server" ControlToValidate="txtEdtModel" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Vehicle Model" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtModel" runat="server" TabIndex="4" Text='<%# Bind("Model") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        Reg Year
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtRegYear" runat="server" TabIndex="5" Text='<%# Bind("RegYear") %>' />
                    </td>
                    <td>
                        Capacity
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtCapacity" runat="server"  Text='<%# Bind("Capacity") %>' TabIndex="6"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fuel Average
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtFuelAverage" runat="server"  Text='<%# Bind("FuelAverage") %>' TabIndex="6"/>
                    </td>
                    <td>
                        Work Site
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtEdtWorksite" runat="server"  Text='<%# Bind("WorkSite") %>' TabIndex="7"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Chassis Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtChassisNo" runat="server"  Text='<%# Bind("ChassisNo") %>' TabIndex="6"/>
                    </td>
                    <td>
                        Engine Number
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtEngineNo" runat="server"  Text='<%# Bind("EngineNo") %>' TabIndex="7"/>
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEdtRemark" runat="server"  Text='<%# Bind("Remark") %>' Width="80%" MaxLength="400" TabIndex="8"/>    
                    </td>
                </tr>
            </table>
            <div class="m clear">
            </div>
            
        </fieldset>     
        </EditItemTemplate>
        <ItemTemplate>
            <fieldset><legend>Equipment Detail</legend>
            <div class="m clear">
                <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');" Text="Delete" />
                <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server"/>
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Vehicle No
                    </td>
                    <td>
                        <asp:Label ID="lblVehicleNo" runat="server" Text='<%# Bind("sName") %>' ></asp:Label>
                    </td>
                    <td>
                        Company Name
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyName" runat="server" Text='<%# Bind("CompanyName") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Vehilce Type
                    </td>
                    <td>
                        <asp:Label ID="lblVehicleType" runat="server" Text='<%# Bind("sType") %>'></asp:Label>
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <asp:Label ID="lblModel" runat="server" Text='<%# Bind("Model") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        Reg Year
                    </td>
                    <td>
                        <asp:Label ID="lblRegYear" runat="server" TabIndex="5" Text='<%# Bind("RegYear") %>' />
                    </td>
                    <td>
                        Capacity
                    </td>
                    <td>
                        <asp:Label ID="lblCapacity" runat="server"  Text='<%# Bind("Capacity") %>'/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fuel Average
                    </td>
                    <td>
                        <asp:Label ID="lblFuelAverage" runat="server"  Text='<%# Bind("FuelAverage") %>'/>
                    </td>
                    <td>
                        Work Site
                    </td>
                    <td>
                        <asp:Label ID="lblWorksite" runat="server"  Text='<%# Bind("WorkSite") %>'/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Chassis Number
                    </td>
                    <td>
                        <asp:Label ID="lblChassisNo" runat="server"  Text='<%# Bind("ChassisNo") %>' TabIndex="6"/>
                    </td>
                    <td>
                        Engine Number
                        
                    </td>
                    <td>
                        <asp:Label ID="lblEngineNo" runat="server"  Text='<%# Bind("EngineNo") %>' TabIndex="7"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblRemark" runat="server"  Text='<%# Bind("Remark") %>'/>    
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnVehicleID" runat="server" Value='<%# Bind("lId") %>' Visible="true" />
            </fieldset>
            <AjaxToolkit:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true"
                SuppressHeaderPostbacks="true" TransitionDuration="250" FramesPerSecond="40"
                RequireOpenedPane="false" AutoSize="None">
                <Panes>
                    <AjaxToolkit:AccordionPane ID="accDocument" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Document</Header>
                        <Content>
                            <fieldset>
                                <legend>Upload Document</legend>
                                <table border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white">
                                    <tr>
                                        <td width="110px" align="center">
                                            <asp:TextBox ID="txtDocName" placeholder="Document Name" runat="server" ></asp:TextBox>
                                            <asp:HiddenField ID="hdnUploadPath" runat="server" />
                                            <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="txtDocName"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="center">
                                            <AjaxToolkit:CalendarExtender ID="CalFromDate" runat="server" Enabled="True"
                                                EnableViewState="False" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                                PopupPosition="BottomRight" TargetControlID="txtValidFrom">
                                            </AjaxToolkit:CalendarExtender>
                                          <asp:TextBox ID="txtValidFrom" runat="server" placeholder="Valid From" Width="100px"></asp:TextBox>
                                            &nbsp;To &nbsp; 
                                          <asp:TextBox ID="txtValidTo" runat="server" placeholder="Valid To" Width="100px"></asp:TextBox>   
                                            <AjaxToolkit:CalendarExtender ID="CalValidTo" runat="server" Enabled="True"
                                                EnableViewState="False" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                                PopupPosition="BottomRight" TargetControlID="txtValidTo">
                                            </AjaxToolkit:CalendarExtender>
                                        </td>
                                        <td align="center">
                                            <asp:TextBox ID="txtRenewalMonth" runat="server" placeholder="Renewal Cycle (Month)"></asp:TextBox>
                                            <asp:CompareValidator ID="CompValMonth" runat="server" ControlToValidate="txtRenewalMonth"
                                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="Invalid Month" ErrorMessage="Invalid Month."
                                                Display="Dynamic" ValidationGroup="validateDocument"></asp:CompareValidator>   
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                                            <asp:FileUpload ID="fuDocument" runat="server" />
                                                <asp:Button ID="btnSaveDocument" runat="server" OnClick="btnSaveDocument_Click" Text="Upload" ValidationGroup="validateDocument" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Download</legend>
                                <asp:GridView ID="gvTransportDocument" runat="server" AutoGenerateColumns="False" Width="90%"
                                    DataKeyNames="lid" CssClass="table" CellPadding="4" PagerStyle-CssClass="pgr" 
                                    DataSourceID="SqlDataSourceDocument" OnRowCommand="gvTransportDocument_RowCommand"
                                    AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocTypeName" HeaderText="Document" SortExpression="DocTypeName" />
                                        <asp:BoundField DataField="ValidFrom" HeaderText="Valid From" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ValidTill" HeaderText="Valid Till" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                                    CommandArgument='<%#Eval("lid") %>' OnClientClick="return confirm('Are you sure to remove document?');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </Content>
                   </AjaxToolkit:AccordionPane>
                </Panes>
            </AjaxToolkit:Accordion> 
            
        </ItemTemplate>
        <EmptyDataTemplate>
            &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="Add New Equipment" CommandName="New" />
        </EmptyDataTemplate>
    </asp:FormView>
    
    <fieldset id="fsMainBorder" runat="server">
    <legend>Manage Equipment</legend>   
    <!-- Filter Content Start-->
    <div class="m clear">
        <div class="fleft">
            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>&nbsp;&nbsp;
        </div>
        <div class="fleft"><uc1:DataFilter ID="DataFilter1" runat="server" /></div>
    </div>
    <div class="clear">
    </div>
    <!-- Filter Content END-->
        <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="False" DataSourceID="GridViewSqlDataSource"
            AllowSorting="true" AllowPaging="True" CssClass="table" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="lId" PageSize="20" PagerStyle-CssClass="pgr" OnSelectedIndexChanged="gvEquipment_SelectedIndexChanged"
            OnPreRender="gvEquipment_PreRender" PagerSettings-Position="TopAndBottom">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle No" SortExpression="sName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkVehicleNo" runat="Server" Text='<%#Eval("sName") %>' CommandName="select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="sName" HeaderText="Vehicle No" Visible="false" />
                <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" />
                <asp:BoundField DataField="sType" HeaderText="Type" SortExpression="sType" />
                <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" />
                <asp:BoundField DataField="RegYear" HeaderText="Reg Year" SortExpression="RegYear" />
                <asp:BoundField DataField="FuelAverage" HeaderText="Fuel Avg" SortExpression="FuelAverage" />
                <asp:BoundField DataField="WorkSite" HeaderText="Work Site" SortExpression="WorkSite" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="TR_GetEquipmentMSById" SelectCommandType="StoredProcedure"
            InsertCommand="TR_insEquipmentMS" InsertCommandType="StoredProcedure" 
            UpdateCommand="TR_updEquipmentMS" UpdateCommandType="StoredProcedure" 
            DeleteCommand="TR_delEquipmentMS" DeleteCommandType="StoredProcedure"
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvEquipment" Name="lid" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="sName" Type="string" />
                <asp:Parameter Name="CompanyName" Type="string" />
                <asp:Parameter Name="sType" Type="string" />
                <asp:Parameter Name="Model" Type="string" />
                <asp:Parameter Name="RegYear" Type="string" />
                <asp:Parameter Name="Capacity" Type="string" />
                <asp:Parameter Name="FuelAverage" Type="string" />
                <asp:Parameter Name="WorkSite" Type="string" />
                <asp:Parameter Name="ChassisNo" Type="string" />
                <asp:Parameter Name="EngineNo" Type="string" />
                <asp:Parameter Name="Remark" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="lId" />
                <asp:Parameter Name="sName" Type="string" />
                <asp:Parameter Name="CompanyName" Type="string" />
                <asp:Parameter Name="sType" Type="string" />
                <asp:Parameter Name="Model" Type="string" />
                <asp:Parameter Name="RegYear" Type="string" />
                <asp:Parameter Name="Capacity" Type="string" />
                <asp:Parameter Name="FuelAverage" Type="string" />
                <asp:Parameter Name="WorkSite" Type="string" />
                <asp:Parameter Name="ChassisNo" Type="string" />
                <asp:Parameter Name="EngineNo" Type="string" />
                <asp:Parameter Name="Remark" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lId" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetEquipmentMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_getEquipmentDoc" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvEquipment" Name="VehicleID" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>


<%@ Page Title="Customer License" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="CustomerLicense.aspx.cs" Inherits="Master_CustomerLicense" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="Gvpager" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="updAdhocReport" runat="server">
    <ContentTemplate>
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
        <div class="clear">
        </div>
    </div>
    <div>
        <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" 
            DataKeyNames="lid" Width="100%" OnItemCommand="FormView1_ItemCommand" 
            OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated">
            <InsertItemTemplate>
            <fieldset>
                <legend>Add New License</legend>
                <div class="m clear">
                    <asp:Button ID="btnAddButton" runat="server" ValidationGroup="InsertRequired" CommandName="Insert" Text="Save" />
                    <asp:Button ID="btnAddCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Customer
                            <asp:RequiredFieldValidator ID="RFVCustomer" runat="server" ErrorMessage="Please Select Customer" SetFocusOnError="true"
                               InitialValue="0" Text="*" ControlToValidate="ddCustomer" Display="Dynamic" ValidationGroup="InsertRequired"></asp:RequiredFieldValidator>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="DsCustomer" SelectedValue='<%# Bind("CustomerId") %>'
                               DataTextField="CustName" DataValueField="lid" AppendDataBoundItems="true">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Type
                            <asp:RequiredFieldValidator ID="RFVLicneseType" runat="server" ErrorMessage="Please Select License Type" SetFocusOnError="true"
                               InitialValue="0" Text="*" ControlToValidate="ddSchemeType" Display="Dynamic" ValidationGroup="InsertRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddSchemeType" runat="server" SelectedValue='<%# Bind("LicenseTypeId") %>'
                               DataSourceID="DsSchemeTypeMS" DataTextField="SchemeType" DataValueField="lid" AppendDataBoundItems="true"> 
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Licence No
                            <asp:RequiredFieldValidator ID="RFVLicenseNO" runat="server" ErrorMessage="Please Enter License No" SetFocusOnError="true"
                               InitialValue="" Text="*" ControlToValidate="txtLicenceNo" Display="Dynamic" ValidationGroup="InsertRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenceNo" runat="server" Text='<%# Bind("LicenceNo") %>' MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            Port 
                            <asp:RequiredFieldValidator ID="RFVPort" runat="server" ErrorMessage="Please Select Port" SetFocusOnError="true"
                               InitialValue="0" Text="*" ControlToValidate="ddPort" Display="Dynamic" ValidationGroup="InsertRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPort" runat="server" DataSourceID="DsPort" SelectedValue='<%# Bind("PortId") %>' 
                              DataTextField="PortName" DataValueField="lid" AppendDataBoundItems="true" >
                                <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                            </asp:DropDownList> 
                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Issue Date
                            <asp:RequiredFieldValidator ID="RFVtxtLicenseIssueDate" runat="server" ErrorMessage="Please Enter Date" SetFocusOnError="true"
                               InitialValue="" Text="*" ControlToValidate="txtLicenseIssueDate" Display="Dynamic" ValidationGroup="InsertRequired"></asp:RequiredFieldValidator>
                            <AjaxToolkit:CalendarExtender ID="CalIssueDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgIssueDate" PopupPosition="BottomRight"
                                TargetControlID="txtLicenseIssueDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseIssueDate" runat="server" Text='<%# Bind("LicenseIssueDate","{0:dd/MM/yyyy}") %>' Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgIssueDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                        <td>
                            Validity
                            <asp:RequiredFieldValidator ID="RFVLicenseValidity" runat="server" ErrorMessage="Please Enter Date" SetFocusOnError="true"
                               InitialValue="" Text="*" ControlToValidate="txtLicenseValidityDate" Display="Dynamic" ValidationGroup="InsertRequired"></asp:RequiredFieldValidator>
                            <AjaxToolkit:CalendarExtender ID="CalValidDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgValidDate" PopupPosition="BottomRight"
                                TargetControlID="txtLicenseValidityDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseValidityDate" runat="server" Text='<%# Bind("LicenseValidityDate","{0:dd/MM/yyyy}") %>' Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgValidDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>License Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseValue" runat="server" Text='<%# Bind("LicenseValue") %>' />
                        </td>
                        <td></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Received Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtOpenValue" runat="server" Text='<%# Bind("OpenBalance") %>' />
                        </td>
                        <td>Receive Date
                            <AjaxToolkit:CalendarExtender ID="CalOpenDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOpenDate" PopupPosition="BottomRight"
                                TargetControlID="txtOpenDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOpenDate" runat="server" Text='<%# Bind("OpenDate","{0:dd/MM/yyyy}") %>' Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgOpenDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="m clear">
                </div>
            </fieldset>
        </InsertItemTemplate>
            <EditItemTemplate>
            <fieldset>
                <legend>Update License Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Customer
                        </td>
                        <td>
                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                        </td>
                        <td>Type</td>
                        <td>
                            <asp:Label ID="txtSchemeType" runat="server" Text='<%# Eval("SchemeType") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Licence No
                        </td>
                        <td>
                            <asp:Label ID="lblLicenceNo" runat="server" Text='<%# Eval("LicenceNo") %>'></asp:Label>
                        </td>
                        <td>Port Code
                        </td>
                        <td>
                            <asp:TextBox ID="txtPortCode" runat="server" Text='<%# Bind("PortCode") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>Issue Date
                            <AjaxToolkit:CalendarExtender ID="CalIssueDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgIssueDate" PopupPosition="BottomRight"
                                TargetControlID="txtLicenseIssueDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseIssueDate" runat="server" Text='<%# Bind("LicenseIssueDate","{0:dd/MM/yyyy}") %>' Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgIssueDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                        <td>Validity
                            <AjaxToolkit:CalendarExtender ID="CalValidDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgValidDate" PopupPosition="BottomRight"
                                TargetControlID="txtLicenseValidityDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseValidityDate" runat="server" Text='<%# Bind("LicenseValidityDate","{0:dd/MM/yyyy}") %>' Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgValidDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>License Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseValue" runat="server" Text='<%# Bind("LicenseValue") %>' />
                        </td>
                        <td></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Received Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtOpenValue" runat="server" Text='<%# Bind("OpenBalance") %>' />
                        </td>
                        <td>Receive Date
                            <AjaxToolkit:CalendarExtender ID="CalOpenDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOpenDate" PopupPosition="BottomRight"
                                TargetControlID="txtOpenDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOpenDate" runat="server" Text='<%# Bind("OpenDate","{0:dd/MM/yyyy}") %>' Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgOpenDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="m clear">
                </div>
            </fieldset>
        </EditItemTemplate>
            <ItemTemplate>
            <asp:HiddenField ID="hdnLicenseId" runat="server" Value='<%# Bind("lId") %>' Visible="true" />
            <fieldset>
                <legend>License Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Customer
                        </td>
                        <td>
                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("CustName") %>'></asp:Label>
                        </td>
                        <td>Type</td>
                        <td>
                            <asp:Label ID="txtSchemeType" runat="server" Text='<%# Bind("SchemeType") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Licence No
                        </td>
                        <td>
                            <asp:Label ID="lblLicenceNo" runat="server" Text='<%# Bind("LicenceNo") %>'></asp:Label>
                        </td>
                        <td>Port Code
                        </td>
                        <td>
                            <asp:Label ID="lblPortCode" runat="server" Text='<%# Bind("PortCode") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>Issue Date
                        </td>
                        <td>
                            <asp:Label ID="lblLicenseIssueDate" runat="server" Text='<%# Bind("LicenseIssueDate","{0:dd/MM/yyyy}") %>' />
                        </td>
                        <td>Validity</td>
                        <td>
                            <asp:Label ID="lblLicenseValidityDate" runat="server" Text='<%# Bind("LicenseValidityDate","{0:dd/MM/yyyy}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>License Value
                        </td>
                        <td>
                            <asp:Label ID="lblLicenseValue" runat="server" Text='<%# Bind("LicenseValue") %>' />
                        </td>
                        <td></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Received Value                            
                        </td>
                        <td>
                            <asp:Label ID="lblOpenValue" runat="server" Text='<%# Bind("OpenBalance") %>' />
                        </td>
                        <td>Received Date</td>
                        <td>
                            <asp:Label ID="lblOpenDate" runat="server" Text='<%# Bind("OpenDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Return License</legend>
                <asp:Button ID="btnReturnLicense" Text="Return License" OnClick="btnReturnLicense_Click" runat="server" />
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Return Date
                            <AjaxToolkit:CalendarExtender ID="CalReturnDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgIssueDate" PopupPosition="BottomRight"
                                TargetControlID="txtReturnDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReturnDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                        </td>
                        <td>
                            Return Address</td>
                        <td>
                            <asp:TextBox ID="txtReturnAddress" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Dispatch Mode
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDispatchMode" runat="server">
                                <asp:ListItem Text="Courier" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Hand Delivery" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Courier/Person Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtCourierName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>License History</legend>
                <asp:GridView ID="gvLicenseHistory" runat="server" Width="100%" AutoGenerateColumns="False" 
                    CssClass="table" DataSourceID="HistoryDataSource">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpenDate" HeaderText="Received Date" SortExpression="OpenDate" DataFormatString="{0:dd/MM/yyy}"/>
                        <asp:BoundField DataField="OpenBalance" HeaderText="Received Value" SortExpression="OpenBalance" />
                        <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" SortExpression="CloseDate" DataFormatString="{0:dd/MM/yyy}" />
                        <asp:BoundField DataField="DispatchModeName" HeaderText="Dispatch Mode" SortExpression="DispatchModeName" />
                        <asp:BoundField DataField="DispatchName" HeaderText="Courier/Person" SortExpression="DispatchName"/>
                    </Columns>
                </asp:GridView>
            </fieldset>

            <asp:SqlDataSource ID="HistoryDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                runat="server" SelectCommand="GetCustomsLicenseHistory" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvCustomerLicense" Name="LicenseId" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ItemTemplate>
            <EmptyDataTemplate>
                &nbsp;&nbsp;<asp:Button ID="btnnew" runat="server" Text="New License" CommandName="New" CssClass="buttonTest" />
            </EmptyDataTemplate>
        </asp:FormView>
        </div>
        <fieldset id="fsMainBorder" runat="server">
            <legend>Customer License Detail</legend>
            <div class="clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left:30px; padding-top:3px;">
                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <div class="clear">
            </div>
            <div>
                <asp:GridView ID="gvCustomerLicense" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="lId" PageSize="20" AllowPaging="true"
                    DataSourceID="GridviewSqlDataSource" AllowSorting="true" PagerSettings-Position="TopAndBottom"
                    OnSelectedIndexChanged="gvCustomerLicense_SelectedIndexChanged" OnPreRender="gvCustomerLicense_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer" SortExpression="CustName">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#Eval("lId") %>'>
                                </asp:Label>
                                <asp:LinkButton CausesValidation="false" ID="lnkCustName" Text='<%#Eval("CustName") %>'
                                    runat="server" CommandName="Select"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" Visible="false" />
                        <asp:BoundField DataField="PortCode" HeaderText="Port" SortExpression="PortCode" />
                        <asp:BoundField DataField="SchemeType" HeaderText="Type" SortExpression="SchemeType" />
                        <%--<asp:BoundField DataField="LicenceNo" HeaderText="License No" SortExpression="LicenceNo" />--%>
                        <asp:TemplateField HeaderText="License No" SortExpression="LicenseNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLicenseNo" runat="server" Text='<%#Eval("LicenceNo") %>' OnClick="lnkLicenseNo_Click" CommandArgument='<%#Eval("LicenceNo")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LicenceNo" HeaderText="License No" SortExpression="LicenseNo" Visible="false"/>
                        <asp:BoundField DataField="LicenseIssueDate" HeaderText="Issue Date" SortExpression="LicenseIssueDate" DataFormatString="{0:dd/MM/yyy}"/>
                        <asp:BoundField DataField="LicenseValidityDate" HeaderText="Validity" SortExpression="LicenseValidityDate" DataFormatString="{0:dd/MM/yyy}"/>
                        <asp:BoundField DataField="LicenseValue" HeaderText="Value" SortExpression="LicenseValue" />
                        <asp:BoundField DataField="OpenDate" HeaderText="Received Date" SortExpression="OpenDate" DataFormatString="{0:dd/MM/yyy}"/>
                        <asp:BoundField DataField="OpenBalance" HeaderText="Received Value" SortExpression="OpenBalance" />
                        <%--<asp:BoundField DataField="CloseBalance" HeaderText="Balance" SortExpression="CloseBalance" />
                        <asp:BoundField DataField="CloseDate" HeaderText="Return Date" SortExpression="CloseDate" DataFormatString="{0:dd/MM/yyy}" />--%>
                    
                    </Columns>
                    <PagerTemplate>
                        <Gvpager:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </fieldset>
        
        <div id="divDataSource">
            <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                runat="server" SelectCommand="GetCustomerLicenseByID" SelectCommandType="StoredProcedure"
                InsertCommand="insCustomerLicense" InsertCommandType="StoredProcedure" OnInserted="FormViewDataSource_Inserted"
                UpdateCommand="updCustomerLicense" UpdateCommandType="StoredProcedure"
                OnUpdated="FormviewSqlDataSource_Updated">
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvCustomerLicense" Name="lid" PropertyName="SelectedValue" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="CustomerID" />
                    <asp:Parameter Name="PortID" />
                    <asp:Parameter Name="LicenceNo" Type="string" />
                    <asp:Parameter Name="LicenseTypeId" />
                    <asp:Parameter Name="LicenseIssueDate" Type="DateTime" />
                    <asp:Parameter Name="LicenseValidityDate" Type="DateTime" />
                    <asp:Parameter Name="LicenseValue" Type="string" />
                    <asp:Parameter Name="OpenBalance" Type="string" />
                    <asp:Parameter Name="OpenDate" Type="DateTime" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="lId" />
                    <asp:Parameter Name="PortCode" Type="string" />
                    <asp:Parameter Name="LicenseIssueDate" Type="DateTime" />
                    <asp:Parameter Name="LicenseValidityDate" Type="DateTime" />
                    <asp:Parameter Name="LicenseValue" Type="string" />
                    <asp:Parameter Name="OpenBalance" Type="string" />
                    <asp:Parameter Name="OpenDate" Type="DateTime" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetCustomsLicenseMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </div>
        
        <div id="divPopup1">
            <asp:LinkButton ID="lnkModelPopup" runat="server" Text="" />
            <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
                TargetControlID="lnkModelPopup" PopupControlID="Panel1"></AjaxToolkit:ModalPopupExtender>
            <asp:Panel ID="Panel1" style="display:none" runat="server">
            <fieldset><legend>Job License Detail</legend>
        <div class="header">
            <div class="fleft">
                <asp:LinkButton ID="lnkXslSummary" runat="server" OnClick="lnkXslSummary_Click"
                    data-tooltip="&nbsp; Export To Excel">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Excel.jpg" /></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblPopupMessage" runat="server" Text="Job License Detail"  ForeColor="Red"></asp:Label>
            </div>
            <div class="fright">
                <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server"
                    OnClick="btnCancelPopup_Click" ToolTip="Close" />
            </div>
        </div>
        <div class="clear"></div>            
        <div id="Div5" runat="server" style="max-height: 550px; min-width:500px; overflow: auto;">
            <asp:GridView ID="gvLicenseDetails" runat="server" CssClass="table" AutoGenerateColumns="false"
                AllowPaging="false" AllowSorting="true" >
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="JobrefNo" HeaderText="Job No" SortExpression="JobrefNo" />
                    <asp:BoundField DataField="PortName" HeaderText="Port" SortExpression="PortName" />
                    <asp:BoundField DataField="BOENo" HeaderText="BE No" SortExpression="BOENo" />
                    <asp:BoundField DataField="BOEDate" HeaderText="BE Date" SortExpression="BOEDate" DataFormatString="{0:dd/MM/yyy}"/>
                    <asp:BoundField DataField="Supplier" HeaderText="Supplier" SortExpression="Supplier" />
                    <asp:BoundField DataField="SchemeName" HeaderText="Type" SortExpression="LicenseType" />
                    <asp:BoundField DataField="LicenseAmount" HeaderText="Amount" SortExpression="Amount" />
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        </asp:Panel>
        </div>
        <div id="divDataSource2">             
            <asp:SqlDataSource ID="DsLicenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetCustomerJobLicense" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="LicenseNo" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DsCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetCompanyByCategoryID" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="CategoryID" DefaultValue="1" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DsPort" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetPortMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <asp:SqlDataSource ID="DsSchemeTypeMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetSchemeTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



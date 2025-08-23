<%@ Page Title="View Report" Language="C#" MasterPageFile="~/CustomerMaster.master" AutoEventWireup="true" CodeFile="ViewCustAdHocReport.aspx.cs" 
    Inherits="Reports_ViewCustAdHocReport"  Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    
        <div>
            <div align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="valSummary" runat="server" ShowSummary="false" ShowMessageBox="true"
                ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
            <div class="m clear">
            <fieldset>
            <legend>Generate Report</legend> 
            <p>
                <asp:Table ID="CustomUITable" runat="server" CssClass="table">
                    <asp:TableRow>
                        <asp:TableCell>
                            Job Date From
                            <cc1:CalendarExtender ID="CalExtJobFromDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
                                TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                        
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy" ></asp:TextBox>
                            <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                                runat="server"/>
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate" SetFocusOnError="true"
                                Text="*" ValidationGroup="Required" Display="Dynamic" ErrorMessage="Please Select Job Date From"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date" 
                                CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                            </asp:CompareValidator>    
                        </asp:TableCell>
                        <asp:TableCell>
                            Job Date To
                            <cc1:CalendarExtender ID="CalExtJobFromTo" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
                                TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                        
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy" ></asp:TextBox>
                            <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                runat="server"/>
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" Text="*" ControlToValidate="txtToDate"
                                ValidationGroup="Required" Display="Dynamic" ErrorMessage="Please Select Job Date To" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                                Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                            </asp:CompareValidator>    
                        </asp:TableCell>
		        <asp:TableCell>
                            <asp:DropDownList ID="ddShipmentType" runat="server">
                                <asp:ListItem Value="" Text="All" ></asp:ListItem>
                                <asp:ListItem Value="1" Text="Cleared" ></asp:ListItem>
                                <asp:ListItem Value="0" Text="Un-Cleared" ></asp:ListItem>
                            </asp:DropDownList>
                        <%--<asp:CheckBox ID="chkStatus" runat="server" Text="Shipment Cleared" />--%>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </p>
            <div class="m clear">
                <p>
                    <div>
                        <asp:CheckBox ID="chkMerge" Text="Merge Records" runat="server" />&nbsp;&nbsp;
                        <asp:Button ID="btnShowReport" runat="server" Text="Generate Report" ValidationGroup="Required"
                            OnClick="btnShowReport_Click" TabIndex="40" />
                    </div>
                </p>
            </div>
            </fieldset>
            </div>      
            <div class="clear">
            </div>
            <asp:GridView ID="gvViewReport" AutoGenerateColumns="true" AlternatingRowStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="Left" runat="server" Width="100%" CellPadding="4" CssClass="table"
                GridLines="Both" HeaderStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="Top">
                <Columns>
                <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>    

</asp:Content>


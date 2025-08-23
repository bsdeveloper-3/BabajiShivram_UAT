<%@ Page Title="DO Planning Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DOProcessReport.aspx.cs" 
    Inherits="Reports_DOProcessReport" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
     <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div>
                <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateFrom" PopupPosition="BottomRight"
                    TargetControlID="txtDateFrom">
                </cc1:CalendarExtender>
                <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateTo" PopupPosition="BottomRight"
                    TargetControlID="txtDateTo">
                </cc1:CalendarExtender>
            </div>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>DO Report</legend>    
            <div>
                <div class="fleft">
                    <asp:Button ID="btnShowReport" Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
                </div>
                <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            </div>
            <div class="m clear"></div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Date From
                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td>
                        Date To
                        <asp:RequiredFieldValidator ID="RFVDateTo" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateTo" ErrorMessage="Please Enter TO Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValDateTo" runat="server" ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid To Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                </tr>    
                <tr>
                    <td>DO Remark</td>
                    <td>
                        <asp:DropDownList ID="ddRemark" runat="server">
                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Cheque Ready" Value="1"></asp:ListItem>
                            <asp:ListItem Text="DD Ready" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Cheque Not Prepare By KAM" Value="3"></asp:ListItem>
                            <asp:ListItem Text="RTGS Confirmation Awaited" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Charges Awaited From Shipping line" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Payment confirmation awaited" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Type of delivery not confirm (LOADED / DESTUFF)" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Invoice Requested" Value="8"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <div class="m clear"></div>
            <div>
                <asp:GridView ID="gvDOReport" runat="server" AutoGenerateColumns="False" CssClass="table">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="JobRefNo" SortExpression="JobRefNo" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                        <asp:BoundField DataField="FFName" HeaderText="Console Agent" SortExpression="FFName" />
                        <asp:BoundField DataField="ShippingName" HeaderText="Shipping Name" SortExpression="ShippingName" />
                        <asp:BoundField DataField="JobType" HeaderText="Job Type" SortExpression="JobType" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Job Created Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ETA" HeaderText="ETA" SortExpression="ETA" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="DOProcessRMK" HeaderText="DO Process" SortExpression="DOProcessRMK" />
                        <asp:BoundField DataField="DOProcessRemarkDate" HeaderText="Process Date" SortExpression="DOProcessRemarkDate" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                </asp:GridView>
            </div>
       </fieldset> 
        <asp:SqlDataSource ID="datasrcDO" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommandType="StoredProcedure" SelectCommand="rptDOProcessingRemark" >
            <SelectParameters>
                <asp:ControlParameter Name="DateFrom" ControlID="txtDateFrom" PropertyName="Text" Type="datetime" />
                <asp:ControlParameter Name="DateTo" ControlID="txtDateTo" PropertyName="Text" Type="datetime"/>
                <asp:ControlParameter Name="RemarkID" ControlID="ddRemark" PropertyName="SelectedValue" />        
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
            </asp:SqlDataSource> 
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>


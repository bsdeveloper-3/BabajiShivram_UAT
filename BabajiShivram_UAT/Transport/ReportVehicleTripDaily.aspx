<%@ Page Title="Vehicle Trip Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ReportVehicleTripDaily.aspx.cs" Inherits="Transport_ReportVehicleTripDaily" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">
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
            <fieldset><legend>Vehicle Trip Report</legend>    
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
                    <td>Date From
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
                    <td>Date To
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
                    <td>Branch</td>
                    <td>
                        <asp:DropDownList ID="ddBranch" runat="server"></asp:DropDownList>
                    </td>
                    <td></td>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <div class="m clear"></div>
            <div>
                <asp:GridView ID="gvTripReport" runat="server" AutoGenerateColumns="True" CssClass="table">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
       </fieldset> 
        <asp:SqlDataSource ID="datasrcTrip" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommandType="StoredProcedure" SelectCommand="TR_rptTripDetailDaily" >
            <SelectParameters>
                <asp:ControlParameter Name="StartDate" ControlID="txtDateFrom" PropertyName="Text" Type="datetime" />
                <asp:ControlParameter Name="ENDDate" ControlID="txtDateTo" PropertyName="Text" Type="datetime"/>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:ControlParameter Name="BranchId" ControlID="ddBranch" PropertyName="SelectedValue" />        
            </SelectParameters>
            </asp:SqlDataSource> 
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>


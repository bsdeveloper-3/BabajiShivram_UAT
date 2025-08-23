<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserReport.aspx.cs" Inherits="Reports_UserReport" 
    MasterPageFile="~/MasterPage.master" Title="BS Group User Report" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">

    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
     
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
    <fieldset>
        <legend>BS Group User Report</legend>    
    <div>
        <div class="fleft">
            <asp:Button ID="btnShowReport" Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
            <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_OnClick" runat="server" CausesValidation="false"/>
        </div>
    <div class="fleft" style="margin-left:30px;">
        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
        </asp:LinkButton>
    </div>
    </div>
    <div class="m clear"></div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>Job Creation Date From
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
            <td>Job Creation Date To
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
            <td>BS Group</td>
            <td>
                <asp:ListBox ID="lstGroupList" runat="server" SelectionMode="Multiple" Height="120px"></asp:ListBox>
            </td>
            <td></td>
            <td>
            </td>
        </tr>
    </table>
    <div class="m clear"></div>
    <div>
        <asp:GridView ID="gvUserReport" runat="server" AutoGenerateColumns="True" CssClass="table">
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
        <asp:SqlDataSource ID="datasrcUserReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommandType="StoredProcedure" SelectCommand="rptMISCustomerwiseStage">
            <SelectParameters>
                <asp:ControlParameter Name="DateFrom" ControlID="txtDateFrom" PropertyName="Text" Type="datetime" />
                <asp:ControlParameter Name="DateTo" ControlID="txtDateTo" PropertyName="Text" Type="datetime"/>
                <asp:Parameter Name="strGroupId" Type="string" DefaultValue="0" />
            </SelectParameters>
            </asp:SqlDataSource> 
    
</asp:Content>
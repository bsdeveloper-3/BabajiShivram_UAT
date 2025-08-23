<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" EnableEventValidation="false"
    Inherits="Reports_Report" MasterPageFile="~/MasterPage.master" Title="Report" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
        TargetControlID="txtFromDate">
    </cc1:CalendarExtender>
     <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
        TargetControlID="txtToDate">
    </cc1:CalendarExtender>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="clear"></div>
    <fieldset>
        <legend>DSR Job Report</legend>    
    <asp:Button ID="btnbtnDownloadExcel" Text="Download To Excel" runat="server" OnClick="btnDownloadExcel_Click" ValidationGroup="validateReport"/>
    <asp:Button ID="btnCancel" Text="Cancel" CssClass="cancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"  />
    
    <div class="m clear"></div>
      <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Customer
                <asp:RequiredFieldValidator ID="RFVCustomer" runat="server" ControlToValidate="ddCustomer" InitialValue="0"
                 ErrorMessage="*" Text="Required" ValidationGroup="validateReport" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="m">
                <asp:DropDownList ID="ddCustomer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                Shipment Type
            </td>
            <td class="m">
                <asp:DropDownList ID="ddShipmentType" runat="server">
                    <asp:ListItem Value="1" Text="Cleared" ></asp:ListItem>
                    <asp:ListItem Value="0" Text="Un-Cleared" ></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Customer Division
            </td>
            <td>
                <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddDivision_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                Customer Plant
            </td>
            <td>
                <asp:DropDownList ID="ddPlant" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Babaji Branch
            </td>
            <td>
                <asp:DropDownList ID="ddBabajiBranch" runat="server"></asp:DropDownList>
            </td>
           <td></td> 
           <td></td>
        </tr>
        <tr>
            <td>
               Out of Charge From Date
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox>
                <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" ValidationGroup="validateReport"
                    Text="Invalid Date" ErrorMessage="Invalid Date." Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck"></asp:CompareValidator>
            </td>
            <td>
               Out of Charge TO Date
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
                <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ValidationGroup="validateReport"
                    Text="Invalid Date" ErrorMessage="Invalid Date." Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck"></asp:CompareValidator>
            </td>
        </tr>
          
    </table>
    </fieldset>
        <div class="m clear"></div>
            <asp:GridView ID="gvReportField" runat="server" AutoGenerateColumns="true" Width="100%"
               CellPadding="4" Cssclass="table">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="InvoiceNoDate" HtmlEncode="False" />--%>
                </Columns>
            </asp:GridView>
        <div id="divSqlDataSource">
            <asp:SqlDataSource ID="ReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="rptCustomerReport" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerId" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddDivision" Name="DivisionId" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddPlant" Name="PlantId" PropertyName="SelectedValue" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddBabajiBranch" Name="BabajiBranchId" PropertyName="SelectedValue" />
                    <%--<asp:Parameter Name="OutOfChargeFromDate" Type="datetime" ConvertEmptyStringToNull="true"/>
                    <asp:Parameter Name="OutOfChargeToDate" Type="datetime" ConvertEmptyStringToNull="true"/>--%>
                    <%--<asp:ControlParameter ControlID="txtFromDate" Name="OutOfChargeFromDate" PropertyName="Text" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txtToDate" Name="OutOfChargeToDate" PropertyName="Text" ConvertEmptyStringToNull="false" />--%>
                    <asp:ControlParameter ControlID="ddShipmentType" Name="DeliveryStatus" PropertyName="SelectedValue" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
                </asp:SqlDataSource>
        </div>
</asp:Content>

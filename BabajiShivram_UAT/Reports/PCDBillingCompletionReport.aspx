<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCDBillingCompletionReport.aspx.cs" Inherits="Reports_PCDBillingCompletionReport" MasterPageFile="~/MasterPage.master" Title="PCA Billing Completion" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div class="m clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fright">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        </asp:Panel>
    </div>
    <div class="m">
    </div>
    <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
        PagerStyle-CssClass="pgr" DataKeyNames="AlertId" AllowPaging="True" AllowSorting="True" Width="100%"
        PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender" DataSourceID="PCDSqlDataSource">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
          
            <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo"/>
            <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" />
            <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
            <asp:BoundField DataField="MAWBNo" HeaderText="MBL/MAWB No" SortExpression="MAWBNo" />
            <asp:BoundField DataField="MAWBDate" HeaderText="MBL/MAWB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MAWBDate" />
            <asp:BoundField DataField="HAWBNo" HeaderText="HBL/HAWB No" SortExpression="HAWBNo" />
            <asp:BoundField DataField="HAWBDate" HeaderText="HBL/HAWB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="HAWBDate" />
            <asp:BoundField DataField="BOENo" HeaderText="BOE No" SortExpression="BOENo" />
            <asp:BoundField DataField="BOEDate" HeaderText="BOE Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BOEDate" />
            <asp:BoundField DataField="IGMNo" HeaderText="IGM No" SortExpression="IGMNo" />
            <asp:BoundField DataField="IGMDate" HeaderText="IGM Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="IGMDate" />
            <asp:BoundField DataField="InvoiceNumber" HeaderText="Customer Invoice No" SortExpression="InvoiceNumber" />
            <asp:BoundField DataField="InvoiceDate" HeaderText="Customer Invoice Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="InvoiceDate" />
            <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
            <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out Of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" />
            <asp:BoundField DataField="VesselName" HeaderText="Vessel Name" SortExpression="VesselName" />
            <asp:BoundField DataField="Supplier" HeaderText="Supplier" SortExpression="Supplier" />
            <asp:BoundField DataField="DeliveryDestination" HeaderText="Delivery Destination" SortExpression="DeliveryDestination" />
            <asp:BoundField DataField="GrossWT" HeaderText="Gross Weight" SortExpression="GrossWT" />
            <asp:BoundField DataField="NoOfPackages" HeaderText="No Of Packages" SortExpression="NoOfPackages" />
            <asp:BoundField DataField="TransportBy" HeaderText="TransportBy" SortExpression="TransportBy" />
            <asp:BoundField DataField ="Port" HeaderText="Port" SortExpression="Port" />
            <asp:BoundField DataField ="ShortDescription" HeaderText="Product Description" SortExpression="ShortDescription" />
            <asp:BoundField DataField="FileSentDt" HeaderText="File Sent to Billing Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FileSentDt" />
            <asp:BoundField DataField="FileComletionDt" HeaderText="File Completion Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FileComletionDt" />
          
        </Columns>
        <PagerTemplate>
            <asp:GridViewPager runat="server" />
        </PagerTemplate>
    </asp:GridView>
    <div>
        <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="rptPCDBillingCompletion" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<%@ Page Title="EWay Tracking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EWayTracking.aspx.cs" Inherits="EWayBill_EWayTracking" EnableEventValidation="false" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
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
        <div align="center" style="vertical-align: top">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="clear"></div>
        <div>
            <fieldset><legend>EWay Bill Detail</legend>
            <div class="m clear">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                 <div class="fleft">
                    &nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ImageAlign="Middle" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    &nbsp;
                </div>
            </div>
            <div class="clear"></div>
        <div>
        <asp:GridView ID="GridViewEWay" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="lid" 
            PagerStyle-CssClass="pgr" PageSize="80" AllowPaging="True" AllowSorting="True" 
            PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="false" DataSourceID="SqlDataSourceBill"
            OnRowCommand="GridViewEWay_RowCommand" OnRowDataBound="GridViewEWay_RowDataBound" OnPreRender="GridViewEWay_PreRender">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" SortExpression="JobRefNo"/>
                <asp:BoundField DataField="EWayBillNo" HeaderText="EWay Bill No" Visible="false"/>
                <asp:TemplateField HeaderText="EWay Bill No" SortExpression="EWayBillNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPrintBill" runat="Server" Text='<%#Eval("EWayBillNo") %>' CommandName="print"
                            CommandArgument='<%#Eval("EWayBillNo") %>' CausesValidation="false" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                <%--<asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName"/>--%>
                <asp:BoundField DataField="DocumentType" HeaderText="Doc Type" SortExpression="DocumentType"/>
                <asp:BoundField DataField="DocumentNo" HeaderText="Doc No" SortExpression="DocumentNo"/>
                <asp:BoundField DataField="DocumentDate" HeaderText="Doc Date" SortExpression="DocumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="FromGSTIN" HeaderText="Bill From" SortExpression="FromGSTIN"/>
                <asp:BoundField DataField="ToGSTIN" HeaderText="Bill To" SortExpression="ToGSTIN"/>
                <asp:BoundField DataField="TransportGSTIN" HeaderText="Transporter" SortExpression="TransportGSTIN"/>
                <asp:BoundField DataField="DeliveryPinCode" HeaderText="PIN" SortExpression="DeliveryPinCode"/>
                <asp:BoundField DataField="DeliveryPlace" HeaderText="Delivery Place" SortExpression="DeliveryPlace"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                <asp:BoundField DataField="rejectStatus" HeaderText="Reject?" SortExpression="RejectStatus"/>
                <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo"/>
                <%--<asp:BoundField DataField="BillGenDate" HeaderText="EWay Bill Date" SortExpression="BillGenDate"/>--%>
                <asp:BoundField DataField="EWayBillDate" HeaderText="EWay Bill Date" SortExpression="EWayBillDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="ValidityDate" HeaderText="Validity Date" SortExpression="ValidityDate"/>
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"/>                        
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager ID="GridViewPager1" runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </div>
        </fieldset>
    </div>
    <div>
        <asp:SqlDataSource ID="SqlDataSourceBill" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetEWayTracking" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


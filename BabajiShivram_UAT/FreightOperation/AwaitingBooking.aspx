<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AwaitingBooking.aspx.cs" Inherits="FreightOperation_AwaitingBooking" 
    MasterPageFile="~/MasterPage.master" Title="Awaiting Booking/Shipment Awarded" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updateOperation" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updateOperation" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset><legend>Awaiting Boooking</legend>
            <div class="m clear">
                <div class="fleft">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                &nbsp;
                </div>
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <div class="fleft">
                   &nbsp;&nbsp;<span style="color:Red;">**</span> <span style="color:Blue;"> Aging: Award Date to Today</span>
                </div>
            </div>
            <div class="clear">
            </div>
            <div>
                <asp:GridView ID="gvFreight" runat="server" DataSourceID="GridViewSqlDataSource" DataKeyNames="EnqId" Width="100%" 
                    AllowPaging="True" PageSize="20" AllowSorting="true" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom"
                    CssClass="table" AutoGenerateColumns="False" OnRowCommand="gvFreight_RowCommand" OnPreRender="gvFreight_PreRender">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enquiry No" SortExpression="ENQRefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewFreight" runat="Server" Text='<%#Eval("ENQRefNo") %>' CommandName="navigate"
                         CommandArgument='<%#Eval("EnqId") %>' CausesValidation="false" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:BoundField DataField="ENQRefNo" HeaderText="Enquiry No" Visible="false" />
                    <%--<asp:BoundField DataField="ENQDate" HeaderText="Enquiry Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ENQDate"/>--%>
                    <%--<asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>--%>
                    <asp:BoundField DataField="StatusDate" HeaderText="Award Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StatusDate"/>
                    <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging"/>
                    <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser"/>
                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                    <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo"/>
                    <%--<asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo"/>--%>
                    <%--<asp:BoundField DataField="AgentName" HeaderText="Agent" SortExpression="AgentName"/>--%>
                    <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName"/>
                    <asp:BoundField DataField="CountOf20" HeaderText="20" SortExpression="CountOf20"/>
                    <asp:BoundField DataField="CountOf40" HeaderText="40" SortExpression="CountOf40"/>
                    <asp:BoundField DataField="LCLVolume" HeaderText="LCL(CBM)" SortExpression="LCLVolume"/>
                    <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages"/>
                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight"/>
                    <%--<asp:BoundField DataField="ChargeableWeight" HeaderText="Charge WT" SortExpression="ChargeableWeight"/>--%>
                    <asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName"/>
                    <%--<asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName"/>--%>
                    <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName"/>
                    <asp:BoundField DataField="LoadingPortName" HeaderText="Loading Port" SortExpression="LoadingPortName"/>
                    <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged" SortExpression="PortOfDischargedName"/>
                    <asp:BoundField DataField="SalesRepName" HeaderText="Sales Rep" SortExpression="SalesRepName"/>
                
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager ID="GridViewPager1" runat="server" />
                </PagerTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FOP_GetAwaitingBooking" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
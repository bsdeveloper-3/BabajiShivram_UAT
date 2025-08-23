<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightTracking.aspx.cs" Inherits="Freight_FreightTracking" 
    MasterPageFile="~/MasterPage.master" Title="Freight Tracking" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
<script src="../JS/jquery-3.1.0.min.js" type="text/javascript"></script>
    <script src="../JS/toastr/toastr.min.js" type="text/javascript"></script>
    <link href="../JS/toastr/toastr.min.css" rel="stylesheet" type="text/css" />
     
    <script type="text/javascript">

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "0",
            "hideDuration": "0",
            "timeOut": "0",
            "extendedTimeOut": "0",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

       // toastr.info("Agent Information Updation Pending! Please Provide Details", "Information");
    </script>
    <div>
<%--<div id="toast"></div>--%>
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
        &nbsp;<asp:Button ID="btnNewEnquiry" runat="server" Text="New Freight Enquiry" OnClick="btnNewEnquiry_Click"/>
    </div>
    <fieldset>
        <legend>Freight Status Detail</legend>
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
                         CommandArgument='<%#Eval("EnqId")+";"+ Eval("TypeName") +";"+ Eval("ModeName")%>' CausesValidation="false" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ENQRefNo" HeaderText="Enquiry No" Visible="false" />
                <asp:BoundField DataField="ENQDate" HeaderText="Enquiry Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ENQDate"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StatusDate"/>
                <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser"/>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo"/>
                <%--<asp:BoundField DataField="AgentName" HeaderText="Agent" SortExpression="AgentName"/>--%>
                <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName"/>
                <asp:BoundField DataField="CountOf20" HeaderText="Count 20" SortExpression="CountOf20"/>
                <asp:BoundField DataField="CountOf40" HeaderText="Count 40" SortExpression="CountOf40"/>
                <asp:BoundField DataField="LCLVolume" HeaderText="LCL(CBM)" SortExpression="LCLVolume"/>
                <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages"/>
                <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight"/>
                <asp:BoundField DataField="ChargeableWeight" HeaderText="Charge WT" SortExpression="ChargeableWeight"/>
                <asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName"/>
                <asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName"/>
                <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName"/>
                <asp:BoundField DataField="LoadingPortName" HeaderText="Loading Port" SortExpression="LoadingPortName"/>
                <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged" SortExpression="PortOfDischargedName"/>
                <asp:BoundField DataField="SalesRepName" HeaderText="Sales Rep" SortExpression="SalesRepName"/>
                <asp:BoundField DataField="Remarks" HeaderText="Remark" SortExpression="Remarks"/>
                <asp:BoundField DataField="EnquiryValue" HeaderText="Enquiry Value" SortExpression="EnquiryValue"/>
                
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="FR_GetFreightForTracking" SelectCommandType="StoredProcedure">
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RejectedBillingAdvice.aspx.cs" Inherits="PCA_RejectedBillingAdvice" 
    MasterPageFile="~/MasterPage.master" Title="Rejected Job" EnableEventValidation="false" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
<script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
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
    <div class="clear"></div>
    <fieldset>
        <legend>Rejected Job</legend>
    <div class="m clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
    <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
        PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="100%"
        PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender" 
        DataSourceID="PCDRejectedSqlDataSource">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No"/>
            <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
            <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
            <asp:TemplateField HeaderText="PCA To Customer" SortExpression="PCDRequired">
                <ItemTemplate>
                    <%# (Boolean.Parse(Eval("PCDRequired").ToString())) ? "Yes" : "No"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out Of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" />
            <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
            <asp:TemplateField HeaderText="Aging" >
               <ItemTemplate>
                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Today –Billing Scrutiny Rejection Date"></asp:Label>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Billing Scrutiny Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField HeaderText="Rejection Remark" DataField="RejectRemark"/>
            <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy"/>
            <asp:BoundField HeaderText="Remark" />
        </Columns>
        <PagerTemplate>
            <asp:GridViewPager runat="server" />
        </PagerTemplate>
    </asp:GridView>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="PCDRejectedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetRejectedPCABillingAdvice" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
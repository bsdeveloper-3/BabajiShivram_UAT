<%@ Page Title="Bill Dispatch Status" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillDispatchStatus.aspx.cs" Inherits="PCA_BillDispatchStatus" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc2" %>
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
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        <div class="clear"></div>
        <fieldset>
        <legend>Bill</legend>
            <div class="clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc2:DataFilter ID="DataFilter2" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <div class="clear"></div>
            <div>
            <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="Billid" AllowPaging="true"
                PagerSettings-Position="TopAndBottom" AllowSorting="True" Width="100%" PageSize="80" 
                OnRowCommand="gvRecievedJobDetail_RowCommand" OnRowDataBound="gvRecievedJobDetail_RowDataBound"
                OnPreRender="gvRecievedJobDetail_PreRender" DataSourceID="SqlDataSourceCustomer">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandArgument='<%#Eval("JobId")%>' CommandName="JobSelect" Enabled="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false"/>
                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                    <asp:BoundField DataField="BillNo" HeaderText="Bill No" SortExpression="BillNo"/>
                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" SortExpression="BillDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" SortExpression="BillAmount" />
                    <asp:BoundField DataField="EBillDate" HeaderText="E-Bill Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EBillDate"/>
                    <asp:BoundField DataField="ClientPortalDate" HeaderText="Portal Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ClientPortalDate"/>
                    <asp:BoundField DataField="ClientPortalRefNo" HeaderText="Portal Ref" SortExpression="ClientPortalRefNo"/>
                    <asp:BoundField DataField="DocketNo" HeaderText="AWB" SortExpression="DocketNo"/>
                    <asp:BoundField DataField="CourierName" HeaderText="Courier" SortExpression="CourierName"/>
                    <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DispatchDate"/>
                    <asp:BoundField DataField="PCDDeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="PCDDeliveryDate"/>
                    <asp:BoundField DataField="BillStatusName" HeaderText="Status" SortExpression="BillStatusName"/>
                    <asp:TemplateField HeaderText="View Bill">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDocument" runat="server" Text="View"
                                CommandName="View" CommandArgument='<%#Eval("BillId")%>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
            </asp:GridView>
            </div>
        </fieldset>
        
        <div id="divDataSource">
            <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BL_GetOpenBillList" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
    
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


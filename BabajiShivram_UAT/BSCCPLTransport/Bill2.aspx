<%@ Page Title="Bill Submission History" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="Bill2.aspx.cs" Inherits="BSCCPLTransport_Bill2" %>
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
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server"></asp:Label>
             </div>
            <fieldset><legend>Customer Billing Advice History - BSCCPL</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter1" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkJobExport" runat="server" OnClick="lnkJobExport_Click">
                        <asp:Image ID="imgExcel_JobDetail" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="m clear"></div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransReqID"
                    DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                    OnRowCommand="gvJobDetail_RowCommand" PagerSettings-Position="TopAndBottom">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View History">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkView" CommandName="select" runat="server" Text='View'
                                    ToolTip="View Bill History" CommandArgument='<%# Eval("TransReqId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" SortExpression="JobRefNo" />
                        <asp:BoundField HeaderText="Customer" DataField="CustName" ReadOnly="true" SortExpression="CustName" />
                        <asp:BoundField HeaderText="Bill Amount" DataField="TotalBillAmount" ReadOnly="true" SortExpression="TotalBillAmount" />
                        <asp:BoundField HeaderText="User" DataField="UserName" ReadOnly="true" SortExpression="UserName" />
                        <asp:BoundField HeaderText="Date" DataField="CreatedDate" ReadOnly="true" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            
                <div>
                <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TRS_GetCustBillPost" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:Parameter Name="CompanyID" DefaultValue="12" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


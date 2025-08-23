<%@ Page Title="Pending Transporter Bill" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="PendingTransBill.aspx.cs" Inherits="BSCCPLTransport_PendingTransBill" %>
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
            <fieldset><legend>Pending Transport Bill - BSCCPL</legend>
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
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransID"
                    DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                    OnRowCommand="gvJobDetail_RowCommand" PagerSettings-Position="TopAndBottom">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Eval("TRRefNo")%>'
                                    ToolTip="Click To Update Bill Detail" CommandArgument='<%# Eval("TransporterId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" ReadOnly="true" Visible="false" />
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" SortExpression="JobRefNo" />
                        <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" SortExpression="Transporter" />
                        <asp:BoundField HeaderText="Customer" DataField="CustName" ReadOnly="true" SortExpression="CustName" />
                        <asp:BoundField HeaderText="Pkg" DataField="SumPackages" ReadOnly="true" Visible="false" />
                        <asp:BoundField HeaderText="20" DataField="Sum20" ReadOnly="true" Visible="false" />
                        <asp:BoundField HeaderText="40" DataField="Sum40" ReadOnly="true" Visible="false" />
                        <asp:BoundField HeaderText="Total Vehicles" DataField="NoofVehicles" ReadOnly="true" Visible="false" />
                        <asp:BoundField HeaderText="Reporting Date" DataField="ReportingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" Visible="false" />
                        <asp:BoundField HeaderText="Unloading Date" DataField="UnloadingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" Visible="false" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            
                <div>
                <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TRS_GetPendingTransBill" SelectCommandType="StoredProcedure">
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


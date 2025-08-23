<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LRDetails.aspx.cs"
    Inherits="Master_LRDetails" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="JobRequired" CssClass="errorMsg" EnableViewState="false" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingLRCopy" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingLRCopy" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <fieldset>
                <legend>LR Details</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:GridView ID="gvLRDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                        OnRowCommand="gvLRDetail_RowCommand" AllowPaging="True" AllowSorting="True"
                        OnPreRender="gvLRDetail_PreRender" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataSourceID="LRDetailsSqlDataSource">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="C.N. No" SortExpression="CNNo">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkJobRefNo" CommandName="detail" runat="server" Text='<%#Eval("CNNo") %>'
                                        CommandArgument='<%#Eval("lid") + "," + Eval("CNNo")  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                           <%--   <asp:TemplateField HeaderText="LR Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Download" CommandName="Download" CommandArgument='<%#Eval("lid") +"," + Eval("CNNo") + "," + Eval("CompId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:BoundField DataField="CNNo" HeaderText="C N No" SortExpression="CNNo" Visible="false"/>
                            <asp:BoundField DataField="CompId" HeaderText="Comp Id" Visible="false" />
                            <asp:BoundField DataField="liableToPay" HeaderText="liable Pay To" Visible="false" />
                            <asp:BoundField DataField="CompanyNm" HeaderText="Company Name" SortExpression="CompanyNm" />
                            <asp:BoundField DataField="LiablePayTo" HeaderText="Liable Pay To" SortExpression="LiablePayTo" Visible="false"/>
                            <asp:BoundField DataField="CNDate" HeaderText="CN Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CNDate" />
                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="InvoiceDate" />
                            <asp:BoundField DataField="LRFrom" HeaderText="LR From" SortExpression="LRFrom" />
                            <asp:BoundField DataField="LRTo" HeaderText="LR To" SortExpression="LRTo" />                           
                            
                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" />
                            <asp:BoundField DataField="OurJobNo" HeaderText="Our Job No" SortExpression="OurJobNo" />

                             <asp:TemplateField HeaderText="LR Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkLRDownload" runat="server" Text="Download" CommandName="Download" CommandArgument='<%#Eval("lid") +"," + Eval("CNNo") + "," + Eval("CompId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="LRDetailsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetLRDetailsForUser" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


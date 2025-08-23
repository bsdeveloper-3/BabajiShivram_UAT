<%@ Page Title="My Pacco Order List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MyPaccoOrder.aspx.cs" Inherits="PCA_MyPaccoOrder" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
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
                <fieldset class="fieldset-AutoWidth">
                    <legend>My Pacco Order List</legend>
                    <div class="clear"></div>
                    <div>
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:datafilter id="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" OnPreRender="gvOrderDetail_PreRender" OnRowCommand="gvOrderDetail_RowCommand"
                            DataKeyNames="AWBId" DataSourceID="OrderSqlDataSource" AllowPaging="True" AllowSorting="True"
                            PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order No" SortExpression="OrderNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOrderNo" CommandName="select" runat="server" Text='<%#Eval("OrderNo") %>'
                                            CommandArgument='<%#Eval("AWBId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="OrderNo" HeaderText="Order No" Visible="false" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                                <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StatusDate"/>
                                <asp:BoundField DataField="AWBNo" HeaderText="AWB No" SortExpression="AWBNo"/>
                                <asp:BoundField DataField="AWBDate" HeaderText="AWB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="AWBDate" />
                                <asp:BoundField DataField="LSPName" HeaderText="LSP Name" SortExpression="LSPName" />
                                <asp:BoundField DataField="FromCity" HeaderText="From" SortExpression="FromCity" />
                                <asp:BoundField DataField="ToCity" HeaderText="To Location" SortExpression="ToCity" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" />
                                <asp:TemplateField HeaderText="Print">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrintAWB" CommandName="PrintAWB" runat="server" Text="Print" 
                                            CommandArgument='<%#Eval("OrderNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check AWB">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkGetAWB" CommandName="CheckAWB" runat="server" Text="Check Status" 
                                            CommandArgument='<%#Eval("OrderNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="OrderSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="MP_GetMyPaccoOrder" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div id="Job_Popup">
                <asp:LinkButton ID="lnkModelPopup21" runat="server" />
                    <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkModelPopup21"
                        PopupControlID="Panel21">
                    </AjaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="Panel21" Style="display: none" runat="server">
                        <fieldset>
                            <legend>JOB DETAIL</legend>
                            <div class="header">
                                    <div class="fright">
                                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server"
                                            OnClick="btnCancelPopup_Click" ToolTip="Close" />
                                    </div>
                                </div>
                            <div class="clear"></div>
                            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                                <asp:GridView ID="gvJobBasic" runat="server" CssClass="table" AutoGenerateColumns="false" AllowPaging="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" />
                                        <asp:BoundField DataField="CustName" HeaderText="Customer" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


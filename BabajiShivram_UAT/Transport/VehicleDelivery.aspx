<%@ Page Title="Vehicle Delivery" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleDelivery.aspx.cs"
    Inherits="Transport_VehicleDelivery" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblMessage.ClientID%>').className = '';
        }

        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "Images/minus.png";
                img.title = 'Collapse';
            }
            else {
                div.style.display = "none";
                img.src = "Images/plus.png";
                img.title = 'Expand';
            }
        }
    </script>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <fieldset>
                <legend>Vehicle Delivery</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="imgExcel1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="lId" AllowPaging="True" AllowSorting="True" Width="100%"
                    PageSize="20" PagerSettings-Position="TopAndBottom" OnRowCommand="gvJobDetail_RowCommand"
                    OnPreRender="gvJobDetail_PreRender" DataSourceID="DataSourcePendingDelivery">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TR Ref No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                 <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("TRRefNo") %>'
                                    CommandArgument='<%#Eval("lId")+ ";" + Eval("Moduleid") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TRRefNo" HeaderText="TR Ref No" Visible="false" />
                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo" />
                        <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
                        <asp:BoundField DataField="Division" HeaderText="Division" SortExpression="Division" />
                        <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" />
                        <asp:BoundField DataField="TransMode" HeaderText="TransMode" SortExpression="TransMode" />
                        <asp:BoundField DataField="LocationFrom" HeaderText="From" SortExpression="LocationFrom" />
                        <asp:BoundField DataField="Destination" HeaderText="To" SortExpression="Destination" />
                        <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" SortExpression="DeliveryType" />
                        <asp:BoundField DataField="NoOfPkgs" HeaderText="Pkgs" SortExpression="NoOfPkgs" />
                        <asp:BoundField DataField="Count20" HeaderText="Count20" SortExpression="Count20" />
                        <asp:BoundField DataField="Count40" HeaderText="Count40" SortExpression="Count40" />
                        <asp:BoundField DataField="GrossWeight" HeaderText="Gross Wt" SortExpression="GrossWeight" />
                        <asp:BoundField DataField="RequestedBy" HeaderText="Created By" SortExpression="RequestedBy" />
                        <asp:BoundField DataField="dtDate" HeaderText="Created On" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <div>
                    <asp:SqlDataSource ID="DataSourcePendingDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetPendingVehicleDelivery" SelectCommandType="StoredProcedure">
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


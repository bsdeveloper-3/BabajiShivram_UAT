<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FreightMISStage.aspx.cs" 
    Inherits="Freight_FreightMISStage" Title="Month wise status" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div class="clear"></div>
    <asp:UpdatePanel ID="upReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset><legend>MIS - Monthly Status</legend>
                <div class="m clear">
                <div class="fleft">
                <asp:LinkButton ID="lnkExportStage" runat="server" OnClick="lnkExportStage_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                &nbsp;
            </div>
                </div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Report Mode
                        </td>
                        <td>
                            <asp:DropDownList ID="ddMode" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="-ALL-" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="-AIR-"></asp:ListItem>
                                <asp:ListItem Value="2" Text="-SEA-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Customer
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomer" CssClass="SearchTextbox" Width="80%" runat="server" placeholder="Search" 
                                TabIndex="3" MaxLength="100" AutoPostBack="true"></asp:TextBox>
                            <div id="divwidthCust">
                            </div>
                            <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                CompletionListElementID="divwidthCust" ServicePath="../WebService/FreightCustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust" 
                                ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true"></cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                </table>
                <div>
                <div>
                    <asp:GridView ID="gvStageReport" runat="server" AutoGenerateColumns="True" CssClass="table" 
                        DataSourceID="DataSourceStage">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset> 
            
            <asp:SqlDataSource ID="DataSourceStage" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommandType="StoredProcedure" SelectCommand="FR_MISStatusMonthwise" >
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddMode" Name="Mode" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="txtCustomer" Name="CustomerName" PropertyName="Text" ConvertEmptyStringToNull="false" />
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource> 
            
            <fieldset id="fs2" runat="server" visible="false"><legend>MIS - Customer Status</legend>
                <div class="m clear">
                <div class="fleft">
                <asp:LinkButton ID="lnkExportCustomer" runat="server" OnClick="lnkExportCustomer_Click">
                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                &nbsp;
                </div>
                </div>
                <div>
                    <asp:GridView ID="gvCustomerReport" runat="server" AutoGenerateColumns="False" CssClass="table">
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField HeaderText="Status" DataField="Status" />
                            <asp:BoundField HeaderText="Apr" DataField="Apr" />
                            <asp:BoundField HeaderText="May" DataField="May" />
                            <asp:BoundField HeaderText="Jun" DataField="Jun" />
                            <asp:BoundField HeaderText="Jul" DataField="Jul" />
                            <asp:BoundField HeaderText="Aug" DataField="Aug" />
                            <asp:BoundField HeaderText="Sep" DataField="Sep" />
                            <asp:BoundField HeaderText="Oct" DataField="Oct" />
                            <asp:BoundField HeaderText="Nov" DataField="Nov" />
                            <asp:BoundField HeaderText="Dec" DataField="Dec" />
                            <asp:BoundField HeaderText="Jan" DataField="Jan" />
                            <asp:BoundField HeaderText="Feb" DataField="Feb" />
                            <asp:BoundField HeaderText="Mar" DataField="Mar" />
                            <asp:BoundField HeaderText="Total" DataField="Total" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset> 
            
            <asp:SqlDataSource ID="DataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommandType="StoredProcedure" SelectCommand="FR_MISCustomerMonthwise" >
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource> 
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FreightMISProfitCustomer.aspx.cs" 
    Inherits="Freight_FreightMISProfitCustomer" Title="Customer Wise Profit" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFreightReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div class="clear"></div>
    <asp:UpdatePanel ID="upFreightReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset><legend>CUSTOMER - Buy / Sell</legend>
                <div>
                    <asp:DropDownList ID="ddMode" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="0" Text="-All Mode-"></asp:ListItem>
                    <asp:ListItem Value="1" Text="AIR"></asp:ListItem>
                    <asp:ListItem Value="2" Text="SEA"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="lnkExportProfit" runat="server" OnClick="lnkExportProfit_Click"> 
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                </div>
                <div>
                    <asp:GridView ID="gvProfit" runat="server" AutoGenerateColumns="False" CssClass="table"
                        DataKeyNames="CustID" OnRowCommand="gvProfit_RowCommand" DataSourceID="DataSourceProfit">
                        <Columns>
                            <asp:templatefield headertext="Sl">
                                <itemtemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </itemtemplate>
                            </asp:templatefield>
                            <asp:TemplateField HeaderText="Customer">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCustomer" runat="server" Text='<%#Eval("Customer") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";0" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAPR" runat="server" Text='<%#Eval("APR") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";4" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="May">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkMay" runat="server" Text='<%#Eval("MAY") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";5" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkJun" runat="server" Text='<%#Eval("JUN") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";6" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="July">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkJuly" runat="server" Text='<%#Eval("JUL") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";7" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Aug">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAug" runat="server" Text='<%#Eval("AUG") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";8" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSep" runat="server" Text='<%#Eval("Sep") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";9" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Oct">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOct" runat="server" Text='<%#Eval("Oct") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";10" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNov" runat="server" Text='<%#Eval("Nov") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";11" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dec">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDec" runat="server" Text='<%#Eval("Dec") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";12" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkJan" runat="server" Text='<%#Eval("Jan") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";1" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkFeb" runat="server" Text='<%#Eval("Feb") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";2" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkMar" runat="server" Text='<%#Eval("Mar") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";3" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTotal" runat="server" Text='<%#Eval("Total") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";0" %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset> 
            
            <div class="clear"> </div>
            <fieldset><legend>Freight Detail</legend>        
            <!-- Aging Job Detail -->
    
            <asp:Panel ID="pnlProfitDetailXLS" runat="server" Visible="false">
            <asp:LinkButton ID="lnkProfitDetailXls" runat="server" OnClick="lnkProfitDetailXls_Click"> 
                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            </asp:Panel>
            <div class="clear">
            </div>
            <asp:GridView ID="gvUserDetail" runat="server" AutoGenerateColumns="False" CssClass="table" 
                PagerStyle-CssClass="pgr" AllowPaging="True" Width="99%" PageSize="20" PagerSettings-Position="TopAndBottom"
                 OnPreRender="gvUserDetail_PreRender" DataSourceID="ProfitDetailSqlDataSource" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderText="Sl" >
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="SPC" DataField="SPC" SortExpression="SPC" />
                    <asp:BoundField HeaderText="Mode" DataField="TransMode" SortExpression="TransMode" />
                    <asp:BoundField HeaderText="Customer" DataField="Customer" SortExpression="Customer" />
                    <asp:BoundField HeaderText="Job No" DataField="FRJOBNo" SortExpression="FRJOBNo" />
                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" SortExpression="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="CAN Amount" DataField="CANAmount" SortExpression="CANAmount" />
                    <asp:BoundField HeaderText="Invoice Amount" DataField="AgentInvoiceAmount" SortExpression="AgentInvoiceAmount" />
                    <asp:BoundField HeaderText="Profit" DataField="Profit" SortExpression="Profit" />
                </Columns>
                <PagerTemplate>
                <asp:GridViewPager ID="GridViewPager1" runat="server" />
            </PagerTemplate>
            </asp:GridView>
            </fieldset>
            <div>

            <asp:SqlDataSource ID="DataSourceProfit" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommandType="StoredProcedure" SelectCommand="FR_MISProfitCust" >
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddMode" Name="ModeID" PropertyName="SelectedValue" />
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource> 

            <asp:SqlDataSource ID="ProfitDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FR_MISProfitCustDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                <asp:Parameter Name="CustId" Type="int32" />
                <asp:Parameter Name="MonthId" Type="int32" />
                <asp:ControlParameter ControlID="ddMode" Name="ModeID" PropertyName="SelectedValue" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>


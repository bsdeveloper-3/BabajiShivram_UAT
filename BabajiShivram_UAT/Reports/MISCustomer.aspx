<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MISCustomer.aspx.cs" Inherits="Reports_MISCustomer" 
 MasterPageFile="~/MasterPage.master" Title="MIS Customer" EnableEventValidation="false"%>
 <%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false"></asp:Label>
            </div>
            <div>
                <asp:DropDownList ID="ddContainerType" runat="server" AutoPostBack="true" Visible="false">
                    <asp:ListItem Value="0" Text="ALL"></asp:ListItem>
                    <asp:ListItem Value="1" Text="FCL"></asp:ListItem>
                    <asp:ListItem Value="2" Text="LCL"></asp:ListItem>
                    <asp:ListItem Value="3" Text="AIR"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clear">
            </div>
        <fieldset>
        <legend>MIS Customer</legend>    
            <div>
                <div class="fleft">
                    <asp:LinkButton ID="lnkCustomerXls" runat="server" OnClick="lnkCustomerXls_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>&nbsp;
                </div>
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                
            </div>
            <div class="clear"> </div>
            <asp:GridView ID="gvCustomerWiseJob" runat="server" AutoGenerateColumns="False" DataKeyNames="CustomerId,TransModeId" 
                CssClass="table" OnPreRender="gvCustomerWiseJob_PreRender" DataSourceID="MISCustomerSqlDataSource" 
                OnSelectedIndexChanged="gvCustomerWiseJob_SelectedIndexChanged" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCustomer" runat="server" Text='<%#Eval("Customer") %>' CommandName="select" CommandArgument='<%#Eval("CustomerId") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Customer" DataField="Customer" Visible="false" />
                <asp:BoundField HeaderText="Type" DataField="Type" SortExpression="Type" />
                <asp:BoundField HeaderText="NO OF JOBS" DataField="NoofJobsAir" SortExpression="NoofJobsAir"/>
                <asp:BoundField HeaderText="NO OF JOBS - FCL" DataField="NOOFJOBSFCL" SortExpression="NOOFJOBSFCL"/>
                <asp:BoundField HeaderText="NO OF JOBS - LCL" DataField="NOOFJOBSLCL" SortExpression="NOOFJOBSLCL"/>
                <asp:BoundField HeaderText="NO OF CONT - 20" DataField="NOOFCONT20" SortExpression="NOOFCONT20"/>
                <asp:BoundField HeaderText="NO OF CONT - 40" DataField="NOOFCONT40" SortExpression="NOOFCONT40"/>
                <asp:BoundField HeaderText="TEU" DataField="TEU" SortExpression="TEU" />
                <asp:BoundField HeaderText="GROSS WEIGHT" DataField="GrossWeight" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField HeaderText="NO OF PKGS" DataField="NoOfPKGS" ItemStyle-HorizontalAlign="Right" />
            </Columns>
        </asp:GridView>
        </fieldset>
        <div>
        <asp:SqlDataSource ID="MISCustomerSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            DataSourceMode="DataSet" EnableCaching="true" CacheDuration="300"
            SelectCommand="rptMISCustomerwise" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <%--<asp:ControlParameter ControlID="ddContainerType" Name="ContainerType" PropertyName="SelectedValue" />--%>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
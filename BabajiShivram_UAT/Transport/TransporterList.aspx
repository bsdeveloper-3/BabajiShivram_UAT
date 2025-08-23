<%@ Page Title="Transporter List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransporterList.aspx.cs" 
        Inherits="Transport_TransporterList" EnableEventValidation="false" %> 

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
       
    <fieldset>
        <legend>Transporter Detail</legend>
    <div>
    <!-- Filter Content Start-->
	<div class="fleft">
        <uc1:DataFilter ID="DataFilter1" runat="server" />
    </div>
    <!-- Filter Content END-->
    <div class="fleft">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
        </asp:LinkButton>
    </div>
    </div>
    <div class="clear"></div>
    <div>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="GridViewSqlDataSource"
            DataKeyNames="lid" Width="100%" AllowPaging="True" PageSize="20" AllowSorting="true" 
            CssClass="table" OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False"
            PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom" OnPreRender="GridView1_PreRender">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company Name" SortExpression="CustName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkcustname" runat="Server" Text='<%#Eval("CustName") %>' CommandName="navigate"
                         CommandArgument='<%#Eval("lId") %>' CausesValidation="false" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CustName" HeaderText="Transporter Name" Visible="false" />
		        <asp:BoundField DataField="CustCode" HeaderText="Code" SortExpression="CustCode"/>
                <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" SortExpression="ContactPerson" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" />
		        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" Visible="false" />
		        <asp:BoundField DataField="ReferredBy" HeaderText="Referred By" SortExpression="ReferredBy" Visible="false" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />  
                <asp:BoundField DataField="dtDate" HeaderText="Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
                <%--<asp:TemplateField HeaderText="Add Branch">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbranchdetail" runat="server" CommandArgument='<%#Eval("lid") %>'
                            CommandName="Navigate" CausesValidation="False">Add New Branch</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetCustomerMSByType" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="CustomerTypeID" DefaultValue="6" DbType="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FaqDetails.aspx.cs"
     Inherits="FAQ_UpdateFaqDetails" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <fieldset>
    <legend>FAQ  Details </legend>
        <center><asp:Label ID="lblerror" runat="server" ></asp:Label></center>
        <br />   <br />
         <!-- Filter Content Start-->
        <div class="fleft">
        <uc1:DataFilter ID="DataFilter1" runat="server" />
        </div>
        <br /> <br />
    <!-- Filter Content END-->
    <asp:GridView ID="GridFaqDetails" runat="server" AutoGenerateColumns="False"
        Width="100%" PagerStyle-CssClass="pgr" DataSourceID="FAQDetailDataSorce"
        OnRowCommand="GridFaqDetails_RowCommand" CssClass="table" CellPadding="4"
        AllowPaging="True" AllowSorting="True" PageSize="20" DataKeyNames="LId" Style="white-space: normal">
        <Columns>
              <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
             <asp:BoundField DataField="sName" HeaderText="Service" SortExpression="sName" />
            <asp:BoundField DataField ="FAQCreatedBy" HeaderText="FAQ Created By" SortExpression="FAQCreatedBy" />
             <asp:BoundField DataField="dtDate" HeaderText="FAQ Created Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
             <asp:BoundField DataField="FAQUpdatedBy" HeaderText="FAQ Updated By" SortExpression="FAQUpdatedBy" />
             <asp:BoundField DataField="updDate" HeaderText="FAQ Updated Date" SortExpression="updDate" DataFormatString="{0:dd/MM/yyyy}"  />
            <asp:TemplateField HeaderText="Update FAQ">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="select" Text="Update"
                        CommandArgument='<%#Eval("lId")%>' />  
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remove FAQ">
                <ItemTemplate>
                     <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete"  CausesValidation="false"
                   runat="server" Text="Remove" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove  FAQ Report Field ?')"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="FAQDetailDataSorce" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetFAQDeatils" SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure"
            OnDeleted="DataSourceFAQDetail_Deleted" DeleteCommand="delFAQDetail">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
             <DeleteParameters>
                <asp:Parameter Name="lid" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="outPut" Direction="outPut" Size="4" />
                </DeleteParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>


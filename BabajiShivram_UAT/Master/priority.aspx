<%@ Page Title="Priority Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="priority.aspx.cs" Inherits="Master_priority" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
<div>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPriorityMaster" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../images/processing.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>

<asp:UpdatePanel ID="upPriorityMaster" runat="server" UpdateMode="Conditional" RenderMode="Inline">
  <contenttemplate>
    <fieldset id="Fieldset1" runat="server" style="width:80%"><legend>Priority Detail</legend>
      <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:ValidationSummary ID="ValSummaryPriority" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="PriorityRequired" CssClass="errorMsg" EnableViewState="false" />
        </div>
       <asp:GridView ID="grvPriority" runat="server" DataSourceID="datasourcePriority" AutoGenerateColumns="false" CssClass="table"
            DataKeyNames="lid" PagerStyle-CssClass="pgr" AllowSorting="True" 
            OnRowEditing="grvPriority_RowEditing" OnRowUpdating="grvPriority_RowUpdating"> 
        <Columns>
           <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton  ID="lnkUpdate" CommandName="Update" CommandArgument='<%#Eval("lid") %>' ToolTip="Update" Width="45" runat="server" Text="Update" Font-Underline="true" ValidationGroup="PriorityRequired"></asp:LinkButton>
                <asp:LinkButton  ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false" runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
            </EditItemTemplate>
            </asp:TemplateField>
                
                <asp:BoundField DataField="Priority"  HeaderText="Priority"  SortExpression="Priority" ReadOnly="true" />

            <asp:TemplateField HeaderText="Level">
                <ItemTemplate>
                    <asp:Label ID="lblPriorityID" runat="server" Text='<%#BIND("lOrder") %>' ></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPriorityID" runat="server"   Text='<%#Bind("lOrder") %>' MaxLength="1" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Refv" runat="server" ControlToValidate="txtPriorityID" ErrorMessage="Please Enter Priority" ValidationGroup="PriorityRequired"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Regexpression1"  runat="server" ControlToValidate="txtPriorityID" ErrorMessage="Please Enter only numeric value" ValidationExpression='^[0-9]$' ValidationGroup="PriorityRequired"></asp:RegularExpressionValidator>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>        
    </fieldset>
    <asp:SqlDataSource ID="datasourcePriority" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="GetPriorityMs" SelectCommandType="StoredProcedure"  
        OnUpdated="datasourcePriority_Updated" UpdateCommand="updbillingPriority" UpdateCommandType="StoredProcedure">
        <UpdateParameters>
            <asp:Parameter Name="lId" Type="int32"/>
            <asp:Parameter Name="lOrder" Type="int32"/> 
            <asp:SessionParameter Name="lUserId" SessionField="UserId" />
            <asp:Parameter Name="OutPut" Type="int32" Direction="Output" Size="4" />
        </UpdateParameters>
        </asp:SqlDataSource>     
   </contenttemplate>
</asp:UpdatePanel>
</asp:Content>


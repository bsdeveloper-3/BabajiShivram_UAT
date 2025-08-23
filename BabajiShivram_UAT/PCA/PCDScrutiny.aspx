<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCDScrutiny.aspx.cs" Inherits="PCDScrutiny" 
    MasterPageFile="~/MasterPage.master" Title="PCA Scrutiny" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript" language="javascript">
    function ConfirmSubmit()
    {
        if(confirm('Are you sure wants to Approve?'))
        {
            
        }
        else
        {
            return false;
        }
    }
    </script>
     
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
        <div class="m clear">
        <asp:Button ID="btnApprove" Text="Approve" runat="server" ValidationGroup="Required" OnClick="btnApprove_Click" 
            OnClientClick="if(Page_ClientValidate()) return ConfirmSubmit(); return false;"/>
        <asp:Button ID="btnReject" Text="Reject" runat="server" ValidationGroup="Required" OnClick="btnReject_Click" 
        OnClientClick="if(Page_ClientValidate()) return confirm('Are you sure wants to Reject?'); return false"/>
        <asp:Button ID="btnCancel" Text="Cancel" CssClass="btn" CausesValidation="false" runat="server" OnClick="btnCancel_Click" />
    </div>
    <fieldset>
        <legend>PCA Scrutiny</legend>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                BS Job No.
            </td>
            <td>
                <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
            </td>
            <td>
                Cust Ref No.
            </td>
            <td>
                <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Customer Name
            </td>
            <td>
                <asp:Label ID="lblCustName" runat="server"></asp:Label>
            </td>
            <td>
                PCA TO Customer
            </td>
            <td>
                <asp:Label ID="lblPCDToCustomer" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Documents forwarded on 
            </td>
            <td>
                <asp:Label ID="lblAdvicedate" runat="server"></asp:Label>
            </td>
            <td>
                Name of Person forwarded
            </td>
            <td>
                <asp:Label ID="lblPersonName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
              Remark
              <asp:RequiredFieldValidator ID="RFV05" runat="server" ControlToValidate="txtRemark" Text="*"
                SetFocusOnError="true"  Display="Dynamic"  ErrorMessage="Please Enter Remark" ValidationGroup="Required"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
            </td>
        </tr>    
        
    </table>
    </fieldset>
    <fieldset>
        <legend>Scrutiny History</legend>
    
        <asp:GridView ID="gvScrutinyHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
            Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId"
            DataSourceID="DataSourceScrutinyHistory" CellPadding="4" AllowPaging="True"
            AllowSorting="True" PageSize="40">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Status" DataField="Status" />
                <asp:BoundField HeaderText="Requested By" DataField="RequestedBy" />
                <asp:BoundField HeaderText="Request Date" DataField="RequestedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                <asp:BoundField HeaderText="Request Remark" DataField="RequestRemark" />
                <asp:BoundField HeaderText="Authorised By" DataField="AuthorisedBy" />
                <asp:BoundField HeaderText="Authorise Date" DataField="AuthorisedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                <asp:BoundField HeaderText="Author Remark" DataField="AuthorRemark" />
            </Columns>
        </asp:GridView>
    
    </fieldset>
    <div class="clear"></div>
    <fieldset>
        <legend>Billing Advice Document</legend>  
        <asp:GridView ID="gvPCDDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
            Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True" 
            AllowSorting="True" PageSize="20" DataSourceID="PCDDocumentSqlDataSource" 
            OnRowDataBound="gvPCDDocument_RowDataBound" OnRowCommand="gvPCDDocument_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DocumentName" HeaderText="PCA Document" SortExpression="DocumentName" />
                <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" SortExpression="UploadedBy" />
                <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                <asp:TemplateField HeaderText="Download">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                            CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>    

    <div id="divDatasource">
        <asp:SqlDataSource ID="DataSourceScrutinyHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetPCDScrutinyApprovalHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="JobId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                <asp:Parameter Name="DocumentForType" DefaultValue="3" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
</asp:Content>
<%@ Page Title="Bill status Reports" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillStatusReport.aspx.cs" Inherits="Reports_BillStatusReport"  Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upBillPendingReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
     <asp:UpdatePanel ID="upBillPendingReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div>
                <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateFrom" PopupPosition="BottomRight"
                    TargetControlID="txtDateFrom">
                </cc1:CalendarExtender>
                <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateTo" PopupPosition="BottomRight"
                    TargetControlID="txtDateTo">
                </cc1:CalendarExtender>
            </div>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset  style="width:80%" ><legend>Bill Balance Report</legend>    
             <div class="fright">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                </div>
            
              <center>
             <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">                   
                <th colspan="4" class="heading">Bill Balance Report</th>
                <tr>
                    <td>Date From
                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td>Date To
                        <asp:RequiredFieldValidator ID="RFVDateTo" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateTo" ErrorMessage="Please Enter TO Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValDateTo" runat="server" ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid To Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                </tr>      
                <tr>
                    <td>Branch</td>
                    <td>
                        <asp:DropDownList ID="Drpbranch" runat="server"   AppendDataBoundItems="True">    
                        <asp:ListItem  Selected="True" Value="0" Text="All"></asp:ListItem>                  
                        </asp:DropDownList>
                    </td>
                    <td>Customer</td>
                    <td>
                        <asp:DropDownList ID="DrpCustomer" runat="server"  AppendDataBoundItems="True">                   
                        <asp:ListItem  Selected="True" Value="0" Text="All"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>              
                                              
                <tr><td align="center" colspan="4"><asp:Button ID="btnShowReport"  Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" Text="Cancel" OnClick="Cancel_OnClick" runat="server" ValidationGroup="Required" /></td>
                </tr>
            </table>
            </center>
           
            <div>
                <asp:GridView ID="gvbillReport" runat="server" AutoGenerateColumns="True" CssClass="gridview" AllowPaging="false">
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
       <asp:SqlDataSource ID="Dsstage" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>" 
       SelectCommandType="StoredProcedure" SelectCommand="GetBillStages">       
       </asp:SqlDataSource>      
       <asp:SqlDataSource ID="DsAllBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>" 
       SelectCommandType="StoredProcedure" SelectCommand="GetAllBranch">              
       </asp:SqlDataSource>    
       <asp:SqlDataSource ID="DsCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>" 
       SelectCommandType="StoredProcedure" SelectCommand="GetCustomerMS">       
       </asp:SqlDataSource>    
       <asp:SqlDataSource ID="DsbillReports" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>" 
       SelectCommandType="StoredProcedure" SelectCommand="rptBillingStatusReports">
       <SelectParameters>
      <asp:ControlParameter Name="FromDate" ControlID="txtDateFrom" PropertyName="Text" Type="DateTime" />
      <asp:ControlParameter Name="ToDate" ControlID="txtDateTo" PropertyName="Text" Type="DateTime"/>          
      <asp:ControlParameter Name="Branch" ControlID="Drpbranch" PropertyName="SelectedValue" Type="int16"/>    
      <asp:ControlParameter Name="Customer" ControlID="DrpCustomer" PropertyName="SelectedValue" Type="int16"/>                                                

      </SelectParameters>       

       </asp:SqlDataSource>                  
        </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>

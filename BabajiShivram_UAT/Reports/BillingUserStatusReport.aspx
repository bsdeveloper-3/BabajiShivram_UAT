<%@ Page Title="Billing User Status Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillingUserStatusReport.aspx.cs" Inherits="Reports_BillingUserStatusReport"  Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
<script type="text/javascript">
    function OnUserSelected(source, eventArgs) {
        var results = eval('(' + eventArgs.get_value() + ')');
        $get('<%=hdnUserId.ClientID%>').value = results.Userid;
    }
    $addHandler
    (

        $get('<%=txtUser.ClientID%>'), 'keypress',

        function () {
            $get('ctl00_ContentPlaceHolder1_hdnUserId').value = '0';
        }
    );

    </script>
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
<%--                <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateTo" PopupPosition="BottomRight"
                    TargetControlID="txtDateTo1">
                </cc1:CalendarExtender>
--%>            </div>
                  
          
            <fieldset style="width:80%"><legend>User Status Report</legend>    
              <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
              <br/>
              <div class="fright">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
               
                </div>            
              <center>
              <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">                   
                <tr>
                    <th class="heading" colspan="4">
                        User Status Report</th></tr>
                    <tr>
                        <td>
                            <div ID="divUnderClear">
                                <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
                                <asp:TextBox ID="txtUser" runat="server" AutoPostBack="true" MaxLength="100" 
                                    OnTextChanged="txtUser_TextChanged" placeholder="User Name" Width="30%"></asp:TextBox>
                                <div ID="divwidthCust">
                                </div>
                                <cc1:AutoCompleteExtender ID="UserExtender" runat="server" 
                                    BehaviorID="divwidthCust" CompletionListCssClass="AutoExtender" 
                                    CompletionListElementID="divwidthCust" 
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                    CompletionListItemCssClass="AutoExtenderList" ContextKey="4317" 
                                    FirstRowSelected="true" MinimumPrefixLength="2" 
                                    OnClientItemSelected="OnUserSelected" ServiceMethod="GetUserCompletionList" 
                                    ServicePath="~/WebService/UserAutoComplete.asmx" TargetControlID="txtUser" 
                                    UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                            </div>
                        </td>
                        <td>
                            <asp:DropDownList ID="drp1" runat="server"  AutoPostBack="true"
                                onselectedindexchanged="drp1_SelectedIndexChanged">
                                <asp:ListItem Value="0">Day</asp:ListItem>
                                <%--<asp:ListItem Value="1">Weekly</asp:ListItem>--%>
                                <asp:ListItem Value="2">Monthly</asp:ListItem>
                                <asp:ListItem Value="3">Yearly</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="td_Day" runat="server" colspan="2"> Date
                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>

                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>

                        <asp:TextBox ID="txtDateFrom" runat="server" Width="100px"></asp:TextBox>

                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                          </td>
                        <td id="td_Week" runat="server" visible="false" colspan="2">
                          Date From
                        <asp:RequiredFieldValidator ID="RFVFomDate1" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateFrom1" ErrorMessage="Please Enter From Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>

                        <asp:CompareValidator ID="ComValFromDate1" runat="server" ControlToValidate="txtDateFrom1" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>
                        <asp:TextBox ID="txtDateFrom1" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="Image2" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Date To
                        <asp:RequiredFieldValidator ID="RFVToDate1" ValidationGroup="Required" runat="server"
                        Text="*" ControlToValidate="txtDateTo1" ErrorMessage="Please Enter To Date"
                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>

                        <asp:CompareValidator ID="ComValToDate1" runat="server" ControlToValidate="txtDateTo1" Display="Dynamic" ValidationGroup="Required"
                        ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>
                        <asp:TextBox ID="txtDateTo1" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="Image3" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                          </td>
                        <td id="td_Month" runat="server" visible="false"> Month
                          <asp:DropDownList Id="drpMonth" runat="server">
                          <asp:ListItem Value=1>JAN</asp:ListItem>
                          <asp:ListItem Value=2>FEB</asp:ListItem>
                          <asp:ListItem Value=3>MAR</asp:ListItem>
                          <asp:ListItem Value=4>APR</asp:ListItem>
                          <asp:ListItem Value=5>MAY</asp:ListItem>
                          <asp:ListItem Value=6>JUN</asp:ListItem>
                          <asp:ListItem Value=7>JUL</asp:ListItem>
                          <asp:ListItem Value=8>AUG</asp:ListItem>
                          <asp:ListItem Value=9>SEP</asp:ListItem>
                          <asp:ListItem Value=10>OCT</asp:ListItem>
                          <asp:ListItem Value=11>NOV</asp:ListItem>
                          <asp:ListItem Value=12>DEC</asp:ListItem>
                          </asp:DropDownList>
                          </td>
                        <td id="td_Year" runat="server" visible="false"> Year
                          <asp:DropDownList Id="drpYear" runat="server">
                          <asp:ListItem Value="2014">2014</asp:ListItem>
                          <asp:ListItem Value="2015">2015</asp:ListItem>
                          <asp:ListItem Value="2016">2016</asp:ListItem>
                          <asp:ListItem Value="2017">2017</asp:ListItem>
                          </asp:DropDownList>
                          </td>
                       </tr>
                   <tr>
                   <td align="center" colspan="4"><asp:Button ID="btnShowReport"  Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" Text="Cancel" OnClick="Cancel_OnClick" runat="server" ValidationGroup="Required" /></td>
                </tr>
         
         </tr>   
                                                                                          
            </table>
              <div>
             <asp:GridView ID="gvUserStatusReport" runat="server"  AutoGenerateColumns="true" 
                CssClass="gridview" AllowPaging="true">                  
                </asp:GridView>
            </div>
              </center>       
            </fieldset>        
       <asp:SqlDataSource ID="DsUserFileDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>" 
       SelectCommandType="StoredProcedure" SelectCommand="rptUserFileReports">       
       <SelectParameters>
       <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserId" />                 
      <asp:ControlParameter Name="Time"  ControlID="drp1" PropertyName="SelectedValue"/>    
      <asp:ControlParameter Name="Day"   ControlID="txtDateFrom"  Type="DateTime" ConvertEmptyStringToNull="true"   />    
      <asp:ControlParameter Name="Month" ControlID="drpMonth" PropertyName="SelectedValue"/>    
      <asp:ControlParameter Name="Year"  ControlID="drpYear" PropertyName="SelectedValue" />    
      </SelectParameters>        
       </asp:SqlDataSource>                        
        </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>




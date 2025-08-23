<%@ Page Title="Update Task Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="UpdatePendingMiscellanceCustomerTask.aspx.cs" Inherits="UpdatePendingMiscellanceCustomerTask" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <fieldset><center><asp:Label ID="lblerror" runat="server" Font-Size="Large" Font-Bold="true" style="color:green" EnableViewState="false"></asp:Label></center>
         <br />
   <legend>Update Pending Customer Task</legend>
        <asp:Panel ID="PanBack" runat="server" Visible="true">
           <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
         </asp:Panel>
         <asp:Panel ID="PanComBack" runat="server" Visible="false">
         <asp:Button  ID="btncompBack" runat="server" OnClick="btncompBack_Click" Text="Back" />
         </asp:Panel>
         <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="width: 15%">
                <td style="width: 2%">
                    Customer Name
                </td>
                <td style="width: 15%">
                    <asp:Label ID="lblCustomerName" runat="server" ></asp:Label>
                </td>
                <td style="width: 2%">
                    Consignee Name
                </td>
                <td style="width: 15%">
                        <asp:Label ID="lblConsigneeName" runat="server" ></asp:Label>
                </td>
            </tr>
             <tr>
                 <td style="width: 2%">
                     Contact Person 
                 </td>
                 <td style="width: 15%">
                       <asp:Label ID="lblContactPerson" runat="server" ></asp:Label>
                 </td>
                 <td style="width: 2%">
                     Branch Name
                 </td>
                 <td style="width: 15%">
                     <asp:Label ID="lblBranchName" runat="server" ></asp:Label>
                 </td>
             </tr>
              <tr style="width: 15%">
                 <td style="width: 2%">
                     Start Date
                 </td>
                 <td style="width: 15%">
                      <asp:Label ID="lblStartDate" runat="server"  DataFormatString="{0:dd/MM/yyyy}" ></asp:Label>
                 </td>
           
                 <td style="width: 2%" >
                     Activity Type
                 </td>
                 <td style="width: 15%">
                     <asp:Label ID="lblActivityType" runat="server" ></asp:Label>
                 </td>
                    </tr>
             <tr style="width: 15%">
                <td style="width: 2%">
                    Subject 
                </td>
                <td style="width: 15%">
                    <asp:Label ID="lblSubject" runat="server"  ></asp:Label>
                </td>
                <td style="width: 2%">
                    Priority
                </td>   
                <td style="width: 15%">
                    <asp:Label ID="lblPriority" runat="server" ></asp:Label>
                </td>
            </tr>
             <tr style="width: 15%">
               <td style="width: 2%">Activity</td>
                 <td style="white-space: normal;">
                    <asp:Label ID="lblActivityDeatil" runat="server"  ></asp:Label>
                 </td>
                <td>
                    Estimated Date
                </td>
                <td>
                    <asp:Label ID="lblestmatedate" runat="server" Text="Label"></asp:Label>
                </td>
             </tr>

            <asp:Panel ID="PanBabaji" runat="server"  Visible="false">
             <tr style="width: 15%">
                 <td style="width: 2%">
                    Job No
                 </td>
                 <td>
                     <asp:Label ID="lblRefJobNO" runat="server" ></asp:Label>
                     <asp:Label ID="lblcustid" runat="server"  Visible="false"></asp:Label>
                     <asp:Label ID="lblbranchid" runat="server" Visible="false" ></asp:Label>
                      <asp:Label ID="lbljobno" runat="server" Visible="false" ></asp:Label>
                 </td>
                  <td>
                    Billable
                  </td>
                 <td>
                    <asp:Label ID="lblBillable" runat="server"></asp:Label>
                 </td>
             </tr>
           </asp:Panel>
            </table>
         <br />
            <asp:Panel runat="server" ID="Panl4" Visible="true">
        <div class="m clear">
        
   <asp:Button ID="btnUpdate" Text="Update Customer Task"  runat="server"  OnClick="btnUpdate_Click" Visible="true"   ValidationGroup="rec"/>   </div> 
           <asp:Label ID="LUserType" runat="server"  Visible="false" ></asp:Label>
             <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" >

                <asp:Panel ID="panUser" runat="server"  Visible="false"> 
                      <tr style="width: 15%">
                 <td style="width: 1%">
                 Billable
                         <asp:RequiredFieldValidator ID="RFVchkBillabal" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="rblBillabal" SetFocusOnError="True" ValidationGroup="rec">
                    </asp:RequiredFieldValidator>

                   </td>
                   <td>
                <asp:RadioButtonList ID="rblBillabal" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow"  AutoPostBack="True"   OnSelectedIndexChanged="rblBillabal_SelectedIndexChanged" >
                            <asp:ListItem Text="Yes &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                        </asp:RadioButtonList>
                      
                   </td>

                         <td style="width: 2%" >
                  Operation Job
               </td>
               <td style="width: 15%">

                   <asp:DropDownList ID="ddOperationJob" runat="server" Enabled="false"  OnSelectedIndexChanged="ddOperationJob_SelectedIndexChanged" AutoPostBack="true"  >

                        <asp:ListItem Text="-Select-" Value="0">
                        </asp:ListItem>
                        <asp:ListItem Text="Operation Job" Value="1">
                        </asp:ListItem>
                         <asp:ListItem Text="MMS Job" Value="2">
                        </asp:ListItem>
                   </asp:DropDownList>

               </td>
                       </tr>
                    <tr>
                        
                         <td>
                           
                             Billable Job No 
                           <asp:RequiredFieldValidator ID="RFVBillable" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="ddlBillablalJobNo" SetFocusOnError="True" InitialValue="0"
                        ValidationGroup="rec" Enabled="false"></asp:RequiredFieldValidator>
                        </td>
                        <td>

                             <asp:DropDownList ID="ddlBillablalJobNo" runat="server"  Width="63%" Enabled="false" >
                                 <asp:ListItem Text="Select" Value="0">
                                        </asp:ListItem>
                             </asp:DropDownList>

                        </td>
                          <td>
                            <asp:Button ID="btnRefresh" runat="server"  OnClick="btnRefresh_Click" Text="Refresh" /></td><td></td>

                    </tr>
                 </asp:Panel>
                 

                 <tr style="width: 15%">

                   <td style="width: 1%">
                      Follow Up Update
                      <asp:RequiredFieldValidator ID="RqfFollowupUpdate" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="TxtFollowupUpdate" SetFocusOnError="True"
                        ValidationGroup="rec"></asp:RequiredFieldValidator>
                 </td>
                 <td style="width: 5%">
                      <asp:TextBox ID="TxtFollowupUpdate" Width="55%" runat="server" TextMode="MultiLine"></asp:TextBox>
                    
                 </td>

                 <td style="width: 2%">
                   Estimated  Date
                            <AjaxToolkit1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="Image2" PopupPosition="BottomRight"
                                TargetControlID="TxtEstimatedDate">
                            </AjaxToolkit1:CalendarExtender>
                         <asp:RequiredFieldValidator ID="RqvEnddate" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="TxtEstimatedDate" SetFocusOnError="True" 
                        ValidationGroup="rec"></asp:RequiredFieldValidator>
                          
               </td>
               <td style="width: 5%"> 
                     
                           <asp:TextBox ID="TxtEstimatedDate" Width="20%"  runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="Image2" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
               </td>
                     </tr>
                 <tr style="width: 15%" >

                 <td style="width: 1%">
                     Status
                     <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="ddlStatus" SetFocusOnError="True"
                        ValidationGroup="rec" InitialValue="0"></asp:RequiredFieldValidator>
                 </td>
                 <td style="width: 10%">
                      <asp:DropDownList ID="ddlStatus" runat="server">
                           
                     </asp:DropDownList>
                 </td>
                 

                 <td style="width: 1%">
                       Follow Up Date
                      <AjaxToolkit1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="Image1" PopupPosition="BottomRight"
                                TargetControlID="TxtFolloupDate">
                            </AjaxToolkit1:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RFVFolloupDate" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="TxtFolloupDate" SetFocusOnError="True"
                        ValidationGroup="rec"></asp:RequiredFieldValidator>
                 
                 </td>
                  
                 <td style="width: 10%">
                    <asp:TextBox ID="TxtFolloupDate" Width="20%" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="Image1" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                 </td>
                        </tr>
                 <tr style="width: 15%" >

                      <td style="width: 1%">
                  Document Attachment
                     
                 </td>
                 <td style="width: 10%">
               <asp:FileUpload ID="fuUplodedoc" runat="server" />
                <asp:HiddenField ID="HdinfuUplodedoc" runat="server" />

                 </td>
                     <td style="width: 1%">
                     
                 </td>
                 <td style="width: 10%">

                 </td>

             </tr>
         </table>
           </asp:Panel> 
     </fieldset>
  
    <fieldset><legend>
         Customer Task Details
              </legend>

          <asp:GridView ID="GridMiscellanceCustomerDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="lid"
        Width="100%" PagerStyle-CssClass="pgr" DataSourceID="MiscellanceCustomerDetailsDataSorce"
        CssClass="table" CellPadding="4"   OnRowCommand="GridMiscellanceCustomerDetails_RowCommand"  
        OnRowDataBound="GridMiscellanceCustomerDetails_RowDataBound"
        AllowPaging="True" AllowSorting="True" PageSize="20" style="white-space: normal;" >
        <Columns>
          
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="FollowupDate" HeaderText="Follow Up Date" SortExpression="FollowupDate" DataFormatString="{0:dd/MM/yyyy}"  />
            <asp:BoundField DataField="FollowupUpdate" HeaderText="Follow Up Update"  SortExpression="FollowupUpdate" />
            <asp:BoundField DataField="sName" HeaderText="Task Updated Person Name" SortExpression="sName" />
             <asp:BoundField DataField="dtDate" HeaderText="Task Updated Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}"  />
             <asp:TemplateField HeaderText="Download" >
            <ItemTemplate>
            
                <asp:LinkButton ID="lnkDownload" runat="server" Text="Task Document " CommandName="download"
                    CommandArgument='<%#Eval("TaskFilePath") %>' Visible="true"></asp:LinkButton>
                
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
         <asp:Label ID="lblChack" runat="server"  Visible="false"></asp:Label>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="MiscellanceCustomerDetailsDataSorce" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetEmployeeMiscellaneousCustomerTask" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TaskID" SessionField="TaskID" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>


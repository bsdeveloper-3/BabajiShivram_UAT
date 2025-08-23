<%@ Page Title="New Customer Task " Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="MiscellaneousCustomerTask.aspx.cs" Inherits="MiscellaneousCustomerTask" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <fieldset>
        <center>
            <asp:Label ID="Lblerror" runat="server" Style="color: green" Font-Bold="true" Font-Size="Large" EnableViewState="false"></asp:Label></center>
        <legend>New Customer Task</legend>
    
        <div class="m clear">
            <asp:Button ID="btnSubmit" Text="Submit" runat="server" ValidationGroup="rec" OnClick="btnSubmit_Click" />
        </div>

        <asp:UpdatePanel ID="UpdCustomerTask" runat="server">
            <ContentTemplate>
                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="width: 15%">
                        <td style="width: 2%">Customer
                   <asp:RequiredFieldValidator ID="RFVCustomer" runat="server" ErrorMessage="*"
                       ForeColor="#CC0000" ControlToValidate="ddlCustomer" SetFocusOnError="True" InitialValue="0"
                       ValidationGroup="rec">
                   </asp:RequiredFieldValidator>
                        </td>

                        <td style="width: 15%">
                            <asp:DropDownList ID="ddlCustomer" runat="server" Width="63%" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>

                        <td>Billable
                         <asp:RequiredFieldValidator ID="RFVchkBillabal" runat="server" ErrorMessage="*"
                             ForeColor="#CC0000" ControlToValidate="rblBillabal" SetFocusOnError="True" ValidationGroup="rec">
                         </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblBillabal" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="rblBillabal_SelectedIndexChanged">
                                <asp:ListItem Text="Yes &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>

                    <tr>

                        <td style="width: 2%">Operation Job
                        </td>
                        <td style="width: 15%">

                            <asp:DropDownList ID="ddOperationJobMMS" runat="server" Enabled="false" OnSelectedIndexChanged="ddOperationJobMMS_SelectedIndexChanged" AutoPostBack="true">

                                <asp:ListItem Text="-Select-" Value="0">
                                </asp:ListItem>
                                <asp:ListItem Text="Operation Job" Value="1">
                                </asp:ListItem>
                                <asp:ListItem Text="MMS Job" Value="2">
                        </asp:ListItem>
                            </asp:DropDownList>

                        </td>


                        <td style="width: 2%">Contact Person  
                   <asp:RequiredFieldValidator ID="RFVContactPerson" runat="server" ErrorMessage="*"
                       ForeColor="#CC0000" ControlToValidate="TxtContactPerson" SetFocusOnError="True"
                       ValidationGroup="rec"></asp:RequiredFieldValidator>

                        </td>
                        <td style="width: 15%">

                            <asp:TextBox ID="TxtContactPerson" runat="server" Width="61%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 2%">Branch
                    <asp:RequiredFieldValidator ID="RFVBranch" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="ddlBranch" SetFocusOnError="True" InitialValue="0"
                        ValidationGroup="rec"></asp:RequiredFieldValidator>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" Width="63%" Enabled="false" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>


                        <td>Billable Job No 
                           <asp:RequiredFieldValidator ID="RFVBillable" runat="server" ErrorMessage="*"
                               ForeColor="#CC0000" ControlToValidate="ddlBillablalJobNo" SetFocusOnError="True" InitialValue="0"
                               ValidationGroup="rec" Enabled="false"></asp:RequiredFieldValidator>
                        </td>
                        <td>


                            <asp:DropDownList ID="ddlBillablalJobNo" runat="server" Width="63%" Enabled="false">
                                <asp:ListItem Text="Select" Value="0">
                                </asp:ListItem>
                            </asp:DropDownList>
                               <asp:Button ID="btnRef" runat="server" OnClick="btnRef_Click" Text="Refresh" />

                        </td>

                    </tr>
                    <tr>

                        <td style="width: 2%">Activity Type 
                     <asp:RequiredFieldValidator ID="RFVActivityType" runat="server" ErrorMessage="*"
                         ForeColor="#CC0000" ControlToValidate="ddlActivityType" SetFocusOnError="True" InitialValue="0"
                         ValidationGroup="rec"></asp:RequiredFieldValidator>


                        </td>
                        <td>
                            <asp:DropDownList ID="ddlActivityType" runat="server" class="form-control" TabIndex="10" Width="63%">
                                <asp:ListItem Text="-Select-" Value="0">
                                </asp:ListItem>
                                <asp:ListItem Text="Services" Value="1">
                                </asp:ListItem>
                                <asp:ListItem Text="Claim" Value="2">
                                </asp:ListItem>
                                <asp:ListItem Text="Licence Registration" Value="3">
                                </asp:ListItem>

                            </asp:DropDownList>

                        </td>


                        <td style="width: 2%">Priority  
                  <asp:RequiredFieldValidator ID="RFVPriority" runat="server" ErrorMessage="*"
                      ForeColor="#CC0000" ControlToValidate="ddlPriority" SetFocusOnError="True" InitialValue="0"
                      ValidationGroup="rec"></asp:RequiredFieldValidator>


                        </td>
                        <td>

                            <asp:DropDownList ID="ddlPriority" runat="server" class="form-control" TabIndex="10" Width="63%">
                                <%-- <asp:ListItem Text="-Select-" Value="0">
                        </asp:ListItem>
                        <asp:ListItem Text="Normal" Value="1">
                        </asp:ListItem>
                        <asp:ListItem Text="High" Value="2">
                        </asp:ListItem>
                        <asp:ListItem Text="Intences" Value="3">
                        </asp:ListItem>
                        <asp:ListItem Text="Priority" Value="4">
                        </asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 2%">Start Date 
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="Image1" PopupPosition="BottomRight"
                                TargetControlID="TxtStartDate">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVSTART" runat="server" ErrorMessage="*"
                                ForeColor="#CC0000" ControlToValidate="TxtStartDate" SetFocusOnError="True"
                                ValidationGroup="rec"></asp:RequiredFieldValidator>
                            <%-- <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="TxtStartDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="MskExtRemindDate" ControlToValidate="TxtStartDate" IsValidEmpty="false"
                                InvalidValueMessage="Reminder Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Reminder Date." EmptyValueBlurredText="*" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/08/2017" MaximumValue="31/12/2040"
                                runat="Server" ValidationGroup="validateReminder"></AjaxToolkit:MaskedEditValidator>--%>
                        </td>

                        <td>

                            <asp:TextBox ID="TxtStartDate" Width="30%" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="Image1" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                        </td>

                        <td>Estimated Date
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="Image2" PopupPosition="BottomRight"
                                TargetControlID="TxtEstimatedDate">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RqvEnddate" runat="server" ErrorMessage="*"
                                ForeColor="#CC0000" ControlToValidate="TxtEstimatedDate" SetFocusOnError="True"
                                ValidationGroup="rec"></asp:RequiredFieldValidator>
                            <%-- <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="TxtEstimatedEndDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" ControlExtender="MskExtRemindDate" ControlToValidate="TxtEstimatedEndDate" IsValidEmpty="false"
                                InvalidValueMessage="Reminder Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Reminder Date." EmptyValueBlurredText="*" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/08/2017" MaximumValue="31/12/2040"
                                runat="Server" ValidationGroup="validateReminder"></AjaxToolkit:MaskedEditValidator>--%>


                        </td>
                        <td>

                            <asp:TextBox ID="TxtEstimatedDate" Width="30%" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="Image2" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 2%">Assigned Employee 
                     <asp:RequiredFieldValidator ID="RFVAssignedEmployee" runat="server" ErrorMessage="*"
                         ForeColor="#CC0000" ControlToValidate="ddlAssignedemp" SetFocusOnError="True" InitialValue="0"
                         ValidationGroup="rec"></asp:RequiredFieldValidator>
                        </td>
                        <td>


                            <asp:DropDownList ID="ddlAssignedemp" runat="server" OnSelectedIndexChanged="ddlAssignedemp_SelectedIndexChanged" Width="63%"></asp:DropDownList>
                            <asp:Label ID="lblEmployeeEmailId" runat="server" Font-Bold="True" Visible="false"></asp:Label>
                        </td>






                        <td style="width: 2%">Upload  Document 
                        </td>
                        <td>
                            <asp:FileUpload ID="FuUplodeCustDoc" runat="server" />
                            <asp:HiddenField ID="HdinCustDoc" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 2%">Subject
                    <asp:RequiredFieldValidator ID="RFVSubject" runat="server" ErrorMessage="*"
                        ForeColor="#CC0000" ControlToValidate="TxtSubject" SetFocusOnError="True"
                        ValidationGroup="rec"></asp:RequiredFieldValidator>


                        </td>
                        <td>

                            <asp:TextBox ID="TxtSubject" runat="server" Width="70%" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>Activity Details 
                  <asp:RequiredFieldValidator ID="RFVActivityDetails" runat="server" ErrorMessage="*"
                      ForeColor="#CC0000" ControlToValidate="txtACTIVTIdETAILS" SetFocusOnError="True"
                      ValidationGroup="rec"></asp:RequiredFieldValidator>

                        </td>
                        <td>

                            <asp:TextBox ID="txtACTIVTIdETAILS" runat="server" TextMode="MultiLine" Height="50px"></asp:TextBox>
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>

</asp:Content>


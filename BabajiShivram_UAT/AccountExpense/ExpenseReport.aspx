<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpenseReport.aspx.cs" 
    Inherits="AccountExpense_ExpenseReport" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <style type="text/css">
        /*Calendar Control CSS*/
        .cal_Theme1 .ajax__calendar_container {
            background-color: #DEF1F4;
            border: solid 1px #77D5F7;
        }

        .cal_Theme1 .ajax__calendar_header {
            background-color: #ffffff;
            margin-bottom: 4px;
        }

        .cal_Theme1 .ajax__calendar_title,
        .cal_Theme1 .ajax__calendar_next,
        .cal_Theme1 .ajax__calendar_prev {
            color: #004080;
            padding-top: 3px;
        }

        .cal_Theme1 .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px #77D5F7;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            color: #004080;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            text-align: center;
        }

        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_day,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_month,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_year,
        .cal_Theme1 .ajax__calendar_active {
            color: #004080;
            font-weight: bold;
            background-color: #DEF1F4;
        }

        .cal_Theme1 .ajax__calendar_today {
            font-weight: bold;
        }

        .cal_Theme1 .ajax__calendar_other,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_today,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
            color: #bbbbbb;
        }
    </style>

     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <br />

    <div>
        <asp:Panel ID="panSez" runat="server">
            <fieldset class="fieldset">
                <legend>Expense Report</legend>

                <table width="90%">
                    <tr>
                        <td><b>Payment Completed Date  -  </b></td>
                        <td>&nbsp&nbsp;<b>From :</b>&nbsp;&nbsp;<asp:TextBox ID="txtfrom" runat="server" Width="100px" BackColor="#FFFFCC"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrom" Format="dd/MM/yyyy" CssClass=" cal_Theme1"></asp:CalendarExtender>
                        </td>
                        <td><b>To :</b>&nbsp;&nbsp;<asp:TextBox ID="txtTo" runat="server" Width="100px" BackColor="#FFFFCC"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTo" Format="dd/MM/yyyy" CssClass=" cal_Theme1"></asp:CalendarExtender>
                        </td>

                        <td>Type of Expense
                            <asp:RequiredFieldValidator ID="rfvExpenseType" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlExpenseType" InitialValue="0"
                                Text="*" ErrorMessage="Please Select Type of Expense."> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExpenseType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceExpense" BackColor="#FFFFCC"
                                    DataTextField="sName" DataValueField="lid" TabIndex="5" ToolTip="Select Type Of Expense." AutoPostBack="true">
                                    
                                </asp:DropDownList>

                                   <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                     SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            </td>

                        <td> <%--Today's Duty Report : --%>
                            <asp:CheckBox ID="chkToday" runat="server" checked="false" Visible="false" /> <%-- OnCheckedChanged="chkToday_OnCheckedChanged"--%>
                        </td>
                        <td></td>
                        <td>
                            <%--<asp:Button ID="btnSummary" runat="server" Text="Summary" />--%><%-- OnClick="btnSummary_Click" />--%>
                        </td>
                        <td>
                            <asp:Button ID="btnDetail" runat="server" Text="Detail" OnClick="btnDetail_Click" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkExportReport" runat="server" OnClick="lnkExportReport_Click"
                            data-tooltip="&nbsp; Export To Excel">
                              <asp:Image ID="imgExcelReport" runat="server" ImageUrl="../Images/Excel.jpg" />
                        </asp:LinkButton>
                        </td>
                    </tr>
                </table>                         

                <br />
                <br />         
      
            </fieldset>
        </asp:Panel>

           <asp:GridView ID="gridDetailReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" OnPageIndexChanging="gridDetailReport_OnPageIndexChanging">
                    <Columns>
                                               
                         <asp:TemplateField HeaderText="SNO">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>                                
                                <asp:Label ID="lblDate" runat="server" Text='<%# (Eval("Date", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("Date", "{0:dd/MM/yyyy}"))%>'></asp:Label>                                
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("Location") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch">
                            <ItemTemplate>
                                <asp:Label ID="lblBranch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BS Job No">
                            <ItemTemplate>
                                <asp:Label ID="lblJobRefNo" runat="server" Text='<%#Eval("BS Job No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                       <%-- <asp:TemplateField HeaderText="Delivered Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDeliveredDate" runat="server" Text='<%#Eval("PCDDeliveryDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="ACP/NON ACP">
                            <ItemTemplate>
                                <asp:Label ID="lblACPNonACP" runat="server" Text='<%#Eval("ACP/NON ACP") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Party Name">
                            <ItemTemplate>
                                <asp:Label ID="lblPartyName" runat="server" Text='<%#Eval("Party Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="IEC No">
                            <ItemTemplate>
                                <asp:Label ID="lblICENo" runat="server" Text='<%#Eval("IEC No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BOE No">
                            <ItemTemplate>
                                <asp:Label ID="lblBOENo" runat="server" Text='<%#Eval("BOE No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BOE Date">
                            <ItemTemplate>
                                <asp:Label ID="lblBOEDate" runat="server" Text='<%#Eval("BOE Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TR6Challen No">
                            <ItemTemplate>
                                <asp:Label ID="lblTR6Challan" runat="server" Text='<%#Eval("TR6Challen No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Duty Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblDutyAmnt" runat="server" Text='<%#Eval("Duty Amt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Int Amt">
                            <ItemTemplate>
                                <asp:Label ID="lblInterestAmnt" runat="server" Text='<%#Eval("Interest Amt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Penalty Amt">
                            <ItemTemplate>
                                <asp:Label ID="lblPenaltyAmnt" runat="server" Text='<%#Eval("Penalty Amt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Recd Mail From (Sender Name)">
                            <ItemTemplate>
                                <asp:Label ID="lblSenderName" runat="server" Text='<%#Eval("SenderName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Approved By">
                            <ItemTemplate>
                                <asp:Label ID="lblApproveBy" runat="server" Text='<%#Eval("Approved By") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="RD/DUTY/PENALTY">
                            <ItemTemplate>
                                <asp:Label ID="lblRdDutyPen" runat="server" Text='<%#Eval("RD/DUTY/PENALTY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Advance Details">
                            <ItemTemplate>
                                <asp:Label ID="lblAdvDetails" runat="server" Text='<%#Eval("Advance Details") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblReqRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                    </Columns>
                </asp:GridView>



          <asp:GridView ID="gridAllPayment" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" OnPageIndexChanging="gridAllPayment_OnPageIndexChanging">
                    <Columns>
                       
                         <asp:TemplateField HeaderText="SNO">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="BS Job No">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("BS Job No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Party Name">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("Party Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="IEC No">
                            <ItemTemplate>
                                <asp:Label ID="Label21" runat="server" Text='<%#Eval("IEC No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>    

                         <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="Label19" runat="server" Text='<%#Eval("Location") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Location Code">
                            <ItemTemplate>
                                <asp:Label ID="Label20" runat="server" Text='<%#Eval("LocationCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>                                
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                           <asp:TemplateField HeaderText="Total Amount">
                            <ItemTemplate>
                                <asp:Label ID="Label14" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Requested By">
                            <ItemTemplate>
                                <asp:Label ID="Label16" runat="server" Text='<%#Eval("Requested By") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="BOE No">
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("BL No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BOE Date">
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("BLDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Advance Details">
                            <ItemTemplate>
                                <asp:Label ID="Label18" runat="server" Text='<%#Eval("Advance Details") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Approved By">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Approved By") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                     

                        <asp:TemplateField HeaderText="Payment Ref No">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("PaymentRefNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 

                         <asp:TemplateField HeaderText="Ref Date">
                            <ItemTemplate>                                
                                <asp:Label ID="Label23" runat="server" Text='<%# (Eval("PaymentRefDate", "{0:dd/MM/yyyy}") == "01/01/0001" ? "" : Eval("PaymentRefDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>                                
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Ref No">
                            <ItemTemplate>
                                <asp:Label ID="Label25" runat="server" Text='<%#Eval("ReferenceNo") %>'></asp:Label>
                                
                            </ItemTemplate>
                        </asp:TemplateField> 

                         <asp:TemplateField HeaderText="Pay Date">
                            <ItemTemplate>                                
                                <asp:Label ID="Label26" runat="server" Text='<%# (Eval("PayDate", "{0:dd/MM/yyyy}") == "01/01/0001" ? "" : Eval("PayDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>                                
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Bank Name">
                            <ItemTemplate>
                                <asp:Label ID="Label22" runat="server" Text='<%#Eval("BankName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                       

                        <asp:TemplateField HeaderText="Pay Remark">
                            <ItemTemplate>
                                <asp:Label ID="Label24" runat="server" Text='<%#Eval("Narration") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        
                         <asp:TemplateField HeaderText="Payment By">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("paymentUpdateBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>  

                        <asp:TemplateField HeaderText="Payment Date">
                            <ItemTemplate>
                                <asp:Label ID="Label27" runat="server" Text='<%#Eval("PaymentDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        
                         <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>               
                       
                    </Columns>
                </asp:GridView>

    </div>

</asp:Content>


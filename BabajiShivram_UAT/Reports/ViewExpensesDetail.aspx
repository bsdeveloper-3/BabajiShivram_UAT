<%@ Page Title="Job Expense Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ViewExpensesDetail.aspx.cs" Inherits="Reports_ViewExpensesDetail" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }
    </style>
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
                
    </script>
    
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnPreAlertId" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
                <asp:HiddenField ID="hdnMode" runat="server" />
                <asp:HiddenField ID="hdnBoeTypeId" runat="server" />
                <asp:HiddenField ID="hdnDeliveryType" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div class="clear"></div>
                <fieldset>
                    <legend>Job Detail</legend>
                        <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId"
                            Width="100%" OnDataBound="FVJobDetail_DataBound">
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>BS Job No.
                                        </td>
                                        <td>
                                            <%# Eval("JobRefNo") %>
                                                &nbsp;
                                            <asp:Label ID="lblInbondJobNo" Text="Inbond Job" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>Cust Ref No.
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("CustRefNo") %>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customer
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Customer")%>
                                            </span>
                                        </td>
                                        <td>Consignee
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Consignee")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customer Division
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("Division") %>'></asp:Label>
                                        </td>
                                        <td>Customer Plant
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPlant" runat="server" Text='<%#Eval("Plant") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mode
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Mode")%>
                                            </span>
                                        </td>
                                        <td>Port of Discharge
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Port")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Babaji Branch
                                        </td>
                                        <td>
                                            <span>
                                                <asp:Label ID="lblBabajiBranch" runat="server" Text='<%#Eval("BabajiBranch") %>'></asp:Label>
                                            </span>
                                        </td>
                                        <td>Priority
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Priority")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Gross Weight
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("GrossWT")%> <span>Kgs</span>
                                            </span>
                                        </td>
                                        <td>Packages
                                        </td>
                                        <td>
                                            <%# Eval("NoOfPackages")%>
                                            &nbsp;
                                            <%#Eval("PackageTypeName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Container
                                        </td>
                                        <td>
                                            <span>
                                                20" - <b> <%# Eval("Count20")%> </b> &nbsp;&nbsp;&nbsp;
                                                40" - <b><%# Eval("Count40")%> </b> &nbsp;&nbsp;&nbsp;
                                                LCL - <b><%# Eval("CountLCL")%> </b> 
                                            </span>
                                        </td>
                                        <td>
                                            Total Container
                                        </td>
                                        <td>
                                            <b><%# Eval("CountTotal")%> </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Job Type
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("JobType")%>
                                            </span>
                                        </td>
                                        <td>RMS/NonRMS
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("RMSNonRMS")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>BoE No
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BOENo")%>
                                            </span>
                                        </td>
                                        <td>BoE Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BOEDate","{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>MBL No
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("MAWBNo")%>
                                            </span>
                                        </td>
                                        <td>MBL Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("MAWBDate","{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Delivery Type
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("DeliveryType")%>
                                            </span>
                                        </td>
                                        <td>First Check Required?	
                                        </td>
                                        <td>
                                            <span>
                                                <%# (Boolean.Parse(Eval("FirstCheckRequired").ToString())) ? "Yes": "No"%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Transport By
                                        </td>
                                        <td>
                                            <%# (Convert.ToBoolean(Eval("TransportationByBabaji"))) ? "Babaji": "Customer"%>
                                        </td>
                                        <td>KAM
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("KAMUser")%>
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:FormView> 
                </fieldset>
                
                <fieldset>
                    <legend>Additional Expense Detail</legend>
    
                    <asp:FormView ID="fvExpense" runat="server" DefaultMode="ReadOnly" Width="100%" DataKeyNames="lId" >
                    <ItemTemplate>        
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Expense Type
                                </td>
                                <td>
                                    <asp:Label ID="lblExpenseType" Text='<%#Bind("ExpenseName") %>' runat="server"></asp:Label>
                                </td>
                                <td>
                                    Expense Amount
                                </td>
                                <td>
                                    <asp:Label ID="lblExAmount" Text='<%#Bind("Amount") %>' runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Receipt No.
                                </td>
                                <td>
                                    <asp:Label ID="lblReceiptNo" Text='<%#Bind("ReceiptNo") %>' runat="server"></asp:Label>
                                </td>
                                <td>
                                    Receipt Date
                                </td>
                                <td>
                                    <asp:Label ID="lblReceiptDate" Text='<%#Bind("ReceiptDate","{0:dd/MM/yyyy}") %>' runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>     
                                <td>
                                    Receipt / Non Receipt
                                </td>
                                <td>
                                    <asp:Label ID="lblReceiptable" Text='<%#Bind("ReceiptType") %>' runat="server" ></asp:Label>
                                </td>
                                <td>
                                   Payment Type 
                                </td>
                                <td>
                                    <asp:Label ID="lblPaymentType" Text='<%#Bind("PaymentTypeName") %>' runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>         
                                <td>
                                    Receipt Amount
                                </td>
                                <td>
                                    <asp:Label ID="lblReceiptAmount" Text='<%#Bind("ReceiptAmount") %>' runat="server"></asp:Label>
                                </td>
                                <td>
                                    Billable / Non Billable
                                </td>
                                <td>
                                    <asp:Label ID="lblBillable" Text='<%#Bind("BillableNoBillable") %>' runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Paid To
                                </td>
                                <td>
                                    <asp:Label ID="lblPaidTo" Text='<%#Bind("PaidTo") %>' runat="server"></asp:Label>
                                </td>
                                <td>
                                  
                                </td>
                                <td>

                                </td>
                            </tr>   
                        </table>
                    </ItemTemplate>
                    </asp:FormView>
                </fieldset>
                        
                <fieldset>
                    <legend>Expense Approval History</legend>
                    <asp:GridView ID="gvInvoiceHistory" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lId" DataSourceID="DataSourceExpenseHistory" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Status" DataField="StatusName" />
                            <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 500px; white-space:normal;">
                                <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="User" DataField="UserName" />
                            <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                            </Columns>
                    </asp:GridView>
                </fieldset>
                
                <fieldset>
                    <legend>Previous Expense Request</legend>   
                        <asp:GridView ID="grdJobHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                            PagerStyle-CssClass="pgr" DataKeyNames="ExpenseID" AllowPaging="True" AllowSorting="True" PageSize="20"
                            DataSourceID="DataSourceJobHistory" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl" >
                            <ItemTemplate>
                                <%# Container.DataItemIndex +1%>
                            </ItemTemplate>
                            </asp:TemplateField>                   
                            <asp:BoundField DataField="ExpenseName" HeaderText="Expense Type" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" />
                            <asp:BoundField DataField="PaidTo" HeaderText="Paid To" />
                            <asp:BoundField DataField="ReceiptType" HeaderText="Receipt" />
                            <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" />
                            <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" />
                            <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amt" />
                            <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="BillableNoBillable" HeaderText="Billable ?" />
                            <asp:BoundField DataField="StatusName" HeaderText="Status" />
                            <%--<asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="StatusUser" HeaderText="Audit By" />
                            <asp:BoundField DataField="StatusRemark" HeaderText="Audit Remark" />--%>
                            <asp:BoundField DataField="UserName" HeaderText="Created By" />
                            <asp:BoundField DataField="dtDate" HeaderText="Creation Date" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                        
                <div id="div1">
                    <asp:SqlDataSource ID="DataSourceExpenseHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetJobExpenseHistory" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="ExpenseID" SessionField="ExpenseID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceJobHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetJobPrevExpense" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="ExpenseID" SessionField="ExpenseID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


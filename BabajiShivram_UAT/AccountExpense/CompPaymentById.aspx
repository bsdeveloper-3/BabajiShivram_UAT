<%@ Page Title="Fund Request Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CompPaymentById.aspx.cs"
    Inherits="AccountExpense_CompPaymentById" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpPayment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upExpPayment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnPaymentTypeId" runat="server" Value="0" />
            </div>
            <div class="m clear">
                <asp:Button ID="btnSubmit" Text="Save" Visible="false" runat="server" ValidationGroup="Required"
                    TabIndex="39" />
                <asp:Button ID="btnCancel" Text="Cancel" Visible="false" CausesValidation="false" TabIndex="40"
                    runat="server" />
            </div>
            <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%">
                <ItemTemplate>
                    <fieldset style="width: 90%">
                        <legend>Job Details</legend>

                        <div class="m clear">
                            <asp:Button ID="btnEditJob" runat="server"  CssClass="btn" Text="Edit" OnClick="btnEditJob_Click"/>
                            <%--<asp:Button ID="btnBackButton" runat="server"  UseSubmitBehavior="false"
                                Text="Back" CommandArgument="PaymentRequests.aspx" CausesValidation="false" OnClick="btnBackButton_Click" />--%>
                        </div>

                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>BS Job Number</td>
                                <td>
                                    <asp:Label ID="lblBSJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                </td>
                                <td>Branch</td>
                                <td>
                                    <asp:Label ID="lblBranch" runat="server" Text='<%#Eval("BranchName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Payment Type</td>
                                <td>
                                    <asp:Label ID="lblPaymentType" runat="server" Text='<%#Eval("PaymentTypeName1") %>'></asp:Label>
                                </td>
                                <td>Request Type</td>
                                <td>
                                    <asp:Label ID="lblExpenseType" runat="server" Text='<%#Eval("ExpenseTypeName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Amount</td>
                                <td>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                </td>
                                <td>Paid To</td>
                                <td>
                                    <asp:Label ID="lblPaidTo" runat="server" Text='<%#Eval("PaidTo") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Remark</td>
                                <td>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                </td>
                                <td>Created By</td>
                                <td>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Created Date</td>
                                <td>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate","{0:dd/MM/yyyy HH:mm:ss tt}") %>'></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </fieldset>
                </ItemTemplate>

                <EditItemTemplate>
                   <fieldset style="width: 90%">
                    <legend>Job Details</legend>
                     <div class="m clear">
                        <asp:Button ID="btnUpdateJob" runat="server"  CssClass="update"
                            Text="Update" ValidationGroup="JobRequired" OnClick="btnUpdateJob_Click" /> 
                        <asp:Button ID="btnCancelButton" runat="server"  CausesValidation="False"
                            CssClass="cancel" Text="Cancel" OnClick="btnCancelButton_Click" />
                     </div>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>BS Job Number
                            </td>
                            <td>
                                <%# Eval("JobRefNo") %>
                            </td>
                            <td>Branch
                            </td>
                            <td>
                                <%# Eval("BranchName") %>
                            </td>
                        </tr>           
                         
                         <tr>
                            <td>Payment Type
                            </td>
                            <td>
                                <%# Eval("PaymentTypeName") %>
                            </td>
                            <td>Request Type
                            </td>
                            <td>
                                <%# Eval("ExpenseTypeName") %>
                            </td>
                        </tr>   
                         
                         <tr>
                                <td>Amount</td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount") %>' ></asp:TextBox>                                                                     
                                </td>
                                <td>Paid To</td>
                                <td>
                                    <asp:TextBox ID="txtPaidTo" runat="server" Text='<%#Eval("PaidTo") %>' Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Remark</td>
                                <td>
                                    <%#Eval("Remark") %>
                                </td>
                                <td>Created By</td>
                                <td>
                                    <%#Eval("CreatedBy") %>
                                </td>
                            </tr>
                            <tr>
                                <td>Created Date</td>
                                <td>
                                    <%#Eval("CreatedDate","{0:dd/MM/yyyy HH:mm:ss tt}") %>
                                </td>
                                <td>
                                    Rejection Remark
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server" Text="" Width="400px"></asp:TextBox>
                                </td>
                            </tr>             

                    </table>
                  </fieldset>
                </EditItemTemplate>

            </asp:FormView>
        
            <asp:FormView ID="fsStampDuty" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%" OnDataBound="fsStampDuty_DataBound">
                <ItemTemplate>
                    <fieldset style="width: 90%" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Party Name</td>
                                <td>
                                    <asp:Label ID="lblPartyName_StampDuty" runat="server" Text='<%#Eval("PartyName") %>'></asp:Label>
                                </td>
                                <td>Client Address</td>
                                <td>
                                    <asp:Label ID="lblClientAddress" runat="server" Text='<%#Eval("ClientAdd") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>BOE No</td>
                                <td>
                                    <asp:Label ID="lblBoeNo_StampDuty" runat="server" Text='<%#Eval("BOENo") %>'></asp:Label>
                                </td>
                                <td>BOE Date</td>
                                <td>
                                    <asp:Label ID="lblBOEDate_StampDuty" runat="server" Text='<%#Eval("BOEDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Assessable Value</td>
                                <td>
                                    <asp:Label ID="lblAssessableValue" runat="server" Text='<%#Eval("AssessableValue") %>'></asp:Label></td>
                                <td>B/L No</td>
                                <td>
                                    <asp:Label ID="lbl_BLNo" runat="server" Text='<%#Eval("BLNo") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>B/L Date</td>
                                <td>
                                    <asp:Label ID="lbl_BLDate" runat="server" Text='<%#Eval("BLDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </td>
                                <td>IGM</td>
                                <td>
                                    <asp:Label ID="lblIGM" runat="server" Text='<%#Eval("IGMNo") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Custom Duty</td>
                                <td>
                                    <asp:Label ID="lblCustomDuty" runat="server" Text='<%#Eval("CustomDuty") %>'></asp:Label></td>
                                <td>Total (Assessable Value + Custom Duty)</td>
                                <td>
                                    <asp:Label ID="lblAssCustomTotal" runat="server" Text='<%#Eval("AssCustomTotal") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Stamp Duty</td>
                                <td>
                                    <asp:Label ID="lblStampDuty" runat="server" Text='<%#Eval("StampDuty") %>'></asp:Label>
                                </td>
                                <td>GST No</td>
                                <td>
                                    <asp:Label ID="txtGSTNo" runat="server" Text='<%#Eval("GSTNo") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ItemTemplate>
            </asp:FormView>

            <asp:FormView ID="fsDutyPayment" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%" OnDataBound="fsDutyPayment_DataBound">
                <ItemTemplate>
                    <fieldset style="width: 90%" runat="server">
                        <div class="m clear">
                            <asp:Button ID="btnEditDutyJob" runat="server"  CssClass="btn" Text="Edit" OnClick="btnEditDutyJob_Click" />
                            <%--<asp:Button ID="btnBackButton" runat="server"  UseSubmitBehavior="false"
                             Text="Back" CommandArgument="PaymentRequests.aspx" CausesValidation="false" OnClick="btnBackButton_Click"/>--%>
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Party Name</td>
                                <td>
                                    <asp:Label ID="lblPartyName" runat="server" Text='<%#Eval("PartyName") %>'></asp:Label>
                                </td>
                                <td>IEC No</td>
                                <td>
                                    <asp:Label ID="lblIECNo" runat="server" Text='<%#Eval("IECNo") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>BOE No</td>
                                <td>
                                    <asp:Label ID="lblBoeNo" runat="server" Text='<%#Eval("BOENo") %>'></asp:Label>
                                </td>
                                <td>BOE Date</td>
                                <td>
                                    <asp:Label ID="lblBoeDate" runat="server" Text='<%#Eval("BOEDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Location Code</td>
                                <td>
                                    <asp:Label ID="lblLocationCode" runat="server" Text='<%#Eval("LocationCode") %>'></asp:Label>
                                </td>
                                <td>ACP / Non-ACP ?</td>
                                <td>
                                    <asp:Label ID="lblACPNonACP" runat="server" Text='<%#Eval("ACPNonACPName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>TR6 Challan No</td>
                                <td>
                                    <asp:Label ID="lblChallenNo" runat="server" Text='<%#Eval("TR6ChallenNo") %>'></asp:Label>
                                </td>
                                <td>Duty Amount</td>
                                <td>
                                    <asp:Label ID="lblDutyAmount" runat="server" Text='<%#Eval("DutyAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Interest Amount</td>
                                <td>
                                    <asp:Label ID="lblIntAmount" runat="server" Text='<%#Eval("InterestAmount") %>'></asp:Label>
                                </td>
                                <td>Penalty Amount</td>
                                <td>
                                    <asp:Label ID="lblPenaltyAmount" runat="server" Text='<%#Eval("PenaltyAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Total</td>
                                <td>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                </td>
                                <td>Received Mail From (name)</td>
                                <td>
                                    <asp:Label ID="lblRecdMailFrom_Name" runat="server" Text='<%#Eval("RecdMailFrom") %>'></asp:Label>
                                    <asp:Label ID="lblRecdMailFrom_Mail" runat="server" Text='<%#Eval("RecdMailFromMailId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Approved By</td>
                                <td>
                                    <asp:Label ID="lblApprovedBy" runat="server" Text='<%#Eval("ApprovedBy") %>'></asp:Label>
                                </td>
                                <td>RD / Duty / Penalty</td>
                                <td>
                                    <asp:Label ID="lblRdDutyPenalty" runat="server" Text='<%#Eval("PaymentTypeName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Advance Details</td>
                                <td>
                                    <asp:Label ID="lblAdvanceDetails" runat="server" Text='<%#Eval("AdvanceDetails") %>'></asp:Label>
                                </td>
                                <td>Status</td>
                                <td>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("CurrentStatus") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Penalty Approval Mail</td>
                                <td>
                                    <asp:Label ID="lblPenaltyMail" runat="server" Text='<%#Eval("PenaltyAppMail") %>'></asp:Label>
                                </td>
                                <td>Download Penalty Copy
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkPenaltyCopy" runat="server" Text="Download" OnClick="lnkPenaltyCopy_OnClick"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnPenaltyCopyPath" runat="server" Value='<%#Eval("PenaltyCopyPath") %>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ItemTemplate>


                <EditItemTemplate>
                   <fieldset style="width: 90%">
                    <legend>Job Details</legend>
                     <div class="m clear">
                        <asp:Button ID="btnUpdateJobDuty" runat="server"  CssClass="update"
                            Text="Update" ValidationGroup="JobRequired" OnClick="btnUpdateJobDuty_Click" /> 
                        <asp:Button ID="btnDutyCancelButton" runat="server"  CausesValidation="False"
                            CssClass="cancel" Text="Cancel" OnClick="btnDutyCancelButton_Click" />
                     </div>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">                        
                        <tr>
                            <td>Party Name</td>
                            <td>
                                <%#Eval("PartyName") %>
                            </td>
                            <td>IEC No</td>
                            <td>
                                <%#Eval("IECNo") %>
                            </td>
                        </tr>
                        <tr>
                            <td>BOE No</td>
                            <td>
                                <%#Eval("BOENo") %>
                            </td>
                            <td>BOE Date</td>
                            <td>
                                <%#Eval("BOEDate","{0:dd/MM/yyyy}") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Location Code</td>
                            <td>
                                <%#Eval("LocationCode") %>  
                            </td>
                            <td>ACP / Non-ACP ?</td>
                            <td>
                                <asp:DropDownList ID="ddlACPNonACP" runat="server" ></asp:DropDownList>
                                <%--<asp:Label ID="lblACPNonACP" runat="server" Text='<%#Eval("ACPNonACPName") %>'></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>TR6 Challan No</td>
                            <td>
                                <%--<asp:Label ID="lblChallenNo" runat="server" Text='<%#Eval("TR6ChallenNo") %>'></asp:Label>--%>
                                <asp:TextBox ID="txtChallanNo" runat="server" Text='<%#Eval("TR6ChallenNo") %>'></asp:TextBox>
                            </td>
                            <td>Duty Amount</td>
                            <td>
                                <%--<asp:Label ID="lblDutyAmount" runat="server" Text='<%#Eval("DutyAmount") %>'></asp:Label>--%>
                                <asp:TextBox ID="txtDutyAmount" runat="server" Text='<%#Eval("DutyAmount") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Interest Amount</td>
                            <td>
                                <%--<asp:Label ID="lblIntAmount" runat="server" Text='<%#Eval("InterestAmount") %>'></asp:Label>--%>
                                <asp:TextBox ID="txtIntAmount" runat="server" Text='<%#Eval("InterestAmount") %>'></asp:TextBox>
                            </td>
                            <td>Penalty Amount</td>
                            <td>
                                <%--<asp:Label ID="lblPenaltyAmount" runat="server" Text='<%#Eval("PenaltyAmount") %>'></asp:Label>--%>
                                <asp:TextBox ID="txtPenaltyAmount" runat="server" Text='<%#Eval("PenaltyAmount") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Total</td>
                            <td>
                                <%#Eval("Total") %>
                            </td>
                            <td>Received Mail From (name)</td>
                            <td>
                                <%#Eval("RecdMailFrom") %>
                                <%#Eval("RecdMailFromMailId") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Approved By</td>
                            <td>
                                <%#Eval("ApprovedBy") %>
                            </td>
                            <td>RD / Duty / Penalty</td>
                            <td>
                                <asp:DropDownList ID="ddlRdDutyPenalty" runat="server"></asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td>Advance Details</td>
                            <td>
                                <asp:TextBox ID="txtAdvanceDetails" runat="server" Text='<%#Eval("AdvanceDetails") %>'></asp:TextBox>                                
                            </td>
                            <td>Status</td>
                            <td>
                                <%#Eval("CurrentStatus") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Penalty Approval Mail</td>
                            <td>
                                <%#Eval("PenaltyAppMail") %>
                            </td>
                            <td>Download Penalty Copy
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkPenaltyCopy" runat="server" Text="Download" OnClick="lnkPenaltyCopy_OnClick"></asp:LinkButton>
                                <asp:HiddenField ID="hdnPenaltyCopyPath" runat="server" Value='<%#Eval("PenaltyCopyPath") %>' />
                                <%--<asp:FileUpload ID="fuPenaltyCopy" runat="server" /> --%>
                            </td>
                        </tr>
                    </table>
                  </fieldset>
                </EditItemTemplate>
            </asp:FormView>


            <asp:FormView ID="fsRTGS" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%" OnDataBound="fsRTGS_DataBound">
                <ItemTemplate>
                    <fieldset runat="server" style="width: 90%">
                        <legend>Payment Details</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Payment Ref No
                                </td>
                                <td>
                                    <asp:Label ID="lblRefNo" runat="server" Text='<%#Eval("RefNo") %>'></asp:Label>
                                </td>

                                <td>Payment Date
                                </td>
                                <td>
                                    <asp:Label ID="lblPaymentDt_RTGS" runat="server" Text=' <%# (Eval("PaymentDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PaymentDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Bank Name
                                </td>
                                <td>
                                    <asp:Label ID="lblBankName_RTGS" runat="server" Text='<%#Eval("BankName") %>'></asp:Label>
                                </td>
                                <td>Download POD
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkPODCopy" runat="server" Text="Download" OnClick="lnkPODCopy_OnClick"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnPODCopyPath" runat="server" Value='<%#Eval("PODDocPath") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>Updated By
                                </td>
                                <td>
                                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy") %>'></asp:Label>
                                </td>
                                  <td>Updated Date
                                </td>
                                <td>
                                    <asp:Label ID="lblUpdatedDate" runat="server" Text='<%# (Eval("UpdatedDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("UpdatedDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>
                                </td>
                            </tr>
                         
                        </table>
                    </fieldset>
                </ItemTemplate>
            </asp:FormView>

            <asp:FormView ID="fsCheque_DD" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%">
                <ItemTemplate>
                    <fieldset runat="server" style="width: 90%">
                        <legend>Payment Details</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Payment Ref No</td>
                                <td>
                                    <asp:Label ID="lblChequeNo" runat="server" Text='<%#Eval("ChequeNo") %>'></asp:Label>
                                </td>
                                <td>Payment Date            
                                </td>
                                <td>
                                    <asp:Label ID="lblChequeDate" runat="server" Text='<%# (Eval("ChequeDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("ChequeDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Bank Name
                                </td>
                                <td>
                                    <asp:Label ID="lblBankName" runat="server" Text='<%#Eval("BankName") %>'></asp:Label>
                                </td>
                                <td>Narration</td>
                                <td>
                                    <asp:Label ID="lblNarration" runat="server" Text='<%#Eval("Narration") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>                             
                                <td>Updated By
                                </td>
                                <td>
                                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy") %>'></asp:Label>
                                      
                                </td>
                                <td>Updated Date
                                </td>
                                <td>
                                    <asp:Label ID="lblUpdatedDate" runat="server" Text='<%# (Eval("UpdatedDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("UpdatedDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>
                                </td>
                            </tr>
                     
                        </table>
                    </fieldset>
                </ItemTemplate>
            </asp:FormView>

            <asp:FormView ID="fsCash" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%">
                <ItemTemplate>
                    <fieldset style="width: 90%" runat="server">
                        <legend>Payment Details</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Ref No                          
                                </td>
                                <td>
                                    <asp:Label ID="lblRefNo_Cash" runat="server" Text='<%#Eval("RefNo")%>'></asp:Label>
                                </td>
                                <td>Payment Date                          
                                </td>
                                <td>
                                    <asp:Label ID="lblPaymentDate" runat="server" Text=' <%# (Eval("PaymentDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PaymentDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>
                                </td>
                            </tr>
                            <tr>                              
                                <td>Updated By
                                </td>
                                <td>
                                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy") %>'></asp:Label>

                                  
                                </td>
                                <td>Updated Date
                                </td>
                                <td>
                                    <asp:Label ID="lblUpdatedDate" runat="server" Text='<%# (Eval("UpdatedDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("UpdatedDate", "{0:dd/MM/yyyy}"))%>'></asp:Label>
                                </td>
                            </tr>                         
                        </table>
                    </fieldset>
                </ItemTemplate>
            </asp:FormView>


            <fieldset style="width: 90%">
              <legend>Documents</legend>

                <div id="dvUploadNewFile" runat="server" style="max-height: 200px; overflow: auto;">
                     <asp:FileUpload ID="fuDocument" runat="server" />                            
                     <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click"/>
                </div>
                <br />    
                <div>
                    <asp:GridView ID="gvPaymentReqDocs" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DocumentSqlDataSource"
                        OnRowCommand="gvPaymentReqDocs_RowCommand" CellPadding="4" AllowPaging="True"
                        AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="lid" HeaderText="Document" Visible="false"/>
                            <asp:BoundField DataField="FileName" HeaderText="Document" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Uploaded By" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Uploaded On" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download" ToolTip="Download" 
                                        CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" CommandName="Del" ToolTip="Delete" Width="39" CausesValidation="false" CommandArgument='<%#Eval("lid")%>'
                                                runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField> 

                        </Columns>
                    </asp:GridView>
                </div>
                <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetExpenseDocDetails" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </fieldset>

            <fieldset style="width: 90%">
                <legend>Request History
                </legend>
                <div>
                    <asp:GridView ID="gvReqHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceRequestHistory"
                        CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Updated By" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Updated Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:SqlDataSource ID="DataSourceRequestHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentStatus" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="RequestId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="FormViewDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentRequestById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetApprovedJobExpense" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


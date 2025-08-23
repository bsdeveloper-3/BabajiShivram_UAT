<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCDBillingDetail.aspx.cs" Inherits="PCDBillingDetail"
 MasterPageFile="~/MasterPage.master" Title="PCA Billing Detail" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    
        <cc1:TabContainer ID="TabJobDetail" runat="server" ActiveTabIndex="0" CssClass="Tab">
            <cc1:TabPanel ID="TabPCDBilling" runat="server" HeaderText="Billing Detail">
                <ContentTemplate>
        
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    BS Job No.
                    <asp:HiddenField ID="hdnCustomerId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>
                    Customer Name
                </td>
                <td>
                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Recipient Name
                </td>
                <td>
                    <asp:Label ID="lblPersonName" runat="server"></asp:Label>
                </td>
                <td>
                    Documents Scrutiny By
                </td>
                <td>
                    <asp:Label ID="lblScrutinyBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Date on Received the documents after Scrutiny
                </td>
                <td>
                    <asp:Label ID="lblScrutinyDate" runat="server"></asp:Label>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table> 
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <div class="m clear"></div>
            <asp:FormView ID="fvInvoice" runat="server" DataKeyNames="lId" DataSourceID="DataSourcePCDInvoice" 
            Width="99%" OnItemInserted="fvInvoice_ItemInserted" OnItemUpdated="fvInvoice_ItemUpdated"
             OnItemDeleted="fvInvoice_ItemDeleted" OnDataBound="fvInvoice_DataBound" DefaultMode="ReadOnly">
                <EmptyDataTemplate>
                    <asp:Button ID="btnNew" Text="New Invoice" OnClick="btnNew_Click" runat="server" CausesValidation="false"/>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnEditButton" Text="Edit" CommandName="Edit" runat="server" CausesValidation="false" />
                    <asp:Button ID="btnDeleteButton" Text="Delete" OnClientClick="return confirm('Sure to delete?');" CausesValidation="false"
                        runat="server" CommandName="Delete" />
                    <asp:Button ID="btnNew" Text="New" runat="server" OnClick="btnNew_Click" CausesValidation="false" />
                
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Draft Invoice Prepared Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtDraftInvoiceDate" runat="server" Text='<%# Bind("DraftInvoiceDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                Checking Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtCheckingDate" runat="server" Text='<%# Bind("CheckingDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Final Typing Date (Back to Back Billing)
                            </td>
                            <td>
                                <asp:TextBox ID="txtTypingDate" runat="server" Text='<%# Bind("FinalTypingDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                Generlising Date (Documents as per Customer)
                            </td>
                            <td>
                                <asp:TextBox ID="txtGenerlisingDate" runat="server" Text='<%# Bind("GenerlisingDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Number
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Text='<%# Bind("InvoiceNo") %>'></asp:TextBox>
                            </td>
                            <td>
                                Invoice Date
                            </td>
                            <td>
                               <asp:TextBox ID="txtInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Amount
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceAmount" runat="server" Text='<%# Bind("InvoiceAmount") %>'></asp:TextBox>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                           
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdateButton" Text="Update" runat="server" CommandName="Update" ValidationGroup="RequiredInvoice"  />
                    <asp:Button ID="btnUpdateCancelButton" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel" />
                
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Draft Invoice Prepared Date
                                <cc1:CalendarExtender ID="CalDraftInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDraftInvoiceDate" PopupPosition="BottomRight"
                                    TargetControlID="txtDraftInvoiceDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVDraftInvoiceDate" runat="server" ControlToValidate="txtDraftInvoiceDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Draft Invoice Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompValDraftInvoiceDate" runat="server" ControlToValidate="txtDraftInvoiceDate" Display="Dynamic" Text="Invalid date."
                                    ErrorMessage="Invalid Draft Invoice Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDraftInvoiceDate" runat="server" Text='<%# Bind("DraftInvoiceDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgDraftInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>
                                Checking Date
                                <cc1:CalendarExtender ID="CalRFVCheckingDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCheckingDate" PopupPosition="BottomRight"
                                    TargetControlID="txtCheckingDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVCheckingDate" runat="server" ControlToValidate="txtCheckingDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Checking Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValCheckingDate" runat="server" ControlToValidate="txtCheckingDate" Display="Dynamic" 
                                    Text="Invalid date." ErrorMessage="Invalid Checking Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" 
                                    Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCheckingDate" runat="server" Text='<%# Bind("CheckingDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgCheckingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Final Typing Date (Back to Back Billing)
                                <cc1:CalendarExtender ID="CalRFVTypingDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgTypingDate" PopupPosition="BottomRight"
                                    TargetControlID="txtTypingDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVTypingDate" runat="server" ControlToValidate="txtTypingDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Final Typing Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValTypingDate" runat="server" ControlToValidate="txtTypingDate" Display="Dynamic" 
                                    Text="Invalid date." ErrorMessage="Invalid Final Typing Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" 
                                    Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTypingDate" runat="server" Text='<%# Bind("FinalTypingDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgTypingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>
                                Generlising Date (Documents as per Customer)
                                <cc1:CalendarExtender ID="CalRFVGenerlisingDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgGenerlisingDate" PopupPosition="BottomRight"
                                    TargetControlID="txtGenerlisingDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVGenerlisingDate" runat="server" ControlToValidate="txtGenerlisingDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Generlising Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValGenerlisingDate" runat="server" ControlToValidate="txtGenerlisingDate" Display="Dynamic" Text="Invalid date."
                                    ErrorMessage="Invalid Generlising Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" 
                                    Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGenerlisingDate" runat="server" Text='<%# Bind("GenerlisingDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgGenerlisingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Number
                                <asp:RequiredFieldValidator ID="RFVInvoiceEdit" runat="server" ControlToValidate="txtInvoiceNo"
                                    SetFocusOnError="true" ErrorMessage="Invoice Number Required" Display="Dynamic" Text="*" ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Text='<%# Bind("InvoiceNo") %>'></asp:TextBox>
                            </td>
                            <td>
                                Invoice Date
                                <cc1:CalendarExtender ID="calInvoiceEdit" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvoiceEdit" PopupPosition="BottomRight"
                                    TargetControlID="txtInvoiceDate">
                                </cc1:CalendarExtender>
                                <asp:CompareValidator ID="ComValDateEdit" runat="server" ControlToValidate="txtInvoiceDate" Display="Dynamic"
                                    ErrorMessage="Invalid Invoice Date." Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck"
                                    ValidationGroup="RequiredInvoice">
                                </asp:CompareValidator>
                            </td>
                            <td>
                               <asp:TextBox ID="txtInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate","{0:dd/MM/yyyy}") %>' Width="100px"></asp:TextBox>
                               <asp:Image ID="imgInvoiceEdit" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Amount
                                <asp:RequiredFieldValidator ID="RFVAmountEdit" runat="server" ControlToValidate="txtInvoiceAmount"
                                    SetFocusOnError="true" ErrorMessage="*" Display="Dynamic"
                                    Text="*" ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegxAmount" runat="server" ControlToValidate="txtInvoiceAmount"
                                    SetFocusOnError="true" ErrorMessage="Invalid Amount." Display="Dynamic"
                                    ValidationGroup="RequiredInvoice" ValidationExpression="^[0-9]\d{0,10}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceAmount" runat="server" Text='<%# Bind("InvoiceAmount") %>' MaxLength="10"></asp:TextBox>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>        
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:Button ID="btnInsertButton" Text="Save" ValidationGroup="RequiredInvoice" runat="server" CommandName="Insert" />
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
              
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Draft Invoice Prepared Date
                                <cc1:CalendarExtender ID="CalNewDraftInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNewDraftInvoiceDate" PopupPosition="BottomRight"
                                    TargetControlID="txtNewDraftInvoiceDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVDraftInvoiceDate" runat="server" ControlToValidate="txtNewDraftInvoiceDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Draft Invoice Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompValDraftInvoiceDate" runat="server" ControlToValidate="txtNewDraftInvoiceDate" Display="Dynamic" Text="Invalid date."
                                    ErrorMessage="Invalid Draft Invoice Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewDraftInvoiceDate" runat="server" Text='<%# Bind("DraftInvoiceDate") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgNewDraftInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>
                                Checking Date
                                <cc1:CalendarExtender ID="CalNewCheckingDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNewCheckingDate" PopupPosition="BottomRight"
                                    TargetControlID="txtNewCheckingDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVNewCheckingDate" runat="server" ControlToValidate="txtNewCheckingDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Checking Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValCheckingDate" runat="server" ControlToValidate="txtNewCheckingDate" Display="Dynamic" 
                                    Text="Invalid date." ErrorMessage="Invalid Checking Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" 
                                    Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewCheckingDate" runat="server" Text='<%# Bind("CheckingDate") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgNewCheckingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Final Typing Date (Back to Back Billing)
                                <cc1:CalendarExtender ID="CalNewTypingDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNewTypingDate" PopupPosition="BottomRight"
                                    TargetControlID="txtNewTypingDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVNewTypingDate" runat="server" ControlToValidate="txtNewTypingDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Final Typing Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValTypingDate" runat="server" ControlToValidate="txtNewTypingDate" Display="Dynamic" 
                                    Text="Invalid date." ErrorMessage="Invalid Final Typing Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" 
                                    Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewTypingDate" runat="server" Text='<%# Bind("FinalTypingDate") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgNewTypingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>
                                Generlising Date (Documents as per Customer)
                                <cc1:CalendarExtender ID="CalNewGenerlisingDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNewGenerlisingDate" PopupPosition="BottomRight"
                                    TargetControlID="txtNewGenerlisingDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVGenerlisingDate" runat="server" ControlToValidate="txtNewGenerlisingDate" SetFocusOnError="true"
                                    ValidationGroup="RequiredInvoice" Text="*" ErrorMessage="Please Enter Generlising Date."></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValGenerlisingDate" runat="server" ControlToValidate="txtNewGenerlisingDate" Display="Dynamic" Text="Invalid date."
                                    ErrorMessage="Invalid Generlising Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" 
                                    Operator="DataTypeCheck" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewGenerlisingDate" runat="server" Text='<%# Bind("GenerlisingDate") %>' Width="100px"></asp:TextBox>
                                <asp:Image ID="imgNewGenerlisingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Number
                                <asp:RequiredFieldValidator ID="RFVNewInvoice" runat="server" ControlToValidate="txtNewInvoiceNo" SetFocusOnError="true" 
                                    ErrorMessage="Invoice Number Required" Display="Dynamic" Text="*" ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewInvoiceNo" runat="server" Text='<%# Bind("InvoiceNo") %>'></asp:TextBox>
                            </td>
                            <td>
                                Invoice Date
                                <asp:RequiredFieldValidator ID="RFVNewINvoiceDate" runat="server" ControlToValidate="txtNewInvoiceDate"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice Date" Display="Dynamic"
                                    ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                <cc1:CalendarExtender ID="calNewInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNewInvoiceDate" PopupPosition="BottomRight"
                                    TargetControlID="txtNewInvoiceDate">
                                </cc1:CalendarExtender>
                                <asp:CompareValidator ID="ComValNewInvoiceDate" runat="server" ControlToValidate="txtNewInvoiceDate" Display="Dynamic"
                                    ErrorMessage="Invalid Invoice Date." Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck"
                                    ValidationGroup="RequiredInvoice">
                                </asp:CompareValidator>
                            </td>
                            <td>
                               <asp:TextBox ID="txtNewInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate","{0:dd/MM/yyyy}") %>' Width="100"></asp:TextBox>
                               <asp:Image ID="imgNewInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Amount
                                <asp:RequiredFieldValidator ID="RFVNewAmount" runat="server" ControlToValidate="txtNewInvoiceAmount"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice Amount" Display="Dynamic"
                                    ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegxNewAmount" runat="server" ControlToValidate="txtNewInvoiceAmount"
                                    SetFocusOnError="true" ErrorMessage="Invalid Amount." Display="Dynamic"
                                    ValidationGroup="RequiredInvoice" ValidationExpression="^[0-9]\d{0,10}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewInvoiceAmount" runat="server" Text='<%# Bind("InvoiceAmount") %>' MaxLength="10"></asp:TextBox>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                          
                </InsertItemTemplate>
            </asp:FormView>
        
             <div id="divGridView">
            <asp:GridView ID="gvPCDInvoice" runat="server" AutoGenerateColumns="False" Width="99%" AutoGenerateSelectButton="true" 
                CssClass="table" PageSize="20" PagerSettings-Position="TopAndBottom" DataKeyNames="lId" PagerStyle-CssClass="pgr"  
                AllowPaging="true" AllowSorting="true" OnSelectedIndexChanged="gvPCDInvoice_SelectedIndexChanged" 
                DataSourceID="GridviewSqlDataSource" OnRowDataBound="gvPCDInvoice_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Invoice No" SortExpression="InvoiceNo">
                        <ItemTemplate>
                            <asp:LinkButton CausesValidation="false" ID="lnkInvoiceNo" Text='<%#Eval("InvoiceNo") %>'
                                runat="server" CommandName="Select" OnClick="lnkInvoiceNo_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="DraftInvoiceDate" HeaderText="Draft Invoice Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DraftInvoiceDate" />
                    <asp:BoundField DataField="CheckingDate" HeaderText="Checking Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CheckingDate" />
                    <asp:BoundField DataField="FinalTypingDate" HeaderText="Final Typing Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FinalTypingDate" />
                    <asp:BoundField DataField="GenerlisingDate" HeaderText="Generlising Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="GenerlisingDate" />
                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="InvoiceDate" />
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Invoice Amount" SortExpression="InvoiceAmount" />
                </Columns>
            </asp:GridView>
            
        </div>
        <div id="divDatasource">
            <asp:SqlDataSource ID="DataSourcePCDInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetPCDInvoiceById" SelectCommandType="StoredProcedure"
                InsertCommand="insPCDInvoice" InsertCommandType="StoredProcedure"
                UpdateCommand="updPCDInvoice" UpdateCommandType="StoredProcedure"
                DeleteCommand="delPCDInvoice" DeleteCommandType="StoredProcedure"
                OnInserted="DataSourceFormView_Inserted" OnUpdated="DataSourceFormView_Updated"
                OnDeleted="DataSourceFormView_Deleted" >
                <SelectParameters>
                    <asp:ControlParameter Name="lId" ControlID="gvPCDInvoice" PropertyName="SelectedValue" />
                </SelectParameters>
                <InsertParameters>
                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    <asp:Parameter Name="DraftInvoiceDate" DbType="dateTime" />
                    <asp:Parameter Name="CheckingDate" DbType="dateTime" />
                    <asp:Parameter Name="FinalTypingDate" DbType="dateTime" />
                    <asp:Parameter Name="GenerlisingDate" DbType="dateTime" />
                    <asp:Parameter Name="InvoiceNo" />
                    <asp:Parameter Name="InvoiceDate" DbType="dateTime" />
                    <asp:Parameter Name="InvoiceAmount" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="lId" />
                    <asp:Parameter Name="DraftInvoiceDate" DbType="dateTime" />
                    <asp:Parameter Name="CheckingDate" DbType="dateTime" />
                    <asp:Parameter Name="FinalTypingDate" DbType="dateTime" />
                    <asp:Parameter Name="GenerlisingDate" DbType="dateTime" />
                    <asp:Parameter Name="InvoiceNo" />
                    <asp:Parameter Name="InvoiceDate" DbType="dateTime" />
                    <asp:Parameter Name="InvoiceAmount" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="lId" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Direction="Output" Size="4"/>
                </DeleteParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetPCDInvoice" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                </SelectParameters> 
            </asp:SqlDataSource>
        </div>
            
       </table> 
        </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="TabPCDDocument" runat="server" HeaderText="PCA Document">
                <ContentTemplate>
                <%--<table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            PCD Document
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPCDDocument" runat="server"> </asp:DropDownList>
                        </td>
                        <td>
                    </tr>
                    <tr>
                        <td colspan="3" class="m">
                            <asp:FileUpload ID="fuDocument" runat="server" />
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"  />
                        </td>
                    </tr>
                </table>--%>
                
                <div class="m clear"></div>
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
                            <asp:BoundField DataField="DocumentName" HeaderText="PCA Document" />
                            <asp:BoundField DataField="PCDToCustomer" HeaderText="PCA To Customer" />
                            <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                            <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <div>
                    <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="JobId" SessionField="JobId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
    </div>
    </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>        
    
    </ContentTemplate>
   </asp:UpdatePanel> 
 </asp:Content>  
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewInvoice.aspx.cs" Inherits="InvoiceTrack_NewInvoice" 
    MasterPageFile="~/MasterPage.master" Culture="en-GB" Title="Invoice Received" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        function OnVendorSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtGSTN.ClientID%>').value = results.GSTIN;
            $get('<%=hdnPartyCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorName.ClientID%>').focus();
        }
        function OnGSTINSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnPartyCode.ClientID%>').value = results.Code;
        }
    
        function ActiveTabChanged12()
        {  
            /* Clear Error Message on Tab change Event */          
           document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
           document.getElementById('<%=lblError.ClientID%>').className = '';
        }

        function calculateAmount()
        {
            var objAmount = 0.0; var objGstRate = 0.0; var objTotalAmount = 0.0;

            objAmount = $get('<%=txtTaxAmount.ClientID%>').value
            var objGstRate = $get('<%=txtGSTRate.ClientID%>').value

            objGSTAmount = (objAmount * objGstRate) / 100.00;

            $get('<%=txtGSTAmount.ClientID%>').value = objGSTAmount;

            objTotalAmount = parseFloat(objAmount) + parseFloat(objGSTAmount);
            $get('<%=txtTotalAmount.ClientID%>').value = objTotalAmount;


            /*display the result*/
          //  var tot_price=price+(price*0.18);
           // var divobj = document.getElementById('tot_amount');
          //  divobj.value = tot_price;
        }
    

    </script>

<asp:UpdatePanel ID="upJobDetail" runat="server">
    <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="InvoiceRequired" CssClass="errorMsg" EnableViewState="false" />
        <asp:HiddenField ID="hdnPartyCode" runat="server" />
    </div>
    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="Tab" CssTheme="None"
        OnClientActiveTabChanged="ActiveTabChanged12">
    <cc1:TabPanel ID="TabJob" runat="server" HeaderText="Invoice Received">
        <ContentTemplate>
            <fieldset><legend>Invoice Detail </legend>
                <div class="m clear">
                <asp:Button ID="btnSaveInvoice" runat="Server" Text="Save Invoice" OnClick="btnSaveInvoice_Click" 
                OnClientClick="if(Page_ClientValidate('InvoiceRequired')) return confirm('Sure to Add Invoice?'); return false;" ValidationGroup="InvoiceRequired" />
                <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server" />
                </div>    
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Vendor Name
                            <asp:RequiredFieldValidator ID="RFVVendorName" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtVendorName" InitialValue="" ErrorMessage="Please Enter Vendor Name"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorName" runat="server"></asp:TextBox>
                            <div id="divwidthVendor"></div>
                            <cc1:AutoCompleteExtender ID="AutoCompleteVendor" runat="server" TargetControlID="txtVendorName"
                                CompletionListElementID="divwidthVendor" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthVendor"
                                ContextKey="8868" UseContextKey="True" OnClientItemSelected="OnVendorSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            GST No
                            <asp:RequiredFieldValidator ID="RFVGSTN" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtGSTN" InitialValue="" ErrorMessage="Please Enter Vendor GST No"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTN" runat="server" MaxLength="15"></asp:TextBox>
                            <div id="divwidthGST"></div>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtenderGST" runat="server" TargetControlID="txtGSTN"
                                CompletionListElementID="divwidthGST" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                ServiceMethod="GetCompletionListByGSTIN" MinimumPrefixLength="2" BehaviorID="divwidthGST"
                                ContextKey="8844" UseContextKey="True" OnClientItemSelected="OnGSTINSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            Company
<%--                            <asp:RequiredFieldValidator ID="RFVComp" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="ddCompany" InitialValue="0" ErrorMessage="Please Select Group Company"
                                Text="*"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCompany" runat="server">
                                <asp:ListItem Text="BABAJI SHIVRAM CLEARING & CARRIERS PVT LTD." Value="BSCCPL" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="BABJI SHIVRAM GLOBAL SUPPLY BASE PRIVATE LIMITED" Value="BSGSBP"></asp:ListItem>
                                <asp:ListItem Text="BABAJI SHIVRAM GLOBAL SERVICES PTE.LTD.SINGAPORE" Value="BSGSPL"></asp:ListItem>
                                <asp:ListItem Text="Babaji Shivram Project Forwarding LLC" Value="BSPF"></asp:ListItem>
                                <asp:ListItem Text="DAVADA SHIPPING AGENCIES PRIVATE LIMITED" Value="DSA"></asp:ListItem>
                                <asp:ListItem Text="INDIA SHIPPING AGENCIES" Value="ISA"></asp:ListItem>
                                <asp:ListItem Text="NAVBHARAT CLEARING AGENTS PVT LTD." Value="NBCPL"></asp:ListItem>
                                <asp:ListItem Text="NAV JEEVAN AGENCIES" Value="NJA"></asp:ListItem>
                                <asp:ListItem Text="UNITED COMMERCIAL CORPORATION" Value="UCC"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Branch
                            <asp:RequiredFieldValidator ID="RFVBabajiBranch" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="ddBranch" InitialValue="0" ErrorMessage="Please Select Babaji Branch"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddBranch" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Division
                            <asp:RequiredFieldValidator ID="RFVDivision" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="ddDivision" InitialValue="0" ErrorMessage="Please Select Division"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDivision" runat="server">
                                <asp:ListItem Text="--Division--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="CHA" Value="CH" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Container Agency" Value="CA"></asp:ListItem>
                                <asp:ListItem Text="CFS" Value="CF"></asp:ListItem>
                                <asp:ListItem Text="EC" Value="EC"></asp:ListItem>
                                <asp:ListItem Text="FF" Value="Freight Forwarding"></asp:ListItem>
                                <asp:ListItem Text="Marine Logistics" Value="ML"></asp:ListItem>
                                <asp:ListItem Text="TS" Value="Transport"></asp:ListItem>
                                <asp:ListItem Text="WH" Value="Warehouse"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Job Number</td>
                        <td>
                            <asp:TextBox ID="txtJobRefNo" runat="server" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Invoice No
                            <asp:RequiredFieldValidator ID="RFVInvoiceNo" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtInvoiceNo" InitialValue="" ErrorMessage="Please Enter Invoice No"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            Invoice Date
                            <cc1:CalendarExtender ID="calInvoiceDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgMAWBDate"
                                Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtInvoiceDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MskExtInvoiceDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MskValInvoiceDate" ControlExtender="MskExtInvoiceDate" ControlToValidate="txtInvoiceDate" IsValidEmpty="false" 
                                InvalidValueMessage="Invalid Invoice Date" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" 
                                MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2017"
                                MinimumValueBlurredText="*" MaximumValueBlurredMessage="*"
                                EmptyValueBlurredText="*" EmptyValueMessage="Invoice Date Required"  
                                Runat="Server" ValidationGroup="InvoiceRequired"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td>
                            Taxable Amount
                            <asp:RequiredFieldValidator ID="RFVTaxAmount" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtTaxAmount" InitialValue="" ErrorMessage="Please Enter Tax Amount"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTaxAmount" runat="server" TextMode="Number" MaxLength="20" onchange="calculateAmount();" Width="80px"></asp:TextBox>
                            <asp:RadioButtonList ID="rdTaxType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="IGST" Value="1"></asp:ListItem>
                                <asp:ListItem Text="CGST/SGST" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RFVType" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="rdTaxType" InitialValue="" ErrorMessage="Please Select GST Applicable Type"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            GST Rate %
                        </td>
                        <td>
                           <asp:TextBox ID="txtGSTRate" runat="server" Width="70px" MaxLength="4" TextMode="Number" onchange="calculateAmount()"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVTaxrate" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtGSTRate" InitialValue="" ErrorMessage="Please Enter Tax Rate"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            GST Amount
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTAmount" runat="server" PlaceHolder="GST Amount" Text="0" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVGSTAmount" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtGSTAmount" InitialValue="0" ErrorMessage="Please Calculate GST Amount"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Total Amount
                            <asp:RequiredFieldValidator ID="RFVTotalAmount" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtTotalAmount" InitialValue="0" ErrorMessage="Please Calculate Total Amount"
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalAmount" runat="server" Width="100px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Payment Terms</td>
                        <td>
                            <asp:DropDownList ID="ddPaymentTerms" runat="server">
                                <asp:ListItem Text="30 Days" Value="30 Days" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="60 Days" Value="60 Days"></asp:ListItem>
                                <asp:ListItem Text="90 Days" Value="90 Days"></asp:ListItem>
                                <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                                <asp:ListItem Text="Immeidate" Value="Immeidate"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Category Of Vendor</td>
                        <td>
                            <asp:DropDownList ID="ddVendorCategory" runat="server">
                                <asp:ListItem Text="Admin" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Agency" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Reimbursement" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                       <td>MSME ?
                           <asp:RequiredFieldValidator ID="RFVMSME" ValidationGroup="InvoiceRequired" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="rdMSME" InitialValue="" ErrorMessage="Please Select MSME"
                                Text="*"></asp:RequiredFieldValidator>
                       </td>
                        <td>
                            <asp:RadioButtonList ID="rdMSME" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                            
                        </td>
                        <td>
                            MSME Certificate
                        </td>
                        <td>
                            <asp:FileUpload  ID="FileMSME" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remark / Instrunctions
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            Invoice Copy
                        </td>
                        <td>
                            <asp:FileUpload  ID="FileInvoice" runat="server"/>
                        </td>
                    </tr>
                    
                </table>
            </fieldset>
            
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel ID="TabInvoice" runat="server" HeaderText="Forward Invoice">
        <ContentTemplate>
            <div style="overflow:scroll; width:100%">
            <fieldset class="fieldset-AutoWidth">
                <legend>Forward To HO</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear"></div>
                
                <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" 
                    DataKeyNames="lid" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20" PagerSettings-Position="TopAndBottom" 
                    OnRowDataBound="gvInvoice_RowDataBound" OnRowCommand="gvInvoice_RowCommand" DataSourceID="DataSourcePendingInvoice">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Forward To HO">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkForwardInvoice" CommandName="forward" runat="server" Text="Forward"
                                    CommandArgument='<%#Eval("lId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="Status"
                            ReadOnly="true" />
                        <asp:BoundField DataField="TokanNo" HeaderText="Token No" SortExpression="TokanNo"
                            ReadOnly="true" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                            ReadOnly="true" />    
                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"
                            ReadOnly="true" />
                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}"
                            SortExpression="InvoiceDate" ReadOnly="true" />
                        <asp:TemplateField HeaderText="GST No">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTNo" Text='<%# Eval("GSTNo")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalAmount" Text='<%# Eval("TotalAmount")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxAmount" Text='<%# Eval("TaxAmount")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CGST" HeaderText="CGST" SortExpression="CGST" ReadOnly="true" />
                        <asp:BoundField DataField="SGST" HeaderText="SGST" SortExpression="SGST" ReadOnly="true" />
                        <asp:BoundField DataField="IGST" HeaderText="IGST" SortExpression="IGST" ReadOnly="true" />
                        <asp:BoundField DataField="BillTypeName" HeaderText="Invoice Type" SortExpression="BillTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                        <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />
                        
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourcePendingInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="INV_GetInvoiceByStatus" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </cc1:TabPanel>
    
</cc1:TabContainer>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LetterGenerate.aspx.cs"
    Inherits="Reports_LetterGenerate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1"></cc1:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upStockReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upStockReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="True" ShowSummary="true" ValidationGroup="Required" CssClass="errorMsg" />
            <fieldset>
                <legend>Letter Generate</legend>
                <div>
                    <table>
                        <tr>
                            <td>
                                Job Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBEType" runat="server">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Inbond" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Exbond" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVFile2" runat="server" ControlToValidate="ddlBEType" SetFocusOnError="true"
                                    InitialValue="0"
                                    Text="*" ErrorMessage="Select Letter Type" ValidationGroup="Required"></asp:RequiredFieldValidator>

                            </td>
                            <td>Job Number</td>
                            <td>
                                <asp:TextBox ID="txtJobId" runat="server" Width="150px"></asp:TextBox>
                                <cc1:AutoCompleteExtender ServiceMethod="GetCompletionList" MinimumPrefixLength="1"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtJobId"
                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                                <asp:RequiredFieldValidator ID="RFVFile1" runat="server" ControlToValidate="txtJobId" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Enter Job No" ValidationGroup="Required"></asp:RequiredFieldValidator>

                            </td>
                            <td>Letter Type</td>
                            <td>
                                <asp:DropDownList ID="ddlLetterType" runat="server">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Annexure" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Triple Duty Bond" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Sales Purchase Letter" Value="3"></asp:ListItem>

                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="ddlLetterType" SetFocusOnError="true"
                                    InitialValue="0"
                                    Text="*" ErrorMessage="Select Letter Type" ValidationGroup="Required"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" ValidationGroup="Required" />
                                <asp:Button ID="btnExport" runat="server" Text="PDF" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                        </tr>
                    </table>
                </div>

                <br />
                <%--REQUEST LETTER--%>
                <div id="dvRequestLetter" runat="server" align="center" visible="false">
                    <body>
                        <table width="95%" runat="server" style="border: solid; border-width: thin">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <caption>
                                <br />
                                <br />
                                <tr>
                                    <td align="right">DT.14.05.20&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">To</td>
                                </tr>
                                <tr>
                                    <td align="left">THE DY. COMMI.OF CUSTOMS</td>
                                </tr>
                                <tr>
                                    <td align="left">JNCH. </td>
                                </tr>
                                <tr>
                                    <td align="left">JNPT, NHAVA SHEVA</td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Sub :
                                        <asp:Label ID="lblSub" runat="server"></asp:Label>
                                    </b></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Ref :
                                        <asp:Label ID="lblRef" runat="server"></asp:Label>
                                    </b></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Sub :- Request letter </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Dear Sir,</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp; WE M/s. EMCURE PHARMACEUTICALS LTD . having our office located at MIDC PLOT NO. P-2, PUNE INFOTECH,PARK, PHASE-II HINJEWADI,PUNE, PIN-411057 Due to lockdown COVID-19, the physical submission of documents and undertaking is not possible. So original bond will be submitted after lockdown. as per notification of P.N. No.45/20 dtd.07.04.20. In view of the above we request your good self to kindly accept Bond online and complete the bond formalities. </td>
                                </tr>
                                <tr>
                                    <td>Thanking you,</td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Yours faithfully For </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>AUTHORISED SIGNATORY</td>
                                </tr>
                            </caption>
                        </table>
                    </body>
                </div>
                <%--REQUEST LETTER--%>

                <%--Annexure--%>
                <div id="dvAnnexure" runat="server" align="center" visible="false">
                    <body>
                        <table width="80%" runat="server" style="border: solid; border-width: thin; font-size: 15px; padding: 10px 10px;">
                            <tr>
                                <td align="center" colspan="2">Form of application for Transfer of goods from one Warehouse to another in Port
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">The Dy. Commissioner of Customs,
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">Bond Department
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">Nhava Sheva,<br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">Dear Sir,
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">Please permit the transfer of the under mentioned goods for the unexpired period of warehousing under Section 67 of the Customs Act, 1962 as per the details given below:
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Bond Number &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblbndNum" runat="server"></asp:Label></b>
                                </td>
                                <td>To Warehouse &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblToWh" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Date of Bond Registration &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lbldtofbondReg" runat="server"></asp:Label></b>
                                </td>
                                <td>(Name & Address) &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblNameAdd" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>From Ware House &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblFromWH" runat="server"></asp:Label></b>
                                </td>
                                <td></td>
                            </tr>
                            <tr align="left">
                                <td>(Name & Address) &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblFmNameAdd" runat="server"></asp:Label></b>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr align="left">
                                <td>S.S &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblSS" runat="server"></asp:Label></b>
                                </td>
                                <td>Port &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblPort" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>PRESENT OWNER &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    <b>
                                        <asp:Label ID="lblPresentOwner" runat="server"></asp:Label></b>
                                </td>
                                <td>M/S.   &nbsp;&nbsp;&nbsp;:-&nbsp;
                                   <b>
                                       <asp:Label ID="lblMS" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>Date of transfer granted</n> &nbsp;&nbsp;&nbsp;:-&nbsp;
                                    (To be filled up by Department
                                
                                    <b>
                                        <asp:Label ID="lblDtOfTransferGranted" runat="server"></asp:Label></b>
                                </td>
                                <td>Regd. Off.: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <div style="word-wrap: break-word; width: 300px; white-space: normal;">
                                        <b>
                                            <asp:Label ID="lblRegdOff" runat="server"></asp:Label></b>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left"><b>IGM details:
                                    <asp:Label ID="lblIGMDetail" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                IGM No. : 
                                    <asp:Label ID="lblIGMNo" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                DT: 
                                    <asp:Label ID="lblDt" runat="server"></asp:Label></b>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><b>Line No. 145 
                                    <asp:Label ID="lbllineNo145" runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    Dt: 
                                    <asp:Label ID="lblBOEDt" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:GridView ID="gvAnnexure" runat="server" AutoGenerateColumns="false"
                                        CssClass="table" Width="90%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex +1%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Unit" HeaderText="Unit" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="No">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex +1%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemNo" HeaderText="ItemNo" ReadOnly="true" />
                                            <asp:BoundField DataField="WEIGHT" HeaderText="WEIGHT" ReadOnly="true" />
                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ReadOnly="true" />
                                            <asp:BoundField DataField="CTHNo" HeaderText="CTHNo" ReadOnly="true" />
                                            <asp:BoundField DataField="AssessableValue" HeaderText="AssessableValue" ReadOnly="true" />
                                            <asp:BoundField DataField="RateOfDuty" HeaderText="RateOfDuty" ReadOnly="true" />

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">New Bond No. 
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                Yours faithfully,
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Yours faithfully,</td>
                            </tr>
                            <tr>
                                <td colspan="2">(To be filled up
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <b>
                                       <asp:Label ID="lblConsigneeNm" runat="server"></asp:Label>
                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblInBondCust" runat="server"></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                     by Department)
                                </td>
                            </tr>
                        </table>
                    </body>
                </div>
                <%--Annexure--%>

                <%--Triple Duty Bond--%>
                <div id="dvTripleDutyBond" runat="server" align="center" visible="false">
                    <body>
                        <table width="80%" runat="server" style="border: solid; border-width: thin; font-size: 14px; padding: 10px 10px;">
                            <tr>
                                <td>
                                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="2" style="font-size: 20px;"><b>Consignment Bond</b><br /><br /></td>
                            </tr>
                            <tr>
                                <td colspan="2">(Bond to be executed by the importer under sub-section (1) of Section 59 of the Customs Act, 1962for the purpose of warehousing of goods to be imported by them.)
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="2">
                                    <b>[In terms of Circular No.18/2016-Customs]</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">KNOW ALL MEN BY THESE PRESENTS THAT WE <b> M/s. 
                                    <asp:Label ID="lblConName" runat="server"></asp:Label>
                                </b>. having our office located <b>at
                                    <asp:Label ID="lblConsigneeAdd" runat="server"></asp:Label>
                                </b>. and holding <b>
                                    <asp:Label ID="lblIECNo" runat="server"></asp:Label>
                                </b>, here in after referred to as the “importer”, (which expression shall include our successors, heirs, executors, administrators and legal representatives) hereby jointly and severally bind ourselves to the President of India hereinafter referred to as the “President” (which expression shall include his successors and assigns) in the sum of Rs. <b>
                                    <asp:Label ID="lblTotalDuty" runat="server"></asp:Label></b>  to be paid to the President, for which payment well and truly to be made, we bind ourselves, our successors, heirs, executors, administrators and legal representatives firmly by these presents.<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <b>Sealed with our seal(s) this 11TH day of JULY 2020.</b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">WHEREAS, we the importers have filed a <b>
                                    <asp:Label ID="lblBOEDetail" runat="server"></asp:Label>
                                </b>for warehousing under section 46 of the Customs Act, (hereinafter referred to as the said Act) and the same has been assessed to duty under section 17 or section 18 of the said Act (strike which is not applicable) in respect of goods mentioned below.  
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:GridView ID="gvTrippleDuty" runat="server" AutoGenerateColumns="false"
                                        CssClass="table" Width="80%">
                                        <Columns>
                                            <asp:BoundField DataField="Port" HeaderText="PORT OF IMPORT" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="DESCRIPTION OF GOODS">
                                                <ItemTemplate>
                                                    <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Bind("DESCRIPTION") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SI No of Invoice">
                                                <ItemTemplate>
                                                    <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                                        <asp:Label ID="lblNoOfInvoice" runat="server" Text='<%#Bind("InvoiceDetail") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DESCRIPTION  AND NO OF PACKAGES">
                                                <ItemTemplate>
                                                    <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                                        <asp:Label ID="lblNoOfPck" runat="server" Text='<%#Bind("NoOfPack") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AssessableValue" HeaderText="VALUE" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="WAREHOUSE CODE AND ADDRESS">
                                                <ItemTemplate>
                                                    <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                                        <asp:Label ID="lblWHCode" runat="server" Text='<%#Bind("InBondDetails") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>


                            <tr>
                                <td colspan="2">AND WHEREAS Section 59(1) of the said Act requires the execution of a bond equal to thrice the amount of duty assessed on goods for which a bill of entry for warehousing has been presented under the said section46.
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <b>NOW THE CONDITIONS of the above written bond is such that, if we:</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">(1) Comply with all the provisions of the Act, the rules and regulations made thereunder in respect of such goods;<br />
                                    (2) Pay on or before the specified date in the notice of demand, all duties and interest payable under sub-section (2) of section 61; and<br />
                                    (3) Pay all penalties and fines incurred for contravention of the provisions of the Act or the rules or regulations made thereunder, in respect of such goods;<br />
                                    (4) In the event of our failure to discharge our obligation, pay the full amount of duty chargeable on account of such goods together with their interest, fine and penalties payable under section 72, in respect of such goods;<br />
                                    Then the above written bond shall be void and of no effect otherwise the same shall remain in full force and virtue.<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <b>IT IS HEREBY AGREED AND DECLARED that:</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">(1) The Bond shall continue in full force notwithstanding the transfer of the goods to another warehouse.<br />
                                    (2) The President through the Deputy/Assistant Commissioner or any other officer may recover any amount due under this Bond in the manner laid down under sub-section (1) of section 142 of the said Act without prejudice to any other mode of recovery.<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <b>IN THE WITNESS WHEREOF,</b> the importer has herein, set and subscribed his hands and seals the day, month and year first above written.
                                    SIGNED AND DELIVERED by or on behalf of the importer at Mumbai in the presence of:
                                    (Signature(s) of the importer/authorised signatory)<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><b>Witness:</b></td>
                            </tr>
                            <tr>
                                <td colspan="2">Name & Signature<br />
                                    <br />
                                    1. : 
                                    <br />
                                    <br />
                                    <br />

                                    2. :<br />
                                    <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr align="center">
                                <td>Accepted by me this</td>
                                <td style="text-align: left;">For and on behalf of the President of India.<br />
                                    (Assistant/Deputy Commissioner)   
                                    <br />
                                    Signature and date<br />
                                    Name:<br />
                                </td>
                            </tr>
                        </table>
                    </body>
                </div>
                <%--Triple Duty Bond--%>

                <%--Sales Purchase Letter--%>
                <div id="dvSalePurchaseLetter" runat="server" align="center" visible="false">
                    <body>
                        <table width="80%" runat="server" style="border: solid; border-width: thin; font-size: 14px; padding: 10px 10px;">
                            <tr align="right">
                                <td>
                                    <asp:Label ID="lblTodayDt1" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>To
                                    <br />
                                    The Assistant Commission of Customs
                                    <br />
                                    Bond Department<br />
                                    Jawahar Custom House,<br />
                                    Nhava Sheva<br />
                                    <br />
                                    Dear Sir,
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Sub :
                                        <asp:Label ID="lblSub1" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Ref :
                                        <asp:Label ID="lblRef1" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>With reference to the captioned bonded of the goods we wish to bring to your kind notice that we intend to transfer the same to M/S .<b><asp:Label ID="lblExCustNm" runat="server"></asp:Label>
                                </b>.. You are requested to allow the transfer as provided under provision to sub section of (3) section 59 of the customs act 1962.
                                    <br />
                                    <br />

                                    The said M/S <b>
                                        <asp:Label ID="lblEXCustNm1" runat="server"></asp:Label>
                                    </b>. will execute the necessary Bond in lieu of the double duty executed and submitted by us. And thereafter the transferee shall be entitled  to clear the goods as and when they deem fit or as may be directed by the authorities of customs.
                                    <br />
                                    <br />

                                    You are requested to permit the same and oblige.<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Thanking you,<br />
                                    <br />

                                    Yours faithfully,<br />
                                    <b>
                                        <asp:Label ID="lblYoursFaithfully1" runat="server"></asp:Label><br />
                                        <br />
                                        <br />
                                        Authorized Signatory.<br />
                                        <br />
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 1px solid darkgray; border-radius: 6px; beditid: r3st1; color: #000000; blabel: main; font-size: 12pt; font-family: calibri;"></td>
                            </tr>


                            <tr align="right">
                                <td>
                                    <asp:Label ID="lblTodayDt2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>To
                                    <br />
                                    The Assistant Commission of Customs
                                    <br />
                                    Bond Department<br />
                                    Jawahar Custom House,<br />
                                    Nhava Sheva<br />
                                    <br />
                                    Dear Sir,
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Sub :
                                        <asp:Label ID="lblSub2" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Ref :
                                        <asp:Label ID="lblRef2" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>With reference to the above , please find enclosed herewith the letter of M/s.<b>
                                    <asp:Label ID="lblInbondDetail" runat="server"></asp:Label>
                                </b>with itself is self explanatory. We confirm having acquired the ownership of the subject goods as per provision to sub-section 3 of section 59 of the Customs act 1962. Read with Notification No. 64/93-CVS (N.T.) dated 27th December 1994.<br />
                                    <br />

                                    We submit here with the Double duty bond as required under the provision of section 59 of CA 62 for the entire consignment and request that the warehoused goods in question please be transferred in our name.<br />
                                    <br />

                                    We shall be clearing the goods at the earliest.<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Thanking you,<br />
                                    <br />

                                    Yours faithfully,<br />
                                    <b>
                                        <asp:Label ID="lblYoursFaithfully2" runat="server"></asp:Label><br />
                                        <br />
                                        <br />
                                        Authorized Signatory.<br />
                                        <br />
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 1px solid darkgray; border-radius: 6px; beditid: r3st1; color: #000000; blabel: main; font-size: 12pt; font-family: calibri;"></td>
                            </tr>


                            <tr align="right">
                                <td>
                                    <asp:Label ID="lblTodayDt3" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>To
                                    <br />
                                    The Assistant Commission of Customs
                                    <br />
                                    Bond Department<br />
                                    Jawahar Custom House,<br />
                                    Nhava Sheva<br />
                                    <br />
                                    Dear Sir,
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Sub :
                                        <asp:Label ID="lblSub3" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Ref :
                                        <asp:Label ID="lblRef3" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>We wish to bring to your kind notice that we have sold the above consignment to :-<br />
                                    <br />
                                    <b>
                                        <asp:Label ID="lblDetail" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                            <tr>
                                <td>Thanking you,<br />
                                    <br />

                                    Yours faithfully,<br />
                                    <b>
                                        <asp:Label ID="lblYoursFaithfully3" runat="server"></asp:Label><br />
                                        <br />
                                        <br />
                                        Authorized Signatory.<br />
                                        <br />
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 1px solid darkgray; border-radius: 6px; beditid: r3st1; color: #000000; blabel: main; font-size: 12pt; font-family: calibri;"></td>
                            </tr>


                            <tr align="right">
                                <td>
                                    <asp:Label ID="lblTodayDt4" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>To
                                    <br />
                                    The Assistant Commission of Customs
                                    <br />
                                    Bond Department<br />
                                    Jawahar Custom House,<br />
                                    Nhava Sheva<br />
                                    <br />
                                    Dear Sir,
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Sub :
                                        <asp:Label ID="lblSub4" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Ref :
                                        <asp:Label ID="lblRef4" runat="server"></asp:Label></b><br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>We have purchased the above goods from <b>M/s.<asp:Label ID="lblInDetail1" runat="server"></asp:Label></b>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Thanking you,<br />
                                    <br />

                                    Yours faithfully,<br />
                                    <b>For<asp:Label ID="lblYoursFaithfully4" runat="server"></asp:Label><br />
                                        <br />
                                        <br />
                                        Authorized Signatory.<br />
                                        <br />
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 1px solid darkgray; border-radius: 6px; beditid: r3st1; color: #000000; blabel: main; font-size: 12pt; font-family: calibri;"></td>
                            </tr>
                        </table>
                    </body>
                </div>
                <%--Sales Purchase Letter--%>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


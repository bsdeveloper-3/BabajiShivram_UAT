<%@ Page Title="Vendor KYC Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VendorKYCDetail.aspx.cs"
    Inherits="Service_VendorKYCDetail" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager2" ScriptMode="Release" />
    <asp:UpdatePanel ID="upnlVendorApproval" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="errorMsg"></asp:Label>
                <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                <fieldset>
                    <legend>Vendor Details  </legend>
                    <asp:FormView ID="FVKYCDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="lid"
                        Width="100%" DataSourceID="SqlDataKYCDetail">
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Company Name
                                    </td>
                                    <td>
                                        <asp:Label ID="txtCompanyName" runat="server" Text='<%#Eval("VendorName") %>'></asp:Label>
                                    </td>
                                    <td>Type
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Vendor"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Legal Name
                                    </td>
                                    <td>
                                        <asp:Label ID="txtLegalName" runat="server" Text='<%#Eval("LegalName") %>'></asp:Label>
                                    </td>
                                    <td>Trade Name
                                    </td>
                                    <td>
                                        <asp:Label ID="txtTradeName" runat="server" Text='<%#Eval("TradeName") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Division
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("Division") %>'></asp:Label>
                                    </td>
                                    <td>Mobile No
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOfficeTelephone" runat="server" Text='<%#Eval("OfficeTelephone") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Contact Person
                                    </td>
                                    <td>
                                        <asp:Label ID="lblContactPerson" runat="server" Text='<%#Eval("ContactPerson") %>'></asp:Label>
                                    </td>
                                    <td>
                                        Contact No
                                    </td>
                                    <td>
                                        <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("ContactNo") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>EMail
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>' ></asp:Label>
                                    </td>
                                    <td>Credit Days
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCrDays" runat="server" Text='<%#Eval("CreditDays") %>'></asp:Label>
                                    </td>
                                </tr>
                                    <tr>
                                    <td>GSTN
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGSTN" runat="server" Text='<%#Eval("GSTN") %>' ></asp:Label>
                                    </td>
                                    <td>Pan NO
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPanno" runat="server" Text='<%#Eval("PanNo") %>'></asp:Label>
                                    </td>
                                </tr>
                                </tr>
                                    <tr>
                                    <td>HOD
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHod" runat="server" Text='<%#Eval("HODName") %>' ></asp:Label>
                                    </td>
                                    <td>Kam Name
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("KamName") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Service Catagory</td>
                                    <td>
                                        <asp:Label ID="lblCatagory" runat="server" Text='<%#Eval("VendorType") %>'></asp:Label>
                                    </td>
                                    <td>Address
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Country
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country") %>'></asp:Label>
                                    </td>
                                    <td>State
                                    </td>
                                    <td>
                                        <asp:Label ID="lblState" runat="server" Text='<%#Eval("State") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                     <td>City  
                                     </td>
                                     <td>
                                         <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>'></asp:Label>
                                     </td>
                                     <td>Pincode                                 
                                     </td>
                                     <td>
                                         <asp:Label ID="lblPincode" runat="server" Text='<%#Eval("Pincode") %>'></asp:Label>
                                     </td>
                                 </tr>
                                        <tr>
                                            <td>Account No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblaccount" runat="server" Text='<%#Eval("AccountNo") %>'></asp:Label>
                                            </td>
                                            <td>Bank Name
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBankname" runat="server" Text='<%#Eval("BankName") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>IFSC Code
                                            </td>
                                            <td>
                                                <asp:Label ID="lbliIFSCcode" runat="server" Text='<%#Eval("IFSCCode") %>'></asp:Label>
                                            </td>
                                            <td>MICR Code                                 
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMICRcode" runat="server" Text='<%#Eval("MICRCode") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td>Account Type
                                        </td>
                                        <td>
                                            <asp:Label ID="lblaccounttype" runat="server" Text='<%#Eval("AccountType") %>'></asp:Label>
                                        </td>  
                                             <td> GST RegType
                                            </td>
                                          <td>
                                            <asp:Label ID="lblGsttype" runat="server" Text='<%#Eval("GSTRegTypeName") %>'></asp:Label>
                                        </td>
                                    </tr>
                                       

                                <%--                                <tr>
                                    <td>GST Reg. Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGSTRegType" runat="server" class="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlGSTRegType_SelectedIndexChanged">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Registered" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Un-Registered" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Foreign" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="SEZ" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPan" runat="server" Text="Pan No" Visible="false"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RFVPan" runat="server" InitialValue="" ControlToValidate="txtPan" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please Enter Pan NO" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revPAN" runat="server" ControlToValidate="txtPan" ErrorMessage="Invalid PAN Number."
                                            ValidationExpression="^[A-Z]{5}[0-9]{4}[A-Z]{1}$" ValidationGroup="Required" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPan" runat="server" Width="250" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- <td>
                                                Location
                                                <asp:RequiredFieldValidator ID="RFVLocation" runat="server" InitialValue="" ControlToValidate="txtLocation" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Please Enter Location" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                    <asp:TextBox ID="txtLocation" runat="server" MaxLength="100"></asp:TextBox>
                                            </td>
                                    <td>
                                        <asp:Label ID="lblGstn" runat="server" Text="GST No:" Visible="false"></asp:Label>
                                        <asp:RegularExpressionValidator ID="rfvgst" runat="server" ControlToValidate="txtGSTN" ErrorMessage="Invalid GSTNO Number."
                                            ValidationExpression="\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}" ValidationGroup="Required" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGSTN" runat="server" Width="250" Visible="false"></asp:TextBox>
                                    </td>
                                    <td>HOD
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddHOD" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>KAM
         
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKAM" runat="server" Width="250"></asp:TextBox>
                                    </td>
                                    <td>CCM
       
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCCM" runat="server" Width="250"></asp:TextBox>
                                    </td>
                                </tr>--%>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                </fieldset>
                <fieldset>
                    <legend>Upload Document</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Document Type
                                <asp:DropDownList ID="ddDocument" runat="server" CssClass="DropDownBox">
                                </asp:DropDownList></td>
                            <td>
                                <asp:FileUpload ID="fuDocument" runat="server" /> &nbsp;&nbsp;
                                <asp:Button ID="btnUpload" runat="server"
                                  OnClick="btnUpload_Click"  Text="Upload Document"  />
                            </td>
                            </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Download Documents</legend>
                    <asp:GridView ID="GridViewDoc" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false"
                        CssClass="table" Width="100%" DataSourceID="DataSourceDocDownload" OnRowCommand="GridViewDoc_RowCommand">
                        <%-- --%>
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                            <asp:BoundField DataField="dtDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%# Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                <fieldset>
                    <legend>Vendor History</legend>
                    <asp:GridView ID="gvApprovalHistory" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false" PagerStyle-CssClass="pgr"
                        CssClass="table" Width="100%" DataSourceID="DataSourceApprovalHistory" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--          <asp:BoundField DataField="LId" HeaderText="VendorId" />--%>
                            <asp:BoundField DataField="StatusName" HeaderText="StatusName" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                            <asp:BoundField DataField="UserName" HeaderText="Created  By" />
                            <asp:BoundField DataField="dtDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:SqlDataSource ID="SqlDataKYCDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="VN_GetVendorDetails" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:SessionParameter Name="VendorKYCID" SessionField="ViewvendorKYCID" />
    </SelectParameters>
</asp:SqlDataSource>

    <asp:SqlDataSource ID="DataSourceDocDownload" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="VN_GetKYCDocument" SelectCommandType="StoredProcedure"><%--VN_GetUploadDocuments--%>
        <SelectParameters>
            <asp:SessionParameter Name="VendorKYCId" SessionField="ViewvendorKYCID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DataSourceApprovalHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="VN_GetVednorHistory" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="VendorKycId" SessionField="ViewvendorKYCID" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewTransport.aspx.cs" Inherits="Transport_NewTransport" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" />
    <script type="text/javascript">

        function OnCustomerSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnCustId.ClientID%>').value = results.ClientId;
            $get('<%=txtCustomer.ClientID%>').focus();
        }
        $addHandler
            (
            $get('<%=txtCustomer.ClientID%>'), 'keyup',

            function () {
                $get('<%=hdnCustId.ClientID %>').value = '0';
            }
            );

    </script>
    <div>
        <asp:UpdateProgress ID="updProgress1" AssociatedUpdatePanelID="updTransport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div>
        <div align="center" class="m clear">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hdnMode" runat="server" Value="0" />
                 <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
        </div>
        <fieldset>
            <legend>Transport Detail</legend>
            <asp:UpdatePanel ID="updTransport" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
                <ContentTemplate>
                    <div>
                        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="validateTransport" CssClass="errorMsg" EnableViewState="false" />
                    </div>
                    <div class="m clear">
                        <asp:Button ID="btnSave" Text="Save" ValidationGroup="validateTransport" runat="server" OnClick="btnSave_Click" TabIndex="12" />
                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server" CausesValidation="false" TabIndex="13" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                        <tr>
                            <td>Ref No
                            </td>
                            <td>
                                <asp:Label ID="lblRefNo" runat="server"></asp:Label>
                            </td>
                            <td>Date
                            </td>
                            <td>
                                <span><%=DateTime.Now.ToString("dd/MM/yyyy") %> </span>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer
                                <asp:RequiredFieldValidator ID="RFVCustName" runat="server" ValidationGroup="validateTransport" SetFocusOnError="true"
                                    ControlToValidate="txtCustomer" Text="*" ErrorMessage="Please Enter Customer Name" InitialValue=""> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomer" runat="server" TabIndex="1" placeholder="Search Customer" MaxLength="100" Width="60%" AutoPostBack="true" OnTextChanged="txtCustomer_TextChanged"></asp:TextBox>
                                <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                                <div id="divwidthCust">
                                </div>
                                <cc1:autocompleteextender id="CustomerExtender" runat="server" targetcontrolid="txtCustomer"
                                    completionlistelementid="divwidthCust" servicepath="../WebService/CustomerAutoComplete.asmx"
                                    servicemethod="GetCompletionList" minimumprefixlength="2" behaviorid="divwidthCust"
                                    contextkey="4317" usecontextkey="True" onclientitemselected="OnCustomerSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight" firstrowselected="true">
                                </cc1:autocompleteextender>
                            </td>
                            <td>Customer Division
                                <asp:RequiredFieldValidator ID="rfvDivision" runat="server" ValidationGroup="validateTransport" SetFocusOnError="true"
                                    ControlToValidate="ddDivision" Text="*" ErrorMessage="Please Select Division" InitialValue="0"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddDivision" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddDivision_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnDivision" runat="server" Value='<%#Eval("DivisionId") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>Customer Plant
                                <asp:RequiredFieldValidator ID="rfvPlant" runat="server" ValidationGroup="validateTransport" SetFocusOnError="true"
                                    ControlToValidate="ddPlant" Text="*" ErrorMessage="Please Select Plant" InitialValue="0"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddPlant" runat="server" TabIndex="3"></asp:DropDownList>
                                <asp:HiddenField ID="hdnPlant" runat="server" Value='<%#Eval("PlantId") %>' />
                            </td>
                            <%--<td>Job No
                                <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ValidationGroup="validateTransport" SetFocusOnError="true"
                                    ControlToValidate="txtJobNo" Text="*" ErrorMessage="Please Enter Job No" InitialValue=""> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNo" runat="server" TabIndex="4" Enabled="false" MaxLength="100" Width="38%"></asp:TextBox>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>Branch
                                        <asp:RequiredFieldValidator ID="rfvDDBranch1" runat="server" ControlToValidate="ddlBranch" InitialValue="0"
                                            SetFocusOnError="true" Text="Required" ErrorMessage="Please Select Branch." Display="Dynamic"
                                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBranch" runat="server" Width="250px" TabIndex="1" ToolTip="Branch" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>

                            </td>
                            <td>Job No
                                <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ValidationGroup="validateTransport" SetFocusOnError="true"
                                    ControlToValidate="txtJobNo" Text="*" ErrorMessage="Please Enter Job No" InitialValue=""> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNo" runat="server" TabIndex="4" Enabled="false" MaxLength="100" Width="38%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Location From
                            </td>
                            <td>
                                <asp:TextBox ID="txtFrom" runat="server" TabIndex="5" Width="60%"></asp:TextBox>
                            </td>
                            <td>Destination
                            </td>
                            <td>
                                <asp:TextBox ID="txtTo" runat="server" TabIndex="6" Width="60%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mode
                                <asp:RequiredFieldValidator ID="rfvMode" ValidationGroup="validateTransport" runat="server"
                                    Display="Dynamic" ControlToValidate="ddMode" InitialValue="0" ErrorMessage="Please Select Mode"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddMode" runat="server" TabIndex="7" AutoPostBack="true" OnSelectedIndexChanged="ddMode_SelectedIndexChanged">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>No Of Packages
                                 <asp:RegularExpressionValidator ControlToValidate="txtNoOfPkgs" ID="regExVal" runat="server" ErrorMessage="Provide no of packages more than 0."
                                     ValidationExpression="^[1-9]*" ForeColor="Red" SetFocusOnError="true" Display="Dynamic">
                                 </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoOfPkgs" runat="server" TabIndex="8" Width="20%" type="Number"></asp:TextBox>
                                <asp:CompareValidator ID="CompValPackgs" runat="server" ControlToValidate="txtNoOfPkgs"
                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid No Of Packages"
                                    Display="Dynamic" ValidationGroup="validateTransport"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Weight (Kgs)
                            </td>
                            <td>
                                <asp:TextBox ID="txtGrossWeight" runat="server" TabIndex="9" Width="20%"></asp:TextBox>
                                <asp:CompareValidator ID="ComValGrossWT" runat="server" ControlToValidate="txtGrossWeight"
                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Gross Weight"
                                    Display="Dynamic" ValidationGroup="validateTransport"></asp:CompareValidator>
                            </td>
                            <td>Delivery Type
                                <asp:RequiredFieldValidator ID="rfvDeliveryType" runat="server" ControlToValidate="ddDeliveryType" SetFocusOnError="true"
                                    Display="Dynamic" InitialValue="0" ErrorMessage="Please select delivery type" Text="*" ValidationGroup="validateTransport"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddDeliveryType" runat="server" TabIndex="10" AutoPostBack="true" OnSelectedIndexChanged="ddDeliveryType_SelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Loaded" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="DeStuff" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Break Bulk" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td>Pick Up Address
                                 <asp:RequiredFieldValidator ID="rfvPickUpAdd" runat="server" ControlToValidate="txtPickupAdd" SetFocusOnError="true"
                                     Text="*" ErrorMessage="Please Enter Pick Up Address" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPickupAdd" runat="server" TextMode="MultiLine" ToolTip="Enter PickUpAdd"
                                    TabIndex="4" PlaceHolder="PickUp Address" Width="250px" Visible="true"></asp:TextBox><%--AutoPostBack="True"--%>
                            </td>
                            <td>Drop Address
                                <asp:RequiredFieldValidator ID="rfvDropAdd" runat="server" ControlToValidate="txtDropAdd" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Drop Address" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDropAdd" runat="server" TextMode="MultiLine" ToolTip="Enter DropAdd"
                                    TabIndex="4" PlaceHolder="Drop Address" Width="250px" Visible="true"></asp:TextBox><%--AutoPostBack="True"--%>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;State
                            </td>
                            <td></td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State
                            </td>
                        </tr>
                        <tr>
                            <td>Enter PickUp Pincode
                               <asp:RequiredFieldValidator ID="rfvPickUpPin" runat="server" ControlToValidate="txtPincode1" SetFocusOnError="true"
                                   Text="*" ErrorMessage="Please Enter PickUp Pincode " ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPincode1" runat="server" Width="115px" AutoPostBack="True" EnableViewState="true" CssClass="SearchTextbox" ToolTip="Enter Pincode" placeholder="Search" TabIndex="3" OnTextChanged="txtPincode1_TextChanged"></asp:TextBox><%----%>
                                <asp:HiddenField ID="hdnPincodeId" runat="server" Value="0" />
                                &nbsp;
                                <asp:TextBox ID="txtCity1" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                                                            &nbsp;
                                <asp:TextBox ID="txtState1" runat="server" Width="115px" Enabled="false"></asp:TextBox>

                            </td>
                            <td>Enter Drop Pincode
                                <asp:RequiredFieldValidator ID="rvfDropPin" runat="server" ControlToValidate="txtPincode2" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Drop Pincode " ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPincode2" runat="server" Width="115px" AutoPostBack="True" EnableViewState="true" CssClass="SearchTextbox" ToolTip="Enter Pincode" placeholder="Search" TabIndex="3" OnTextChanged="txtPincode2_TextChanged"></asp:TextBox><%----%>
                                <asp:HiddenField ID="hdnpinid" runat="server" Value="0" />
                                &nbsp; 
                                <asp:TextBox ID="txtCity2" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                                                            &nbsp;
                                <asp:TextBox ID="txtState2" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Remarks</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" TabIndex="11" TextMode="MultiLine" Width="85%" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                KAM
                            </td>
                            <td>
                                <asp:TextBox ID="txtKAM" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td></td><td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmpty_Letter" runat="server" Text="Empty Letter" AutoPostBack="true" ViewStateMode="Enabled" Visible="false"></asp:Label><%--ValidationGroup="Required"--%> 
                                     <asp:RequiredFieldValidator ID="rfvLoadedDocuments" runat="server" ControlToValidate="loadedDocuments" SetFocusOnError="true"
                                  Text="*" ErrorMessage="Please select a  document to upload"></asp:RequiredFieldValidator><%----%>
                            </td>
                             <td>                                 
                             <div class="file-upload">                                  
                              <label for="FileUpload1" class="file-upload-label" > </label>                               
                                 <asp:FileUpload ID="loadedDocuments" runat="server"  CssClass="file-upload-input" Visible="false"  ViewStateMode="Enabled" /> <%----%>
                               <%--  <asp:Button ID="UpdBtn" Text="Upload File" runat="server" Visible="false" OnClick="UpdBtn_Click"/>--%>
                             </div>
                            </td>   
                        </tr>
                    </table>
                    <br />
                    <fieldset id="fsContainerDetails" runat="server">
                        <legend>Add Container</legend>
                        <div class="m clear">
                            <asp:Button ID="btnAddContainer" Text="Add" OnClick="btnAddContainer_Click"
                                ValidationGroup="valContainer" runat="server" TabIndex="24" />
                            <asp:Button ID="btnCancelContainer" Text="Cancel" TabIndex="25" OnClick="btnCancelContainer_Click" BackColor="#1c698a"
                                CausesValidation="false" runat="server" Visible="false" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="border: 1px solid #1c698a;">
                            <tr>
                                <td>Container No
                                    <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                        ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter Container No"
                                        Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11" TabIndex="21"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                        ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter 11 Digit Container No."
                                        Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                </td>
                                <td>Container Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddContainerType" TabIndex="22" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged">
                                        <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Container Size
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddContainerSize" runat="server" TabIndex="23">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="gvContainer" CssClass="table" runat="server" AutoGenerateColumns="false" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="PkId" HeaderText="Sl" ReadOnly="true" />
                                <asp:BoundField DataField="JobId" HeaderText="Job Id" Visible="false" ReadOnly="true" />
                                <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                <asp:BoundField DataField="ContainerSize" HeaderText="Container Size" />
                                <asp:BoundField DataField="ContainerType" HeaderText="Container Type" />
                                <asp:BoundField DataField="SealNo" HeaderText="Seal No" Visible="false" />
                                <asp:BoundField DataField="UserId" HeaderText="User Id" Visible="false" ReadOnly="true" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton Text="Delete" runat="server" OnClick="OnContainerDelete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetDeliveryDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>


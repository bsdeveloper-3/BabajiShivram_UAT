<%@ Page Title="Shippng Line Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShippingDetails.aspx.cs"
    Inherits="Master_ShippingDetails" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVpager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    
    <div align="center">
        <asp:HiddenField ID="hdnUserCountryId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnCustFilePath" runat="server" Value="" />
        <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
    </div>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear">
            </div>

            <div style="overflow: auto; width: 100%;">
                <fieldset>
                    <legend>Shipping Info</legend>
                    <asp:FormView ID="FormView1" runat="server" Width="100%"
                        DataSourceID="FormviewSqlDataSource" DataKeyNames="lid" >

                        <%-- OnItemUpdated="FormView1_ItemUpdated"   OnItemUpdated="FormView1_ItemUpdated" OnItemDeleted="FormView1_ItemDeleted"  OnItemInserted="FormView1_ItemInserted"  OnDataBound="FormView1_DataBound"--%>

                        <EditItemTemplate>
                            <div class="m clear">
                                <asp:Button ID="btnUpdateButton" runat="server" Text="Update"
                                    ValidationGroup="Required" TabIndex="12"  OnClick="btnUpdateButton_Click"/>
                                <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel" TabIndex="13" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Shipping Line Name
                                        <asp:RequiredFieldValidator ID="RFVEditName" runat="server" Text="*" Display="Dynamic"
                                            ErrorMessage="Please Enter Customer Name" ControlToValidate="txtUpdCustomerName"
                                            ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdnlid" Value='<%# Eval("lid")%>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUpdCustomerName" runat="server" Text='<%# Bind("CompName") %>'
                                            MaxLength="100" TabIndex="1" Width="80%"></asp:TextBox>
                                    </td>
                                    <td>Shipping Code
                                        <asp:RequiredFieldValidator ID="RFVCode" runat="server" ControlToValidate="txtCustCode"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Code" ValidationGroup="Required"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustCode" runat="server" Text='<%# Bind("SCode") %>' MaxLength="100"
                                            TabIndex="6" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td> Shipping Line Code
                                        <asp:RequiredFieldValidator ID="RFVShippingLineCode" runat="server" ControlToValidate="txtUpdAddress"
                                            Text="*" ErrorMessage="Please Enter 4 Char Shipping Line Code" ValidationGroup="Required"
                                            Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtShippingLineCode" runat="server" MaxLength="4" Text='<%# Bind("ShippingLineCode") %>'
                                            TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td> Address
                                        <asp:RequiredFieldValidator ID="RFV9" runat="server" ControlToValidate="txtUpdAddress"
                                            Text="*" ErrorMessage="Please Enter Corporate Address" ValidationGroup="Required"
                                            Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtUpdAddress" runat="server" MaxLength="400" Text='<%# Bind("SAddress") %>'
                                            TextMode="MultiLine" TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </EditItemTemplate>

                        <ItemTemplate>
                            <div class="m clear">
                                <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                               <%-- <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');"
                                    Text="Delete" />  --%>
                                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                            </div>
                            <table bgcolor="white" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>Shipping Line Name
                                    </td>
                                    <td>
                                        <asp:Label ID="lbcustomername" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnShippingId" runat="server" Value='<%# Bind("lId") %>' />
                                    </td>
                                    <td>System Code
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustCode" runat="server" Text='<%#Eval("SCode") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnSCode" runat="server" Value='<%# Bind("SCode") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td> Shipping Line Code
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblShippingLineCode" runat="server" Text='<%#Eval("ShippingLineCode") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td> Address
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lbladdress" runat="server" Text='<%#Eval("SAddress") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                </fieldset>
            </div>
            <div id="divDataSource">
                <asp:SqlDataSource ID="FormviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetShippingMSBylid" SelectCommandType="StoredProcedure">
                    <%-- DeleteCommand="DelCustomer"
                    DeleteCommandType="StoredProcedure" InsertCommand="insCustomerMS" InsertCommandType="StoredProcedure"
                    UpdateCommand="UpdateShipMaster" UpdateCommandType="StoredProcedure"--%>
                   <%--  UpdateCommandType="StoredProcedure" >--%>

                    <%--OnInserted="FormviewSqlDataSource_Inserted"
                                OnUpdated="FormviewSqlDataSource_Updated" OnSelected="FormviewSqlDataSource_Selected"
                                OnDeleted="FormviewSqlDataSource_Deleted">  OnUpdated="FormviewSqlDataSource_Updated" --%>
                    <SelectParameters>
                        <asp:SessionParameter Name="lId" SessionField="ShipId" />
                    </SelectParameters>

                 <%--   <DeleteParameters>
                        <asp:Parameter Name="lid" />
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </DeleteParameters>
                    <UpdateParameters>     
                        <asp:Parameter Name="lid" Type="Int32" />                  
                        <asp:Parameter Name="ContactName" Type="String" />
                        <asp:Parameter Name="Address" Type="String" />                                  
                        <asp:SessionParameter Name="lUser" SessionField="UserId" Type="Int32"/>
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />                        
                    </UpdateParameters>--%>
                      <%-- <asp:Parameter Name="MobileNo" Type="String" />
                        <asp:Parameter Name="ContactNo" Type="String" />        --%>  
                  <%--  <InsertParameters>
                        <asp:Parameter Name="CustName" Type="String" />
                        <asp:Parameter Name="CustCode" Type="String" />
                        <asp:Parameter Name="ContactPerson" Type="String" />
                        <asp:Parameter Name="Email" Type="String" />
                        <asp:Parameter Name="MobileNo" Type="String" />
                        <asp:Parameter Name="ContactNo" Type="String" />
                        <asp:Parameter Name="Address" Type="String" />
                        <asp:Parameter Name="PCDRequired" Type="Boolean" />
                        <asp:Parameter Name="ReferredBy" Type="String" />
                        <asp:Parameter Name="TransportationRequired" Type="Boolean" />
                        <asp:Parameter Name="SVBApplicable" Type="Boolean" />
                        <asp:Parameter Name="AddToUserList" Type="Boolean" />
                        <asp:Parameter Name="IECNo" Type="String" />
                        <asp:Parameter Name="IncomeTaxNo" Type="String" />
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </InsertParameters>--%>
                </asp:SqlDataSource>
            </div>

            <fieldset>
                <legend>Shipping Details</legend>

                <div align="center">
                    <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
                  </div>

                <asp:GridView ID="gvShippingDetail" runat="server" AllowPaging="true"
                    CssClass="table" PagerStyle-CssClass="pgr" Width="100%" PagerSettings-Position="TopAndBottom"
                    PageSize="20" AllowSorting="true" ShowFooter="true" AutoGenerateColumns="false"
                    DataKeyNames="lid" OnPageIndexChanging="gvShippingDetail_PageIndexChanging" OnRowEditing="gvShippingDetail_RowEditing"
                    OnRowCancelingEdit="gvShippingDetail_RowCancelingEdit"  OnRowUpdating="gvShippingDetail_RowUpdating"
                    OnRowCommand="gvShippingDetail_RowCommand" OnRowDeleting="gvShippingDetail_RowDeleting">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="lid" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtlid" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtlidfooter" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Name">
                            <ItemTemplate>
                                <asp:Label ID="lblPName" runat="server" Text='<%#Eval("ContactName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPName" runat="server" Text='<%#Eval("ContactName") %>'></asp:TextBox>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPNamefooter" runat="server" Text='<%#Eval("ContactName") %>' TabIndex="1"></asp:TextBox>

                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Id">
                            <ItemTemplate>
                                <asp:Label ID="lblEmailId" runat="server" Text='<%#Eval("EmailId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmailId" runat="server" Text='<%#Eval("EmailId") %>' Width="250px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtEmailIdfooter" runat="server" Text=" " TabIndex="1"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile No">
                            <ItemTemplate>
                                <asp:Label ID="lblMobNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMobNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtMobNofooter" runat="server" Text='<%#Eval("MobileNo") %>' TabIndex="1"></asp:TextBox>

                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact No">
                            <ItemTemplate>
                                <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("ContactNo") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContactNo" runat="server" Text='<%#Eval("ContactNo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtContactNofooter" runat="server" Text='<%#Eval("ContactNo") %>' TabIndex="1"></asp:TextBox>

                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAddress" runat="server" Text='<%#Eval("Address") %>'></asp:TextBox>
                                
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtAddressfooter" runat="server" Text='<%#Eval("Address") %>' TabIndex="1"></asp:TextBox>
                                
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" runat="server"
                                    Text="Delete" Font-Underline="true" OnClientClick="return confirm('Sure to delete?');"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


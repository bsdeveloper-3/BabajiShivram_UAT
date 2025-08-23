<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceField.aspx.cs"
    Inherits="FreightOperation_InvoiceField" Title="Freight Invoice Item" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <script type="text/javascript">
        function OnAirSacSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnAirSacId').value = results.SacId;
        }

        function OnSeaSacSelected(source, eventArgs) {
            //  alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnSeaSacId').value = results.SacId;
        }
    </script>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:HiddenField ID="hdnAirSacId" runat="server" />
                <asp:HiddenField ID="hdnSeaSacId" runat="server" />
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Invoice Field Setup</legend>
                <asp:GridView ID="gvField" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnPageIndexChanging="gvField_PageIndexChanging" DataKeyNames="lId" OnRowCommand="gvField_RowCommand"
                    OnRowUpdating="gvField_RowUpdating" OnRowDeleting="gvField_RowDeleting" OnRowEditing="gvField_RowEditing"
                    Width="100%" OnRowCancelingEdit="gvField_RowCancelingEdit" OnSorting="gvField_Sorting" OnRowDataBound="gvField_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FieldId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldId" runat="server" Text='<%#Eval("lId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFieldId" runat="server" Text='<%#Eval("lId") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFieldIdFooter" runat="server" Text='<%#Eval("lId") %>'></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field Name" SortExpression="FieldName">
                            <ItemTemplate>
                                <asp:Label ID="lblField_Name" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtField_Name" runat="server" Text='<%#Eval("FieldName") %>' MaxLength="100"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtField_Namefooter" runat="server" Text='<%#Eval("FieldName") %>'
                                    MaxLength="100"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Report Header">
                            <ItemTemplate>
                                <asp:Label ID="lblHeader_Name" runat="server" Text='<%#Eval("ReportHeader") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtHeader_Name" runat="server" Text='<%#Eval("ReportHeader") %>' MaxLength="100"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtHeader_Namefooter" runat="server" Text='<%#Eval("ReportHeader") %>'
                                    MaxLength="100"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UoM">
                            <ItemTemplate>
                                <asp:Label ID="lblUOMType" runat="server" Text='<%#Eval("UnitOfMeasurement") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddFieldUnit" runat="server" SelectedValue='<%#Eval("UoMid") %>' Width="100px">
                                    <asp:ListItem Text="per KG" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="per MT" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="per FRT" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="per TEU" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="per FEU" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="per BL" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="% Of" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="per CBM" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="per AWB" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="per Cont" Value="10"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddFieldUnitFooter" runat="server" Width="100px">
                                    <asp:ListItem Text="per KG" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="per MT" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="per FRT" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="per TEU" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="per FEU" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="per BL" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="% Of" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="per CBM" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="per AWB" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="per Cont" Value="10"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Percentage Of">
                    <ItemTemplate>
                        <asp:Label ID="lblPercentageField" runat="server" Text='<%#Eval("PercentOfFieldName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddPercentField" runat="server" DataSourceID="dataSourceInvoiceMS"
                             DataValueField="lid" DataTextField="FieldName" AppendDataBoundItems="true" Width="150px"
                             SelectedValue='<%#Eval("PercentOfFieldId") %>' TabIndex="3">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddPercentFieldFooter" runat="server" TabIndex="3" DataSourceID="dataSourceInvoiceMS"
                             DataValueField="lid" DataTextField="FieldName" AppendDataBoundItems="true" Width="150px">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </FooterTemplate>
                </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Tax ?">
                            <ItemTemplate>
                                <asp:Label ID="lblTax" runat="server" Text='<%#Eval("TaxRequired") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddTaxApplicable" runat="server" SelectedValue='<%#Eval("IsTaxable") %>' Width="80px">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="YES" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="NO" Value="False"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddTaxApplicableFooter" runat="server" Width="100px">
                                    <asp:ListItem Text="--Select--" Value="false"></asp:ListItem>
                                    <asp:ListItem Text="YES" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="NO" Value="False"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--    <asp:TemplateField HeaderText="SAC Code">
                            <ItemTemplate>
                                <asp:Label ID="lblSACCode" runat="server" Width="100px" Text='<%#Eval("SACCode") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSACCode" runat="server" Text='<%#Eval("SACCode") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSACCodeFooter" runat="server" Text='<%#Eval("SACCode") %>' MaxLength="6" Width="80px"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Tax Rate (%)">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxRate" runat="server" Width="100px" Text='<%#Eval("TaxRate") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTaxRate" runat="server" Text='<%#Eval("TaxRate") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTaxRateFooter" runat="server" Text='<%#Eval("TaxRate") %>' MaxLength="6" Width="80px"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Air SAC Code">
                            <ItemTemplate>
                                <asp:Label ID="lblAirSACCode" runat="server" Width="100px" Text='<%#Eval("AirSACCode") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAirSACCode" runat="server" Text='<%#Eval("AirSACCode") %>' Width="80px"
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divwidthCust_Air" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="CustomerExtender_Air" runat="server" TargetControlID="txtAirSACCode"
                                    CompletionListElementID="divwidthCust_Air" ServicePath="~/WebService/SACAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust_Air"
                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnAirSacSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtAirSACCodeFooter" runat="server" Text='<%#Eval("AirSACCode") %>' MaxLength="6" Width="80px"
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divwidthCust2_Air" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="CustomerExtender_Air" runat="server" TargetControlID="txtAirSACCodeFooter"
                                    CompletionListElementID="divwidthCust2_Air" ServicePath="~/WebService/SACAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust2_Air"
                                    ContextKey="5698" UseContextKey="True" OnClientItemSelected="OnAirSacSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sea SAC Code">
                            <ItemTemplate>
                                <asp:Label ID="lblSeaSACCode" runat="server" Width="100px" Text='<%#Eval("SeaSACCode") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSeaSACCode" runat="server" Text='<%#Eval("SeaSACCode") %>' Width="80px"
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divwidthCust_Sea" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="CustomerExtender_Sea" runat="server" TargetControlID="txtSeaSACCode"
                                    CompletionListElementID="divwidthCust_Sea" ServicePath="~/WebService/SACAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust_Sea"
                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnSeaSacSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSeaSACCodeFooter" runat="server" Text='<%#Eval("SeaSACCode") %>' MaxLength="6" Width="80px"
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divwidthCust2_Sea" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="CustomerExtender_Sea" runat="server" TargetControlID="txtSeaSACCodeFooter"
                                    CompletionListElementID="divwidthCust2_Sea" ServicePath="~/WebService/SACAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust2_Sea"
                                    ContextKey="5698" UseContextKey="True" OnClientItemSelected="OnSeaSacSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--    <asp:TemplateField HeaderText="GST %">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxRate" runat="server" Text='<%#Eval("TaxRate") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTaxRate" runat="server" Text='<%#Eval("TaxRate") %>' Width="60px" MaxLength="5"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTaxRateFooter" runat="server" Text='<%#Eval("TaxRate") %>' Width="60px" MaxLength="5"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <%--      <asp:TemplateField HeaderText="CGST %">
                            <ItemTemplate>
                                <asp:Label ID="lblCGST" runat="server" Text='<%#Eval("CGST") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SGST %">
                            <ItemTemplate>
                                <asp:Label ID="lblSGST" runat="server" Text='<%#Eval("SGST") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IGST %">
                            <ItemTemplate>
                                <asp:Label ID="lblIGST" runat="server" Text='<%#Eval("IGST") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRemark" runat="server" Text='<%#Eval("Remark") %>' TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtRemarkFooter" runat="server" TextMode="MultiLine" MaxLength="200" Text='<%#Eval("Remark") %>'></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                <%--&nbsp; <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" OnClientClick="return confirm('Sure to delete?');"
                                runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <%--<asp:SqlDataSource ID="dataSourceInvoiceMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBsImport %>"
                SelectCommand="FOP_GetInvoiceFieldMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


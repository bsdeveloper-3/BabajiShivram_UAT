<%@ Page Title="Report - Bill Rejection" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillRejection_Report.aspx.cs" Inherits="Reports_BillRejection_Report" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updBillrejection" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

      <asp:UpdatePanel ID="updBillrejection" runat="server" UpdateMode="Conditional" RenderMode="Inline">
      <ContentTemplate>
          <div align="center">
              <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
          </div>
          <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="width: 100%; align-content: center;">
              <tr>
                  <td>Date From
                      <cc1:calendarextender id="CalExtJobFromDate" runat="server" enabled="True" enableviewstate="False"
                          firstdayofweek="Sunday" format="dd-MMM-yyyy" popupbuttonid="imgFromDate" popupposition="BottomRight"
                          targetcontrolid="txtFromDate">
                      </cc1:calendarextender>
                  </td>
                  <td>
                      <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                      <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                          runat="server" />
                     <%-- <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date"
                          CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                      </asp:CompareValidator>--%>
                  </td>
                  <td>Date To
                      <cc1:calendarextender id="CalExtJobFromTo" runat="server" enabled="True" enableviewstate="False"
                          firstdayofweek="Sunday" format="dd-MMM-yyyy" popupbuttonid="imgToDate" popupposition="BottomRight"
                          targetcontrolid="txtToDate">
                      </cc1:calendarextender>
                  </td>
                  <td>
                      <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                      <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                          runat="server" />
                      <%--<asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date."
                          Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                      </asp:CompareValidator>--%>
                  </td>
                  <td>
                      &nbsp;&nbsp;
                      <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter" OnClick="btnAddFilter_Click" /> 
                      &nbsp;&nbsp;
                      <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click" /> 
                  </td>
              </tr>
          </table>
         
          <div class="clear"></div>
          <div class="clear"></div>

              <fieldset>
                <legend>Bill Rejection Report</legend>
                <div>
                    <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceReport"
                    ShowFooter="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                        <asp:BoundField HeaderText="Babaji Branch" DataField="Babaji Branch" />  
                        <asp:BoundField HeaderText="Customer" DataField="Customer" />
                        <asp:BoundField HeaderText="Consignee" DataField="Consignee" />
                        <asp:BoundField HeaderText="Count of Container No" DataField="Count of Container No" />
                        <asp:BoundField HeaderText="Job Creation Date" DataField="Job Creation Date" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField HeaderText="KAM" DataField="KAM" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="CFS Name" DataField="CFS Name" />
                        <asp:BoundField HeaderText="OOC From" DataField="OOC From" />
                        <asp:BoundField HeaderText="Last Dispatch Date" DataField="Last Dispatch Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Transportation By" DataField="Transportation By" />
                        <asp:BoundField HeaderText="Sum Of 20" DataField="Sum Of 20" />
                        <asp:BoundField HeaderText="Sum Of 40" DataField="Sum Of 40" />
                        <asp:BoundField HeaderText="Sum Of LCL" DataField="Sum Of LCL" />
                        <asp:BoundField HeaderText="RMS/NonRMS" DataField="RMS/NonRMS" />
                        <asp:BoundField HeaderText="Billing Advice Completed Date" DataField="Billing Advice Completed Date" DataFormatString="{0:dd/MM/yy}" />
                        <asp:BoundField HeaderText="Rejected Date" DataField="Rejected Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Rejection Completed Date" DataField="Rejection Completed Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="FollowUp Date" DataField="FollowUp Date" DataFormatString="{0:dd/MM/yy}"/>
                 </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_rptBillRejection_Report" SelectCommandType="StoredProcedure">   
                     <SelectParameters>
                         <asp:ControlParameter Name="FromDate" ControlID="txtFromDate" PropertyName="Text" Type="datetime" />
                         <asp:ControlParameter Name="ToDate" ControlID="txtToDate" PropertyName="Text" Type="datetime"/>
                     </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



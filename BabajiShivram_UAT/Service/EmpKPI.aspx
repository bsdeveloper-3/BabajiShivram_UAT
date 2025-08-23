<%@ Page Title="KPI" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EmpKPI.aspx.cs" Inherits="Service_EmpKPI" Culture="en-GB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/jquery-3.1.0.min.js" type="text/javascript"></script>
    <script src="../JS/toastr/toastr.min.js" type="text/javascript"></script>
    <link href="../JS/toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript">

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "0",
            "hideDuration": "0",
            "timeOut": "0",
            "extendedTimeOut": "0",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        toastr.info("In case of any queries/Concern you can mail to HR - Shalaka, Response will be provided via email.", "Information");
    </script>--%>
    <%--<div id="toast"></div>--%>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="clear"></div>
        <fieldset>
        <legend>Employee</legend>
        <div class="m clear">
            <asp:Button ID="btnSubmit" CssClass="btn" Text="Save" runat="server" 
                OnClick="btnSubmit_Click" ValidationGroup="Required" TabIndex="7"/>
            <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server"
                 CausesValidation="false" TabIndex="8"/>
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Name
                </td>
                <td>
                    <asp:Label ID="lblEmpName" runat="server" MaxLength="100"></asp:Label>
                </td>
                <td>
                    Emp Code
                </td>
                <td>
                    <asp:Label ID="lblEmpCode" runat="server" MaxLength="100"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    HOD
                    <asp:RequiredFieldValidator ID="RFVDept" runat="server" ControlToValidate="ddHOD"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Select Department Name"
                        InitialValue="0" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddHOD" runat="server" class="form-control">
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Chairman" Value="189"></asp:ListItem>
                        <asp:ListItem Text="Abdul Qureshi" Value="206"></asp:ListItem>
                        <asp:ListItem Text="Adiraju Murthy" Value="940"></asp:ListItem>
                        <asp:ListItem Text="Amit Thakur" Value="883"></asp:ListItem>
                        <asp:ListItem Text="Devendra Donde" Value="313"></asp:ListItem>
                        <asp:ListItem Text="Dhaval Davada" Value="3"></asp:ListItem>
                        <asp:ListItem Text="G. Badrinarayanan " Value="181"></asp:ListItem>
                        <asp:ListItem Text="Harish Kumar" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Javed Shaikh" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Mandar Patil" Value="224"></asp:ListItem>
                        <asp:ListItem Text="Manish Radhakrishnan" Value="185"></asp:ListItem>
                        <asp:ListItem Text="Manisha Waghmare" Value="68"></asp:ListItem>
                        <asp:ListItem Text="Navin Sequeira" Value="933"></asp:ListItem>
                        <asp:ListItem Text="Navin Sharma" Value="938"></asp:ListItem>
                        <asp:ListItem Text="Neeraj Mandovara" Value="926"></asp:ListItem>
                        <asp:ListItem Text="Rekha Deshpande" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Sajid Shaikh" Value="1021"></asp:ListItem>
                        <asp:ListItem Text="Siddhi Bhosale" Value="23"></asp:ListItem>
                        <asp:ListItem Text="Sunil Shekhawat" Value="931"></asp:ListItem>
                        <asp:ListItem Text="Thanigaivelu" Value="112"></asp:ListItem>
                        <asp:ListItem Text="Vishal Diwevdi" Value="106"></asp:ListItem>
                        <asp:ListItem Text="Yogesh Sawant" Value="928"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Remark
                </td>
                <td>
                   <asp:TextBox ID="txtRemarks" runat="server" MaxLength="400" TextMode="MultiLine"></asp:TextBox> 
                </td>
            </tr>
        </table>
        </fieldset>
        <div>
        <fieldset>
        <legend runat="server" id="legendLog">KPI - Particulars</legend>
        <div>
            
            <div id="tblKPI" class="x_content" runat="server">

                <div class="table-responsive">
                    <table class="table table-striped jambo_table bulk_action">
                    <thead>
                        <tr class="headings">
                            <th class="column-title" width="10%">Sl.</th>
                            <th class="column-title">Particulars -  - Key Performance Indicators( Minimum 5)</th>
                        </tr>
                    </thead>
                    <tbody>
                         
                        <tr class="odd pointer">
                        <td>
                            1
                            <asp:RequiredFieldValidator ID="RFV1" runat="server" ControlToValidate="txtKPI1"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI1"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            2
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtKPI2"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI2"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            3
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtKPI3"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI3"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            4
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtKPI4"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI4"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            5
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtKPI5"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI5"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            6
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI6"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            7
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI7"  runat="server" Width="90%" MaxLength="300"> </asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            8
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI8"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            9
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI9"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                            10
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI10"  runat="server" Width="90%" MaxLength="300"></asp:TextBox></td>
                        </tr>
                    </tbody>
                    </table>
                      
                </div>
            </div>
            <div>
            <asp:GridView ID="gvKPI" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                DataKeyNames="lId" DataSourceID="DataSourceKPI" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40"
                 OnRowEditing="gvKPI_RowEditing" OnRowUpdating="gvKPI_RowUpdating">
                <Columns>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" 
                            ToolTip="Click To Update Employee Detail"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Detail" runat="server"
                            Text="Update" ValidationGroup="Required"></asp:LinkButton>
                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Update" CausesValidation="false"
                            runat="server" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Particulars">
                        <ItemTemplate>
                            <asp:Label ID="lblParticulars" runat="server" Text='<%#Bind("KPIParticular")%>' ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEdtParticulars" runat="server" Text='<%#Bind("KPIParticular")%>' Width="90%" TextMode="MultiLine"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="HOD" DataField="HODName" ReadOnly="true" />--%>
                    <asp:BoundField HeaderText="Review By" DataField="ApprovedBY" ReadOnly="true" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="DataSourceKPI" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="KPI_GetEmpParticular" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="EMPID" SessionField="UserId" />
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </div>
            
        </fieldset>
        </div>
        
        
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>


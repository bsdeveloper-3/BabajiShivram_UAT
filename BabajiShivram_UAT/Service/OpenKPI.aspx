<%@ Page Title="KPI" Language="C#" MasterPageFile="~/CommonMaster.master" AutoEventWireup="true" 
        CodeFile="OpenKPI.aspx.cs" Inherits="Service_OpenKPI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
    function OnUserSelected(source, eventArgs) {
        var results = eval('(' + eventArgs.get_value() + ')');
        $get('<%=hdnUserId.ClientID%>').value = results.Userid;
    }

    function TextKeyPress() {
        $get('ctl00_ContentPlaceHolder1_hdnUserId').value = '0';
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
    
    <link href="../CSS/babaji-shivram.css" rel="stylesheet" type="text/css" />
    <div class="right_col" role="main" style="background-color: #feeace">
        <div class="row tile_count">
            <div class="x_panel" style="width: 80%; height: 100%; background-color: #b0e0e6">
                <div align="center">
                    <asp:Label ID="lblError" runat="server" Font-Size="Large"></asp:Label>
                </div>
                <div class="tab-content">
                    <h2 style="color: #337ab7">
                        EMPLOYEE DETAILS 
                    </h2>
                </div>
                <div class="x_panel">
                    <div class="x_content">
                        <div class="row">
                            <div class="col-md-8 col-sm-8 col-xs-8 form-group">
                                <asp:Label ID="l1" runat="server" Text="Employee Name:" Font-Bold="True"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                    ForeColor="#CC0000" ControlToValidate="txtEmpName" SetFocusOnError="True"
                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                <div ID="divwidthCust">
                                </div>
                                <AjaxToolkit:AutoCompleteExtender ID="UserExtender" runat="server" 
                                    BehaviorID="divwidthCust" CompletionListCssClass="AutoExtender" 
                                    CompletionListElementID="divwidthCust" 
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                    CompletionListItemCssClass="AutoExtenderList" ContextKey="4317" 
                                    FirstRowSelected="true" MinimumPrefixLength="2" 
                                     OnClientItemSelected="OnUserSelected" ServiceMethod="GetUserCompletionList" 
                                    ServicePath="~/WebService/UserAutoComplete.asmx" TargetControlID="txtEmpName" 
                                    UseContextKey="True">
                                </AjaxToolkit:AutoCompleteExtender>
                                <asp:TextBox ID="txtEmpName" runat="server" class="form-control" TabIndex="1" MaxLength="50"
                                    AutoPostBack="true" OnTextChanged="txtEmpName_TextChanged" onKeyPress="TextKeyPress()"  ></asp:TextBox>
                                <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 form-group">
                                <asp:Label ID="lblEmpCode" runat="server" Text="Employee Code:" Font-Bold="True"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="*"
                                    ForeColor="#CC0000" ControlToValidate="txtEmpCode" SetFocusOnError="True" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtEmpCode" runat="server" class="form-control" TabIndex="1" MaxLength="50"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8 col-sm-8 col-xs-8 form-group">
                                <asp:Label ID="Label7" runat="server" Text="Employee Email:" Font-Bold="True"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                    ForeColor="#CC0000" ControlToValidate="txtEmail" SetFocusOnError="True"
                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtEmail" runat="server" Type="Email" class="form-control" TabIndex="2" MaxLength="100"></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 form-group"  >
                                
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4 form-group">
                                <asp:Label ID="l4" runat="server" Text="HOD :" Font-Bold="True"></asp:Label>
                                <asp:RequiredFieldValidator ID="RefHOD" runat="server" ControlToValidate="ddHOD"
                                    Text="*" Display="Dynamic" ErrorMessage="*" ValidationGroup="Required" SetFocusOnError="true"
                                    ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                <asp:DropDownList ID="ddHOD" runat="server" class="form-control">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Chairman" Value="189"></asp:ListItem>
                                    <asp:ListItem Text="Abdul Qureshi" Value="206"></asp:ListItem>
                                    <asp:ListItem Text="Adiraju Murthy" Value="940"></asp:ListItem>
                                    <asp:ListItem Text="Amod Pathak" Value="663"></asp:ListItem>
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
                                    <asp:ListItem Text="Tarique Pathan" Value="932"></asp:ListItem>
                                    <asp:ListItem Text="Thanigaivelu" Value="112"></asp:ListItem>
                                    <asp:ListItem Text="Vishal Diwevdi" Value="106"></asp:ListItem>
                                    <asp:ListItem Text="Yogesh Sawant" Value="928"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 form-group">
                                <asp:Label ID="lblRemark" runat="server" Text="Remark :" Font-Bold="True"></asp:Label>
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 form-group">
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="x_panel">
                    <div class="x_title">
                        <h2 style="color: #337ab7">
                            KPI - Particulars (Minimum 5) <small></small>
                        </h2>
                        <ul class="nav navbar-right panel_toolbox">
                           <%-- <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>--%>
                            <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                                aria-expanded="false"></a></li>
                        </ul>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        
                        <div class="table-responsive">
                    <table class="table table-striped jambo_table bulk_action">
                    <thead>
                        <tr class="headings">
                            
                        <th class="column-title" width="10%">Sl.</th>
                        <th class="column-title">Particulars -  - Key Performance Indicators( Minimum 5)</th>
                        <th </th>
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
                            <asp:TextBox ID="txtKPI1"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                2
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtKPI2"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI2"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                3
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtKPI3"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI3"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                4
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtKPI4"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI4"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                5
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtKPI5"
                            Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKPI5"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> 

                        </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                6
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI6"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                7
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI7"  runat="server" Width="80%" MaxLength="300"> </asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                8
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI8"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                9
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI9"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                        <tr class="odd pointer">
                        <td>
                                10
                        </td>
                        <td>
                                <asp:TextBox ID="txtKPI10"  runat="server" Width="80%" MaxLength="300"></asp:TextBox></td>
                        <td> </td>
                        </tr>
                    </tbody>
                    </table>
                      
                </div>
                            <div class="clearfix">
                            </div>
                        
                    </div>
                </div>
                
                <div class="x_panel">
                    
                    <div class="row">
                        
                        <div class="col-md-12 col-sm-12 col-xs-12 form-group">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" 
                                class="btn btn-primary" ValidationGroup="Required" />
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


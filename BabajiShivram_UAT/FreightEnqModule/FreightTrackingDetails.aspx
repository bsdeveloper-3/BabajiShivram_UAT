<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightTrackingDetails.aspx.cs"
    Inherits="FreightEnqModule_FreightTrackingDetails" Culture="en-GB" MasterPageFile="~/FreightEnqModule/FreightEnqMaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="scrptMngr" />
    <link href="FR_assets/css/layout-datatables.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .min
        {
            top: 94% !important;
        }
        div#ctl00_ContentPlaceHolder1_ModalPopupExtender1_backgroundElement
        {
            position: absolute;
            left: 0px;
            top: 0px;
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function MinimizeMsg() {
            var button = document.getElementById('ctl00_ContentPlaceHolder1_btnMinMaxPopup');
            if (button.value.toString() == 'Minimize') {
                var popup = document.getElementById('ctl00_ContentPlaceHolder1_pnlpopup');
                popup.classList.add('min');
                button.value = 'Maximize';
                return false;
            }
            else if (button.value.toString() == 'Maximize') {
                var popup = document.getElementById('ctl00_ContentPlaceHolder1_pnlpopup');
                popup.classList.remove('min');
                button.value = 'Minimize';
                return false;
            }
        }

        function ValidateDoc() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_txtDocumentName").value == "") {
                document.getElementById("ctl00_ContentPlaceHolder1_txtDocumentName").style.backgroundColor = "#FAEEEF";
                document.getElementById("ctl00_ContentPlaceHolder1_txtDocumentName").style.borderWidth = "2px";
                document.getElementById("ctl00_ContentPlaceHolder1_txtDocumentName").style.borderColor = "rgba(255, 0, 0, 0.48)";
                _toastr("Enter Document Name..!!", "top-full-width", "error", false);
                document.getElementById("ctl00_ContentPlaceHolder1_txtDocumentName").focus();
                return false;
            }
        }

        function ResetControls() {

            var ddlStatus = document.getElementById('<%= ddlStatus.ClientID %>');
            var txtRemarks = document.getElementById('<%= txtRemarks.ClientID %>');
            var txtStatusDate = document.getElementById('<%= txtStatusDate.ClientID %>');

            ddlStatus.selectedIndex = 0;
            txtRemarks.value = '';
            //txtStatusDate.value = '';
            var date = new Date();
            txtStatusDate.value = date.toLocaleDateString('dd/MM/yyyy');
        }
    </script>
    <div id="wrapper" class="clearfix">
        <aside id="aside">
	        <nav id="sideNav"><!-- MAIN MENU -->
		        <ul class="nav nav-list">
			        <li><!-- dashboard -->
				        <a class="dashboard" href="Dashboard.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
					        <i class="main-icon fa fa-dashboard"></i> <span>Dashboard</span>
				        </a>
			        </li>	
                    <li><!-- Enquiry -->
                        <a href="FreightEnq.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
	                        <i class="main-icon fa fa-pencil-square-o"></i><span>Enquiry</span>
                        </a>
                    </li>	
                    <li class="active"><!-- Enquiry Tracking-->
                        <a href="EnquiryTracking.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
	                        <i class="main-icon fa fa-bar-chart-o"></i><span>Enquiry Tracking</span>
                        </a>
                    </li>																		
		        </ul>		
	        </nav>
	        <span id="asidebg"><!-- aside fixed background --></span>
        </aside>
        <!-- /ASIDE -->
        <!-- HEADER -->
        <header id="header">
	        <!-- Mobile Button -->
	        <button id="mobileMenuBtn"></button>

	        <!-- Logo -->
	        <span class="logo pull-left" style="margin-left:4px;">
		        <img src="FR_assets/images/Babaji_logo3.jpg" alt="admin panel" height="50px" width="225px" style="padding:3px" />
                &nbsp;&nbsp;
            </span>
            <span class="logo pull-left" style="margin-left: 60px;">
               <img src="https://img.alicdn.com/tps/TB1wy3QNXXXXXXMaXXXXXXXXXXX-347-38.png" alt="" height="50px" width="300px" style="padding:3px;">
	        </span>
            <!-- Logo -->		

	        <nav>
		       <!-- OPTIONS LIST -->
		        <ul class="nav pull-right">
			        <!-- USER OPTIONS -->
			        <li class="dropdown pull-left">
				        <a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
					        <img class="user-avatar" alt="" src="FR_assets/images/noavatar.jpg" height="34" /> 
					        <span class="user-name">
						        <span class="hidden-xs">
							       <asp:Label ID="lblUserName" runat="server"></asp:Label> <i class="fa fa-angle-down"></i>
						        </span>
					        </span>
				        </a>
				        <ul class="dropdown-menu hold-on-click">					       
					      
					        <li><a href="FRlogin.aspx"><i class="fa fa-power-off"></i> Log Out</a></li>
				        </ul>
			        </li>
			        <!-- /USER OPTIONS -->

		        </ul>
		        <!-- /OPTIONS LIST -->
	        </nav>
        </header>
        <!-- /HEADER -->
        <section id="middle">
            <!-- page title -->
            <header id="page-header">
	            <h1><strong>Enquiry Details</strong></h1>	
                <ol class="breadcrumb">
                    <li><a href="EnquiryTracking.aspx">Enquiry Tracking</a></li>
                    <li class="active">Enquiry Details</li>
                </ol>        
            </header>
            <!-- /page title -->
	        <div id="content" class="padding-20">
                <div class="row">
                    <div class="col-md-12">					
	                    <div class="panel panel-primary">
                            <div class="panel-heading panel-heading-transparent">
			                    <span class="pull-left" style="padding-right: 15px;">
                                    <a href="javascript:window.history.back();" style="color: white;">
                                        <i class="fa fa-backward" style="font-size: 17px;"></i>
                                    </a>
                                </span>
                                <strong>Enquiry Details</strong>
		                     </div>
		                     <div class="panel-body">
				                <div id="dvErrorsList" runat="server"></div>
                                <asp:HiddenField ID="hdnEnqId" runat="server"/>
                                <asp:HiddenField ID="hdnStatusId" runat="server" />
                                <asp:HiddenField ID="hdnDocDir" runat="server"/>
                                  <div class="tabs nomargin-top">

                                        <!-- tabs -->
                                        <ul class="nav nav-tabs nav-top-border">
                                            <li class="active">
                                                <a href="#tab1" data-toggle="tab">
                                                    <i class="fa fa-folder-open" style="margin-right: 6px"></i>Freight Detail
                                                </a>
                                            </li>
                                            <li>
                                                <a href="#tab2" data-toggle="tab">
                                                    <i class="fa fa-file-text" style="margin-right: 6px"></i>Document
                                                </a>
                                            </li>
                                          
                                        </ul>

                                        <!-- tabs content -->
                                        <div class="tab-content">

                                            <div id="tab1" class="tab-pane active">
                                                <div id="dvFreightErrors" runat="server"></div>
                                                <div class="row" style="height:30px"></div>
                                                
                                                <h4><span class="label label-primary">Status History</span></h4>
                                                <%-- / STATUS HISTORY OF ENQUIRY / --%>
                                                <div class="row" id="dvStatusHistory" runat="server" style="padding:15px;padding-top:2px">
                                                    <div class="form-group">
                                                        <div class="col-md-12 col-sm-12">
                                                        </div>
                                                    </div>
                                                </div> 
                                               

                                                <h4><span class="label label-primary">Freight Detail</span></h4>
                                                <%-- / ENQUIRY DETAILS / --%>
                                                <div class="row" id="EnqDetails" runat="server" style="padding:15px;padding-top:2px">
                                                    <div class="form-group">
                                                        <div class="col-md-12 col-sm-12">
                                                        </div>
                                                    </div>
                                                </div>   
                                            </div>

                                            <div id="tab2" class="tab-pane">
                                                <!-- Div content -->

                                                <div class="row" style="height:50px">
                                                </div>
                                                <div id="divMsgAll_Doc" runat="server"></div>
                                             
                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-md-8 col-sm-8">
                                                            <label>Document Name *</label>
                                                             <asp:TextBox ID="txtDocumentName" runat="server" TabIndex="1" CssClass="form-control" placeholder="Document Name"></asp:TextBox>
                                                           <%-- <asp:RequiredFieldValidator ID="rfvDocName" runat="server" SetFocusOnError="true" ControlToValidate="txtDocumentName"
                                                                Display="Dynamic" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="vgUploadDoc"></asp:RequiredFieldValidator>--%>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-md-8 col-sm-8">
                                                            <asp:FileUpload ID="fil_doc" runat="server" class="custom-file-upload" data-btn-text="Select Document" TabIndex="2" />      
                                                        </div>
                                                        <div class="col-md-4">
                                                         </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-2 col-sm-2">
                                                        <asp:Button ID="btnUploadDoc" class="btn btn-3d btn-primary" runat="server" Text="Upload Document" ValidationGroup="vgUploadDoc"
                                                            Width="150px" TabIndex="5" OnClientClick="return ValidateDoc();" OnClick="btnUploadDoc_OnClick" CausesValidation="true" />
                                                    </div>

                                                    <div class="col-md-4 col-sm-4">
                                                        <asp:Button ID="btnClearDoc" class="btn btn-3d btn-default"
                                                            runat="server" Text="Cancel" Width="100px" ValidationGroup="clear" TabIndex="6" OnClick="btnClearDoc_OnClick" CausesValidation="false" />
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="form-group">
                                                    </div>
                                                </div>

                                                <div class="row" id="dvDocList" runat="server" style="border: 1px solid #ddd;">
                                                    <div class="form-group">
                                                        <div class="col-md-12 col-sm-12" style="padding-top: 20px;">
                                                            
                                                            <h4><span class="label label-primary">Download Document</span></h4>
                                                            <div class="">
                                                                <table class="table table-bordered table-striped" id="sample_2">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>Sr.No.</th>
                                                                            <th>Document Name</th>
                                                                            <th>Uploaded By</th>
                                                                            <th>Uploaded Date</th>
                                                                            <th></th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>

                                                                        <%
                                                                            if (Convert.ToString(ViewState["FreightDocList"]) != null)
                                                                            {
                                                                                if (Convert.ToString(ViewState["FreightDocList"]) != "")
                                                                                {
                                                                                    var lstDoc = (List<AlibabaFreightTracking>)ViewState["FreightDocList"];
                                                                                    if (lstDoc.Count > 0)
                                                                                    {
                                                                                        for (int i = 0; i < lstDoc.Count; i++)
                                                                                        { 
                                                                        %>

                                                                        <tr>
                                                                            <td><%=(i+1).ToString() %></td>
                                                                            <td><%=lstDoc[i].DocName.ToString()%></td>
                                                                            <td><%=lstDoc[i].lUser.ToString()%></td>
                                                                            <td><%=Convert.ToDateTime(lstDoc[i].UploadedDate).ToString("dd/MMM/yyyy")%></td>
                                                                            <td><a target="_blank" href="../UploadFiles/<%=lstDoc[i].DocPath.ToString()%>"><%=lstDoc[i].DocPath.ToString()%></a></td>
                                                                         </tr>

                                                                        <% 
                                                                            }
                                                                                    }
                                                                                }
                                                                            }
                                                                              
                                                                        %>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>                                              
                                                <!-- Div content -->
                                            </div>

                                        </div>
                                    </div>


                                    <div class="row">
                                    </div>   

                                    <!-- Fullwidth Modal -->
                                    <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
	                                    <div class="modal-dialog modal-lg">
		                                    <div class="modal-content">

			                                    <!-- header modal -->
			                                    <div class="modal-header">
				                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
				                                    <h4 class="modal-title" id="myLargeModalLabel">Change Freight Status</h4>
			                                    </div>

			                                    <!-- body modal -->
			                                    <div class="modal-body">
                                                <div id="dvErrorStatus" runat="server"> </div>
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-md-6 col-sm-6">
                                                                <div class="fancy-form">
                                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control ddlColor" TabIndex="1" ToolTip="Current Status">
                                                                        <asp:ListItem Value="0" Selected="True">-- Select Current Status --</asp:ListItem>
                                                                        <asp:ListItem Value="3" Selected="False">Awarded</asp:ListItem>
                                                                        <asp:ListItem Value="4" Selected="False">Lost</asp:ListItem>
                                                                        <asp:ListItem Value="6" Selected="False">Budgetary</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Select Current Status </em>
                                                                    </span> 
                                                                </div>
                                                                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" SetFocusOnError="true" ControlToValidate="ddlStatus" InitialValue="0"
                                                                Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ValidationGroup="vgStatus"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-6 col-sm-6">
                                                                <div class="fancy-form">
                                                                    <asp:TextBox ID="txtStatusDate" runat="server" TabIndex="2" CssClass="form-control datepicker" 
                                                                        data-format="dd/mm/yyyy" ToolTip="Select status date."></asp:TextBox>
                                                                    <i class="fa fa-calendar"></i>
                                                                    <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                        <em>Select Status Date </em>
                                                                    </span> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-md-12 col-sm-12">
                                                                <div class="fancy-form">
                                                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" ToolTip="Status Remarks" runat="server" TabIndex="3" CssClass="form-control masked word-count"
                                                                    Rows="3" data-maxlength="200" data-info="textarea-words-info1" placeholder="Status Remarks ..."></asp:TextBox>
                                                                    <i class="fa fa-comment-o"></i>
                                                                    <span class="fancy-hint size-11 text-muted">
                                                                        <strong>Hint:</strong> 200 words allowed!
                                                                        <span class="pull-right">
                                                                            <span id="textarea-words-info1">0/200</span> Words
                                                                        </span>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator ID="rfvremarks" runat="server" SetFocusOnError="true" ControlToValidate="txtRemarks"
                                                                Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ValidationGroup="vgStatus"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
			                                    </div>

			                                    <!-- Modal Footer -->
			                                    <div class="modal-footer">
				                                  <%--  <button type="button" class="btn btn-default" data-dismiss="modal" tabindex="5">Close</button>--%>
                                                    <asp:Button ID="btnClose" class="btn btn-3d btn-default" runat="server" Text="Close" CausesValidation="false"
                                                        Width="100px" data-dismiss="modal" TabIndex="5" OnClick="btnClose_OnClick" />
				                                    <asp:Button ID="btnSaveStatus" class="btn btn-3d btn-primary" runat="server" Text="Update Status" ValidationGroup="vgStatus"
                                                        Width="150px" TabIndex="4" OnClick="btnSaveStatus_OnClick" CausesValidation="true" />
			                                    </div>

		                                    </div>
	                                    </div>
                                    </div>

                            </div>
	                    </div>
                    </div>						
                </div>
	        </div>
    </section>
    </div>
    <!-- JAVASCRIPT FILES -->
    <script type="text/javascript">
        var plugin_path = 'FR_assets/plugins/';
    </script>
    <script type="text/javascript" src="FR_assets/plugins/jquery/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="FR_assets/js/scripts.js"></script>
    <script src="FR_assets/js/app.js" type="text/javascript"></script>
    <script type="text/javascript" src="FR_assets/plugins/magnific-popup/jquery.magnific-popup.min.js"></script>
    <%--    <script type="text/javascript" src="FR_assets/plugins/bootstrap/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="FR_assets/plugins/smoothscroll.js"></script>
    <!-- PAGE LEVEL SCRIPTS -->
    <script type="text/javascript" src="FR_assets/plugins/datatables/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="FR_assets/plugins/datatables/js/dataTables.tableTools.min.js"></script>
    <script type="text/javascript" src="FR_assets/plugins/datatables/js/dataTables.colReorder.min.js"></script>
    <script type="text/javascript" src="FR_assets/plugins/datatables/js/dataTables.scroller.min.js"></script>
    <script type="text/javascript" src="FR_assets/plugins/datatables/dataTables.bootstrap.js"></script>
    <script type="text/javascript" src="FR_assets/plugins/select2/js/select2.full.min.js"></script>
    <script type="text/javascript">

        if (jQuery().dataTable) {

            function initTable2() {
                var table = jQuery('#sample_2');

                /* Table tools samples: https://www.datatables.net/release-datatables/extras/TableTools/ */

                /* Set tabletools buttons and button container */

                $.extend(true, $.fn.DataTable.TableTools.classes, {
                    "container": "btn-group tabletools-btn-group pull-right",
                    "buttons": {
                        "normal": "btn btn-sm btn-default",
                        "disabled": "btn btn-sm btn-default disabled"
                    }
                });

                var oTable = table.dataTable({
                    "order": [
                        [0, 'DESC']
                    ],
                    "lengthMenu": [
                        [5, 15, 20, -1],
                        [5, 15, 20, "All"] // change per page values here
                    ],

                    // set the initial value
                    "pageLength": 10,
                    "dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

                    "tableTools": {
                        "sSwfPath": "FR_assets/plugins/datatables/extensions/TableTools/swf/copy_csv_xls_pdf.swf",
                        "aButtons": [{
                            "sExtends": "pdf",
                            "sButtonText": "PDF"
                        }, {
                            "sExtends": "csv",
                            "sButtonText": "CSV"
                        }, {
                            "sExtends": "xls",
                            "sButtonText": "Excel"
                        }, {
                            "sExtends": "print",
                            "sButtonText": "Print",
                            "sInfo": 'Please press "CTRL+P" to print or "ESC" to quit',
                            "sMessage": "Generated by DataTables"
                        }, {
                            "sExtends": "copy",
                            "sButtonText": "Copy"
                        }]
                    }
                });

                var tableWrapper = jQuery('#sample_2_wrapper'); // datatable creates the table wrapper by adding with id {your_table_jd}_wrapper
                tableWrapper.find('.dataTables_length select').select2(); // initialize select2 dropdown
            }

            initTable2();
        }
    </script>
    <!-- MODAL POPUP EVENTS -->
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenHL"
        PopupControlID="pnlpopup" CancelControlID="btnCancelPopup" BackgroundCssClass="modalBackground"
        X="700" Y="160">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" Style="display: none;">
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        MESSAGING <span class="pull-right">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancelPopup_OnClick" Text="Close"
                                CssClass="btn-xs btn-3d btn-default" CausesValidation="false" />
                            &nbsp;
                            <asp:Button ID="btnMinMaxPopup" runat="server" Text="Minimize" OnClientClick="return MinimizeMsg();"
                                CssClass="btn-xs btn-3d btn-default" CausesValidation="false" />
                        </span>
                    </div>
                    <div id="dvMsgAll" runat="server">
                    </div>
                    <div class="panel-body" style="background-color: rgba(66, 139, 202, 0.16); padding-bottom: 1px">
                        <%--<div class="col-md-8">--%>
                        <div class="col-md-8 col-sm-8" style="padding-right: 2px; padding-left: 2px">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    RECENT CHAT HISTORY
                                </div>
                                <div id="dvRecentChats" runat="server" class="panel-body" style="background-color: White">
                                </div>
                                <div class="panel-footer">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Enter Message"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:Button ID="btnSendMsg" runat="server" Text="SEND" Height="34px" CssClass="btn btn-info"
                                                OnClick="btnSendMsg_OnClick" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4" style="padding-right: 2px; padding-left: 2px">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    USERS
                                </div>
                                <div id="dvOnlineUsers" runat="server" class="panel-body" style="background-color: White">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:HyperLink ID="HiddenHL" runat="server" />
    <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_OnClick" CausesValidation="false" />
    <!-- /MODAL POPUP EVENTS -->
</asp:Content>

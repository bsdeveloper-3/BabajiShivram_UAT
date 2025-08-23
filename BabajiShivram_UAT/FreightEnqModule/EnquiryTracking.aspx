<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EnquiryTracking.aspx.cs"
    MasterPageFile="~/FreightEnqModule/FreightEnqMaster.master" Inherits="FreightEnqModule_EnquiryTracking" %>

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
    <script type="text/javascript">
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
					        <li id="li_Inbox" runat="server"></li>					       
					        <li class="divider"></li>
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
	            <h1><strong>Enquiry Tracking</strong></h1>	          
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
                                <strong>Manage Tracking</strong>
		                    </div>
		                    <div class="panel-body">
				                <fieldset>
                                    <div id="dvError" runat="server"><!-- DANGER -->
                                    </div>		
                                    <table class="table table-striped table-bordered table-hover" id="sample_2">
                                        <thead>
                                            <tr>
                                              <%--  <th>Sr.No.</th>--%>
                                                <th>Enquiry No</th>
                                                <th>Enquiry Date</th>
                                                <th>Status</th>
                                                <th>Status Date</th>
                                                <th>Customer</th>
                                                <th>Mode</th>
                                                <th>Terms</th>
                                                <% LoginClass loggedInUser = new LoginClass();
                                                   if (loggedInUser.glRoleId.ToString() != "")
                                                   {
                                                       if (loggedInUser.glRoleId.ToString() == "9" || loggedInUser.glRoleId.ToString() == "53")
                                                       {                            
                                                %>
                                                <th>Created By</th>
                                                <%    }
                                                   }
                                                %>
                                            </tr>
                                        </thead>
                                        <tbody>

                                            <% if (Convert.ToString(ViewState["FreightTracking"]) != null)
                                               {
                                                   if (Convert.ToString(ViewState["FreightTracking"]) != "")
                                                   {
                                                       var lstFreightTracking = (List<AlibabaFreightTracking>)ViewState["FreightTracking"];
                                                       if (lstFreightTracking != null)
                                                       {
                                                           if (lstFreightTracking.Count > 0)
                                                           {
                                                               foreach (var list in lstFreightTracking)
                                                               { %>
                                            <tr>
                                                <td><a href='FreightTrackingDetails.aspx?id=<%=(Convert.ToString(list.lid)) %>'><%=(list.EnqRefNo.ToString()) %> </a></td>
                                                <td><%=(Convert.ToDateTime(list.EnqDate).ToString("dd/MMM/yyyy")) %></td>
                                                <td><%=(list.Status.ToString())%></td>
                                                <td><%=(Convert.ToDateTime(list.StatusDate).ToString("dd/MMM/yyyy")) %></td>
                                                <td><%=(list.CustomerName.ToString())%></td>
                                                <td><%=(list.Mode.ToString())%></td>
                                                <td><%=(list.Terms.ToString())%></td>
                                                <%if (loggedInUser.glRoleId.ToString() != "")
                                                  {
                                                      if (loggedInUser.glRoleId.ToString() == "9" || loggedInUser.glRoleId.ToString() == "53")
                                                      {                            
                                                %>
                                                <td><%=(list.lUser.ToString())%></td>
                                               <%     }
                                                  }
                                                %>
                                           </tr>

                                            <%                }
                                                           }
                                                       }
                                                   }
                                               } %>
                                        </tbody>
                                    </table>				          
					            </fieldset>	                            	            
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

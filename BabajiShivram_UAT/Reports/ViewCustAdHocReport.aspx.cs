using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using AjaxControlToolkit;
using System.Collections.Generic;
using ClosedXML.Excel;

public partial class Reports_ViewCustAdHocReport : System.Web.UI.Page
{

    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    List<string> FromDatevalue = new List<string>();
    List<string> ToDatevalue = new List<string>();
    string[] Fname, fvalue;
    string FilterOtherField, FilterDate, Filter1, FilterTextField;
    DateTime FromDate, ToDate;
    string[] FDate, TDate, Date;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnShowReport);
        ScriptManager1.RegisterPostBackControl(gvViewReport);

        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            //lblTitle.Text = "View Report";
            lblTitle.Text = "Report Name : " + Session["ReportName"] + "";

            if (gvViewReport.Rows.Count == 0)
            {

            }
        }
    }

    private void GenerateExcelMultiTab()
    {
        lblError.Text = ""; string ReportName = "DSR";

        if (Session["ReportName"] != null)
            ReportName = Session["ReportName"].ToString();

        string strFileName = ReportName + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        string strDeliveryStatus = ddShipmentType.SelectedValue;

        int ReportId = Convert.ToInt32(Session["ReportId"]);
        int FinYear = Convert.ToInt32(Session["FinYearId"]);

        DateTime DateFrom = Commonfunctions.CDateTime(txtFromDate.Text.Trim());
        DateTime DateTo = Commonfunctions.CDateTime(txtToDate.Text.Trim());

        string strReportFilter = GenerateFilter(ReportId);

        DataView dvReport = new DataView();

        DataView dvReportAll = new DataView();

        if (strDeliveryStatus == "")
        {
            // Get Shipment Cleared Result

            dvReport = DBOperations.GetCustAdhocReport(ReportId, DateFrom, DateTo, strDeliveryStatus, strReportFilter, FinYear, LoggedInUser.glCustUserId);
            
            dvReport.Table.TableName = "Cleared";

            DataTable dtViewCloned = dvReport.ToTable();
            dtViewCloned.TableName = "Un-Cleared";

            dvReportAll = dtViewCloned.DefaultView;

           // dvReport.RowFilter = "ClearedStatus =1";

            //dvReportAll.RowFilter = "ClearedStatus =0";

        

            if (dvReport.Count > 0 || dvReportAll.Count > 0)
            {
                // Update Last Report Generated User Name
                int result = DBOperations.updAdhocReportLastGeneratedBy(ReportId, LoggedInUser.glCustUserId);

                ExportFunctionMultiTab("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dvReport, dvReportAll);
            }
            else
            {
                lblError.Text = "Report Data Not Found!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            // Get Result For Delivery Dropdown Selection

            //dvReport = DBOperations.GetAdhocReport(ReportId, DateFrom, DateTo, strDeliveryStatus, strReportFilter, FinYear, LoggedInUser.glCustUserId);
            dvReport = DBOperations.GetAdhocReportCust(ReportId, DateFrom, DateTo, strDeliveryStatus, strReportFilter, FinYear, LoggedInUser.glCustUserId);

            if (strDeliveryStatus == "1")
                dvReport.Table.TableName = "Cleared";
            else
                dvReport.Table.TableName = "Un-Cleared";

            if (dvReport.Count > 0)
            {
                // Update Last Report Generated User Name
                int result = DBOperations.updAdhocReportLastGeneratedBy(ReportId, LoggedInUser.glUserId);

                //ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dvReport, dvReportAll);
                ExportFunctionMultiTab("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dvReport, dvReportAll);

            }
            else
            {
                lblError.Text = "Report Data Not Found!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        GenerateExcelMultiTab();
    }
    protected void btnShowReport_Old_Click(object sender, EventArgs e)
    {
        //lblError.Text = "";
        //string ReportName = Session["ReportName"].ToString();
        //string strFileName = ReportName + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        //string strDeliveryStatus = ddShipmentType.SelectedValue;
        //int ReportId = Convert.ToInt32(Session["ReportId"]);
        //int FinYear = Convert.ToInt32(Session["FinYearId"]);

        //DateTime DateFrom = Commonfunctions.CDateTime(txtFromDate.Text.Trim());
        //DateTime DateTo = Commonfunctions.CDateTime(txtToDate.Text.Trim());

        //string strReportFilter = GenerateFilter(ReportId);

        //DataView dvReport =  DBOperations.GetCustAdhocReport(ReportId, DateFrom, DateTo, strDeliveryStatus, strReportFilter, FinYear, LoggedInUser.glCustUserId);

        //if (dvReport.Count > 0)
        //{
        //    // Update Last Report Generated User Name
        //    int result = DBOperations.updAdhocReportLastGeneratedBy(ReportId, LoggedInUser.glCustUserId);

        //    ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dvReport);
        //}
        //else
        //{
        //    lblError.Text = "Report Data Not Found!";
        //    lblError.CssClass = "errorMsg";
        //}
    }

    private void ExportFunction(string header, string contentType, DataView dvReport)
    {
        lblError.Text = "";
        string Report = Session["ReportName"].ToString();
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvViewReport.Caption = Report + " On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");


        if (chkMerge.Checked)
        {
            int columnIndex = dvReport.Table.Columns.IndexOf("BS Job No");

            if (columnIndex >= 0)
            {
                // Clone Dataview for Column data type change after filling up Date
                DataTable dtCloned = dvReport.Table.Clone();

                // Change each column data type to String To Allow Concatenation
                foreach (DataColumn dtCol in dtCloned.Columns)
                {
                    dtCol.DataType = typeof(System.String);
                }

                // Import modified column rows from Data View to cloned data table
                foreach (DataRow row in dvReport.Table.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                // Compare function for Single Row View Against Job Ref No

                gvViewReport.DataSource = Compare(dtCloned);

            }//END_IF
            else
            {
                // Bind with Multiple Row View bcoz "BS Job No" not found
                gvViewReport.DataSource = dvReport;
            }
        }
        else
        {
            // For Multiple Row View Against Job Ref No
            gvViewReport.DataSource = dvReport;
        }

        gvViewReport.DataBind();


        gvViewReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();

    }

    private string GenerateFilter(int ReportId)
    {
        string strFilter = "";

        Dictionary<int, SqlParameter> AttributeValues = new Dictionary<int, SqlParameter>();

        SqlDataReader drCondition = DBOperations.GetReportConditionFields(ReportId);

        if (drCondition.HasRows)
        {
            while (drCondition.Read())
            {
                int FieledId = Convert.ToInt32(drCondition["FieldId"]);
                FieldDataType FieldType = (FieldDataType)drCondition["FieldDataType"];
                AttributeValues[FieledId] = GetValueFromCustomField(FieledId, FieldType);
            }
        }

        drCondition.Close();
        drCondition.Dispose();

        List<string> listName = new List<string>();
        List<string> listvalue = new List<string>();
        List<string> DateField = new List<string>();

        foreach (int fieldid in AttributeValues.Keys)
        {

            if (AttributeValues[fieldid].Value.ToString().Trim() != "")
            {
                string aaa = fieldid.ToString();
                string aaavalue = AttributeValues[fieldid].Value.ToString();

                DataSet dsFieldName = DBOperations.GetReportFieldNameById(fieldid);
                string FieldName = dsFieldName.Tables[0].Rows[0]["Fieldname"].ToString();
                int Fieldtype = Convert.ToInt32(dsFieldName.Tables[0].Rows[0]["FieldDataType"].ToString());

                if (Fieldtype == (Int32)FieldDataType.Date)
                {
                    DateField.Add(FieldName);
                }
                else
                {
                    listName.Add(FieldName);
                }

                if (aaavalue != ToDate.ToShortDateString())
                {
                    listvalue.Add(aaavalue);
                }
                else
                {
                    ToDatevalue.Add(aaavalue);
                }

                if (Fieldtype == (Int32)FieldDataType.Date)
                {
                    Date = DateField.ToArray();
                    FDate = FromDatevalue.ToArray();
                    TDate = ToDatevalue.ToArray();
                }
                else
                {
                    Fname = listName.ToArray();
                    fvalue = listvalue.ToArray();
                }

            }

        }// END_FOREACH

        if (FromDate != default(DateTime) && ToDate != default(DateTime) && fvalue != null)
        {
            #region Date filter
            for (int i = 0; i < FDate.Length; i++)
            {
                if (FDate.Length == 1)
                {
                    Filter1 = "CONVERT(varchar,CONVERT(datetime,[" + Date[i] + "],  103),112) Between " +
                             "CONVERT(varchar,CONVERT(datetime,'" + FDate[i] + "',  103),112) AND " +
                             "CONVERT(varchar,CONVERT(datetime,'" + TDate[i] + "',  103),112)";
                    // Filter1 = "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "'";
                }
                else
                {
                    Filter1 += "CONVERT(varchar,CONVERT(datetime,[" + Date[i] + "],  103),112) Between " +
                        "CONVERT(varchar,CONVERT(datetime,'" + FDate[i] + "',  103),112) AND " +
                        "CONVERT(varchar,CONVERT(datetime,'" + TDate[i] + "',  103),112) AND ";

                    //Filter1 += "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "' AND ";
                }

            }

            if (FDate.Length == 1)
            {
                FilterDate = Filter1;
            }
            else
            {
                FilterDate = Filter1.Substring(0, Filter1.LastIndexOf("AND"));
            }
            #endregion

            #region Other Field
            for (int i = 0; i < Fname.Length; i++)
            {
                if (Fname.Length == 1)
                {

                    // FilterTextField = "[" + Fname[i] + "]='" + fvalue[i] + "'";

                    FilterTextField = "[" + Fname[i] + "] like '" + fvalue[i] + "%'";
                }
                else
                {
                    // FilterTextField += "[" + Fname[i] + "]='" + fvalue[i] + "'AND ";

                    FilterTextField += "[" + Fname[i] + "] LIKE '" + fvalue[i] + "%' AND ";
                }
            }
            if (Fname.Length == 1)
            {
                FilterOtherField = FilterTextField;
            }
            else
            {
                FilterOtherField = FilterTextField.Substring(0, FilterTextField.LastIndexOf("AND"));
            }
            #endregion

            strFilter = FilterDate + " AND " + FilterOtherField;

        }
        else if (fvalue != null && FromDate == default(DateTime) && ToDate == default(DateTime))
        {
            for (int i = 0; i < Fname.Length; i++)
            {
                if (Fname.Length == 1)
                {
                    //  Filter1 = "[" + Fname[i] + "]='" + fvalue[i] + "'";

                    Filter1 = "[" + Fname[i] + "] LIKE '" + fvalue[i] + "%'";

                }
                else
                {
                    strFilter += "[" + Fname[i] + "]='" + fvalue[i] + "'AND ";

                }
            }
            if (Fname.Length == 1)
            {
                strFilter = Filter1;
            }
            else
            {
                strFilter = strFilter.Substring(0, strFilter.LastIndexOf("AND"));
            }

        }
        else if (FromDate != default(DateTime) && ToDate != default(DateTime) && fvalue == null)
        {

            for (int i = 0; i < FDate.Length; i++)
            {
                if (FDate.Length == 1)
                {
                    Filter1 = "CONVERT(varchar,CONVERT(datetime,[" + Date[i] + "],  103),112) Between " +
                         "CONVERT(varchar,CONVERT(datetime,'" + FDate[i] + "',  103),112) AND " +
                         "CONVERT(varchar,CONVERT(datetime,'" + TDate[i] + "',  103),112)";
                    //  Filter1 = "[" + Date[i] + "] Between '" + FDate[i] + "'  AND '" + TDate[i] + "'";
                }
                else
                {
                    Filter1 += "CONVERT(varchar,CONVERT(datetime,[" + Date[i] + "],  103),112) Between " +
                    "CONVERT(varchar,CONVERT(datetime,'" + FDate[i] + "',  103),112) AND " +
                    "CONVERT(varchar,CONVERT(datetime,'" + TDate[i] + "',  103),112) AND ";

                    //  Filter1 += "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "'AND ";
                }
            }
            if (FDate.Length == 1)
            {
                strFilter = Filter1;
            }
            else
            {
                strFilter = Filter1.Substring(0, Filter1.LastIndexOf("AND"));
            }

        }

        else if (FromDate == default(DateTime) && ToDate == default(DateTime) && fvalue == null)
        {
            lblError.Visible = true;
            lblError.Text = "Please fill the Details";
            lblError.CssClass = "errorMsg";

        }

        return strFilter;
    }

    protected SqlParameter GetValueFromCustomField(int FieldId, FieldDataType FieldType)
    {

        SqlParameter userInpurParam = new SqlParameter();

        // Find Control By FieldId

        Control ctrlId = CustomUITable.FindControl(FieldId.ToString());

        switch (FieldType)
        {
            case FieldDataType.Alphanumeric:
                TextBox tbBox = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBox.Text.Trim().ToUpper();
                break;
            case FieldDataType.Numeric:
                TextBox tbBoxNum = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBoxNum.Text.Trim();
                break;
            case FieldDataType.Date:
                TextBox tbBoxDate = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.Date;
                userInpurParam.Value = tbBoxDate.Text.Trim();
                if (!string.IsNullOrEmpty(tbBoxDate.Text))
                {
                    FromDate = Convert.ToDateTime(userInpurParam.Value);
                    FromDatevalue.Add(FromDate.ToShortDateString());
                }
                else
                {
                    FromDate = default(DateTime);
                }
                //newly added for to date
                // int id = FieldId + 2;(old)
                string id = "DateTo" + FieldId;
                Control ctrlIdToDate = CustomUITable.FindControl(id.ToString());
                TextBox tbBoxDateTo = (TextBox)ctrlIdToDate;
                userInpurParam.DbType = DbType.Date;
                userInpurParam.Value = tbBoxDateTo.Text.Trim();
                if (!string.IsNullOrEmpty(tbBoxDateTo.Text))
                {
                    ToDate = Convert.ToDateTime(userInpurParam.Value);
                    ToDatevalue.Add(ToDate.ToShortDateString());
                }
                else
                {
                    ToDate = default(DateTime);
                }
                break;
            case FieldDataType.Percent:
                // use a  Text Box

                TextBox tbBoxPercnt = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBoxPercnt.Text.Trim().ToUpper();
                break;
            case FieldDataType.Currency:
                // use a  Text Box

                TextBox tbBoxCurrency = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBoxCurrency.Text.Trim().ToUpper();
                break;
            case FieldDataType.CheckBox:
                // use a  CheckBox

                CheckBox chkBox = (CheckBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                if (chkBox.Checked)
                {
                    userInpurParam.Value = "YES";
                }
                else
                {
                    userInpurParam.Value = "NO";
                }
                break;
        }

        return userInpurParam;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        int ReportId = 0;
        if (Session["ReportId"] != null)
        {
            ReportId = Convert.ToInt32(Session["ReportId"]);

            SqlDataReader drFilter = DBOperations.GetReportConditionFields(ReportId);

            if (drFilter.HasRows)
            {
                while (drFilter.Read())
                {
                    int FieldId = Convert.ToInt32(drFilter["FieldId"]);
                    string FieldName = drFilter["Fieldname"].ToString();
                    FieldDataType FieldType = (FieldDataType)drFilter["FieldDataType"];

                    AddConditionalField(FieldId, FieldType, FieldName);
                }
            }

            drFilter.Close();
            drFilter.Dispose();
        }
    }

    private void AddConditionalField(int FieldId, FieldDataType FieldType, string FieldName)
    {
        TableRow tr = new TableRow();

        tr = CreateCustomAttributeUI(FieldId, FieldType, FieldName);

        CustomUITable.Rows.Add(tr);
    }

    private TableRow CreateCustomAttributeUI(int FieldId, FieldDataType DataTypeId, string FieldName)
    {
        TableRow tr = new TableRow();
        // Field Name
        TableCell tdName = new TableCell();
        TableCell tdNameTo = new TableCell();
        TableCell tdControl = new TableCell();
        TableCell tdControlTO = new TableCell();

        tdName.Text = FieldName;

        switch (DataTypeId)
        {
            case FieldDataType.Alphanumeric:

                tr.Cells.Add(tdName);
                // use a  Text Box

                TextBox tbBox = new TextBox();
                tbBox.ID = FieldId.ToString(); //GetId(FieldId);
                tbBox.MaxLength = 200;
                tbBox.TabIndex = 5;
                tdControl.Controls.Add(tbBox);
                //add RequiredFieldValidator
                tdControl.Controls.Add(CreateRequiredFieldValidator(FieldId.ToString(), "Please Enter " + FieldName + "", ""));
                tdControl.ColumnSpan = 4;
                tr.Cells.Add(tdControl);
                break;
            case FieldDataType.Numeric:

                tr.Cells.Add(tdName);
                // user Numeric Text Box with validation
                TextBox tbBoxNum = new TextBox();
                tbBoxNum.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxNum.MaxLength = 20;
                tbBoxNum.TabIndex = 5;
                tdControl.Controls.Add(tbBoxNum);
                //add RequiredFieldValidator
                tdControl.Controls.Add(CreateRequiredFieldValidator(FieldId.ToString(), "Please Enter " + FieldName + "", ""));
                // Add a numeric CompareValidator
                tdControl.Controls.Add(CreateDataTypeCheckCompareValidator(FieldId.ToString(), ValidationDataType.Double, "Invalid numeric value."));
                tdControl.ColumnSpan = 4;
                tr.Cells.Add(tdControl);
                break;
            case FieldDataType.Date:
                tdName.Text = FieldName + " From";
                tr.Cells.Add(tdName);
                // Date Text Box with validation
                TextBox tbBoxDate = new TextBox();
                tbBoxDate.Width = Unit.Pixel(100);
                tbBoxDate.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxDate.TabIndex = 5;
                tdControl.Controls.Add(tbBoxDate);
                //Image for calender
                Image imgcal = new Image();
                imgcal.ID = "img" + FieldId.ToString();
                imgcal.ImageUrl = "~/Images/btn_calendar.gif";
                imgcal.ImageAlign = ImageAlign.Top;

                tdControl.Controls.Add(imgcal);
                // Add Ajax Date Extender
                AjaxControlToolkit.CalendarExtender calDate = new CalendarExtender();
                calDate.ID = "Cal" + FieldId.ToString();
                calDate.TargetControlID = FieldId.ToString();
                calDate.PopupButtonID = imgcal.ID;
                calDate.Format = "dd/MM/yyyy";
                //  calDate.Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                tdControl.Controls.Add(calDate);
                //Add  a RequiredFieldvalidator
                tdControl.Controls.Add(CreateRequiredFieldValidator(FieldId.ToString(), "Please Select " + FieldName + " " + "From" + "", ""));
                // Add a Date CompareValidator
                tdControl.Controls.Add(CreateDataTypeCheckCompareValidator(FieldId.ToString(), ValidationDataType.Date, "Invalid date value."));
                tr.Cells.Add(tdControl);
                //Label for Date To
                Label lblName = new Label();
                lblName.Text = FieldName + "&nbsp;" + "To";

                lblName.ID = "lblTo" + FieldId.ToString();
                tdNameTo.Controls.Add(lblName);
                tr.Cells.Add(tdNameTo);

                // TextBox for Date To
                TextBox tbBoxDateTo = new TextBox();
                tbBoxDateTo.Width = Unit.Pixel(100);
                tbBoxDateTo.ID = "DateTo" + FieldId.ToString();
                tdControlTO.Controls.Add(tbBoxDateTo);
                //Image for Calender
                Image imgTo = new Image();
                imgTo.ID = "imgTo" + FieldId.ToString();
                imgTo.ImageUrl = "~/Images/btn_calendar.gif";
                imgTo.ImageAlign = ImageAlign.Top;
                tdControlTO.Controls.Add(imgTo);
                // Add Ajax Date Extender

                AjaxControlToolkit.CalendarExtender calDateTo = new CalendarExtender();
                calDateTo.ID = "CalTo" + FieldId.ToString();
                calDateTo.TargetControlID = tbBoxDateTo.ID;
                calDateTo.PopupButtonID = imgTo.ID;
                calDateTo.Format = "dd/MM/yyyy";

                tdControlTO.Controls.Add(calDateTo);
                //Add  a RequiredFieldvalidator
                tdControlTO.Controls.Add(CreateRequiredFieldValidator(tbBoxDateTo.ID.ToString(), "Please Select " + FieldName + " " + "To" + "", ""));
                // Add a Date CompareValidator
                tdControlTO.Controls.Add(CreateDataTypeCheckCompareValidator(tbBoxDateTo.ID, ValidationDataType.Date, "Invalid date value."));
                tr.Cells.Add(tdControlTO);
                break;
            case FieldDataType.Percent:
                tr.Cells.Add(tdName);
                // use a  Text Box

                TextBox tbBoxPercnt = new TextBox();
                tbBoxPercnt.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxPercnt.MaxLength = 50;
                tbBoxPercnt.TabIndex = 5;
                tdControl.Controls.Add(tbBoxPercnt);
                //add a RequiredFieldValidator
                tdControl.Controls.Add(CreateRequiredFieldValidator(FieldId.ToString(), "Please Enter " + FieldName + "", ""));
                tdControl.ColumnSpan = 4;
                tr.Cells.Add(tdControl);
                break;
            case FieldDataType.Currency:
                tr.Cells.Add(tdName);
                // use a  Text Box

                TextBox tbBoxCurrency = new TextBox();
                tbBoxCurrency.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxCurrency.MaxLength = 50;
                tbBoxCurrency.TabIndex = 5;
                tdControl.Controls.Add(tbBoxCurrency);
                //add a RequiredFieldValidator
                tdControl.Controls.Add(CreateRequiredFieldValidator(FieldId.ToString(), "Please Enter " + FieldName + "", ""));
                tr.Cells.Add(tdControl);
                break;
            case FieldDataType.CheckBox:
                tr.Cells.Add(tdName);
                // use a  CheckBox

                CheckBox chkBox = new CheckBox();
                chkBox.Text = "Yes";
                chkBox.ID = FieldId.ToString(); //GetId(FieldId);
                chkBox.TabIndex = 5;
                tdControl.Controls.Add(chkBox);
                tdControl.ColumnSpan = 4;
                tr.Cells.Add(tdControl);
                break;
        }

        return tr;

    }

    private RequiredFieldValidator CreateRequiredFieldValidator(string DynamicFieldid, string ErrorMessage)
    {
        return CreateRequiredFieldValidator(DynamicFieldid, ErrorMessage, String.Empty);
    }

    private RequiredFieldValidator CreateRequiredFieldValidator(string DynamicFieldid, string ErrorMessage, string InitialValue)
    {
        RequiredFieldValidator rfv = new RequiredFieldValidator();
        rfv.ID = ("ReqVal_" + DynamicFieldid.ToString());
        rfv.ControlToValidate = DynamicFieldid.ToString();
        rfv.Display = ValidatorDisplay.Dynamic;
        rfv.ValidationGroup = "Required";
        rfv.SetFocusOnError = true;
        rfv.Text = "*";
        rfv.ErrorMessage = ErrorMessage;
        rfv.InitialValue = InitialValue;
        return rfv;
    }

    private CompareValidator CreateDataTypeCheckCompareValidator(string DynamicFieldid, ValidationDataType DataType, string ErrorMessage)
    {
        CompareValidator cv = new CompareValidator();
        cv.ID = ("CompVal_" + DynamicFieldid);
        cv.ControlToValidate = DynamicFieldid;
        cv.Display = ValidatorDisplay.Dynamic;
        cv.Operator = ValidationCompareOperator.DataTypeCheck;
        cv.Type = DataType;
        cv.ErrorMessage = ErrorMessage;
        cv.ValidationGroup = "Required";
        cv.SetFocusOnError = true;
        return cv;
    }

    /***********Second Option**********************/
    public static void MergeRows(GridView gridView)
    {
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                {
                    if (row.Cells[i].Text != "BS Job No")
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }
    }

    /*************** Third Option -- Working **************/

    private DataView Compare(DataTable dt)
    {
        String strAtual = String.Empty;
        DataView dv = new DataView(dt);
        foreach (DataRowView row in dv)
        {
            if (strAtual.Equals(row.Row["BS Job No"].ToString()))
            {
                foreach (DataColumn dataColumn in dv.Table.Columns)
                {
                    String columnName = (dataColumn.ColumnName);

                    // Change Data Type Of Column To String For Cell Value Concatenation
                    dataColumn.DataType = typeof(System.String);

                    if (columnName != "BS Job No")
                    {
                        string strTagNumb = row[columnName].ToString();
                        CompareDelete(strTagNumb, strAtual, dv, columnName);
                    }
                }
                row.Delete();
                continue;
            }
            if (!string.IsNullOrEmpty(row.Row["BS Job No"].ToString()))
                strAtual = row.Row["BS Job No"].ToString();
        }

        return dv;
    }

    private void CompareDelete(string strTagNumb, string strAtual, DataView dt, String columnName)
    {
        foreach (DataRowView row in dt)
        {
            if (row.Row["BS Job No"].ToString().Equals(strAtual))
            {
                if (!row.Row[columnName].ToString().Contains(strTagNumb))
                {
                    // string nl = Environment.NewLine;
                    row.Row[columnName] += string.Concat(", ", strTagNumb);
                }
                return;
            }
        }
    }


    private void ExportFunctionMultiTab(string header, string contentType, DataView dvReport, DataView dvReportAll)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            lblError.Text = "";
            string Report = Session["ReportName"].ToString();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //gvViewReport.Caption = Report + " On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

            // Clone Dataview for Column data type change after filling up Date

            DataTable dtCloned = new DataTable();
            DataTable dtClonedAll = new DataTable();

            if (dvReport.Count > 0)
            {
                dtCloned = dvReport.ToTable().Clone();
                //dtCloned.Columns.Remove("ClearedStatus");
                // Clone Dataview for Column data type change and filtered data
                // Change each column data type to String To Allow Concatenation
                foreach (DataColumn dtCol in dtCloned.Columns)
                {
                    dtCol.DataType = typeof(System.String);
                }

                // Import modified column rows from Data View to cloned data table
                foreach (DataRow row in dvReport.ToTable().Rows)
                {
                    dtCloned.ImportRow(row);
                }
            }

            if (dvReportAll.Count > 0)
            {
                dtClonedAll = dvReportAll.ToTable().Clone();
                //dtClonedAll.Columns.Remove("ClearedStatus");

                // Change each column data type to String To Allow Concatenation
                foreach (DataColumn dtColAll in dtClonedAll.Columns)
                {
                    dtColAll.DataType = typeof(System.String);
                }

                // Import modified column rows from Data View to cloned data table
                foreach (DataRow rowAll in dvReportAll.ToTable().Rows)
                {
                    dtClonedAll.ImportRow(rowAll);
                }
            }
            if (chkMerge.Checked)
            {
                int columnIndex = dvReport.Table.Columns.IndexOf("BS Job No");

                if (columnIndex >= 0)
                {
                    /****************** Cleared ************************************/

                    if (dvReport.Count > 0)
                    {
                        // Compare function for Single Row View Against Job Ref No
                        //Remove Duplicate Cleared Row and Add DataTable as Worksheet.

                        wb.Worksheets.Add(Compare(dtCloned).Table);
                    }

                    /**************** Un-Cleared ******************************************/
                    if (dvReportAll.Count > 0)
                    {
                        // Compare function for Single Row View Against Job Ref No

                        //Remove Duplicate UnCleared and Add DataTable as Worksheet.

                        wb.Worksheets.Add(Compare(dtClonedAll).Table);
                    }
                }//END_IF
                else
                {
                    lblError.Text = "System Error for Record Merge!";
                    lblError.CssClass = "errorMsg";
                    return;
                }
            }//END_IF_Merge
            else
            {
                // For Multiple Row View Against Job Ref No

                if (dtCloned.Rows.Count > 0)
                    wb.Worksheets.Add(dtCloned);

                if (dtClonedAll.Rows.Count > 0)
                    wb.Worksheets.Add(dtClonedAll);
            }

            //gvViewReport.DataBind();
            //gvViewReportAll.DataBind();

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            //gvViewReport.RenderControl(hw);
            //gvViewReportAll.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.End();
        }
    }
}
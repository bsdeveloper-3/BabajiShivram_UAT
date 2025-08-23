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
using System.Collections.Generic;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.IO;


public partial class Reports_ViewAdhocCustomerReport : System.Web.UI.Page
{
    LoginClass LoggedInCustomer = new LoginClass();
    List<Control> controls = new List<Control>();

    //Control dateTo;
    string[] Fname, fvalue;
    string FilterOtherField, FilterDate, Filter1,FilterTextField;
    DateTime FromDate, ToDate;
    string[] FDate, TDate, Date;

    List<string> FromDatevalue = new List<string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnShowReport);
        ScriptManager1.RegisterPostBackControl(gvViewReport);


        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Report Name : " + Session["ReportName"] + "";

            if (gvViewReport.Rows.Count == 0)
            {


            }
        }

    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string ReportName = Session["ReportName"].ToString();
        string strFileName = ReportName + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        int ReportId = Convert.ToInt32(Session["ReportId"]);
        int FinYear = Convert.ToInt32(Session["FinYearId"]);

        int CustomerUserId = LoggedInCustomer.glCustUserId;

        DateTime DateFrom = Convert.ToDateTime(txtFromDate.Text.Trim());
        DateTime DateTo = Convert.ToDateTime(txtToDate.Text.Trim());

        string strReportFilter = GenerateFilter(ReportId);

        DataView dvReport = DBOperations.GetAdhocCustomerReport(ReportId, DateFrom, DateTo, strReportFilter, CustomerUserId, FinYear);

        if(dvReport.Count > 0)
        {
             lblError.Visible = false;
             int result = DBOperations.updAdhocReportLastGeneratedBy(ReportId, LoggedInCustomer.glCustUserId);

             ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dvReport);
             
            if (result == 0)
            {

            }
            else if (result == 1)
            {
                lblError.Text = "Report Data Not Found!";
                lblError.CssClass = "errorMsg";
            }
        }

        else
        {
            lblError.Text = "Report Data Not Found!";
            lblError.CssClass = "errorMsg";
        }

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


            List<string> listName = new List<string>();
            List<string> listvalue = new List<string>();
            List<string> DateField = new List<string>();
            List<string> ToDatevalue = new List<string>();


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

            } // END_FOREACH
                if (FromDate != default(DateTime) && ToDate != default(DateTime) && fvalue != null)
                {
                    #region Date filter
                    for (int i = 0; i < FDate.Length; i++)
                    {
                        if (FDate.Length == 1)
                        {
                            Filter1 = "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "'";
                            
                        }
                        else
                        {
                            Filter1 += "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "' AND ";

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
                            FilterTextField = "[" + Fname[i] + "]='" + fvalue[i] + "'";
                           

                            // Filter1 = "[" + Fname[i] + "] like '%" + fvalue[i] + "%'";
                        }
                        else
                        {
                            FilterTextField += "[" + Fname[i] + "]='" + fvalue[i] + "'AND ";
                        

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
                            Filter1 = "[" + Fname[i] + "]='" + fvalue[i] + "'";
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
                            Filter1 = "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "'";
                            
                        }
                        else
                        {
                            Filter1 += "[" + Date[i] + "] Between '" + FDate[i] + "' AND '" + TDate[i] + "'AND ";
                           

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

    #region Report Field

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
                imgcal.TabIndex = 5;
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
                tbBoxDateTo.TabIndex = 5;
                tdControlTO.Controls.Add(tbBoxDateTo);
                //Image for Calender
                Image imgTo = new Image();
                imgTo.ID = "imgTo" + FieldId.ToString();
                imgTo.ImageUrl = "~/Images/btn_calendar.gif";
                imgTo.ImageAlign = ImageAlign.Top;
                imgTo.TabIndex = 5;
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
                //int id = FieldId + 2;//(old working)
                string id = "DateTo" + FieldId;
                Control ctrlIdToDate = CustomUITable.FindControl(id.ToString());
                TextBox tbBoxDateTo = (TextBox)ctrlIdToDate;
                userInpurParam.DbType = DbType.Date;
                userInpurParam.Value = tbBoxDateTo.Text.Trim();
                if (!string.IsNullOrEmpty(tbBoxDateTo.Text))
                {
                    ToDate = Convert.ToDateTime(userInpurParam.Value);


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

    #endregion


    #region ExportData
        
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType, DataView dvReport)
    {
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
        
        gvViewReport.DataSource = dvReport;
        gvViewReport.DataBind();
        gvViewReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();

    }
    #endregion


}

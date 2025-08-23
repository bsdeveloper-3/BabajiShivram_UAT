using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Reports_BillingChartOverall : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Dept KPI";

            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }
        
        string strReportMonth = txtReportDate.Text.Trim();

        CreateBillingKPI(strReportMonth);
        CreateWithRejectionKPI(strReportMonth);
        CreateClearanceChart(strReportMonth);
        
    }

    #region Chart

    private void CreateBillingKPI(string ReportMonth)
    {
        // Billing KPI - Bill Receive Date To Dispatch Date - Excluding Rejection Days

        DataSet dsBillingKPI = BillingOperation.GetBillingKPI(ReportMonth);

        //*********************Sector Chart Start *******************************

        DataTable BillingData = dsBillingKPI.Tables[0];

        if (BillingData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[BillingData.Rows.Count];
            int[] YPointMember = new int[BillingData.Rows.Count];

            for (int count = 0; count < BillingData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = BillingData.Rows[count]["RangeName"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(BillingData.Rows[count]["NoOfJobs"]);
            }

            //binding chart control  
            BillingKPIChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            BillingKPIChart.Series[0].Label = "#PERCENT{P0}";
            BillingKPIChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            //BillingKPIChart.Series[0].SmartLabelStyle = 

            //   BillingKPIChart.Series[0].LegendText    = "#VALX" + " | " + "#VALY" + " |  " + "#PERCENT{P0}";
            BillingKPIChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            BillingKPIChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            BillingKPIChart.Series[0].ChartType = SeriesChartType.Pie;
            //Enabled 3D  
            BillingKPIChart.ChartAreas["BillingKPIArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table*********************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            BillingKPIChart.Legends["BillingKPILegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Aging";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            BillingKPIChart.Legends["BillingKPILegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            BillingKPIChart.Legends["BillingKPILegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            BillingKPIChart.Legends["BillingKPILegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            BillingKPIChart.Legends["BillingKPILegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            BillingKPIChart.Legends["BillingKPILegends"].HeaderSeparatorColor = Color.FromArgb(64, 64, 64, 64);

            // Add item column separator of type line
            BillingKPIChart.Legends["BillingKPILegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            BillingKPIChart.Legends["BillingKPILegends"].ItemColumnSeparatorColor = Color.FromArgb(64, 64, 64, 64);


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/

        }
    }

    private void CreateWithRejectionKPI(string ReportMonth)
    {
        // Billing KPI - Bill Receive Date To Dispatch Date - Including Rejection Days

        DataSet dsWithRejection = BillingOperation.GetBillingKPIWithReject(ReportMonth);

        //*********************Customer Chart Start *******************************

        DataTable RejectionData = dsWithRejection.Tables[0];

        if (RejectionData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[RejectionData.Rows.Count];
            int[] YPointMember = new int[RejectionData.Rows.Count];

            for (int count = 0; count < RejectionData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = RejectionData.Rows[count]["RangeName"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(RejectionData.Rows[count]["NoOfJobs"]);
            }

            //binding chart control  
            KPIWithRejectChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            KPIWithRejectChart.Series[0].Label = "#PERCENT{P0}";
            KPIWithRejectChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            KPIWithRejectChart.Series[0].LegendText = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            KPIWithRejectChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            KPIWithRejectChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            KPIWithRejectChart.Series[0].ChartType = SeriesChartType.Pie;
            //Enabled 3D  
            KPIWithRejectChart.ChartAreas["RejectArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table******************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            KPIWithRejectChart.Legends["RejectLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Aging";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            KPIWithRejectChart.Legends["RejectLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            KPIWithRejectChart.Legends["RejectLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            KPIWithRejectChart.Legends["RejectLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            KPIWithRejectChart.Legends["RejectLegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            KPIWithRejectChart.Legends["RejectLegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            KPIWithRejectChart.Legends["RejectLegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            KPIWithRejectChart.Legends["RejectLegends"].ItemColumnSeparatorColor = Color.Black;

            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/
        }

    }

    private void CreateClearanceChart(string ReportMonth)
    {
        // Billing KPI - Clearance Date To Dispatch Date
        DataSet dsBillingClearance = BillingOperation.GetBillingKPIClearance(ReportMonth);

        //*********************Clearance Chart Start *******************************

        DataTable ClearanceData = dsBillingClearance.Tables[0];

        if (ClearanceData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[ClearanceData.Rows.Count];
            int[] YPointMember = new int[ClearanceData.Rows.Count];

            for (int count = 0; count < ClearanceData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = ClearanceData.Rows[count]["RangeName"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(ClearanceData.Rows[count]["NoOfJobs"]);
            }

            //binding chart control  
            ClearanceChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            ClearanceChart.Series[0].Label = "#PERCENT{P0}";
            ClearanceChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            ClearanceChart.Series[0].LegendText = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            ClearanceChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            ClearanceChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            ClearanceChart.Series[0].ChartType = SeriesChartType.Pie;
            //Enabled 3D  
            ClearanceChart.ChartAreas["ClearanceArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table*********************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            ClearanceChart.Legends["ClearanceLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Aging";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            ClearanceChart.Legends["ClearanceLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            ClearanceChart.Legends["ClearanceLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.BottomRight;
            ClearanceChart.Legends["ClearanceLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            ClearanceChart.Legends["ClearanceLegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            ClearanceChart.Legends["ClearanceLegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            ClearanceChart.Legends["ClearanceLegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            ClearanceChart.Legends["ClearanceLegends"].ItemColumnSeparatorColor = Color.Black;


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/
        }
    }
        
    #endregion

    #region Exoprt PDF

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        string strFileName = "PerformanceChart_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt ") + ".pdf";
        
        Document Doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        PdfWriter.GetInstance(Doc, Response.OutputStream);
        Doc.Open();

        iTextSharp.text.Font TextFontformat = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
        Paragraph ParaSpacing = new Paragraph();
        ParaSpacing.SpacingBefore = 5;

        string strHeader = DateTime.Now.ToString("dd/MM/yyyyy") + " - Performance Report For Mode ";


        Doc.Add(new Paragraph(strHeader, TextFontformat));

        Doc.Add(ParaSpacing);

        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Billing KPI Without Rejection
            BillingKPIChart.SaveImage(memoryStream, ChartImageFormat.Png);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
            img.ScalePercent(75f);
            Doc.Add(img);
        }

        using (MemoryStream memoryStream1 = new MemoryStream())
        {
            // Billing KPI With Rejection
            KPIWithRejectChart.SaveImage(memoryStream1, ChartImageFormat.Png);
            iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(memoryStream1.GetBuffer());
            img1.ScalePercent(75f);
            Doc.Add(img1);

        }

        using (MemoryStream memoryStream4 = new MemoryStream())
        {
            // Customer Billing
            //CustomerChart.SaveImage(memoryStream4, ChartImageFormat.Png);
            //iTextSharp.text.Image img4 = iTextSharp.text.Image.GetInstance(memoryStream4.GetBuffer());
            //img4.ScalePercent(75f);
            //Doc.Add(img4);
        }

        using (MemoryStream memoryStream2 = new MemoryStream())
        {
            // Branch
            ClearanceChart.SaveImage(memoryStream2, ChartImageFormat.Png);
            iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(memoryStream2.GetBuffer());
            img2.ScalePercent(75f);
            Doc.Add(img2);
        }
        
        Doc.Close();

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(Doc);
        Response.End();
    }

    #endregion
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Data;
using System.Web.UI.DataVisualization.Charting;

public partial class Reports_KPIKAM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "KAM KPI";
        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        gvKAMKPIReport.DataSource = DataSourceKAMKPI;
        gvKAMKPIReport.DataBind();

        if (gvKAMKPIReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            DataView view = (DataView)DataSourceKAMKPI.Select(DataSourceSelectArguments.Empty);

            CreateBOEChartPIE(view);

            CreateBOEChartBAR(view);
        }
    }

    private void CreateBOEChartPIE(DataView view)
    {

        DataTable KAMData = GroupByCount("Name", "JobCount", view.ToTable());

        KAMData.DefaultView.Sort = "Count desc";

        KAMData = KAMData.DefaultView.ToTable();

        /********************* ASP BOE BAR Chart Start *******************************

        cTestChart.Series["Testing"].ChartType = SeriesChartType.Bar;
        int i=0;
        foreach (DataRow dr in KAMData.Rows)
        { 
            int y = (int) dr[i];
            
            cTestChart.Series["Testing"].Points.AddXY(dr[i].ToString(),y);
        }

        /*********************BOE PIE Chart Start ***********************************/

        if (KAMData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record
            string[] XPointMember = new string[KAMData.Rows.Count];
            int[] YPointMember = new int[KAMData.Rows.Count];

            //for (int count = 0; count < KAMData.Rows.Count; count++)
            int count = 0;
            foreach (DataRow dr in KAMData.Rows)
            {
                //storing Values for X axis
                XPointMember[count] = dr["Name"].ToString();

                //storing values for Y Axis
                YPointMember[count] = Convert.ToInt32(dr["Count"]);

                count = count + 1;
            }

            //binding chart control
            KAMKPIChartPIE.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            KAMKPIChartPIE.Series[0].Label = "#PERCENT{P0}";
            KAMKPIChartPIE.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            //   SectorChart.Series[0].LegendText    = "#VALX" + " | " + "#VALY" + " |  " + "#PERCENT{P0}";
            KAMKPIChartPIE.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            KAMKPIChartPIE.Series[0].BorderWidth = 10;
            //setting Chart type   
            KAMKPIChartPIE.Series[0].ChartType = SeriesChartType.Pie;
            //Enabled 3D  
            KAMKPIChartPIE.ChartAreas["KAMKPIArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table****************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            KAMKPIChartPIE.Legends["KAMKPILegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Name";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            KAMKPIChartPIE.Legends["KAMKPILegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Importer";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            KAMKPIChartPIE.Legends["KAMKPILegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            KAMKPIChartPIE.Legends["KAMKPILegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            KAMKPIChartPIE.Legends["KAMKPILegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            KAMKPIChartPIE.Legends["KAMKPILegends"].HeaderSeparatorColor = Color.FromArgb(64, 64, 64, 64);

            // Add item column separator of type line
            KAMKPIChartPIE.Legends["KAMKPILegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            KAMKPIChartPIE.Legends["KAMKPILegends"].ItemColumnSeparatorColor = Color.FromArgb(64, 64, 64, 64);


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /***************************************************************************/

        }
    }

    private void CreateBOEChartBAR(DataView view)
    {
        DataTable KAMData = GroupByCount("Name", "Importer", view.ToTable());

        KAMData.DefaultView.Sort = "Count desc";

        KAMData = KAMData.DefaultView.ToTable();

        string[] x = new string[KAMData.Rows.Count];
        decimal[] y = new decimal[KAMData.Rows.Count];
        
        for (int i = 0; i < KAMData.Rows.Count; i++)
        {
            x[i] = KAMData.Rows[i][0].ToString();
            y[i] = Convert.ToInt32(KAMData.Rows[i][1]);
        }

        BarChart1.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y });

        BarChart1.CategoriesAxis = string.Join(",", x);

        BarChart1.ChartTitle = "KAM KPI - IMPORTER";
        
        //BarChart1.Style.Add("float", "left");

        if (x.Length > 3)
        {
            BarChart1.ChartWidth = (x.Length * 60).ToString();
        }

        ///////////////////////////////////////////////////

        DataTable BOEJobData = GroupByCount("Name", "JobCount", view.ToTable());

        BOEJobData.DefaultView.Sort = "Count desc";

        BOEJobData = BOEJobData.DefaultView.ToTable();

        string[] x1 = new string[BOEJobData.Rows.Count];
        decimal[] y1 = new decimal[BOEJobData.Rows.Count];

        for (int i = 0; i < BOEJobData.Rows.Count; i++)
        {
            x1[i] = BOEJobData.Rows[i][0].ToString();
            y1[i] = Convert.ToInt32(BOEJobData.Rows[i][1]);
        }

        BarChart2.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y1 });

        BarChart2.CategoriesAxis = string.Join(",", x1);

        BarChart2.ChartTitle = "KAM KPI - JOB";

        BarChart2.Style.Add("float", "left");

        if (x1.Length > 3)
        {
            BarChart2.ChartWidth = (x1.Length * 60).ToString();
        }
    }

    public DataTable GroupBySUM(string i_sGroupByColumn, string i_sAggregateColumn, DataTable i_dSourceTable)
    {
        DataView dv = new DataView(i_dSourceTable);

        //getting distinct values for group column
        DataTable dtGroup = dv.ToTable(true, new string[] { i_sGroupByColumn });

        //adding column for the row count
        dtGroup.Columns.Add("Count", typeof(int));

        //looping thru distinct values for the group, counting
        foreach (DataRow dr in dtGroup.Rows)
        {
            dr["Count"] = i_dSourceTable.Compute("SUM(" + i_sAggregateColumn + ")", i_sGroupByColumn + " = '" + dr[i_sGroupByColumn] + "'");
        }

        //returning grouped/counted result

        return dtGroup;
    }

    public DataTable GroupByCount(string i_sGroupByColumn, string i_sAggregateColumn, DataTable i_dSourceTable)
    {
        DataView dv = new DataView(i_dSourceTable);

        //getting distinct values for group column
        DataTable dtGroup = dv.ToTable(true, new string[] { i_sGroupByColumn });

        //adding column for the row count
        dtGroup.Columns.Add("Count", typeof(int));

        //looping thru distinct values for the group, counting
        foreach (DataRow dr in dtGroup.Rows)
        {
            dr["Count"] = i_dSourceTable.Compute("COUNT(" + i_sAggregateColumn + ")", i_sGroupByColumn + " = '" + dr[i_sGroupByColumn] + "'");
        }

        //returning grouped/counted result

        return dtGroup;
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "BOEKPIReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        gvKAMKPIReport.DataSource = DataSourceKAMKPI;
        gvKAMKPIReport.DataBind();

        if (gvKAMKPIReport.Rows.Count < 1)
        {
            // lblMessage.Text = "No Record Found!";
            // lblMessage.CssClass = "errorMsg";
        }
        else
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvKAMKPIReport.AllowPaging = false;
            gvKAMKPIReport.AllowSorting = false;

            gvKAMKPIReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}
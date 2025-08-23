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
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Reports_ChartReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle  =   (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text   =   "Performance Chart";

        int ModeId      =   Convert.ToInt16(ddMode.SelectedValue);
        int DurationId  =   Convert.ToInt16(ddDuration.SelectedValue);

        CreateSectorChart(ModeId, DurationId);
        CreateCustomerChart(ModeId, DurationId);
        CreateBranchChart(ModeId, DurationId);
        CreatePortChart(ModeId, DurationId);
        CreateJobTypeChart(ModeId, DurationId);
        CreateTEUChart(ModeId, DurationId);
    }

    #region Chart

    private void CreateSectorChart(int ModeId, int DurationId)
    {
        DropDownList ddFinYear = (DropDownList) Page.Master.FindControl("ddFinYear");
        int FinYear = Convert.ToInt16(ddFinYear.SelectedValue);

        DataSet dsSector = DBOperations.GetChartSector(LoggedInUser.glUserId,ModeId,DurationId, FinYear);

        // DataSet dsMonth = DBOperations.GetChartMonth(LoggedInUser.glUserId, FinYear);

        //*********************Sector Chart Start *******************************

        DataTable SectorData = dsSector.Tables[0];

        if (SectorData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember   =   new string[SectorData.Rows.Count];
            int[] YPointMember      =   new int[SectorData.Rows.Count];

            for (int count = 0; count < SectorData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = SectorData.Rows[count]["Sector"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(SectorData.Rows[count]["JobCount"]);
            }

            //binding chart control  
            SectorChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            SectorChart.Series[0].Label    = "#PERCENT{P0}";
            SectorChart.Series[0].ToolTip  = "#VALX" + "-" + "#VALY";
                        
            //SectorChart.Series[0].SmartLabelStyle = 

         //   SectorChart.Series[0].LegendText    = "#VALX" + " | " + "#VALY" + " |  " + "#PERCENT{P0}";
            SectorChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            SectorChart.Series[0].BorderWidth   = 10;
            //setting Chart type   
            SectorChart.Series[0].ChartType     = SeriesChartType.Pie;
            //Enabled 3D  
            SectorChart.ChartAreas["SectorArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table*********************************/

            LegendCellColumn firstColumn    =   new LegendCellColumn();
            firstColumn.ColumnType          =   LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText          =   "Color";
            firstColumn.HeaderBackColor     =   Color.WhiteSmoke;
            SectorChart.Legends["SectorLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn   =   new LegendCellColumn();
            secondColumn.ColumnType         =   LegendCellColumnType.Text;
            secondColumn.HeaderText         =   "Sector";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor    =   Color.WhiteSmoke;
            secondColumn.Alignment          =   System.Drawing.ContentAlignment.MiddleLeft;
            SectorChart.Legends["SectorLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn    =   new LegendCellColumn();
            thirdColumn.ColumnType          =   LegendCellColumnType.Text;
            thirdColumn.HeaderText          =   "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor     =   Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            SectorChart.Legends["SectorLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn   =   new LegendCellColumn();
            fourthColumn.ColumnType         =   LegendCellColumnType.Text;
            fourthColumn.HeaderText         =   "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor    =   Color.WhiteSmoke;
            fourthColumn.Alignment          =   System.Drawing.ContentAlignment.MiddleRight;
            SectorChart.Legends["SectorLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            SectorChart.Legends["SectorLegends"].HeaderSeparator        = LegendSeparatorStyle.Line;
            SectorChart.Legends["SectorLegends"].HeaderSeparatorColor   = Color.FromArgb(64, 64, 64, 64);

            // Add item column separator of type line
            SectorChart.Legends["SectorLegends"].ItemColumnSeparator    = LegendSeparatorStyle.Line;
            SectorChart.Legends["SectorLegends"].ItemColumnSeparatorColor = Color.FromArgb(64, 64, 64, 64);


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            
            /**********************************************************************/

        }
    }

    private void CreateCustomerChart(int ModeId, int DurationId)
    {
        DropDownList ddFinYear = (DropDownList)Page.Master.FindControl("ddFinYear");
        int FinYear = Convert.ToInt16(ddFinYear.SelectedValue);

        DataSet dsCustomer = DBOperations.GetChartCustomer(LoggedInUser.glUserId, ModeId, DurationId, FinYear);

        //*********************Customer Chart Start *******************************

        DataTable CustomerData = dsCustomer.Tables[0];

        if (CustomerData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember   = new string[CustomerData.Rows.Count];
            int[] YPointMember      = new int[CustomerData.Rows.Count];

            for (int count = 0; count < CustomerData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = CustomerData.Rows[count]["Customer"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(CustomerData.Rows[count]["JobCount"]);
            }

            //binding chart control  
            CustomerChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            CustomerChart.Series[0].Label   = "#PERCENT{P0}";
            CustomerChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            CustomerChart.Series[0].LegendText      = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            CustomerChart.Series[0].LegendToolTip   = "#VALY";
            
            //Setting width of line  
            CustomerChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            CustomerChart.Series[0].ChartType = SeriesChartType.Pie;
            //Enabled 3D  
            CustomerChart.ChartAreas["CustomerArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table******************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType      = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText      = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            CustomerChart.Legends["CustomerLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType     = LegendCellColumnType.Text;
            secondColumn.HeaderText     = "Customer";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment       = System.Drawing.ContentAlignment.MiddleLeft;
            CustomerChart.Legends["CustomerLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType       = LegendCellColumnType.Text;
            thirdColumn.HeaderText       = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment       = System.Drawing.ContentAlignment.MiddleRight;
            CustomerChart.Legends["CustomerLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType     = LegendCellColumnType.Text;
            fourthColumn.HeaderText     = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment       = System.Drawing.ContentAlignment.MiddleRight;
            CustomerChart.Legends["CustomerLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            CustomerChart.Legends["CustomerLegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            CustomerChart.Legends["CustomerLegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            CustomerChart.Legends["CustomerLegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            CustomerChart.Legends["CustomerLegends"].ItemColumnSeparatorColor = Color.Black;
            
            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/
        }
 
    }

    private void CreateBranchChart(int ModeId, int DurationId)
    {
        DropDownList ddFinYear = (DropDownList)Page.Master.FindControl("ddFinYear");
        int FinYear = Convert.ToInt16(ddFinYear.SelectedValue);

        DataSet dsBranch = DBOperations.GetChartBranch(LoggedInUser.glUserId, ModeId, DurationId, FinYear);

        //*********************Branch Chart Start *******************************

        DataTable BranchData = dsBranch.Tables[0];

        if (BranchData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[BranchData.Rows.Count];
            int[] YPointMember = new int[BranchData.Rows.Count];

            for (int count = 0; count < BranchData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = BranchData.Rows[count]["Branch"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(BranchData.Rows[count]["JobCount"]);
            }

            //binding chart control  
            BranchChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            BranchChart.Series[0].Label     = "#PERCENT{P0}";
            BranchChart.Series[0].ToolTip   = "#VALX" + "-" + "#VALY";

            BranchChart.Series[0].LegendText    = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            BranchChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            BranchChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            BranchChart.Series[0].ChartType = SeriesChartType.Pie;
            //Enabled 3D  
            BranchChart.ChartAreas["BranchArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table*********************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            BranchChart.Legends["BranchLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Branch";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            BranchChart.Legends["BranchLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            BranchChart.Legends["BranchLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.BottomRight;
            BranchChart.Legends["BranchLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            BranchChart.Legends["BranchLegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            BranchChart.Legends["BranchLegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            BranchChart.Legends["BranchLegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            BranchChart.Legends["BranchLegends"].ItemColumnSeparatorColor = Color.Black;


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/
        }
    }

    private void CreatePortChart(int ModeId, int DurationId)
    {
        DropDownList ddFinYear = (DropDownList)Page.Master.FindControl("ddFinYear");
        int FinYear = Convert.ToInt16(ddFinYear.SelectedValue);

        DataSet dsPort = DBOperations.GetChartPort(LoggedInUser.glUserId, ModeId, DurationId, FinYear);

        //*********************Port Chart Start ********************************************

        DataTable PortData = dsPort.Tables[0];

        if (PortData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[PortData.Rows.Count];
            int[] YPointMember = new int[PortData.Rows.Count];

            for (int count = 0; count < PortData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = PortData.Rows[count]["Port"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(PortData.Rows[count]["JobCount"]);
            }

            //binding chart control  
            PortChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            PortChart.Series[0].Label   = "#PERCENT{P0}";
            PortChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            PortChart.Series[0].LegendText      = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            PortChart.Series[0].LegendToolTip   = "#VALY";
            
            //Setting width of line  
            PortChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            PortChart.Series[0].ChartType = SeriesChartType.Pie;
            PortChart.ChartAreas["PortArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table****************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            PortChart.Legends["PortLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Port";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            PortChart.Legends["PortLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            PortChart.Legends["PortLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            PortChart.Legends["PortLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            PortChart.Legends["PortLegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            PortChart.Legends["PortLegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            PortChart.Legends["PortLegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            PortChart.Legends["PortLegends"].ItemColumnSeparatorColor = Color.Black;


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/
        }
    }

    private void CreateJobTypeChart(int ModeId, int DurationId)
    {
        DropDownList ddFinYear = (DropDownList)Page.Master.FindControl("ddFinYear");
        int FinYear = Convert.ToInt16(ddFinYear.SelectedValue);

        DataSet dsJobType = DBOperations.GetChartJobType(LoggedInUser.glUserId, ModeId, DurationId, FinYear);

        //*********************Port Chart Start *******************************

        DataTable JobTypeData = dsJobType.Tables[0];

        if (JobTypeData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobTypeData.Rows.Count];
            int[] YPointMember = new int[JobTypeData.Rows.Count];

            for (int count = 0; count < JobTypeData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = JobTypeData.Rows[count]["JobType"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(JobTypeData.Rows[count]["JobCount"]);
            }

            //binding chart control  
            JobTypeChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            JobTypeChart.Series[0].Label = "#PERCENT{P0}";
            JobTypeChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            JobTypeChart.Series[0].LegendText = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            JobTypeChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            JobTypeChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            JobTypeChart.Series[0].ChartType = SeriesChartType.Pie;
            JobTypeChart.ChartAreas["JobTypeArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table*********************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            JobTypeChart.Legends["JobTypeLegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Type";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            JobTypeChart.Legends["JobTypeLegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "Job";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            JobTypeChart.Legends["JobTypeLegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            JobTypeChart.Legends["JobTypeLegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            JobTypeChart.Legends["JobTypeLegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            JobTypeChart.Legends["JobTypeLegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            JobTypeChart.Legends["JobTypeLegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            JobTypeChart.Legends["JobTypeLegends"].ItemColumnSeparatorColor = Color.Black;


            // Set Min cell column attributes            
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;

            /**********************************************************************/
        }
    }

    private void CreateTEUChart(int ModeId, int DurationId)
    {
        DropDownList ddFinYear = (DropDownList)Page.Master.FindControl("ddFinYear");
        int FinYear = Convert.ToInt16(ddFinYear.SelectedValue);

        DataSet dsTEU = DBOperations.GetChartTEU(LoggedInUser.glUserId, ModeId, DurationId, FinYear);

        //********************* TEU Chart Start *******************************************

        DataTable TEUData = dsTEU.Tables[0];

        if (TEUData.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[TEUData.Rows.Count];
            int[] YPointMember = new int[TEUData.Rows.Count];

            for (int count = 0; count < TEUData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = TEUData.Rows[count]["Port"].ToString();

                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(TEUData.Rows[count]["TotalTEU"]);
            }

            //binding chart control  
            TEUChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            // Show Percentage
            TEUChart.Series[0].Label = "#PERCENT{P0}";
            TEUChart.Series[0].ToolTip = "#VALX" + "-" + "#VALY";

            TEUChart.Series[0].LegendText = "#VALX" + " | " + "#VALY" + " | " + "#PERCENT{P0}";
            TEUChart.Series[0].LegendToolTip = "#VALY";

            //Setting width of line  
            TEUChart.Series[0].BorderWidth = 10;
            //setting Chart type   
            TEUChart.Series[0].ChartType = SeriesChartType.Pie;
            TEUChart.ChartAreas["TEUArea"].Area3DStyle.Enable3D = true;

            /******************Format Legend Style - Table****************************/

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            TEUChart.Legends["TEULegends"].CellColumns.Add(firstColumn);

            // Add second cell column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Port";
            //secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Text = "#VALX";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            secondColumn.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            TEUChart.Legends["TEULegends"].CellColumns.Add(secondColumn);

            // Add third cell column
            LegendCellColumn thirdColumn = new LegendCellColumn();
            thirdColumn.ColumnType = LegendCellColumnType.Text;
            thirdColumn.HeaderText = "TEU";
            //secondColumn.Text = "#LEGENDTEXT";
            thirdColumn.Text = "#VALY";
            thirdColumn.HeaderBackColor = Color.WhiteSmoke;
            thirdColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            TEUChart.Legends["TEULegends"].CellColumns.Add(thirdColumn);

            // Add fourth cell column
            LegendCellColumn fourthColumn = new LegendCellColumn();
            fourthColumn.ColumnType = LegendCellColumnType.Text;
            fourthColumn.HeaderText = "%";
            //secondColumn.Text = "#LEGENDTEXT";
            fourthColumn.Text = "#PERCENT{P0}";
            fourthColumn.HeaderBackColor = Color.WhiteSmoke;
            fourthColumn.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            TEUChart.Legends["TEULegends"].CellColumns.Add(fourthColumn);

            // Add header separator of type line
            TEUChart.Legends["TEULegends"].HeaderSeparator = LegendSeparatorStyle.Line;
            TEUChart.Legends["TEULegends"].HeaderSeparatorColor = Color.Black;

            // Add item column separator of type line
            TEUChart.Legends["TEULegends"].ItemColumnSeparator = LegendSeparatorStyle.Line;
            TEUChart.Legends["TEULegends"].ItemColumnSeparatorColor = Color.Black;


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
        string strFileName = "PerformanceChart_"+DateTime.Now.ToString("dd/MM/yyyy hh:mm tt ") +".pdf";

        DropDownList ddFinYear = (DropDownList)Page.Master.FindControl("ddFinYear");
        string strFinYear = ddFinYear.SelectedItem.Text;

        Document Doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        PdfWriter.GetInstance(Doc, Response.OutputStream);
        Doc.Open();

        iTextSharp.text.Font TextFontformat = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
        Paragraph ParaSpacing       =   new Paragraph();
        ParaSpacing.SpacingBefore   =   5;

        string strHeader = DateTime.Now.ToString("dd/MM/yyyyy") + " - Performance Report For Mode " + ddMode.SelectedItem.Text
                + " And Duration " + ddDuration.SelectedItem.Text + " - " + strFinYear;

        Doc.Add(new Paragraph(strHeader, TextFontformat));

        Doc.Add(ParaSpacing);

        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Sector
            SectorChart.SaveImage(memoryStream, ChartImageFormat.Png);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
            img.ScalePercent(75f);
            Doc.Add(img);
        }

        using (MemoryStream memoryStream1 = new MemoryStream())
        {
            // Customer
            CustomerChart.SaveImage(memoryStream1, ChartImageFormat.Png);
            iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(memoryStream1.GetBuffer());
            img1.ScalePercent(75f);
            Doc.Add(img1);

        }

        using (MemoryStream memoryStream4 = new MemoryStream())
        {
            // Job Type
            JobTypeChart.SaveImage(memoryStream4, ChartImageFormat.Png);
            iTextSharp.text.Image img4 = iTextSharp.text.Image.GetInstance(memoryStream4.GetBuffer());
            img4.ScalePercent(75f);
            Doc.Add(img4);
        }

        using (MemoryStream memoryStream2 = new MemoryStream())
        {
            // Branch
            BranchChart.SaveImage(memoryStream2, ChartImageFormat.Png);
            iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(memoryStream2.GetBuffer());
            img2.ScalePercent(75f);
            Doc.Add(img2);
        }

        using (MemoryStream memoryStream3 = new MemoryStream())
        {
            // Port
            PortChart.SaveImage(memoryStream3, ChartImageFormat.Png);
            iTextSharp.text.Image img3 = iTextSharp.text.Image.GetInstance(memoryStream3.GetBuffer());
            img3.ScalePercent(75f);
            Doc.Add(img3);
        }


        using (MemoryStream memoryStream5 = new MemoryStream())
        {
            // TEU
            TEUChart.SaveImage(memoryStream5, ChartImageFormat.Png);
            iTextSharp.text.Image img5 = iTextSharp.text.Image.GetInstance(memoryStream5.GetBuffer());
            img5.ScalePercent(75f);
            Doc.Add(img5);
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
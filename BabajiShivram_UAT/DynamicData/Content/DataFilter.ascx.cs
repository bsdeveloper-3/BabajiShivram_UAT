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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using BSImport.CustomControls;

public partial class DynamicData_Content_DataFilter : System.Web.UI.UserControl
{
    private static Random _random = new Random();

    public delegate void RefreshDataGridView();

    public event RefreshDataGridView OnFilterAdded;

    public delegate void BindDataGridView();

        public event BindDataGridView OnDataBound;

        List<Filter> filters;
        public List<Filter> Filters
        {
            get { return filters; }
            set { filters = value; }
        }

        public String Info
        {
            set { lblInfo.Text = value; }
        }

        string filterSessionID;
        public string FilterSessionID
        {
            get { return filterSessionID; }
            set { filterSessionID = value; }
        }

        Panel addFilterPanel;

        DataControlFieldCollection dataColumns;

        public DataControlFieldCollection DataColumns
        {
            get { return dataColumns; }
            set { dataColumns = value; }
        }

        //public Dictionary<string,Type>

        private Object dataSource;

        public Object DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            filters = new List<Filter>();

            addFilterPanel = new Panel();

            if (!Page.IsPostBack)
            {

                //Init filters
                if (Session[FilterSessionID + "DataFilters"] != null && ((List<Filter>)Session[FilterSessionID + "DataFilters"]).Count > 0)
                {
                    filters = (List<Filter>)Session[FilterSessionID + "DataFilters"];
                    LoadFilterPanel();
                    OnDataBound();
                    btnAddNewFilter.Visible = false;
                    btnAndNewFilter.Visible = true;
                    btnOrNewFilter.Visible = true;


                }
                else
                {

                    Session[FilterSessionID + "DataFilters"] = filters;

                }

                Session[FilterSessionID + "AddFilterPanel"] = addFilterPanel;

                AddFirstFilter();
            }
            else
            {
		 //-----------start Billing---------------
                if (Session[FilterSessionID + "DataFilters"] != null && ((List<Filter>)Session[FilterSessionID + "DataFilters"]).Count > 0)
                {

                //Restore current filters;
                filters = (List<Filter>)Session[FilterSessionID + "DataFilters"];
		}

                LoadFilterPanel();

                addFilterPanel = (Panel)Session[FilterSessionID + "AddFilterPanel"];
                LoadAddFilterPanel(addFilterPanel);
            }
            
        }

        public void FilterDataSource()
        {
            string filterString = BuildFilter();
            ((SqlDataSource)DataSource).FilterExpression = filterString;
        }

        private string BuildFilter()
        {
            StringBuilder filtersString = new StringBuilder();


            foreach (Filter f in filters)
            {
                //GetFilter
                //filtersString.Append(f.ToString());
                filtersString.Append(f.GetFilter());
            }
            return filtersString.ToString();
        }
    
        public void LoadFilterPanel()
        {
            pnlNewFilter.Controls.Clear();
            for (int i = 0; i < filters.Count; i++)
            {

                //Creating The Panel
                Panel newFilter = new Panel();
                newFilter.ID = "dataFilters" + i.ToString();

                //Creating Update Button
                Button updateFilterButton = new Button();
                updateFilterButton.Text = "Update";
                updateFilterButton.ID = "btnUpdate_" + i.ToString();
                updateFilterButton.CssClass = "ilbuttons";
                updateFilterButton.Click += new EventHandler(updateFilterButton_Click);
                // Set Update Button a Default Button Of Filter Panel
                newFilter.DefaultButton = updateFilterButton.ID;

                //Creating Remove Button
                Button removeFilterButton = new Button();
                removeFilterButton.Text = "Remove";
                removeFilterButton.CssClass = "ilbuttons";
                removeFilterButton.ID = "btnRemove_" + i.ToString();
                removeFilterButton.Click += new EventHandler(removeFilterButton_Click);

                //Creating Culomn Name Drop Down List
                DropDownList ddlColumnName = new DropDownList();
                ddlColumnName.ID = "ddlColumnName_" + i.ToString();
                ddlColumnName.Items.AddRange(BuildDataColumns());
                ddlColumnName.SelectedValue = filters[i].ColumnName;
                ddlColumnName.Style.Add(HtmlTextWriterStyle.Direction, "ltr");

                //Creating Culomn Operation Drop Down List
                // Added By Amit
                ListItem lstEquals = new ListItem("Equals To", "=");
                ListItem lstLike = new ListItem("Like", "LIKE");
                ListItem lstEqualsNo = new ListItem(" Not Equals", "<>");
                ListItem lstGrater = new ListItem("Greater Than", ">");
                ListItem lstLess = new ListItem("Less than", "<");
                //

                //Creating Culomn Operation Drop Down List
                DropDownList ddlColumnOperation = new DropDownList();
                ddlColumnOperation.ID = "ddlColumnOperation_" + i.ToString();

                ddlColumnOperation.Items.Add(lstLike);
                ddlColumnOperation.Items.Add(lstEquals);
                ddlColumnOperation.Items.Add(lstEqualsNo);
                ddlColumnOperation.Items.Add(lstGrater);
                ddlColumnOperation.Items.Add(lstLess);
                /*
                ddlColumnOperation.Items.Add("=");
                ddlColumnOperation.Items.Add("LIKE");
                ddlColumnOperation.Items.Add("NOT LIKE");
                ddlColumnOperation.Items.Add("<>");
                ddlColumnOperation.Items.Add(">");
                ddlColumnOperation.Items.Add(">=");
                ddlColumnOperation.Items.Add("<");
                ddlColumnOperation.Items.Add("<=");
                */
                ddlColumnOperation.SelectedValue = filters[i].Operation;
                //ddlColumnOperation.Style.Add(HtmlTextWriterStyle.Direction, "rtl");

                //ddlColumnOperation.CssClass = "checkBoxs";

                //Create Value Textbox
                TextBox txtColumnValue = new TextBox();
                txtColumnValue.ID = "txtColumnValue_" + i.ToString();
                txtColumnValue.Text = filters[i].ColumnValue;
                txtColumnValue.TextChanged += new EventHandler(txtColumnValue_TextChanged);
                txtColumnValue.CssClass = "SearchTextbox";
                txtColumnValue.Style.Add(HtmlTextWriterStyle.Direction, "ltr");

                // Literal COntrol for Space b/w Dynamic control
                LiteralControl litSpace = new LiteralControl();
                litSpace.ID = "ltSpace" + i.ToString();
                litSpace.Text = "&nbsp;";
                //
                LiteralControl litSpace2 = new LiteralControl();
                litSpace2.ID = "ltSpace2" + i.ToString();
                litSpace2.Text = "&nbsp;";

                newFilter.Controls.Add(ddlColumnName);
                newFilter.Controls.Add(litSpace);
                newFilter.Controls.Add(ddlColumnOperation);
                newFilter.Controls.Add(litSpace2);
                newFilter.Controls.Add(txtColumnValue);

                newFilter.Controls.Add(updateFilterButton);
                newFilter.Controls.Add(removeFilterButton);

                pnlNewFilter.Controls.Add(newFilter);


            }
        }

        void txtColumnValue_TextChanged(object sender, EventArgs e)
        {
            int filterID = Int32.Parse(((TextBox)sender).ID.Split('_')[1]);
            Panel p = (Panel)(pnlNewFilter.FindControl("dataFilters" + filterID.ToString()));
            filters[filterID].ColumnName = ((DropDownList)p.FindControl("ddlColumnName_" + filterID.ToString())).SelectedValue;
            filters[filterID].Operation = ((DropDownList)p.FindControl("ddlColumnOperation_" + filterID.ToString())).SelectedValue;
            filters[filterID].ColumnValue = ((TextBox)p.FindControl("txtColumnValue_" + filterID.ToString())).Text;
            Session[FilterSessionID + "DataFilters"] = filters;
            LoadFilterPanel();
            OnDataBound();
        }
        protected void updateFilterButton_Click(object sender, EventArgs e)
        {
            int filterID = Int32.Parse(((Button)sender).ID.Split('_')[1]);
            Filter f = new Filter();
            f = filters[filterID];
            Panel p = (Panel)(pnlNewFilter.FindControl("dataFilters" + filterID.ToString()));
            f.ColumnName = ((DropDownList)p.FindControl("ddlColumnName_" + filterID.ToString())).SelectedValue;
            f.Operation = ((DropDownList)p.FindControl("ddlColumnOperation_" + filterID.ToString())).SelectedValue;
            f.ColumnValue = ((TextBox)p.FindControl("txtColumnValue_" + filterID.ToString())).Text;
            UpdateFilter(filterID, f);
        }

        protected void removeFilterButton_Click(object sender, EventArgs e)
        {
            int filterID = Int32.Parse(((Button)sender).ID.Split('_')[1]);
            RemoveFilterAt(filterID);

        }

        protected void btnAddNewFilter_Click(object sender, EventArgs e)
        {
            Session[filterSessionID + "Prefix"] = "";
            AddNewFilter();

            btnAddNewFilter.Visible = false;

        }

        private void AddNewFilter()
        {

            addFilterPanel = (Panel)Session[FilterSessionID + "AddFilterPanel"];

            //Creating The Panel
            Panel newFilter = new Panel();

            string strRandomValue = RandomString(4);
            newFilter.ID = "filterPanel" + strRandomValue;
            
            //Creating Submit Button
            Button submitFilterButton = new Button();
            submitFilterButton.Text = "Add Filter";
            submitFilterButton.ID = "btnSubmit";
            submitFilterButton.CssClass = "ilbuttons";
            submitFilterButton.Click += new EventHandler(submitFilterButton_Click);
            // Set Submit Button a Default Button Of Filter Panel
            newFilter.DefaultButton = submitFilterButton.ID;

            //Creating Cancel Button
            Button cancelFilterButton = new Button();
            cancelFilterButton.Text = "Cancel";
            cancelFilterButton.CssClass = "ilbuttons";
            cancelFilterButton.ID = "btnCancel";
            cancelFilterButton.Click += new EventHandler(cancelFilterButton_Click);
            
            cancelFilterButton.CausesValidation = false;

            //Creating Culomn Name Drop Down List
            DropDownList ddlColumnName = new DropDownList();
            ddlColumnName.ID = "ddlColumnName";
            ddlColumnName.Items.AddRange(BuildDataColumns());
            ddlColumnName.Style.Add(HtmlTextWriterStyle.Direction, "ltr");

            //Creating Culomn Operation Drop Down List
            // Added By Amit
            ListItem lstEquals = new ListItem("Equals To", "=");
            ListItem lstLike = new ListItem("Like", "LIKE");
            ListItem lstEqualsNo = new ListItem("Not Equals", "<>");
            ListItem lstGrater = new ListItem("Greater Than", ">");
            ListItem lstLess = new ListItem("Less than", "<");
            //

            //Creating Culomn Operation Drop Down List
            DropDownList ddlColumnOperation = new DropDownList();
            ddlColumnOperation.ID = "ddlColumnOperation";

            ddlColumnOperation.Items.Add(lstLike);
            ddlColumnOperation.Items.Add(lstEquals);
            ddlColumnOperation.Items.Add(lstEqualsNo);
            ddlColumnOperation.Items.Add(lstGrater);
            ddlColumnOperation.Items.Add(lstLess);

            /*
            ddlColumnOperation.Items.Add("Equals To", "=");
            ddlColumnOperation.Items.Add("Like", "LIKE");
            ddlColumnOperation.Items.Add("Not Like", "NOT LIKE");
            ddlColumnOperation.Items.Add("Not Equals", "<>");
            ddlColumnOperation.Items.Add("Greater Than", ">");
            ddlColumnOperation.Items.Add("Greater Than Equal", ">=");
            ddlColumnOperation.Items.Add("Less Than", "<");
            ddlColumnOperation.Items.Add("Less Than Equal", "<=");
            */

            //ddlColumnOperation.Style.Add(HtmlTextWriterStyle.Direction, "rtl");
            //ddlColumnOperation.CssClass = "checkBoxs";

            // Literal COntrol for Space b/w Dynamic control
            LiteralControl litSpace = new LiteralControl();
            litSpace.ID = "ltSpace1";// +filtersCounter.ToString();
            litSpace.Text = "&nbsp;";
            //
            LiteralControl litSpace2 = new LiteralControl();
            litSpace2.ID = "ltSpace2";// +filtersCounter.ToString();
            litSpace2.Text = "&nbsp;";

            //Create Value Textbox
            TextBox txtColumnValue = new TextBox();
            txtColumnValue.ID = "txtColumnValue";
            txtColumnValue.CssClass = "SearchTextbox";
            txtColumnValue.Style.Add(HtmlTextWriterStyle.Direction, "ltr");


            newFilter.Controls.Add(ddlColumnName);
          //  newFilter.Controls.Add(litSpace);
            newFilter.Controls.Add(ddlColumnOperation);
           // newFilter.Controls.Add(litSpace2);
            newFilter.Controls.Add(txtColumnValue);
            // Submit Filter Button Required Field Valdator
            RequiredFieldValidator RFVFilter = new RequiredFieldValidator();
	    
            //------------------start Billing----------------
            //RFVFilter.ValidationGroup = "FilterValdation";
	     RFVFilter.ValidationGroup = newFilter.ID;
            //------------------end Billing----------------


            RFVFilter.InitialValue = "";
            RFVFilter.ControlToValidate = txtColumnValue.ID;
	     //------------------start Billing----------------
            //submitFilterButton.ValidationGroup = "FilterValdation";
	      submitFilterButton.ValidationGroup = newFilter.ID;
            //------------------end Billing----------------	
            newFilter.Controls.Add(RFVFilter);

            newFilter.Controls.Add(submitFilterButton);
            newFilter.Controls.Add(cancelFilterButton);

            Session.Add(FilterSessionID + "AddFilterPanel", newFilter);
            LoadAddFilterPanel(newFilter);


        }

        private void LoadAddFilterPanel(Panel p)
        {



            if (p != null)
            {

                foreach (Control c in p.Controls)
                {
                    if (c.GetType() == typeof(Button))
                    {
                        Button b = (Button)c;
                        if (b != null)
                        {
                            if (b.ID.StartsWith("btnS"))
                            {
                                b.Click += new EventHandler(this.submitFilterButton_Click);
                            }
                            else if (b.ID.StartsWith("btnC"))
                            {
                                b.Click += new EventHandler(this.cancelFilterButton_Click);
                            }

                        }
                    }

                    if (c.GetType() == typeof(TextBox))
                    {
                        TextBox t = (TextBox)c;
                        if (t != null)
                        {
                            //t.TextChanged += new EventHandler(this.txtColumnValue_TextChanged);
                        }
                    }

                }
                pnlNewFilter.Controls.Add(p);
            }


        }

        private ListItem[] BuildDataColumns()
        {

            ListItem[] li = new ListItem[CountBoundDataColumns()];

            int i = 0;
            int j = 0;
            while (i < DataColumns.Count)
            {
                if (DataColumns[i].GetType() == typeof(BoundField))
                {
                    li[j] = new ListItem(((BoundField)DataColumns[i]).HeaderText, ((BoundField)DataColumns[i]).DataField);
                    j++;
                }
                i++;
            }

            return li;
        }

        private int CountBoundDataColumns()
        {
            try
            {
                int counter = 0;
                for (int i = 0; i < DataColumns.Count; i++)
                {
                    if (DataColumns[i].GetType() == typeof(BoundField))
                    {
                        counter++;
                    }
                }
                return counter;
            }
            catch
            {
                throw new Exception("Can not add filters");
            }
        }

        void cancelFilterButton_Click(object sender, EventArgs e)
        {
            Session.Remove(FilterSessionID + "AddFilterPanel");


            Panel p = (Panel)((Button)sender).Parent;

            pnlNewFilter.Controls.Remove(p);

            if (Filters.Count > 0)
            {
                btnAndNewFilter.Visible = true;
                btnOrNewFilter.Visible = true;
            }
            else
            {
                AddFirstFilter();
                //btnAddNewFilter.Visible = true;
            }
        }

        void submitFilterButton_Click(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            Session.Remove(FilterSessionID + "AddFilterPanel");
            Button b = (Button)sender;
            Panel p = (Panel)((Button)sender).Parent;
            string pref = Session[filterSessionID + "Prefix"].ToString();
            AddFilter(new Filter(Session[filterSessionID + "Prefix"].ToString(), ((DropDownList)p.FindControl("ddlColumnName")).SelectedValue, ((DropDownList)p.FindControl("ddlColumnOperation")).SelectedValue, ((TextBox)p.FindControl("txtColumnValue")).Text));

            pnlNewFilter.Controls.Remove(p);

            OnDataBound();


        }

        public void AddFilter(Filter f)
        {
            if (filters.Count > 0 && f.Prefix == "")
            {
                f.Prefix = "AND";
            }
            filters.Add(f);
            if (filters.Count > 0)
            {
                filters[0].Prefix = "";
            }
            Session[FilterSessionID + "DataFilters"] = filters;
            LoadFilterPanel();
            btnAddNewFilter.Visible = false;
            btnAndNewFilter.Visible = true;
            btnOrNewFilter.Visible = true;
            OnDataBound();
        }

        public void InsertFilter(int index, Filter f)
        {
            filters.Insert(index, f);
            if (filters.Count > 0)
            {
                filters[0].Prefix = "";
            }
        }

        public void RemoveFilterAt(int index)
        {
            filters.RemoveAt(index);
            if (filters.Count > 0)
            {
                filters[0].Prefix = "";

                Session[FilterSessionID + "DataFilters"] = filters;
                LoadFilterPanel();
                OnDataBound();
            }
            else
            {
                LoadFilterPanel();
                AddFirstFilter();
                OnDataBound();
                    btnAndNewFilter.Visible = false;
                    btnOrNewFilter.Visible = false;
                //    btnAddNewFilter.Visible = true;
                 
            }
            
        }

        public void RemoveFilter(Filter filter)
        {
            if (filter.ColumnValue == "")
            {
                foreach (Filter f in Filters)
                {
                    if (filter.ColumnName == f.ColumnName && filter.Operation == f.Operation)
                    {
                        Filters.Remove(f);
                        break;
                    }
                }

            }
            else
            {
                foreach (Filter f in Filters)
                {
                    if (filter.ColumnName == f.ColumnName && filter.Operation == f.Operation && filter.ColumnValue == f.ColumnValue)
                    {
                        Filters.Remove(f);
                    }
                }

            }

            if (filters.Count > 0)
            {
                filters[0].Prefix = "";
            }
            else
            {
                btnAndNewFilter.Visible = false;
                btnOrNewFilter.Visible = false;
                btnAddNewFilter.Visible = true;
            }
            Session[FilterSessionID + "DataFilters"] = filters;
            LoadFilterPanel();
            OnDataBound();
        }
    
        public void ClearFilters()
        {
            filters.Clear();
            btnAndNewFilter.Visible = false;
            btnOrNewFilter.Visible = false;
            btnAddNewFilter.Visible = true;
        }

        public void UpdateFilter(int index, Filter f)
        {
            filters[index] = f;
            Session[FilterSessionID + "DataFilters"] = filters;
            LoadFilterPanel();
            OnDataBound();
        }

        protected void btnAndNewFilter_Click(object sender, EventArgs e)
        {
            Session[filterSessionID + "Prefix"] = " AND ";
            AddNewFilter();
            btnAndNewFilter.Visible = false;
            btnOrNewFilter.Visible = false;
        }

        protected void btnOrNewFilter_Click(object sender, EventArgs e)
        {
            Session[filterSessionID + "Prefix"] = " OR ";
            AddNewFilter();
            btnAndNewFilter.Visible = false;
            btnOrNewFilter.Visible = false;

        }
    
        // Show First Filter On Page Load
        
        public void AddFirstFilter()
    {
        Session[filterSessionID + "Prefix"] = "";
        AddNewFilter();

        btnAddNewFilter.Visible = false;
    }

        public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            // 26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }
}

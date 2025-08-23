using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using BSImport;

/// <summary>
/// Summary description for SEZOperation
/// </summary>
public class SEZOperation
{
    public SEZOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region For Document Master

    public static int AddJobDocument(string JobDocumentName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobDocumentName", SqlDbType.NVarChar).Value = JobDocumentName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_inJobDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDocument(int lid, string JobStatusName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@JobDocumentName", SqlDbType.NVarChar).Value = JobStatusName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_updJobDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteJobDocument(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_delJobDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetJobDocumentMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("SEZ_GetJobDocumentMS", "command");
    }

    #endregion




    // For OtherJob - Increment JobRefNo ---------
    public static string GetGenerateSEZJobNo(int i) //int FinYearId,
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@BranchType", SqlDbType.Int).Value = i;
        cmd.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_GetNextSEZJobNo", cmd, "@OutPut");

        return Convert.ToString(SPresult);
    }


    public static int AddSEZJobDetail(string JobRefNo, int SEZType, int RequestType, int SEZMode, int ClientName, int Division, int Plant,
      int Currency, decimal ExRate, decimal AssesableValue, string InwardBENo, string InwardJobNo, DateTime InwardBEDate,
      string BENo, DateTime BEDate, string RequestId,  DateTime OutwardDate, DateTime PCDFrDahej, DateTime PCDSentClient,
      DateTime FileSentToBilling, string BillingStatus, string Remark, decimal CIFValue, int GrossWt,
      bool Discount, bool ReImport, bool PrevImport, string SupplierName,
      string BuyerName, string SchemeCode, bool PrevExpGoods, bool CessDetail, bool LicenceRegNo, bool ReExport, bool PrevExport,
      int FinYear, int updUser, int lUser)
      
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobRefNo",			  SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SEZType",			  SqlDbType.Int).Value = SEZType;
        cmd.Parameters.Add("@RequestType",        SqlDbType.Int).Value = RequestType;
        cmd.Parameters.Add("@SEZMode",			  SqlDbType.Int).Value = SEZMode;        
        cmd.Parameters.Add("@ClientName",	      SqlDbType.Int).Value = ClientName;
        cmd.Parameters.Add("@Division",           SqlDbType.Int).Value = Division;
        cmd.Parameters.Add("@Plant",              SqlDbType.Int).Value = Plant;       
        cmd.Parameters.Add("@Currency",			  SqlDbType.Int).Value = Currency;        
        cmd.Parameters.Add("@ExRate",	    	  SqlDbType.Decimal).Value = ExRate;
        cmd.Parameters.Add("@AssesableValue",	  SqlDbType.Decimal).Value = AssesableValue;
        cmd.Parameters.Add("@InwardBENo",		  SqlDbType.NVarChar).Value = InwardBENo;
        cmd.Parameters.Add("@InwardJobNo",		  SqlDbType.NVarChar).Value = InwardJobNo;        
        if (InwardBEDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@InwardBEDate",   SqlDbType.DateTime).Value = InwardBEDate;
        }
        cmd.Parameters.Add("@BENo",				  SqlDbType.NVarChar).Value = BENo;
        if (BEDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@BEDate",         SqlDbType.DateTime).Value = BEDate;
        }
        cmd.Parameters.Add("@RequestId",		  SqlDbType.NVarChar).Value = RequestId;      

        if (OutwardDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@OutwardDate",   SqlDbType.DateTime).Value = OutwardDate;
        }

        if (PCDFrDahej != DateTime.MinValue)
        {
            cmd.Parameters.Add("@PCDFrDahej",    SqlDbType.DateTime).Value = PCDFrDahej;
        }
        if (PCDSentClient != DateTime.MinValue)
        {
            cmd.Parameters.Add("@PCDSentClient", SqlDbType.DateTime).Value = PCDSentClient;
        }
        if (FileSentToBilling != DateTime.MinValue)
        {
            cmd.Parameters.Add("@FileSentToBilling", SqlDbType.DateTime).Value = FileSentToBilling;
        }       
       
        cmd.Parameters.Add("@BillingStatus",	     SqlDbType.NVarChar).Value = BillingStatus;
        cmd.Parameters.Add("@Remark",			     SqlDbType.NVarChar).Value = Remark;    
        cmd.Parameters.Add("@CIFValue",              SqlDbType.Decimal).Value = CIFValue;
        cmd.Parameters.Add("@GrossWt",               SqlDbType.Int).Value = GrossWt;

        cmd.Parameters.Add("@Discount", SqlDbType.Int).Value = Discount;
        cmd.Parameters.Add("@ReImport", SqlDbType.Int).Value = ReImport;
        cmd.Parameters.Add("@PrevImport", SqlDbType.Int).Value = PrevImport;

        cmd.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;

        cmd.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        cmd.Parameters.Add("@SchemeCode", SqlDbType.NVarChar).Value = SchemeCode;
        cmd.Parameters.Add("@PrevExpGoods", SqlDbType.Int).Value = PrevExpGoods;
        cmd.Parameters.Add("@CessDetail", SqlDbType.Int).Value = CessDetail;
        cmd.Parameters.Add("@LicenceRegNo", SqlDbType.Int).Value = LicenceRegNo;
        cmd.Parameters.Add("@ReExport", SqlDbType.Int).Value = ReExport;
        cmd.Parameters.Add("@PrevExport", SqlDbType.Int).Value = PrevExport;        

        cmd.Parameters.Add("@FinYear",          SqlDbType.Int).Value = FinYear;       
        cmd.Parameters.Add("@updUser",	        SqlDbType.Int).Value = updUser;
        cmd.Parameters.Add("@lUser",            SqlDbType.Int).Value = lUser;     
        
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insJobDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static DataSet GetSEZJobDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetSEZJobDetailById", command);
    }

    // To Check Is File Sent To Billing Or Not
    public static DataSet GetFileSentBilling(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("SEZ_GetFileSentBilling", command);
    }

    // Add multiple Documents w.r.t. Job
    public static int AddSEZDocument(int JobID, string JobRefNo, int SezDocTypeID, string strDocPath, string strFileName, int lUser)

    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobID;
        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SezDocTypeID", SqlDbType.Int).Value = SezDocTypeID;
        cmd.Parameters.Add("@strDocPath", SqlDbType.NVarChar).Value = strDocPath;
        cmd.Parameters.Add("@strFileName", SqlDbType.NVarChar).Value = strFileName;     
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insDocDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    // Add multiple Invoice Detail w.r.t. job
    public static int AddSEZInvoice(int JobID, string JobRefNo, int SEZType, string InvoiceNum, DateTime InvoiceDt,
        decimal ValueInvoice ,int ddlTermInvoice, string DescriptionProd, decimal Quantity, decimal ItemPrice, decimal ProductValue,
        decimal CTH, int ddlItemType, int lUser)

    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobID;
        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SEZType", SqlDbType.Int).Value = SEZType;        
        cmd.Parameters.Add("@InvoiceNum", SqlDbType.NVarChar).Value = InvoiceNum;

        if (InvoiceDt != DateTime.MinValue)
        {
            cmd.Parameters.Add("@InvoiceDt", SqlDbType.DateTime).Value = InvoiceDt;
        }        
        cmd.Parameters.Add("@ValueInvoice", SqlDbType.Decimal).Value = ValueInvoice;
        cmd.Parameters.Add("@ddlTermInvoice", SqlDbType.Int).Value = ddlTermInvoice;
        cmd.Parameters.Add("@DescriptionProd", SqlDbType.NVarChar).Value = DescriptionProd;
        cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;

        cmd.Parameters.Add("@ItemPrice", SqlDbType.Decimal).Value = ItemPrice;
        cmd.Parameters.Add("@ProductValue", SqlDbType.Decimal).Value = ProductValue;
        cmd.Parameters.Add("@CTH", SqlDbType.Decimal).Value = CTH;
        cmd.Parameters.Add("@ddlItemType", SqlDbType.Int).Value = ddlItemType;

        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insInvoiceDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    // Get the info for documents w.r.t. Job
    public static DataSet GetDocDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("SEZ_GetUploadedDocument", command);
    }

    //Update the SEZ Job Detail
    public static int UpdateSEZJobDetail(int JobId, int Mode, string Supplier, int Currency, decimal AssesableValue,
                decimal ExRate, string InwardBENo, DateTime InwardBEDate, string InwardJobNo, int DaysStore, string BENo, DateTime BEDate,
                string RequestId, decimal DutyAmount, DateTime InwardDate, int NoOfPackage, decimal GrossWeight, int NoOfVehicles, int BEType, 
                DateTime OutwardDate, DateTime PCDFrDahej, DateTime PCDSentClient, DateTime FileSentToBilling, string BillingStatus,
                string RNLogistics, int DutyCustom, int PackagesUnit, decimal CIFValue, string Remark,
                string BuyerName, string SchemeCode, int GrossUnit, bool Discount, bool ReImport, bool PrevImport, int Destination, int Country, int Place ,
                bool PrevExpGoods, bool CessDetail, bool LicenceRegNo, bool ReExport, bool PrevExport, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = Supplier;
        command.Parameters.Add("@Currency", SqlDbType.Int).Value = Currency;
       // command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@AssesableValue", SqlDbType.Decimal).Value = AssesableValue;
       // command.Parameters.Add("@InvoiceValue", SqlDbType.NVarChar).Value = InvoiceValue;
       // command.Parameters.Add("@Term", SqlDbType.Int).Value = Term;        
        command.Parameters.Add("@ExRate", SqlDbType.Decimal).Value = ExRate;
        command.Parameters.Add("@InwardBENo", SqlDbType.NVarChar).Value = InwardBENo;
        if (InwardBEDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InwardBEDate", SqlDbType.Date).Value = InwardBEDate;
        }
        command.Parameters.Add("@InwardJobNo", SqlDbType.NVarChar).Value = InwardJobNo;
        command.Parameters.Add("@DaysStore", SqlDbType.Int).Value = DaysStore;
        command.Parameters.Add("@BENo", SqlDbType.NVarChar).Value = BENo;
        if (BEDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BEDate", SqlDbType.Date).Value = BEDate;
        }
        command.Parameters.Add("@RequestId", SqlDbType.NVarChar).Value = RequestId;
        command.Parameters.Add("@DutyAmount", SqlDbType.Decimal).Value = DutyAmount;
        if (InwardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InwardDate", SqlDbType.Date).Value = InwardDate;
        }
        command.Parameters.Add("@NoOfPackage", SqlDbType.Int).Value = NoOfPackage;
        command.Parameters.Add("@GrossWeight", SqlDbType.Decimal).Value = GrossWeight;
        command.Parameters.Add("@NoOfVehicles", SqlDbType.Int).Value = NoOfVehicles;
        command.Parameters.Add("@ServicesProvide", SqlDbType.Int).Value = BEType;
        if (OutwardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OutwardDate", SqlDbType.Date).Value = OutwardDate;
        }
        if (PCDFrDahej != DateTime.MinValue)
        {
            command.Parameters.Add("@PCDFrDahej", SqlDbType.Date).Value = PCDFrDahej;
        }
        if (PCDSentClient != DateTime.MinValue)
        {
            command.Parameters.Add("@PCDSentClient", SqlDbType.Date).Value = PCDSentClient;
        }
        if (FileSentToBilling != DateTime.MinValue)
        {
            command.Parameters.Add("@FileSentToBilling", SqlDbType.Date).Value = FileSentToBilling;
        }
        command.Parameters.Add("@BillingStatus", SqlDbType.NVarChar).Value = BillingStatus;
        command.Parameters.Add("@RNLogistics", SqlDbType.NVarChar).Value = RNLogistics;

        command.Parameters.Add("@DutyCustom", SqlDbType.NVarChar).Value = DutyCustom;
        command.Parameters.Add("@PackagesUnit", SqlDbType.NVarChar).Value = PackagesUnit;
        command.Parameters.Add("@CIFValue", SqlDbType.NVarChar).Value = CIFValue;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        command.Parameters.Add("@SchemeCode", SqlDbType.NVarChar).Value = SchemeCode;
        command.Parameters.Add("@GrossUnit", SqlDbType.NVarChar).Value = GrossUnit;

        command.Parameters.Add("@Discount", SqlDbType.Bit).Value = Discount;
        command.Parameters.Add("@ReImport", SqlDbType.Bit).Value = ReImport;
        command.Parameters.Add("@PrevImport", SqlDbType.Bit).Value = PrevImport;

        command.Parameters.Add("@PrevExpGoods", SqlDbType.Bit).Value = PrevExpGoods;
        command.Parameters.Add("@CessDetail", SqlDbType.Bit).Value = CessDetail;
        command.Parameters.Add("@LicenceRegNo", SqlDbType.Bit).Value = LicenceRegNo;
        command.Parameters.Add("@ReExport", SqlDbType.Bit).Value = ReExport;
        command.Parameters.Add("@PrevExport", SqlDbType.Bit).Value = PrevExport;
        


        command.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = Destination;
        command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Country;
        command.Parameters.Add("@Place", SqlDbType.NVarChar).Value = Place;
        
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_updJobDetail", command, "@OutPut");     // Update query pending

        return Convert.ToInt32(SPresult);
    }

    public static int AddSEZStatus(int JobId, int statusddl, string statusRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@statusddl", SqlDbType.Int).Value = statusddl;
        
        command.Parameters.Add("@statusRemark", SqlDbType.NVarChar).Value = statusRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insSEZJobStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetSEZJobDetailByJobRefNo(string JobRefNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        return CDatabase.GetDataSet("SEZ_GetSEZJobDetailByJobRefNo", command);
    }
    public static DataSet GetInvoiceDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("SEZ_GetInvDetForInwaToOut", command);
    }
    public static void FillCurrency(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetCurrencyMS", command, "Currency", "lid");
    }

    public static DataSet GetJobIdFromJobRefNo(string JobRefNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        return CDatabase.GetDataSet("SEZ_GetJobIDFrJobRefNo", command);
    }

    // Get Term ID at the time of excel Upload

    public static DataSet GetTermIdFromTerm(string Term)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@Term", SqlDbType.NVarChar).Value = Term;

        return CDatabase.GetDataSet("SEZ_GetTermIdFromTerm", command);
    }


    #region For Gross Weight Unit Master

    public static int AddGrossWtUnit(string GrossWtUnit, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@GName", SqlDbType.NVarChar).Value = GrossWtUnit;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insGrossWtUnit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateGrossWtUnit(int Gid, string GrossWtUnit, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Gid", SqlDbType.Int).Value = Gid;
        command.Parameters.Add("@UName", SqlDbType.NVarChar).Value = GrossWtUnit;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_updGrossWtUnit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteGrossWtUnit(int Gid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Gid", SqlDbType.Int).Value = Gid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_delGrossWtUnit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetGrossWtUnitMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("SEZ_GetGrossWtUnitMS", "command");
    }

    public static void FillGrossWtUnit(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "SEZ_GetGrossWtUnitMS", command, "UName", "Gid");
    }

    #endregion

    #region For Destination Master

    public static int AddDestination(string DName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@DName", SqlDbType.NVarChar).Value = DName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insDestination", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDestination(int Did, string DName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Did", SqlDbType.Int).Value = Did;
        command.Parameters.Add("@DName", SqlDbType.NVarChar).Value = DName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_updDestination", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDestination(int Did, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Did", SqlDbType.Int).Value = Did;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_delDestination", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetDestinationMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("SEZ_GetDestinationMS", "command");
    }

    public static void FillDestinationMS(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "SEZ_GetDestinationMS", command, "DName", "Did");
    }

    #endregion

    #region For Country of Origin Master

    public static int AddCountryOrigin(string CName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CName", SqlDbType.NVarChar).Value = CName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insCountryOrigin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCountryOrigin(int Cid, string CName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Cid", SqlDbType.Int).Value = Cid;
        command.Parameters.Add("@CName", SqlDbType.NVarChar).Value = CName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_updCountryOrigin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCountryOrigin(int Cid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Cid", SqlDbType.Int).Value = Cid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_delCountryOrigin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetCountryOriginMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("SEZ_GetCountryOriginMS", "command");
    }

    public static void FillCountryOriginMS(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "SEZ_GetCountryOriginMS", command, "CName", "Cid");
    }


    #endregion

    #region For Place of Origin Master

    public static int AddPlaceOrigin(string PName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PName", SqlDbType.NVarChar).Value = PName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insPlaceOrigin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdatePlaceOrigin(int Pid, string PName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Pid", SqlDbType.Int).Value = Pid;
        command.Parameters.Add("@PName", SqlDbType.NVarChar).Value = PName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_updPlaceOrigin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeletePlaceOrigin(int Pid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Pid", SqlDbType.Int).Value = Pid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_delPlaceOrigin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetPlaceOriginMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("SEZ_GetPlaceOriginMS", "command");
    }

    public static void FillPlaceOriginMS(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "SEZ_GetPlaceOriginMS", command, "PName", "Pid");
    }

    #endregion

    #region Import DSR Excel Insert Job Record (Inward/Outward)
    //For DTA Sales - 3
    public static int AddSEZExcelJobDetail(string JobRefNo, int SEZType, int CustID, int DivisionId, int PlantId, int RequestType, 
                          string strReqID, string strBENo, DateTime BEDate, 
                          int Mode, decimal ExRate, string GoodsMeasureUnit, bool Discount, bool ReImport, bool PrevImport, string Currency,
                          string SupplierName, decimal AssesableVal, decimal CIFVal,
                          int FinYear, int updUser, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SEZType", SqlDbType.Int).Value = SEZType;        
        cmd.Parameters.Add("@CustID", SqlDbType.Int).Value = CustID;
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        cmd.Parameters.Add("@RequestType", SqlDbType.Int).Value = RequestType;
        cmd.Parameters.Add("@strReqID", SqlDbType.NVarChar).Value = strReqID;
        cmd.Parameters.Add("@strBENo", SqlDbType.NVarChar).Value = strBENo;

        if (BEDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@BEDate", SqlDbType.DateTime).Value = BEDate;
        }
        cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        cmd.Parameters.Add("@ExRate", SqlDbType.Decimal).Value = ExRate;
        cmd.Parameters.Add("@GoodsMeasureUnit", SqlDbType.NVarChar).Value = GoodsMeasureUnit;
        cmd.Parameters.Add("@Discount", SqlDbType.Bit).Value = Discount;
        cmd.Parameters.Add("@ReImport", SqlDbType.Bit).Value = ReImport;
        cmd.Parameters.Add("@PrevImport", SqlDbType.Bit).Value = PrevImport;
        cmd.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;
        cmd.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
        cmd.Parameters.Add("@AssesableVal", SqlDbType.Decimal).Value = AssesableVal;
        cmd.Parameters.Add("@CIFVal", SqlDbType.Decimal).Value = CIFVal;      
       
        cmd.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        cmd.Parameters.Add("@updUser", SqlDbType.Int).Value = updUser;
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insExcelJobDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    // For DTAP -5
    public static int AddSEZExcelDTAPJobDetail(string JobRefNo, int SEZType, int CustID, int DivisionId, int PlantId, int RequestType,
                        string strReqID, string strBENo, DateTime BEDate,
                        decimal ExRate, string Currency, decimal DutyAmount, int FinYear, int updUser, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SEZType", SqlDbType.Int).Value = SEZType;
        cmd.Parameters.Add("@CustID", SqlDbType.Int).Value = CustID;
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        cmd.Parameters.Add("@RequestType", SqlDbType.Int).Value = RequestType;
        cmd.Parameters.Add("@strReqID", SqlDbType.NVarChar).Value = strReqID;
        cmd.Parameters.Add("@strBENo", SqlDbType.NVarChar).Value = strBENo;

        if (BEDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@BEDate", SqlDbType.DateTime).Value = BEDate;
        }
      
        cmd.Parameters.Add("@ExRate", SqlDbType.Decimal).Value = ExRate;       
        cmd.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;
        cmd.Parameters.Add("@DutyAmount", SqlDbType.Decimal).Value = DutyAmount;        

        cmd.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        cmd.Parameters.Add("@updUser", SqlDbType.Int).Value = updUser;
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insDTAPExcelJobDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    // For Shipping Bill - 3
    public static int AddSEZExcelShippingBillJobDetail(string JobRefNo, int SEZType, int CustID, int DivisionId, int PlantId, int RequestType,
                        string strReqID, string strBENo, DateTime BEDate,
                        decimal ExRate, string Currency, string BuyerName, string GoodsMeasureUnit,
                        string SchemeCode, bool PrevExpGoods, bool CessDetail, bool LicenceRegNo, bool ReExport, bool PrevExport,
                        int FinYear, int updUser, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SEZType", SqlDbType.Int).Value = SEZType;
        cmd.Parameters.Add("@CustID", SqlDbType.Int).Value = CustID;
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        cmd.Parameters.Add("@RequestType", SqlDbType.Int).Value = RequestType;
        cmd.Parameters.Add("@strReqID", SqlDbType.NVarChar).Value = strReqID;
        cmd.Parameters.Add("@strBENo", SqlDbType.NVarChar).Value = strBENo;

        if (BEDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@BEDate", SqlDbType.DateTime).Value = BEDate;
        }

        cmd.Parameters.Add("@ExRate", SqlDbType.Decimal).Value = ExRate;
        cmd.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;
        cmd.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        cmd.Parameters.Add("@GoodsMeasureUnit", SqlDbType.NVarChar).Value = GoodsMeasureUnit;

        cmd.Parameters.Add("@SchemeCode", SqlDbType.NVarChar).Value = SchemeCode;
        cmd.Parameters.Add("@PrevExpGoods", SqlDbType.Bit).Value = PrevExpGoods;
        cmd.Parameters.Add("@CessDetail", SqlDbType.Bit).Value = CessDetail;
        cmd.Parameters.Add("@LicenceRegNo", SqlDbType.Bit).Value = LicenceRegNo;
        cmd.Parameters.Add("@ReExport", SqlDbType.Bit).Value = ReExport;
        cmd.Parameters.Add("@PrevExport", SqlDbType.Bit).Value = PrevExport;
        
        cmd.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        cmd.Parameters.Add("@updUser", SqlDbType.Int).Value = updUser;
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insSBExcelJobDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    // Add/Insert Invoice Detail W.r.t. Excel Given Job

    public static int AddSEZExcelInvoice(int JobID, string JobRefNo, string strReqID, int SEZType, string InvoiceNum, DateTime InvoiceDt,
           string InvoiceType, decimal InvoiceValue, string Description, decimal Quantity, decimal ItemPrice, decimal Productvalue, decimal CTH, string ItemType, int lUser)

    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobID;
        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@SEZType", SqlDbType.Int).Value = SEZType;
        cmd.Parameters.Add("@InvoiceNum", SqlDbType.NVarChar).Value = InvoiceNum;
        cmd.Parameters.Add("@strReqID", SqlDbType.NVarChar).Value = strReqID;

        if (InvoiceDt != DateTime.MinValue)
        {
            cmd.Parameters.Add("@InvoiceDt", SqlDbType.DateTime).Value = InvoiceDt;
        }

        cmd.Parameters.Add("@InvoiceType", SqlDbType.NVarChar).Value = InvoiceType;
        cmd.Parameters.Add("@InvoiceValue", SqlDbType.Decimal).Value = InvoiceValue;
        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;
        cmd.Parameters.Add("@ItemPrice", SqlDbType.Decimal).Value = ItemPrice;
        cmd.Parameters.Add("@Productvalue", SqlDbType.Decimal).Value = Productvalue;
        cmd.Parameters.Add("@CTH", SqlDbType.Decimal).Value = CTH;
        cmd.Parameters.Add("@ItemType", SqlDbType.NVarChar).Value = ItemType;

        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insExcelInvoiceDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    // Insert Detail in all MASTERS 

    public static int AddSEZExcelFillMasters(string GoodsMeasurementUnit, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        //command.Parameters.Add("@DutyCustomPF", SqlDbType.NVarChar).Value = DutyCustomPF;
        //command.Parameters.Add("@PackageUnit", SqlDbType.NVarChar).Value = PackageUnit;
        //command.Parameters.Add("@BEType", SqlDbType.NVarChar).Value = BEType;
        command.Parameters.Add("@GoodsMeasurementUnit", SqlDbType.NVarChar).Value = GoodsMeasurementUnit;
        //command.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = Destination;
        //command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Country;
        //command.Parameters.Add("@Place", SqlDbType.NVarChar).Value = Place;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SEZ_insExcelAllMasters", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetGidFromGrossWtUnit(string Uname)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UName", SqlDbType.NVarChar).Value = Uname;
        return CDatabase.GetDataSet("SEZ_GetIDForGrossWtUnit", command);
    }  

    public static DataSet GetDidFromDestination(string Dname)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DName", SqlDbType.NVarChar).Value = Dname;
        return CDatabase.GetDataSet("SEZ_GetIDForDestination", command);
    }  

    public static DataSet GetCidFromCountry(string Cname)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CName", SqlDbType.NVarChar).Value = Cname;

        return CDatabase.GetDataSet("SEZ_GetIDForCountry", command);
    }
    public static DataSet GetPidFromPlace(string Pname)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PName", SqlDbType.NVarChar).Value = Pname;
        return CDatabase.GetDataSet("SEZ_GetIDForPlace", command);
    }

    public static DataSet GetlidFromPackage(string Sname)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@SName", SqlDbType.NVarChar).Value = Sname;
        return CDatabase.GetDataSet("SEZ_GetIDForPackageUnit", command);
    }


    #endregion

    #region Container Details

    public static int ADDSEZContainer(string JobRefNO, string ContanerNo, int ContanerSize, int ContanerType, int UserID)
    {
        SqlCommand command = new SqlCommand();
        // CustomerId ,MaterialSupplied ,CommodityName ,HSNCode,IUser 
        string SPresult = "";
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNO;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContanerNo;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContanerSize;
        command.Parameters.Add("@ContainerType", SqlDbType.NVarChar).Value = ContanerType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("SEZ_insSEZContianer", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetContainerDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("SEZ_GetContainerResult", command);
    }

    public static DataSet GetContainerDetailById(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("SEZ_GetContainer", command);
    }

    #endregion


    #region NEW Changes

    public static int AddInvoiceDetail(int JobId, int InvoiceId, DateTime dtInvDate, string InvValue, string description, string quantity, string itemPrice, string ProdValue, string strCTH, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@dtInvDate", SqlDbType.DateTime).Value = dtInvDate;

        command.Parameters.Add("@InvValue", SqlDbType.NVarChar).Value = InvValue;
        command.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
        command.Parameters.Add("@quantity", SqlDbType.NVarChar).Value = quantity;
        command.Parameters.Add("@itemPrice", SqlDbType.NVarChar).Value = itemPrice;
        command.Parameters.Add("@ProdValue", SqlDbType.NVarChar).Value = ProdValue;
        command.Parameters.Add("@CTH", SqlDbType.NVarChar).Value = strCTH;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updSEZInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int updSEZContainer(string JobRefNo, string ContanerNo, string ContanerSize, string ContanerType, int UserID)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContanerNo;
        command.Parameters.Add("@ContainerSize", SqlDbType.NVarChar).Value = ContanerSize;
        command.Parameters.Add("@ContainerType", SqlDbType.NVarChar).Value = ContanerType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("SEZ_insSEZContianer", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region SEZ Report

    public static int AddSEZReport(string ReportName, string ColumnListId, string strConditionColumnId, int ReporType, int CustomerId, int IsCustomer, int lUser, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = strConditionColumnId;
        command.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReporType;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@IsCustomer", SqlDbType.Int).Value = IsCustomer;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insAdhocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetSEZReportFieldGroupTest()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("SEZ_GetFieldGroup", cmd);
    }

    public static DataSet GetSEZReportChildNodeTest(string ParentNode, int ReportType)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;
        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        return CDatabase.GetDataSet("GetSEZReportChildNodeByParentNode", cmd);
    }


    public static DataView GetSEZReport(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateFrom;
        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTo;
        if (DeliveryStatus != "")
            cmd.Parameters.Add("@DeliveryStatus", SqlDbType.Char).Value = DeliveryStatus;
        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = AdhocFilter;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataView("SEZ_rptAdHocReport", cmd);
    }


    #endregion

}
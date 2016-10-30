using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text;
using System.IO;
using AMCL.DL;
using AMCL.BL;
using AMCL.UTILITY;
using AMCL.GATEWAY;
using AMCL.COMMON;

public partial class ReportViewer_UnitReportLedgerReportViewer : System.Web.UI.Page
{

    OMFDAO opendMFDAO = new OMFDAO();
    UnitHolderRegBL unitHolderRegBLObj = new UnitHolderRegBL();
    Message msgObj = new Message();
    UnitUser userObj = new UnitUser();
    NumberToEnglish numberToEnglisObj = new NumberToEnglish();
    UnitReport reportObj = new UnitReport();
    CommonGateway commonGatewayObj = new CommonGateway();
    BaseClass bcContent = new BaseClass();
    ReportDocument rdoc = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        string fundCode = "";
        string branchCode="";
        string regiNo = "";
        if (BaseContent.IsSessionExpired())
        {
            Response.Redirect("../../Default.aspx");
            return;
        }
        bcContent = (BaseClass)Session["BCContent"];

        userObj.UserID = bcContent.LoginID.ToString();
          
        branchCode = (string)Session["branchCode"];
        fundCode = (string)Session["fundCode"];
        regiNo = (string)Session["regiNo"];


        


        DataTable dtLedgerForReport = (DataTable)Session["dtLedgerForReport"];
        dtLedgerForReport.TableName = "dtLedgerForReport";

        if (dtLedgerForReport.Rows.Count > 0)
        {
            UnitHolderRegistration unitRegObj = new UnitHolderRegistration();
            unitRegObj.FundCode = fundCode.ToString();
            unitRegObj.BranchCode = branchCode.ToString();
            unitRegObj.RegNumber = regiNo.ToString();
            DataTable dtLedgerRegInfo = opendMFDAO.getDtRegInfo(unitRegObj);
            dtLedgerRegInfo.TableName = "dtLedgerRegInfo";
            string regiNumber=dtLedgerRegInfo.Rows[0]["REG_BK"].ToString()+"/"+dtLedgerRegInfo.Rows[0]["REG_BR"].ToString()+"/"+dtLedgerRegInfo.Rows[0]["REG_NO"].ToString();
            string holderName=dtLedgerRegInfo.Rows[0]["HNAME"].Equals(DBNull.Value)? "":dtLedgerRegInfo.Rows[0]["HNAME"].ToString();
            string JointHolderName=dtLedgerRegInfo.Rows[0]["JNT_NAME"].Equals(DBNull.Value)? "":dtLedgerRegInfo.Rows[0]["JNT_NAME"].ToString();
            string address1=dtLedgerRegInfo.Rows[0]["ADDRS1"].Equals(DBNull.Value)? "":dtLedgerRegInfo.Rows[0]["ADDRS1"].ToString();
            string address2=dtLedgerRegInfo.Rows[0]["ADDRS2"].Equals(DBNull.Value)? "":dtLedgerRegInfo.Rows[0]["ADDRS2"].ToString();
            string city=dtLedgerRegInfo.Rows[0]["CITY"].Equals(DBNull.Value)? "":dtLedgerRegInfo.Rows[0]["CITY"].ToString();
            string cip=dtLedgerRegInfo.Rows[0]["CIP"].Equals(DBNull.Value)? "":dtLedgerRegInfo.Rows[0]["CIP"].Equals("Y") ? "YES":"NO";
            string regType = dtLedgerRegInfo.Rows[0]["REG_TYPE"].Equals(DBNull.Value) ? "" : reportObj.getRegTypeFullName(dtLedgerRegInfo.Rows[0]["REG_TYPE"].ToString());




       dtLedgerForReport.WriteXmlSchema(@"D:\Project\Web\AMCL.OpenEndMF\UI\ReportViewer\Report\dtLedgerForReport.xsd");


            string Path = Server.MapPath("Report/rptLedgerStatement.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtLedgerForReport);
            rdoc.Refresh();
            CrystalReportViewer1.ReportSource = rdoc;
            rdoc.SetParameterValue("fundName", opendMFDAO.GetFundName(fundCode.ToString()));
            rdoc.SetParameterValue("branchName", opendMFDAO.GetBranchName(branchCode.ToString()).ToString());
            rdoc.SetParameterValue("branchCode", branchCode.ToString());
            rdoc.SetParameterValue("Regi_No", regiNumber);
            rdoc.SetParameterValue("Holder_Name", holderName.ToString());
            rdoc.SetParameterValue("Jholder_name", JointHolderName.ToString());
            rdoc.SetParameterValue("Address1", address1.ToString());
            rdoc.SetParameterValue("Address2", address2.ToString());
            rdoc.SetParameterValue("City", city.ToString());
            rdoc.SetParameterValue("Cip", cip.ToString());
            rdoc.SetParameterValue("Reg_Type", regType.ToString());

           //rdoc.ExportToDisk(ExportFormatType.PortableDocFormat, "E:\\test.pdf");                       

        }
        else
        {
            Response.Write("No data found");
        }




    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        CrystalReportViewer1.Dispose();
        CrystalReportViewer1 = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}

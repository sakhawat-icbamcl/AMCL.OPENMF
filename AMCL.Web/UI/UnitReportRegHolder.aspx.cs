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
using System.Text;
using System.Data.OracleClient;
using AMCL.DL;
using AMCL.BL;
using AMCL.UTILITY;
using AMCL.GATEWAY;
using AMCL.COMMON;


public partial class UI_UnitReportRegHolder : System.Web.UI.Page
{
    OMFDAO opendMFDAO = new OMFDAO();
    UnitHolderRegBL unitHolderRegBLObj = new UnitHolderRegBL();
    Message msgObj = new Message();
    UnitUser userObj = new UnitUser();
    UnitReport reportObj = new UnitReport();
    CommonGateway commonGatewayObj = new CommonGateway();
    BaseClass bcContent = new BaseClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (BaseContent.IsSessionExpired())
        {
            Response.Redirect("../Default.aspx");
            return;
        }
        bcContent = (BaseClass)Session["BCContent"];

        userObj.UserID = bcContent.LoginID.ToString();
        string fundCode = bcContent.FundCode.ToString();
        string branchCode = bcContent.BranchCode.ToString();
        spanFundName.InnerText = opendMFDAO.GetFundName(fundCode.ToString());
        fundCodeTextBox.Text = fundCode.ToString();
        branchCodeTextBox.Text = branchCode.ToString();
    
   
       // toRegDateTextBox.Text = DateTime.Today.ToString("dd-MMM-yyyy");
       // fromRegDateTextBox.Text = DateTime.Today.ToString("dd-MMM-yyyy"); ;
        ///holderDateofBirthTextBox.Text = DateTime.Today.ToString("dd-MMM-yyyy");
        if (!IsPostBack)
        {
            
        }

    }
   

    

    private void ClearText()
    {

    }
    protected void regCloseButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("UnitHome.aspx");

    }





    protected void ShowReportButton_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sbMaster = new StringBuilder();
            StringBuilder sbFilter = new StringBuilder();
            DataTable dtHolderInfo = new DataTable();
          
            sbMaster.Append(" SELECT U_MASTER.REG_BK,U_MASTER.REG_BR,U_MASTER.REG_BK || '/' || U_MASTER.REG_BR || '/' || U_MASTER.REG_NO AS REG_NO, TO_CHAR(U_MASTER.REG_DT,'DD-MON-YYYY') AS REG_DT, U_MASTER.REG_TYPE, ");
            sbMaster.Append(" U_MASTER.HNAME, U_MASTER.FMH_NAME, OCC_CODE.DESCR AS OCC_CODE, U_MASTER.ADDRS1, U_MASTER.ADDRS2, U_MASTER.CITY, U_MASTER.MO_NAME,U_MASTER.ID_BK_NM_CD,U_MASTER.ID_BK_BR_NM_CD,");
            sbMaster.Append(" U_MASTER.B_DATE, U_MASTER.SEX, U_MASTER.MAR_STAT, U_MASTER.RELIGION, U_MASTER.EDU_QUA, U_MASTER.TEL_NO, U_MASTER.EMAIL,");
            sbMaster.Append(" U_MASTER.CIP, U_MASTER.ID_FLAG, U_MASTER.ID_AC, U_MASTER.BK_FLAG, U_MASTER.SPEC_IN1, U_MASTER.SPEC_IN2, U_MASTER.REMARKS,");
            sbMaster.Append(" U_MASTER.NATIONALITY, U_MASTER.BK_AC_NO, U_MASTER.BK_NM_CD, U_MASTER.BK_BR_NM_CD, U_JHOLDER.JNT_NAME,U_JHOLDER.JNT_MO_NAME,");
            sbMaster.Append(" U_JHOLDER.JNT_FMH_NAME, OCC_CODE_1.DESCR AS JNT_OCC_CODE, U_JHOLDER.JNT_ADDRS1, U_JHOLDER.JNT_ADDRS2,");
            sbMaster.Append(" U_JHOLDER.JNT_NATIONALITY, U_JHOLDER.JNT_CITY, U_JHOLDER.JNT_TEL_NO, U_JHOLDER.JNT_FMH_REL , U_MASTER.REG_NO AS R_NO");
            sbMaster.Append(" FROM   OCC_CODE OCC_CODE_1 RIGHT OUTER JOIN  U_JHOLDER ON OCC_CODE_1.CODE = U_JHOLDER.JNT_OCC_CODE RIGHT OUTER JOIN");
            sbMaster.Append(" OCC_CODE INNER JOIN  U_MASTER ON OCC_CODE.CODE = U_MASTER.OCC_CODE ON U_JHOLDER.REG_BK = U_MASTER.REG_BK AND ");
            sbMaster.Append(" U_JHOLDER.REG_BR = U_MASTER.REG_BR AND U_JHOLDER.REG_NO = U_MASTER.REG_NO WHERE (1 = 1)");
            sbMaster.Append(" AND (U_MASTER.REG_BK = '" + fundCodeTextBox.Text.Trim().ToString().ToUpper() + "') AND (U_MASTER.REG_BR = '" + branchCodeTextBox.Text.Trim().ToString().ToUpper() + "')");
            if (fromRegNoTextBox.Text != "" && toRegNoTextBox.Text != "")
            {
                sbFilter.Append(" AND (U_MASTER.REG_NO BETWEEN " + Convert.ToInt32(fromRegNoTextBox.Text.Trim().ToString()) + " AND " + Convert.ToInt32(toRegNoTextBox.Text.Trim().ToString()) + ")");
            }
            else if (fromRegNoTextBox.Text != "" && toRegNoTextBox.Text == "")
            {
                sbFilter.Append(" AND (U_MASTER.REG_NO> =" + Convert.ToInt32(fromRegNoTextBox.Text.Trim().ToString()) + ")");
            }
            else if (fromRegNoTextBox.Text == "" && toRegNoTextBox.Text != "")
            {
                sbFilter.Append(" AND (U_MASTER.REG_NO <=" + Convert.ToInt32(toRegNoTextBox.Text.Trim().ToString()) + ")");
            }

            if (fromRegDateTextBox.Text != "" && toRegDateTextBox.Text != "")
            {
                sbFilter.Append(" AND ( U_MASTER.REG_DT BETWEEN '" + Convert.ToDateTime(fromRegDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(toRegDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "')");
            }
            else if (fromRegDateTextBox.Text != "" && toRegDateTextBox.Text == "")
            {
                sbFilter.Append(" AND ( U_MASTER.REG_DT >= '" + Convert.ToDateTime(fromRegDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "')");
            }
            else if (fromRegDateTextBox.Text == "" && toRegDateTextBox.Text != "")
            {
                sbFilter.Append(" AND ( U_MASTER.REG_DT <= '" + Convert.ToDateTime(toRegDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "')");
            }

            sbFilter.Append(" ORDER BY R_NO ");
            sbMaster.Append(sbFilter.ToString());

          
            dtHolderInfo = commonGatewayObj.Select(sbMaster.ToString());
    
            if (dtHolderInfo.Rows.Count > 0)
            {
                Session["dtHolderInfo"] = dtHolderInfo;
                Session["fundCode"]=fundCodeTextBox.Text.Trim().ToString();
                Session["branchCode"] = branchCodeTextBox.Text.Trim().ToString();

         
                ClientScript.RegisterStartupScript(this.GetType(), "UnitHolderInfo", "window.open('ReportViewer/UnitReportRegHolderReportViewer.aspx')", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('No data found');", true);
            }
        }
        catch (Exception ex)
        {
         
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('" + ex.Message.Replace("'", "").ToString() + "');", true);
        }

    }
}

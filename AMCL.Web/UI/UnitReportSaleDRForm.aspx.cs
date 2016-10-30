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
using System.IO;
using AMCL.DL;
using AMCL.BL;
using AMCL.UTILITY;
using AMCL.GATEWAY;
using AMCL.COMMON;

public partial class UI_UnitReportSaleDRForm : System.Web.UI.Page
{
    OMFDAO opendMFDAO = new OMFDAO();
    UnitHolderRegBL unitHolderRegBLObj = new UnitHolderRegBL();
    Message msgObj = new Message();
    UnitUser userObj = new UnitUser();
    UnitReport reportObj = new UnitReport();
    CommonGateway commonGatewayObj = new CommonGateway();
    UnitHolderRegistration regObj = new UnitHolderRegistration();
    BaseClass bcContent = new BaseClass();
    UnitSaleBL unitSaleBLObj = new UnitSaleBL();
    
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
            DataTable dtReportStatement = new DataTable();

            sbMaster.Append("SELECT NVL(SALE.SL_NO,0) AS SL_NO, TO_CHAR(SALE.SL_DT, 'DD-MON-YYYY') AS SL_DT, U_MASTER.REG_BK, U_MASTER.REG_BR, U_MASTER.REG_NO AS RG_NO, NVL(SALE.CERT_RCV_BY,'') AS  CERT_RCV_BY, TO_CHAR(NVL(SALE.CERT_DLVRY_DT,''),'DD-MON-YYYY') AS  CERT_DLVRY_DT,");
            sbMaster.Append(" U_MASTER.HNAME,U_MASTER.REG_TYPE, U_JHOLDER.JNT_NAME, U_MASTER.REG_BK || '/' || U_MASTER.REG_BR || '/' || U_MASTER.REG_NO AS REG_NO,");
            sbMaster.Append(" U_MASTER.ADDRS1, U_MASTER.ADDRS2, U_MASTER.CITY, U_MASTER.SPEC_IN1, U_MASTER.SPEC_IN2, U_MASTER.BK_AC_NO,U_MASTER.TIN,U_MASTER.BO,");
            sbMaster.Append(" U_MASTER.BK_NM_CD, U_MASTER.BK_BR_NM_CD,U_MASTER.BK_FLAG, SALE.QTY, SALE.SL_PRICE AS RATE, SALE.QTY * SALE.SL_PRICE AS AMOUNT, SALE.SL_TYPE, ");
            sbMaster.Append(" U_MASTER.CIP, U_MASTER.ID_AC FROM  U_JHOLDER RIGHT OUTER JOIN SALE INNER JOIN U_MASTER ON SALE.REG_BK = U_MASTER.REG_BK AND SALE.REG_BR = U_MASTER.REG_BR AND SALE.REG_NO = U_MASTER.REG_NO ON ");
            sbMaster.Append(" U_JHOLDER.REG_BK = U_MASTER.REG_BK AND U_JHOLDER.REG_BR = U_MASTER.REG_BR AND U_JHOLDER.REG_NO = U_MASTER.REG_NO");
            sbMaster.Append(" WHERE 1=1");


            sbMaster.Append(" AND (U_MASTER.REG_BK = '" + fundCodeTextBox.Text.Trim().ToString().ToUpper() + "') AND (U_MASTER.REG_BR = '" + branchCodeTextBox.Text.Trim().ToString().ToUpper() + "')");

            if (fromSaleNoTextBox.Text != "" && toSaleNoTextBox.Text != "")
            {
                sbFilter.Append(" AND (SALE.SL_NO BETWEEN " + Convert.ToInt32(fromSaleNoTextBox.Text.Trim().ToString()) + " AND " + Convert.ToInt32(toSaleNoTextBox.Text.Trim().ToString()) + ")");
            }
            else if (fromSaleNoTextBox.Text != "" && toSaleNoTextBox.Text == "")
            {
                sbFilter.Append(" AND (SALE.SL_NO>=" + Convert.ToInt32(fromSaleNoTextBox.Text.Trim().ToString()) + ")");
            }
            else if (fromSaleNoTextBox.Text == "" && toSaleNoTextBox.Text != "")
            {
                sbFilter.Append(" AND (SALE.SL_NO<=" + Convert.ToInt32(toSaleNoTextBox.Text.Trim().ToString()) + ")");
            }

          


            sbFilter.Append(" ORDER BY SALE.SL_NO ");
            sbMaster.Append(sbFilter.ToString());
            dtReportStatement = commonGatewayObj.Select(sbMaster.ToString());
         
            if (dtReportStatement.Rows.Count > 0)
            {
                Session["dtReportStatement"] = dtReportStatement;
                Session["fundCode"]=fundCodeTextBox.Text.Trim().ToString();
                Session["branchCode"] = branchCodeTextBox.Text.Trim().ToString();

                ClientScript.RegisterStartupScript(this.GetType(), "UnitHolderInfo", "window.open('ReportViewer/UnitReportSaleReportViewer.aspx')", true);

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
 

    //public DataTable dtGetSaleWithRegInfo()
    //{
    //    StringBuilder sbMaster = new StringBuilder();
    //    StringBuilder sbFilter = new StringBuilder();
    //    DataTable dtReportStatement = new DataTable();
    //    try
    //    {           

    //        sbMaster.Append("SELECT U_MASTER.REG_BK, U_MASTER.REG_BR, U_MASTER.REG_NO, TO_CHAR(U_MASTER.REG_DT, 'DD-MON-YYYY') AS REG_DT,  U_MASTER.REG_TYPE, U_MASTER.HNAME, U_MASTER.FMH_NAME, U_MASTER.ADDRS1, U_MASTER.ADDRS2, U_MASTER.CITY,");
    //        sbMaster.Append(" U_MASTER.B_DATE, U_MASTER.SEX, U_MASTER.MAR_STAT, U_MASTER.RELIGION, U_MASTER.EDU_QUA, U_MASTER.TEL_NO, U_MASTER.EMAIL, U_MASTER.OPN_BAL, U_MASTER.BALANCE, U_MASTER.CIP, U_MASTER.ID_FLAG, U_MASTER.ID_AC, U_MASTER.BK_FLAG, U_MASTER.DIVIDEND,");
    //        sbMaster.Append(" U_MASTER.CIP_DIV, U_MASTER.TOT_DIV, U_MASTER.ENT_DT, U_MASTER.REMARKS, U_MASTER.ENT_TM, U_MASTER.NOMI1_REL, U_MASTER.NOMI2_REL, U_MASTER.NATIONALITY, U_MASTER.OCC_CODE, U_MASTER.BK_AC_NO, U_MASTER.BK_NM_CD, ");
    //        sbMaster.Append(" U_MASTER.BK_BR_NM_CD, U_MASTER.ID_BK_NM_CD, U_MASTER.ID_BK_BR_NM_CD, U_MASTER.MO_NAME, U_MASTER.USER_NM,  U_JHOLDER.JNT_NO, U_JHOLDER.JNT_NAME, U_JHOLDER.JNT_FMH_NAME, U_JHOLDER.JNT_OCC_CODE, U_JHOLDER.JNT_ADDRS1, ");
    //        sbMaster.Append(" U_JHOLDER.JNT_ADDRS2, U_JHOLDER.JNT_NATIONALITY, U_JHOLDER.JNT_CITY, U_JHOLDER.JNT_TEL_NO, U_JHOLDER.JNT_FMH_REL,U_JHOLDER.JNT_MO_NAME, SALE.SL_NO, TO_CHAR(SALE.SL_DT, 'DD-MON-YYYY') AS SL_DT, SALE.SL_PRICE, SALE.QTY, SALE.SL_TYPE, ");
    //        sbMaster.Append(" SALE.USER_NM AS SL_USER_NM, TO_CHAR(SALE.ENT_DT, 'DD-MON-YYYY') AS SL_ENT_DT, SALE.ENT_TM AS SL_ENT_TM, SALE.MONY_RECT_NO, SALE.PAY_TYPE, SALE.CHQ_DD_NO, SALE.CASH_AMT, SALE.BANK_CODE, SALE.BRANCH_CODE,");
    //        sbMaster.Append(" SALE.MULTI_PAY_REMARKS, TO_CHAR(SALE.CHEQUE_DT, 'DD-MON-YYYY') AS CHEQUE_DT FROM    U_MASTER INNER JOIN SALE ON U_MASTER.REG_BK = SALE.REG_BK AND U_MASTER.REG_BR = SALE.REG_BR AND  U_MASTER.REG_NO = SALE.REG_NO LEFT OUTER JOIN");
    //        sbMaster.Append(" U_JHOLDER ON U_MASTER.REG_BK = U_JHOLDER.REG_BK AND U_MASTER.REG_BR = U_JHOLDER.REG_BR AND   U_MASTER.REG_NO = U_JHOLDER.REG_NO");
    //        sbMaster.Append(" WHERE 1=1");


    //        sbMaster.Append(" AND (U_MASTER.REG_BK = '" + fundCodeTextBox.Text.Trim().ToString().ToUpper() + "') AND (U_MASTER.REG_BR = '" + branchCodeTextBox.Text.Trim().ToString().ToUpper() + "')");

    //        if (fromSaleNoTextBox.Text != "" && toSaleNoTextBox.Text != "")
    //        {
    //            sbFilter.Append(" AND (SALE.SL_NO BETWEEN " + Convert.ToInt32(fromSaleNoTextBox.Text.Trim().ToString()) + " AND " + Convert.ToInt32(toSaleNoTextBox.Text.Trim().ToString()) + ")");
    //        }
    //        else if (fromSaleNoTextBox.Text != "" && toSaleNoTextBox.Text == "")
    //        {
    //            sbFilter.Append(" AND (SALE.SL_NO>=" + Convert.ToInt32(fromSaleNoTextBox.Text.Trim().ToString()) + ")");
    //        }
    //        else if (fromSaleNoTextBox.Text == "" && toSaleNoTextBox.Text != "")
    //        {
    //            sbFilter.Append(" AND (SALE.SL_NO<=" + Convert.ToInt32(toSaleNoTextBox.Text.Trim().ToString()) + ")");
    //        }

    //        if (fromRegNoTextBox.Text != "" && toRegNoTextBox.Text != "")
    //        {
    //            sbFilter.Append(" AND (U_MASTER.REG_NO BETWEEN " + Convert.ToInt32(fromRegNoTextBox.Text.Trim().ToString()) + " AND " + Convert.ToInt32(toRegNoTextBox.Text.Trim().ToString()) + ")");
    //        }
    //        else if (fromRegNoTextBox.Text != "" && toRegNoTextBox.Text == "")
    //        {
    //            sbFilter.Append(" AND (U_MASTER.REG_NO >=" + Convert.ToInt32(fromRegNoTextBox.Text.Trim().ToString()) + ")");
    //        }
    //        else if (fromRegNoTextBox.Text == "" && toRegNoTextBox.Text != "")
    //        {
    //            sbFilter.Append(" AND (U_MASTER.REG_NO <=" + Convert.ToInt32(toRegNoTextBox.Text.Trim().ToString()) + ")");
    //        }


    //        if (fromSaleDateTextBox.Text != "" && toSaleDateTextBox.Text != "")
    //        {
    //            sbFilter.Append(" AND ( SL_DT BETWEEN '" + Convert.ToDateTime(fromSaleDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(toSaleDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "')");
    //        }
    //        else if (fromSaleDateTextBox.Text != "" && toSaleDateTextBox.Text == "")
    //        {
    //            sbFilter.Append(" AND ( SL_DT => '" + Convert.ToDateTime(fromSaleDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "')");
    //        }
    //        else if (fromSaleDateTextBox.Text == "" && toSaleDateTextBox.Text != "")
    //        {
    //            sbFilter.Append(" AND (SL_DT <= '" + Convert.ToDateTime(toSaleDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy") + "')");
    //        }
    //        if (saleTypeDropDownList.SelectedValue.ToString() != "0")
    //        {
    //            sbFilter.Append(" AND SL_TYPE='" + saleTypeDropDownList.SelectedValue.ToString() + "'");
    //        }


    //        sbFilter.Append(" ORDER BY SALE.SL_NO ");
    //        sbMaster.Append(sbFilter.ToString());
    //        dtReportStatement = commonGatewayObj.Select(sbMaster.ToString());
    //     }
    //    catch (Exception ex)
    //    {            
    //        throw ex;            
    //    }
    //    return dtReportStatement;

    //}
}

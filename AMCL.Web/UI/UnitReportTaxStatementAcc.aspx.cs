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
using AMCL.DL;
using AMCL.BL;
using AMCL.UTILITY;
using AMCL.GATEWAY;
using AMCL.COMMON;


public partial class UI_UnitReportTaxStatementAcc : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    OMFDAO opendMFDAO = new OMFDAO();
    UnitUser userObj = new UnitUser();
    UnitReport reportObj = new UnitReport();
    BaseClass bcContent = new BaseClass();
    UnitLienBl unitLienBLObj = new UnitLienBl();
    dividendDAO diviDAOObj = new dividendDAO();
    string fundCode = "";
    string branchCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        UnitHolderRegistration regObj = new UnitHolderRegistration();
       
        if (BaseContent.IsSessionExpired())
        {
            Response.Redirect("../Default.aspx");
            return;
        }
        bcContent = (BaseClass)Session["BCContent"];

        userObj.UserID = bcContent.LoginID.ToString();
        fundCode = bcContent.FundCode.ToString();
        branchCode = bcContent.BranchCode.ToString();
        spanFundName.InnerText = opendMFDAO.GetFundName(fundCode.ToString());
      

        
        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = opendMFDAO.dtFundList();
            fundNameDropDownList.DataTextField = "FUND_NM";
            fundNameDropDownList.DataValueField = "FUND_CD";
            fundNameDropDownList.DataBind();

               
        }
    
    }
   
    
    protected void CloseButton_Click(object sender, EventArgs e)
    {         
        Response.Redirect("UnitHome.aspx");
           
    }

    protected void PrintButton_Click(object sender, EventArgs e)
    {
        StringBuilder sbQuery = new StringBuilder();
        sbQuery.Append("SELECT 'NON-CIP:' AS CIP_TYPE, 'HO' AS BRANCH, SUM(DIVIDEND.BALANCE) AS NO_UNITS, SUM(DIVIDEND.TOT_DIVI) AS TOTAL_DIVIDEND, ");
        sbQuery.Append(" NVL(SUM(DIVIDEND.CIP_QTY), 0) AS TOTAL_CIP, SUM(DIVIDEND.DIDUCT) AS TAX_DIDUCT, SUM(DIVIDEND.FI_DIVI_QTY) AS NET_DIVDEND ");
        sbQuery.Append(" FROM  DIVI_PARA INNER JOIN   DIVIDEND ON DIVI_PARA.FUND_CD = DIVIDEND.FUND_CD AND DIVI_PARA.CLOSE_DT = DIVIDEND.CLOSE_DT AND ");
        sbQuery.Append(" DIVI_PARA.F_YEAR = DIVIDEND.FY AND DIVI_PARA.DIVI_NO = DIVIDEND.DIVI_NO WHERE  (DIVIDEND.CIP = 'N') AND (DIVIDEND.REG_BR = 'AMC/01')");
        sbQuery.Append(" AND (DIVI_PARA.FUND_CD = '"+fundNameDropDownList.SelectedValue.ToString().ToUpper()+"')AND (DIVI_PARA.F_YEAR = '" + DividendFYDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" AND (DIVI_PARA.CLOSE_DT = '" + ClosingDateDropDownList.SelectedItem.ToString().ToUpper() + "') AND (DIVI_PARA.FY_PART = '" + fyPartDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" UNION ALL ");
        sbQuery.Append("SELECT 'NON-CIP:' AS CIP_TYPE, 'BRANCH' AS BRANCH, SUM(DIVIDEND.BALANCE) AS NO_UNITS, SUM(DIVIDEND.TOT_DIVI) AS TOTAL_DIVIDEND, ");
        sbQuery.Append(" NVL(SUM(DIVIDEND.CIP_QTY), 0) AS TOTAL_CIP, SUM(DIVIDEND.DIDUCT) AS TAX_DIDUCT, SUM(DIVIDEND.FI_DIVI_QTY) AS NET_DIVDEND ");
        sbQuery.Append(" FROM  DIVI_PARA INNER JOIN   DIVIDEND ON DIVI_PARA.FUND_CD = DIVIDEND.FUND_CD AND DIVI_PARA.CLOSE_DT = DIVIDEND.CLOSE_DT AND ");
        sbQuery.Append(" DIVI_PARA.F_YEAR = DIVIDEND.FY AND DIVI_PARA.DIVI_NO = DIVIDEND.DIVI_NO WHERE  (DIVIDEND.CIP = 'N') AND (DIVIDEND.REG_BR <> 'AMC/01')");
        sbQuery.Append(" AND (DIVI_PARA.FUND_CD = '" + fundNameDropDownList.SelectedValue.ToString().ToUpper() + "')AND (DIVI_PARA.F_YEAR = '" + DividendFYDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" AND (DIVI_PARA.CLOSE_DT = '" + ClosingDateDropDownList.SelectedItem.ToString().ToUpper() + "') AND (DIVI_PARA.FY_PART = '" + fyPartDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" UNION ALL ");
        sbQuery.Append("SELECT 'CIP:' AS CIP_TYPE, 'HO' AS BRANCH, SUM(DIVIDEND.BALANCE) AS NO_UNITS, SUM(DIVIDEND.TOT_DIVI) AS TOTAL_DIVIDEND, ");
        sbQuery.Append(" NVL(SUM(DIVIDEND.CIP_QTY), 0) AS TOTAL_CIP, SUM(DIVIDEND.DIDUCT) AS TAX_DIDUCT, SUM(DIVIDEND.FI_DIVI_QTY) AS NET_DIVDEND ");
        sbQuery.Append(" FROM  DIVI_PARA INNER JOIN   DIVIDEND ON DIVI_PARA.FUND_CD = DIVIDEND.FUND_CD AND DIVI_PARA.CLOSE_DT = DIVIDEND.CLOSE_DT AND ");
        sbQuery.Append(" DIVI_PARA.F_YEAR = DIVIDEND.FY AND DIVI_PARA.DIVI_NO = DIVIDEND.DIVI_NO WHERE  (DIVIDEND.CIP = 'Y') AND (DIVIDEND.REG_BR = 'AMC/01')");
        sbQuery.Append(" AND (DIVI_PARA.FUND_CD = '" + fundNameDropDownList.SelectedValue.ToString().ToUpper() + "')AND (DIVI_PARA.F_YEAR = '" + DividendFYDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" AND (DIVI_PARA.CLOSE_DT = '" + ClosingDateDropDownList.SelectedItem.ToString().ToUpper() + "') AND (DIVI_PARA.FY_PART = '" + fyPartDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" UNION ALL ");
        sbQuery.Append("SELECT 'CIP:' AS CIP_TYPE, 'BRANCH' AS BRANCH, SUM(DIVIDEND.BALANCE) AS NO_UNITS, SUM(DIVIDEND.TOT_DIVI) AS TOTAL_DIVIDEND, ");
        sbQuery.Append(" NVL(SUM(DIVIDEND.CIP_QTY), 0) AS TOTAL_CIP, SUM(DIVIDEND.DIDUCT) AS TAX_DIDUCT, SUM(DIVIDEND.FI_DIVI_QTY) AS NET_DIVDEND ");
        sbQuery.Append(" FROM  DIVI_PARA INNER JOIN   DIVIDEND ON DIVI_PARA.FUND_CD = DIVIDEND.FUND_CD AND DIVI_PARA.CLOSE_DT = DIVIDEND.CLOSE_DT AND ");
        sbQuery.Append(" DIVI_PARA.F_YEAR = DIVIDEND.FY AND DIVI_PARA.DIVI_NO = DIVIDEND.DIVI_NO WHERE  (DIVIDEND.CIP = 'Y') AND (DIVIDEND.REG_BR <> 'AMC/01')");
        sbQuery.Append(" AND (DIVI_PARA.FUND_CD = '" + fundNameDropDownList.SelectedValue.ToString().ToUpper() + "')AND (DIVI_PARA.F_YEAR = '" + DividendFYDropDownList.SelectedItem.Text.ToString() + "')");
        sbQuery.Append(" AND (DIVI_PARA.CLOSE_DT = '" + ClosingDateDropDownList.SelectedItem.ToString().ToUpper() + "') AND (DIVI_PARA.FY_PART = '" + fyPartDropDownList.SelectedItem.Text.ToString() + "')");

        DataTable dtDividendInformation = commonGatewayObj.Select(sbQuery.ToString());
        DataTable dtDiviParaInformation = commonGatewayObj.Select("SELECT * FROM DIVI_PARA WHERE (FUND_CD = '" + fundNameDropDownList.SelectedValue.ToString().ToUpper() + "')AND (F_YEAR = '" + DividendFYDropDownList.SelectedItem.Text.ToString() + "')AND (CLOSE_DT = '" + ClosingDateDropDownList.SelectedItem.ToString().ToUpper() + "') AND (FY_PART = '" + fyPartDropDownList.SelectedItem.Text.ToString() + "') ");

        if ((dtDividendInformation.Rows.Count > 0) && (dtDiviParaInformation.Rows.Count > 0))
        {
            
            Session["dtDividendInformation"] = dtDividendInformation;
            Session["dtDiviParaInformation"] = dtDiviParaInformation;
            Session["fundName"] = fundNameDropDownList.SelectedItem.Text.ToString().ToUpper();
          
            ClientScript.RegisterStartupScript(this.GetType(), "UnitReportTaxCert", "window.open('ReportViewer/UnitReportTaxStatementAccReportViewer.aspx')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "alert('No Data Found');", true);
        }

    }
    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DividendFYDropDownList.DataSource = diviDAOObj.dtGetFundWiseFY(fundNameDropDownList.SelectedValue.ToString().ToUpper());
        DividendFYDropDownList.DataTextField = "F_YEAR";
        DividendFYDropDownList.DataValueField = "F_YEAR";
        DividendFYDropDownList.DataBind();
    }
    protected void DividendFYDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClosingDateDropDownList.DataSource = diviDAOObj.dtGetFYWiseClosinDate(DividendFYDropDownList.SelectedItem.Text.ToString(), fundNameDropDownList.SelectedValue.ToString().ToUpper());
        ClosingDateDropDownList.DataTextField = "CLOSE_DT";
        ClosingDateDropDownList.DataValueField = "DIVI_NO";
        ClosingDateDropDownList.DataBind();

        fyPartDropDownList.DataSource = reportObj.getDtFYPart(fundNameDropDownList.SelectedValue.ToString().ToUpper());
        fyPartDropDownList.DataTextField = "FY_PART";
        fyPartDropDownList.DataValueField = "FY_PART";
        fyPartDropDownList.DataBind();     
    }
}

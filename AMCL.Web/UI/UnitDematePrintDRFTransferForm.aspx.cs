﻿using System;
using System.Data;
using System.Web.UI;
using System.Text;
using System.IO;
using AMCL.DL;
using AMCL.BL;
using AMCL.UTILITY;
using AMCL.GATEWAY;
using AMCL.COMMON;

public partial class UI_UnitDematePrintDRFTransferForm : System.Web.UI.Page
{
    System.Web.UI.Page this_page_ref = null;
    CommonGateway commonGatewayObj = new CommonGateway();
    OMFDAO opendMFDAO = new OMFDAO();    
    UnitUser userObj = new UnitUser();

    UnitSaleBL unitSaleBLObj = new UnitSaleBL();
    UnitHolderRegistration regObj = new UnitHolderRegistration();
    BaseClass bcContent = new BaseClass();
    string fundCode = "";
    string branchCode = "";
    string printType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
       
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

            DRRefNoDropDownList.DataSource = unitSaleBLObj.dtDRRefNoListForDRN("AND SALE.REG_BK='" + fundCode + "'");
            DRRefNoDropDownList.DataTextField = "NAME";
            DRRefNoDropDownList.DataValueField = "ID";
            DRRefNoDropDownList.DataBind();


        }
    
    }

     
    protected void DRRefNoDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtSaleInfoForDRF = unitSaleBLObj.dtGetSaleInfoForDemate(" AND SALE.REG_BK='" + fundCode + "' AND SALE.REG_BR='" + branchCode + "' AND SALE.DRF_REF_NO=" + DRRefNoDropDownList.SelectedValue.ToString());
        long totalUnitsQty = unitSaleBLObj.getTotalUnitsForDRF(" AND SALE.REG_BK='" + fundCode + "' AND SALE.REG_BR='" + branchCode + "' AND SALE.DRF_REF_NO=" + DRRefNoDropDownList.SelectedValue.ToString());

        if (dtSaleInfoForDRF.Rows.Count > 0)
        {
            SaleListGridView.DataSource = dtSaleInfoForDRF;
            SaleListGridView.DataBind();
            FolioNoLabel.Text = dtSaleInfoForDRF.Rows[0]["DRF_REG_FOLIO_NO"].ToString();
            CertNoLabel.Text = dtSaleInfoForDRF.Rows[0]["DRF_CERT_NO"].ToString();
            DistinctNoToLabel.Text = dtSaleInfoForDRF.Rows[0]["DRF_DISTNCT_NO_TO"].ToString();
            DistNoFromLabel.Text = dtSaleInfoForDRF.Rows[0]["DRF_DISTNCT_NO_FROM"].ToString();
            TotalUnitLabel.Text = totalUnitsQty.ToString();
        }
        else
        {
            SaleListGridView.DataSource = dtSaleInfoForDRF;
            SaleListGridView.DataBind();           
            FolioNoLabel.Text = "";
            CertNoLabel.Text = "";
            DistinctNoToLabel.Text = "";
            DistNoFromLabel.Text = "";
            TotalUnitLabel.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "alert ('No Data Found');", true);

        }
    }

    protected void PrintDRFButton_Click(object sender, EventArgs e)
    {
        try
        {
            printType = "DRF";
            StringBuilder sbMaster = new StringBuilder();            
            DataTable dtReportStatement = new DataTable();

            sbMaster.Append("SELECT A.FUND_CD, UPPER(A.FUND_NM) AS FUND_NM,A.SALE_BO_NAME,A.SALE_OF_UNIT_BO,A.ISIN_NO, B.* FROM FUND_INFO  A INNER JOIN (");
            sbMaster.Append(" SELECT REG_BK, 'SALE DRF Ref. No: '||DRF_REF_NO AS DRF_REF_NO ,DRF_REG_FOLIO_NO,DRF_CERT_NO,DRF_DISTNCT_NO_FROM,DRF_DISTNCT_NO_TO,SUM(QTY) QTY FROM SALE");
            sbMaster.Append(" WHERE REG_BK='"+ fundCode + "' AND DRF_REF_NO="+ DRRefNoDropDownList.SelectedValue.ToString() + " GROUP BY REG_BK, DRF_REF_NO,DRF_REG_FOLIO_NO,DRF_CERT_NO,DRF_DISTNCT_NO_FROM,DRF_DISTNCT_NO_TO) B ON A.FUND_CD=B.REG_BK");

            dtReportStatement = commonGatewayObj.Select(sbMaster.ToString());


            if (dtReportStatement.Rows.Count > 0)
            {
                Session["dtReportStatement"] = dtReportStatement;
                Session["printType"] = printType;

                ClientScript.RegisterStartupScript(this.GetType(), "UnitHolderInfo", "window.open('ReportViewer/UnitDematePrintDRFTransfer.aspx')", true);

            }
            else
            {
                Session["dtReportStatement"] = null;
                Session["printType"] = null;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('No data found');", true);
            }
        }
        catch (Exception ex)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('" + ex.Message.Replace("'", "").ToString() + "');", true);
        }

    }

    protected void PrintTransferButton_Click(object sender, EventArgs e)
    {
        try
        {
            printType = "SL_TR";
            StringBuilder sbMaster = new StringBuilder();
            DataTable dtReportStatement = new DataTable();

            sbMaster.Append("SELECT A.FUND_CD, C.SL_NO, A.FUND_NM,A.ISIN_NO,UPPER(D.DP_NAME) AS CUST_DP_NAME, D.DP_ID CUST_DP_ID,A.SALE_BO_NAME,A.SALE_OF_UNIT_BO,C.QTY,UPPER(E.DP_NAME) AS HOLDER_DP_NAME,C.HOLDER_DP_ID,B.HNAME,C.HOLDER_BO,'SALE DRF Ref. No: '||C.DRF_REF_NO AS DRF_REF_NO");
            sbMaster.Append(" FROM  (SELECT FUND_CD, UPPER(FUND_INFO.FUND_NM) AS FUND_NM ,  ISIN_NO,SALE_BO_NAME,SALE_OF_UNIT_BO, CUST_DP_ID FROM FUND_INFO ) A ");
            sbMaster.Append(" INNER JOIN ( SELECT DP_ID,DP_NAME FROM AMCL_DIVIDEND.CDBL_DP_LIST) D ON A.CUST_DP_ID=D.DP_ID  INNER JOIN ( SELECT REG_BK,REG_BR,REG_NO,HNAME FROM U_MASTER) B ");
            sbMaster.Append(" ON B.REG_BK=A.FUND_CD  INNER JOIN ( SELECT REG_BK,REG_BR,REG_NO,SL_NO,HOLDER_BO,SUBSTR(HOLDER_BO, 4, 5) AS HOLDER_DP_ID,QTY,DRF_REF_NO FROM SALE) C ");
            sbMaster.Append(" ON B.REG_BK=C.REG_BK AND B.REG_BR=C.REG_BR AND B.REG_NO=C.REG_NO INNER JOIN (  SELECT DP_ID,DP_NAME FROM AMCL_DIVIDEND.CDBL_DP_LIST) E  ON C.HOLDER_DP_ID=E.DP_ID ");
            sbMaster.Append(" WHERE A.FUND_CD='" + fundCode + "' AND  C.DRF_REF_NO=" + DRRefNoDropDownList.SelectedValue.ToString() + " ORDER BY C.SL_NO");

            dtReportStatement = commonGatewayObj.Select(sbMaster.ToString());


            if (dtReportStatement.Rows.Count > 0)
            {
                Session["dtReportStatement"] = dtReportStatement;
                Session["printType"] = printType;

                ClientScript.RegisterStartupScript(this.GetType(), "UnitHolderInfo", "window.open('ReportViewer/UnitDematePrintDRFTransfer.aspx')", true);

            }
            else
            {
                Session["dtReportStatement"] = null;
                Session["printType"] = null;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('No data found');", true);
            }
        }
        catch (Exception ex)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('" + ex.Message.Replace("'", "").ToString() + "');", true);
        }
    }
}

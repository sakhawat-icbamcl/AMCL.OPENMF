﻿using System;
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
using AMCL.DL;
using AMCL.BL;
using AMCL.UTILITY;
using AMCL.GATEWAY;
using AMCL.COMMON;

public partial class UI_UnitUserCreate : System.Web.UI.Page
{
    OMFDAO opendMFDAO = new OMFDAO();
    EncryptDecrypt encrypt = new EncryptDecrypt();
    UnitUserBL userBLObj = new UnitUserBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {           
            Response.Redirect("../Default.aspx");
        }
        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = opendMFDAO.dtFundList();
            fundNameDropDownList.DataTextField = "FUND_NM";
            fundNameDropDownList.DataValueField = "FUND_CD";
            fundNameDropDownList.DataBind();

            branchNameDropDownList.DataSource = opendMFDAO.dtBranchList();
            branchNameDropDownList.DataTextField = "BR_NM";
            branchNameDropDownList.DataValueField = "BR_CD";
            branchNameDropDownList.DataBind();

            menuCheckBoxList.DataSource = opendMFDAO.getDtMenuList();
            menuCheckBoxList.DataTextField = "M_NAME";
            menuCheckBoxList.DataValueField = "M_ID";
            menuCheckBoxList.DataBind();

            fundCheckBoxList.DataSource = opendMFDAO.getFundForMenu();
            fundCheckBoxList.DataTextField = "FUND_NM";
            fundCheckBoxList.DataValueField = "FUND_CD";
            fundCheckBoxList.DataBind();

            branchCheckBoxList.DataSource = opendMFDAO.getBranchForMenu();
            branchCheckBoxList.DataTextField = "BR_NM";
            branchCheckBoxList.DataValueField = "BR_CD";
            branchCheckBoxList.DataBind();

        }
    
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtUserMenu = new DataTable();
            dtUserMenu.Columns.Add("M_ID", typeof(string));
            DataRow drUserMenu = dtUserMenu.NewRow();

            DataTable dtUserFund = new DataTable();
            dtUserFund.Columns.Add("FUND_CD", typeof(string));
            DataRow drUserFund = dtUserFund.NewRow();

            DataTable dtUserBranch = new DataTable();
            dtUserBranch.Columns.Add("BR_CD", typeof(string));
            DataRow drUserBranch = dtUserBranch.NewRow();

            UnitUser userObj = new UnitUser();
            UnitHolderRegistration regObj = new UnitHolderRegistration();

            bool branchFlag = false;
            bool fundFlag = false;
            userObj.UserID = UserIDTextBox.Text.Trim().ToString();
            userObj.UserPassword = encrypt.Encrypt(passwordTextBox.Text.Trim().ToString());
            userObj.UserName = userNameTextBox.Text.Trim().ToString().ToUpper();
            regObj.FundCode = fundNameDropDownList.SelectedValue.ToString();
            regObj.BranchCode = branchNameDropDownList.SelectedValue.ToString();

            if (menuCheckBoxList.Items.Count > 0)
            {
                for (int loop = 0; loop < menuCheckBoxList.Items.Count; loop++)
                {
                    if(menuCheckBoxList.Items[loop].Selected)
                    {
                        drUserMenu = dtUserMenu.NewRow();
                        drUserMenu["M_ID"] = menuCheckBoxList.Items[loop].Value.ToString();
                        dtUserMenu.Rows.Add(drUserMenu);
                    }
                }
            }
            if (branchCheckBoxList.Items.Count > 0)
            {
                

                for (int loop1 = 0; loop1 < branchCheckBoxList.Items.Count; loop1++)
                {
                    if (branchCheckBoxList.Items[loop1].Selected)
                    {
                        drUserBranch = dtUserBranch.NewRow();
                        drUserBranch["BR_CD"] = branchCheckBoxList.Items[loop1].Value.ToString();
                        dtUserBranch.Rows.Add(drUserBranch);
                        branchFlag = true;
                    }
                }
                if (!branchFlag)
                {
                    drUserBranch = dtUserBranch.NewRow();
                    drUserBranch["BR_CD"] = branchNameDropDownList.SelectedValue.ToString().ToUpper();
                    dtUserBranch.Rows.Add(drUserBranch);
                }
            }
            else
            {
                drUserBranch = dtUserBranch.NewRow();
                drUserBranch["BR_CD"] = branchNameDropDownList.SelectedValue.ToString().ToUpper();
                dtUserBranch.Rows.Add(drUserBranch);
            }
            if (fundCheckBoxList.Items.Count > 0)
            {

                for (int loop2 = 0; loop2 < fundCheckBoxList.Items.Count; loop2++)
                {
                    if (fundCheckBoxList.Items[loop2].Selected)
                    {
                        drUserFund = dtUserFund.NewRow();
                        drUserFund["FUND_CD"] = fundCheckBoxList.Items[loop2].Value.ToString();
                        dtUserFund.Rows.Add(drUserFund);
                        fundFlag = true;
                    }
                }
                if (!fundFlag)
                {
                    drUserFund = dtUserFund.NewRow();
                    drUserFund["FUND_CD"] = fundNameDropDownList.SelectedValue.ToString().ToUpper();
                    dtUserFund.Rows.Add(drUserFund);
                }
            }
            else
            {
                drUserFund = dtUserFund.NewRow();
                drUserFund["FUND_CD"] = fundNameDropDownList.SelectedValue.ToString().ToUpper();
                dtUserFund.Rows.Add(drUserFund);
            }

            if (dtUserMenu.Rows.Count > 0)
            {
                if (dtUserBranch.Rows.Count > 0)
                {
                    if (dtUserFund.Rows.Count > 0)
                    {
                        if (userBLObj.CheckDuplicate(regObj, userObj))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('User Create Failed: Duplicate User ID');", true);
                        }
                        else
                        {
                            userBLObj.saveUserMenue(regObj, userObj, dtUserBranch, dtUserFund, dtUserMenu);
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "window.fnReset();alert ('User Created Successfully');", true);
                            // ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('User Created Successfully');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('User Create Failed: Please Checke Atleast One Fund ');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('User Create Failed: Please Checke Atleast One Branch ');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('User Create Failed: Please Checke Atleast One Menu ');", true);
            }
        }
        catch(Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert ('User Create Failed:" + ex.Message.Replace("'", "").ToString() + "');", true);

        }
   

    }
    protected void regCloseButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("UnitHome.aspx");
    }
}

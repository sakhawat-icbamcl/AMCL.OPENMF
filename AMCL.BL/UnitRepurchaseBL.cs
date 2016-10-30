using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.OracleClient;
using System.Text;
using System.Collections;
using AMCL.COMMON;
using AMCL.DL;
using AMCL.GATEWAY;

/// <summary>
/// Summary description for UnitTransferBL
/// </summary>
namespace AMCL.BL
{
    public class UnitRepurchaseBL
    {
        CommonGateway commonGatewayObj = new CommonGateway();
        OMFDAO opendMFDAO = new OMFDAO();
        //UnitSaleBL unitSaleBLObj = new UnitSaleBL();
        string certificate = "";
        public UnitRepurchaseBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public bool IsDuplicateRepurchase(UnitHolderRegistration regObj, UnitRepurchase repObj)
        {
            DataTable dtRepurchase = new DataTable();         
            dtRepurchase = commonGatewayObj.Select("SELECT * FROM REPURCHASE WHERE  REG_BK='" + regObj.FundCode.ToString().ToUpper() + "'AND REG_BR='" + regObj.BranchCode.ToString() + "' AND REP_NO=" + Convert.ToInt32(repObj.RepurchaseNo.ToString()));            
            if (dtRepurchase.Rows.Count > 0)
                return true;
            else
                return false;

        }
        public int getNextRepurchaseNo(UnitHolderRegistration regObj, UnitUser unitUserObj)
        {
            int nextRepurchaseNo = 0;
            DataTable dtNextRepurchaseNo = new DataTable();

            dtNextRepurchaseNo = commonGatewayObj.Select("SELECT MAX(REP_NO) AS REP_NO FROM REPURCHASE WHERE  REG_BK='" + regObj.FundCode.ToString().ToUpper() + "'AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND USER_NM='" + unitUserObj.UserID.ToString() + "'");

            if (!dtNextRepurchaseNo.Rows[0]["REP_NO"].Equals(DBNull.Value))
            {
                nextRepurchaseNo = Convert.ToInt16(dtNextRepurchaseNo.Rows[0]["REP_NO"].ToString());
            }
            else
            {
                dtNextRepurchaseNo = new DataTable();
                dtNextRepurchaseNo = commonGatewayObj.Select("SELECT MAX(REP_NO) AS REP_NO FROM REPURCHASE WHERE  REG_BK='" + regObj.FundCode.ToString().ToUpper() + "'AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "'");
                nextRepurchaseNo = dtNextRepurchaseNo.Rows[0]["REP_NO"].Equals(DBNull.Value) ? 0 : Convert.ToInt16(dtNextRepurchaseNo.Rows[0]["REP_NO"].ToString());
            }
            return nextRepurchaseNo + 1;

        }
        public int getMaxRepurchaseNo(UnitHolderRegistration regObj)
        {
            int maxRepNo = 0;
            DataTable dtMaxRepNo = commonGatewayObj.Select("SELECT MAX(REP_NO) AS MAX_REP_NO FROM REPURCHASE WHERE REG_BK ='" + regObj.FundCode.ToString().ToUpper() + "'AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "'");
            if (dtMaxRepNo.Rows.Count > 0)
            {
                maxRepNo = Convert.ToInt32(dtMaxRepNo.Rows[0]["MAX_REP_NO"].Equals(DBNull.Value) ? "0" : dtMaxRepNo.Rows[0]["MAX_REP_NO"].ToString());
            }
            return maxRepNo + 1;

        }
        public void saveRepurchase(DataTable dtTransferSaleCert, UnitHolderRegistration regObj, UnitRepurchase repObj, UnitUser unitUserObj)
        {
            try
            {
                UnitTransferBL unitTransferBLObj = new UnitTransferBL();
                commonGatewayObj.BeginTransaction();
                Hashtable htRepurchaseRegInfo = new Hashtable();
                Hashtable htCertNoTemp = new Hashtable();
                int SL_TR_RN = 0;
                int CertNo = 0;
                string certType = "";
                string[] saleNoArray;
                string[] certArray;
                string statusFlag = "R";
                string Old_SL_TR_RN = "";

                for (int loop = 0; loop < dtTransferSaleCert.Rows.Count; loop++)
                {


                    if (dtTransferSaleCert.Rows[loop]["SL_NO"].ToString().ToUpper().IndexOf('S') >= 0)
                    {
                        htRepurchaseRegInfo = new Hashtable();
                        htRepurchaseRegInfo.Add("CURR_REG_BK", regObj.FundCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("CURR_REG_BR", regObj.BranchCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("CURR_REG_NO", regObj.RegNumber.ToString());
                        htRepurchaseRegInfo.Add("STATUS_FLAG", statusFlag.ToUpper().ToString());

                        saleNoArray = dtTransferSaleCert.Rows[loop]["SL_NO"].ToString().Split('S');
                        SL_TR_RN = Convert.ToInt32(saleNoArray[1].ToString());
                        certArray = dtTransferSaleCert.Rows[loop]["CERTIFICATE"].ToString().Split('-');
                        CertNo = Convert.ToInt32(certArray[1].ToString());
                        certType = certArray[0].ToString().ToUpper();
                        commonGatewayObj.Update(htRepurchaseRegInfo, "SALE_CERT", "REG_NO=" + Convert.ToInt32(regObj.RegNumber) + " AND REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND  SL_NO=" + SL_TR_RN + " AND CERT_NO=" + CertNo + " AND CERT_TYPE='" + certType + "'");

                        Old_SL_TR_RN = "S" + SL_TR_RN.ToString();
                    }
                    else if (dtTransferSaleCert.Rows[loop]["SL_NO"].ToString().ToUpper().IndexOf('T') >= 0)
                    {
                        htRepurchaseRegInfo = new Hashtable();
                        htRepurchaseRegInfo.Add("CURR_REG_BK", regObj.FundCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("CURR_REG_BR", regObj.BranchCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("CURR_REG_NO", regObj.RegNumber.ToString());
                        htRepurchaseRegInfo.Add("STATUS_FLAG", statusFlag.ToUpper().ToString());


                        saleNoArray = dtTransferSaleCert.Rows[loop]["SL_NO"].ToString().Split('T');
                        SL_TR_RN = Convert.ToInt32(saleNoArray[1].ToString());
                        certArray = dtTransferSaleCert.Rows[loop]["CERTIFICATE"].ToString().Split('-');
                        CertNo = Convert.ToInt32(certArray[1].ToString());
                        certType = certArray[0].ToString().ToUpper();


                        commonGatewayObj.Update(htRepurchaseRegInfo, "TRANS_CERT", "F_CD='" + regObj.FundCode.ToString().ToUpper() + "'  AND CERT_NO=" + CertNo + " AND CERT_TYPE='" + certType + "'");
                        commonGatewayObj.Update(htRepurchaseRegInfo, "SALE_CERT", "REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND CERT_NO=" + CertNo + " AND CERT_TYPE='" + certType + "'");
                        commonGatewayObj.Update(htRepurchaseRegInfo, "RENEWAL_OUT", "REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND CERT_NO=" + CertNo + " AND CERT_TYPE='" + certType + "'");

                        Old_SL_TR_RN = "T" + SL_TR_RN.ToString();

                    }
                    else if (dtTransferSaleCert.Rows[loop]["SL_NO"].ToString().ToUpper().IndexOf('R') >= 0)
                    {
                        htRepurchaseRegInfo = new Hashtable();
                        htRepurchaseRegInfo.Add("CURR_REG_BK", regObj.FundCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("CURR_REG_BR", regObj.BranchCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("CURR_REG_NO", regObj.RegNumber.ToString());
                        htRepurchaseRegInfo.Add("STATUS_FLAG", statusFlag.ToUpper().ToString());


                        saleNoArray = dtTransferSaleCert.Rows[loop]["SL_NO"].ToString().Split('R');
                        SL_TR_RN = Convert.ToInt32(saleNoArray[1].ToString());
                        certArray = dtTransferSaleCert.Rows[loop]["CERTIFICATE"].ToString().Split('-');
                        CertNo = Convert.ToInt32(certArray[1].ToString());
                        certType = certArray[0].ToString().ToUpper();

                        commonGatewayObj.Update(htRepurchaseRegInfo, "RENEWAL_OUT", "REG_NO=" + Convert.ToInt32(regObj.RegNumber) + " AND REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND CERT_NO=" + CertNo + " AND CERT_TYPE='" + certType + "'");

                        Old_SL_TR_RN = "R" + SL_TR_RN.ToString();
                    }


                  

                    htRepurchaseRegInfo = new Hashtable();
                    htRepurchaseRegInfo.Add("REG_BK", regObj.FundCode.ToString().ToUpper());
                    htRepurchaseRegInfo.Add("REG_BR", regObj.BranchCode.ToString().ToUpper());
                    htRepurchaseRegInfo.Add("REP_NO", Convert.ToInt32(repObj.RepurchaseNo.ToString()));
                    htRepurchaseRegInfo.Add("REG_NO", Convert.ToInt32(regObj.RegNumber.ToString()));

                    certArray = dtTransferSaleCert.Rows[loop]["CERTIFICATE"].ToString().Split('-');

                    htRepurchaseRegInfo.Add("CERT_TYPE", certArray[0].ToString().ToUpper());
                    htRepurchaseRegInfo.Add("CERT_NO", Convert.ToInt32(certArray[1].ToString()));
                    htRepurchaseRegInfo.Add("CERTIFICATE", dtTransferSaleCert.Rows[loop]["CERTIFICATE"].ToString().ToUpper());
                    htRepurchaseRegInfo.Add("QTY", Convert.ToInt32(dtTransferSaleCert.Rows[loop]["QTY"].ToString()));
                    htRepurchaseRegInfo.Add("SL_TR_NO", Old_SL_TR_RN.ToString().ToUpper());

                    commonGatewayObj.Insert(htRepurchaseRegInfo, "REP_CERT_NO");
                }

               



                DataTable dtTransfer = unitTransferBLObj.dtTrnasfer(dtTransferSaleCert);
                for (int i = 0; i < dtTransfer.Rows.Count; i++)
                {                   

                    htRepurchaseRegInfo = new Hashtable();
                    htRepurchaseRegInfo.Add("REP_NO", Convert.ToInt32(repObj.RepurchaseNo.ToString()));
                    htRepurchaseRegInfo.Add("REP_DT", Convert.ToDateTime(repObj.RepurchaseDate.ToString()).ToString("dd-MMM-yyyy"));
                    htRepurchaseRegInfo.Add("SL_TR_NO", dtTransfer.Rows[i]["SL_NO"].ToString());
                    htRepurchaseRegInfo.Add("REG_BK", regObj.FundCode.ToString().ToUpper());
                    htRepurchaseRegInfo.Add("REG_BR", regObj.BranchCode.ToString().ToUpper());
                    htRepurchaseRegInfo.Add("REG_NO", regObj.RegNumber.ToString());
                    htRepurchaseRegInfo.Add("REP_PRICE", decimal.Parse(repObj.RepurchaseRate.ToString()));
                    htRepurchaseRegInfo.Add("QTY", Convert.ToInt32(dtTransfer.Rows[i]["QTY"].ToString()));

                    htRepurchaseRegInfo.Add("USER_NM", unitUserObj.UserID.ToString());
                    htRepurchaseRegInfo.Add("ENT_DT", DateTime.Now.ToString());
                    htRepurchaseRegInfo.Add("ENT_TM", DateTime.Now.ToShortTimeString().ToString());

                    if (repObj.ChequeIssueTo== null)
                    {
                        htRepurchaseRegInfo.Add("CHQ_ISSUE_TO", DBNull.Value);
                    }
                    else
                    {
                        htRepurchaseRegInfo.Add("CHQ_ISSUE_TO", repObj.ChequeIssueTo.ToString());
                    }

                    commonGatewayObj.Insert(htRepurchaseRegInfo, "REPURCHASE");
                }
                commonGatewayObj.CommitTransaction();

            }
            catch (Exception ex)
            {
                commonGatewayObj.RollbackTransaction();
                throw ex;
            }
        }
        public void saveRepurchaseCDS(DataTable dtSaleTrInfo, UnitHolderRegistration regObj, UnitRepurchase repObj, UnitUser unitUserObj)
        {
            try
            {
               
                commonGatewayObj.BeginTransaction();
                Hashtable htRepurchaseRegInfo = new Hashtable();
                
                StringBuilder sbQuery = new StringBuilder();
                string[] saleNoArray;
                for (int loop = 0; loop < dtSaleTrInfo.Rows.Count; loop++)
                {
                    if (dtSaleTrInfo.Rows[loop]["SL_TR_NO"].ToString().ToUpper().IndexOf('S') >= 0)
                    {
                       
                      
                        saleNoArray = dtSaleTrInfo.Rows[loop]["SL_TR_NO"].ToString().Split('S');
                        sbQuery.Append("UPDATE SALE SET QTY_OUT=NVL(QTY_OUT,0)+" + Convert.ToInt64(dtSaleTrInfo.Rows[loop]["SURRENDER_UNITS"].ToString()));
                        sbQuery.Append(" WHERE REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "'");
                        sbQuery.Append(" AND REG_NO=" + Convert.ToInt32(regObj.RegNumber.ToString()) + " AND SL_NO=" + Convert.ToInt32(saleNoArray[1].ToString()));

                        commonGatewayObj.ExecuteNonQuery(sbQuery.ToString());

                        htRepurchaseRegInfo = new Hashtable();
                        htRepurchaseRegInfo.Add("REP_NO",repObj.RepurchaseNo);
                        htRepurchaseRegInfo.Add("REP_DT", repObj.RepurchaseDate);
                        htRepurchaseRegInfo.Add("SL_TR_NO", dtSaleTrInfo.Rows[loop]["SL_TR_NO"]);
                        htRepurchaseRegInfo.Add("REG_BK",  regObj.FundCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("REG_BR",regObj.BranchCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("REG_NO", regObj.RegNumber.ToString());
                        htRepurchaseRegInfo.Add("REP_PRICE", repObj.RepurchaseRate);
                        htRepurchaseRegInfo.Add("QTY", dtSaleTrInfo.Rows[loop]["SURRENDER_UNITS"]);
                        htRepurchaseRegInfo.Add("USER_NM", unitUserObj.UserID.ToString());
                        htRepurchaseRegInfo.Add("ENT_DT", DateTime.Now.ToString());
                        htRepurchaseRegInfo.Add("ENT_TM", DateTime.Now.ToShortTimeString().ToString());

                        commonGatewayObj.Insert(htRepurchaseRegInfo, "REPURCHASE");
                    }                   
                    else if (dtSaleTrInfo.Rows[loop]["SL_TR_NO"].ToString().ToUpper().IndexOf('T') >= 0)
                    {


                        saleNoArray = dtSaleTrInfo.Rows[loop]["SL_TR_NO"].ToString().Split('T');
                        sbQuery.Append("UPDATE TRANSFER SET QTY_OUT=NVL(QTY_OUT,0)+" + Convert.ToInt64(dtSaleTrInfo.Rows[loop]["SURRENDER_UNITS"].ToString()));
                        sbQuery.Append(" WHERE F_CD='" + regObj.FundCode.ToString().ToUpper() + "' AND BR_CODE='" + regObj.FundCode.ToString() + "_" + regObj.BranchCode.ToString().ToUpper() + "'");
                        sbQuery.Append(" AND REG_NO_I=" + Convert.ToInt32(regObj.RegNumber.ToString()) + "AND TR_NO=" + Convert.ToInt32(saleNoArray[1].ToString()));

                        commonGatewayObj.ExecuteNonQuery(sbQuery.ToString());

                        htRepurchaseRegInfo = new Hashtable();
                        htRepurchaseRegInfo.Add("REP_NO", repObj.RepurchaseNo);
                        htRepurchaseRegInfo.Add("REP_DT", repObj.RepurchaseDate);
                        htRepurchaseRegInfo.Add("SL_TR_NO", dtSaleTrInfo.Rows[loop]["SL_TR_NO"]);
                        htRepurchaseRegInfo.Add("REG_BK", regObj.FundCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("REG_BR", regObj.BranchCode.ToString().ToUpper());
                        htRepurchaseRegInfo.Add("REG_NO", regObj.RegNumber.ToString());
                        htRepurchaseRegInfo.Add("REP_PRICE", repObj.RepurchaseRate);
                        htRepurchaseRegInfo.Add("QTY", dtSaleTrInfo.Rows[loop]["SURRENDER_UNITS"]);
                        htRepurchaseRegInfo.Add("USER_NM", unitUserObj.UserID.ToString());
                        htRepurchaseRegInfo.Add("ENT_DT", DateTime.Now.ToString());
                        htRepurchaseRegInfo.Add("ENT_TM", DateTime.Now.ToShortTimeString().ToString());

                        commonGatewayObj.Insert(htRepurchaseRegInfo, "REPURCHASE");
                    }
                }
                                                  
                   

                  
                  
                
                commonGatewayObj.CommitTransaction();

            }
            catch (Exception ex)
            {
                commonGatewayObj.RollbackTransaction();
                throw ex;
            }
        }
        public DateTime getLastRepDate(UnitHolderRegistration regObj, UnitRepurchase repObj)
        {
            DateTime lastSaleDate;
            DataTable dtLastSaleDate = new DataTable();
            dtLastSaleDate = commonGatewayObj.Select("SELECT MAX(REP_DT) AS MAX_REP_DATE FROM REPURCHASE WHERE REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "'AND REP_NO=" + repObj.RepurchaseNo);
            if (dtLastSaleDate.Rows.Count > 0)
            {
                lastSaleDate = Convert.ToDateTime(dtLastSaleDate.Rows[0]["MAX_REP_DATE"].Equals(DBNull.Value) ? DateTime.Today.ToString() : dtLastSaleDate.Rows[0]["MAX_REP_DATE"].ToString());
            }
            else
            {
                lastSaleDate = DateTime.Today;
            }
            return lastSaleDate;

        }
        public decimal getLastRepRate(UnitHolderRegistration regObj, UnitRepurchase repObj)
        {
            decimal lastRepRate = 0;
            DataTable dtLastSaleRate = new DataTable();
            dtLastSaleRate = commonGatewayObj.Select("SELECT DISTINCT REP_PRICE AS MAX_REP_PRICE FROM REPURCHASE WHERE REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND REP_DT IN (SELECT MAX(REP_DT) AS MAX_REP_DATE FROM REPURCHASE WHERE REG_BK='" + regObj.FundCode.ToString().ToUpper() + "' AND REG_BR='" + regObj.BranchCode.ToString().ToUpper() + "' AND REP_NO =" + repObj.RepurchaseNo + ")");
            if (dtLastSaleRate.Rows.Count > 0)
            {
                lastRepRate = Convert.ToDecimal(dtLastSaleRate.Rows[0]["MAX_REP_PRICE"].ToString());
            }
            return lastRepRate;

        }
        public bool IsRepurchaseLock(UnitHolderRegistration regObj)
        {
            bool lockStatus = false;
            DataTable dtLockStatus = commonGatewayObj.Select("SELECT NVL(ALL_LOCK,'N') AS ALL_LOCK, NVL(REP_LOCK,'N') AS REP_LOCK FROM U_MASTER WHERE REG_BK='" + regObj.FundCode.ToString().ToUpper() + "'AND REG_BR='" + regObj.BranchCode.ToString() + "' AND REG_NO=" + regObj.RegNumber);
            if (dtLockStatus.Rows.Count > 0)
            {
                if (dtLockStatus.Rows[0]["ALL_LOCK"].ToString() == "Y" || dtLockStatus.Rows[0]["REP_LOCK"].ToString() == "Y")
                {
                    lockStatus = true;
                }
            }
            return lockStatus;
        }


    }
}

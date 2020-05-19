using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsLogManager
/// </summary>
public class clsLogManager
{
    public static void InsertLog(clsLog objLog)
    {
        try
        {
            String strSQL = "INSERT INTO tbl_Log VALUES(TBL_LOG_SEQ.NEXTVAL, '" + objLog.strIP + "', SYSDATE, '" + objLog.strAction + "', '" + objLog.strTarget_Name + "')";

            clsOracleDBMS objDBMS = new clsOracleDBMS();

            objDBMS.ExecuteSQL(strSQL);
        }
        catch (Exception ex)
        {
            throw new Exception(clsUtilities.GetErr(ex.Message.ToString()));
        }
    }
}


    
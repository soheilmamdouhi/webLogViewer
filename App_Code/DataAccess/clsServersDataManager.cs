using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

public class clsServersDataManager
{
    public static DataTable SelectView()
    {
        String strSQL = "SELECT * FROM TBL_SERVERS_DATA_VIW";

        clsOracleDBMS objDBMS = new clsOracleDBMS();

        return objDBMS.ExecuteSelectSQL(strSQL);
    }

    public static DataTable SelectProjects()
    {
        String strSQL = "SELECT Distinct(PROJECT) FROM tbl_servers_data";

        clsOracleDBMS objDBMS = new clsOracleDBMS();

        return objDBMS.ExecuteSelectSQL(strSQL);
    }

    public static DataTable SelectServer(String strID)
    {
        String strSQL = "SELECT * FROM TBL_SERVERS_DATA WHERE ID = '" + strID + "'";

        clsOracleDBMS objDBMS = new clsOracleDBMS();

        return objDBMS.ExecuteSelectSQL(strSQL);
    }

    public static DataTable SelectServer(String strID, String strProjectName)
    {
        String strSQL = "SELECT * FROM TBL_SERVERS_DATA_VIW WHERE PROJECT = '" + strProjectName + "'";

        clsOracleDBMS objDBMS = new clsOracleDBMS();

        return objDBMS.ExecuteSelectSQL(strSQL);
    }


    public static DataTable SearchServer(String strServerName)
    {
        String strSQL = @"SELECT s.id AS ID, s.name AS ""Server Name"", s.ip AS IP FROM tbl_servers_data s WHERE s.name LIKE '%" + strServerName + "%' ORDER BY s.id";

        clsOracleDBMS objDBMS = new clsOracleDBMS();

        return objDBMS.ExecuteSelectSQL(strSQL);
    }
}
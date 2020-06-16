using System;
using System.Data;
using System.Data.OracleClient;

public class clsOracleDBMS
{
    OracleConnection objOracleConnection;
    OracleCommand objOracleCommand;

    public clsOracleDBMS()
    {
        objOracleConnection = new OracleConnection();
        objOracleCommand = new OracleCommand();

        objOracleCommand.Connection = objOracleConnection;
/*         objOracleConnection.ConnectionString = "Data Source = " + 
                                               "(DESCRIPTION = " + 
                                               "(ADDRESS = (PROTOCOL = TCP)" +
                                               "(HOST = 192.9.200.121)" +
                                               "(PORT = 1521))" + 
                                               "(CONNECT_DATA = " +
                                               "(SERVICE_NAME = mcdsdb)));" +
                                               "User Id = passmg; Password = passmg;"; */

        objOracleConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LogViewerDSIIS"].ConnectionString;
    }

    public void ExecuteSQL(String strSQL)
    {
        try
        {
            objOracleCommand.CommandText = strSQL;

            objOracleConnection.Open();
            objOracleCommand.ExecuteNonQuery();
            objOracleConnection.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(clsUtilities.GetErr(ex.Message.ToString()));
        }
    }

    public DataTable ExecuteSelectSQL(String strSQL)
    {
        try
        {
            objOracleCommand.CommandText = strSQL;

            OracleDataAdapter objOracleDataAdapter = new OracleDataAdapter(objOracleCommand);
            DataTable objDataTable = new DataTable();

            objOracleDataAdapter.Fill(objDataTable);

            return objDataTable;
        }
        catch (Exception ex)
        {
            //throw new Exception(clsUtilities.GetErr(ex.Message.ToString()));
            throw new Exception(ex.Message.ToString());
        }
    }
}
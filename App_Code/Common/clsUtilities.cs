using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsUtilities
/// </summary>
public class clsUtilities
{
    public static String GetErr(String strOrginalErr)
    {
        String strErr = "Unknown Error - Please tell to your site Administrator.";

        if (strOrginalErr.IndexOf("Cannot open database", 0) >= 0)
        {
            strErr = "Connecting to database is not established.";
        }

        if (strOrginalErr.IndexOf("Login Failed for user", 0) >= 0)
        {
            strErr = "Connecting to database is not established.";
        }

        if (strOrginalErr.IndexOf("Invalid object name", 0) >= 0)
        {
            strErr = "Table name is not correct.";
        }

        if (strOrginalErr.IndexOf("Invalid column name", 0) >= 0)
        {
            strErr = "Column name is not correct.";
        }

        if (strOrginalErr.IndexOf("Violation of PRIMARY KEY constraint", 0) >= 0)
        {
            strErr = "There is already an account with that name. You can update this account or create another account with another name.";
        }

        if (strOrginalErr.IndexOf("The INSERT statement conflicted with the", 0) >= 0)
        {
            strErr = "The INSERT statement conflict.";
        }

        return strErr;
    }
}

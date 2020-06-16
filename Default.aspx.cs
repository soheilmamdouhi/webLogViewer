using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Renci.SshNet;
using System.IO;
using System.Data;
using System.Net;

public partial class _Default : Page
{
    public int intID;
    public int intStartLine;
    public String[] stringArr = new String[5];
    public String strTempPath = "C:\\webLogViewer\\temp\\";
    public String strLogPath = "C:\\webLogViewer\\logs\\";

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpBrowserCapabilities objHttpBrowserCapabilities = Request.Browser;

        if (!IsPostBack)
        {
            dgridShowData.DataSource = clsServersDataManager.SelectView();
            dgridShowData.DataBind();

            dgridProjects.DataSource = clsServersDataManager.SelectProjects();
            dgridProjects.DataBind();

            //lblMessageData01.Text = Request.UserHostAddress;
            //lblMessageData02.Text = Request.UserHostName;
            //lblMessageData02.Text = Request.ServerVariables["REMOTE_HOST"];

            clsLog objLog = new clsLog();

            objLog.strIP = Request.UserHostAddress;
            objLog.strLog_Date = DateTime.Now.ToString();
            objLog.strTarget_Name = "";
            objLog.strAction = "L";

            clsLogManager.InsertLog(objLog);
        }
    }

    protected void dgridShowData_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable objDataTable = new DataTable();

        objDataTable = clsServersDataManager.SelectServer(dgridShowData.SelectedRow.Cells[1].Text);

        stringArr = objDataTable.Rows[0].ItemArray.Select(arrServer => arrServer.ToString()).ToArray();

        using (var ssh = new SshClient(stringArr[2], stringArr[6], stringArr[7]))
        {
            ssh.Connect();

            lblTemp.Text = ssh.CreateCommand("wc -l " + stringArr[4] + " | awk '{print $1}'").Execute();

            ssh.Disconnect();
        }

        lblIDData.Text = dgridShowData.SelectedRow.Cells[1].Text;
        lblServerNameData.Text = dgridShowData.SelectedRow.Cells[2].Text;
        lblIPAddressData.Text = dgridShowData.SelectedRow.Cells[3].Text;

        btnRead.Enabled = true;
        btnDownloadFull.Enabled = true;
        btnDownloadLog.Enabled = false;
        lblStatus.Text = "Set";
        lblStatus.ForeColor = System.Drawing.Color.Green;

        lblMessageData01.Text = "";
        lblMessageData02.Text = "";
        txtOutput.Text = "";
    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        try
        {
            if ((lblIDData.Text == "") || (lblIPAddressData.Text == "") || (lblServerNameData.Text == ""))
            {
                lblMessageData01.Text = "Please select a server.";
                lblMessageData02.Text = "";
            }
            else
            {
                DataTable objDataTable = new DataTable();

                objDataTable = clsServersDataManager.SelectServer(dgridShowData.SelectedRow.Cells[1].Text);

                stringArr = objDataTable.Rows[0].ItemArray.Select(arrServer => arrServer.ToString()).ToArray();

                using (var ssh = new SshClient(stringArr[2], stringArr[6], stringArr[7]))
                {
                    ssh.Connect();

                    //intStartLine = Int32.Parse(ssh.CreateCommand("wc -l " + stringArr[4] + " | awk '{print $1}'").Execute());

                    intStartLine = Int32.Parse(lblTemp.Text);

                    int intLastLine = Int32.Parse(ssh.CreateCommand("wc -l " + stringArr[4] + " | awk '{print $1}'").Execute());
                    int intLinesToShow;

                    //intLinesToShow = (intLastLine - intStartLine) + 5;
                    intLinesToShow = intLastLine - intStartLine;

                    if (intLinesToShow == 0)
                    {
                        txtOutput.Text = "There is nothing to show.";
                    }
                    else
                    {
                        txtOutput.Text = "";
                        String strLog = ssh.CreateCommand("tail -" + intLinesToShow.ToString() + " " + stringArr[4]).Execute();

                        strLog = strLog.Replace("\n", "\r\n");

                        txtOutput.Text = strLog;

                        lblMessageData01.Text = "";
                        lblMessageData02.Text = "";

                        ssh.Disconnect();
                        btnDownloadLog.Enabled = true;
                    }

                    lblStatus.Text = "Not Set";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    btnRead.Enabled = false;

                    clsLog objLog = new clsLog();

                    objLog.strIP = Request.UserHostAddress;
                    objLog.strLog_Date = DateTime.Now.ToString();
                    objLog.strTarget_Name = stringArr[1];
                    objLog.strAction = "R";

                    clsLogManager.InsertLog(objLog);
                }
            }
        }
        catch (Exception ex)
        {
            Guid objGUID = Guid.NewGuid();
            String strException = ex.ToString();

            File.WriteAllText(strLogPath + DateTime.Today.ToString("yyyyMMdd") + "-" + objGUID.ToString() + ".log", strException);

            lblMessageData01.Text = "خطایی در سیستم رخ داده است. لطفا کد خطای زیر را به همراه ID سرور به واحد نصب و راه اندازی ارسال نمایید.\r\n";
            lblMessageData02.Text = DateTime.Today.ToString("yyyyMMdd") + "-" + objGUID.ToString();
        }
    }

    protected void btnDownloadLog_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtOutput.Text != "There is nothing to show.")
            {
                DataTable objDataTable = new DataTable();

                objDataTable = clsServersDataManager.SelectServer(dgridShowData.SelectedRow.Cells[1].Text);

                stringArr = objDataTable.Rows[0].ItemArray.Select(arrServer => arrServer.ToString()).ToArray();

                Guid objGUID = Guid.NewGuid();

                String strDestinationPath = strTempPath + stringArr[1] + objGUID.ToString() + ".log";
                File.WriteAllText(strDestinationPath, txtOutput.Text);

                Response.ContentType = "text/plain";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + stringArr[1] + ".log");
                Response.TransmitFile(Server.MapPath("~/temp/" + stringArr[1] + objGUID.ToString() + ".log"));
                Response.Flush();
                Response.SuppressContent = true;
                ApplicationInstance.CompleteRequest();

                btnDownloadLog.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Guid objGUID = Guid.NewGuid();
            String strException = ex.ToString();

            File.WriteAllText(strLogPath + DateTime.Today.ToString("yyyyMMdd") + "-" + objGUID.ToString() + ".log", strException);

            lblMessageData01.Text = "خطایی در سیستم رخ داده است. لطفا کد خطای زیر را به واحد نصب و راه اندازی ارسال نمایید.\r\n";
            lblMessageData02.Text = DateTime.Today.ToString("yyyyMMdd") + "-" + objGUID.ToString();
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            if ((lblIDData.Text == "") || (lblIPAddressData.Text == "") || (lblServerNameData.Text == ""))
            {
                lblMessageData01.Text = "Please select a server.";
                lblMessageData02.Text = "";
            }
            else
            {
                String strDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                DataTable objDataTable = new DataTable();

                objDataTable = clsServersDataManager.SelectServer(dgridShowData.SelectedRow.Cells[1].Text);

                stringArr = objDataTable.Rows[0].ItemArray.Select(arrServer => arrServer.ToString()).ToArray();

                String strFilePath = stringArr[4].Remove(stringArr[4].LastIndexOf("/"), stringArr[4].Length - stringArr[4].LastIndexOf("/"));
                String strFileName = stringArr[4].Remove(0, stringArr[4].LastIndexOf("/") + 1);

                using (var ssh = new SshClient(stringArr[2], stringArr[6], stringArr[7]))
                {
                    ssh.Connect();

                    String strCommandChain = "cd " + strFilePath + "; tar -czvf " + strFileName + ".tgz " + strFileName;
                    ssh.CreateCommand(strCommandChain).Execute();

                    ssh.Disconnect();
                }

                using (var objScpClient = new ScpClient(stringArr[2], stringArr[6], stringArr[7]))
                using (var ssh = new SshClient(stringArr[2], stringArr[6], stringArr[7]))
                {
                    objScpClient.Connect();

                    string objTempFile = Path.GetTempFileName();
                    FileInfo objFileInfo = new FileInfo(objTempFile);

                    
                    objScpClient.Download(stringArr[4] + ".tgz", objFileInfo);

                    Guid objGUID = Guid.NewGuid();

                    String strDestinationPath = strTempPath + strFileName + objGUID.ToString() + ".tgz";

                    objFileInfo.MoveTo(strDestinationPath);

                    objScpClient.Disconnect();

                    Response.ContentType = "application/x-compressed";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".tgz");
                    Response.TransmitFile(Server.MapPath("~/temp/" + strFileName + objGUID.ToString() + ".tgz"));
                    Response.Flush();
                    Response.SuppressContent = true;
                    ApplicationInstance.CompleteRequest();

                    clsLog objLog = new clsLog();

                    objLog.strIP = Request.UserHostAddress;
                    objLog.strLog_Date = DateTime.Now.ToString();
                    objLog.strTarget_Name = stringArr[1];
                    objLog.strAction = "D";

                    clsLogManager.InsertLog(objLog);
                }

                lblMessageData01.Text = "";
                lblMessageData02.Text = "";
            }
        }
        catch (Exception ex)
        {
            Guid objGUID = Guid.NewGuid();
            String strException = ex.ToString();

            File.WriteAllText(strLogPath + DateTime.Today.ToString("yyyyMMdd") + "-" + objGUID.ToString() + ".log", strException);

            lblMessageData01.Text = "خطایی در سیستم رخ داده است. لطفا کد خطای زیر را به واحد نصب و راه اندازی ارسال نمایید.\r\n";
            lblMessageData02.Text = DateTime.Today.ToString("yyyyMMdd") + "-" + objGUID.ToString();
        }
    }

    protected void dgridProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblProjectName.Text = dgridProjects.SelectedRow.Cells[1].Text;

        dgridShowData.DataSource = clsServersDataManager.SelectServer(lblIDData.Text, lblProjectName.Text);
        dgridShowData.DataBind();

        lblMessageData01.Text = "";
        lblMessageData02.Text = "";
        txtOutput.Text = "";
    }
}
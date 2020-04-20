﻿using System;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dgridShowData.DataSource = clsServersDataManager.SelectView();
            dgridShowData.DataBind();

            dgridProjects.DataSource = clsServersDataManager.SelectProjects();
            dgridProjects.DataBind();
        }
    }

    protected void dgridShowData_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblIDData.Text = dgridShowData.SelectedRow.Cells[1].Text;
        lblServerNameData.Text = dgridShowData.SelectedRow.Cells[2].Text;
        lblIPAddressData.Text = dgridShowData.SelectedRow.Cells[3].Text;

        lblMessageData.Text = "";
    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        try
        {
            if ((lblIDData.Text == "") || (lblIPAddressData.Text == "") || (lblServerNameData.Text == ""))
            {
                lblMessageData.Text = "Please select a server.";
            }
            else
            {
                String strDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                DataTable objDataTable = new DataTable();

                objDataTable = clsServersDataManager.SelectServer(dgridShowData.SelectedRow.Cells[1].Text);

                stringArr = objDataTable.Rows[0].ItemArray.Select(arrServer => arrServer.ToString()).ToArray();

                using (var ssh = new SshClient(stringArr[2], "root", stringArr[3]))
                {
                    ssh.Connect();

                    intStartLine = Int32.Parse(ssh.CreateCommand("wc -l " + stringArr[4] + " | awk '{print $1}'").Execute());

                    int intLastLine = Int32.Parse(ssh.CreateCommand("wc -l " + stringArr[4] + " | awk '{print $1}'").Execute());
                    int intLinesToShow;
                    intLinesToShow = (intLastLine - intStartLine) + 5;

                    String strLog = ssh.CreateCommand("tail -" + intLinesToShow.ToString() + " " + stringArr[4]).Execute();

                    strLog = strLog.Replace("\n", "\r\n");
                    txtOutput.Text = strLog;

                    using (WebClient Client = new WebClient())
                    {
                        Client.DownloadFile("http://www.abc.com/file/song/a.mpeg", "a.mpeg");
                    }


                    File.WriteAllText(strDesktop + "\\Log.txt", strLog);
                    lblMessageData.Text = "";

                    ssh.Disconnect();
                }
            }
        }
        catch (Exception ex)
        {
            lblMessageData.Text = ex.ToString();
        }

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            if ((lblIDData.Text == "") || (lblIPAddressData.Text == "") || (lblServerNameData.Text == ""))
            {
                lblMessageData.Text = "Please select a server.";
            }
            else
            {
                String strDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                DataTable objDataTable = new DataTable();

                objDataTable = clsServersDataManager.SelectServer(dgridShowData.SelectedRow.Cells[1].Text);

                stringArr = objDataTable.Rows[0].ItemArray.Select(arrServer => arrServer.ToString()).ToArray();

                String strFilePath = stringArr[4].Remove(stringArr[4].LastIndexOf("/"), stringArr[4].Length - stringArr[4].LastIndexOf("/"));
                String strFileName = stringArr[4].Remove(0, stringArr[4].LastIndexOf("/") + 1);

                using (var ssh = new SshClient(stringArr[2], "root", stringArr[3]))
                {
                    ssh.Connect();

                    String strCommandChain = "cd " + strFilePath + "; tar -czvf " + strFileName + ".tgz " + strFileName;
                    ssh.CreateCommand(strCommandChain).Execute();

                    ssh.Disconnect();
                }

                using (var objScpClient = new ScpClient(stringArr[2], "root", stringArr[3]))
                {
                    objScpClient.Connect();

                    string objTempFile = Path.GetTempFileName();
                    FileInfo objFileInfo = new FileInfo(objTempFile);


                    objScpClient.Download(stringArr[4] + ".tgz", objFileInfo);

                    //String strDestinationPath = strDesktop + "\\" + strFileName + ".tgz";

                    //if (File.Exists(strDestinationPath) == true)
                    //{
                    //    File.Delete(strDestinationPath);
                    //    objFileInfo.MoveTo(strDestinationPath);
                    //}
                    //else
                    //{
                    //    objFileInfo.MoveTo(strDestinationPath);
                    //}

                    objScpClient.Disconnect();
                }

                lblMessageData.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMessageData.Text = ex.ToString();
        }
    }

    protected void dgridProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblProjectName.Text = dgridProjects.SelectedRow.Cells[1].Text;

        dgridShowData.DataSource = clsServersDataManager.SelectServer(lblIDData.Text, lblProjectName.Text);
        dgridShowData.DataBind();

        lblMessageData.Text = "";
    }
}
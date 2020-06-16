<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" validateRequest=false %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-6">
            <h2>Servers</h2>
            <p>
                <asp:GridView ID="dgridProjects" runat="server" CellPadding="4" ForeColor="#333333" OnSelectedIndexChanged="dgridProjects_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField SelectImageUrl="~/Images/checkmark.png" ShowSelectButton="True" />
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
                <asp:Label ID="lblProjectName" runat="server" Visible="False"></asp:Label>
            </p>
            <p>
                <asp:GridView ID="dgridShowData" runat="server" CellPadding="4" ForeColor="#333333" OnSelectedIndexChanged="dgridShowData_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField SelectImageUrl="~/Images/checkmark.png" ShowSelectButton="True" />
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
            </p>
        </div>
        <div class="col-md-6">
            <h2>Server Data</h2>
            <p>
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Red" Text="Not Set"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lblID" runat="server" Font-Bold="True" Text="ID: "></asp:Label>
                <asp:Label ID="lblIDData" runat="server"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lblServerName" runat="server" Font-Bold="True" Text="Server Name: "></asp:Label>
                <asp:Label ID="lblServerNameData" runat="server"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lblIPAddress" runat="server" Font-Bold="True" Text="IP Address:  "></asp:Label>
                <asp:Label ID="lblIPAddressData" runat="server"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lblTemp" runat="server" Visible="False"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lblMessages" runat="server" Font-Bold="True" Text="Messages:"></asp:Label>
&nbsp;
                <asp:Label ID="lblMessageData01" runat="server" ForeColor="Red" style="text-align: right;"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lblMessageData02" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>
                &nbsp;<asp:Button ID="btnRead" runat="server" Text="Show Log" OnClick="btnRead_Click" Width="130px" Enabled="False" />
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnDownloadLog" runat="server" Enabled="False" OnClick="btnDownloadLog_Click" Text="Download Log" Width="130px" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnDownloadFull" runat="server" OnClick="btnDownload_Click" Text="Download Log File" Width="130px" Enabled="False" />
            </p>
            <p>
                <asp:TextBox ID="txtOutput" runat="server" Font-Size="Small" Height="500px" ReadOnly="True" TextMode="MultiLine" Width="100%" Wrap="False"></asp:TextBox>
            </p>
        </div>
    </div>
</asp:Content>

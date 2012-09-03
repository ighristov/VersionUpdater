<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="VersionUpdaterSrv._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <%--    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">
            www.asp.net</a>.
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.</p>
    --%>
    <div style="display: inline-table;">
        <div class="header" style="width: auto; display: block">
            <h2 style="color: White; padding-left: 20px; padding-bottom: 10px">
                Currently uploaded applications</h2>
        </div>
        <div style="float: none; padding-right: 20px; width: 450px;">
            <asp:GridView ID="gvApplications" runat="server" BorderStyle="Solid" BorderWidth="1px"
                CellPadding="4" DataSourceID="odsApplications" ForeColor="#333333" GridLines="Vertical"
                AutoGenerateColumns="False" Width="100%" 
                DataKeyNames="ApplicationName,Version" onrowcommand="gvApplications_RowCommand">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="ApplicationName" HeaderText="Application Name">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="Version" HeaderText="Last Version">
                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateTime" HeaderText="Date">
                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="11px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnDownload" ImageUrl="~/Images/SaveHS.png" CommandName="Select"
                                CommandArgument='<%#Bind("ApplicationName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <div style="float: none; padding-right: 20px; width: 300px;">
                <asp:GridView ID="gvVersions" runat="server" BorderStyle="Solid" BorderWidth="1px"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>
        </div>
        <asp:ObjectDataSource ID="odsApplications" runat="server" SelectMethod="GetApps_and_LastVersion"
            TypeName="VersionUpdaterSrv.XMLHelper"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsVersions" runat="server" SelectMethod="GetVersions"
            TypeName="VersionUpdaterSrv.XMLHelper"></asp:ObjectDataSource>
    </div>
    <div style="width: auto">
        <div class="header" style="width: auto; float: none;">
            <h2 style="color: White; padding-left: 20px;">
                Upload new file</h2>
        </div>
        <div style="padding-top: 10px">
            <table style="display: inline">
                <tr>
                    <td>
                        FileName:
                    </td>
                    <td style="padding-left: 10px">
                        Version:
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px;">
                        <asp:FileUpload ID="FileUpload1" runat="server" Style="width: 100%" />
                    </td>
                    <td style="padding-left: 10px">
                        <asp:TextBox ID="tbVersion" runat="server" Width="70px" Text="1.0.0.0"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px">
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
</asp:Content>

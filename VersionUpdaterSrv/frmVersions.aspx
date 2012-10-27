<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="frmVersions.aspx.cs" Inherits="VersionUpdaterSrv._Default" %>

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
    <div style="display: block; width: 100%">
        <div class="header" style="width: auto; display: block; margin-bottom: 2px">
            <h2 style="color: White; padding-left: 20px;">
                Currently uploaded applications
            </h2>
        </div>
        <div style="float: none; padding-right: 20px; width: 540px;">
            <asp:GridView ID="gvApplications" runat="server" BorderStyle="Solid" BorderWidth="1px"
                CellPadding="4" DataSourceID="odsApplications" ForeColor="#333333" GridLines="Vertical"
                AutoGenerateColumns="False" Width="100%" DataKeyNames="ApplicationName,Version"
                OnRowCommand="gvApplications_RowCommand">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="Application Name">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnSelect" CommandName="Select" CommandArgument='<%#Bind("ApplicationName") %>'
                                Text='<%#Bind("ApplicationName") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ApplicationName" HeaderText="Application Name" Visible="False">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="Version" HeaderText="Last Version">
                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateTime" HeaderText="Date">
                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="11px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FileSize" HeaderText="FileSize">
                        <ItemStyle Width="80px" HorizontalAlign="Right" Font-Size="11px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" Width="40px"/>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnDownloadLastVersion" ImageUrl="~/Images/SaveHS.png"
                                CommandName="Download" CommandArgument='<%#Bind("ApplicationName") %>' />
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
        </div>
        <br />
        <hr />
        <br />
        <div style="width: 100%">
            <div class="header" style="width: 100%; margin-bottom: 2px;">
                <h2 style="color: White; padding-left: 20px;">
                    <asp:Label runat="server" ID="lblVersionsCaption" Text="Versions" ForeColor="White"
                        Style="padding-left: 20px"></asp:Label>
                </h2>
            </div>
            <asp:GridView ID="gvVersions" runat="server" BorderStyle="Solid" BorderWidth="1px"
                CellPadding="4" ForeColor="#333333" GridLines="Vertical" AutoGenerateColumns="False"
                AllowSorting="True" Width="100%" DataKeyNames="ApplicationName,Version" OnRowCommand="gvVersions_RowCommand">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Version" HeaderText="Version">
                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="FileName" DataField="FileName">
                        <ItemStyle Font-Size="11px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FileSize" HeaderText="FileSize">
                        <ItemStyle Width="80px" HorizontalAlign="Right" Font-Size="11px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Checksum" DataField="Checksum">
                        <ItemStyle Width="200px" Font-Size="11px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateTime" HeaderText="Date">
                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="11px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" Width="40px"/>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnDownloadVersion" ImageUrl="~/Images/SaveHS.png"
                                CommandName="Download" CommandArgument='<%#Bind("ApplicationName") %>' />
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
                <EmptyDataTemplate>
                    Please select an application to view its versions.
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
        <asp:ObjectDataSource ID="odsApplications" runat="server" SelectMethod="GetApps_and_LastVersion"
            TypeName="VersionUpdaterSrv.XMLHelper"></asp:ObjectDataSource>
        <%--        <asp:ObjectDataSource ID="odsVersions" runat="server" SelectMethod="GetVersions"
            TypeName="VersionUpdaterSrv.XMLHelper"></asp:ObjectDataSource>
        --%>
    </div>
    <br />
    <hr />
    <div style="width: auto">
        <div class="header" style="width: auto; float: none;">
            <h2 style="color: White; padding-left: 20px;">
                Upload new file</h2>
        </div>
        <div>
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
                    <td style="padding-left: 30px;">
                        <asp:TextBox ID="tbVersion" runat="server" Width="70px" Text="1.0.0.0"></asp:TextBox>
                    </td>
                    <td style="padding-left: 30px">
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSelectFile" runat="server" Text="Please select a file to be uploaded"
                            ForeColor="Red" Visible="false"></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="rfFileName" runat="server" Style="display: block"
                            ControlToValidate="FileUpload1" ErrorMessage="RequiredFieldValidator" ForeColor="#FF3300"
                            Display="Dynamic" SetFocusOnError="True">Please select a file to be uploaded</asp:RequiredFieldValidator>--%>
                    </td>
                    <td style="padding-left: 30px">
                        <asp:Label ID="lblEnterVersion" runat="server" Text="Please enter a version number"
                            ForeColor="Red" Visible="false"></asp:Label>
                        <%--                        <asp:RequiredFieldValidator ID="rfVersion" runat="server" Style="display: block"
                            ControlToValidate="tbVersion" ErrorMessage="RequiredFieldValidator" ForeColor="#FF3300"
                            Display="Dynamic" SetFocusOnError="True">Please enter a version number</asp:RequiredFieldValidator>
                        --%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

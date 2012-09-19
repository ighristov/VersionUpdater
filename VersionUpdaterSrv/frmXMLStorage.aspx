<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="frmXMLStorage.aspx.cs" Inherits="VersionUpdaterSrv.frmXMLStorage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: auto; height: auto">
        <div class="header" style="width: auto; float: none;">
            <h2 style="color: White; padding-left: 20px;">
                XML Storage File</h2>
        </div>
        <asp:TextBox ID="tbXML" runat="server" TextMode="MultiLine" Width="100%" Height="95%"
            Wrap="False" Rows="20"></asp:TextBox>
    </div>
    <div style="width: auto">
        <div class="header" style="width: auto; float: none;">
            <h2 style="color: White; padding-left: 20px;">
                Upload new XML file</h2>
        </div>
        <div>
            <table style="display: inline">
                <tr>
                    <td>
                        FileName:
                    </td>
                    <td style="padding-left: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px;">
                        <asp:FileUpload ID="FileUpload1" runat="server" Style="width: 100%" />
                    </td>
                    <td style="padding-left: 30px">
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSelectFile" runat="server" Text="Please select a file to be uploaded"
                            ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

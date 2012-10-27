<%@ Page Title="Request Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="frmRequest.aspx.cs" Inherits="VersionUpdaterSrv.frmRequest" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <div style="width: auto">
        <div class="header" style="width: auto; float: none;">
            <h2 style="color: White; padding-left: 20px;">
                Request file</h2>
        </div>
        <div style="padding-top: 10px">
            <table style="display: inline">
                <tr>
                    <td>
                        URL:
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="tbURL" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Application Name:
                    </td>
                    <td style="padding-left: 10px">
                        Version:
                    </td>
                    <td style="padding-left: 10px">
                        Where to be saved:
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px;">
                        <asp:TextBox runat="server" ID="tbAppName" Width="100%"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px">
                        <asp:TextBox ID="tbVersion" runat="server" Width="70px" Text="1.0.0.0"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px">
                        <asp:TextBox ID="tbSavePath" runat="server" Width="100%" Text="c:\"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px">
                        <asp:Button ID="btnRequest" runat="server" Text="Request" OnClick="btnRequest_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="lblResult" runat="server" Text="Result:"></asp:Label>
    </div>
</asp:Content>

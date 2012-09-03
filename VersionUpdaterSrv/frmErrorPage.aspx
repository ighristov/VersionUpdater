<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="frmErrorPage.aspx.cs" Inherits="VersionUpdaterSrv.frmErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="color: Red">
        Sorry, an error has just occured.</h2>
    <hr />
    <br />
    <div>
        <b>Error description</b><br />
        <asp:Label ID="lblMainError" runat="server" Text="..." Font-Size="0.9em" ForeColor="Black"></asp:Label>
    </div>
    <hr />
    <br />
    <div id="divBaseException" runat="server">
        <b>Base exception</b><br />
        <asp:Label ID="lblBaseException" runat="server" Text="..." Font-Size="0.9em" ForeColor="Black"></asp:Label>
    </div>
    <hr />
    <br />
    <div id="divInnerException" runat="server">
        <b>Inner exception</b><br />
        <asp:Label ID="lblInnerException" runat="server" Text="..." Font-Size="0.9em" ForeColor="Black"></asp:Label>
    </div>
</asp:Content>

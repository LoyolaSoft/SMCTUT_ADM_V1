﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConductCertificate.aspx.cs" Inherits="CMS.RDLCPages.ConductCertificate" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:reportviewer ID="rvConductCertificate" Width="100%"  BackColor="White" runat="server"></rsweb:reportviewer>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </div>
    </form>
</body>
</html>

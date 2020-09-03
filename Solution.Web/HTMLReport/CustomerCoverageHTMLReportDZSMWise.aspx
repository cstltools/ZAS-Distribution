<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerCoverageHTMLReportDZSMWise.aspx.cs" Inherits="HTMLReport_CustomerCoverageHTMLReportDZSMWise" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../HTML_Report_css/proformatable.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        
        <asp:Label ID="messageLabel" ForeColor="red" runat="server" Text=""></asp:Label>
        
        <br />
        <br />

        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../HTML_Report_css/images/excel.png"
            OnClick="ImageButton1_Click" Width="40px" />
        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="responstable">
            <Columns>
                <asp:BoundField DataField="MiaCode" HeaderText="Mia Code"></asp:BoundField>
                <asp:BoundField DataField="MiaName" HeaderText="Mia Name" />
                <asp:BoundField DataField="TotalCust" HeaderText="Total Customer" />
                <asp:BoundField DataField="TotalColl" HeaderText="Total Collection " />
                <asp:BoundField DataField="TotalExec" HeaderText="Total Excution" />
                <asp:BoundField DataField="TotalRej" HeaderText="Total Rejection" />
                <asp:BoundField DataField="TotalAmnt" HeaderText="Total Amount" />
                <asp:BoundField DataField="CustCov" HeaderText="Customer Coverage" />
                <asp:BoundField DataField="Cov" HeaderText="Coverage(%)" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>


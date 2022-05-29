<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WHBeenCardHTMLReport.aspx.cs"
    Inherits="HTMLReport_ProformaHTMLReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Report</title>
    <%--<link href="../HTML_Report_css/proformatable.css" rel="stylesheet" type="text/css" />--%>
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
    <style type="text/css">
        .alignment
        {
            width: 20%;
            margin: 0 auto;
            color: #008080;
            text-align: center;
        }
        
        table
        {
            text-align: center;
        }
    </style>
</head>
<body>
    <h2 class="alignment">
        Warehouse Bin Card 
    </h2>
    <hr />
    <tr>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
            <span style="font-weight: bold">From: </span>
        </td>
        <td class="TDRight" width="20%">
             <asp:Label ID="FromTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label> &nbsp;
        </td>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
             <span style="font-weight: bold">To: </span>
        </td>
        <td class="TDRight" width="20%">
             <asp:Label ID="toTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label> &nbsp;
        </td>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
             <span style="font-weight: bold">Company: </span>
        </td>
        <td class="TDRight" width="20%">
           <asp:Label ID="dcTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label> &nbsp;
        </td>
    </tr>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../HTML_Report_css/images/excel.png"
            OnClick="ImageButton1_Click" Width="40px" />
        <br />
        <span style="color: #b8860b;">Import Excel</span>
        <br />
        <br />
        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="responstable">
            <Columns>
               
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Description " />
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />               
                <asp:BoundField DataField="OpeningStock" HeaderText="Opening Stock" />
                <asp:BoundField DataField="StockReceive" HeaderText="Stock Receive" />
                <asp:BoundField DataField="TotalReceive" HeaderText="Total Receive" />
                <asp:BoundField DataField="Dhaka" HeaderText="Dhaka" />
                <asp:BoundField DataField="Bogra" HeaderText="Bogra" />
                <asp:BoundField DataField="Chittagong" HeaderText="Chittagong" />
                <asp:BoundField DataField="Noakhali" HeaderText="Noakhali" />
                <asp:BoundField DataField="Sylhet" HeaderText="Sylhet" />
                <asp:BoundField DataField="Khulna" HeaderText="Khulna" />
                <asp:BoundField DataField="Mymensingh" HeaderText="Mymensingh" />
                <asp:BoundField DataField="Rangpur" HeaderText="Rangpur" />
                <asp:BoundField DataField="Rajshahi" HeaderText="Rajshahi" />
                <asp:BoundField DataField="Cumilla" HeaderText="Cumilla" />
                <asp:BoundField DataField="Transit" HeaderText="Transit" />
                <asp:BoundField DataField="Freeze" HeaderText="Freeze" />
                <asp:BoundField DataField="TotalIssue" HeaderText="Total Issue" />
                <asp:BoundField DataField="ClosingStock" HeaderText="Closing Stock" />
                <asp:BoundField DataField="StockValue" HeaderText="Closing Stock Value" />

            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="responstable">
            <Columns>
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Description " />
                <%--<asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="DelivaryInvoiceNo" />--%>
                <%--<asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" />--%>
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                <asp:BoundField DataField="AQty" HeaderText="Available Qty" />
                <asp:BoundField DataField="BookFDel" HeaderText="Book For Delv" />
                <%--<asp:BoundField DataField="VatAmount" HeaderText="VatAmount" />--%>
                <asp:BoundField DataField="Tqty" HeaderText="Transit Qty" />
                <asp:BoundField DataField="RQty" HeaderText="Restricted Qty" />
                <%--<asp:BoundField DataField="ReturnReason" HeaderText="ReturnReason" />--%>
                <asp:BoundField DataField="BQty" HeaderText="Blocked Qty" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesTrackingHTMLReport.aspx.cs"
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
            width: 60%;
            margin: 20px auto;
            color: #008080;
            text-align: center;
        }
        
        html, body
        {
            height: 100%;
        }
        
        html
        {
            display: table;
            margin: auto;
        }
        
        body
        {
            display: table-cell;
            vertical-align: middle;
        }
        
        th, td
        {
            text-align: center;
            padding: 5px;
        }
    </style>
</head>
<body>
    <h2 class="alignment">
         Invoice Monitoring Report 
    </h2>
    <hr />
    <tr>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
            <span style="font-weight: bold">From: </span>
        </td>
        <td class="TDRight" width="20%">
            <asp:Label ID="FromTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
            &nbsp;
        </td>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
            <span style="font-weight: bold">To: </span>
        </td>
        <td class="TDRight" width="20%">
            <asp:Label ID="toTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
            &nbsp;
        </td>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
            <span style="font-weight: bold"></span>
        </td>
        <td class="TDRight" width="20%">
            <asp:Label ID="dcTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
            &nbsp;
        </td>
    </tr>
    <form id="form1" runat="server">
    <div class="box" border="1px">
        <br />
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../HTML_Report_css/images/excel.png"
            OnClick="ImageButton1_Click" Width="40px" />
        <br />
        <span style="color: #b8860b;">Import Excel</span>
        <br />
        <br />
        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="responstable">
            <Columns>
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="InvoiceValue" HeaderText="Invoice Value" />
                <asp:BoundField DataField="ActualSales" HeaderText="Actual Sales" />
                <asp:BoundField DataField="Discount" HeaderText="Discount" />
                <asp:BoundField DataField="InTransit" HeaderText="In-Transit" />
                <asp:BoundField DataField="ReturnValue" HeaderText="ReturnValue" />
                <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" />
                <asp:BoundField DataField="Due" HeaderText="Due" />
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
    <tr>
        <td class="TDLeft" width="13%">
            &nbsp;
        </td>
        <td class="TDRight" width="20%">
            &nbsp;
        </td>
        <td class="TDLeft" width="13%">
            &nbsp;
        </td>
        <td class="TDRight" width="20%">
            &nbsp;
        </td>
        <td class="TDLeft" width="13%">
            &nbsp;
        </td>
        <td class="TDRight" width="20%">
            &nbsp;
        </td>
    </tr>
</body>
</html>

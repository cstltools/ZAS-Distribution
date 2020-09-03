<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelivaryReturnHTMLReport.aspx.cs"
    Inherits="HTMLReport_DelivaryReturnHTMLReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delivary Return Report</title>
    <link href="../HTML_Report_css/delivarytable.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
    
    <style type="text/css">
        .alignment
        {
            width: 20%;
            margin: 0 auto;
            color: #008080;
            text-align: center;
        }
    </style>
</head>
<body>
    <h2 class="alignment">
        Delivary Return Report
    </h2>
    <hr />

    <form id="form1" runat="server">
    <div>
        <br />
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../HTML_Report_css/images/excel.png"
            OnClick="ImageButton1_Click" Width="40px" />
        <br />
        <span style="color: #b8860b;">Import Excel</span>
        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="responstable">
            <Columns>
                <asp:BoundField DataField="ComUnitCode" HeaderText="DC Code" />
                <asp:BoundField DataField="ComUnitName" HeaderText="DC Name" />
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                <asp:BoundField DataField="ExpDate" HeaderText="Exp Date" />
                <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                <asp:BoundField DataField="OrderNo" HeaderText="Order No" />
                <%--<asp:BoundField DataField="OrderDate" HeaderText="OrderDate" />--%>
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" />
                <%--<asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />--%>
                <asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="Dlv Inv No" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="VatAmount" HeaderText="Vat Amount" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                <%--<asp:BoundField DataField="UpdateDate" HeaderText="Update " />--%>
                <asp:BoundField DataField="RegionCode" HeaderText="Region Code" />
                <asp:BoundField DataField="AreaCode" HeaderText="Area Code" />
                <asp:BoundField DataField="TerritoryCode" HeaderText="Territory Code" />
                <asp:BoundField DataField="MiaCode" HeaderText="Mia Code" />
                <%--<asp:BoundField DataField="DistrictCode" HeaderText="Area Code" />--%>
                <asp:BoundField DataField="MarketCode" HeaderText="Market Code" />
                <asp:BoundField DataField="ReturnReason" HeaderText="Return Reason" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>

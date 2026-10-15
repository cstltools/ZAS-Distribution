<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_ProformaHTMLReport, App_Web_kpk3yd3j" %>

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
        
        .box {
  margin: auto;
  width: 50%;
  border: 3px solid black;
  padding: 10px;
  text-align: center;
}
    </style>
</head>
<body>
    <h2 class="alignment">
        STOCK REPORT
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
             <span style="font-weight: bold">DC: </span>
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
                <%--<asp:BoundField DataField="ComUnitCode" HeaderText="DC Code"></asp:BoundField>
                <asp:BoundField DataField="ComUnitName" HeaderText="DC Name" />--%>
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Description " />
                <asp:BoundField DataField="BaseUnit" HeaderText="Pack Size" />
                <%--    <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                <asp:BoundField DataField="ExpDate" HeaderText="Exp.Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="MfgDate" HeaderText="Mfg.Date" DataFormatString="{0:dd-MMM-yyyy}" />--%>
                <asp:BoundField DataField="OpeningStock" HeaderText="Opening Stock" />
                <asp:BoundField DataField="ReceiveFromCentralWarehouse" HeaderText="Rcv From (CWH)" />
                <asp:BoundField DataField="ReceiveFromAreaOfficeInterTransfer" HeaderText="Rcv From (B2B)" />
                <asp:BoundField DataField="ReturnQty" HeaderText="Return Rcv Qty" />
                <asp:BoundField DataField="IntransitWHB2B" HeaderText="In-Transit (WH&B2B)" />
                <asp:BoundField DataField="TotalReceived" HeaderText="Total Received" />
                <asp:BoundField DataField="TotalStock" HeaderText="Total Stock" />
                <asp:BoundField DataField="IssuedToSales" HeaderText="Issued To Sales" />
                <asp:BoundField DataField="IssuedToBonus" HeaderText="Issued To Bonus" />
                <asp:BoundField DataField="SampleIssue" HeaderText="Sample Issue" />
                <asp:BoundField DataField="SalesInTransit" HeaderText="In-Transit (Sales)" />
                <asp:BoundField DataField="IssuedToAreaOfficeInterTransfer" HeaderText="Issued To (B2B)" />
                <asp:BoundField DataField="IssuedToDamageAndOthers" HeaderText="Issued To Damage & Others" />
                <asp:BoundField DataField="Blocked" HeaderText="Blocked" />
                <asp:BoundField DataField="TotalIssue" HeaderText="Total Issued" />
                <asp:BoundField DataField="ClosingStock" HeaderText="Closing Stock" />
                <asp:BoundField DataField="ClosingStockValue" HeaderText="Closing Value" />
                <%--<asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="DelivaryInvoiceNo" />--%>
                <%--<asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" />--%>
                <%--<asp:BoundField DataField="VatAmount" HeaderText="VatAmount" />--%>
                <%--<asp:BoundField DataField="ReturnReason" HeaderText="ReturnReason" />--%>
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
    
    <br />
    
    <div class="box" >
            <span style="font-size: 18px; font-weight: bold;">Total Value: </span> <asp:Label ID="totalReceiveableLabel" CssClass="currency" runat="server"></asp:Label>
        </div>

    </form>
    
     <%--<tr>
        <td class="TDLeft" width="13%">&nbsp;
        </td>
        <td class="TDRight" width="20%">&nbsp;
        </td>
        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
            <span style="font-size: 18px; font-weight: bold;">Total Value: </span>
        </td>
        <td class="TDRight" width="20%">
            
        </td>
        <td class="TDLeft" width="13%"></td>
        <td class="TDRight" width="20%"></td>
    </tr>--%>
</body>
</html>

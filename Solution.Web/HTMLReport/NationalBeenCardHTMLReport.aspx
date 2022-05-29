<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NationalBeenCardHTMLReport.aspx.cs"
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
        National Bin Card 
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
                <asp:BoundField DataField="WHOpeningStock" HeaderText="WHOpening Stock" />
                <asp:BoundField DataField="StockInQuantity" HeaderText="WHStock Receive" />
                <asp:BoundField DataField="WHReceive" HeaderText="Total Receive" />
                <asp:BoundField DataField="WHStockIssue" HeaderText="WHStock Issue" />
                <asp:BoundField DataField="WHInTransit" HeaderText="WH In Transit" />
                <asp:BoundField DataField="WHFreeze" HeaderText="WH Freeze" />
                <asp:BoundField DataField="WHTotalIssue" HeaderText="WHTotal Issue" />
                <asp:BoundField DataField="WHStock" HeaderText="WH Stock" />
                <asp:BoundField DataField="DepotOpeningStock" HeaderText="Depot Opening Stock" />
                <asp:BoundField DataField="DepotReceive" HeaderText="Depot Receive" />
                <asp:BoundField DataField="B2BRcvQty" HeaderText="B2B Receive" />
                <asp:BoundField DataField="B2BTransitQty" HeaderText="B2B In Transit" />
                <asp:BoundField DataField="FreezeQty" HeaderText="FreezeQty" />
                <asp:BoundField DataField="DepotTotalReceive" HeaderText="Depot Total Receive" />
                <asp:BoundField DataField="B2BIssueQty" HeaderText="B2B Issue" />
                <asp:BoundField DataField="SalesQuanty" HeaderText="Sales" />
                <asp:BoundField DataField="BonusQuantity" HeaderText="Bonus" />
                <asp:BoundField DataField="SampleIssue" HeaderText="Sample" />
                <asp:BoundField DataField="SalesTransitQty" HeaderText="Sales In Transit" />
                <asp:BoundField DataField="DamageQty" HeaderText="Damage" />
                <asp:BoundField DataField="DepotTotalIssue" HeaderText="Total Issue" />
                <asp:BoundField DataField="DepotTotalIssue" HeaderText="Depot Total Issue" />
                <asp:BoundField DataField="DepotStock" HeaderText="Depot Stock" />
                <asp:BoundField DataField="ClosingStock" HeaderText="Closing Stock" />

            </Columns>
            <EmptyDataTemplate>
        <div align="center">No records found.</div>
    </EmptyDataTemplate>
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

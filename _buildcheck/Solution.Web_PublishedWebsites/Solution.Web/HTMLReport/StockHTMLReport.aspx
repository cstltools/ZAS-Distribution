<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_ProformaHTMLReport, App_Web_ovixdqxn" %>

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
        
        table {
            text-align: center;
        }
        
    </style>
</head>
<body>
    <h2 class="alignment">
        STOCK REPORT
    </h2>
    <hr />
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
                <asp:BoundField DataField="ComUnitCode" HeaderText="DC Code"></asp:BoundField>
                <asp:BoundField DataField="ComUnitName" HeaderText="DC Name" />
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Description " />
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                <asp:BoundField DataField="ExpDate" HeaderText="Exp.Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="MfgDate" HeaderText="Mfg.Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="AQty" HeaderText="Available Qty" />
                <asp:BoundField DataField="AvialableValue" HeaderText="Available Value" />
                <asp:BoundField DataField="BookFDel" HeaderText="Book For Delv" />
                <asp:BoundField DataField="Tqty" HeaderText="Transit Qty" />
                <asp:BoundField DataField="RQty" HeaderText="Freeze Qty" />
                <asp:BoundField DataField="RestrictedValue" HeaderText="Freeze Value" />
                <asp:BoundField DataField="BQty" HeaderText="Blocked Qty" />
                <asp:BoundField DataField="BlockedValue" HeaderText="Block Value" />
                <asp:BoundField DataField="ReturnRcvQty" HeaderText="Return Rcv" />
                <asp:BoundField DataField="ReturnRcvValue" HeaderText="Return Rcv Value" />
                <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total" />
                <%--<asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="DelivaryInvoiceNo" />--%>
                <%--<asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" />--%>
                <%--<asp:BoundField DataField="VatAmount" HeaderText="VatAmount" />--%>
                <%--<asp:BoundField DataField="ReturnReason" HeaderText="ReturnReason" />--%>
            </Columns>
        </asp:GridView>
        
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="responstable">
            <Columns>
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Description " />
                <%--<asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="DelivaryInvoiceNo" />--%>
                <%--<asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" />--%>
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                <asp:BoundField DataField="ExpDate" HeaderText="Exp.Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="MfgDate" HeaderText="Mfg.Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="AQty" HeaderText="Available Qty" />
                <asp:BoundField DataField="BookFDel" HeaderText="Book For Delv" />
                <%--<asp:BoundField DataField="VatAmount" HeaderText="VatAmount" />--%>
                <asp:BoundField DataField="Tqty" HeaderText="Transit Qty" />
                <asp:BoundField DataField="RQty" HeaderText="Restricted Qty" />
                <%--<asp:BoundField DataField="ReturnReason" HeaderText="ReturnReason" />--%>
                <asp:BoundField DataField="BQty" HeaderText="Blocked Qty" />
                <asp:BoundField DataField="ReturnRcvQty" HeaderText="Return Rcv" />
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
                <asp:BoundField DataField="ReturnRcvQty" HeaderText="Return Rcv" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>

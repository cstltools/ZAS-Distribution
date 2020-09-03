<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProformaHTMLReport.aspx.cs" Inherits="HTMLReport_ProformaHTMLReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../HTML_Report_css/proformatable.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
        
            <br/>
            <br/>
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="../HTML_Report_css/images/excel.png" onclick="ImageButton1_Click" Width="40px"/>
            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
            CssClass="responstable"  >
            <Columns>
                <asp:BoundField DataField="ComUnitCode" HeaderText="DC Code"  ></asp:BoundField>
                <asp:BoundField DataField="ComUnitName" HeaderText="DistributionCenterName" />
                 <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="ProductDescription " />
                <%--<asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="DelivaryInvoiceNo" />--%>
                <%--<asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" />--%>
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                <asp:BoundField DataField="ExpDate" HeaderText="Exp Date" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="TotalPriceVatAmount" HeaderText="Total Vat" />
                <asp:BoundField DataField="DiscountAmount" HeaderText="Discount" />
                <%--<asp:BoundField DataField="VatAmount" HeaderText="VatAmount" />--%>
                <asp:BoundField DataField="SpecialAmount" HeaderText="Special Disc" />
                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" />
                <%--<asp:BoundField DataField="ReturnReason" HeaderText="ReturnReason" />--%>
                <asp:BoundField DataField="CustomerCode" HeaderText="Customer Id" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" />
                <asp:BoundField DataField="OrderNo" HeaderText="Order No" />
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
                <asp:BoundField DataField="AreaCode" HeaderText="Area Code" />
                <asp:BoundField DataField="MiaCode" HeaderText="Mia Code" />
                <asp:BoundField DataField="DistrictCode" HeaderText="Dist Code" />
                <%--<asp:BoundField DataField="RegionCode" HeaderText="RegionCode" />--%>
                <asp:BoundField DataField="MarketCode" HeaderText="Market Code" />
                <asp:BoundField DataField="MarketName" HeaderText="Market Name" />
                                    
                
            </Columns>
           
        </asp:GridView>

    </div>
    </form>
</body>
</html>

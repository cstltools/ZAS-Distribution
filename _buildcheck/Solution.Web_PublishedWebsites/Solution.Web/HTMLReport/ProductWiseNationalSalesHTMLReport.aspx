<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_ProformaHTMLReport, App_Web_0n34y345" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Report</title>
    <%--<link href="../HTML_Report_css/proformatable.css" rel="stylesheet" type="text/css" />--%>
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
    <style type="text/css">
        .alignment {
            width: 60%;
            margin: 20px auto;
            color: #008080;
            text-align: center;
        }

        html, body {
            height: 100%;
        }

        html {
            display: table;
            margin: auto;
        }

        body {
            display: table-cell;
            vertical-align: middle;
        }

        th, td {
            text-align: center;
            padding: 5px;
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
    <h2 class="alignment">National Slaes Report (Product Wise)
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
        <div class="bb" border="1px">
            <br />
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../HTML_Report_css/images/excel.png"
                OnClick="ImageButton1_Click" Width="40Px" />
            <br />
            <span style="color: #b8860b;">Import Excel</span>
            <br />
            <br />
            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="responstable">
                <Columns>
                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name " />
                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                    <asp:BoundField DataField="SalesQuantity" HeaderText="Total Quantity" />
                    <asp:BoundField DataField="SalesValue" HeaderText="Sales Value" />
                </Columns>
            </asp:GridView>

            <br />

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
        <div class="box" border="1px">
            <span style="font-size: 18px; font-weight: bold;">Total Value: </span> <asp:Label ID="totalReceiveableLabel" CssClass="currency" runat="server"></asp:Label>
        </div>
    </form>

    

    <tr>
        <td class="TDLeft" width="13%">&nbsp;
        </td>
        <td class="TDRight" width="20%">&nbsp;
        </td>
        <td class="TDLeft" width="13%">&nbsp;
        </td>
        <td class="TDRight" width="20%">&nbsp;
        </td>
        <td class="TDLeft" width="13%">&nbsp;
        </td>
        <td class="TDRight" width="20%">&nbsp;
        </td>
    </tr>
</body>
</html>

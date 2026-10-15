<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_WHStockValuationHtmlReport, App_Web_2lvt0ebj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DC Stock Valuation Report</title>
    <link rel="shortcut icon" href="../images/image/favicon.ico" />

    <style>
        body{
            font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color:#333;
        }
        .alignment{
            text-align:center;
            margin:10px 0 6px 0;
        }
        .meta{
            width:100%;
            border-collapse:collapse;
            margin-top:6px;
            font-size:13px;
        }
        .meta td{
            padding:4px 6px;
        }

        .styled-grid {
            width: 100%;
            border-collapse: collapse;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 13px;
            color: #333;
        }

        .styled-grid th {
            background-color: #007acc;
            color: white;
            padding: 6px 8px;
            text-align: left;
            border: 1px solid #ccc;
            white-space: nowrap;
        }

        .styled-grid td {
            padding: 5px 8px;
            border: 1px solid #ddd;
            vertical-align: middle;
        }

        .styled-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .styled-grid tr:hover {
            background-color: #f1f7fd;
        }

        .styled-grid tfoot td {
            font-weight: bold;
            background-color: #e6f7ff;
            padding: 5px 8px;
        }

        .num { text-align:right; }
        .tools{ margin:10px 0; }
        .hint{ color:#b8860b; font-size:12px; }
        .nowrap{ white-space:nowrap; }
    </style>
</head>

<body>
    <h2 class="alignment"> DC STOCK VALUATION REPORT </h2>
    <hr />

    <!-- Report Header Info -->
    <table class="meta">
        <tr>
            <td style="width:13%; text-align:right; padding-right:10px;"><b>From:</b></td>
            <td style="width:20%;">
                <asp:Label ID="FromTextBox" runat="server" CssClass="TextBox"></asp:Label>
            </td>

            <td style="width:13%; text-align:right; padding-right:10px;"><b>To:</b></td>
            <td style="width:20%;">
                <asp:Label ID="toTextBox" runat="server" CssClass="TextBox"></asp:Label>
            </td>

            <td style="width:13%; text-align:right; padding-right:10px;"><b>Wings Name:</b></td>
            <td style="width:21%;">
                <asp:Label ID="companyTextBox" runat="server" CssClass="TextBox"></asp:Label>
            </td>
        </tr>

        <tr>
            <td style="text-align:right; padding-right:10px;"><b></b></td>
            <td>
                <asp:Label ID="productTextBox" Visible="False"  runat="server" CssClass="TextBox"></asp:Label>
            </td>

            <td style="text-align:right; padding-right:10px;"><b></b></td>
            <td>
                <asp:Label ID="warehouseTextBox" runat="server" Visible="False" CssClass="TextBox"></asp:Label>
            </td>

            <td></td>
            <td></td>
        </tr>
    </table>

    <form id="form1" runat="server">
        <div class="tools">
            <asp:ImageButton ID="ImageButton1" runat="server"
                ImageUrl="../HTML_Report_css/images/excel.png"
                OnClick="ImageButton1_Click" Width="40px" />
            <div class="hint">Export Excel</div>
        </div>

        <asp:GridView ID="loadGridView" runat="server"
            AutoGenerateColumns="False"
            ShowFooter="True"
            CssClass="styled-grid"
            OnRowDataBound="loadGridView_RowDataBound">

            <Columns>
                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />

                <asp:BoundField DataField="OpeningQty" HeaderText="Opening Qty" DataFormatString="{0:N3}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>
                <asp:BoundField DataField="OpeningRate" HeaderText="Opening Rate" DataFormatString="{0:N4}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>
                <asp:BoundField DataField="OpeningValue" HeaderText="Opening Value" DataFormatString="{0:N2}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>

                <asp:BoundField DataField="InQty" HeaderText="In Qty" DataFormatString="{0:N3}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>
                <asp:BoundField DataField="OutQty" HeaderText="Out Qty" DataFormatString="{0:N3}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>

                <asp:BoundField DataField="ClosingQty" HeaderText="Closing Qty" DataFormatString="{0:N3}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>
                <asp:BoundField DataField="ClosingRate" HeaderText="Closing Rate" DataFormatString="{0:N4}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>
                <asp:BoundField DataField="ClosingValue" HeaderText="Closing Value" DataFormatString="{0:N2}">
                    <ItemStyle CssClass="num" />
                    <FooterStyle CssClass="num" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>

        <br />
        <!-- Optional Total Summary -->
        <div style="text-align:right; font-weight:bold; margin-top:6px;">
            Total Closing Value:&nbsp;
            <asp:Label ID="lblTotalClosingValue" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>

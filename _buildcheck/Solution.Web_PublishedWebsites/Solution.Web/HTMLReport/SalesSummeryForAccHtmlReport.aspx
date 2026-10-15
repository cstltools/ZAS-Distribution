<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_SalesSummeryForAccHtmlReport, App_Web_zyd4su1o" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Salesroll Summery Report For ACC </title>
    <%--<link href="../HTML_Report_css/proformatable.css" rel="stylesheet" type="text/css" />--%>
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
    <style>
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
        padding: 6px 8px; /* reduced padding */
        text-align: left;
        border: 1px solid #ccc;
    }

    .styled-grid td {
        padding: 5px 8px; /* reduced padding */
        border: 1px solid #ddd;
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
             <%--<span style="font-weight: bold">DC: </span>--%>
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
        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
    CssClass="styled-grid" OnRowDataBound="loadGridView_RowDataBound">
    <Columns>
        <asp:BoundField DataField="CompanyCode" HeaderText="Company Code" />
        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
        <asp:BoundField DataField="ComUnitCode" HeaderText="Depot Code" />
        <asp:BoundField DataField="ComUnitName" HeaderText="Depot Name" />

        <asp:BoundField DataField="OpeningDue" HeaderText="Opening Due" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="SalesAmount" HeaderText="Sales Amount" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="ReturnAmount" HeaderText="Return Amount" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="AIT" HeaderText="AIT" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="ClosingDue" HeaderText="Closing Due" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="SampleExpense" HeaderText="Sample Expense" DataFormatString="{0:N2}" />
    </Columns>
</asp:GridView>


    </div>
    
    <br />
    
<%--    <div class="box" >
            <span style="font-size: 18px; font-weight: bold;">Total Value: </span> <asp:Label ID="totalReceiveableLabel" CssClass="currency" runat="server"></asp:Label>
        </div>--%>

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

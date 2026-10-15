<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_SalesPerformanceReport, App_Web_2lvt0ebj" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Performance Report</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 12px;
            margin: 0;
            padding: 0;
        }

        h2 {
            text-align: center;
            color: #006666;
            margin: 10px 0;
        }

        .date-range {
            text-align: center;
            font-weight: bold;
            margin-bottom: 10px;
        }

        table {
            width: 98%;
            margin: 0 auto 20px auto;
            border-collapse: collapse;
        }

        table th, table td {
            border: 1px solid #999;
            padding: 5px 8px;
        }

        table th {
            background: #f2f2f2;
            text-align: center;
            font-weight: bold;
        }

        .left { text-align: left; }
        .right { text-align: right; }

        .group-header {
            background: #e8f4f8;
            font-weight: bold;
        }

        .subtotal {
            background: #d9edf7;
            font-weight: bold;
        }

        .grand-total {
            background: #f0ad4e;
            color: #fff;
            font-weight: bold;
        }

        .btn-export {
            display: block;
            margin: 10px auto;
            padding: 6px 14px;
            background: #006666;
            color: #fff;
            border: none;
            cursor: pointer;
            font-size: 13px;
        }

        .btn-export:hover {
            background: #004d4d;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

    <h2>Sales Performance Report</h2>

    <div class="date-range">
        <asp:Literal ID="litDateRange" runat="server"></asp:Literal>
    </div>

   <%-- <asp:Button ID="btnExportExcel" runat="server"
        Text="Download as Excel"
        CssClass="btn-export"
        OnClick="btnExportExcel_Click" />--%>

    <asp:Literal ID="litReport" runat="server"></asp:Literal>

</form>
</body>
</html>

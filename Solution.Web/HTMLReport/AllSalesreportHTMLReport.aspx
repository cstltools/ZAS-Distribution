<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllSalesreportHTMLReport.aspx.cs"
    Inherits="HTMLReport_AllSalesreportHTMLReport" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>All Sales & Delivery Report</title>
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
    <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

        body {
            font-family: 'Inter', sans-serif;
            background-color: #f1f5f9;
            color: #1e293b;
            margin: 0;
            padding: 20px;
        }

        .report-container {
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.05), 0 2px 4px -2px rgb(0 0 0 / 0.05);
            padding: 24px;
            margin: 0 auto;
        }

        .report-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 2px solid #e2e8f0;
            padding-bottom: 16px;
            margin-bottom: 16px;
        }

        .header-left h1 {
            font-size: 22px;
            font-weight: 700;
            color: #0f172a;
            margin: 0 0 6px 0;
        }

        .meta-info {
            display: flex;
            gap: 20px;
            font-size: 13px;
            color: #64748b;
            flex-wrap: wrap;
        }

        .meta-item {
            display: flex;
            align-items: center;
            gap: 6px;
        }

        .meta-label {
            font-weight: 600;
            color: #475569;
        }

        .header-right {
            display: flex;
            gap: 10px;
            align-items: center;
        }

        /* Action Buttons */
        .btn-action {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 8px 16px;
            border-radius: 6px;
            font-size: 13px;
            font-weight: 600;
            border: none;
            cursor: pointer;
            transition: all 0.2s ease;
            text-decoration: none;
        }

        .btn-pdf {
            background-color: #0284c7;
            color: #ffffff !important;
        }

        .btn-pdf:hover {
            background-color: #0369a1;
            transform: translateY(-1px);
        }

        .btn-excel-wrapper {
            position: relative;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background-color: #16a34a;
            color: #ffffff !important;
            padding: 8px 16px;
            border-radius: 6px;
            font-size: 13px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
            border: none;
        }

        .btn-excel-wrapper:hover {
            background-color: #15803d;
            transform: translateY(-1px);
        }

        .btn-excel-wrapper input[type="submit"] {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            opacity: 0;
            cursor: pointer;
            z-index: 10;
        }

        /* Table Styling */
        .table-responsive {
            overflow-x: auto;
            margin-top: 15px;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            background: #ffffff;
        }

        .responstable {
            width: 100%;
            border-collapse: collapse;
            font-size: 12px;
            white-space: nowrap;
        }

        .responstable th {
            background-color: #1e293b;
            color: #ffffff;
            font-weight: 600;
            padding: 10px 12px;
            border: 1px solid #0f172a;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            text-align: center;
            position: sticky;
            top: 0;
            z-index: 10;
        }

        .responstable td {
            padding: 8px 10px;
            border: 1px solid #e2e8f0;
            color: #0f172a;
            font-weight: 500;
        }

        .responstable tr:nth-child(even) td {
            background-color: #f8fafc;
        }

        .responstable tr:nth-child(odd) td {
            background-color: #ffffff;
        }

        .responstable tr:hover td {
            background-color: #e2e8f0;
        }

        .responstable tfoot td {
            background-color: #e2e8f0;
            color: #0f172a;
            font-weight: 700;
            border-top: 2px solid #94a3b8;
            border-bottom: 2px solid #94a3b8;
            padding: 10px 10px;
        }

        /* Status Badges */
        .status-badge {
            padding: 3px 8px;
            border-radius: 12px;
            font-size: 11px;
            font-weight: 600;
            display: inline-block;
            text-align: center;
        }
        .status-full, .status-delivered {
            background-color: #d1e7dd;
            color: #0f5132;
            border: 1px solid #badbcc;
        }
        .status-transit {
            background-color: #fff3cd;
            color: #664d03;
            border: 1px solid #ffecb5;
        }
        .status-partial {
            background-color: #cff4fc;
            color: #055160;
            border: 1px solid #b6effb;
        }
        .status-reject {
            background-color: #f8d7da;
            color: #842029;
            border: 1px solid #f5c2c7;
        }

        .text-right { text-align: right !important; }
        .text-center { text-align: center !important; }
        .text-left { text-align: left !important; }

        .empty-template {
            padding: 24px;
            font-size: 15px;
            color: #64748b;
            font-weight: 500;
            text-align: center;
        }

        /* Print Stylesheet */
        @media print {
            @page {
                size: landscape;
                margin: 5mm;
            }
            body {
                background-color: #ffffff;
                padding: 0;
                color: #000000;
            }
            .report-container {
                box-shadow: none;
                padding: 0;
                width: 100% !important;
            }
            .header-right {
                display: none !important;
            }
            .table-responsive {
                border: none;
                overflow: visible !important;
            }
            .responstable {
                zoom: 70%;
            }
            .responstable th {
                background-color: #f1f5f9 !important;
                color: #000000 !important;
                border: 1px solid #cbd5e1 !important;
                font-size: 8px !important;
                padding: 4px 5px !important;
            }
            .responstable td {
                color: #000000 !important;
                border: 1px solid #cbd5e1 !important;
                font-size: 8px !important;
                padding: 4px 5px !important;
            }
            .responstable tfoot td {
                background-color: #cbd5e1 !important;
                color: #000000 !important;
                font-weight: bold !important;
                font-size: 8.5px !important;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="report-container">
        
        <!-- Header Section -->
        <div class="report-header">
            <div class="header-left">
                <h1>All Sales & Delivery Report</h1>
                <div class="meta-info">
                    <div class="meta-item">
                        <span class="meta-label">From:</span>
                        <asp:Label ID="FromTextBox" runat="server"></asp:Label>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">To:</span>
                        <asp:Label ID="toTextBox" runat="server"></asp:Label>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Total Records:</span>
                        <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                </div>
            </div>
            
            <div class="header-right">
                <!-- Export to Excel wrapper -->
                <div class="btn-excel-wrapper">
                    <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                        <path d="M5.884 6.68a.5.5 0 1 0-.768.64L7.349 10l-2.233 2.68a.5.5 0 0 0 .768.64L8 10.781l2.116 2.54a.5.5 0 0 0 .768-.641L8.651 10l2.233-2.68a.5.5 0 0 0-.768-.64L8 9.219l-2.116-2.54z"/>
                        <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2zM9.5 3A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5v2z"/>
                    </svg>
                    <span>Export to Excel</span>
                    <asp:Button ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" />
                </div>
            </div>
        </div>

        <!-- GridView Section -->
        <div class="table-responsive">
            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                CssClass="responstable" ShowFooter="True" OnRowDataBound="loadGridView_RowDataBound">
                <Columns>
                    <%-- 0. Depot Code --%>
                    <asp:BoundField DataField="DepotCode" HeaderText="Depot Code" ItemStyle-CssClass="text-center" />
                    <%-- 1. Depot Name --%>
                    <asp:BoundField DataField="DepotName" HeaderText="Depot Name" ItemStyle-CssClass="text-left" />

                    <%-- 2. Order No --%>
                    <asp:BoundField DataField="OrderNo" HeaderText="Order No" ItemStyle-CssClass="text-center" />
                    <%-- 3. Order Date --%>
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" ItemStyle-CssClass="text-center" />

                    <%-- 4. Customer Code --%>
                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" ItemStyle-CssClass="text-center" />
                    <%-- 5. Customer Name --%>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-CssClass="text-left" />
                    <%-- 6. Customer Type --%>
                    <asp:BoundField DataField="CustomerType" HeaderText="Customer Type" ItemStyle-CssClass="text-center" />
                    <%-- 7. Market Code --%>
                    <asp:BoundField DataField="MarketCode" HeaderText="Market Code" ItemStyle-CssClass="text-center" />
                    <%-- 8. Market Name --%>
                    <asp:BoundField DataField="MarketName" HeaderText="Market Name" ItemStyle-CssClass="text-left" />
                    <%-- 9. Territory Code --%>
                    <asp:BoundField DataField="TerritoryCode" HeaderText="Territory Code" ItemStyle-CssClass="text-center" />
                    <%-- 10. Territory Name --%>
                    <asp:BoundField DataField="TerritoryName" HeaderText="Territory Name" ItemStyle-CssClass="text-left" />
                    <%-- 11. Area Code --%>
                    <asp:BoundField DataField="AreaCode" HeaderText="Area Code" ItemStyle-CssClass="text-center" />
                    <%-- 12. Area Name --%>
                    <asp:BoundField DataField="AreaName" HeaderText="Area Name" ItemStyle-CssClass="text-left" />
                    <%-- 13. Region Code --%>
                    <asp:BoundField DataField="RegionCode" HeaderText="Region Code" ItemStyle-CssClass="text-center" />
                    <%-- 14. Region Name --%>
                    <asp:BoundField DataField="RegionName" HeaderText="Region Name" ItemStyle-CssClass="text-left" />
                    <%-- 15. MIO Code --%>
                    <asp:BoundField DataField="MiaCode" HeaderText="MIO Code" ItemStyle-CssClass="text-center" />
                    <%-- 16. MIO Name --%>
                    <asp:BoundField DataField="MiaName" HeaderText="MIO Name" ItemStyle-CssClass="text-left" />

                    <%-- 17. Product Code --%>
                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" ItemStyle-CssClass="text-center" />
                    <%-- 18. Product Name --%>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-CssClass="text-left" />
                    <%-- 19. Pack Size --%>
                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size" ItemStyle-CssClass="text-center" />
                    <%-- 20. Batch No --%>
                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No" ItemStyle-CssClass="text-center" />
                    <%-- 21. Exp Date --%>
                    <asp:BoundField DataField="ExpDate" HeaderText="Exp Date" ItemStyle-CssClass="text-center" />
                    <%-- 22. Unit Price --%>
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />

                    <%-- 23. Invoice No --%>
                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" ItemStyle-CssClass="text-center" />
                    <%-- 24. Invoice Date --%>
                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" ItemStyle-CssClass="text-center" />
                    <%-- 25. Invoiced Qty --%>
                    <asp:BoundField DataField="InvoicedQty" HeaderText="Invoiced Qty" DataFormatString="{0:N0}" ItemStyle-CssClass="text-right" />
                    <%-- 26. Invoice Gross --%>
                    <asp:BoundField DataField="InvoiceGrossAmount" HeaderText="Invoice Gross" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 27. Inv Trade Disc --%>
                    <asp:BoundField DataField="InvoiceTradeDiscount" HeaderText="Inv Trade Disc" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 28. Inv Special Disc --%>
                    <asp:BoundField DataField="InvoiceSpecialDiscount" HeaderText="Inv Special Disc" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 29. Inv VAT --%>
                    <asp:BoundField DataField="InvoiceVATAmount" HeaderText="Inv VAT" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 30. Inv Net Amount --%>
                    <asp:BoundField DataField="InvoiceNetAmount" HeaderText="Inv Net Amount" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />

                    <%-- 31. Delivery Status --%>
                    <asp:TemplateField HeaderText="Delivery Status" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <span class='status-badge <%# GetStatusClass(Eval("DeliveryStatus")) %>'>
                                <%# Eval("DeliveryStatus") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 32. Transit Aging (Days) --%>
                    <asp:BoundField DataField="TransitAgingDays" HeaderText="Transit Aging (Days)" ItemStyle-CssClass="text-center" />
                    <%-- 33. In-Transit Qty --%>
                    <asp:BoundField DataField="InTransitQty" HeaderText="In-Transit Qty" DataFormatString="{0:N0}" ItemStyle-CssClass="text-right" />

                    <%-- 34. Delivery Inv No --%>
                    <asp:BoundField DataField="DeliveryInvoiceNo" HeaderText="Delivery Inv No" ItemStyle-CssClass="text-center" />
                    <%-- 35. Delivery Conf. Date --%>
                    <asp:BoundField DataField="DeliveryConfirmationDate" HeaderText="Delivery Conf. Date" ItemStyle-CssClass="text-center" />
                    <%-- 36. Delivered Qty --%>
                    <asp:BoundField DataField="DeliveredQty" HeaderText="Delivered Qty" DataFormatString="{0:N0}" ItemStyle-CssClass="text-right" />
                    <%-- 37. Delivered Gross --%>
                    <asp:BoundField DataField="DeliveryGrossAmount" HeaderText="Delivered Gross" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 38. Delv Trade Disc --%>
                    <asp:BoundField DataField="DeliveryTradeDiscount" HeaderText="Delv Trade Disc" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 39. Delv Special Disc --%>
                    <asp:BoundField DataField="DeliverySpecialDiscount" HeaderText="Delv Special Disc" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 40. Delv VAT --%>
                    <asp:BoundField DataField="DeliveryVATAmount" HeaderText="Delv VAT" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                    <%-- 41. Delivered Net Sales --%>
                    <asp:BoundField DataField="DeliveredNetAmount" HeaderText="Delivered Net Sales" DataFormatString="{0:N2}" ItemStyle-CssClass="text-right" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-template">No records found.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>

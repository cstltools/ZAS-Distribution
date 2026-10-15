<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_NationalStockReportHTMLReport, App_Web_2lvt0ebj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>National Stock Report - HTML Preview</title>
    <link rel="shortcut icon" href="../images/image/favicon.ico" />
    <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

        body {
            font-family: 'Inter', sans-serif;
            background-color: #f1f5f9;
            color: #1e293b;
            margin: 0;
            padding: 24px;
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
            padding-bottom: 20px;
            margin-bottom: 20px;
        }

        .header-left h1 {
            font-size: 24px;
            font-weight: 700;
            color: #0f172a;
            margin: 0 0 8px 0;
        }

        .meta-info {
            display: flex;
            gap: 24px;
            font-size: 14px;
            color: #64748b;
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
            gap: 12px;
            align-items: center;
        }

        /* Styled Action Buttons */
        .btn-action {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 10px 18px;
            border-radius: 6px;
            font-size: 14px;
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
            gap: 8px;
            background-color: #16a34a;
            color: #ffffff !important;
            padding: 10px 18px;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-excel-wrapper:hover {
            background-color: #15803d;
            transform: translateY(-1px);
        }

        .btn-excel-wrapper input[type="image"] {
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
        }

        .responstable {
            width: 100%;
            border-collapse: collapse;
            font-size: 13px;
            text-align: center;
        }

        .responstable th {
            background-color: #1e293b;
            color: #ffffff;
            font-weight: 600;
            padding: 12px 14px;
            border-bottom: 2px solid #0f172a;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            white-space: nowrap;
        }

        .responstable td {
            padding: 10px 14px;
            border-bottom: 1px solid #e2e8f0;
            color: #0f172a; /* High contrast dark text */
            white-space: nowrap;
            font-weight: 500;
        }

        /* Zebra striping (Even/Odd Row pattern) */
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
            font-weight: bold;
            border-top: 2px solid #94a3b8;
            border-bottom: 2px solid #94a3b8;
            padding: 10px 14px;
        }

        .empty-template {
            padding: 24px;
            font-size: 16px;
            color: #64748b;
            font-weight: 500;
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
                max-width: 100% !important;
            }
            .header-right {
                display: none !important;
            }
            .table-responsive {
                border: none;
                overflow: visible !important;
                width: 100% !important;
                max-width: 100% !important;
            }
            .responstable {
                width: 100% !important;
                table-layout: auto !important;
                zoom: 70%; /* Scale down to fit landscape page nicely */
            }
            .responstable th {
                background-color: #f1f5f9 !important;
                color: #000000 !important;
                border: 1px solid #cbd5e1 !important;
                font-size: 6.5px !important;
                padding: 3px 4px !important;
                white-space: normal !important; /* Allow headers to wrap */
                line-height: 1.1 !important;
                font-weight: bold !important;
            }
            .responstable td {
                color: #000000 !important;
                border: 1px solid #cbd5e1 !important;
                font-size: 6.5px !important;
                padding: 3px 4px !important;
                white-space: nowrap !important; /* Prevent numbers from breaking */
            }
            /* Enforce zebra striping on print */
            .responstable tr:nth-child(even) td {
                background-color: #f8fafc !important;
            }
            .responstable tr:nth-child(odd) td {
                background-color: #ffffff !important;
            }
            .responstable tfoot td {
                background-color: #cbd5e1 !important;
                color: #000000 !important;
                font-weight: bold !important;
                border-top: 2px solid #475569 !important;
                border-bottom: 2px solid #475569 !important;
                font-size: 7px !important;
                padding: 3px 4px !important;
            }
            /* Allow description column to wrap and give it moderate width */
            .responstable td:nth-child(2), .responstable th:nth-child(2) {
                white-space: normal !important;
                min-width: 100px !important;
                max-width: 140px !important;
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
                <h1>National Stock Report</h1>
                <div class="meta-info">
                    <div class="meta-item">
                        <span class="meta-label">Company:</span>
                        <asp:Label ID="companyTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                    </div>
                </div>
            </div>
            
            <div class="header-right">
                <!-- Export to Excel wrapper -->
                <div class="btn-excel-wrapper">
                    <!-- Modern Excel Icon -->
                    <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                        <path d="M5.884 6.68a.5.5 0 1 0-.768.64L7.349 10l-2.233 2.68a.5.5 0 0 0 .768.64L8 10.781l2.116 2.54a.5.5 0 0 0 .768-.641L8.651 10l2.233-2.68a.5.5 0 0 0-.768-.64L8 9.219l-2.116-2.54z"/>
                        <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2zM9.5 3A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5v2z"/>
                    </svg>
                    <span>Export Excel</span>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../HTML_Report_css/images/excel.png"
                        OnClick="ImageButton1_Click" />
                </div>
                
                <!-- Print to PDF button -->
                <button type="button" class="btn-action btn-pdf" onclick="window.print();">
                    <!-- Modern Printer Icon -->
                    <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                        <path d="M2.5 8a.5.5 0 1 0 0-1 .5.5 0 0 0 0 1z"/>
                        <path d="M5 1a2 2 0 0 0-2 2v2H2a2 2 0 0 0-2 2v3a2 2 0 0 0 2 2h1v1a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2v-1h1a2 2 0 0 0 2-2V7a2 2 0 0 0-2-2h-1V3a2 2 0 0 0-2-2H5zM4 3a1 1 0 0 1 1-1h6a1 1 0 0 1 1 1v2H4V3zm1 5a2 2 0 0 0-2 2v1H2a1 1 0 0 1-1-1V7a1 1 0 0 1 1-1h12a1 1 0 0 1 1 1v3a1 1 0 0 1-1 1h-1v-1a2 2 0 0 0-2-2H5zm7 2v3a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1v-3a1 1 0 0 1 1-1h6a1 1 0 0 1 1 1z"/>
                    </svg>
                    Print to PDF
                </button>
            </div>
        </div>

        <!-- GridView Section -->
        <div class="table-responsive">
            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="responstable" ShowFooter="True">
                <Columns>
                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Description" />
                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                    <asp:BoundField DataField="AQty" HeaderText="Available Qty" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="BookFDel" HeaderText="Book For Delv" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="Tqty" HeaderText="Transit Qty" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="RQty" HeaderText="Restricted Qty" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="BQty" HeaderText="Blocked Qty" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="WHQty" HeaderText="WH Qty" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="FZQty" HeaderText="Freeze Qty" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="ReturnRcvQty" HeaderText="Return Rcv" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="TotalQty" HeaderText="Total Qty" DataFormatString="{0:N0}" />
                    
                    <asp:BoundField DataField="AvialableValue" HeaderText="Available Value" DataFormatString="{0:N4}" />
                    <asp:BoundField DataField="RestrictedValue" HeaderText="Restricted Value" DataFormatString="{0:N4}" />
                    <asp:BoundField DataField="TransitValue" HeaderText="Transit Value" DataFormatString="{0:N4}" />
                    <asp:BoundField DataField="BlockedValue" HeaderText="Blocked Value" DataFormatString="{0:N4}" />
                    <asp:BoundField DataField="WhStockValue" HeaderText="WH Stock Value" DataFormatString="{0:N4}" />
                    <asp:BoundField DataField="ReturnRcvValue" HeaderText="Return Rcv Value" DataFormatString="{0:N4}" />
                    <asp:BoundField DataField="Avg_Rate" HeaderText="Landing Price" DataFormatString="{0:F4}" />
                    <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total Value" DataFormatString="{0:N4}" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-template" align="center">No records found.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
        
    </div>
    </form>
</body>
</html>

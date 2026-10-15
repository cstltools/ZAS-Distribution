<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupInvoiceReport.aspx.cs" Inherits="SInventory_RPTVIEW_GroupInvoiceReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Group Invoice Report</title>
    <style>
        * { box-sizing: border-box; }
        body { font-family: 'Times New Roman', Georgia, serif; font-size: 11px; color: #000; background: #ddd; margin: 0; }

        .report-toolbar {
            background: #2f3640; padding: 10px 20px; text-align: right; font-family: Calibri, Arial, sans-serif;
        }
        .report-toolbar button, .report-toolbar input[type=submit] {
            padding: 6px 16px; margin-left: 8px; cursor: pointer; border: 1px solid #888; border-radius: 3px; background: #fff; font-family: Calibri, Arial, sans-serif;
        }

        .page {
            width: 210mm; min-height: 297mm; margin: 20px auto; background: #fff; padding: 1in 10mm; /* 1in header + footer gap */
            border: 1px solid #ccc; box-shadow: 0 0 6px rgba(0,0,0,.2);
        }
        .page.pdf-mode { padding: 0 10mm; } /* PDF export adds the 1in gap per page itself */

        /* --- "Invoice" title: centered dotted box, matches the original Crystal banner --- */
        .invoice-title-wrap { text-align: center; margin-bottom: 10px; }
        .invoice-title {
            display: inline-block; padding: 2px 40px; border-top: 1px dotted #000; border-bottom: 1px dotted #000;
            font-style: italic; font-size: 13px;
        }

        /* --- Sold To / three-column info block --- */
        .sold-to-label { font-style: italic; text-decoration: underline; font-weight: bold; margin-bottom: 2px; }
        .sold-to-block { border-bottom: 1px solid #000; padding-bottom: 6px; margin-bottom: 8px; }
        .header-grid { display: flex; justify-content: space-between; }
        .header-grid > div { padding-right: 8px; }
        .col-soldto { width: 30%; }
        .col-mid { width: 34%; }
        .col-right { width: 36%; }
        .header-grid label { font-weight: bold; display: inline-block; min-width: 95px; }
        .col-right label { min-width: 80px; }
        .header-grid div div { margin-bottom: 1px; }

        /* --- Reference No line --- */
        .ref-block { margin-bottom: 6px; }
        .reference-no { margin: 8px 0 2px 0; border-bottom: 1px solid #000; padding-bottom: 2px; }
        .ref-label { font-style: italic; text-decoration: underline; font-weight: bold; }

        /* --- Product table: compact, thin borders, multi-level header, narrow rotated columns --- */
        table.product-table { width: 100%; border-collapse: collapse; table-layout: fixed; margin-bottom: 4px; }
        table.product-table th, table.product-table td { border: 1px solid #000; padding: 2px 3px; font-size: 10px; vertical-align: middle; }
        table.product-table thead th { background: #fff; font-weight: bold; text-align: center; }
        table.product-table thead { display: table-header-group; } /* repeat header on print page breaks */
        table.product-table td.center { text-align: center; }
        table.product-table td.num, table.product-table th.num { text-align: right; }
        table.product-table td.right, table.product-table th.right { text-align: right; }
        table.product-table td.prod-name { text-align: left; }
        table.product-table th.vert, table.product-table td.vert {
            writing-mode: vertical-rl; text-align: center; white-space: nowrap; width: 5%;
        }
        table.product-table tr.subtotal-row td, table.product-table tr.grand-total-row td { background: #f2f2f2; }
        table.grand-total-table { margin-top: 4px; }

        .below-total { margin-top: 8px; }
        .in-words, .remarks { margin-bottom: 4px; }

        /* --- Signature block: four columns with a dashed line and label, matching the original --- */
        .signature-block { display: flex; justify-content: space-between; margin-top: 50px; }
        .sig-col { width: 24%; text-align: center; }
        .sig-value { min-height: 14px; font-size: 10px; }
        .sig-line { border-top: 1px dashed #000; margin-top: 14px; }
        .sig-label { font-weight: bold; font-size: 10px; margin-top: 2px; }

        .error-box { max-width: 600px; margin: 60px auto; background: #fff; border: 1px solid #c00; padding: 20px; text-align: center; color: #900; font-family: Calibri, Arial, sans-serif; }

        @page {
            size: A4 portrait;
            margin: 1in 10mm; /* 1in header + footer gap on every printed page */
            /* CSS Paged Media page numbering, per spec. Not rendered by mainstream browsers today
               (Chrome/Firefox print engines do not implement @page margin boxes) — included as a
               standards-based best effort per the "where technically possible" requirement; this is
               documented as unverified/unsupported in the audit rather than claimed to work. */
            @bottom-center { content: "Page " counter(page) " of " counter(pages); }
        }
        @media print {
            .report-toolbar { display: none !important; }
            body { background: #fff; }
            .page { border: none; box-shadow: none; margin: 0; width: auto; min-height: auto; padding: 0 10mm; }
            .ref-block { page-break-inside: avoid; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="pdfFileNameHidden" runat="server" />

    <div class="report-toolbar">
        <button type="button" onclick="window.print();">Print</button>
        <button type="button" onclick="downloadPdf();">Download PDF</button>
        <asp:Button ID="exportExcelButton" runat="server" Text="Download Excel" OnClick="exportExcelButton_Click" />
        <button type="button" onclick="window.close();">Close</button>
    </div>

    <asp:Label ID="errorLabel" runat="server" CssClass="error-box" Visible="false" />

    <asp:Panel ID="reportPanel" runat="server">
    <div class="page" id="reportArea">
        <asp:Literal ID="reportBodyLiteral" runat="server" />
    </div>
    </asp:Panel>

    </form>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
    <script>
        function downloadPdf() {
            // Known limitation (documented, not fixed here per scope): this snapshots the whole report as
            // one tall image and slices it by pixel height across A4 pages. It does not know where table
            // rows or sections begin/end, so on a long invoice a row (or a Reference No + table header)
            // can be split across a page boundary. Achieving row-aware pagination would require generating
            // the PDF from structured data (a real PDF/table library) rather than a canvas screenshot,
            // which is a much larger change than this fix pass and is not justified by a new dependency.
            if (typeof html2canvas === 'undefined' || !(window.jspdf && window.jspdf.jsPDF)) {
                alert('PDF generation is unavailable right now (the PDF library did not load, possibly a network/CDN issue). Please check your connection and try again, or use Print instead.');
                return;
            }

            var el = document.getElementById('reportArea');
            var fileName = document.getElementById('pdfFileNameHidden').value || 'GroupInvoice.pdf';

            el.classList.add('pdf-mode');
            html2canvas(el, { scale: 2 }).then(function (canvas) {
                el.classList.remove('pdf-mode');
                var pdf = new window.jspdf.jsPDF('p', 'mm', 'a4');
                var pageWidth = pdf.internal.pageSize.getWidth();
                var margin = 25.4; // 1 inch header + footer gap on every page
                var pxPerMm = canvas.width / pageWidth;
                var slicePx = Math.floor((pdf.internal.pageSize.getHeight() - 2 * margin) * pxPerMm);

                for (var y = 0; y < canvas.height - 1; y += slicePx) {
                    var h = Math.min(slicePx, canvas.height - y);
                    var slice = document.createElement('canvas');
                    slice.width = canvas.width;
                    slice.height = h;
                    slice.getContext('2d').drawImage(canvas, 0, y, canvas.width, h, 0, 0, canvas.width, h);
                    if (y > 0) pdf.addPage();
                    pdf.addImage(slice.toDataURL('image/png'), 'PNG', 0, margin, pageWidth, h / pxPerMm);
                }

                pdf.save(fileName);
            }).catch(function () {
                el.classList.remove('pdf-mode');
                alert('Could not generate the PDF. Please try Print instead.');
            });
        }
    </script>
</body>
</html>

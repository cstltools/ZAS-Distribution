<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardPanel.aspx.cs" Inherits="SInventory_UI_DashboardPanel" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SalesRoll | Dashboard </title>
    <link href="../assets/css/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../assets/css/plugins/daterangepicker.css">
    <style>
        .highcharts-figure, .highcharts-data-table table {
            min-width: 310px;
            max-width: 1200px;
            margin: 1em auto;
        }

        #container {
            height: 400px;
        }

        /*#product-chart-container {
            height: 500px;
        }*/

        .btn.btn-icon {
            width: 40px;
            line-height: 30px;
            height: 40px;
            padding: 3px;
            text-align: center;
        }

        .btn-outline-success {
            color: #20a98e;
            background-color: #fff;
            background-color: transparent;
        }

        .center {
            position: absolute;
            display: block;
            margin-left: auto;
            margin-right: auto;
            top: 40%;
            left: 45%;
        }

        #dc-chart-container {
            height: 500px;
        }

        .highcharts-data-table table {
            font-family: Verdana, sans-serif;
            border-collapse: collapse;
            border: 1px solid #EBEBEB;
            margin: 10px auto;
            text-align: center;
            width: 100%;
            max-width: 500px;
        }

        .highcharts-data-table caption {
            padding: 1em 0;
            font-size: 1.2em;
            color: #555;
        }

        .highcharts-data-table th {
            font-weight: 600;
            padding: 0.5em;
        }

        .highcharts-data-table td, .highcharts-data-table th, .highcharts-data-table caption {
            padding: 0.5em;
        }

        .highcharts-data-table thead tr, .highcharts-data-table tr:nth-child(even) {
            background: #f8f8f8;
        }

        .highcharts-data-table tr:hover {
            background: #f1f7ff;
        }

        table {
            text-align: center !important;
            border: 1px solid #E2E5E8 !important;
        }

        .table thead th {
            background-color: #7a9ebd !important;
            font-size: 11px !important;
            color: #fff !important;
        }

        .table.table-xs th {
            padding: 0.3rem 2rem !important;
        }

        .table tbody tr {
            border: 1px solid #6b799c !important;
        }

        .table tbody td {
            font-size: 11px !important;
        }

        .table > tbody > tr:not(th):nth-child(even) {
            background-color: #d7e3ee !important;
        }

        .table.table-xs td {
            padding: 0.1rem 2rem !important;
        }



        .primary-breadcrumb, .inverse-breadcrumb, .danger-breadcrumb, .info-breadcrumb, .warning-breadcrumb, .success-breadcrumb {
            background: linear-gradient(45deg, #6B799C, #73b4ff);
            color: #fff;
        }

            .primary-breadcrumb h5, .inverse-breadcrumb h5, .danger-breadcrumb h5, .info-breadcrumb h5, .warning-breadcrumb h5, .success-breadcrumb h5, .primary-breadcrumb a, .inverse-breadcrumb a, .danger-breadcrumb a, .info-breadcrumb a, .warning-breadcrumb a, .success-breadcrumb a, .primary-breadcrumb .breadcrumb-title li:last-child a, .inverse-breadcrumb .breadcrumb-title li:last-child a, .danger-breadcrumb .breadcrumb-title li:last-child a, .info-breadcrumb .breadcrumb-title li:last-child a, .warning-breadcrumb .breadcrumb-title li:last-child a, .success-breadcrumb .breadcrumb-title li:last-child a, .primary-breadcrumb .breadcrumb-item + .breadcrumb-item::before, .inverse-breadcrumb .breadcrumb-item + .breadcrumb-item::before, .danger-breadcrumb .breadcrumb-item + .breadcrumb-item::before, .info-breadcrumb .breadcrumb-item + .breadcrumb-item::before, .warning-breadcrumb .breadcrumb-item + .breadcrumb-item::before, .success-breadcrumb .breadcrumb-item + .breadcrumb-item::before {
                color: #fff;
            }

            .caption-breadcrumb .breadcrumb-header span, .primary-breadcrumb .breadcrumb-header span, .inverse-breadcrumb .breadcrumb-header span, .danger-breadcrumb .breadcrumb-header span, .info-breadcrumb .breadcrumb-header span, .warning-breadcrumb .breadcrumb-header span, .success-breadcrumb .breadcrumb-header span {
                display: block;
                font-size: 13px;
                margin-top: 5px;
            }



        .borderless-card {
            width: 100%;
        }

        .card .card-block {
            padding: 25px;
        }

        .caption-breadcrumb .breadcrumb-header, .primary-breadcrumb .breadcrumb-header, .inverse-breadcrumb .breadcrumb-header, .danger-breadcrumb .breadcrumb-header, .info-breadcrumb .breadcrumb-header, .warning-breadcrumb .breadcrumb-header, .success-breadcrumb .breadcrumb-header {
            display: inline-block;
        }

        .page-header-breadcrumb {
            float: right;
        }

        .breadcrumb-item {
            float: left;
        }

        li {
            display: list-item;
            text-align: -webkit-match-parent;
        }

        ul {
            padding-left: 0;
            list-style-type: none;
            margin-bottom: 0;
        }

        .display-block {
            width: 100%;
            display: block;
        }


        .main-card h5 {
            font-size: 16px !important;
        }

        .card-header {
            background-color: #D7E3EE;
        }

        .pcoded-content {
            position: relative;
            display: block;
            padding: 25px 5px !important;
        }

        .main-card {
            border-radius: 3px !important;
            border: 1px solid #DFDFDF;
            border-left: 2px solid #73b4ff !important;
            box-shadow: none !important;
        }

        .child-card {
            border-radius: 3px !important;
            border: 1px solid #DFDFDF !important;
            box-shadow: none !important;
        }

        .card-header {
            padding: 10px 15px !important;
            border-radius: 0 !important;
        }

        .child-card-header {
            background-color: #EEEEEE;
            padding: 5px 12px !important;
            color: #0000cd !important;
            border-bottom: 2px solid #73b4ff !important;
        }

        table {
            text-align: center !important;
            border: 1px solid #E2E5E8 !important;
        }

        .table thead th {
            background-color: #7a9ebd !important;
            font-size: 14px !important;
            color: #fff !important;
            vertical-align: central !important;
        }

        .table .table-xs th {
            padding: 0.3rem 2rem !important;
        }

        .table tbody tr {
            border: 1px solid #6b799c !important;
        }

        .table tbody td {
            font-size: 11px !important;
        }

        .table > tbody > tr:not(th):nth-child(even) {
            background-color: #d7e3ee !important;
        }

        .table.table-xs td {
            padding: 0.3rem 0.4rem !important;
        }

        .form-container {
            margin: 0 auto !important;
            margin-top: 10px;
        }

        .text-c-red {
            color: red !important;
        }

        .example-container {
            height: 300px;
            overflow: auto;
        }

        .indented span.label-text {
            font-weight: lighter !important;
            display: inline-block;
            font-size: 1em !important;
            padding: 1px 7px !important;
        }


        .btn-container {
            min-width: 300px;
            margin: 0 auto;
        }

        .invalid-tooltip {
            position: absolute;
            top: -90%;
            left: 20%;
            z-index: 5;
            display: block;
            max-width: 203%;
            padding: 0.20rem 0.5rem;
            margin-top: .1rem;
            font-size: 0.575rem;
            line-height: 1.5;
            color: #fff;
            background-color: #B0441C;
            border-radius: 5px;
        }

            .invalid-tooltip::after {
                content: " ";
                position: absolute;
                top: 100%; /* At the bottom of the tooltip */
                left: 50%;
                margin-left: -5px;
                border-width: 5px;
                border-style: solid;
                border-color: #B0441C transparent transparent transparent;
            }

            .invalid-tooltip .show {
                display: block;
            }

        .detail-container {
            border: 1px solid #DFDFDF;
            margin: 3px 10px;
            padding: 10px 15px;
            overflow: hidden;
        }

        .detail-container-footer {
            margin: 0 auto !important;
            width: 100%;
            padding: 5px 15px;
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
        }

            .detail-container-footer .total-box {
                margin: 2px 5px;
                flex: 0 1 19%;
                border: 1px solid green;
                vertical-align: central;
                text-align: center;
                font-weight: bold;
                padding: 3px 0;
            }


        .btn-xs {
            padding: 0.05rem 0.5rem;
            font-size: 0.875rem;
            line-height: 1.5;
            border-radius: 2px;
        }

        .select2-container--default .select2-selection--single .select2-selection__rendered {
            padding: 0px !important;
            padding-left: 0px;
            padding-left: 5px !important;
            color: #0c1011 !important;
            line-height: 27px !important;
        }

        .form-check label {
            font-weight: bold;
        }

        .table thead th > label,
        .total-box > label {
            margin-bottom: 0 !important;
        }

        .card .card-block, .card .card-body {
            padding: 12px 10px 1px 12px !important;
        }

        .main-card-head {
            display: flex;
            justify-content: space-between;
            align-items: baseline;
        }

        .pcoded-main-container {
            top: -20px;
        }
    </style>

    <script src="../Assets/js/vendors/jquery/jquery.min.js"></script>
    <script src="../CssFilesAll/bower_components/jquery-ui/js/jquery-ui.min.js"></script>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-more.js"></script>
    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>
    <script src="https://code.highcharts.com/modules/funnel.js"></script>

    <script src="~/assets/js/vendor-all.min.js"></script>
    <script src="~/assets/js/plugins/bootstrap.min.js"></script>
    <script src="~/assets/js/pcoded.min.js"></script>

    <!-- datepicker js -->
    <script src="../assets/js/plugins/moment.min.js"></script>
    <script src="../assets/js/plugins/daterangepicker.js"></script>
    <script src="../assets/js/pages/ac-datepicker.js"></script>



</head>
<body>


    <div class="pcoded-main-container" style="padding: 15px; margin: 0 auto !important;">
        <div class="pcoded-wrapper">
            <div class="pcoded-content">
                <div class="pcoded-inner-content">
                    <div class="main-body">
                        <div class="page-wrapper">
                            <div class="page-body">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <div class="card main-card  pb-4">
                                            <div class="card-header main-card-head">
                                                <h5 class=""><i class="fas fa-1x fa-th-large "></i>&nbsp;Dashboard </h5>
                                                
                                            </div>
                                            <div class="card-body">

                                                <div class="row p-15">
                                                    <div class="col-12">
                                                        <div class="card main-card text-center" style="border: none !important;">
                                                            <div class="card-body">

                                                                <h2 class="card-text text-uppercase mt-2" style="color: #22A2CB">ZAS Corporation </h2>
                                                                <h3 class="card-title text-uppercase mb-2 text-c-blue" style="margin: 0 !important">Enterprise Resource Planing (ERP) </h3>

                                                                <%--<p class="card-text"><small class="text-muted">Dashboard</small> </p>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row p-15">

                                                    <div class="col-sm-3"></div>
                                                    <div class="col-sm-3">

                                                        <div class="card child-card text-center">
                                                            <div class="card-body">
                                                                <p class="card-text">
                                                                    <i class="fas fa-6x fa-file-invoice-dollar text-c-green" style="color: #900c3f" aria-hidden="true"></i>
                                                                </p>
                                                                <h4 class="card-title mt-2 text-uppercase"> SalesRoll Dashboard </h4>
                                                                <a href="../SInventory_UI/Dashboard.aspx" class="btn btn-sm btn-primary" style="background-color: #900c3f; color: #fff;"> Go to dashboard &nbsp; <i class="fa fa-arrow-circle-right" aria-hidden="true"></i>
                                                                </a>
                                                                <br />
                                                                <br />
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="col-sm-3">

                                                        <div class="card child-card text-center">
                                                            <div class="card-body">
                                                                <p class="card-text">
                                                                    <i class="fas fa-6x fas fa-chart-line text-c-blue" style="color: #22A2CB" aria-hidden="true"></i>
                                                                </p>
                                                                <h4 class="card-title mt-2 text-uppercase"> Accounts Dashboard </h4>
                                                                <a href="../SInventory_UI/AccountsPanel.aspx" class="btn btn-sm btn-primary" style="background-color: #22A2CB; color: #fff;">Go to dashboard &nbsp; <i class="fa fa-arrow-circle-right" aria-hidden="true"></i>
                                                                </a>
                                                                <br />
                                                                <br />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3"></div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    </div>





    <script src="../assets/js/vendor-all.min.js"></script>
    <script src="../assets/js/plugins/bootstrap.min.js"></script>
    <script src="../assets/js/pcoded.min.js"></script>
    <script src="../assets/js/horizontal-menu.js"></script>

</body>
</html>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="SInventory_UI_Dashboard" %>

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

        .info-card {
            padding: 0 !important;
        }



        .notification-card .card-block .notify-icon i {
            font-size: 30px;
        }

        .notification-card .card-block .notify-cont {
            padding: 15px 0;
            border-left: 1px solid #fff;
        }

        .bg-c-megenta {
            background-color: #826BBF;
        }

        .div-hide {
            display: none !important;
        }

        .div-show {
            display: block !important;
        }

        .flex-card-container {
            display: flex;
        }


        .flex-card {
            display: flex;
            flex: 1 0 0;
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
                                                <div class="col-sm-4 mb-0">
                                                    <div class="form-group row">
                                                        <label for="ddlCardCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                        <div class="col-sm-8">
                                                            <select id="ddlCardCompany" class="form-control form-control-sm "></select>
                                                        </div>

                                                    </div>
                                                </div>

                                                <a href="../SInventory_UI/DashboardPanel.aspx" class="btn btn-sm btn-info">
                                                    <i class="feather bold icon-corner-up-right"></i>Back to list
                                                </a>
                                            </div>
                                            <div class="card-body">
                                                <div class="row" style="padding: 10px 15px 0 15px !important; margin-bottom: -20px;">
                                                    <div class="col-md-6 col-xl-3">
                                                        <div class="card bg-c-blue notification-card">
                                                            <div class="card-block info-card" style="padding: 0 8px !important;">
                                                                <div class="row align-items-center">
                                                                    <div class="col-4 notify-icon"><i class="fa fa-cart-plus"></i></div>
                                                                    <div class="col-8 notify-cont">
                                                                        <h4 id="OrderNo">0</h4>
                                                                        <p>Order Received</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-xl-2">
                                                        <div class="card bg-c-green notification-card">
                                                            <div class="card-block" style="padding: 0 8px !important;">
                                                                <div class="row align-items-center">
                                                                    <div class="col-4 notify-icon"><i class="fas fa-file-invoice-dollar"></i></div>
                                                                    <div class="col-8 notify-cont">
                                                                        <h4 id="invoiceNo">0</h4>
                                                                        <p>Total Invoice</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-xl-2">
                                                        <div class="card bg-c-yellow notification-card">
                                                            <div class="card-block" style="padding: 0 8px !important;">
                                                                <div class="row align-items-center">
                                                                    <div class="col-4 notify-icon"><i class="fas fa-dolly-flatbed"></i></div>
                                                                    <div class="col-8 notify-cont">
                                                                        <h4 id="deliveryNo">0</h4>
                                                                        <p>Delivery Confirmed</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6 col-xl-2">
                                                        <div class="card bg-c-megenta notification-card">
                                                            <div class="card-block" style="padding: 0 8px !important;">
                                                                <div class="row align-items-center">
                                                                    <div class="col-4 notify-icon"><i class="fas fa-chart-bar"></i></div>
                                                                    <div class="col-8 notify-cont">
                                                                        <h4 id="collection">0</h4>
                                                                        <p>Total Collection</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-6 col-xl-3">
                                                        <div class="card bg-c-red notification-card">
                                                            <div class="card-block" style="padding: 0 8px !important;">
                                                                <div class="row align-items-center">
                                                                    <div class="col-4 notify-icon"><i class="fas fa-funnel-dollar"></i></div>
                                                                    <div class="col-8 notify-cont">
                                                                        <h4 id="due">0</h4>
                                                                        <p>Total Due</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- notification counter end -->
                                                </div>


                                                <div class="row p-15 flex-card-container">

                                                    <!-- Group Sales (Sales Funnel) -->
                                                    <div class="col-md-12 col-xl-5 div-show flex-card">
                                                        <div class="accordion" id="accordionExample">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="">Current Month Sales (Wings Contribution) </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne" class="card-body mt-3 collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" style="padding-bottom: 10px !important">



                                                                    <div class="row form-container">


                                                                        <div id="sales-funnel" style="height: 428px;"></div>
                                                                        <img id="salesFunnel-spinner" class="center" style="height: 80px; width: 80px;" src="../Assets/loading.gif" />
                                                                    </div>
                                                                </div>

                                                                <div class="accordion" id="accordionExample11" style="width: 96% !important; margin: 0 auto;">

                                                                    <div class="card child-card">
                                                                        <div class="card-header child-card-header text-left" id="headingOne11">
                                                                            <h5 class="mb-0">
                                                                                <a href="#!" data-toggle="collapse" data-target="#collapseOne11" aria-expanded="true" aria-controls="collapseOne" class=""><i class="fa fa-plus-square" aria-hidden="true"></i>&nbsp; View DataTable (Wings Wise Contribution in total-sales) </a>
                                                                            </h5>
                                                                        </div>
                                                                        <div id="collapseOne11" class="card-body mt-3 collapse hide" aria-labelledby="headingOne11" data-parent="#accordionExample11" style="padding-bottom: 10px !important">



                                                                            <div class="row form-container">
                                                                                <div class="table-responsive">
                                                                                    <table class="table table-bordered table-xs">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th># SL</th>
                                                                                                <th>Wings Name </th>
                                                                                                <th>Amount (TK.) </th>
                                                                                                <th>Percentage (%) </th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody id="salesfunnel-detail">
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>





                                                    <%--============================= Monthly Sales Trend start ===============================--%>

                                                    <div class="col-md-12 col-xl-7">
                                                        <div class="accordion" id="accordionExample1">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne1">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne1" aria-expanded="true" aria-controls="collapseOne" class="">Monthly Sales (Wings Wise) </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne1" class="card-body mt-3 collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" style="padding-bottom: 10px !important">

                                                                    <div class="row ">
                                                                        <%--<div class="col-sm-6">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesTrendCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlSalesTrendCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>--%>

                                                                        <div class="col-sm-7">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesTrendFinancialYear" class="col-sm-4 col-form-label col-form-label-sm text-right">Financial Year <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlSalesTrendFinancialYear" class="form-control form-control-sm ">
                                                                                        <option value="2019">2019</option>
                                                                                        <option value="2020">2020</option>
                                                                                        <option value="2021">2021</option>
                                                                                        <option value="2022">2022</option>
                                                                                        <option value="2023">2023</option>
                                                                                        <option value="2024">2024</option>
                                                                                        <option value="2025">2025</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                    </div>


                                                                    <div class="row form-container table-responsive loader-block">
                                                                        <div id="container" style="height: 380px; width: 100% !important;"></div>
                                                                        <img id="salesTrend-spinner" class="center" style="height: 80px; width: 80px;" src="../Assets/loading.gif" />

                                                                    </div>
                                                                </div>


                                                                <div class="accordion" id="accordionExample21" style="width: 96% !important; margin: 0 auto;">

                                                                    <div class="card child-card">
                                                                        <div class="card-header child-card-header text-left" id="headingOne21">
                                                                            <h5 class="mb-0">
                                                                                <a href="#!" data-toggle="collapse" data-target="#collapseOne21" aria-expanded="true" aria-controls="collapseOne" class=""><i class="fa fa-plus-square" aria-hidden="true"></i>&nbsp; View DataTable (Monthly Sales Trend) </a>
                                                                            </h5>
                                                                        </div>
                                                                        <div id="collapseOne21" class="card-body mt-3 collapse hide" aria-labelledby="headingOne21" data-parent="#accordionExample21" style="padding-bottom: 10px !important">
                                                                            <div class="row form-container">
                                                                                <div class="table-responsive">
                                                                                    <table class="table table-bordered table-xs">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th># SL</th>
                                                                                                <th>Month Name </th>
                                                                                                <th>Critical Care</th>
                                                                                                <th>Dermal</th>
                                                                                                <th>Gloves & Instrument</th>
                                                                                                <th>Gyno</th>
                                                                                                <th>Nutrition</th>
                                                                                                <th>NOC</th>
                                                                                                <th>Onco</th>
                                                                                                <th>Opthalmic</th>
                                                                                                <th>OTC</th>
                                                                                                <th>Dental ( Clinic )</th>
                                                                                                <th>Baby Care</th>
                                                                                                <th>Hematology</th>
                                                                                                <th>Neurology</th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody id="salestrend-detail">
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row p-15 flex-card-container">


                                                    <%--============================= Expire product ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionExpireProduct">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingExpire">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseExpireProduct" aria-expanded="true" aria-controls="collapseOne" class="">Expire Product </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseExpireProduct" class="card-body mt-3 collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" style="padding-bottom: 10px !important">

                                                                    <div class="row ">
                                                                        <div class="col-sm-6">
                                                                            <div class="form-group row">
                                                                                <label for="ddlExpiredProductCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlExpiredProductCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-5">
                                                                            <div class="form-group row">
                                                                                <label for="ddlExpireInMonth" class="col-sm-4 col-form-label col-form-label-sm text-right">Expired In (Month) <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlExpireInMonth" class="form-control form-control-sm ">
                                                                                        <option value="1">1</option>
                                                                                        <option value="2">2</option>
                                                                                        <option value="3">3</option>
                                                                                        <option value="4">4</option>
                                                                                        <option value="5">5</option>
                                                                                        <option value="6">6</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadExpireProduct()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>



                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                    </div>


                                                                    <div class="row form-container table-responsive loader-block">
                                                                        <div class="row form-container">
                                                                            <div class="table-responsive">
                                                                                <table class="table table-bordered table-xs">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th># SL</th>
                                                                                            <th>Product Code </th>
                                                                                            <th>Product Name </th>
                                                                                            <th>Pack Size </th>
                                                                                            <th>Mfg. Date </th>
                                                                                            <th>Exp. Date </th>
                                                                                            <th>Batch No </th>
                                                                                            <th>Stock Qty</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody id="expire-detail">
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row p-15">

                                                    <%--============================= Top 50 Customer ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionTopCustomer">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingTopCustomer">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseTopCustomer" aria-expanded="true" aria-controls="collapseOne" class="">Top 50 Customer </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseTopCustomer" class="card-body mt-3 collapse show" aria-labelledby="headingTopCustomer" data-parent="#accordionTopCustomer" style="padding-bottom: 10px !important">

                                                                    <div class="row ">

                                                                        <div class="col-sm-1"></div>

                                                                        <div class="col-sm-4">
                                                                            <div class="form-group row">
                                                                                <label for="ddlTopCustomerCompany" class="col-sm-4 col-form-label col-form-label-sm text-right"> Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlTopCustomerCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="txtFromDate" class="col-sm-4 col-form-label col-form-label-sm text-right"> From Date <span class="text-sm-left text-c-red">[*]</span></label>
                                                                                <div class="col-sm-8">

                                                                                    <input class="form-control  form-control-sm " type="date" id="txtTopFromDate" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="txtToDate" class="col-sm-3 col-form-label col-form-label-sm text-right">To Date <span class="text-sm-left text-c-red">[*]</span></label>
                                                                                <div class="col-sm-8">

                                                                                    <input class="form-control  form-control-sm " type="date" id="txtTopToDate" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadTop50Customer()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>


                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                    </div>

                                                                    <div class="row form-container">
                                                                        <table class="table table-bordered table-xs">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th># SL</th>
                                                                                    <th>Customer Code</th>
                                                                                    <th>Customer Name</th>
                                                                                    <th>Actual Sales </th>
                                                                                    <th>Due </th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody id="top50-customer">
                                                                            </tbody>
                                                                        </table>
                                                                    </div>

                                                                </div>

                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                
                                                
                                                
                                                <div class="row p-15">

                                                    <%--============================= Top Priority Product Sales ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionTopPriority">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingTopPriority">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#accordionTopPriority" aria-expanded="true" aria-controls="collapseTopPriority" class="">Priority Product Sales </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseTopPriority" class="card-body mt-3 collapse show" aria-labelledby="headingTopPriority" data-parent="#accordionTopPriority" style="padding-bottom: 10px !important">

                                                                    <div class="row ">


                                                                        <div class="col-sm-2">
                                                                            <div class="form-group row">
                                                                                <label for="ddlTopCustomerCompany" class="col-sm-4 col-form-label col-form-label-sm text-right"> Wings  <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlTopPriorityCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlRegion" class="col-sm-4 col-form-label col-form-label-sm text-right"> Region  <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlRegion" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="txtFromDate" class="col-sm-4 col-form-label col-form-label-sm text-right"> From Date <span class="text-sm-left text-c-red">[*]</span></label>
                                                                                <div class="col-sm-8">

                                                                                    <input class="form-control  form-control-sm " type="date" id="txtTopPriorityFromDate" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="txtToDate" class="col-sm-3 col-form-label col-form-label-sm text-right">To Date <span class="text-sm-left text-c-red">[*]</span></label>
                                                                                <div class="col-sm-8">

                                                                                    <input class="form-control  form-control-sm " type="date" id="txtTopPriorityToDate" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadTopPriorityProductSales()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>


                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                    </div>

                                                                    <div class="row form-container">
                                                                        <table class="table table-bordered table-xs">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th># SL</th>
                                                                                    <th>Product Code</th>
                                                                                    <th>Product Name</th>
                                                                                    <th>Target </th>
                                                                                    <th>Sales </th>
                                                                                    <th>Achivement (%) </th>
                                                                                    <th>Time Pass (%) </th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody id="TopPriorityProductSales">
                                                                            </tbody>
                                                                        </table>
                                                                    </div>

                                                                </div>

                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="row p-15">




                                                    <%--============================= Wings Sales Per Day start ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionExample2">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne2">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne2" aria-expanded="true" aria-controls="collapseOne" class="">Sales & Collection Comparison (Day wise) </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne2" class="card-body mt-3 collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" style="padding-bottom: 10px !important">

                                                                    <div class="row ">
                                                                        <div class="col-sm-4">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlSalesCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesFinancialYear" class="col-sm-4 col-form-label col-form-label-sm text-right">Financial Year <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlSalesFinancialYear" class="form-control form-control-sm ">
                                                                                        <option value="2019">2019</option>
                                                                                        <option value="2020">2020</option>
                                                                                        <option value="2021">2021</option>
                                                                                        <option value="2022">2022</option>
                                                                                        <option value="2023">2023</option>
                                                                                        <option value="2024">2024</option>
                                                                                        <option value="2025">2025</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-4">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-3 col-form-label col-form-label-sm text-right">Month name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlSalesMonth" class="form-control form-control-sm ">

                                                                                        <option value="1">January</option>
                                                                                        <option value="2">February</option>
                                                                                        <option value="3">March</option>
                                                                                        <option value="4">April</option>
                                                                                        <option value="5">May</option>
                                                                                        <option value="6">June</option>
                                                                                        <option value="7">July</option>
                                                                                        <option value="8">August</option>
                                                                                        <option value="9">September</option>
                                                                                        <option value="10">October</option>
                                                                                        <option value="11">November</option>
                                                                                        <option value="12">December</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadWingsWisePerDaySales()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>



                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                    <div class="row form-container">
                                                                        <div id="dc-chart-container" style="width: 100% !important;"></div>

                                                                        <img id="monthlySales-spinner" class="center" style="height: 80px; width: 80px;" src="../Assets/loading.gif" />


                                                                    </div>
                                                                </div>

                                                                <div class="accordion" id="accordionExample31" style="width: 96% !important; margin: 0 auto;">

                                                                    <div class="card child-card">
                                                                        <div class="card-header child-card-header text-left" id="headingOne31">
                                                                            <h5 class="mb-0">
                                                                                <a href="#!" data-toggle="collapse" data-target="#collapseOne31" aria-expanded="true" aria-controls="collapseOne" class=""><i class="fa fa-plus-square" aria-hidden="true"></i>&nbsp; View DataTable (Wings Sales Status)</a>
                                                                            </h5>
                                                                        </div>
                                                                        <div id="collapseOne31" class="card-body mt-3 collapse hide" aria-labelledby="headingOne31" data-parent="#accordionExample31" style="padding-bottom: 10px !important">
                                                                            <div class="row form-container">
                                                                                <table class="table table-bordered table-xs">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th># SL</th>
                                                                                            <th>Date </th>
                                                                                            <th>Actual Sales (TK.) </th>
                                                                                            <th>Collection (TK.) </th>
                                                                                            <th>Total Due (TK.) </th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody id="salesstatus-detail">
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                    <%--============================= National Product Sales (Wings Wise) start ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionExample3">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne3">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne3" aria-expanded="true" aria-controls="collapseOne" class="">National Product Sales ( Wings Wise - ( By Default current month sales ) ) </a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne3" class="card-body mt-3 collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" style="padding-bottom: 10px !important">

                                                                    <div class="row ">

                                                                        <div class="col-sm-1"></div>

                                                                        <div class="col-sm-4">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlProductSalesCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="txtFromDate" class="col-sm-4 col-form-label col-form-label-sm text-right">From Date <span class="text-sm-left text-c-red">[*]</span></label>
                                                                                <div class="col-sm-8">

                                                                                    <input class="form-control  form-control-sm " type="date" id="txtFromDate" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="txtToDate" class="col-sm-3 col-form-label col-form-label-sm text-right">To Date <span class="text-sm-left text-c-red">[*]</span></label>
                                                                                <div class="col-sm-8">

                                                                                    <input class="form-control  form-control-sm " type="date" id="txtToDate" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadProductWiseSalesChart()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>


                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="row form-container">
                                                                        <div id="product-chart-container" style="height: 1000px; width: 100%;"></div>
                                                                        <img id="productSales-spinner" class="center" style="height: 100px; width: 100px;" src="../Assets/loading.gif" />
                                                                    </div>
                                                                </div>

                                                                <div class="accordion" id="accordionExample41" style="width: 96% !important; margin: 0 auto;">

                                                                    <div class="card child-card">
                                                                        <div class="card-header child-card-header text-left" id="headingOne41">
                                                                            <h5 class="mb-0">
                                                                                <a href="#!" data-toggle="collapse" data-target="#collapseOne41" aria-expanded="true" aria-controls="collapseOne" class=""><i class="fa fa-plus-square" aria-hidden="true"></i>&nbsp; View DataTable (Product Sales)</a>
                                                                            </h5>
                                                                        </div>
                                                                        <div id="collapseOne41" class="card-body mt-3 collapse hide" aria-labelledby="headingOne41" data-parent="#accordionExample41" style="padding-bottom: 10px !important">
                                                                            <div class="row form-container">
                                                                                <table class="table table-bordered table-xs">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th># SL</th>
                                                                                            <th>Product</th>
                                                                                            <th>Sales Quantity</th>
                                                                                            <th>Actual Sales (TK.) </th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody id="productsales-detail">
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                    <%--============================= Ageing Report start ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionExample5">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne5">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne5" aria-expanded="true" aria-controls="collapseOne5" class="">Ageing Report</a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne5" class="card-body mt-3 collapse show" aria-labelledby="headingOne5" data-parent="#accordionExample5" style="padding-bottom: 10px !important">
                                                                    <div class="row ">
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlAglingCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ageing" class="col-sm-5 col-form-label col-form-label-sm text-right">Time Period <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <div class="form-check form-check-inline mt-1">
                                                                                        <input class="form-check-input " type="radio" name="inlineRadioOptions" id="ageing" value="Day" />
                                                                                        <label class="form-check-label bold">Daily</label>
                                                                                    </div>
                                                                                    <div class="form-check form-check-inline mt-1">
                                                                                        <input class="form-check-input ageing" type="radio" name="inlineRadioOptions" id="ageing" value="Month" />
                                                                                        <label class="form-check-label bold">Monthly</label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesFinancialYear" class="col-sm-3 col-form-label col-form-label-sm text-right">Year <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlAglingFinancialYear" class="form-control form-control-sm ">
                                                                                        <option value="2019">2019</option>
                                                                                        <option value="2020">2020</option>
                                                                                        <option value="2021">2021</option>
                                                                                        <option value="2022">2022</option>
                                                                                        <option value="2023">2023</option>
                                                                                        <option value="2024">2024</option>
                                                                                        <option value="2025">2025</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>



                                                                        <div class="col-sm-2">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-3 col-form-label col-form-label-sm text-right">Month <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlAglingMonth" class="form-control form-control-sm ">

                                                                                        <option value="1">January</option>
                                                                                        <option value="2">February</option>
                                                                                        <option value="3">March</option>
                                                                                        <option value="4">April</option>
                                                                                        <option value="5">May</option>
                                                                                        <option value="6">June</option>
                                                                                        <option value="7">July</option>
                                                                                        <option value="8">August</option>
                                                                                        <option value="9">September</option>
                                                                                        <option value="10">October</option>
                                                                                        <option value="11">November</option>
                                                                                        <option value="12">December</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadAglingReport()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>


                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row form-container">
                                                                        <div class="table-responsive">

                                                                            <table class="table table-bordered table-xs">
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th># SL</th>
                                                                                        <th>Name of the region </th>
                                                                                        <th>1 - 10 (Days) </th>
                                                                                        <th>11 - 20 (Days)</th>
                                                                                        <th>21 - 30 (Days)</th>
                                                                                        <th>31 - 40 (Days)</th>
                                                                                        <th>41 - 50 (Days)</th>
                                                                                        <th>51 - 60 (Days)</th>
                                                                                        <th>60 + (Days)</th>
                                                                                    </tr>
                                                                                </thead>
                                                                                <tbody id="agling-detail">
                                                                                </tbody>
                                                                            </table>

                                                                            <img id="ageing-spinner" class="center" style="height: 100px; width: 100px;" src="../Assets/loading.gif" />
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <%--============================= Business Summery Report start ===============================--%>

                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionExample6">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne6">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne56" aria-expanded="true" aria-controls="collapseOne6" class="">Business Summery Report</a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne6" class="card-body mt-3 collapse show" aria-labelledby="headingOne6" data-parent="#accordionExample6" style="padding-bottom: 10px !important">
                                                                    <div class="row ">
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlBusinessSummeryCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesFinancialYear" class="col-sm-5 col-form-label col-form-label-sm text-right">Time Period <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <div class="form-check form-check-inline mt-1">
                                                                                        <input class="form-check-input" type="radio" name="inlineRadioOptions1" value="Day" />
                                                                                        <label class="form-check-label bold">Daily</label>
                                                                                    </div>
                                                                                    <div class="form-check form-check-inline mt-1">
                                                                                        <input class="form-check-input" type="radio" name="inlineRadioOptions1" value="Month" />
                                                                                        <label class="form-check-label bold">Monthly</label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesFinancialYear" class="col-sm-3 col-form-label col-form-label-sm text-right">Year <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlBusinessSummeryFinancialYear" class="form-control form-control-sm ">
                                                                                        <option value="2019">2019</option>
                                                                                        <option value="2020">2020</option>
                                                                                        <option value="2021">2021</option>
                                                                                        <option value="2022">2022</option>
                                                                                        <option value="2023">2023</option>
                                                                                        <option value="2024">2024</option>
                                                                                        <option value="2025">2025</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>



                                                                        <div class="col-sm-2">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-3 col-form-label col-form-label-sm text-right">Month <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-8">
                                                                                    <select id="ddlBusinessSummeryMonth" class="form-control form-control-sm ">

                                                                                        <option value="1">January</option>
                                                                                        <option value="2">February</option>
                                                                                        <option value="3">March</option>
                                                                                        <option value="4">April</option>
                                                                                        <option value="5">May</option>
                                                                                        <option value="6">June</option>
                                                                                        <option value="7">July</option>
                                                                                        <option value="8">August</option>
                                                                                        <option value="9">September</option>
                                                                                        <option value="10">October</option>
                                                                                        <option value="11">November</option>
                                                                                        <option value="12">December</option>
                                                                                    </select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-1">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesMonth" class="col-sm-1 col-form-label col-form-label-sm text-right">&nbsp;</label>
                                                                                <div class="col-sm-10">

                                                                                    <button type="button" onclick="LoadBusinessSummeryReport()" class="btn btn-sm btn-success">
                                                                                        <%--<i class="feather icon-thumbs-up"></i>--%>
                                                                                        <i class="fa fa-1x fa-retweet" aria-hidden="true"></i>
                                                                                    </button>


                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row form-container">
                                                                        <div class="table-responsive">
                                                                            <table class="table table-bordered table-xs">
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th># SL</th>
                                                                                        <th>Name of the region </th>
                                                                                        <th>Previous Due</th>
                                                                                        <th>(Today's/Selected Month) Due</th>
                                                                                        <th>Total Due</th>
                                                                                        <th>Previous Due Collection</th>
                                                                                        <th>(Today's/Selected Month) Collection</th>
                                                                                        <th>Total Collection</th>
                                                                                    </tr>
                                                                                </thead>
                                                                                <tbody id="BusinessSummery-detail">
                                                                                </tbody>
                                                                            </table>

                                                                            <img id="business-spinner" class="center" style="height: 100px; width: 100px;" src="../Assets/loading.gif" />
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

                    </div>
                </div>
            </div>
        </div>
    </div>





    <script src="../assets/js/vendor-all.min.js"></script>
    <script src="../assets/js/plugins/bootstrap.min.js"></script>
    <script src="../assets/js/pcoded.min.js"></script>
    <script src="../assets/js/horizontal-menu.js"></script>


    <script type="text/javascript">

        $(document).ready(function () {


            $(function () {
                $('#txtToDate').daterangepicker({
                    singleDatePicker: true,
                    showDropdowns: true,
                    minYear: 1901,
                    maxYear: parseInt(moment().format('YYYY'), 10)
                }, function (start, end, label) {
                    var years = moment().diff(start, 'years');
                    alert("You are " + years + " years old!");
                });
            });

            function formatDate(date) {
                var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();

                if (month.length < 2)
                    month = '0' + month;
                if (day.length < 2)
                    day = '0' + day;

                return [year, month, day].join('-');
            }


            //<----------------- Set Date ---------------------->
            
            //var date = new Date();
            //var fDay = new Date(date.getFullYear(), date.getMonth(), 1);
            //var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

            
            
            //var startDate = formatDate((new Date(date.getFullYear(), date.getMonth(), 1)).substr(6));
            //var endDate = formatDate((new Date(date.getFullYear(), date.getMonth() + 1, 0)).substr(6));
            
            //alert(lastDay);
            
            //$('#txtTopFromDate').val(startDate);
            //$('#txtTopToDate').val(endDate);

            // <-------------- Load & Set Initial Data ------------------>

            LoadDropDownlist();

            $("input[name='inlineRadioOptions'][value='Day']").prop('checked', true);
            $("input[name='inlineRadioOptions1'][value='Day']").prop('checked', true);

            // Dashboard Card

            SetDashBoardCardInfo();

            $('#ddlCardCompany').change(function () {
                SetDashBoardCardInfo(); // Load On Change
            });


            //<------------- Expire Product ---------------------->

            LoadExpireProduct();

            //<------------- Expire Product ---------------------->

            LoadTop50Customer();

            //<------------- Expire Product ---------------------->

            LoadTopPriorityProductSales();

            //<------------- Sales Funnel ---------------------->

            LoadSalesFunnel();

            $("#headingOne11").click(function () {

                LoadSalesFunnelDataTable(); // Load Datatable

            });

            // <----------------- Sales Trend ----------------->

            LoadWingsSalesTrendChart();

            $("#headingOne21").click(function () {

                LoadSalesTrendDataTable(); // Load Datatable

            });


            $('#ddlSalesTrendCompany').change(function () {
                LoadFinancialYear();
                //LoadSalesTrendDataTable();
                //LoadWingsSalesTrendChart();
            });


            $('#ddlSalesTrendFinancialYear').change(function () {
                LoadSalesTrendDataTable();
                LoadWingsSalesTrendChart(); // Load On Change
            });

            //<----------------------  Per Day Sales -------------------->

            LoadWingsWisePerDaySales();

            $('#ddlSalesCompany').change(function () {

                LoadFinancialYearMonth();
                //LoadWingsSalesStatusDataTable();
                //LoadWingsWisePerDaySales();
            });

            $("#headingOne31").click(function () {

                LoadWingsSalesStatusDataTable(); // Load Datatable

            });

            //<--------------- National Product Sales ------------------>

            LoadProductWiseSalesChart();

            $("#headingOne41").click(function () {

                LoadProductSalesDataTable(); // Load Datatable

            });

            // <----------------- Ageing Report --------------->
            LoadAglingReport();

            // <--------------- Business Summery Report ---------------->
            LoadBusinessSummeryReport();


            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);

            $('#txtFromDate').val(firstDay);

            //$('#ddlSalesMonth').change(function () {

            //    LoadWingsSalesStatusDataTable();
            //    LoadWingsWisePerDaySales();
            //});

            //$('#ddlSalesFinancialYear').change(function () {
            //    LoadWingsSalesStatusDataTable();
            //    LoadWingsWisePerDaySales();
            //});


            $('#ddlProductSalesCompany').change(function () {
                LoadProductWiseSalesChart();
                LoadProductSalesDataTable();
            });

            $('#ddlAglingCompany').change(function () {
                LoadAglingReport();
            });

            $('#ddlAglingFinancialYear').change(function () {
                LoadAglingReport();
            });

            $('#ddlAglingMonth').change(function () {
                LoadAglingReport();
            });

            //$("input[name='inlineRadioOptions']").change(function() {

            //    alert(hi);

            //});

            $('input[type=radio][name=inlineRadioOptions]').change(function () {

                alert("hi");

            });

            //$('input[type=radio][name=inlineRadioOptions]').on('change', function() {
            //    debugger;
            //    LoadAglingReport();                
            //});


        });


        //<------------- Expire Product ---------------------->

        function LoadExpireProduct() {

            var expireIn = $('#ddlExpireInMonth').val();
            var companyId = $('#ddlExpiredProductCompany').val();


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyWiseExpireProduct",
                data: JSON.stringify({ companyId: companyId, expireIn: expireIn }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var html = '';

                    for (var i in result) {


                        html += '<tr>';
                        html += '<td>' + (parseInt(i) + 1) + '</td>';
                        html += '<td>' + result[i].ProductCode + '</td>';
                        html += '<td>' + result[i].ProductName + '</td>';
                        html += '<td>' + result[i].PackSize + '</td>';
                        html += '<td>' + result[i].MfgDate + '</td>';
                        html += '<td>' + result[i].ExpDate + '</td>';
                        html += '<td>' + result[i].BatchNo + '</td>';
                        html += '<td>' + numberWithCommas(result[i].StockQty) + '</td>';
                        html += '</tr>';

                    }


                    $('#expire-detail').html(html);



                }
            });
        }
        

        //<------------- Top50 Customer ---------------------->

        function LoadTop50Customer() {

            var fromDate = $('#txtTopFromDate').val();
            var toDate = $('#txtTopToDate').val();
            var companyId = $('#ddlTopCustomerCompany').val();


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetTop50Customer",
                data: JSON.stringify({ companyId: companyId, fromDate: fromDate, toDate: toDate }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var html = '';

                    for (var i in result) {


                        html += '<tr>';
                        html += '<td>' + (parseInt(i) + 1) + '</td>';
                        html += '<td>' + result[i].CustomerCode + '</td>';
                        html += '<td>' + result[i].CustomerName + '</td>';
                        html += '<td>' + numberWithCommas(result[i].ActualSales) + '</td>';
                        html += '<td>' + numberWithCommas(result[i].Due) + '</td>';
                        html += '</tr>';

                    }


                    $('#top50-customer').html(html);



                }
            });
        }


        //<------------- Top Priority Product ---------------------->

        function LoadTopPriorityProductSales() {

            var fromDate = $('#txtTopPriorityFromDate').val();
            var toDate = $('#txtTopPriorityToDate').val();
            var companyId = $('#ddlTopPriorityCompany').val();
            var regionId = $('#ddlRegion').val();


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetTopPriorityProductsales",
                data: JSON.stringify({ companyId: companyId, regionId: regionId, fromDate: fromDate, toDate: toDate }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var html = '';

                    for (var i in result) {


                        html += '<tr>';
                        html += '<td>' + (parseInt(i) + 1) + '</td>';
                        html += '<td>' + result[i].ProductCode + '</td>';
                        html += '<td>' + result[i].ProductName + '</td>';
                        html += '<td>' + numberWithCommas(result[i].TargetQty) + '</td>';
                        html += '<td>' + numberWithCommas(result[i].SalesQty) + '</td>';
                        html += '<td>' + numberWithCommas(parseFloat(result[i].Achivment).toFixed(2)) + ' %</td>';
                        html += '<td>' + numberWithCommas(parseFloat(result[i].TimePass).toFixed(2)) + ' %</td>';
                        html += '</tr>';

                    }


                    $('#TopPriorityProductSales').html(html);



                }
            });
        }


        function LoadAglingReport() {

            var timePeriod = $("input[name='inlineRadioOptions']:checked").val();

            debugger;

            var companyId = $('#ddlAglingCompany').val();
            var year = $('#ddlAglingFinancialYear').val();
            var month = $('#ddlAglingMonth').val();

            $("#ageing-spinner").show();

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetAglingReport",
                data: JSON.stringify({ companyId: companyId, timePeriod: timePeriod, year: year, month: month }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var html = '';

                    var total10 = 0;
                    var total20 = 0;
                    var total30 = 0;
                    var total40 = 0;
                    var total50 = 0;
                    var total60 = 0;
                    var total61 = 0;

                    $.each(result, function (index, value) {
                        debugger;

                        total10 = total10 + value.Oneto10;
                        total20 = total20 + value.Tento20;
                        total30 = total30 + value.Twentyto30;
                        total40 = total40 + value.Thirtyto40;
                        total50 = total50 + value.Fortyto50;
                        total60 = total60 + value.Fiftyto60;
                        total61 = total61 + value.SixtyPlus;

                        html += '<tr>';
                        html += '<td>' + (parseInt(index) + 1) + '</td>';
                        html += '<td>' + value.RegionName + '</td>';
                        html += '<td>' + numberWithCommas(value.Oneto10) + '</td>';
                        html += '<td>' + numberWithCommas(value.Tento20) + '</td>';
                        html += '<td>' + numberWithCommas(value.Twentyto30) + '</td>';
                        html += '<td>' + numberWithCommas(value.Thirtyto40) + '</td>';
                        html += '<td>' + numberWithCommas(value.Fortyto50) + '</td>';
                        html += '<td>' + numberWithCommas(value.Fiftyto60) + '</td>';
                        html += '<td>' + numberWithCommas(value.SixtyPlus) + '</td>';
                        html += '</tr>';

                    });

                    html += '<tr style="font-weight: bold;">';
                    html += '<td></td>';
                    html += '<td> Total (Tk.) </td>';
                    html += '<td>' + numberWithCommas(parseFloat(total10).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(total20).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(total30).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(total40).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(total50).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(total60).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(total61).toFixed(2)) + '</td>';
                    html += '</tr>';

                    $('#agling-detail').html(html);

                    $("#ageing-spinner").hide();
                }
            });
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function LoadBusinessSummeryReport() {

            var timePeriod = $("input[name='inlineRadioOptions1']:checked").val();

            debugger;

            var companyId = $('#ddlBusinessSummeryCompany').val();
            var year = $('#ddlBusinessSummeryFinancialYear').val();
            var month = $('#ddlBusinessSummeryMonth').val();

            $("#business-spinner").show();

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetBusinessSummeryReport",
                data: JSON.stringify({ companyId: companyId, timePeriod: timePeriod, year: year, month: month }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var html = '';

                    var prevDueTotal = 0;
                    var todDueTotal = 0;
                    var dueTotal = 0;

                    var prevDueCollectionTotal = 0;
                    var todDueCollectionTotal = 0;
                    var dueCollectionTotal = 0;

                    $.each(result, function (index, value) {
                        debugger;


                        prevDueTotal = prevDueTotal + value.PreviousDue;
                        todDueTotal = todDueTotal + value.TodaysDue;
                        dueTotal = dueTotal + value.TotalDue;

                        prevDueCollectionTotal = prevDueCollectionTotal + value.PrevousDueCollection;
                        todDueCollectionTotal = todDueCollectionTotal + value.TodaysCollection;
                        dueCollectionTotal = dueCollectionTotal + value.TotalCollection;

                        html += '<tr>';
                        html += '<td>' + (parseInt(index) + 1) + '</td>';
                        html += '<td>' + value.RegionName + '</td>';
                        html += '<td>' + numberWithCommas(value.PreviousDue) + '</td>';
                        html += '<td>' + numberWithCommas(value.TodaysDue) + '</td>';
                        html += '<td>' + numberWithCommas(value.TotalDue) + '</td>';
                        html += '<td>' + numberWithCommas(value.PrevousDueCollection) + '</td>';
                        html += '<td>' + numberWithCommas(value.TodaysCollection) + '</td>';
                        html += '<td>' + numberWithCommas(value.TotalCollection) + '</td>';
                        html += '</tr>';

                    });

                    html += '<tr style="font-weight: bold;">';
                    html += '<td></td>';
                    html += '<td>Total Amount (Tk.)</td>';
                    html += '<td>' + numberWithCommas(parseFloat(prevDueTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(todDueTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(dueTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(prevDueCollectionTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(todDueCollectionTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(dueCollectionTotal).toFixed(2)) + '</td>';
                    html += '</tr>';

                    $('#BusinessSummery-detail').html(html);
                    $("#business-spinner").hide();

                }
            });
        }


        function LoadFinancialYear() {

            var salesTrendCompanyId = $('#ddlSalesTrendCompany').val();

            debugger;

            $.ajax({


                type: "POST",
                url: "Dashboard.aspx/GetFinancialYear",
                data: JSON.stringify({ companyId: salesTrendCompanyId }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {


                    var result = data.d;

                    $('#ddlSalesTrendFinancialYear').empty();

                    for (var i in result) {
                        $('#ddlSalesTrendFinancialYear').append($("<option></option>").val(result[i].FinancialYearId)
                            .html(result[i].FinancialYear));
                    }


                }
            });
        }

        function LoadFinancialYearMonth() {

            var salesCompanyId = $('#ddlSalesCompany').val();



            //$.ajax({


            //    type: "POST",
            //    url: "Dashboard.aspx/GetFinancialYear",
            //    data: JSON.stringify({ companyId: salesCompanyId }),
            //    dataType: "JSON",
            //    contentType: "application/json;charset=utf-8",
            //    async: false,
            //    success: function (data) {



            //        var result = data.d;

            //        $('#ddlSalesFinancialYear').empty();

            //        for (var i in result) {
            //            $('#ddlSalesFinancialYear').append($("<option></option>").val(result[i].FinancialYearId)
            //                .html(result[i].FinancialYear));
            //        }


            //    }
            //});
        }


        function LoadDropDownlist() {

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlCardCompany').empty();

                    for (var i in result) {
                        $('#ddlCardCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlCardCompany').val();

                }
            });


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    $('#ddlTopPriorityCompany').empty();

                    for (var i in result) {
                        $('#ddlTopPriorityCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlTopPriorityCompany').val();

                }
            });
            

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetRegionInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    $('#ddlRegion').empty();

                    for (var i in result) {
                        $('#ddlRegion').append($("<option></option>").val(result[i].RegionId)
                            .html(result[i].RegionName));
                    }

                    $('#ddlRegion').val();

                }
            });
            
            
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlTopCustomerCompany').empty();

                    for (var i in result) {
                        $('#ddlTopCustomerCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlTopCustomerCompany').val();

                }
            });

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlExpiredProductCompany').empty();

                    for (var i in result) {
                        $('#ddlExpiredProductCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlExpiredProductCompany').val();

                }
            });


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlSalesTrendCompany').empty();

                    for (var i in result) {
                        $('#ddlSalesTrendCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlSalesTrendCompany').val();

                }
            });

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlSalesCompany').empty();

                    for (var i in result) {
                        $('#ddlSalesCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlSalesCompany').val();

                }
            });



            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlAglingCompany').empty();

                    for (var i in result) {
                        $('#ddlAglingCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlAglingCompany').append("<option value='0'>All Wings </option>");

                    $('#ddlAglingCompany').val();

                }
            });

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlBusinessSummeryCompany').empty();

                    for (var i in result) {
                        $('#ddlBusinessSummeryCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlBusinessSummeryCompany').append("<option value='0'>All Wings</option>");

                    $('#ddlBusinessSummeryCompany').val();

                }
            });

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyInfo",
                data: JSON.stringify({}),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#ddlProductSalesCompany').empty();

                    for (var i in result) {
                        $('#ddlProductSalesCompany').append($("<option></option>").val(result[i].CompanyId)
                            .html(result[i].CompanyName));
                    }

                    $('#ddlProductSalesCompany').val();

                }
            });


            var salesTrendCompanyId = $('#ddlSalesTrendCompany').val();



            //$.ajax({


            //    type: "POST",
            //    url: "Dashboard.aspx/GetFinancialYear",
            //    data: JSON.stringify({ companyId: salesTrendCompanyId }),
            //    dataType: "JSON",
            //    contentType: "application/json;charset=utf-8",
            //    async: false,
            //    success: function (data) {



            //        var result = data.d;

            //        $('#ddlSalesTrendFinancialYear').empty();

            //        for (var i in result) {
            //            $('#ddlSalesTrendFinancialYear').append($("<option></option>").val(result[i].FinancialYearId)
            //                .html(result[i].FinancialYear));
            //        }


            //    }
            //});

            //var salesCompanyId = $('#ddlSalesCompany').val();

            //$.ajax({
            //    type: "POST",
            //    url: "Dashboard.aspx/GetFinancialYear",
            //    data: JSON.stringify({ companyId: salesCompanyId }),
            //    dataType: "JSON",
            //    contentType: "application/json;charset=utf-8",
            //    async: false,
            //    success: function (data) {



            //        var result = data.d;

            //        $('#ddlSalesFinancialYear').empty();

            //        for (var i in result) {
            //            $('#ddlSalesFinancialYear').append($("<option></option>").val(result[i].FinancialYearId)
            //                .html(result[i].FinancialYear));
            //        }


            //    }
            //});


        }


        function SetDashBoardCardInfo() {


            var companyId = $('#ddlCardCompany').val();

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetDashBoardCardInfo",
                data: JSON.stringify({ companyId: companyId }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    for (var i in result) {

                        $("#OrderNo").html(result[i].NoOfOrder);
                        $("#invoiceNo").html(result[i].NoOfInvoice);
                        $("#actualSales").html(result[i].ActualSales);
                        $("#deliveryNo").html(result[i].DeliveryConfirmed);
                        $("#collection").html(result[i].TotalCollection);
                        $("#due").html(numberWithCommas(parseFloat(result[i].TotalDue).toFixed(2)));
                    }

                    $('#productsales-detail').html(html);



                }
            });
        }

        function LoadProductSalesDataTable() {

            var fromDate = $('#txtFromDate').val();
            var toDate = $('#txtToDate').val();
            var companyId = $('#ddlProductSalesCompany').val();


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyWiseProductSales",
                data: JSON.stringify({ companyId: companyId, fromDate: fromDate, toDate: toDate }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;
                    var value = [];
                    var name = [];
                    var label = [];

                    var html = '';

                    var totalSales = 0;

                    for (var i in result) {

                        totalSales = totalSales + result[i].SalesValue;

                        html += '<tr>';
                        html += '<td>' + (parseInt(i) + 1) + '</td>';
                        html += '<td>' + result[i].ProductName + '</td>';
                        html += '<td>' + numberWithCommas(result[i].SalesQuantity) + '</td>';
                        html += '<td>' + numberWithCommas(result[i].SalesValue) + '</td>';
                        html += '</tr>';

                    }

                    html += '<tr style="font-weight: bold;">';
                    html += '<td></td>';
                    html += '<td></td>';
                    html += '<td> Total Amount (Tk.)</td>';
                    html += '<td>' + numberWithCommas(parseFloat(totalSales).toFixed(2)) + '</td>';
                    html += '</tr>';

                    $('#productsales-detail').html(html);



                }
            });
        }


        function LoadSalesTrendDataTable() {


            var financialYear = $('#ddlSalesTrendFinancialYear').val();


            if (financialYear == 0) {
                financialYear = new Date().getFullYear();
            }


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetWingsSalesTrend",
                data: JSON.stringify({ year: financialYear }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    debugger;

                    var result = data.d;
                    var label = [];

                    var valueArray = new Array();


                    for (var i in result) {

                        if (label.indexOf(result[i].MonthName) == -1) {
                            label.push(result[i].MonthName);
                        }

                    }


                    $.each(label, function (index, value) {

                        debugger;

                        var babyCare = 0;
                        var criticalCare = 0;
                        var dermal = 0;
                        var gloves = 0;
                        var gyno = 0;
                        var nutrition = 0;
                        var noc = 0;
                        var onco = 0;
                        var opthalmic = 0;
                        var otc = 0;
                        var dental = 0;
                        var hematology = 0;
                        var neurology = 0;

                        for (var j in result) {

                            if (value == result[j].MonthName && result[j].CompanyName == "Baby Care") {
                                babyCare = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Critical Care") {
                                criticalCare = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Dermal") {
                                dermal = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Gloves & Instruments") {
                                gloves = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Gyno") {
                                gyno = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Nutrition") {
                                nutrition = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "NOC") {
                                noc = result[i].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Onco") {
                                onco = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Opthalmic") {
                                opthalmic = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "OTC") {
                                otc = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Dental ( Clinic )") {
                                dental = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Hematology") {
                                hematology = result[j].SalesValue;
                            }

                            if (value == result[j].MonthName && result[j].CompanyName == "Neurology") {
                                neurology = result[j].SalesValue;
                            }


                        }


                        valueArray[index] = new Array(value, criticalCare, dermal, gloves, gyno, nutrition, noc, onco, opthalmic, otc, dental, babyCare, hematology, neurology);
                    });

                    console.log(valueArray);


                    var criticalTotal = 0;
                    var dermalTotal = 0;
                    var glovesTotal = 0;
                    var gynoTotal = 0;
                    var nutritionTotal = 0;
                    var nocTotal = 0;
                    var oncoTotal = 0;
                    var opthalmicTotal = 0;
                    var otcTotal = 0;
                    var dentalTotal = 0;
                    var babyCareTotal = 0;
                    var hematologyTotal = 0;
                    var neurologyTotal = 0;

                    var html = '';

                    $.each(valueArray, function (index, value) {

                        debugger;

                        criticalTotal = criticalTotal + value[1];
                        dermalTotal = dermalTotal + value[2];
                        glovesTotal = glovesTotal + value[3];
                        gynoTotal = gynoTotal + value[4];
                        nutritionTotal = nutritionTotal + value[5];
                        nocTotal = nocTotal + value[6];
                        oncoTotal = oncoTotal + value[7];
                        opthalmicTotal = opthalmicTotal + value[8];
                        otcTotal = otcTotal + value[9];
                        dentalTotal = dentalTotal + value[10];
                        babyCareTotal = babyCareTotal + value[11];
                        hematologyTotal = hematologyTotal + value[12];
                        neurologyTotal = neurologyTotal + value[13];

                        html += '<tr>';
                        html += '<th>' + (parseInt(index) + 1) + '</th>';
                        html += '<th>' + numberWithCommas(value[0]) + '</th>';
                        html += '<td>' + numberWithCommas(value[1]) + '</td>';
                        html += '<td>' + numberWithCommas(value[2]) + '</td>';
                        html += '<td>' + numberWithCommas(value[3]) + '</td>';
                        html += '<td>' + numberWithCommas(value[4]) + '</td>';
                        html += '<td>' + numberWithCommas(value[5]) + '</td>';
                        html += '<td>' + numberWithCommas(value[6]) + '</td>';
                        html += '<td>' + numberWithCommas(value[7]) + '</td>';
                        html += '<td>' + numberWithCommas(value[8]) + '</td>';
                        html += '<td>' + numberWithCommas(value[9]) + '</td>';
                        html += '<td>' + numberWithCommas(value[10]) + '</td>';
                        html += '<td>' + numberWithCommas(value[11]) + '</td>';
                        html += '<td>' + numberWithCommas(value[12]) + '</td>';
                        html += '<td>' + numberWithCommas(value[13]) + '</td>';
                        html += '</tr>';
                    });

                    html += '<tr style="font-weight: bold;">';
                    html += '<td colspan="2" style="text-align: right;">Total Sales Amount (Tk.)</td>';
                    html += '<td>' + numberWithCommas(parseFloat(criticalTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(dermalTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(glovesTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(gynoTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(nutritionTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(nocTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(oncoTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(opthalmicTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(otcTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(dentalTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(babyCareTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(hematologyTotal).toFixed(2)) + '</td>';
                    html += '<td>' + numberWithCommas(parseFloat(neurologyTotal).toFixed(2)) + '</td>';
                    html += '</tr>';


                    $('#salestrend-detail').html(html);


                },
                error: function () {
                    alert('Failed');
                }
            });
        }
        function LoadWingsSalesTrendChart() {

            var financialYear = $('#ddlSalesTrendFinancialYear').val();


            if (financialYear == 0) {
                financialYear = new Date().getFullYear();
            }


            $("#salesTrend-spinner").show();

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetWingsSalesTrend",
                data: JSON.stringify({ year: financialYear }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var value = [];
                    var name = [];
                    var label = [];

                    var babyCareArray = [];
                    var darmalArray = [];
                    var dentalArray = [];
                    var criticalCareArray = [];
                    var globsArray = [];
                    var gynoArray = [];
                    var nocArray = [];
                    var nutritionArray = [];
                    var oncoArray = [];
                    var opthalmicArray = [];
                    var otcArray = [];
                    var hematology = [];
                    var neurology = [];



                    for (var i in result) {

                        if (label.indexOf(result[i].MonthName) == -1) {
                            label.push(result[i].MonthName);
                        }

                        if (result[i].CompanyName == "Baby Care") {
                            babyCareArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Dental ( Clinic )") {
                            dentalArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Dermal") {
                            darmalArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Critical Care") {
                            criticalCareArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Gloves & Instruments") {
                            globsArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Gyno") {
                            gynoArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "NOC") {
                            nocArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Nutrition") {
                            nutritionArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Onco") {
                            oncoArray.push(result[i].SalesValue);
                        }


                        if (result[i].CompanyName == "Opthalmic") {
                            opthalmicArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "OTC") {
                            otcArray.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Hematology") {
                            hematology.push(result[i].SalesValue);
                        }

                        if (result[i].CompanyName == "Neurology") {
                            neurology.push(result[i].SalesValue);
                        }

                    }

                    console.log(label);

                    Highcharts.chart('container', {
                        chart: {
                            type: 'line'
                        },
                        title: {
                            text: ''
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            categories: label
                        },
                        yAxis: {
                            title: {
                                text: 'Amount (TK.)'
                            }
                        },
                        plotOptions: {
                            line: {
                                dataLabels: {
                                    enabled: true
                                },
                                enableMouseTracking: false
                            }
                        },
                        series: [{
                            name: 'Baby Care',
                            data: babyCareArray
                        }, {
                            name: 'Critical Care',
                            data: criticalCareArray
                        }, {
                            name: 'Dental Clinic',
                            data: dentalArray
                        }, {
                            name: 'Gloves & Instruments',
                            data: globsArray
                        }, {
                            name: 'Gyno',
                            data: gynoArray
                        }, {
                            name: 'Darmal',
                            data: darmalArray
                        }, {
                            name: 'NOC',
                            data: nocArray
                        }, {
                            name: 'Nutrition',
                            data: nutritionArray
                        }, {
                            name: 'Onco',
                            data: oncoArray
                        }, {
                            name: 'Opthalmic',
                            data: opthalmicArray
                        }, {
                            name: 'OTC',
                            data: otcArray
                        }, {
                            name: 'Hematology',
                            data: hematology
                        }, {
                            name: 'Neurology',
                            data: neurology
                        }]


                    });

                    $("#salesTrend-spinner").hide();
                },
                error: function () {
                    alert('505 - Request Denied !!');
                }
            });


        }

        function LoadWingsSalesStatusDataTable() {

            var companyId = $('#ddlSalesCompany').val();
            var financialYearId = $('#ddlSalesFinancialYear').val();
            var monthId = $('#ddlSalesMonth').val();




            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyWiseSales",
                data: JSON.stringify({ companyId: companyId, year: financialYearId, month: monthId }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var html = '';

                    var salesTotal = 0;
                    var collectionTotal = 0;
                    var dueTotal = 0;

                    for (var i in result) {

                        salesTotal = salesTotal + result[i].SalesValue;
                        collectionTotal = collectionTotal + result[i].CollectionValue;
                        dueTotal = result[i].DueValue;

                        html += '<tr>';
                        html += '<td>' + (parseInt(i) + 1) + '</td>';
                        html += '<td>' + result[i].InvoiceDate + '</td>';
                        html += '<td>' + numberWithCommas(result[i].SalesValue) + '</td>';
                        html += '<td>' + numberWithCommas(result[i].CollectionValue) + '</td>';
                        html += '<td>' + numberWithCommas(result[i].DueValue) + '</td>';
                        html += '</tr>';

                    }


                    html += '<tr style="font-weight: bold;">';
                    html += '<td></td>';
                    html += '<td> Total Amount </td>';
                    html += '<td>' + numberWithCommas(salesTotal) + '</td>';
                    html += '<td>' + numberWithCommas(collectionTotal) + '</td>';
                    html += '<td>' + numberWithCommas(dueTotal) + ' (End of the month)</td>';
                    html += '</tr>';

                    $('#salesstatus-detail').html(html);


                },
                error: function () {
                    alert('Failed');
                }
            });
        }

        function LoadSalesFunnelDataTable() {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetGroupSalesCompanyWise",
                data: JSON.stringify({ fromDate: '2020-09-01 00:00:00.000', toDate: '2020-09-30 00:00:00.000' }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var totalCount = 0;

                    for (var j in result) {

                        totalCount = parseFloat(totalCount) + parseFloat(result[j].SalesValue);
                    }




                    var html = '';

                    for (var i in result) {

                        var parcent = 0;

                        parcent = parseFloat(parcent) + ((parseFloat(result[i].SalesValue) * 100) / totalCount);

                        html += '<tr>';
                        html += '<td>' + (parseInt(i) + 1) + '</td>';
                        html += '<td>' + result[i].CompanyName + '</td>';
                        html += '<td>' + numberWithCommas(result[i].SalesValue) + '</td>';
                        html += '<td>' + parseFloat(parcent).toFixed(2) + ' % </td>';
                        html += '</tr>';


                    }


                    html += '<tr style="font-weight: bold;">';
                    html += '<td></td>';
                    html += '<td>Total Amount (Tk.)</td>';
                    html += '<td>' + numberWithCommas(parseFloat(totalCount).toFixed(2)) + '</td>';
                    html += '<td>100% </td>';
                    html += '</tr>';

                    $('#salesfunnel-detail').html(html);


                }
            });
        }

        function LoadWingsWisePerDaySales() {

            var companyId = $('#ddlSalesCompany').val();
            var financialYearId = $('#ddlSalesFinancialYear').val();
            var monthId = $('#ddlSalesMonth').val();

            $("#monthlySales-spinner").show();


            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyWiseSales",
                data: JSON.stringify({ companyId: companyId, year: financialYearId, month: monthId }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var salesValue = [];
                    var collectionValue = [];
                    var dueValue = [];
                    var label = [];

                    for (var i in result) {

                        salesValue.push(result[i].SalesValue);
                        collectionValue.push(result[i].CollectionValue);
                        dueValue.push(result[i].DueValue);
                        label.push(result[i].InvoiceDate);

                    }

                    console.log(dueValue);


                    Highcharts.chart('dc-chart-container', {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: ''
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            type: 'date',
                            categories: label,
                            crosshair: true
                        },
                        yAxis: {
                            min: 0,
                            labels: {
                                rotation: -90,
                                style: {
                                    fontSize: '13px',
                                    fontFamily: 'Verdana, sans-serif'
                                }
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                              '<td style="padding:0"><b>{point.y:.1f} TK.</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0,
                                borderWidth: 0,
                                dataLabels: {
                                    enabled: true,
                                    format: '{point.y:.1f}'
                                }
                            }
                        },
                        series: [{
                            name: 'Sales',
                            data: salesValue

                        }, {
                            name: 'Collection',
                            data: collectionValue

                        }
                            //, {
                            //name: 'Due',
                            //data: dueValue

                            //}
                        ]
                    });

                    $("#monthlySales-spinner").hide();
                },
                error: function () {
                    alert('Failed');
                }
            });


        }
        function LoadSalesFunnel() {



            $("#salesFunnel-spinner").show();

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetGroupSalesCompanyWise",
                data: JSON.stringify({ fromDate: '2020-09-01 00:00:00.000', toDate: '2020-09-30 00:00:00.000' }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var totalCount = 0;

                    for (var j in result) {

                        totalCount = parseFloat(totalCount) + parseFloat(result[j].SalesValue);
                    }

                    var wingseArray = new Array();

                    for (var i in result) {

                        var parcent = 0;
                        parcent = parseFloat(parcent) + ((parseFloat(result[i].SalesValue) * 100) / totalCount);

                        wingseArray[i] = new Array(result[i].CompanyName, result[i].SalesValue);
                    }

                    console.log(wingseArray);

                    Highcharts.chart('sales-funnel', {
                        chart: {
                            type: 'funnel'
                        },
                        title: {
                            text: ''
                        },
                        plotOptions: {
                            series: {
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b> ({point.y:,.0f})',
                                    softConnector: true
                                },
                                center: ['35%', '50%'],
                                neckWidth: '30%',
                                neckHeight: '30%',
                                width: '70%'
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        series: [{
                            name: 'Company Contribution',
                            data: wingseArray
                        }],

                        responsive: {
                            rules: [{
                                condition: {
                                    maxWidth: 500
                                },
                                chartOptions: {
                                    plotOptions: {
                                        series: {
                                            dataLabels: {
                                                inside: true
                                            },
                                            center: ['50%', '50%'],
                                            width: '100%'
                                        }
                                    }
                                }
                            }]
                        }
                    });

                    $("#salesFunnel-spinner").hide();

                }
            });
        }


        function LoadDcWiseSalesChart() {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyWiseDcSales",
                data: JSON.stringify({ companyId: '1', fromDate: '2020-09-01 00:00:00.000', toDate: '2020-09-30 00:00:00.000' }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;

                    var babyCareArray = [];
                    var darmalArray = [];
                    var dentalArray = [];
                    var criticalCareArray = [];
                    var globsArray = [];
                    var gynoArray = [];
                    var nocArray = [];
                    var nutritionArray = [];
                    var oncoArray = [];
                    var opthalmicArray = [];
                    var otcArray = [];



                    for (var i in result) {

                        if (result[i].CompanyName == "ZAS Baby Care") {
                            babyCareArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Dental ( Clinic )") {
                            dentalArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Dermal") {
                            darmalArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Critical Care") {
                            criticalCareArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Gloves & Instruments") {
                            globsArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Gyno") {
                            gynoArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS NOC") {
                            nocArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Nutrition") {
                            nutritionArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS Onco") {
                            oncoArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }


                        if (result[i].CompanyName == "ZAS Opthalmic") {
                            opthalmicArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                        if (result[i].CompanyName == "ZAS OTC") {
                            otcArray.push({ name: result[i].UnitName, value: result[i].SalesValue });
                        }

                    }

                    //console.log(babyCareArray);



                    Highcharts.chart('dc-chart-container', {
                        chart: {
                            type: 'packedbubble',
                            height: '45%'
                        },
                        title: {
                            text: 'Company Wise'
                        },
                        tooltip: {
                            useHTML: true,
                            pointFormat: '<b>{point.name}:</b> {point.value} TK.'
                        },
                        plotOptions: {
                            packedbubble: {
                                minSize: '10%',
                                maxSize: '85%',
                                zMin: 0,
                                zMax: 500,
                                layoutAlgorithm: {
                                    splitSeries: true,
                                    gravitationalConstant: 0.3
                                },
                                dataLabels: {
                                    enabled: true,
                                    format: '{point.name}',
                                    filter: {
                                        property: 'y',
                                        operator: '>',
                                        value: 450
                                    },
                                    style: {
                                        color: 'black',
                                        textOutline: 'none',
                                        fontWeight: 'normal'
                                    }
                                }
                            }
                        },
                        series: [{
                            name: 'Baby Care',
                            data: babyCareArray
                        }, {
                            name: 'Critical Care',
                            data: criticalCareArray
                        }, {
                            name: 'Dental Clinic',
                            data: dentalArray
                        }, {
                            name: 'Gloves & Instruments',
                            data: globsArray
                        }, {
                            name: 'Gyno',
                            data: gynoArray
                        }, {
                            name: 'Darmal',
                            data: darmalArray
                        }, {
                            name: 'NOC',
                            data: nocArray
                        }, {
                            name: 'Nutrition',
                            data: nutritionArray
                        }, {
                            name: 'Onco',
                            data: oncoArray
                        }, {
                            name: 'Opthalmic',
                            data: opthalmicArray
                        }, {
                            name: 'OTC',
                            data: otcArray
                        }]
                    });



                }
            });
        }

        function LoadProductWiseSalesChart() {

            var fromDate = $('#txtFromDate').val();
            var toDate = $('#txtToDate').val();
            var companyId = $('#ddlProductSalesCompany').val();

            $("#productSales-spinner").show();

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCompanyWiseProductSales",
                data: JSON.stringify({ companyId: companyId, fromDate: fromDate, toDate: toDate }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {

                    var result = data.d;
                    var value = [];
                    var name = [];
                    var label = [];


                    for (var i in result) {

                        name.push('Sales (DC)');
                        value.push(result[i].SalesQuantity);
                        label.push(result[i].ProductName);

                    }


                    Highcharts.chart('product-chart-container', {
                        chart: {
                            type: 'bar'
                        },

                        title: {
                            text: ''
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            categories: label,
                            title: {
                                text: null
                            },
                            scrollbar: {
                                enabled: true
                            },
                            tickLength: 0
                        },
                        yAxis: {

                            title: {
                                text: '',
                                align: 'high'
                            },

                            labels: {
                                overflow: 'justify'
                            }
                        },
                        tooltip: {
                            valueSuffix: ' TK.'
                        },
                        plotOptions: {
                            bar: {
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'top',
                            x: -40,
                            y: 80,
                            floating: true,
                            borderWidth: 1,
                            backgroundColor:
                                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
                            shadow: true
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: "Sales Quantity",
                            data: value
                        }]
                    });

                }
            });

            $("#productSales-spinner").hide();
        }



    </script>
</body>
</html>

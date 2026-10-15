<%@ page language="C#" autoeventwireup="true" inherits="SInventory_UI_Dashboard, App_Web_5xlyooqp" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

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

        /*.table > tbody > tr:not(th):nth-child(even) {
            background-color: #d7e3ee !important;
        }
        */
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
            background-color: white;
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
        .head {
            font-weight: bold;
            font-size: larger;
            background: #d7e3ee !important;


        }
        .total {
            background: #9da4b0;
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
                                                <h5 class=""><i class="fas fa-1x fa-th-large "></i>&nbsp;Cash Flow </h5>
                                                <a href="../SInventory_UI/AccountsPanel.aspx" class="btn btn-sm btn-info">
                                                            <i class="feather bold icon-corner-up-right"></i>Back to list
                                                        </a>
                                            </div>
                                            <div class="card-body">

                                                <div class="row p-15">
                                                    <!-- order-card start -->


                                                    <div class="col-md-12 col-xl-12">
                                                        <div class="accordion" id="accordionExample5">
                                                            <div class="card child-card">
                                                                <div class="card-header child-card-header text-center" id="headingOne5">
                                                                    <h5 class="mb-0">
                                                                        <a href="#!" data-toggle="collapse" data-target="#collapseOne5" aria-expanded="true" aria-controls="collapseOne5" class="">Cash Flow</a>
                                                                    </h5>
                                                                </div>
                                                                <div id="collapseOne5" class="card-body mt-3 collapse show" aria-labelledby="headingOne5" data-parent="#accordionExample5" style="padding-bottom: 10px !important">
                                                                    <div class="row ">
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesTrendCompany" class="col-sm-4 col-form-label col-form-label-sm text-right">Wings name <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlSalesTrendCompany" class="form-control form-control-sm "></select>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group row">
                                                                                <label for="ddlSalesTrendFinancialYear" class="col-sm-4 col-form-label col-form-label-sm text-right">Financial Year <span class="text-sm-left text-c-green"></span>:  </label>
                                                                                <div class="col-sm-7">
                                                                                    <select id="ddlSalesTrendFinancialYear" class="form-control form-control-sm "></select>
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


                                                                    </div>
                                                                    
                                                                    <div class="row">
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group">
                                                                                
                                                                                
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group">
                                                                                
                                                                                
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group">
                                                                                <button id="view" name="View" class="btn btn-primary" >Search</button>
                                                                                
                                                                                
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group">
                                                                                
                                                                                
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                    <h1>InFlow</h1>
                                                                    <div class="row form-container">
                                                                        <div class="table-responsive">
                                                                            <table class="table table-bordered table-xs">
                                                                                <%--<thead>
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
                                                                                </thead>--%>
                                                                                <tbody id="cashinflowdetail">
                                                                                </tbody>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                    <h1>OutFlow</h1>
                                                                    <div class="row form-container">
                                                                        <div class="table-responsive">
                                                                            <table class="table table-bordered table-xs">
                                                                                <%--<thead>
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
                                                                                </thead>--%>
                                                                                <tbody id="cashoutflowdetail">
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


            LoadDropDownlist();
            

            $('#ddlSalesTrendCompany').change(function () {
                LoadFinancialYear();

            });
            $('#view').click(function () {
                GetCashflowData();

            });

            $('#ddlSalesTrendFinancialYear').change(function () {
                GetFinDateData();
            });


            



        });


        

        function LoadFinancialYear() {

            var salesTrendCompanyId = $('#ddlSalesTrendCompany').val();

            debugger;

            $.ajax({


                type: "POST",
                url: "CashFlow.aspx/GetFinancialYear",
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

        


        function LoadDropDownlist() {

            

            $.ajax({
                type: "POST",
                url: "CashFlow.aspx/GetCompanyInfo",
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

            








        }


        function GetCashflowData() {


            var companyId = $('#ddlSalesTrendCompany').val();
            var finyearId = $('#ddlSalesTrendFinancialYear').val();
            var startdate = $('#txtFromDate').val();
            var todate = $('#txtToDate').val();
            
            $.ajax({
                type: "POST",
                url: "CashFlow.aspx/GetCashflowData",
                data: JSON.stringify({ companyId: companyId,finyearId:finyearId,startdate:startdate,enddate:todate }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {


                    var catmap = new Map();
                    var subsimap = new Map();
                    

                    debugger;
                    var result = data.d;
                    var htmld = '';
                    var htmlm = '';
                    var htmlmain = '';
                    //var amount = 0;

                    var catflags = [],subsiflags = [], l = result.length, j;
                    for (j = 0; j < l; j++) {
                        if (subsiflags[result[j].SubsidiryLName]) {
                            if (catflags[result[j].Category]) continue;
                            catflags[result[j].Category] = true;
                            catmap.set(result[j].Category, 0);
                        } else {
                            subsiflags[result[j].SubsidiryLName + ':' + result[j].Category] = true;
                            subsimap.set(result[j].SubsidiryLName + ':' + result[j].Category, 0);
                            if (catflags[result[j].Category]) continue;
                            catflags[result[j].Category] = true;
                            catmap.set(result[j].Category, 0);
                        }

                    }
                    var a = 0;
                    for (let [key, value] of subsimap) {
                        var cate = key.split(':')[1];

                        if (cate == 'Inflow of Cash') {
                            //$.each(subsimap, function (key, value) {

                            var name = '';
                            var code = '';
                            var cat = '';
                            htmld = '';
                            for (var i in result) {

                                if (result[i].SubsidiryLName + ':' + result[i].Category == key) {

                                    name = result[i].SubsidiryLName;
                                    code = result[i].SubsidiryLCode;
                                    cat = result[i].Category;

                                    htmld = htmld + '<tr>';
                                    htmld = htmld + '<td>' + result[i].AccountCode + '</td>';
                                    htmld = htmld + '<td>' + result[i].AccountName + '</td>';
                                    htmld = htmld + '<td>' + result[i].AmountBDT + '</td>';
                                    htmld = htmld + '</tr>';

                                    var amount = result[i].AmountBDT;
                                    var preamnt = subsimap.get(result[i].SubsidiryLName + ':' + result[i].Category);
                                    var preamntCat = catmap.get(result[i].Category);
                                    subsimap.set(result[i].SubsidiryLName + ':' + result[i].Category, amount + preamnt);
                                    catmap.set(result[i].Category, amount + preamntCat);
                                }


                                //htmld=htmld+'<td>'+result[i].


                            }
                            htmlm = '';
                            htmlm = htmlm + '<tr class="head">';
                            htmlm = htmlm + '<td>' + code + '</td>';
                            htmlm = htmlm + '<td>' + name + '</td>';
                            htmlm = htmlm + '<td>' + subsimap.get(name + ':' + cat) + '</td>';
                            htmlm = htmlm + '</tr>';

                            htmlmain = htmlmain + htmlm + htmld;


                        }

                    }
                    htmlm = '';
                    htmlm = htmlm + '<tr class="total">';
                    htmlm = htmlm + '<td></td>';
                    htmlm = htmlm + '<td>Total</td>';
                    htmlm = htmlm + '<td>' + catmap.get("Inflow of Cash") + '</td>';
                    htmlm = htmlm + '</tr>';
                    
                    $('#cashinflowdetail').html(htmlmain+htmlm);

                    htmlmain = '';
                    for (let [key, value] of subsimap) {
                        var cate = key.split(':')[1];

                        if (cate == 'Outflow of Cash') {
                            //$.each(subsimap, function (key, value) {

                            var name = '';
                            var code = '';
                            var cat = '';
                            htmld = '';
                            for (var i in result) {

                                if (result[i].SubsidiryLName + ':' + result[i].Category == key) {

                                    name = result[i].SubsidiryLName;
                                    code = result[i].SubsidiryLCode;
                                    cat = result[i].Category;

                                    htmld = htmld + '<tr>';
                                    htmld = htmld + '<td>' + result[i].AccountCode + '</td>';
                                    htmld = htmld + '<td>' + result[i].AccountName + '</td>';
                                    htmld = htmld + '<td>' + result[i].AmountBDT + '</td>';
                                    htmld = htmld + '</tr>';

                                    var amount = result[i].AmountBDT;
                                    var preamnt = subsimap.get(result[i].SubsidiryLName + ':' + result[i].Category);
                                    var preamntCat = catmap.get(result[i].Category);
                                    subsimap.set(result[i].SubsidiryLName + ':' + result[i].Category, amount + preamnt);
                                    catmap.set(result[i].Category, amount + preamntCat);
                                }


                                //htmld=htmld+'<td>'+result[i].


                            }
                            htmlm = '';
                            htmlm = htmlm + '<tr class="head">';
                            htmlm = htmlm + '<td>' + code + '</td>';
                            htmlm = htmlm + '<td>' + name + '</td>';
                            htmlm = htmlm + '<td>' + subsimap.get(name + ':' + cat) + '</td>';
                            htmlm = htmlm + '</tr>';

                            htmlmain = htmlmain + htmlm + htmld;


                        }

                    }
                    htmlm = '';
                    htmlm = htmlm + '<tr class="total">';
                    htmlm = htmlm + '<td></td>';
                    htmlm = htmlm + '<td>Total</td>';
                    htmlm = htmlm + '<td>' + catmap.get("Outflow of Cash") + '</td>';
                    htmlm = htmlm + '</tr>';

                    $('#cashoutflowdetail').html(htmlmain+htmlm);



                }
            });
        }




       
        function GetFinDateData() {

            var salesCompanyId = $('#ddlSalesTrendCompany').val();
            var salesfinyear = $('#ddlSalesTrendFinancialYear').val();

            $.ajax({
                type: "POST",
                url: "CashFlow.aspx/GetFinancialYearDate",
                data: JSON.stringify({ companyId: salesCompanyId, finyearId: salesfinyear }),
                dataType: "JSON",
                contentType: "application/json;charset=utf-8",
                async: false,
                success: function (data) {



                    var result = data.d;

                    $('#txtFromDate').val(result[0].StartDate);
                    $('#txtToDate').val(result[0].EndDate);

                    


                }
            });

        }

       



    </script>
</body>
</html>

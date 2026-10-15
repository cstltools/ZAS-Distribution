<%@ page language="C#" autoeventwireup="true" inherits="HTMLReport_InvoicePrintPreview, App_Web_zyd4su1o" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    
    <style>
    
    @import url('https://fonts.googleapis.com/css?family=PT+Serif&display=swap');

     * {
         margin: 0;
         padding: 0;
         box-sizing: border-box;
         line-height: 1.5em;
         font-family: 'PT Serif', serif;
         letter-spacing: .1em;
     }

     .cstl-container {
         width: 100%;
         display: block;
         margin: 0 auto;
     }

     .cstl-content{
         display: flex;
         flex-direction: column;  
         justify-content: space-between;
     }

     .cstl-header,.cstl-footer{
        width: 98%;
        height: 20vh;
        /* background-color: orange; */
     }

     .cstl-master-info {
        width: 98%;
        height: auto;
        /* background-color: orangered; */

        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: .5em 2em;
     }

     .invoice-title-container{
        width: 98%;
        height: auto;
     }

     .cstl-details-info{
        width: 98%;
        height: auto;
        /* background-color: lightseagreen; */
     }

     .invoice-info-container,
     .invoice-company-info,
     .invoice-client-info
     {
         display: flex;
         flex-direction: column;
         flex: 1 0 32%;
         justify-content: space-between;
     }

     .master-heading{
         padding: .3em 0;
     }

     .cstl-header-title{
         height: 10vh;
         text-align: center;
         margin-bottom: 2em;
     }


     .invoice-title-container h2{
         background-color: #BBC0C4;
         margin: 0 auto;
         border: 1px solid #494949;
         width: 40%;
         padding: .5em 1em;
     }
    
    .title{
        padding: 0.2em 0;
    }
    .title label{
        font-weight: bold;
    }

    /* Other */


    .cstl-other{
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
    }

    .other-container {
       display: flex;
       justify-content: space-between;
       padding: .5em 2em;
    }

   

    /* Table responsive-table */

    .cstl-details-container{
         padding: .5em 2em;
     }

     caption {
        margin-bottom: 1em;
        font-size: 1em;
        font-weight: bold;
        text-align: left;
        font-size: 1.5em; 
    }

    .responsive-table{ 
        border-collapse: collapse;
        width: 100%;
        text-align: left;
    }

    thead tr{
        border-bottom: 2px solid #494949; 
    }

    tbody tr{
        border-bottom: .1em dotted gray; 
    }

    tbody tr:nth-child(even){
        background-color: #EFEEED;
    }

    thead th{
        text-transform: uppercase;
    }

    td[data-type=currency] {
      text-align: center; 
    }

    th,td{
        padding: .2em .1em;
    }  


    body.print-page
    {
           -webkit-transform: scale(0.85);  /* Saf3.1+, Chrome */
                 -moz-transform: scale(0.85);  /* FF3.5+ */
                  -ms-transform: scale(0.85);  /* IE9 */
                   -o-transform: scale(0.85);  /* Opera 10.5+ */
                      transform: scale(0.85);
            margin: -50px -73px 0;  /* big hack to reposition the page top the top and left of the viewer control */
    }

    </style>
    

    <script language="javascript">

        function printpr() {

            var prtContent = document.getElementById('print-area');
            //in between these two lines many lines there its not necessary here00
            window.print();
        }
    </script>
</head>
<body id="print-area">
    <form id="form1" runat="server">
   <div class="cstl-container">

        <div class="cstl-content">
            <!-- Header start -->
            <header class="cstl-header">
                
            </header>
    
            <!-- Header start -->
            <header class="cstl-header-title">
                <div class="invoice-title-container">
                    <h2> Invoice </h2>
                </div>
            </header>

            <!-- Report Master Information -->
            <section class="cstl-master-info">
                <div class="invoice-info-container">
                    <h2 class="master-heading"> Invoice Info </h2>
                    
                    <div class="title">
                        <label> Invoice ID: </label> <span> 2019 - 100124 </span>
                    </div>

                    <div class="title">
                        <label> Invoice Date: </label> <span> Sep 03, 2019 </span>
                    </div>

                    <div class="title">
                        <label> MIO Code: </label> <span> ZAS-0001 </span>
                    </div>

                    <div class="title">
                        <label> MIO Name: </label> <span> Jiaul Hasan </span>
                    </div>

                    <div class="title">
                        <label> Delivery Man: </label> <span> Saif Hasan </span>
                    </div>
                </div>

                <div class="invoice-company-info">
                    <h2 class="master-heading"> Sales Center </h2>
                    <div class="title">
                            <label> Invoice ID: </label> <span> 2019 - 100124 </span>
                        </div>
    
                        <div class="title">
                            <label> Invoice Date: </label> <span> Sep 03, 2019 </span>
                        </div>
    
                        <div class="title">
                            <label> MIO Code: </label> <span> ZAS-0001 </span>
                        </div>
    
                        <div class="title">
                            <label> MIO Name: </label> <span> Jiaul Hasan </span>
                        </div>
    
                        <div class="title">
                            <label> Delivery Man: </label> <span> Saif Hasan </span>
                        </div>
                </div>

                <div class="invoice-client-info">
                    <h2 class="master-heading"> Sold To </h2>
                    <div class="title">
                            <label> Invoice ID: </label> <span> 2019 - 100124 </span>
                        </div>
    
                        <div class="title">
                            <label> Invoice Date: </label> <span> Sep 03, 2019 </span>
                        </div>
    
                        <div class="title">
                            <label> MIO Code: </label> <span> ZAS-0001 </span>
                        </div>
    
                        <div class="title">
                            <label> MIO Name: </label> <span> Jiaul Hasan </span>
                        </div>
    
                        <div class="title">
                            <label> Delivery Man: </label> <span> Saif Hasan </span>
                        </div>
                </div>
            </section>
    
            <!-- Report Details Information -->
            <section class="cstl-details-info">
                <div class="cstl-details-container">
                        <table class="responsive-table">
                                <caption> Item Details </caption>
                                <thead>
                                  <tr>
                                    <th scope="col">Film Title</th>
                                    <th scope="col">Released</th>
                                    <th scope="col">Studio</th>
                                    <th scope="col">Worldwide Gross</th>
                                    <th scope="col">Domestic Gross</th>
                                    <th scope="col">Foreign Gross</th>
                                    <th scope="col">Budget</th>
                                  </tr>
                                </thead>
                                
                                <tbody>
                                  <tr>
                                    <th scope="row">Frozen</th>
                                    <td data-title="Released">2013</td>
                                    <td data-title="Studio">Disney</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,276,480,335</td>
                                    <td data-title="Domestic Gross" data-type="currency">$400,738,009</td>
                                    <td data-title="Foreign Gross" data-type="currency">$875,742,326</td>
                                    <td data-title="Budget" data-type="currency">$150,000,000</td>
                                  </tr>
                                   <tr>
                                    <th scope="row">Incredibles 2</th>
                                    <td data-title="Released">2018</td>
                                    <td data-title="Studio">Disney Pixar</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,210,072,582</td>
                                    <td data-title="Domestic Gross" data-type="currency">$606,782,977</td>
                                    <td data-title="Foreign Gross" data-type="currency">$602,369,069</td>
                                    <td data-title="Budget" data-type="currency">$200,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Minions</th>
                                    <td data-title="Released">2015</td>
                                    <td data-title="Studio">Universal</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,159,398,397</td>
                                    <td data-title="Domestic Gross" data-type="currency">$336,045,770</td>
                                    <td data-title="Foreign Gross" data-type="currency">$823,352,627</td>
                                    <td data-title="Budget" data-type="currency">$74,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Toy Story 3</th>
                                    <td data-title="Released">2010</td>
                                    <td data-title="Studio">Disney Pixar</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,066,969,703</td>
                                    <td data-title="Domestic Gross" data-type="currency">$415,004,880</td>
                                    <td data-title="Foreign Gross" data-type="currency">$651,964,823</td>
                                    <td data-title="Budget" data-type="currency">$200,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Despicable Me 3</th>
                                    <td data-title="Released">2017</td>
                                    <td data-title="Studio">Universal</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,034,799,409</td>
                                    <td data-title="Domestic Gross" data-type="currency">$264,624,300</td>
                                    <td data-title="Foreign Gross" data-type="currency">$770,175,109</td>
                                    <td data-title="Budget" data-type="currency">$80,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Finding Dory</th>
                                    <td data-title="Released">2016</td>
                                    <td data-title="Studio">Disney Pixar</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,028,570,889</td>
                                    <td data-title="Domestic Gross" data-type="currency">$486,295,561</td>
                                    <td data-title="Foreign Gross" data-type="currency">$542,275,328</td>
                                    <td data-title="Budget" data-type="currency">$175,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Zootopia</th>
                                    <td data-title="Released">2016</td>
                                    <td data-title="Studio">Disney</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$1,023,227,498</td>
                                    <td data-title="Domestic Gross" data-type="currency">$341,268,248</td>
                                    <td data-title="Foreign Gross" data-type="currency">$681,959,250</td>
                                    <td data-title="Budget" data-type="currency">$150,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Despicable Me 2</th>
                                    <td data-title="Released">2013</td>
                                    <td data-title="Studio">Universal</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$970,761,885</td>
                                    <td data-title="Domestic Gross" data-type="currency">$368,061,265</td>
                                    <td data-title="Foreign Gross" data-type="currency">$602,700,620</td>
                                    <td data-title="Budget" data-type="currency">$76,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">The Lion King</th>
                                    <td data-title="Released">1994</td>
                                    <td data-title="Studio">Disney</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$987,483,777</td>
                                    <td data-title="Domestic Gross" data-type="currency">$422,783,777</td>
                                    <td data-title="Foreign Gross" data-type="currency">$564,700,000</td>
                                    <td data-title="Budget" data-type="currency">$45,000,000</td>
                                  </tr>
                                  <tr>
                                    <th scope="row">Finding Nemo</th>
                                    <td data-title="Released">2003</td>
                                    <td data-title="Studio">Pixar</td>
                                    <td data-title="Worldwide Gross" data-type="currency">$936,743,261</td>
                                    <td data-title="Domestic Gross" data-type="currency">$380,843,261</td>
                                    <td data-title="Foreign Gross" data-type="currency">$555,900,000</td>
                                    <td data-title="Budget" data-type="currency">$94,000,000</td>
                                  </tr>
                                </tbody>
                              </table>

                </div>
            </section>

            <!-- Other Section -->
            <section class="cstl-other">
                <div class="cstl-details-container">
                    <div class="other-container">
                        <div class="other-content">
                                <!-- <div class="title">
                                    <label> MIO Name: </label> <span> Jiaul Hasan </span>
                                </div>
                                <div class="title">
                                    <label> MIO Name: </label> <span> Jiaul Hasan </span>
                                </div> -->
                            </div> 
                        <div class="other-content">
                            <div class="title">
                                <label> Total Price: </label> <span> Jiaul Hasan </span>
                            </div>
                            <div class="title">
                                <label> Total VAT: </label> <span> Jiaul Hasan </span>
                            </div>

                            <div class="title">
                                <label> Total Due: </label> <span> Jiaul Hasan </span>
                            </div>
                        </div>  
                    </div> 
                </div>                  
            </section>

            <!-- Report footer Information -->
            <section class="cstl-footer">
              <div class="cstl-details-container">
                  
              </div>
            </section>
        </div>

    </div>
    </form>
</body>
</html>

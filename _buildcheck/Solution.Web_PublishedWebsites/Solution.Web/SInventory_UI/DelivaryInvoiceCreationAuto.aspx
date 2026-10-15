<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_DelivaryInvoiceCreationAuto, App_Web_dfmjdr5z" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.css" rel="stylesheet" />
    <style type="text/css">
        .balance {
            color: #008080;
            font-size: 20px;
        }

        .deliveryMan {
            width: 250px !important;
            display: block;
            width: 100%;
            padding: .375rem 2.25rem .375rem .75rem;
            -moz-padding-start: calc(0.75rem - 3px);
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: #212529;
            background-color: #fff;
            background-image: url(data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23343a40' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e);
            background-repeat: no-repeat;
            background-position: right .75rem center;
            background-size: 16px 12px;
            border: 1px solid #ced4da;
            border-radius: .25rem;
            transition: border-color .15s ease-in-out, box-shadow .15s ease-in-out;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            margin-bottom: 1px !important;
        }
    </style>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">Delivery Invoice Creation
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            <span runat="server" visible="False">Company Name:</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown" Visible="False"
                                OnSelectedIndexChanged="manufacDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Delivery Man: 
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="delDropDownList" runat="server" CssClass="DropDown deliveryMan" AutoPostBack="True" OnSelectedIndexChanged="marketDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="dateTextBox" runat="server" Width="135px" CssClass="TextBoxCalander" AutoPostBack="True"
                                OnTextChanged="dateTextBox_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                TargetControlID="dateTextBox">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Batch No :
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="batchDropDownList" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="searchButton" runat="server" BackColor="#339933" OnClick="Button1_Click"
                                CssClass="button-padding-right" Text="Search" />
                            <asp:Button ID="clearButton" runat="server" BackColor="#cc6600" OnClick="clearButton_OnClick"
                                Text="Clear" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td class="TDLeft" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">

                                <asp:GridView ID="orderGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="ComUnitId,ManufacId,OrderId,InvoiceId">
                                    <Columns>
                                        <asp:BoundField DataField="OrderCode" HeaderText="Order No" />
                                        <asp:BoundField DataField="SubmissionDate" HeaderText="Order Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="MarketName" HeaderText="Market" />
                                        <asp:BoundField DataField="MIOCode" HeaderText="MIO Code" />
                                        <asp:BoundField DataField="MIOName" HeaderText="MIO Name" />
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice Code" />
                                        <asp:BoundField DataField="InvoiceDate" HeaderText="InvoiceDate" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="statusDropDownList" runat="server" AutoPostBack="True" OnTextChanged="statusDropDownList_OnTextChanged">
                                                    <asp:ListItem>Full</asp:ListItem>
                                                    <asp:ListItem>Partial</asp:ListItem>
                                                    <asp:ListItem>Reject</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Submit">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="reasonReturnDropDownList" Visible="False" runat="server" CssClass="TextBoxMini">
                                                    <asp:ListItem Value="1">Cash Short</asp:ListItem>
                                                    <asp:ListItem Value="2">No Order</asp:ListItem>
                                                    <asp:ListItem Value="3">Wrong Order</asp:ListItem>
                                                    <asp:ListItem Value="4">Damaged  Broken</asp:ListItem>
                                                    <asp:ListItem Value="5">Quality Issue</asp:ListItem>
                                                    <asp:ListItem Value="11">Shop Closed</asp:ListItem>
                                                    <asp:ListItem Value="6">Order Cancelled</asp:ListItem>
                                                    <asp:ListItem Value="7">Slow Moving</asp:ListItem>
                                                    <asp:ListItem Value="8">Price Error</asp:ListItem>
                                                    <asp:ListItem Value="9">Return Others</asp:ListItem>
                                                    <asp:ListItem Value="10">Date Expired</asp:ListItem>
                                                </asp:DropDownList>
                                                <contenttemplate>
                                                    <asp:Button ID="gotoinvoiceButton" runat="server" Text="Submit" OnClick="gotoinvoiceButton_Click"
                                                        OnClientClick="return confirm('Are you sure you want to Submit ?');" />
                                                </contenttemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
                    
                    
                     <!-- Select2 Initialization Script -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $(".deliveryMan").select2({
                placeholder: "Select an option",
                allowClear: true
            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(".deliveryMan").select2({
                placeholder: "Select an option",
                allowClear: true
            });
        });
    </script>


</asp:Content>

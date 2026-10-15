<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ProductEntry, App_Web_pkilq3oa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.css" rel="stylesheet" />
    
    <style type="text/css">
    /* Style the table to use Flexbox for inline layout */
    #ContentPlaceHolder1_cbxTradeOrRecipe {
        display: flex; /* Flexbox for horizontal alignment */
        gap: 20px; /* Space between radio button groups */
        align-items: center; /* Align items vertically */
        justify-content: flex-start; /* Align items to the start */
    }

    /* Optional: Styling for labels */
    #ContentPlaceHolder1_cbxTradeOrRecipe label {
        margin-left: 5px; /* Space between the radio button and its label */
        font-size: 14px; /* Font size for labels */
        cursor: pointer; /* Add a pointer cursor to labels */
    }

    /* Optional: Style for radio buttons */
    #ContentPlaceHolder1_cbxTradeOrRecipe input[type="radio"] {
        cursor: pointer; /* Add pointer cursor */
    }

    .RawMaterials {

            width:250px !important;
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
                        <td colspan="6" class="TableHeading">Product Entry
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: center;">&nbsp; View List </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productListImageButton" runat="server"
                                ImageUrl="~/images/viewList.png" OnClick="productImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Company Name <span class="star-mark">* </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Product Code <span class="star-mark">* </span></td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="productCodeTextBox_OnTextChanged"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">&nbsp;</td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Product Name <span class="star-mark">* </span></td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="productNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="productNameTextBox_OnTextChanged"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Description  <span class="star-mark">* </span></td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="descriptionTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Category <span class="star-mark">* </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="categoryNameDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Manufacturer <span class="star-mark">* </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="manufacDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Pack Size <span class="star-mark">* </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="packSizeDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">UOM <span class="star-mark">* </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="stockUOMDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Product Type<span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="typeDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Product Brand<span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="productSQDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Shipping Carton 
                            
                            <%--<span class="star-mark"> * </span>--%>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="shippingCartonSizeDropDownList" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">

                            <asp:RadioButtonList ID="cbxTradeOrRecipe"
                                AutoPostBack="True"
                                CellPadding="5"
                                CellSpacing="5"
                                RepeatColumns="2"
                                RepeatDirection="Vertical"
                                TextAlign="Right"
                                OnSelectedIndexChanged="cbxTradeOrRecipe_Clicked"
                                runat="server">
                                 <asp:ListItem Value="Trade" Selected="True" > Trade </asp:ListItem>
                                 <asp:ListItem Value="Recipe" > Recipe </asp:ListItem>
                               
                               
                               

                            </asp:RadioButtonList>

                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    
                    <tr >
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Raw Material Name:<span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">

                             <asp:DropDownList ID="ddlRawMaterials"   runat="server"
                                CssClass="DropDown RawMaterials">
                            </asp:DropDownList>

                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click1"
                                Text="Submit" />

                            <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E1B12C" runat="server" OnClick="clearButton_OnClick"
                                Text="Cancel" />
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
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
            $(".RawMaterials").select2({
                placeholder: "Select an option",
                allowClear: true
            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(".RawMaterials").select2({
                placeholder: "Select an option",
                allowClear: true
            });
        });
    </script>


</asp:Content>


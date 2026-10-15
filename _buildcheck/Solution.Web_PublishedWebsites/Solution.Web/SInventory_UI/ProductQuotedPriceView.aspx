<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ProductDiscountView, App_Web_clqxceh0" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    
    <style type="text/css">
        .button-padding-right {
            margin-right: 5px;
        }

        .style1 {
            vertical-align: TOP;
            text-align: LEFT;
            font-weight: BOLD;
            font-size: 7pt;
            color: #2E2E2E;
            font-family: Verdana,Arial,Times New Roman;
            text-decoration: NONE;
            background: #F2F2F2;
            height: 9px;
        }

        .style2 {
            vertical-align: TOP;
            text-align: LEFT;
            font-weight: NONE;
            font-size: 9pt;
            color: #000000;
            font-family: Estrangelo Edessa,Arial,Times New Roman;
            text-decoration: NONE;
            background: #F2F2F2;
            height: 9px;
        }


        #ContentPlaceHolder1_excelButton {
            background: #00b894;
        }
    </style>

    <style type="text/css">
        /*AutoComplete flyout */
        .autocomplete_completionListElement {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: default;
            overflow: auto;
            font-family: Calibri;
            font-size: 12px;
            text-align: left;
            list-style-type: none;
            margin-left: 0px;
            padding-left: 0px;
            max-height: 350px;
            width: 40% !important;
        }

        /* AutoComplete highlighted item */

        .autocomplete_highlightedListItem {
            background-color: yellow;
            color: black;
            padding: 1px;
        }

        /* AutoComplete item */

        .autocomplete_listItem {
            background-color: white;
            color: blue;
            padding: 0px;
        }
    </style>
    

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
    
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading"> Product Quoted Price View </td>
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
                        <td width="13%" class="TDLeft" style="text-align: center">Add New</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productInfoNewImageButton" runat="server"
                                ImageUrl="~/images/Add.png" OnClick="miaTargetNewImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: center">Reload</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productInfoReloadImageButton" runat="server"
                                ImageUrl="~/images/refresh.png"
                                OnClick="miaTargetReloadImageButton_Click" />
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
                    <tr runat="server" visible="True" id="reportType">
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Company Name :
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="CompanyDropDown" AutoPostBack="True" OnSelectedIndexChanged="CompanyDropDown_OnSelectedIndexChanged" runat="server" CssClass="DropDown" Width="175px">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                    </tr>



                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Customer Code: &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custCodeTextBox" runat="server" CssClass="textbox" AutoPostBack="True"
                                OnTextChanged="custCodeTextBox_TextChanged"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCustomernew"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="custCodeTextBox" UseContextKey="True">
                            </ajaxToolkit:AutoCompleteExtender>
                            <asp:HiddenField ID="CustomerIdHiddenField" runat="server" />
                            
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
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

                        <td width="20%" class="TDRight" colspan="6">
                            <div id="gridContainer1" style="height: 400px; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview" DataKeyNames="QuotedPriceId"
                                    OnRowCommand="loadGridView_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                        <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="QuotedPrice" HeaderText="Quoted Price" />
                                        <asp:BoundField DataField="ActiveDate" HeaderText="Active Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="InactiveDate" HeaderText="Inactive Date"  DataFormatString="{0:dd-MMM-yyyy}"/>
                                        <asp:BoundField DataField="ActionStatus" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Edit" Visible="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="editImageButton" runat="server"
                                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData"
                                                    ImageUrl="~/images/edit.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton2" runat="server"
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="DeleteData" ImageUrl="~/images/lineDelete.png"/>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
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
    
    
    
    

</asp:Content>


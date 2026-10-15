<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ProductDiscountView, App_Web_htam3fti" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /*AutoComplete flyout */
        .autocomplete_completionListElement
        {
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
        
        .autocomplete_highlightedListItem
        {
            background-color: yellow;
            color: black;
            padding: 1px;
        }
        
        /* AutoComplete item */
        
        .autocomplete_listItem
        {
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
                        <td colspan="6" class="TableHeading">Product Discount View</td>
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
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Group Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="groupDropDownList" Width="180px" runat="server" CssClass="DropDown"
                                Enabled="False" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Company Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" Width="180px" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Customer Code:
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
                            <asp:HiddenField ID="custIdHiddenField" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Search" />
                            <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E8BC31" runat="server"
                                OnClick="clearButton_OnClick" Text="Reload" />
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>

                        <td width="20%" class="TDRight" colspan="6">
                            <div id="gridContainer1" style="height: 400px; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview" DataKeyNames="DiscountId"
                                    OnRowCommand="loadGridView_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                        <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="DiscountPercentage" HeaderText="Discount(%)" />
                                        <%--<asp:BoundField DataField="ActiveDate" HeaderText="Active Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="InactiveDate" HeaderText="Inactive Date"  DataFormatString="{0:dd-MMM-yyyy}"/>--%>
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


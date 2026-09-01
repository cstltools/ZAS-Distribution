<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="CustMasterView.aspx.cs" Inherits="SInventory_UI_CustMasterView" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Customer Master View
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
                        <td width="13%" class="TDLeft" style="text-align: center">
                            Add new
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="cusMasterNewImageButton" runat="server" ImageUrl="~/images/Add.png"
                                OnClick="CustMasterNewImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: center">
                            Reload
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="custMasterReloadImageButton" runat="server" ImageUrl="~/images/refresh.png"
                                OnClick="CustMasterReloadImageButton_Click" />
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
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Customer Code:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custcodenameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Customer Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="customerNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" OnTextChanged="customerNameTextBox_OnTextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                ServiceMethod="GetCustomerInfo" ServicePath="WebService.asmx" TargetControlID="customerNameTextBox"
                                UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Region Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="regionDropDownList" runat="server" CssClass="DropDown" AutoPostBack="true"
                                OnTextChanged="regionDropDownList_OnTextChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Area Code:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="areaDropDownList" runat="server" AutoPostBack="true" CssClass="DropDown"
                                OnTextChanged="areaDropDownList_OnTextChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Territory Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="territoryDropDownList" runat="server" AutoPostBack="true" CssClass="DropDown"
                                OnTextChanged="territoryDropDownList_OnTextChanged">
                            </asp:DropDownList>
                            <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                                Market Name:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:DropDownList ID="marketDropDownList" runat="server" CssClass="DropDown" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                    </tr>
                    <tr >
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Payment Type:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:RadioButtonList ID="paymentTypeDropDownList" RepeatDirection="Vertical"
                                TextAlign="Left" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="cash">Cash</asp:ListItem>
                                <asp:ListItem Value="credit">Credit</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
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
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%" style="text-align: center">
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" BackColor="#339933" CssClass="button-margin-right" Text="Search" />
                            
                             <asp:Button ID="cancelButton" BackColor="#cc6600" runat="server" OnClick="cancelButton_Click"
                                Text="Cancel" />
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
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
                        <td class="TDLeft" colspan="6">
                            <div id="gridContainer1" style="height: 500px; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="CustomerMasterId" OnRowCommand="loadGridView_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="ConPerson" HeaderText="Contact Person" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" />
                                        <asp:BoundField DataField="CellNo" HeaderText="CellNo" />
                                        <asp:BoundField DataField="TradelicenseNo" HeaderText="Drug License No" />
                                        <asp:BoundField DataField="MarketName" HeaderText="Market Name" />
                                        <asp:BoundField DataField="TerritoryName" HeaderText="Territory Name" />
                                        <asp:BoundField DataField="AreaName" HeaderText="Area Name" />
                                        <asp:BoundField DataField="RegionName" HeaderText="Region Name" />
                                        <asp:BoundField DataField="CategoryName" HeaderText="Category" />

                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="editImageButton" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                    CommandName="EditData" ImageUrl="~/images/edit.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--  <asp:TemplateField HeaderText="Invoice Detail">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="viewImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="viewData" 
                                                ImageUrl="~/images/viewlists.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
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
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
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
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
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

</asp:Content>

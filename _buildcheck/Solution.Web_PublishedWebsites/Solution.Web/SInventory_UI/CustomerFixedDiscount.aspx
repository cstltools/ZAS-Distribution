<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CustomerFixedDiscount, App_Web_bhssqfjy" %>
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
                        <td colspan="6" class="TableHeading">
                            Customer Fixed Discount
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
                            <asp:HiddenField ID="fixedDiscountIdHiddenField" runat="server" />
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
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                           Search Customer: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custCodeTextBox" runat="server" Width="180px" CssClass="textbox" AutoPostBack="True"
                                OnTextChanged="custCodeTextBox_TextChanged"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCustomerInfoByCode"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="custCodeTextBox" >
                            </ajaxToolkit:AutoCompleteExtender>
                        </td>
                       
                        <td class="TDLeft" width="13%">
                            <asp:HiddenField ID="custIdHiddenField" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Customer Name: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="customerNameLabel" Width="180px" runat="server" CssClass="TextBox">
                            </asp:Label>
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Discount Percent (%): <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="disPerTextBox" runat="server" Width="170px" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="vatperTextBox_FilteredTextBoxExtender" runat="server"
                                FilterType="Custom, Numbers" TargetControlID="disPerTextBox" ValidChars="0123456789" />
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Is Active <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="isActiveCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="isActiveCheckBox_OnCheckedChanged" />
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Active Date <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="activeDtTextBox"  runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="activeDtTextBox_CalendarExtender" runat="server"
                                Format="dd-MMM-yyyy" PopupButtonID="ImageButton" TargetControlID="activeDtTextBox">
                            </ajaxToolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <div runat="server" id="newDiscount" visible="false">
                        
                        <tr>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                In-Active Date  <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="inActiveDateTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton3" TargetControlID="inActiveDateTextBox">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton3" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Discount Percent % (New) <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newDisPerTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                    FilterType="Custom, Numbers" TargetControlID="newDisPerTextBox" ValidChars="0123456789" />
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
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Active Date (New) <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newActiveDateTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton1" TargetControlID="newActiveDateTextBox">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            </td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                        </tr>
                        
                    </div>
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
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit" />
                            
                            <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E1B12C" runat="server" OnClick="clearButton_OnClick"
                                Text="Clear" />
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


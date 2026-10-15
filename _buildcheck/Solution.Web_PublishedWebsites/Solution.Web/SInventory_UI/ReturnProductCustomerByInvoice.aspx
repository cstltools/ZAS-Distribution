<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ReturnProductCustomerByInvoice, App_Web_htam3fti" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style type="text/css">
        
       
        
    </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Delivery Sales Return
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="DelivaryInvoiceCreationAuto.aspx">Back to List</asp:HyperLink>                          --%>
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
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Delivery Invoice No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="delInvNoTextBox0" runat="server" AutoPostBack="True" CssClass="TextBox"></asp:TextBox>
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
                            
                        </td>
                        <td class="TDRight" width="20%">
                             &nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Payment Type:
                        </td>
                        <td class="TDRight" width="20%">
                             <asp:RadioButtonList ID="paymentTypeRadioButtonList" RepeatDirection="Horizontal"  runat="server">
                                <asp:ListItem Text="Cash" Value="Cash" />
                                <asp:ListItem Text="Credit" Value="Credit" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                     <tr runat="server" Visible="False">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Adj. Invoice No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropdownList ID="ddlAdjustmentInv" runat="server" CssClass="TextBox"></asp:DropdownList>
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
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="Button2" runat="server" BackColor="#339933" Text="Search" OnClick="Button2_Click" />
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
                            <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="DelivaryInvoiceCreationAuto.aspx">Back to List</asp:HyperLink>                          --%>
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
                    <div id="uiel" runat="server" visible="True">
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
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Customer Code:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="custCodeTextBox" runat="server" AutoPostBack="True" CssClass="TextBox"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Customer Name:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="custNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="True"></asp:Label>
                                <%--<asp:AutoCompleteExtender ID="custNameTextBox_AutoCompleteExtender" runat="server"
                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                    CompletionSetCount="10" ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx"
                                    TargetControlID="custNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                </asp:AutoCompleteExtender>--%>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Customer Address:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="custAddressTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Salse Center:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="comUnitNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                MIO Code:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="miaCodeTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                MIO Name:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="miaNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Payment Amount:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="payamntTextBox" runat="server" AutoPostBack="True" CssClass="TextBox"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Territory Name:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="areaNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Merket Name:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="marketNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Customer Category:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="custCategoryTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Order No:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="orderNoTextBox" runat="server" CssClass="TextBox"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Order Date:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="orderDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:Label>
                                <%--<asp:ImageButton ID="imgorderDate" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Invoice Date:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="invDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Delivery Person Mob.No:
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Label ID="deliverypersonMobileTextBox" runat="server" CssClass="TextBox"></asp:Label>
                            </td>
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                Delivery Person Name:
                            </td>
                            <td width="20%" class="TDRight">
                                <%--<asp:CalendarExtender ID="orderDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="orderDateTextBox"
                                    PopupButtonID="imgorderDate">
                                </asp:CalendarExtender>--%>
                                <asp:Label ID="deliverypersonNameTextBox" runat="server" CssClass="TextBox"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Remarks:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="remarksTextBox" runat="server" CssClass="TextBox"></asp:Label>
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

                        <tr runat="server" id="asdfs">
                            <td width="13%" class="TDLeft" >
                               
                            </td>
                            <td width="20%" class="TDRight">
                                
                            </td>
                            <td id="Td3" width="13%" class="TDLeft" runat="server" visible="False">
                                Payment Type
                            </td>
                            <td id="Td4" width="20%" class="TDRight" runat="server" visible="False">
                                <asp:DropDownList ID="payTypeDDL" runat="server" CssClass="DropDown">
                                </asp:DropDownList>
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
                                <asp:HiddenField ID="invoiceNoHiddenField" runat="server" />
                                <asp:HiddenField ID="invoiceHiddenField" runat="server" />
                                <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%" colspan="6">
                                <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                    <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                        DataKeyNames="InvoiceId,InvoiceDetailId,UnitPrice,UnitVatAmount">
                                        <Columns>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                            <asp:BoundField DataField="AvialableQty" HeaderText="Avialable Qty " />
                                            <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" />
                                            <%-- <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />--%>
                                            <asp:TemplateField HeaderText="Return Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="retQtyTextBox" runat="server" AutoPostBack="True" OnTextChanged="retQtyTextBox_OnTextChanged" ></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Return Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="retamountTextBox" runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
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
                               <asp:Button ID="Button1" runat="server" Text="Save Return" OnClick="saveButton_Click" CssClass="button-padding-right"
                                            OnClientClick="return confirm('Are you sure you want to Return Invoice ?');" /> 
                                            
                               <asp:Button ID="clearButton" runat="server" BackColor="#cc6600" OnClick="clearButton_OnClick"
                                    Text="Clear" />
                            </td>
                            <td class="TDLeft" width="13%">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
                                    DisplayAfter="0" DynamicLayout="true">
                                    <ProgressTemplate>
                                        <center>
                                            <asp:Image ID="Img2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                        </center>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%">
                                <asp:HiddenField ID="hdCustomerMasterId" runat="server" />
                                <asp:HiddenField ID="hdComUnitId" runat="server" />
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:HiddenField ID="hdMiaId" runat="server" />
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
                    </div>
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

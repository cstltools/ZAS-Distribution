<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ManualOrderCreation, App_Web_t3jyjdi4" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Manual Order Creation
                        </td>
                         <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="msgLabel" runat="server"></asp:Label>
                             </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                      <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                     <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                             </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                     
                     <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            Manufacturer :</td>
                        <td class="TDLeft" width="13%">
                             <asp:DropDownList ID="manufacturerDropDownList" runat="server" 
                                 CssClass="radioButtonList" AutoPostBack="True"
                                 onselectedindexchanged="manufacturerDropDownList_SelectedIndexChanged">
                    </asp:DropDownList></td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                             </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                      <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            Order No :</td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="orderNoTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                          </td>
                        <td class="TDRight" width="20%">
                            Order Date :</td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="orderDateTextBox" runat="server" CssClass="TextBoxCalander" ></asp:TextBox>
                             <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                             <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" TargetControlID="orderDateTextBox"
                                PopupButtonID="imgDate">
                            </ajaxToolkit:CalendarExtender>
                          </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                     
                      <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            Distribution Center :</td>
                        <td class="TDLeft" width="13%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDRight" width="20%">
                            Customer Code</td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="custCodeTextBox" runat="server" AutoPostBack="True" 
                                ontextchanged="custCodeTextBox_TextChanged"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            MIO Code</td>
                        <td class="TDLeft" width="13%">
                            <asp:Label ID="mioCodeLabel" runat="server"></asp:Label>
                        </td>
                        <td class="TDRight" width="20%">
                            Customer Name</td>
                        <td class="TDLeft" width="13%">
                            <asp:Label ID="custNameLabel" runat="server"></asp:Label>
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            MIO Name</td>
                        <td class="TDLeft" width="13%">
                            <asp:Label ID="mioNameLabel" runat="server"></asp:Label>
                        </td>
                        <td class="TDRight" width="20%">
                            Market Name</td>
                        <td class="TDLeft" width="13%">
                            <asp:Label ID="marketNameLabel" runat="server"></asp:Label>
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%" colspan="4">
                            <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" >
                                           
                                <Columns>
                                    <asp:BoundField DataField="SL" HeaderText="SL" />
                                    <asp:TemplateField HeaderText="Product Code">
                                        <ItemTemplate>
                                            <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="TextBoxCalander" 
                                                AutoPostBack="True" ToolTip="true" ontextchanged="productCodeTextBox_TextChanged" Text= <%# Eval("ProductCode")%>></asp:TextBox>
                                            <asp:HiddenField ID="unitpriceHiddenField" Value= <%# Eval("UnitPrice")%> runat="server" />
                                            <asp:HiddenField ID="productidHiddenField" Value= <%# Eval("ProductId")%> runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="productNameTextBox" runat="server" 
                                               CssClass="TextBox" Text= <%# Eval("ProductName")%> AutoPostBack="True" ToolTip="true"
                                                ontextchanged="productNameTextBox_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                         DelimiterCharacters="" EnableCaching="true"
                                        Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                        ServiceMethod="GetProductByMenufracturer" ServicePath="SInventoryWebService.asmx"  TargetControlID="productNameTextBox" 
                                        UseContextKey="True"
                                        CompletionListCssClass="autocomplete_completionListElement" 
                                        CompletionListItemCssClass="autocomplete_listItem" 
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        ShowOnlyCurrentWordInCompletionListItem="true"
                                        >
                                    </ajaxToolkit:AutoCompleteExtender>
                                               
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PackSize">
                                        <ItemTemplate>
                                            <asp:TextBox ID="packSizeTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True" Text= <%# Eval("PackSize")%>></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Req. Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="reqQtyTextBox" runat="server" CssClass="TextBoxCalander" Text= <%# Eval("Quantity")%>></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT">
                                        <ItemTemplate>
                                            <asp:TextBox ID="vatTextBox" runat="server" CssClass="TextBoxCalander" Text= <%# Eval("VAT")%>></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TP">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tpTextBox" runat="server" CssClass="TextBoxCalander" Text= <%# Eval("UnitPrice")%>></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                                ImageUrl="~/images/lineAdd.png" onclick="ImageButton1_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" 
                                                ImageUrl="~/images/lineDelete.png" onclick="ImageButton2_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                           
                            </asp:GridView>
                        </td>
                        
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="submitButton" runat="server" onclick="submitButton_Click" 
                               OnClientClick="return confirm('Are you sure you want to Create Order ?');" Text="Submit" />
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                </table>
                 </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


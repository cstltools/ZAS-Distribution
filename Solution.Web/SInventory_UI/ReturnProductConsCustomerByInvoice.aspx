<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="ReturnProductConsCustomerByInvoice.aspx.cs" Inherits="SInventory_UI_ReturnProductCustomerByInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Consumer Cutomer Return</td>
                    </tr>
                     <tr>
                        <td class="TDLeft" width="13%">
                           <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="DelivaryInvoiceCreationAuto.aspx">Back to List</asp:HyperLink>                          --%>
                        </td>
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
                            Delivery Invoice No</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="delInvNoTextBox0" runat="server" AutoPostBack="True" 
                                CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:Button ID="Button2" runat="server" Text="Search" onclick="Button2_Click" />
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <div id="uiel" runat="server" Visible="True">
                    <tr>
                        <td class="TDLeft" width="13%">
                            Payment Amount</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="payamntTextBox" runat="server" AutoPostBack="True" 
                                CssClass="TextBox"></asp:TextBox>
                        </td>
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
                        <td width="13%" class="TDLeft">
                            Customer Code</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="custCodeTextBox" runat="server" AutoPostBack="True" 
                                CssClass="TextBox"  ></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            CustomerName</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="custNameTextBox" runat="server" CssClass="TextBox" 
                                AutoPostBack="True" ></asp:TextBox>
                            
                            <asp:AutoCompleteExtender ID="custNameTextBox_AutoCompleteExtender" runat="server"
                                         DelimiterCharacters="" EnableCaching="true"
                                        Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                        ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx"  TargetControlID="custNameTextBox" 
                                        UseContextKey="True"
                                        CompletionListCssClass="autocomplete_completionListElement" 
                                        CompletionListItemCssClass="autocomplete_listItem" 
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        ShowOnlyCurrentWordInCompletionListItem="true"
                                        >
                                    </asp:AutoCompleteExtender>


                        </td>
                        <td width="13%" class="TDLeft">
                            Customer Address</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="custAddressTextBox" runat="server" CssClass="TextBox" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            Invoice Date</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="invDateTextBox" runat="server" CssClass="DropDown"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    <tr runat="server" id="asdfs">
                        <td width="13%" class="TDLeft" >
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td id="Td3" width="13%" class="TDLeft" runat="server" Visible="False">
                            &nbsp;</td>
                        <td id="Td4" width="20%" class="TDRight" runat="server" Visible="False">
                            &nbsp;</td>
                        <td id="Td1" width="13%" class="TDLeft">
                            </td>
                        <td id="Td2" width="20%" class="TDRight" >
                            
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
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:HiddenField ID="invoiceNoHiddenField" runat="server" />
                            <asp:HiddenField ID="invoiceHiddenField" runat="server" />
                            <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                            <br>
                               <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="InvoiceId,InvoiceDetailId,UnitPrice,UnitVatAmount" >
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
                                            <asp:TextBox ID="retamountTextBox" runat="server" AutoPostBack="True" Enabled="False" ></asp:TextBox>
                                                 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   </Columns>
                            </asp:GridView>
                               <br>
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
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Button1" runat="server" Text="Save Return" OnClick="saveButton_Click" OnClientClick="return confirm('Are you sure you want to Return Invoice ?');" />
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
                            &nbsp;</td>
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
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
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



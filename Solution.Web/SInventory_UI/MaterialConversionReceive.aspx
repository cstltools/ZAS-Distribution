<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="MaterialConversionReceive.aspx.cs" Inherits="SInventory_UI_MaterialConversionReceive" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">WH StockIn List
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
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:LinkButton ID="viewLinkButton" CssClass="btn buttons smallBtn" ForeColor="green"
                                OnClick="viewLinkButton_OnClick" runat="server">View Details</asp:LinkButton>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Company Name: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Warehouse: <span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="warehouseDropDownList" Width="175px" Height="23px" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                            <asp:HiddenField ID="stockInIdHiddenField" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Stock In Date: <span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="stockInDateTextBox" Width="150px" runat="server" CssClass="TextBoxCalander"
                                AutoPostBack="True" OnTextChanged="stockInDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                            <cc1:CalendarExtender ID="manufacturerDate1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="stockInDateTextBox" PopupButtonID="imgDate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Challan No: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="challanNoTextBox" Height="23px" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">Challan Date: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="challanDateTextBox" Width="150px" runat="server" CssClass="TextBoxCalander"
                                AutoPostBack="True" OnTextChanged="challanDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton4"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="challanDateTextBox" PopupButtonID="ImageButton4">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;" class="TDLeft">Reference No:&nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="referenceNoTextBox" Height="23px" CssClass="TextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Reference Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="referenceDateTextBox" Width="150px" Height="23px" runat="server"
                                CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="referenceDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton5"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="referenceDateTextBox" PopupButtonID="ImageButton5">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">Remarks:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="remarksTextBox" Height="20px" TextMode="MultiLine" Rows="2" Width="170px"
                                runat="server"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlSupplier" Width="175px" Height="23px" runat="server" Visible="False"
                                CssClass="DropDown"> 
                            </asp:DropDownList>
                             <%-- Supplier: <span style="color: red">*</span>--%>
                        </td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Production Unit <span class="star-mark">* </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlProductionUnit" AutoPostBack="True" OnSelectedIndexChanged="ddlProductionUnit_OnSelectedIndexChanged" runat="server" CssClass="DropDown"></asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">&nbsp; </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
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
                        <td width="20%" class="TDRight" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="MaterialConversionMasterId" OnRowCommand="loadGridView_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" AutoPostBack="True" OnCheckedChanged="chkSelect_OnCheckedChanged"  runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="TextBoxCalander" Width="100px" ReadOnly="True" Text='<%# Eval("ProductCode") %>'></asp:TextBox>
                                                <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                                <asp:HiddenField ID="hdfMaterialConversionMasterId" Value='<%# Eval("MaterialConversionMasterId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productNameTextBox" runat="server" ReadOnly="True" Width="200px" CssClass="TextBoxCalander"
                                                    Text='<%# Eval("ProductName") %>'></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                    CompletionSetCount="10" ServiceMethod="GetProduct" ServicePath="SInventoryWebService.asmx"
                                                    TargetControlID="productNameTextBox" UseContextKey="True"
                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pack Size">
                                            <ItemTemplate>
                                                <asp:TextBox ID="packSizeTextBox" CssClass="TextBoxCalander" ReadOnly="True" runat="server" Width="60px" Text='<%# Eval("PackSize") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Batch">
                                            <ItemTemplate>
                                                <asp:TextBox ID="batchTextBox" CssClass="TextBoxCalander" runat="server" Text='<%# Eval("Batch_No") %>' Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mfg. Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="mfgDateTextBox" runat="server"  CssClass="TextBoxCalander" Text='<%# Eval("Mfg_Date") %>'
                                                    AutoPostBack="True" OnTextChanged="mfgDateTextBox_OnTextChanged" DataFormatString="{0:dd-MMM-yyyy}"></asp:TextBox>
                                                <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="4" ID="ImageButton15"></asp:ImageButton>
                                                <cc1:CalendarExtender ID="manufacturerDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="mfgDateTextBox"
                                                    PopupButtonID="ImageButton15">
                                                </cc1:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exp. Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="expDateDateTextBox" runat="server"
                                                    CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="expDateDateTextBox_OnTextChanged"  Text='<%# Eval("Exp_Date") %>'
                                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:TextBox>
                                                <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="4" ID="ImageButton25"></asp:ImageButton>
                                                <cc1:CalendarExtender ID="ExpDate" runat="server" PopupButtonID="ImageButton25" Format="dd-MMM-yyyy"
                                                    TargetControlID="expDateDateTextBox">
                                                </cc1:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="reqQtyTextBox" runat="server" CssClass="TextBoxCalander" Width="80px" ReadOnly="True" Text='<%# Eval("FinalOutputQuantityInt") %>'
                                                    AutoPostBack="True" OnTextChanged="reqQtyTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQuantity" runat="server"
                                                    Enabled="True" TargetControlID="reqQtyTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:TextBox ID="costPriceTextBox" ReadOnly="True" runat="server" Width="100px" Text='<%# Eval("CostPerUnit") %>' CssClass="TextBoxCalander"
                                                    AutoPostBack="True" OnTextChanged="costPriceTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPrice" runat="server" Enabled="True"
                                                    TargetControlID="costPriceTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False" HeaderText="Vat (Per Unit)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="vatTextBox" runat="server" CssClass="TextBoxCalander" Width="80px" ReadOnly="True" Text='<%# Eval("Vat") %>'
                                                    AutoPostBack="True" OnTextChanged="vatTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderVat" runat="server" Enabled="True"
                                                    TargetControlID="vatTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="totalValueTextBox" runat="server" ReadOnly="True"  Width="120px" CssClass="TextBoxCalander" Text='<%# Eval("TotalCostOfThisBatch") %>'
                                                   AutoPostBack="True" OnTextChanged="totalValueTextBox_OnTextChanged"></asp:TextBox>
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Total Qty :
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="totalQtyTextBox" Height="23px" runat="server" class="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" style="text-align: right; padding-right: 10px;" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Total Vat:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="totalVatTextBox" Height="23px" ReadOnly="True" class="TextBox" runat="server"></asp:TextBox>
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Grand Total:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="grandTotalTextBox" Height="23px" runat="server" class="TextBox"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
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
                            <asp:Button ID="submitButton" CssClass="button-padding-right" runat="server" OnClick="submitButton_Click" OnClientClick="return confirm('Are you sure you want to Save ?');" Text="Submit" />
                            <asp:Button ID="cancelButton" BackColor="#cc6600" runat="server" OnClick="cancelButton_Click" Text="Cancel" />
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
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

</asp:Content>

<%--<asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                        <asp:BoundField DataField="ProductionUnitName" HeaderText="Production Unit " />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name " />
                                        <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                        <asp:BoundField DataField="RecipeSetupCode" HeaderText="Recipe Code" />
                                        <asp:BoundField DataField="MaterialConversionCode" HeaderText="Conversion Code" />
                                        <asp:BoundField DataField="FinalOutputQuantity" HeaderText="Final Output Quantity" />
                                        <asp:BoundField DataField="TotalCostOfThisBatch" HeaderText="Total Cost Of This Batch" />
                                        <asp:BoundField DataField="CostPerUnit" HeaderText="Cost Per Unit" />--%>
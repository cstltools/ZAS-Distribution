<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ManualOrderApproval, App_Web_dfmjdr5z" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid;
        }
    </style>
    <style scoped>
        .example_a
        {
            color: #fff !important;
            text-transform: uppercase;
            text-decoration: none;
            background: #ed3330;
            padding: 20px;
            border-radius: 5px;
            display: inline-block;
            border: none;
            transition: all 0.4s ease 0s;
        }
        
        .example_a:hover
        {
            background: #434343;
            letter-spacing: 1px;
            -webkit-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
            -moz-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
            box-shadow: 5px 40px -10px rgba(0,0,0,0.57);
            transition: all 0.4s ease 0s;
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
                            Discount Approval
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            Take Action :
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:RadioButtonList ID="actionRadioButtonList" runat="server" CssClass="inline-rb">
                            </asp:RadioButtonList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit0_Click" Text="Submit Action" />
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
                        <td width="13%" class="TDLeft" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 80%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="DiscountId">
                                    <Columns>
                                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                        
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="DiscountPercentage" HeaderText="Discount Percentage (%)" />
                                        <%--<asp:BoundField DataField="ActiveDate" HeaderText="Active Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="InActiveDate" HeaderText="InActive Date" DataFormatString="{0:dd-MMM-yyyy}" />--%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="invoiceButton" runat="server" Text="Preview Due History &gt;"
                                                    ForeColor="#EC3D18" OnClick="invoiceButton_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
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
    <div>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="HiddenField10"
            PopupControlID="Panel10" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:HiddenField ID="HiddenField10" runat="server"></asp:HiddenField>
        <asp:Panel ID="Panel10" runat="server" Style="display: none; overflow: scroll; padding: 10px"
            Width="1000px" CssClass="modalPopup">
            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
                    <div style="text-align: center">
                        <table width="100%">
                            <span>
                                <h3>
                                    Due History
                                </h3>
                            </span>
                            <hr />
                            <tr>
                                <td width="20%" class="TDRight" colspan="6">
                                    <div id="Div1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                        <asp:GridView ID="dueGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                            DataKeyNames="CustomerMasterId">
                                            <Columns>
                                                <asp:TemplateField HeaderText="#SL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                                <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="DueAmount" HeaderText="Due Amount" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
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
                                </td>
                                <td width="20%" class="TDRight">
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
                                </td>
                                <td width="20%" class="TDRight">
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
                                </td>
                                <td width="20%" class="TDRight">
                                    <asp:Button ID="Button41" Text="Close" OnClick="Button41_OnClick" runat="server"
                                        CssClass="example_a" BackColor="#ED3B58" />
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
                                </td>
                                <td width="20%" class="TDRight">
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
        </asp:Panel>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle"
                    ImageUrl="~/images/progress-bar-opt.gif" Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

<%@ Page Title="Salesroll" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="CommonUI_HomePage" %>

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
            width: 400px;
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
    <table width="100%" class="TableWorkArea">
        <%-- <tr>
                <td colspan="6" class="TableHeading">
                    HOME
                </td>
                

            </tr>--%>
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
            </td>
            <td width="20%" class="TDRight">
            </td>
            <td width="13%" class="TDLeft">
            </td>
            <td width="20%" class="TDRight">
                &nbsp;
            </td>
            <td width="13%" class="TDLeft">
                &nbsp;
            </td>
            <td width="20%" class="TDRight">
            </td>
        </tr>
        <tr>
            <td width="13%" class="TDLeft">
            </td>
            <td width="20%" class="TDRight">
            </td>
            <td width="13%" class="TDLeft">
                &nbsp;
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
            </td>
            <td width="20%" class="TDRight">
                &nbsp;
            </td>
            <td width="13%" class="TDLeft" colspan="2" rowspan="3">
                <%--  <asp:Image ID="Image2" runat="server" Height="120px" 
                        ImageUrl="~/images/logo1.png" Width="320px" />--%>
            </td>
            <td width="13%" class="TDLeft">
                &nbsp;
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
                &nbsp;
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
                &nbsp;
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
            <td width="13%" class="TDLeft" colspan="2">
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
            </td>
            <td width="20%" class="TDRight">
            </td>
            <td width="13%" class="TDLeft">
            </td>
            <td width="20%" class="TDRight">
                &nbsp;
            </td>
            <td width="13%" class="TDLeft">
                &nbsp;
            </td>
            <td width="20%" class="TDRight">
            </td>
        </tr>
        <tr>
            <td width="13%" class="TDLeft">
            </td>
            <td width="20%" class="TDRight">
            </td>
            <td width="13%" class="TDLeft">
                &nbsp;
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
            <tr>
                <td width="13%" class="TDLeft">
                </td>
                <td width="20%" class="TDRight">
                </td>
                <td width="13%" class="TDLeft">
                </td>
                <td width="20%" class="TDRight">
                    &nbsp;
                </td>
                <td width="13%" class="TDLeft">
                    &nbsp;
                </td>
                <td width="20%" class="TDRight">
                </td>
            </tr>
            <tr>
                <td width="13%" class="TDLeft">
                </td>
                <td width="20%" class="TDRight">
                </td>
                <td width="13%" class="TDLeft">
                    &nbsp;
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
        </tr>
    </table>
    
   <div>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="HiddenField10"
        PopupControlID="Panel10" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField10" runat="server" />
    <asp:Panel ID="Panel10" runat="server" Style="display: none; overflow: scroll; padding: 10px"
        Width="1000px" CssClass="modalPopup">
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div style="text-align: center">
                    <table width="100%">
                        <span>
                            <h2>
                                Product Expire Soon
                            </h2>
                        </span>
                        <hr />
                        <tr>
                            <td width="20%" class="TDRight" colspan="6">
                                <div id="Div1" style="height: 450px; overflow: auto; width: 97%; margin: 0 auto;">
                                    <asp:GridView ID="dueGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                        DataKeyNames="ProductCode">
                                        <Columns>
                                            <asp:TemplateField HeaderText="#SL">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="PackSize" HeaderText="PackSize" />
                                            <asp:BoundField DataField="MfgDate" HeaderText="Mfg. Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="ExpDate" HeaderText="Exp. Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                                            <asp:BoundField DataField="StockQty" HeaderText="StockQty" />
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

</asp:Content>




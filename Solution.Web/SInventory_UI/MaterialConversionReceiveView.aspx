<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="MaterialConversionReceiveView.aspx.cs" Inherits="SInventory_UI_WarehouseStockInView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Warehouse Stock In View
                        </td>
                    </tr>
                   
                    <tr>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                        </td>
                        <td width="20%"  class="TDRight">
                            <br />
                            <asp:LinkButton ID="viewLinkButton" CssClass="btn buttons smallBtn" ForeColor="green" OnClick="addNewLinkButton_OnClick" runat="server">Add New</asp:LinkButton>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <br />
                          <%--  <asp:LinkButton ID="LinkButton1" CssClass="btn buttons smallBtn" ForeColor="green" OnClick="reloadLinkButton_OnClick" runat="server">Reload</asp:LinkButton>--%>
                        </td>
                    </tr>
                    <tr>

                        <td class="TDRight" colspan="6">
                            <div id="gridContainer1" style="height: 500px; overflow: auto; width: 97%; margin: 0 auto">
                                &nbsp;<asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview" DataKeyNames="WHStockInMasterID" OnRowCommand="loadGridView_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="WHStockInCode" HeaderText="Code" />
                                        <asp:BoundField DataField="WHStockInDate" HeaderText="StockIn Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="TotalQuantity" HeaderText="Total Qty" />
                                        <asp:BoundField DataField="TotalValue" HeaderText="Total Amt" />
                                         <asp:BoundField DataField="Status" HeaderText="Status" />
                                           <asp:TemplateField HeaderText="Report">
                                        <ItemTemplate>
                                         
                                            <asp:ImageButton ID="printButton" runat="server" 
                                                  OnClick="printButton_Click"  ImageUrl="../images/image/if_paste-clipboard-copy_2931174.png"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="editImageButton" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                    CommandName="EditData" ImageUrl="~/images/edit.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="deleteImageButton" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                    CommandName="DeleteData" OnClientClick="return confirm('Are you sure you want to Delete ?');" ImageUrl="~/images/lineDelete.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>

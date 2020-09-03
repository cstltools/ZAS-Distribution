<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="LogisticeRollBackOperation.aspx.cs" Inherits="SInventory_UI_LogisticeRollBackOperation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Logistics Rollback Operation</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Steps </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="DropDown" 
                                AutoPostBack="True" onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                                <asp:ListItem> --- Select any one --- </asp:ListItem>
                                <asp:ListItem Value="SG">STO Generate</asp:ListItem>
                                <asp:ListItem Value="PG">Picking Generate</asp:ListItem>
                                <asp:ListItem Value="CG">Challan Generate</asp:ListItem>
                            </asp:DropDownList>
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                     <tr id="sto" runat="server" Visible="False">
                        <td width="13%" class="TDLeft">
                        </td>
                        <%--<td width="20%" class="TDRight">
                        </td>--%>
                        <td width="43%" class="TDLeft" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="ReqId" >
                                <Columns>
                                    <asp:BoundField DataField="ReqNo" HeaderText="Req. No" />
                                    <asp:BoundField DataField="ReqDate" HeaderText="Req. Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                   <%-- <asp:BoundField DataField="WearhouseName" HeaderText="WearhouseName" />--%>
                                    <asp:BoundField DataField="ComUnitCode" HeaderText="D.C. Code" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="D.C. Name" />
                                    <asp:BoundField DataField="Qty" HeaderText="Total Qty." />
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData" 
                                                OnClientClick="return confirm('Are you sure you want to Delete ?');"    ImageUrl="~/images/lineDelete.png"  OnClick="ImageButton2_Click"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView> 
                            </div>
                            
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
                    <tr id="picking" runat="server" Visible="False">
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%" colspan="4">
                            <asp:GridView ID="viewReqGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="ReqId,ComUnitId">
                                <Columns>
                                    <asp:BoundField DataField="ReqNo" HeaderText="Requision No" />
                                    <asp:BoundField DataField="ReqDate" DataFormatString="{0:dd-MMM-yyyy}" 
                                        HeaderText="Req Date" />
                                        
                                        
                                         <asp:BoundField DataField="PickingNo" HeaderText="Picking No" />
                                    <asp:BoundField DataField="PickingDate" DataFormatString="{0:dd-MMM-yyyy}" 
                                        HeaderText="Picking Date" />
                                        
                                        
                                        

                                    <asp:BoundField DataField="ComUnitCode" HeaderText="D.C. Code" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="D.C. Name" />
                                    
                                    
                                    <%--<asp:BoundField DataField="ManufacName" HeaderText="Manufacturer" />--%>
                                    

                                   <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData" 
                                                OnClientClick="return confirm('Are you sure you want to Delete ?');"    ImageUrl="~/images/lineDelete.png"  OnClick="editImageButton_OnClick"/>
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
                    <tr id="challan" runat="server" Visible="False">
                       
                            <td class="TDLeft" width="13%">
                                &nbsp;</td>
                            <td class="TDRight" width="20%" colspan="4">
                                <asp:GridView ID="stockInTraGridView" runat="server" 
                                    AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="ReqId">
                                    <Columns>
                                        <asp:BoundField DataField="IssueChalanNo" HeaderText="ChalanNo" />
                                        <asp:BoundField DataField="IssuChalanDate" DataFormatString="{0:dd-MMM-yyyy}" 
                                            HeaderText="ChalanDate" />
                                        <asp:BoundField DataField="TruckNo" HeaderText="TruckNo" />
                                        <asp:BoundField DataField="DriverName" HeaderText="DriverName" />
                                        <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData" 
                                                OnClientClick="return confirm('Are you sure you want to Delete ?');"    ImageUrl="~/images/lineDelete.png"  OnClick="edit2ImageButton_OnClick"/>
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
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
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


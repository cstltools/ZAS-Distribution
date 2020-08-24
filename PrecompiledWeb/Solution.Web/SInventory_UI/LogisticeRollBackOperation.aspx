<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_LogisticeRollBackOperation, App_Web_pcm1iuwz" %>

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
                        <td width="13%" class="TDLeft">
                            Steps</td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="DropDown" 
                                AutoPostBack="True" onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
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
                        <td width="43%" class="TDLeft" colspan="4">
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
                                    
                                    
                                    <asp:BoundField DataField="ManufacName" HeaderText="Manufacturer" />
                                    

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
</asp:Content>


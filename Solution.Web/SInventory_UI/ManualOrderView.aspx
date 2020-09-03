<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="ManualOrderView.aspx.cs" Inherits="SInventory_UI_ManualOrderView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
               <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Manual Order View</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp; Add New :</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="EmpCetegoryAddImageButton" runat="server" 
                                ImageUrl="~/images/Add.png" onclick="custCetegoryAddImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            Reload</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="EmpCetegoryReloadImageButton" runat="server" 
                                ImageUrl="~/images/refresh.png" 
                                onclick="custCetegoryReloadImageButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        
                        <td width="13%" class="TDLeft" colspan="6">
                             <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="OrderId" 
                                onrowcommand="loadGridView_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="OrderCode" HeaderText="OrderCode" />
                                    <asp:BoundField DataField="SubmissionDate" HeaderText="SubmissionDate" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" />
                                    <asp:BoundField DataField="MIOName" HeaderText="MIOName" />
                                    <asp:BoundField DataField="MarketName" HeaderText="MarketName" />
                                    <asp:BoundField DataField="TerritoryName" HeaderText="TerritoryName" />
                                    <asp:BoundField DataField="RegionName" HeaderText="RegionName" />
                                    <asp:BoundField DataField="AreaName" HeaderText="AreaName" />
                                    <asp:BoundField DataField="QTY" HeaderText="QTY" />
                                    
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData" 
                                                ImageUrl="~/images/edit.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                        <td width="13%" class="TDLeft" >
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
</asp:Content>


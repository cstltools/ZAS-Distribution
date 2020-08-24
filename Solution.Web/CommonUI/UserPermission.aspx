<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="UserPermission.aspx.cs" Inherits="CommonUI_UserPermission" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            User Wise Menu</td>
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
                            <asp:Label ID="msgLabel" runat="server"></asp:Label>
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
                            User Name :</td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="userDropDownList" runat="server" CssClass="DropDown" 
                                AutoPostBack="True" 
                                onselectedindexchanged="userDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            Main Menu :</td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="mainMenuDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="mainMenuDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                       <tr>
                        <td width="13%" class="TDLeft">
                            Main Menu Panal :</td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            Menu Option :</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                  
                    <tr >
                        <td width="13%" class="TDLeft" colspan="2">
                            <div id ="container1" style ="height:300px;overflow:auto;width:auto ">
                            <asp:GridView ID="mainMenuGridView" runat="server" CssClass="gridview" 
                                AutoGenerateColumns="False" DataKeyNames="SL,Status" 
                                onrowdatabound="mainMenuGridView_RowDataBound" onrowcommand="mainMenuGridView_RowCommand" 
                                
                               >
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="mainMenuCheckBox" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Main Menu" DataField="ManuName" />
                                    <asp:TemplateField HeaderText="Save">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="mainMenuSaveImageButton" runat="server" 
                                                ImageUrl="~/images/Save.png" CommandName="SaveData" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="deleteMainMenuBytton" runat="server" ImageAlign="Middle" 
                                                             ImageUrl="~/images/DeleteIcon.jpg"  CommandName="Remove" CommandArgument='<%# Container.DataItemIndex %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                        </td>
                        
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight" colspan="2" rowspan="3">
                             <div id ="container2" style ="height:600px;overflow:auto;width:auto ">
                            <asp:GridView ID="menuGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview"  DataKeyNames="SL,Status" 
                                onrowdatabound="menuGridView_RowDataBound" onrowcommand="menuGridView_RowCommand"
                                 >
                                 <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="menuCheckBox" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Main Menu" DataField="ManuName" />
                                     <asp:TemplateField HeaderText="Save">
                                         <ItemTemplate>
                                             <asp:ImageButton ID="saveMenuImageButton" runat="server" 
                                                 ImageUrl="~/images/Save.png" CommandName="SaveData" CommandArgument='<%# Container.DataItemIndex %>' />
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="deleteMainMenuBytton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="Remove" 
                                                ImageAlign="Middle" ImageUrl="~/images/DeleteIcon.jpg" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                       
                    </tr>
                    
                    
                    

                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight" colspan="2">
                            &nbsp;</td>
                       
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" colspan="2">
                             <div id ="continer3" style ="height:300px;overflow:auto;width:auto ">
                            </div>
                        </td>
                       
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight" colspan="2">
                            <div id ="Div1" style ="height:300px;overflow:auto;width:auto ">
                            </div>
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="HRM_UI_TDLUserView, App_Web_ufowa0j4" %>
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
                             User List</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            Add New</td>
                        <td width="20%" class="TDRight">
                          <asp:ImageButton ID="addImageButton" runat="server" ImageUrl="~/images/Add.png" 
                                onclick="addImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            Reload</td>
                        <td width="20%" class="TDRight">
                           <asp:ImageButton ID="reloadLinkButton" runat="server" 
                                ImageUrl="~/images/refresh.png" onclick="reloadLinkButton_Click" />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="6" style="padding: 10px 15px; background-color: #f7f9fa; border-top: 1px solid #e2e8f0; border-bottom: 1px solid #e2e8f0; text-align: left; vertical-align: middle;">
                            <span style="font-weight: bold; font-family: Calibri, Arial, sans-serif; font-size: 13px; color: #333; margin-right: 10px; vertical-align: middle;">Search User:</span>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBox" Width="250px" style="float: none !important; display: inline-block; vertical-align: middle; height: 20px; padding: 3px 6px; border: 1px solid #cbd5e1; border-radius: 3px;"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="button-padding-right" style="float: none !important; display: inline-block; margin-left: 8px; vertical-align: middle; height: 26px; padding: 0 15px; background-color: #0284c7; color: white; border: none; border-radius: 3px; cursor: pointer; font-weight: bold; font-size: 12px;" />
                            <asp:Button ID="btnClearSearch" runat="server" Text="Clear" OnClick="btnClearSearch_Click" BackColor="#F47322" style="float: none !important; display: inline-block; margin-left: 4px; vertical-align: middle; height: 26px; padding: 0 15px; background-color: #F47322; color: white; border: none; border-radius: 3px; cursor: pointer; font-weight: bold; font-size: 12px;" />
                        </td>
                    </tr>
                    <tr>
                        

                    <td width="20%" class="TDRight" colspan="6">
                            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="UserId" 
                                onrowcommand="loadGridView_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                    <asp:BoundField DataField="UserType" HeaderText="User Type" />
                                <%--    <asp:BoundField DataField="EmpMasterCode" HeaderText="Emp Master Code" />--%>
                                    <asp:BoundField DataField="LoginName" HeaderText="Login Name" />
                                     <asp:BoundField DataField="Password" HeaderText="Password" />
                                    <asp:BoundField DataField="UserStatus" HeaderText="User Status" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="ContactNo" HeaderText="Contact No" />
                                     <asp:TemplateField HeaderText="Edit">
                                         <ItemTemplate>
                                             <asp:ImageButton ID="editImageButton" runat="server" 
                                                 CommandArgument='<%# Eval("UserId") %>' CommandName="EditData" 
                                                 ImageUrl="~/images/edit.png" />
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Delete">
                                         <ItemTemplate>
                                             <asp:ImageButton ID="deleteImageButton" runat="server" 
                                                 CommandArgument='<%# Eval("UserId") %>' CommandName="DeleteData" 
                                                 ImageUrl="~/images/lineDelete.png" OnClientClick="return confirm('Are you sure you want to delete this user?');" />
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






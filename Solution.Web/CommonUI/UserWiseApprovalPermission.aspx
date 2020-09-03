<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="UserWiseApprovalPermission.aspx.cs" Inherits="CommonUI_UserWiseApprovalPermission" %>
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
                            User Wise Approval Permission</td>
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
                            Menu Option :</td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="appMenuDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="appMenuDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                       <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label>
                           </td>
                        <td width="13%" class="TDLeft">
                            <asp:Button ID="submitButton" runat="server" onclick="submitButton_Click" 
                                Text="Submit" />
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                  
                    <tr >
                        <td width="13%" class="TDLeft" colspan="2">
                            <div id ="container1" style ="height:300px;overflow:auto;width:auto ">
                            </div>
                        </td>
                        
                        <td width="13%" class="TDLeft">
                            &nbsp;Approval Action :</td>
                        <td width="20%" class="TDRight" colspan="2" rowspan="3">
                             <div id ="container2" style ="height:600px;overflow:auto;width:auto ">
                                 <asp:RadioButtonList ID="actionRadioButtonList" runat="server">
                                 </asp:RadioButtonList>
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


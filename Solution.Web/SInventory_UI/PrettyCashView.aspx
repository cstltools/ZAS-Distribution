<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="PrettyCashView.aspx.cs" Inherits="SInventory_UI_PrettyCashView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">Pretty Cash View
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: center">Add New</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productInfoNewImageButton" runat="server"
                                ImageUrl="~/images/Add.png" OnClick="miaTargetNewImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: center"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>

                        <td width="20%" class="TDRight" colspan="6">
                            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                                CssClass="gridview" DataKeyNames="PettyCashmasterId,ActiveStatus"
                                OnRowCommand="loadGridView_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="PettyCashmasterId" HeaderText="PettyCashmasterId" />
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                    <asp:BoundField DataField="PostingDate" HeaderText="Posting Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="CashAcc" HeaderText="Cash Account" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                    <asp:BoundField DataField="PerticularName" HeaderText="Perticuler Name" />
                                    <asp:BoundField DataField="CashReason" HeaderText="CashReason" />
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                   

                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server"
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData"
                                                ImageUrl="~/images/edit.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server"
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="DeleteData" ImageUrl="~/images/lineDelete.png"
                                                OnClientClick="return GetConfirmation();" OnClick="itemdeleteImageButton_Click" />
                                            <script type="text/javascript">
                                                function GetConfirmation() {
                                                    var reply = confirm("Ary you sure you want to delete this?");
                                                    if (reply) {
                                                        return true;
                                                    }
                                                    else {
                                                        return false;
                                                    }
                                                }
                                            </script>
                                            <%--                                            <asp:LinkButton ID="itemdeleteImageButton" runat="server" class="btn btn-danger btn-sm  btnTextShadow" CommandName="DeleteData" OnClick="itemdeleteImageButton_Click"><i class="fa fa-trash" aria-hidden="true"></i>--%>
                                            <%-- </asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Print">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton3" runat="server"
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="ViewData" ImageUrl="~/images/report-disk-icon.png"
                                                OnClick="itemViewReportImageButton_Click" />
                                            <%--                                            <asp:LinkButton ID="itemdeleteImageButton" runat="server" class="btn btn-danger btn-sm  btnTextShadow" CommandName="DeleteData" OnClick="itemdeleteImageButton_Click"><i class="fa fa-trash" aria-hidden="true"></i>--%>
                                            <%-- </asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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



</asp:Content>


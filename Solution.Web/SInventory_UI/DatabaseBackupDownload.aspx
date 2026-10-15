<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="DatabaseBackupDownload.aspx.cs" Inherits="SInventory_UI_DatabaseBackupDownload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- No UpdatePanel: file download needs a full postback. --%>
    <table width="100%" class="TableWorkArea">
        <tr>
            <td class="TableHeading">Database Backup Download</td>
        </tr>
        <tr>
            <td class="TDLeft">
                <asp:Label ID="messageLabel" runat="server" ForeColor="Red"></asp:Label>
                <asp:GridView ID="backupGridView" runat="server" AutoGenerateColumns="False"
                    CssClass="gridview" OnRowCommand="backupGridView_RowCommand"
                    EmptyDataText="No backup found!">
                    <Columns>
                        <asp:BoundField DataField="Database" HeaderText="Database" />
                        <asp:TemplateField HeaderText="Download">
                            <ItemTemplate>
                                <asp:LinkButton ID="downloadLinkButton" runat="server" CommandName="DownloadFile"
                                    CommandArgument="<%# Container.DataItemIndex %>">Download</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

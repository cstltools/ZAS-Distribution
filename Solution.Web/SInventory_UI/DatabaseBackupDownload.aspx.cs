using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public partial class SInventory_UI_DatabaseBackupDownload : System.Web.UI.Page
{
    private const string BackupFolder = @"F:\DailyBackup";

    // Fixed whitelist: the client only ever sends an index, never a file name.
    private static readonly string[][] Backups =
    {
        new[] { "DBbackup 1", "SalesRollDB_ZAS_backup_*.bak" },
        new[] { "DBbackup 2", "CSTL_ERP_DB_backup_*.bak" }
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        // Backups hold all company data: admin only.
        if (Session["UserId"] == null || Session["UserId"].ToString() != "1")
        {
            Response.Redirect("~/Login.aspx");
            return;
        }
        if (!IsPostBack) LoadGrid();
    }

    private static FileInfo Latest(string pattern)
    {
        var dir = new DirectoryInfo(BackupFolder);
        return dir.Exists
            ? dir.GetFiles(pattern).OrderByDescending(f => f.LastWriteTime).FirstOrDefault()
            : null;
    }

    private void LoadGrid()
    {
        backupGridView.DataSource = Backups.Select(b =>
        {
            FileInfo f = Latest(b[1]);
            return new
            {
                Database = b[0],
                FileName = f == null ? null : f.Name,
                SizeMB = f == null ? 0 : f.Length / 1048576.0,
                Modified = f == null ? (DateTime?)null : f.LastWriteTime
            };
        }).ToList();
        backupGridView.DataBind();
    }

    protected void backupGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "DownloadFile") return;

        int i;
        if (!int.TryParse(e.CommandArgument.ToString(), out i) || i < 0 || i >= Backups.Length) return;

        FileInfo f = Latest(Backups[i][1]);
        if (f == null)
        {
            messageLabel.Text = "Backup file not found in " + BackupFolder;
            return;
        }

        Server.ScriptTimeout = 3600; // large .bak files
        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + f.Name + "\"");
        Response.AddHeader("Content-Length", f.Length.ToString());
        Response.TransmitFile(f.FullName);
        Response.Flush();
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
}

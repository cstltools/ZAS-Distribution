using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Library.DAL.InternalCls
{
   public class ClsApprovalAction
    {
       ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
       public void LoadActionControlByUser(RadioButtonList rdl, string pageName, string userName)
       {
           string query = @"SELECT * FROM dbo.tblActionUserWiseApproval UA INNER JOIN dbo.tblActionPageWiseStep PS ON UA.ManuSL = PS.ManuSL "+
                        " INNER JOIN dbo.tblActionSteps SA ON PS.ASId = SA.ASId "+
                        " INNER JOIN tblActionStepDetail AD ON SA.ASId=AD.ASId AND UA.ActionId=AD.ActionId "+
                        " INNER JOIN dbo.tblMainMenu MM ON UA.ManuSL=MM.SL "+
                        " WHERE UA.LoginName='" + userName + "' AND MM.URL LIKE '%" + pageName + "%'";

           DataTable dtActionCondition = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");



           string query1 = @"select * from tblAction where IsShow=1 ";
           DataTable dtAction = aCommonInternalDal.DataContainerDataTable(query1, "SSIDB");
           rdl.DataSource = dtAction;
           rdl.DataTextField = "ActionText";
           rdl.DataValueField = "ActionId";
           rdl.DataBind();

           
               if (dtActionCondition.Rows.Count>0)
               {
                   for (int i = 0; i < rdl.Items.Count; i++)
                   {
                       if (Convert.ToInt32(rdl.Items[i].Value.ToString()) == Convert.ToInt32(dtActionCondition.Rows[0]["ActionId"].ToString()))
                       {
                           rdl.Items[i].Enabled = true;
                           
                           
                       }
                       else
                       {
                           if (rdl.Items[i].Text.ToString() != "Cancel")
                           {
                               rdl.Items[i].Enabled = false;
                           }
                       }
                       if (rdl.Items[i].Text.ToString() == dtActionCondition.Rows[0]["ActionCondition"].ToString())
                       {
                           rdl.Items[i].Enabled = false;
                       }
                   }
               }
               else
               {
                   for (int i = 0; i < rdl.Items.Count; i++)
                   {

                       rdl.Items[i].Enabled = false;
                   }
               }
       }
       public string LoadForApprovalByUserCondition(string pageName, string userName)
       {
           string returnString = "";

           string query = @"SELECT * FROM dbo.tblActionUserWiseApproval UA INNER JOIN dbo.tblActionPageWiseStep PS ON UA.ManuSL = PS.ManuSL " +
                        " INNER JOIN dbo.tblActionSteps SA ON PS.ASId = SA.ASId " +
                        " INNER JOIN tblActionStepDetail AD ON SA.ASId=AD.ASId AND UA.ActionId=AD.ActionId " +
                        " INNER JOIN dbo.tblMainMenu MM ON UA.ManuSL=MM.SL " +
                        " WHERE UA.LoginName='" + userName + "' AND MM.URL LIKE '%" + pageName + "%'";

           DataTable dtActionCondition = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

           if (dtActionCondition.Rows.Count > 0)
           {
               returnString = dtActionCondition.Rows[0]["ActionCondition"].ToString();
           }
           else
           {
               returnString = "";
           }
           return returnString;
       }
    }
}

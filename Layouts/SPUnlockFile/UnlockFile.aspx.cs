using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPUnlockFile.Layouts.SPUnlockFile
{
    public partial class UnlockFile : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strListItemId = Request.QueryString["SPListItemId"];
            string strListId = Request.QueryString["SPListId"];
            var web = SPContext.Current.Web;
            var list = web.Lists.GetList(Guid.Parse(strListId), false);
            var item = list.Items.GetItemById(int.Parse(strListItemId));
            var file = item.File;

            if(!IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    if (file.LockId != null)
                    {
                        try
                        {
                            file.ReleaseLock(file.LockId);
                            lblText.Text = file.ServerRelativeUrl + " unlocked";
                        }
                        catch (Exception ex)
                        {
                            lblText.Text = ex.Message;
                        }
                    }
                    else
                    {
                        lblText.Text = file.ServerRelativeUrl + " is not locked";
                    }
                });
            }
        }
    }
}

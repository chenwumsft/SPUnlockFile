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

            if (!IsPostBack)
            {
                if (file.LockId != null)
                {
                    lbUnlock.Enabled = true;
                    lblText.Text = string.Format("{0} is locked by {1}", file.ServerRelativeUrl, file.LockedByUser);
                }
                else
                {
                    lbUnlock.Enabled = false;
                    lblText.Text = file.ServerRelativeUrl + " is not locked";
                }
            }
        }

        protected void lbUnlock_Click(object sender, EventArgs e)
        {
            string strListItemId = Request.QueryString["SPListItemId"];
            string strListId = Request.QueryString["SPListId"];
            var web = SPContext.Current.Web;
            var list = web.Lists.GetList(Guid.Parse(strListId), false);
            var item = list.Items.GetItemById(int.Parse(strListItemId));
            var file = item.File;

            try
            {
                if (file.LockId != null)
                {
                    file.ReleaseLock(file.LockId);
                    lblText.Text = file.ServerRelativeUrl + " has been unlocked";
                    lbUnlock.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblText.Text = ex.Message;
            }
        }
    }
}

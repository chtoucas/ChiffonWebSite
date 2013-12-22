namespace Chiffon.Errors
{
    using System.Web.UI;

    public partial class ErrorMasterPage : MasterPage
    {
        public void ShowHomeLink()
        {
            HomeLinkPanel.Visible = true;
        }
    }
}
namespace Dajunctic
{
    public class BasePopup : BaseView
    {
        public virtual void BeforeShow(object data = null)
        {
        }

        public virtual void AfterShow()
        {
        }

        public virtual void BeforeDismiss()
        {
        }

        public virtual void AfterDismiss()
        {
        }

        public virtual void BeforePause()
        {
        }

        public virtual void AfterPause()
        {
        }

        public virtual void BeforeResume()
        {
        }

        public virtual void AfterResume()
        {
        }

        public void Dismiss()
        {
            if (PopupController.Instance != null)
            {
                PopupController.Instance.DismissPopup();
            }
        }
    }
}
using System;
using System.Windows.Forms;
using WinFormsMvp.Binder;

namespace WinFormsMvp.Forms
{
    public class MvpPanel : Panel, IView
    {
        private readonly PresenterBinder presenterBinder = new PresenterBinder();

        public MvpPanel()
        {
            ThrowExceptionIfNoPresenterBound = true;

            presenterBinder.PerformBinding(this);
        }

        #region Implementation of IView

        public bool ThrowExceptionIfNoPresenterBound { get; private set; }

        public event EventHandler Load;

        protected virtual void OnLoadFinished(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler handler = Load;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}

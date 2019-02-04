using WinFormsMvp.Unity;
using WinFormsMvp.Binder;
using Microsoft.Practices.Unity;

using MVP.Source.Services;

namespace MVP
{
    public partial class ThisAddIn
    {
        private static IUnityContainer unityContainer;
 

        public IUnityContainer UnityContainer
        {
            get
            {
                return unityContainer;
            }
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            // Framework start up. More information in:
            // http://www.codeproject.com/Articles/522809/WinForms-MVP-An-MVP-Framework-for-Winforms
            unityContainer = new UnityContainer();
            // Presenters will be automatically injected by the DI container.
            PresenterBinder.Factory = new UnityPresenterFactory(unityContainer);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}

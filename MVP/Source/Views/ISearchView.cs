using System;
using WinFormsMvp;

namespace MVP.Source.Views
{
    public interface ISearchView : IView
    {
        event EventHandler ViewLoading;
        event EventHandler SearchInDocument;
        event EventHandler SaveSearchData;
        event EventHandler OnInit;

        void ConfirmLoaded();
        void BindAttribute(string type, string attribute, object model);

        string SearchText { get; set; }
        string StatusText { get; set; }
    }
}

using System;
using WinFormsMvp;

using MVP.Source.Models;

namespace MVP.Source.Views
{
    public interface ISearchView : IView
    {
        event EventHandler ViewLoding;
        event EventHandler SearchInDocument;
        event EventHandler SaveSearchData;
        event EventHandler OnInit;

        void ConfirmLoaded();
        void BindAttribute(string type, string attribute, SearchData model);

        string SearchText { get; set; }
        string StatusText { get; set; }
    }
}

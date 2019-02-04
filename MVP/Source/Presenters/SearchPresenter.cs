using System;

using WinFormsMvp;
using MVP.Source.Models;
using MVP.Source.Services;
using MVP.Source.Repositories;
using MVP.Source.Views;

namespace MVP.Presenters
{
    public class SearchPresenter : Presenter<ISearchView>
    {
        private IWordService wordService;
        private ISearchDataRepository searchDataRepository;

        private SearchData searchData;

        public SearchPresenter(ISearchView view) : base(view)
        {
            View.ViewLoding += View_ViewLoding;
            View.SearchInDocument += View_SearchInDocument;
            View.SaveSearchData += View_SaveSearchData;
            View.OnInit += View_OnInit;

            wordService = WordService.Instance;
            searchDataRepository = SearchDataXmlRepository.Instance;

            this.searchData = searchDataRepository.GetSearchData();
        }

        private void View_SearchInDocument(object sender, EventArgs e)
        {
            View.StatusText = Properties.Locale.SearchView_Searching;
            if (wordService.FindNext(View.SearchText))
            {
                View.StatusText = "Texto encontrado!";
            }
            else
            {
                View.StatusText = "Nenhuma ocorrência encontrada!";
            }
        }

        private void View_SaveSearchData(object sender, EventArgs e)
        {
            searchDataRepository.SetSearchData(this.searchData);
        }

        private void View_ViewLoding(object sender, EventArgs e)
        {
            View.StatusText = Properties.Locale.SearchView_ReadyToBegin;
        }
        private void View_OnInit(object sender, EventArgs e)
        {
            View.BindAttribute("Text", "Text", this.searchData);
        }
        
    }
}

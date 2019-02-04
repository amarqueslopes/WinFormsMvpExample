using MVP.Source.Views;
using WinFormsMvp.Forms;
using System;
using System.Windows.Forms;

namespace MVP.Source.Forms
{
    public partial class SearchView : MvpForm, ISearchView
    {
        public SearchView()
        {
            InitializeComponent();
            searchButton.Text = Properties.Locale.SearchView_Search;
            searchForLabel.Text = Properties.Locale.SearchView_SearchFor;
        }

        protected override void OnLoad(EventArgs e)
        {
            OnViewLoding();

            base.OnLoad(e);
        }

        protected virtual void OnViewLoding()
        {
            ViewLoading?.Invoke(this, EventArgs.Empty);
        }


        public event EventHandler ViewLoading;
        public event EventHandler SearchInDocument;
        public event EventHandler SaveSearchData;
        public event EventHandler OnInit;

        public string SearchText
        {
            get
            {
                return this.searchTextBox.Text;
            }
            set
            {
                this.searchTextBox.Text = value;
            }
        }

        public void BindAttribute(string type, string attribute, object model)
        {
            if (model != null)
            {
                this.searchTextBox.DataBindings.Add(type, model, attribute, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public string StatusText
        {
            get
            {
                return this.statusMessageLabel.Text;
            }
            set
            {
                this.statusMessageLabel.Text = value;
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SearchInDocument(sender, e);
        }

        public void ConfirmLoaded()
        {
            StatusText = Properties.Locale.SearchView_Loaded;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchInDocument(sender, e);
        }

        private void SearchView_Load(object sender, EventArgs e)
        {
            OnInit(null, EventArgs.Empty);
        }

        private void SearchView_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSearchData(sender, e);
        }
    }
}

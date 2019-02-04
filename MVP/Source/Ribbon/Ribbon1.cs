using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

using MVP.Source.Forms;

namespace MVP.Source.Ribbon
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            this.searchButton.Label = Properties.Locale.Ribbon_SearchButton;
            this.searchButton.Image = Properties.Resources.search;
        }

        private void searchButton_Click(object sender, RibbonControlEventArgs e)
        {
            SearchView searchView = new SearchView();
            searchView.ShowDialog();
        }
    }
}

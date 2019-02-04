using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVP.Source.Models;

namespace MVP.Source.Repositories
{
    interface ISearchDataRepository
    {
        SearchData GetSearchData();
        void SetSearchData(SearchData searchData);
    }
}

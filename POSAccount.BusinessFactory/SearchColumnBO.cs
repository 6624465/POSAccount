using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;

namespace POSAccount.BusinessFactory
{

    public class SearchColumnBO
    {
        private SearchColumnDAL searchcolumn;
        public SearchColumnBO()
        {

            searchcolumn = new SearchColumnDAL();
        }

        public List<SearchColumn> GetList()
        {
            return searchcolumn.GetList();
        }

        public List<SearchColumn> GetCategoryList(string searchCategory = "")
        {
            return searchcolumn.GetListByCategory(searchCategory);
        }


        public bool SaveSearchColumn(SearchColumn newItem)
        {

            return searchcolumn.Save(newItem);

        }

        public bool DeleteSearchColumn(SearchColumn item)
        {
            return searchcolumn.Delete(item);
        }

        public SearchColumn GetSearchColumn(SearchColumn item)
        {
            return (SearchColumn)searchcolumn.GetItem<SearchColumn>(item);
        }

    }
}

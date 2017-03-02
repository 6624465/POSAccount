using POSAccount.ViewModals.Search;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using POSAccount.BusinessFactory;
using System.Data;
using System.Reflection;
using System.Collections;

namespace POSAccount.Areas.Search.Controllers
{
    public class SearchService
    {

        public static void Search<T>(SearchVm search)//SearchResultVm<T>
        {
            var obj = new SearchResultVm<T>();
            /*
            IRestClient client = new RestClient(ApiBaseUrl);
            var request = new RestRequest(Method.POST);
            request.Resource = "search/";
            request.AddJsonBody(search);

            var obj = client.Execute<SearchResultVm<T>>(request).Data;
            obj.searchWrapper.searchVm = search;
            */
            IDataReader searchResult = null;
            IList IList;
            switch (search.SearchTable)
            {
                //case SearchCategories.APINVOICE:
                //    return PartialView("Customer", SearchService.Search<Customer>(search));

                case SearchCategories.APINVOICE:
                    searchResult = new APInvoiceBO().PerformSearch(search.whereclause);
                    break;
                    //IList = DataReaderMapToList<NetTms.Objects.Search.Customer>(searchResult);
            }

           // return obj;
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                try
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    list.Add(obj);
                }
                catch (Exception ex)
                {

                }
            }
            return list;
        }
    }
}
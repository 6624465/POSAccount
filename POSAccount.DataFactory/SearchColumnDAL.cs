using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using POSAccount.Contract;
using System.Data.Common;
using POSAccount.Contract;

namespace POSAccount.DataFactory
{
    public class SearchColumnDAL  
    {
        private Database db;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchColumnDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<SearchColumn> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTSEARCHCOLUMN, MapBuilder<SearchColumn>.BuildAllProperties()).ToList();
        }

        public bool Save<T>(T item) where T : IContract
        {

            return false;

        }

        public bool Delete<T>(T item) where T : IContract
        {
            return false;
        }

        public IContract GetItem<T>(IContract lookupItem) where T : IContract
        {

            IRowMapper<SearchColumn> rowmapper = MapBuilder<SearchColumn>.BuildAllProperties();
            IParameterMapper parametermapper = new SearchColumnParameter("SearchCategory");

            var accessor = db.CreateSprocAccessor(DBRoutine.LISTSEARCHCOLUMN, parametermapper, rowmapper);
            var Item = accessor.Execute(((SearchColumn)lookupItem).SearchCategory).FirstOrDefault();
            return Item;
        }



        public List<SearchColumn> GetListByCategory(string searchCategory = "")
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTSEARCHCOLUMNBYCATEGORY,
                                            MapBuilder<SearchColumn>.BuildAllProperties(),
                                            searchCategory).ToList();
        }




        #endregion


        /// <summary>
        /// private classes for assigning parameters to the Accessor
        /// </summary>
        internal class SearchColumnParameter : IParameterMapper
        {
            private string paramProp;


            internal SearchColumnParameter(string paramVar)
            {
                paramProp = paramVar;

            }

            #region IParameterMapper Members

            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = paramProp;
                parameter.Value = parameterValues[0];
                command.Parameters.Add(parameter);
            }

            #endregion
        }





    }
}

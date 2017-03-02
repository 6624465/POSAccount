using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;

namespace POSAccount.BusinessFactory
{
    public class SystemWideConfigurationBO
    {
        private SystemWideConfigurationDAL systemwideconfigurationDAL;
        public SystemWideConfigurationBO()
        {

            systemwideconfigurationDAL = new SystemWideConfigurationDAL();
        }

        public List<SystemWideConfiguration> GetList()
        {
            return systemwideconfigurationDAL.GetList();
        }


        public bool SaveSystemWideConfiguration(SystemWideConfiguration newItem)
        {

            return systemwideconfigurationDAL.Save(newItem);

        }

        public bool DeleteSystemWideConfiguration(SystemWideConfiguration item)
        {
            return systemwideconfigurationDAL.Delete(item);
        }

        public SystemWideConfiguration GetSystemWideConfiguration(SystemWideConfiguration item)
        {
            return (SystemWideConfiguration)systemwideconfigurationDAL.GetItem<SystemWideConfiguration>(item);
        }

    }
}

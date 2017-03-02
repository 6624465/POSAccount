using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class AssetHeaderBO
    {
        private AssetHeaderDAL   assetheaderDAL;
        public AssetHeaderBO() {

            assetheaderDAL = new AssetHeaderDAL();
        }

        public List<AssetHeader> GetList(short branchID)
        {
            return assetheaderDAL.GetList(branchID);
        }


        public bool SaveAssetHeader(AssetHeader newItem)
        {

            return assetheaderDAL.Save(newItem);

        }

        public bool DeleteAssetHeader(AssetHeader item)
        {
            return assetheaderDAL.Delete(item);
        }

        public AssetHeader GetAssetHeader(AssetHeader item)
        {
            return (AssetHeader)assetheaderDAL.GetItem<AssetHeader>(item);
        }

    }
}

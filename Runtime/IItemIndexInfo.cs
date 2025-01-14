namespace UnityEngine.UI
{
    public interface IItemIndexInfo
    {
        #region ItemIndex

        public int GetViewIndex(GameObject go);

        public int GetDataIndex(GameObject go);

        public int ViewIndexToDataIndex(int viewIndex);

        public int DataIndexToViewIndex(int dataIndex);

        #endregion

        public GameObject GetItem(int dataIndex);
    }
}
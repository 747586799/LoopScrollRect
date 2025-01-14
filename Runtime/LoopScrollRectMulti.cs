using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public abstract class LoopScrollRectMulti : LoopScrollRectBase, IItemEffect, IItemIndexInfo
    {
        public Action<int, GameObject> OnProvideItem { get; set; }

        [HideInInspector]
        [NonSerialized]
        public LoopScrollMultiDataSource dataSource = null;

        protected override void ProvideData(Transform transform, int index)
        {
            dataSource.ProvideData(transform, index);
        }

        // Multi Data Source cannot support TempPool
        protected override RectTransform GetFromTempPool(int itemIdx)
        {
            RectTransform nextItem = prefabSource.GetObject(itemIdx).transform as RectTransform;
            nextItem.transform.SetParent(m_Content, false);
            nextItem.gameObject.SetActive(true);
            OnProvideItem?.Invoke(itemIdx, nextItem.gameObject);
            ProvideData(nextItem, itemIdx);
            return nextItem;
        }

        protected override void ReturnToTempPool(bool fromStart, int count)
        {
            Debug.Assert(m_Content.childCount >= count);
            if (fromStart)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    prefabSource.ReturnObject(m_Content.GetChild(i));
                }
            }
            else
            {
                int t = m_Content.childCount - count;
                for (int i = m_Content.childCount - 1; i >= t; i--)
                {
                    prefabSource.ReturnObject(m_Content.GetChild(i));
                }
            }
        }

        protected override void ClearTempPool()
        {
        }

        #region ItemIndex

        public int GetViewIndex(GameObject go)
        {
            for (int i = deletedItemTypeStart; i < m_Content.childCount - deletedItemTypeEnd; i++)
            {
                if (m_Content.GetChild(i) == go.transform)
                {
                    return i - deletedItemTypeStart;
                }
            }

            return -1;
        }

        public int GetDataIndex(GameObject go)
        {
            int viewIndex = GetViewIndex(go);
            return ViewIndexToDataIndex(viewIndex);
        }

        public int ViewIndexToDataIndex(int viewIndex)
        {
            if (viewIndex < 0)
                return -1;
            return viewIndex + itemTypeStart;
        }

        public int DataIndexToViewIndex(int dataIndex)
        {
            int viewIndex = dataIndex - itemTypeStart;
            if (viewIndex < 0)
                return -1;
            return viewIndex;
        }

        #endregion


        public GameObject GetItem(int dataIndex)
        {
            int viewIndex = DataIndexToViewIndex(dataIndex);
            if (viewIndex < 0 || viewIndex >= m_Content.childCount)
                return null;
            return m_Content.GetChild(viewIndex).gameObject;
        }
    }
}
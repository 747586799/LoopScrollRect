using System;

namespace UnityEngine.UI
{
    public interface IItemEffect
    {
        /// <summary>
        /// The first item id in LoopScroll.
        /// </summary>
        public int itemTypeStart { get; }

        /// <summary>
        /// The last item id in LoopScroll.
        /// </summary>
        public int itemTypeEnd { get; }

        public Action<int, GameObject> OnProvideItem { get; set; }

        public GameObject GetItem(int dataIndex);
    }
}
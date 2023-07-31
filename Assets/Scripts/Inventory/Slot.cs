using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Inventory
{
    public class Slot : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField]
        private Image image;

        public Item Item { get; set; }
        public int SlotIndex => gameObject.transform.GetSiblingIndex();

        private Color _transparent;

        private void Awake()
        {
            this._transparent = new Color(0, 0, 0, 0);
        }

        private void Start()
        {
            ClearSlot();
        }

        public void SetItem(ItemType itemType)
        {
            Item item = GameManager.Instance.ItemManager.GetItemByType(itemType);
            this.Item = item;
            this.image.sprite = item.ItemIcon;
            this.image.color = Color.white;
        }

        public void ClearSlot()
        {
            this.Item = null;
            this.image.sprite = null;
            this.image.color = _transparent;
        }
    }
}
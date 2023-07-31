using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemManager : MonoBehaviour
    {
        private readonly List<GameObject> _itemPrefabs = new();
        private readonly List<Item> _items = new();

        public void LoadAllItems()
        {
            foreach (var item in Resources.LoadAll<GameObject>("Items"))
            {
                _itemPrefabs.Add(item);
                _items.Add(item.GetComponent<Item>());
            }

            Debug.Log("Loaded " + _items.Count + " Items");
        }

        public static void SpawnItem(Vector2 position, ItemType itemType, int amount = 1)
        {
            SpawnItem(position, GameManager.Instance.ItemManager.GetItemByType(itemType), amount);
        }

        public static void SpawnItem(Vector2 position, Item item, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject itemPrefab = GameManager.Instance.ItemManager.GetItemPrefabByType(item.ItemType);
                GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
            }
        }

        public Item GetItemByType(ItemType itemType)
        {
            return _items.Find(item => item.ItemType == itemType);
        }

        public GameObject GetItemPrefabByType(ItemType itemType)
        {
            return _itemPrefabs.Find(item => item.GetComponent<Item>().ItemType == itemType);
        }
    }
}
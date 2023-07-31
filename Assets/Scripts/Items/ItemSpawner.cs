using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemType itemTypeToSpawn = ItemType.RANDOM;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        private void Start()
        {
            if (itemTypeToSpawn == ItemType.RANDOM)
            {
                do
                {
                    itemTypeToSpawn = EnumUtils.GetRandomItemType();
                } while (itemTypeToSpawn == ItemType.RANDOM);
            }

            Instantiate(GameManager.Instance.ItemManager.GetItemPrefabByType(itemTypeToSpawn), 
                new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
    }
}
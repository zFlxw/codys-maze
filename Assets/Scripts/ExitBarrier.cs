using UnityEngine;

public class ExitBarrier : MonoBehaviour
{
    // false = horizontal, true = vertical
    [SerializeField]
    private bool rotation = false;

    public void SpawnBarricade(GameObject barricade)
    {
        GameObject newItem = Instantiate(barricade, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        if (rotation)
        {
            newItem.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}

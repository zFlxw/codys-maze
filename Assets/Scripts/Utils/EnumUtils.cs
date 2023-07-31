using System;
using System.Diagnostics;

public class EnumUtils
{
    public static ItemType GetRandomItemType()
    {
        Array array = Enum.GetValues(typeof(ItemType));
        double r = UnityEngine.Random.Range(0.0f, 1.0f);
        ItemType itemType = (ItemType)array.GetValue(UnityEngine.Random.Range(0, array.Length - 1));

        while (((double)((int)itemType) / 100) < r || itemType == ItemType.RANDOM || itemType == ItemType.UNDEFINED)
        {
            itemType = (ItemType)array.GetValue(UnityEngine.Random.Range(0, array.Length));
            r = UnityEngine.Random.Range(0.0f, 1.0f);
        }

        return itemType;
    }
}

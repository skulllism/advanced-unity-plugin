using UnityEngine;
using AdvancedUnityPlugin;

public class weapon : MonoBehaviour
{
    public void Test(Equipable e)
    {
        Debug.Log(e.itemID);
    }
}

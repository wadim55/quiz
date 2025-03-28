using UnityEngine;
using UnityEngine.UI;
using YG;

public class Test : MonoBehaviour
{
    private int y;
    public Text t;
    void Start()
    {
        YG2.saves.coins++;
        t.text = YG2.saves.coins.ToString();
    }

   
}

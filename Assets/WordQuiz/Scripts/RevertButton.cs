using UnityEngine;
using UnityEngine.EventSystems;

public class RevertButton : MonoBehaviour, IPointerClickHandler
{
   


    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Inst.PlayClip(0);
       GameManager.Inst.ChangeScene(0);
    }
}

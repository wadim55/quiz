using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite[] sound;
    [SerializeField] private Image voicer;
    private bool SoundOn = true;
   

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SoundOn)
        {
            voicer.sprite = sound[1];
            GameManager.Inst.MuteSound(true);
        }
        else
        {
            voicer.sprite = sound[0];
            GameManager.Inst.MuteSound(false);
        }
        SoundOn = !SoundOn;
       
    }
}

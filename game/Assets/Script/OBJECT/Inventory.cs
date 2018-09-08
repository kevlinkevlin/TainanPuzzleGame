using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Fungus;
public class Inventory : MonoBehaviour, IHasChanged
{
    [SerializeField]
    Transform slots;
    [SerializeField]
    Text inventoryText;
    [SerializeField]
    Flowchart flowchart;
    [SerializeField]
    AudioSource putAudioSource;
    protected AudioSource openAudioSource;
    
    // Use this for initialization
    void Awake()
    {
        openAudioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        
        HasChanged();
    }

    #region IHasChanged implementation
    public void HasChanged()
    {
        
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        //builder.Append(" - ");
        foreach (Transform slotTransform in slots)
        {
           
            GameObject item = slotTransform.GetComponent<Slot>().item;
            if (item)
            {   
                PutSound();
                builder.Append(item.name);
                //   builder.Append(" - ");
            }
        }
        if (builder.ToString() == "LIGHT" && flowchart.GetBooleanVariable("磚塊判斷") == false)
        {
            inventoryText.text = "complete";
            flowchart.SetBooleanVariable("磚塊判斷", true);
            OpenSound();
            Flowchart.BroadcastFungusMessage("磚塊");
        }
        else
        {
            inventoryText.text = builder.ToString();
        }
        if (builder.ToString() == "216435"&& flowchart.GetBooleanVariable("排序判斷")==false)
        {
            inventoryText.text = "complete";
            flowchart.SetBooleanVariable("排序判斷", true);
            OpenSound();
            Flowchart.BroadcastFungusMessage("排序");
        }
        else 
        {
            inventoryText.text = builder.ToString();
        }
    }
    #endregion
    protected void OpenSound()
    {
        if (openAudioSource != null)
        {
            openAudioSource.Play();
        }
    }
    protected void PutSound()
    {
        if (putAudioSource != null)
        {
            putAudioSource.Play();
        }
    }
}


namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}
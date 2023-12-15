using System.Collections;
using Core.EventsSO.GameEventsTypes;
using Core.SingletonsSO;
using Inventory;
using Inventory.Item;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ProtoChess : MonoBehaviour
{
    public InputAction interactInput;
    public GameObject textInteract;
    public GameObject textLoot;
    public bool lootable= true;
    public GameObject top;
    public PlayerInventorySO inventory;
    public UnityEvent action;
    public bool isInfinite;
    [SerializeField] private PopUpDataGameEvent popUpDataGameEvent;
    private PopUpData popUpData;
    
    void Start()
    {
        interactInput.Disable();
        popUpData = new PopUpData();

    }
    
    void Update()
    {
        if (interactInput.IsPressed() && lootable)
        {
            lootable = false;
            top.GetComponent<Animator>().SetBool("Lootable",false);
            textInteract.SetActive(false);
            textLoot.SetActive(true);
            this.action?.Invoke();
            if (isInfinite)
            {
                StartCoroutine("Cooldown");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && lootable)
        {
            interactInput.Enable();
            textInteract.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            interactInput.Disable();
            textInteract.SetActive(false);
            textLoot.SetActive(false);
        }
    }
    
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.25f);
        top.GetComponent<Animator>().SetBool("Lootable",true);
        lootable = true;
        textLoot.SetActive(false);
        textInteract.SetActive(true);
    }

    public void AddItem(GridItemSO item)
    {
        this.inventory.AddItemToInventory(ItemData.CreateItemData(item));
        popUpData.icon = item.icon;
        popUpData.title = item.itemName;
        popUpDataGameEvent.RaiseEvent(popUpData);
        //PlayerHandler.Instance.AddItemToInventory(ItemData.CreateItemData(item));
    }
}

using Core.SingletonsSO;
using Inventory.Item;
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
    public UnityEvent action;
    
    void Start()
    {
        interactInput.Disable();
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

    public void AddItem(GridItemSO itemDefinition)
    {
        PlayerHandler.Instance.AddItemToInventory(ItemData.CreateItemData(itemDefinition));
    }
}

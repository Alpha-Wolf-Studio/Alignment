using UnityEngine;
public class PlayerInventory : PlayerState
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        playerController.TryOnpenInventory();
    }
}
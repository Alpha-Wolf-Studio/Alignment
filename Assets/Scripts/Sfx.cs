using UnityEngine;
public class Sfx : MonoBehaviour
{
    private static Sfx _sfx;
    public static Sfx Get() => _sfx;

    private void Awake()
    {
        _sfx = this;
    }
    public bool GetEnable(ListSfx sound)
    {
        return listOn[(int)sound];
    }
    public string GetList(ListSfx sound)
    {
        return listSfx[(int)sound];
    }
    public enum ListSfx
    {
        CompiAttack,
        RaptorAttack,
        TrikeAttack,
        DiloAttack,

        PlayerStepsOn,
        PlayerStepsOff,
        PlayerPickItem,
        PlayerDropItem,
        PlayerAttack,
        PlayerJetPack,
        PlayerJump,
        PlayerArmorDamage,
        PlayerEnergyDamage,
        PlayerDie,

        CompletedTask,
        CompletedQuest,
        HelpPopUp,
        PauseOn,
        PauseOff,

        UiClickButton,
        UiButtonEnter,
        UiButtonExit,
        UiOpenInventory,
        UiCloseInventory,
        UiSwapItem,
        UiUseItem,
        UiCraftSuccessful,
        UiCraftFail
    }
    string[] listSfx =
    {
        "CompiAttack",
        "RaptorAttack",
        "TrikeAttack",
        "DiloAttack",

        "PlayerStepsOn",
        "PlayerStepsOff",
        "PlayerPickItem",
        "PlayerDropItem",
        "PlayerAttack",
        "PlayerJetPack",
        "New_EventTest", //"PlayerJump",
        "PlayerArmorDamage",
        "PlayerEnergyDamage",
        "PlayerDie",

        "CompletedTask",
        "CompletedQuest",
        "HelpPopUp",
        "PauseOn",
        "PauseOff",

        "UiClickButton",
        "UiButtonEnter",
        "UiButtonExit",
        "UiOpenInventory",
        "UiCloseInventory",
        "UiSwapItem",
        "UiUseItem",
        "UiCraftSuccessful",
        "UiCraftFail"
    };
    bool[] listOn =
    {
        false,  // CompiAttack,   
        false,  // RaptorAttack,   
        false,  // TrikeAttack,    
        false,  // DiloAttack,               
                //
        false,  // PlayerStepsOn,            
        false,  // PlayerStepsOff,           
        false,  // PlayerPickItem,           
        false,  // PlayerDropItem,           
        false,  // PlayerAttack,             
        false,  // PlayerJetPack,            
        true,   // PlayerJump,               
        false,  // PlayerArmorDamage,        
        false,  // PlayerEnergyDamage,       
        false,  // PlayerDie,                
                // 
        false,  // CompletedTask,            
        false,  // CompletedQuest,           
        false,  // HelpPopUp,                
        false,  // PauseOn,                  
        false,  // PauseOff,                 
                // 
        false,  // UiClickButton,            
        false,  // UiButtonEnter,            
        false,  // UiButtonExit,             
        false,  // UiOpenInventory,          
        false,  // UiCloseInventory,         
        false,  // UiSwapItem,               
        false,  // UiUseItem,                
        false,  // UiCraftSuccessful,        
        false   // UiCraftFail               
    };
}
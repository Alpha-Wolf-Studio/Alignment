using UnityEngine;
public class Sfx : MonoBehaviourSingleton<Sfx>
{
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
        PlayerRunOn,
        PlayerRunOff,
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
        AmbField,
        AmbBeach,
        AmbForest,
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
        "PlayerRunOn",
        "PlayerRunOff",
        "PlayerPickItem",
        "PlayerDropItem",
        "PlayerAttack",
        "PlayerJetPack",
        "PlayerJump",
        "PlayerArmorDamage",
        "PlayerEnergyDamage",
        "PlayerDie",

        "CompletedTask",
        "CompletedQuest",
        "HelpPopUp",
        "AmbField",
        "AmbBeach",
        "AmbForest",
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
        false,   // CompiAttack,           
        false,   // RaptorAttack,          
        false,   // TrikeAttack,           
        false,   // DiloAttack,            
                                           
        true,   // PlayerStepsOn,          
        true,   // PlayerStepsOff,         
        true,   // PlayerRunOn,           
        true,   // PlayerRunOff,           
        true,   // PlayerPickItem,         
        true,   // PlayerDropItem,         
        true,   // PlayerAttack,           
        false,   // PlayerJetPack,         
        true,   // PlayerJump,             
        true,   // PlayerArmorDamage,      
        true,   // PlayerEnergyDamage,     
        true,   // PlayerDie,              
                                           
        true,   // CompletedTask,          
        true,   // CompletedQuest,         
        false,   // HelpPopUp,             
        false,   // AmbField,              
        false,   // AmbBeach,              
        false,   // AmbForest,             
        true,   // PauseOn,                
        true,   // PauseOff,               
                                           
        true,   // UiClickButton,          
        true,   // UiButtonEnter,          
        true,   // UiButtonExit,           
        true,   // UiOpenInventory,        
        true,   // UiCloseInventory,       
        true,   // UiSwapItem,             
        true,   // UiUseItem,              
        true,   // UiCraftSuccessful,      
        true    // UiCraftFail             
    };
}
/*


 
 
 
 
 
 */
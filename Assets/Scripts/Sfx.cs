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
        false,   // CompiAttack,                || 
        false,   // RaptorAttack,               || 
        false,   // TrikeAttack,                || 
        false,   // DiloAttack,                 || 
                                                
        true,   // PlayerStepsOn,               || PlayerStepsOn
        true,   // PlayerStepsOff,             || PlayerStepsOff
        true,   // PlayerPickItem,             || PlayerPickItem
        true,   // PlayerDropItem,             || PlayerDropItem
        true,   // PlayerAttack,               || PlayerAttack
        false,   // PlayerJetPack,              || 
        true,   // PlayerJump,                 || PlayerJump
        true,   // PlayerArmorDamage,          || PlayerArmorDamage
        true,   // PlayerEnergyDamage,         || PlayerEnergyDamage
        true,   // PlayerDie,                  || PlayerDie
                                                
        true,   // CompletedTask,              || CompletedTask
        true,   // CompletedQuest,             || CompletedQuest
        false,   // HelpPopUp,                  || 
        false,   // AmbField,                   || 
        false,   // AmbBeach,                   || 
        false,   // AmbForest,                  || 
        true,   // PauseOn,                    || PauseOn
        true,   // PauseOff,                   || PauseOff
                                                
        true,   // UiClickButton,              || UiClickButton
        true,   // UiButtonEnter,              || UiButtonEnter
        true,   // UiButtonExit,               || UiButtonExit
        true,   // UiOpenInventory,            || UiOpenInventory
        true,   // UiCloseInventory,           || UiCloseInventory
        true,   // UiSwapItem,                 || UiSwapItem
        true,   // UiUseItem,                  || UiUseItem
        true,   // UiCraftSuccessful,          || UiCraftSuccessful
        true    // UiCraftFail                 || UiCraftFail
    };
}
/*


 
 
 
 
 
 */
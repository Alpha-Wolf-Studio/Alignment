using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class TextFontReplace : MonoBehaviour
{ 
    [SerializeField] TMP_FontAsset newFontAsset = null;
    [SerializeField] bool start = false; 

    void Update()
    {
        if (start && newFontAsset != null) 
        {
            TextMeshProUGUI[] allObjects = GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in allObjects) 
            {
                text.font = newFontAsset;
            }                    
        }
        start = false;
    }
}

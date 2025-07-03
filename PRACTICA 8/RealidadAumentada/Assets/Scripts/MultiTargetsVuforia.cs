using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
public class MultiTargetVuforia : MonoBehaviour{ 
    [SerializeField] private GameObject startModel; 
    private int modelsCount; 
    private int indexCurrentModel; 
    private bool rotateMode;
    // Start is called before the first frame update 
    void Start(){ 
        modelsCount = transform.childCount; 
        indexCurrentModel = startModel.transform.GetSiblingIndex();
        rotateMode= false;
    } 
    public void ChangeARModel(int index){ 
        transform.GetChild(indexCurrentModel).gameObject.SetActive(false); 
        int newIndex = indexCurrentModel + index; 
        if (newIndex < 0){ 
            newIndex = modelsCount - 1; 
        } 
        else if (newIndex > modelsCount - 1){ 
            newIndex = 0; 
        } 
        GameObject newModel = transform.GetChild(newIndex).gameObject; 
        newModel.SetActive(true); 
        Debug.Log($"Activating model: {newModel.name}, Renderer enabled: {newModel.GetComponent<MeshRenderer>().enabled}");
        indexCurrentModel = newModel.transform.GetSiblingIndex();
    }
} 

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowDebug : MonoBehaviour
{
    
    public List<GameObject> Objects;

    private void Awake()
    {
        Objects.ToList().ForEach(x => x.SetActive(false));
    }

    private void OnMouseUpAsButton()
    {
        if(Objects.Count > 0)
        {
            if(Objects.Count == 2){
            Objects[0].SetActive(true);
            Objects[1].SetActive(true);
            Objects.Remove(Objects[0]); 
            Objects.Remove(Objects[0]); 
            }else{
                Objects[0].SetActive(true);
                Objects.Remove(Objects[0]);
            }
        }
    }
}

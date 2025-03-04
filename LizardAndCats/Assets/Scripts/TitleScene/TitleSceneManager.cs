using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

enum E_BUTTON
{
    START,
    FIRST,
    SECOND,
    THIRD,
    ENDLESS,

    COUNT,
};


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject[] ButtonObj = new GameObject[(int)E_BUTTON.COUNT];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TitleButtonOn()
    {
        
    }
}

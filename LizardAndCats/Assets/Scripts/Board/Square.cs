using Const;
using UnityEngine;

public class Square : MonoBehaviour
{
    [Tooltip("êHÇ◊ï®"), SerializeField]
    GameObject foodObj;
    [Tooltip("ëfçﬁ"), SerializeField]
    GameObject materialObj;
    [Tooltip("ëÉ"), SerializeField]
    GameObject nestObj;

    DropObj dropObj = DropObj.NONE;

    public void Init()
    {
        DeleteObj();
    }

    public void SetObj(DropObj _obj)
    {
        if (dropObj != DropObj.NONE)
        {
            print($"{dropObj}Ç{_obj}Ç…è„èëÇ´î≠ê∂");
        }

        foodObj.SetActive(false);
        materialObj.SetActive(false);
        nestObj.SetActive(false);

        dropObj = _obj;

        switch (dropObj)
        {
            case DropObj.FOOD:
                foodObj.SetActive(true);
                break;
            case DropObj.MATERIAL:
                materialObj.SetActive(true);
                break;
            case DropObj.NEST:
                nestObj.SetActive(true);
                break;
        }

    }

    public void DeleteObj()
    {
        dropObj = DropObj.NONE;
        foodObj.SetActive(false);
        materialObj.SetActive(false);
        nestObj.SetActive(false);
    }
}

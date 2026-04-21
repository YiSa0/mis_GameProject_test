using UnityEngine;

public class MinimapTarget : MonoBehaviour
{
    [Header("綁定實體建築物")]
    [Tooltip("請把這棟建築物一樓門口掛有 SignInteraction 的物件拖曳到這裡")]
    public SignInteraction buildingSign;
}
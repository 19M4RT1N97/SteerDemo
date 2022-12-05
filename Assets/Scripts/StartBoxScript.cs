using Mono.Cecil.Cil;
using UnityEngine;

public class StartBoxScript : MonoBehaviour
{
    [SerializeField] private int StreetId;

    public int GetStreetId() => StreetId;
}

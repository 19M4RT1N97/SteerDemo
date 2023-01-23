using JetBrains.Annotations;
using Mono.Cecil.Cil;
using UnityEngine;

public class StartBoxScript : MonoBehaviour
{
    public int? StreetAngle = null;

    [CanBeNull] public GameObject Car = null;
}

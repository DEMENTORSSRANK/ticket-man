using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseForCoin : MonoBehaviour
{
    public Vector2 Position => transform.position;
    
    public bool IsBusy { get; set; }
}

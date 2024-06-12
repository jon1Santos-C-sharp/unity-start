using System;
using UnityEditor;
using UnityEngine;

public class BattleNPController : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Battle NPC");
    }
}
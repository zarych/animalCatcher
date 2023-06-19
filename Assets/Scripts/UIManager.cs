using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx.Extensions;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;

    void Start()
    {
        AnimalsGroupMovementManager.Instance.SavedAnimals.SubscribeToText(score);
    }
}

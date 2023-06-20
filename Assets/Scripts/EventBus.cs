using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public static Action<GameObject> OnAnimalSpawned;
    public static Action<Vector3, GameObject> OnPlayerTouchedAnimal;
    public static Action<Vector3, GameObject> OnAnimalSaved;
    public static Action<Vector3, float> OnPlayerMoved;
}

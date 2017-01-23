using UnityEngine;
using System.Collections;

public abstract class AbstractAgent : MonoBehaviour
{
    public CityGenerator generator;

    public abstract void agentAction();
}

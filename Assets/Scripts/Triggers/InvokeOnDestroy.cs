using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnDestroy : Trigger
{
    private void OnDestroy()
    {
        Activate();
    }
}

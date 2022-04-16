
using UnityEngine;

public class TestInter : MonoBehaviour
{
    private static int i = 0;
    public void SayHi()
    {
        Debug.Log($"For the {++i} time, i say Hello!");
    }
}

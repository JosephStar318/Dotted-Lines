using System.Collections;
using UnityEngine;

public static class Delay
{
    public static IEnumerator Seconds(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}

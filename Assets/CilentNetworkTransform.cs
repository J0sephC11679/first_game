using UnityEngine;
using Unity.Netcode.Components;

public class CilentNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}

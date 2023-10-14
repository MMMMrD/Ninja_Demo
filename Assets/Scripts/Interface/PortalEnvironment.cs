using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PortalEnvironment
{
    void InitEnvironment(Vector2 point, Transform userTransform, Transform targetTransform, CharacterStats attacker);

    // GameObject _object = ObjectPool.Instance.GetObjectButNoSetActive(prefab);
    // _object.transform.position = transform.position;
    // _object.transform.rotation = transform.rotation;
    // _object.SetActive(true);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGuardiansSpace : MonoBehaviour
{
    [SerializeField] Transform _transform;
    private void Update()
    {
        if (_transform.childCount < 2)
            _transform.transform.GetChild(0).gameObject.SetActive(true);
    }
}

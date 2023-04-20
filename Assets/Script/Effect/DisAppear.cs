using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisAppear : MonoBehaviour
{
    [SerializeField] float disappear_Time;

    private void OnEnable()//활성화 도리때마다 호출되는 함수.
    {
        StartCoroutine(DisAppearCoroutine());
    }
    IEnumerator DisAppearCoroutine()
    {
        yield return new WaitForSeconds(disappear_Time);

        gameObject.SetActive(false);
    }
}

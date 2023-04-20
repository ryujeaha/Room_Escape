using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisAppear : MonoBehaviour
{
    [SerializeField] float disappear_Time;

    private void OnEnable()//Ȱ��ȭ ���������� ȣ��Ǵ� �Լ�.
    {
        StartCoroutine(DisAppearCoroutine());
    }
    IEnumerator DisAppearCoroutine()
    {
        yield return new WaitForSeconds(disappear_Time);

        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallToFade : MonoBehaviour
{
    public GameObject[] _toFade;
    public GameObject[] _toEnable;

    private Material[][] _originalMaterials;

    private int _triggerCounter = 0; // Счетчик коллайдеров

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggerCounter++;

            if (_triggerCounter == 1)
            {
                // Выключаем объекты, если они есть в массиве _toEnable
                foreach (GameObject go in _toEnable)
                {
                    go.SetActive(false);
                }

                _originalMaterials = new Material[_toFade.Length][];

                for (int i = 0; i < _toFade.Length; i++)
                {
                    Renderer renderer = _toFade[i].GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Сохраняем текущие материалы перед изменением на RenderMode Fade
                        _originalMaterials[i] = renderer.materials;
                        Material[] fadeMaterials = new Material[renderer.materials.Length];
                        for (int j = 0; j < renderer.materials.Length; j++)
                        {
                            fadeMaterials[j] = new Material(renderer.materials[j]);
                            fadeMaterials[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            fadeMaterials[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            fadeMaterials[j].SetInt("_ZWrite", 0);
                            fadeMaterials[j].DisableKeyword("_ALPHATEST_ON");
                            fadeMaterials[j].EnableKeyword("_ALPHABLEND_ON");
                            fadeMaterials[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            fadeMaterials[j].renderQueue = 3000;
                        }
                        renderer.materials = fadeMaterials;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggerCounter--;

            if (_triggerCounter == 0)
            {
                // Включаем объекты обратно, если они есть в массиве _toEnable
                foreach (GameObject go in _toEnable)
                {
                    go.SetActive(true);
                }

                for (int i = 0; i < _toFade.Length; i++)
                {
                    Renderer renderer = _toFade[i].GetComponent<Renderer>();
                    if (renderer != null && i < _originalMaterials.Length)
                    {
                        // Возвращаем сохраненные оригинальные материалы
                        renderer.materials = _originalMaterials[i];
                    }
                }
            }
        }
    }
}

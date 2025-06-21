using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGroupAnimation : MonoBehaviour
{
    DefenceGameRule gameRule;
    DefenceRewardAssetData rewardData;
    [SerializeField] Transform BaseTransfrom;

    List<GameObject> Slots = new List<GameObject>();
    List<GameObject> particleSystems = new List<GameObject>();
    List<Vector2> ResultPos = new List<Vector2>();
    HorizontalLayoutGroup horizontalLayout;

    [SerializeField] AnimationCurve curve;

    private void Start()
    {
        horizontalLayout = GetComponent<HorizontalLayoutGroup>();

    }

    public void OnActive()
    {
        Initailize();
    }

    void Initailize()
    {
        gameRule = (DefenceGameRule)GameManager.Instance.GameMode.Rule;
        rewardData = gameRule.RewardData;

        int rewardCount = rewardData._defenceRewardData.Count;

        if (rewardCount > transform.childCount)
            rewardCount = transform.childCount;

        for (int i = 0; i < rewardCount; ++i)
        {
            Slots.Add(transform.GetChild(i).gameObject);
            particleSystems.Add(Slots[i].transform.GetChild(2).gameObject);
            Slots[i].SetActive(true);
        }

        StartCoroutine(SetResultPos());
    }

    IEnumerator SetResultPos()
    {
        yield return null;

        for (int i = 0; i < Slots.Count; i++)
        {
            ResultPos.Add(Slots[i].transform.position);

            Slots[i].transform.position = BaseTransfrom.position;
            Slots[i].transform.localScale = BaseTransfrom.localScale;
        }

        for (int i = 0; i < Slots.Count; ++i)
        {
            Slots[i].SetActive(false);
            Slots[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            for (int j = 0; j < Slots[i].transform.childCount; j++)
                Slots[i].transform.GetChild(j).gameObject.SetActive(true);
        }

        horizontalLayout.enabled = false;

        StartCoroutine(ShowReward());
    }

    IEnumerator ShowReward()
    {
        float animDuration = 0.3f;

        for (int i = 0; i < Slots.Count; i++)
        {
            GameObject slot = Slots[i];
            slot.SetActive(true);

            Vector3 startPos = BaseTransfrom.position;
            Vector3 endPos = ResultPos[i];

            Vector3 startScale = BaseTransfrom.localScale;
            Vector3 endScale = Vector3.one;

            float elapsed = 0f;

            slot.transform.position = startPos;

            while (elapsed < animDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animDuration);
                float speed = curve.Evaluate(t);
                slot.transform.position = Vector3.Lerp(slot.transform.position, endPos, t * speed);
                slot.transform.localScale = Vector3.Lerp(startScale, endScale, t * speed);

                yield return null;
            }

            slot.transform.position = endPos;
            slot.transform.localScale = endScale;

            UIParticleComponent[] Particles;

            Particles = particleSystems[i].transform.GetComponentsInChildren<UIParticleComponent>();

            foreach (UIParticleComponent pComponent in Particles)
                pComponent.StartParticleEmission();

            yield return new WaitForSeconds(0.1f);
        }
        horizontalLayout.enabled = true;
    }
}

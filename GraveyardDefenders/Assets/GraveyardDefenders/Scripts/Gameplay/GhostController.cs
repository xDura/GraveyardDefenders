using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace XD
{
    public class GhostController : MonoBehaviour
    {
        public PlayerCharacter target;
        public List<Material> materials; //TODO: make this one material when the model is updated on blender
        public float fadeTime;
        public float followVelocity;

        [Header("SineWaveInYAxis")]
        public float sineYFreq;
        public float sineYAmplitude;
        public float offset;

        private string fadeId;

        void Awake()
        {
            fadeId = GetInstanceID() + "Appear";
            CollectMaterials();
            DisappearInstant();
            Appear();
        }

        public void Init(PlayerCharacter character)
        {
            target = character;
            DisappearInstant();
            Appear();
        }

        private void CollectMaterials()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                materials.AddRange(renderers[i].materials);
        }

        [ContextMenu("Appear")]
        public void Appear()
        {
            for (int i = 0; i < materials.Count; i++) materials[i].DOFade(1.0f, fadeTime).SetId(fadeId);
        }

        [ContextMenu("Disappear")]
        public void Disappear()
        {
            for (int i = 0; i < materials.Count; i++) materials[i].DOFade(0.0f, fadeTime).SetId(fadeId);
        }

        public void DisappearAndDespawn()
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (i == 0)
                    materials[i].DOFade(0.0f, fadeTime).SetId(fadeId).OnComplete(Despawn);
            }
        }

        public void Despawn()
        {
            DOTween.Kill(fadeId);
            NPCManager.Instance.RemoveGhost(this);
        }

        private void DisappearInstant()
        {
            Color auxC;
            for (int i = 0; i < materials.Count; i++)
            {
                auxC = materials[i].color;
                auxC.a = 0.0f;  
                materials[i].color = auxC;
            }
        }

        void Update()
        {
            Vector3 auxPos = transform.position;
            if (target)
            {
                Vector3 targetPos = target.transform.position;
                Vector3 toPlayer = targetPos - auxPos;
                toPlayer.y = 0.0f;

                Vector3 velocity = (toPlayer.normalized * (followVelocity * Time.deltaTime));
                auxPos += velocity;

                transform.rotation = Quaternion.LookRotation(toPlayer, Vector3.up);
            }

            auxPos.y = (Mathf.Sin(Time.time * sineYFreq) * sineYAmplitude) + offset;
            transform.position = auxPos;
        }
    }   
}

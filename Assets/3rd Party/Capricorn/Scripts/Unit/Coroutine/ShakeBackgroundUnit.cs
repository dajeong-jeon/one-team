using System.Collections;
using System.Linq;
using UnityEngine;
namespace Dunward.Capricorn
{
    [System.Serializable]
    [UnitDirectory("Image")]
    public class ShakeBackgroundUnit : CoroutineUnit
    {
        public float scale = 1;
        public float elapsedTime = 0.5f;
#if UNITY_EDITOR
        protected override string info => "Shake Background";
        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);
            scale = Mathf.Clamp(UnityEditor.EditorGUI.FloatField(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Scale", scale), 0f, float.MaxValue);
            height += UnityEditor.EditorGUIUtility.singleLineHeight;
            elapsedTime = Mathf.Clamp(UnityEditor.EditorGUI.FloatField(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "elapsedTime", elapsedTime), 0f, float.MaxValue);
            height += UnityEditor.EditorGUIUtility.singleLineHeight;
        }
        public override float GetHeight()
        {
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight * 2;
        }
#endif
        public override IEnumerator Execute(params object[] args)
        {
            var database = Resources.Load<BackgroundDatabase>("BackgroundDatabase");
            var prefab = args[1] as Ref<GameObject>;
            var time = 0f;
            Vector3 originalPosition = prefab.Value.transform.localPosition;
            while (time < elapsedTime)
            {
                float offsetX = Random.Range(-1f, 1f) * scale;
                float offsetY = Random.Range(-1f, 1f) * scale;
                prefab.Value.transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);
                time += Time.deltaTime;
                yield return null;
            }
            prefab.Value.transform.localPosition = originalPosition;
        }
    }
}
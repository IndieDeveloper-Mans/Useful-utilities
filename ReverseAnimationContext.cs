using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Helpers
{
    public static class ReverseAnimationContext
    {
        [MenuItem("Assets/Create/Animation Reverse Clip", false, 60)]
        private static void ReverseClip()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string directoryPath = System.IO.Path.GetDirectoryName(path);
            string fileName = System.IO.Path.GetFileName(path).Split('.')[0];
            string fileExtension = System.IO.Path.GetExtension(path);
            string copiedFilePath = System.IO.Path.Combine(directoryPath, $"{fileName}_Reversed{fileExtension}");
           AssetDatabase.CopyAsset(path, copiedFilePath);

            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(copiedFilePath);

            if (clip == null)
            {
               return;
            }

            float clipLength = clip.length;
           var editorBindings = AnimationUtility.GetCurveBindings(clip);

            foreach (var binding in editorBindings)
            {
               var curve = AnimationUtility.GetEditorCurve(clip, binding);
               var keys = curve.keys;

               var postWrapmode = curve.postWrapMode;
                curve.postWrapMode = curve.preWrapMode;
                curve.preWrapMode = postWrapmode;

                for (int i = 0; i < keys.Length; i++)
                {
                   var K = keys[i];
                    K.time = clipLength - K.time;

                   var tmp = -K.inTangent;
                   K.inTangent = -K.outTangent;
                   K.outTangent = tmp;

                    keys[i] = K;
               }

               curve.keys = keys;
               clip.SetCurve(binding.path, binding.type, binding.propertyName, curve);
            }

            var events = AnimationUtility.GetAnimationEvents(clip);
            foreach (var @event in events)
           {
              @event.time = clipLength - @event.time;
            }
            AnimationUtility.SetAnimationEvents(clip, events);

          Debug.Log("Animation reversed!");
       }

       [MenuItem("Assets/Create/Animation Reverse Clip", true,0)]
       private static bool ReverseClipValidation() => Selection.activeObject is AnimationClip;

       private static AnimationClip SelectedClip => Selection.GetFiltered<AnimationClip>(SelectionMode.Assets).FirstOrDefault();
   }
}

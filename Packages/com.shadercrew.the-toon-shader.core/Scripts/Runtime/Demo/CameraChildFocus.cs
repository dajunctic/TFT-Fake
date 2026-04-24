using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ShaderCrew.TheToonShader.ShaderUtils;

namespace ShaderCrew.TheToonShader
{
    public class CameraChildFocus : MonoBehaviour
    {
        public Camera targetCamera;
        public float distanceFromChild = 5.0f;
        public float yOffset = 0.0f;

        public float transitionSpeed = 2.0f;
        public GameObject currentMaterialText;

        public GameObject originalTemple;
        private bool isInitial = true;

        private Transform[] headerTransforms;
        private Transform[] materialTransforms;
        private string currentKey = "";
        private int currentKeyIndex = 0;
        private int currentMaterialIndex = 0;

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        Dictionary<string, List<Transform>> headersToMaterials;

        RenderPipelineOptions rp;
        void Start()
        {
            rp = ShaderUtils.getCurrentRenderPipeline();

            int childCount = transform.childCount;
            headerTransforms = new Transform[childCount];

            headersToMaterials = new Dictionary<string, List<Transform>>();
            for (int i = 0; i < childCount; i++)
            {
                Transform header = transform.GetChild(i);
                headerTransforms[i] = header;
                string nameKey = header.name;
                headersToMaterials[nameKey] = new List<Transform>();

                int materialCount = header.childCount;
                for (int j = 0; j < materialCount; j++)
                {
                    headersToMaterials[nameKey].Add(header.GetChild(j));

                }
            }
            currentKey = headersToMaterials.Keys.First();

            if(rp == RenderPipelineOptions.URP2D)
            {
                targetPosition = originalTemple.transform.position + -1 * originalTemple.transform.forward * distanceFromChild;
            } else
            {
                targetPosition = originalTemple.transform.position + originalTemple.transform.forward * distanceFromChild;
            }
            targetPosition.y += yOffset;
            targetRotation = Quaternion.LookRotation(originalTemple.transform.position - targetPosition + new Vector3(0, yOffset / 2, 0));


            //// Set initial camera target if there are children
            //if (headersToMaterials[currentKey].Count > 0 && targetCamera != null)
            //{
            //    FocusOnChild(currentMaterialIndex);
            //}
        }

        void Update()
        {
            string test;
            if (!isInitial)
            {
                test = "<b><size=50>" + currentKey + "</size></b>\r\n<b>" + headersToMaterials[currentKey][currentMaterialIndex].name + "</b>";
            }
            else
            {
                if(rp != RenderPipelineOptions.URP2D)
                {
                    test = "<b><size=50>" + "Unity Lit" + "</size></b>";
                }
                else
                {
                    test = "<b><size=50>" + "Unity Sprite Lit Default" + "</size></b>";
                }

            }
            currentMaterialText.GetComponent<Text>().text = test;

            //if (Input.GetKeyDown(KeyCode.D))
            if (SystemIndependentInput.GetKeyDown(KeyCode.D))
            {
                MoveToNextChild();
                isInitial = false;
            }
            //else if (Input.GetKeyDown(KeyCode.A))
            else if(SystemIndependentInput.GetKeyDown(KeyCode.A))
            {
                if (currentMaterialIndex != 0)
                {
                    if (!isInitial)
                        MoveToPreviousChild();
                }
                else
                {

                    if (rp == RenderPipelineOptions.URP2D)
                    {
                        targetPosition = originalTemple.transform.position + -1 * originalTemple.transform.forward * distanceFromChild;
                    }
                    else
                    {
                        targetPosition = originalTemple.transform.position + originalTemple.transform.forward * distanceFromChild;
                    }

                    targetPosition.y += yOffset;

                    if (rp != RenderPipelineOptions.URP2D)
                    {
                        targetRotation = Quaternion.LookRotation(originalTemple.transform.position - targetPosition + new Vector3(0, yOffset / 2, 0));
                    }
                        isInitial = true;
                }

            }
            //else if (Input.GetKeyDown(KeyCode.W))
            else if (SystemIndependentInput.GetKeyDown(KeyCode.W))
                    {
                MoveToNextHeader();
            }
            //else if (Input.GetKeyDown(KeyCode.S))
            else if (SystemIndependentInput.GetKeyDown(KeyCode.S))
            {
                MoveToPreviousHeader();
            }

            targetCamera.transform.position = Vector3.Lerp(targetCamera.transform.position, targetPosition, Time.deltaTime * transitionSpeed);

            if (rp != RenderPipelineOptions.URP2D)
            {
                targetCamera.transform.rotation = Quaternion.Lerp(targetCamera.transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);
            }

        }

        private void MoveToNextChild()
        {
            if (headersToMaterials[currentKey].Count == 0) return;
            if (!isInitial)
            {
                currentMaterialIndex = (currentMaterialIndex + 1) % headersToMaterials[currentKey].Count;

            }
            else
            {
                currentMaterialIndex = 0;

            }
            FocusOnChild(currentMaterialIndex);
        }

        private void MoveToPreviousChild()
        {
            if (headersToMaterials[currentKey].Count == 0) return;
            currentMaterialIndex = (currentMaterialIndex - 1 + headersToMaterials[currentKey].Count) % headersToMaterials[currentKey].Count;
            FocusOnChild(currentMaterialIndex);
        }

        private void MoveToNextHeader()
        {
            if (headerTransforms.Length == 0) return;
            currentKeyIndex = (currentKeyIndex + 1) % headerTransforms.Length;
            currentKey = headerTransforms[currentKeyIndex].name;
            currentMaterialIndex = 0;
            FocusOnChild(0);
        }
        private void MoveToPreviousHeader()
        {
            if (headersToMaterials[currentKey].Count == 0) return;
            currentKeyIndex = (currentKeyIndex - 1 + headerTransforms.Length) % headerTransforms.Length;
            currentKey = headerTransforms[currentKeyIndex].name;
            currentMaterialIndex = 0;
            FocusOnChild(0);
        }
        private void FocusOnChild(int index)
        {
            if (targetCamera == null || index < 0 || index >= headersToMaterials[currentKey].Count) return;
            Transform targetChild = headersToMaterials[currentKey][index];
            //Debug.Log(targetChild.name);


            if (rp == RenderPipelineOptions.URP2D)
            {
                targetPosition = targetChild.position + -1 * targetChild.forward * distanceFromChild;
            }
            else
            {
                targetPosition = targetChild.position + targetChild.forward * distanceFromChild;
            }

            targetPosition.y += yOffset;
            if (rp != RenderPipelineOptions.URP2D)
            {
                targetRotation = Quaternion.LookRotation(targetChild.position - targetPosition + new Vector3(0, yOffset / 2, 0));
            }



        }
    }
}
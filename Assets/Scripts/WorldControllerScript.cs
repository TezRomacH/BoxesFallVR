using System.Collections;
using UnityEngine;

namespace BoxFall
{
    public class WorldControllerScript : MonoBehaviour
    {
        /// <summary>
        /// The prefab to be cloned
        /// </summary>
        public GameObject cubePrefab = null;
        /// <summary>
        /// The pointer to the created cube
        /// </summary>
        public VR_UIpointer UIpointer = null;

        private System.Random random = null;

        private void Start()
        {
            // Seting up data in the model
            Data.Instance.Set(Constants.TimeForNextCube, 3.0f);
            Data.Instance.Set(Constants.CountBoxes, 0);
            Data.Instance.Set(Constants.MaxCountBoxes, 30);

            // binding change 'Constants.CountBoxes' field in the model with Action 'CheckCountBoxes()'
            Data.Instance.BindChangeField(Constants.CountBoxes, CheckCountBoxes);

            random = new System.Random();
            StartCoroutine(YieldBoxes());
        }

        private IEnumerator YieldBoxes()
        {
            while (Data.Instance.GetInt(Constants.CountBoxes) < Data.Instance.GetInt(Constants.MaxCountBoxes))
            {
                // cloning our prefab
                var newCube = Instantiate(cubePrefab, new Vector3(random.Next(-5, 4), 8, random.Next(-4, 5)), Quaternion.identity);
                newCube.GetComponent<Renderer>().material.color = Random.ColorHSV();
                UIpointer.target = newCube;
                Data.Instance.Increase(Constants.CountBoxes, (int)1);

                yield return new WaitForSeconds(Data.Instance.Get<float>(Constants.TimeForNextCube));
            }
        }

        private void CheckCountBoxes()
        {
            if (Data.Instance.GetInt(Constants.CountBoxes) == Data.Instance.GetInt(Constants.MaxCountBoxes))
            {
                // you lose
            }
        }

    }
}

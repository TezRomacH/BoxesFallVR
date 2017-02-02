using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

        public Text label = null;
        public Button startButton = null;

        private System.Random random = null;

        private string formatCountCubes = "Cubes {0}/{1}";

        private void Awake()
        {
            UIpointer.enabled = false;
        }

        private void Start()
        {
            // Seting up data in the model
            Data.Instance.Set(Constants.TimeForNextCube, 2.1f);
            Data.Instance.Set(Constants.CountBoxes, 0);
            Data.Instance.Set(Constants.MaxCountBoxes, 15);

            // binding change 'Constants.CountBoxes' field in the model with Action 'CheckCountBoxes()'
            Data.Instance.BindChangeField(Constants.CountBoxes, CheckCountBoxes);

            random = new System.Random();
        }

        private IEnumerator YieldBoxes()
        {
            while (Data.Instance.GetInt(Constants.CountBoxes) < Data.Instance.GetInt(Constants.MaxCountBoxes))
            {
                // cloning our prefab
                var newCube = Instantiate(cubePrefab, new Vector3(random.Next(-5, 4), 8, random.Next(-4, 5)), Quaternion.identity);
                newCube.GetComponent<Renderer>().material.color = Random.ColorHSV();
                newCube.tag = "Cloned cube";
                UIpointer.target = newCube;
                Data.Instance.Increase(Constants.CountBoxes, (int)1);

                yield return new WaitForSeconds(Data.Instance.Get<float>(Constants.TimeForNextCube));
            }
        }

        private void CheckCountBoxes()
        {
            var count = Data.Instance.GetInt(Constants.CountBoxes);
            var maxCount = Data.Instance.GetInt(Constants.MaxCountBoxes);

            if (count < maxCount)
                label.text = string.Format(formatCountCubes, count, maxCount);
            else
            {
                StopCoroutine(YieldBoxes());
                startButton.interactable = true;
                label.text = "Game over!";
            }
        }

        public void OnStartButton()
        {
            DestroyObjectsByTag("Cloned cube");
            Data.Instance.Set(Constants.CountBoxes, 0);
            startButton.interactable = false;
            UIpointer.enabled = true;
            StartCoroutine(YieldBoxes());
        }


        private void DestroyObjectsByTag(string _tag)
        {
            var gameObjects = GameObject.FindGameObjectsWithTag(_tag);

            foreach (GameObject item in gameObjects)
            {
                Destroy(item);
            }

        }
    }
}

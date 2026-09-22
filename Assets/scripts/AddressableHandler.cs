using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableHandler : MonoBehaviour
{
    [SerializeField] private GameObject defaultFace;
    [SerializeField] private GameObject[] faceModelReferences;
    [SerializeField] private AssetReferenceGameObject[] modelReferences;
    [SerializeField] private TextMeshProUGUI progressText;
	public static int downloaded;

    public static AddressableHandler Instance;
    public Button[] picturebutton, toglebutton, upicturebutton, uptogglebutton;
    public GameObject[] downloadbutton, fillimage,fillimageparent;

    public GameObject[] udownloadbutton, ufillimage, ufillimageparent;

    private const string ModelKeyPrefix = "ModelKey_";
    private int selectedModelIndex = -1;
    public GameObject[] currentModels;
    public bool[] isModelDownloaded;
    public static List<string> addressablestore = new List<string>();
    private void Start()
	{
        Instance = this;
		InitializeModels();
		LoadCurrentModels();
	}

    private void InitializeModels()
    {
        currentModels = new GameObject[modelReferences.Length];
        isModelDownloaded = new bool[modelReferences.Length];
    }

    private void LoadCurrentModels()
    {
        Debug.Log("modelreference " + modelReferences.Length);
        for (int i = 0; i < modelReferences.Length; i++)
        {
            int modelIndex = i; // Store the index in a separate variable for the closure
            string modelAddress = PlayerPrefs.GetString(ModelKeyPrefix + modelIndex);
            addressablestore.Add(modelAddress);
            if (!string.IsNullOrEmpty(modelAddress))
            {
                AsyncOperationHandle<GameObject> handle = modelReferences[modelIndex].LoadAssetAsync<GameObject>();
                handle.Completed += op => OnModelLoaded(op, modelIndex);
            }
        }
    }


    public void DownloadModel(int modelIndex)
    {
        DownloadModel(modelIndex, false);
    }

    public void DownloadModelForUpdate(int modelIndex)
    {
        DownloadModel(modelIndex, true);
    }



    public void DownloadModel(int modelIndex, bool forUpdate)
    {
        int modelCount = modelReferences != null ? modelReferences.Length : 0;
        if (modelIndex < 0 || modelIndex >= modelCount)
        {
            Debug.LogError("Invalid model index: " + modelIndex + ". Model References count: " + modelCount);
            return;
        }

        EnsureModelStateCapacity(modelCount);
        SelectFaceModel(modelIndex);

        if (!isModelDownloaded[modelIndex])
        {
            if (modelIndex == 3)
            {
                StartCoroutine(DownloadModelWithProgress(modelReferences[modelIndex], modelIndex, forUpdate));
                StartCoroutine(DownloadModelWithProgressspartan(modelReferences[8], 8));

            }

            else
            {

                StartCoroutine(DownloadModelWithProgress(modelReferences[modelIndex], modelIndex, forUpdate));
            }

        }
        else
        {

            if (modelIndex > 8)
            {

                picturebutton[modelIndex-1].interactable = true;

                toglebutton[modelIndex-1].interactable = true;
                upicturebutton[modelIndex - 1].interactable = true;
                uptogglebutton[modelIndex - 1].interactable = true;


                udownloadbutton[modelIndex - 1].SetActive(false);
                ufillimageparent[modelIndex - 1].SetActive(false);
                downloadbutton[modelIndex - 1].SetActive(false);
                fillimageparent[modelIndex - 1].SetActive(false);
            }
            else
            {

                picturebutton[modelIndex].interactable = true;

                toglebutton[modelIndex].interactable = true;
                upicturebutton[modelIndex].interactable = true;
                uptogglebutton[modelIndex].interactable = true;


                udownloadbutton[modelIndex].SetActive(false);
                ufillimageparent[modelIndex].SetActive(false);
                downloadbutton[modelIndex].SetActive(false);
                fillimageparent[modelIndex].SetActive(false);

            }
            Debug.Log("Using current model");
          //  InstantiateModel(currentModels[modelIndex], modelIndex);
        }
    }

    private void EnsureModelStateCapacity(int modelCount)
    {
        if (currentModels == null || currentModels.Length != modelCount)
        {
            System.Array.Resize(ref currentModels, modelCount);
        }

        if (isModelDownloaded == null || isModelDownloaded.Length != modelCount)
        {
            System.Array.Resize(ref isModelDownloaded, modelCount);
        }
    }

    public void SelectFaceModelForTheme(string theme)
    {
        switch (theme)
        {
            case "medaldisney":
                SelectFaceModel(0);
                break;
            case "clover":
                SelectFaceModel(1);
                break;
            case "winter":
                SelectFaceModel(2);
                break;
            case "fire":
                SelectFaceModel(3);
                break;
            case "patrik":
                SelectFaceModel(4);
                break;
            case "usa":
                SelectFaceModel(5);
                break;
            case "halloween":
                SelectFaceModel(6);
                break;
            case "thanksgiving":
                SelectFaceModel(7);
                break;
            case "christmas":
                SelectFaceModel(9);
                break;
            case "valentine":
                SelectFaceModel(10);
                break;
            case "balloon":
                SelectFaceModel(11);
                break;
            default:
                selectedModelIndex = -1;
                break;
        }
    }

    public GameObject GetSelectedFacePrefab()
    {
        if (faceModelReferences != null &&
            selectedModelIndex >= 0 &&
            selectedModelIndex < faceModelReferences.Length &&
            faceModelReferences[selectedModelIndex] != null)
        {
            return faceModelReferences[selectedModelIndex];
        }

        return defaultFace;
    }

    public GameObject GetDefaultFacePrefab()
    {
        return defaultFace;
    }

    public void SelectDefaultFace()
    {
        selectedModelIndex = -1;
    }

    private void SelectFaceModel(int modelIndex)
    {
        if (faceModelReferences != null && modelIndex >= 0 && modelIndex < faceModelReferences.Length)
        {
            selectedModelIndex = modelIndex;
            return;
        }

        selectedModelIndex = -1;
    }



    private IEnumerator DownloadModelWithProgress(AssetReferenceGameObject modelReference, int modelIndex, bool forUpdate)
    {
        AsyncOperationHandle<GameObject> downloadHandle = modelReference.LoadAssetAsync<GameObject>();
		while (!downloadHandle.IsDone)
        {
            float downloadPercentage = downloadHandle.PercentComplete * 100f;
            if (modelIndex > 8)
            {
                fillimage[modelIndex-1].GetComponent<Image>().fillAmount = downloadPercentage / 100;
                ufillimage[modelIndex-1].GetComponent<Image>().fillAmount = downloadPercentage / 100;
            }
            else
            {
                fillimage[modelIndex].GetComponent<Image>().fillAmount = downloadPercentage / 100;
                ufillimage[modelIndex].GetComponent<Image>().fillAmount = downloadPercentage / 100;
            }
            //  progressText.text = "Downloading: " + downloadPercentage.ToString("F2") + "%";
            yield return null;
        }
		if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            downloaded = 1;
            Debug.Log("Downloading completed");
            OnModelLoaded(downloadHandle, modelIndex);
            if (modelIndex != 8)
            {
                int buttonIndex = modelIndex > 8 ? modelIndex - 1 : modelIndex;

                if (forUpdate)
                    upicturebutton[buttonIndex].onClick.Invoke();
                else
                    picturebutton[buttonIndex].onClick.Invoke();
            }
         //   InstantiateModel(currentModels[modelIndex], modelIndex);
          //  progressText.text = "Download Complete!";
        }
        else
        {
            Debug.LogError("Failed to download model: " + downloadHandle.OperationException);
        }
    }
    private IEnumerator DownloadModelWithProgressspartan(AssetReferenceGameObject modelReference, int modelIndex)
    {
        AsyncOperationHandle<GameObject> downloadHandle = modelReference.LoadAssetAsync<GameObject>();
        while (!downloadHandle.IsDone)
        {
            float downloadPercentage = downloadHandle.PercentComplete * 100f;
            
            //  progressText.text = "Downloading: " + downloadPercentage.ToString("F2") + "%";
            yield return null;
        }
        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("ye download huwa hai");
            downloaded = 1;
            Debug.Log("Downloading completed");
            OnModelLoaded(downloadHandle, modelIndex);
            //   InstantiateModel(currentModels[modelIndex], modelIndex);
            //  progressText.text = "Download Complete!";
        }
        else
        {
            Debug.LogError("Failed to download model: " + downloadHandle.OperationException);
        }
    }



    public void reassignplayerpref()
    {
        for (int i = 0; i < modelReferences.Length; i++)
        {
            PlayerPrefs.SetString(ModelKeyPrefix + i, addressablestore[i]);


        }
    }


        private void OnModelLoaded(AsyncOperationHandle<GameObject> handle, int modelIndex)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (modelIndex >= 0 && modelIndex < currentModels.Length)
            {
                if (modelIndex != 8)
                {
                    if (modelIndex > 8)
                    {
                        picturebutton[modelIndex-1].interactable = true;

                        toglebutton[modelIndex - 1].interactable = true;
                        upicturebutton[modelIndex - 1].interactable = true;
                        uptogglebutton[modelIndex - 1].interactable = true;
                        downloadbutton[modelIndex - 1].SetActive(false);
                        udownloadbutton[modelIndex - 1].SetActive(false);
                        ufillimageparent[modelIndex - 1].SetActive(false);
                        fillimageparent[modelIndex - 1].SetActive(false);
                    }
                    else
                    {
                        picturebutton[modelIndex].interactable = true;

                        toglebutton[modelIndex].interactable = true;
                        upicturebutton[modelIndex].interactable = true;
                        uptogglebutton[modelIndex].interactable = true;
                        downloadbutton[modelIndex].SetActive(false);
                        udownloadbutton[modelIndex].SetActive(false);
                        ufillimageparent[modelIndex].SetActive(false);
                        fillimageparent[modelIndex].SetActive(false);

                    }
                }
                    currentModels[modelIndex] = handle.Result;

                
                isModelDownloaded[modelIndex] = true;
                Debug.Log("Downloaded model: " + modelIndex+" modellength: "+currentModels.Length+" list:"+addressablestore.Count);
                    string modelAddress = modelReferences[modelIndex].RuntimeKey.ToString();
                    PlayerPrefs.SetString(ModelKeyPrefix + modelIndex, modelAddress);
               
                addressablestore[modelIndex] = modelAddress;
            }
            else
            {
                Debug.LogError("Invalid model index: " + modelIndex);
            }
        }
        else
        {
            Debug.LogError("Failed to load current model: " + handle.OperationException);
        }
    }
    private void InstantiateModel(GameObject modelToInstantiate, int modelIndex)
	{
		// Destroy previously instantiated model, if any
		if (currentModels[modelIndex] != null)
		{
			//Destroy(currentModels[modelIndex]);
		}

		currentModels[modelIndex] = Instantiate(modelToInstantiate, transform.position, Quaternion.identity);
		// Further setup or modifications of the instantiated model can be done here
	}
}

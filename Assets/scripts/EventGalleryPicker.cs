using System.Collections.Generic;
using ImageCropperNamespace;
using UnityEngine;
using UnityEngine.UI;

public class EventGalleryPicker : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject sourcePopup;

    [Header("Image Row")]
    [SerializeField] private RectTransform imageRow;
    [SerializeField] private Button addImageButton;
    [SerializeField] private RawImage thumbnailPrefab;

    [Header("Settings")]
    [SerializeField] private int maximumImages = 20;
    [SerializeField] private int maximumImageSize = 2048;
    [SerializeField] private bool ovalCrop;

    private readonly List<Texture2D> selectedImages = new List<Texture2D>();
    private readonly List<RawImage> thumbnails = new List<RawImage>();

    public List<Texture2D> SelectedImages => selectedImages;

    public void OpenPopup()
    {
        if (selectedImages.Count < maximumImages)
            sourcePopup.SetActive(true);
    }

    public void ClosePopup()
    {
        sourcePopup.SetActive(false);
    }

    public void PickFromGallery()
    {
        ClosePopup();

        if (NativeGallery.IsMediaPickerBusy())
            return;

        NativeGallery.GetImageFromGallery(path =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            Texture2D image = NativeGallery.LoadImageAtPath(
                path,
                maximumImageSize,
                false);

            if (image != null)
                OpenCropper(image);

        }, "Select an image", "image/*");
    }

    public void CaptureFromCamera()
    {
        ClosePopup();

        if (NativeCamera.IsCameraBusy())
            return;

        NativeCamera.TakePicture(path =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            Texture2D image = NativeCamera.LoadImageAtPath(
                path,
                maximumImageSize,
                false);

            if (image != null)
                OpenCropper(image);

        }, maximumImageSize, true, NativeCamera.PreferredCamera.Default);
    }

    private void OpenCropper(Texture2D sourceImage)
    {
        ImageCropper.Instance.Show(
            sourceImage,
            (result, originalImage, croppedImage) =>
            {
                if (result && croppedImage != null)
                    AddImage(croppedImage);

                if (originalImage != null && originalImage != croppedImage)
                    Destroy(originalImage);
            },
            new ImageCropper.Settings
            {
                markTextureNonReadable = false,
                ovalSelection = ovalCrop,
                autoZoomEnabled = true,
                imageBackground = Color.clear,
                selectionMinAspectRatio = 1f,
                selectionMaxAspectRatio = 1f
            });
    }

    private void AddImage(Texture2D image)
    {
        selectedImages.Add(image);

        RawImage thumbnail = Instantiate(thumbnailPrefab, imageRow);
        thumbnail.texture = image;

        // Keep the add button after all selected images.
        thumbnail.transform.SetSiblingIndex(
            addImageButton.transform.GetSiblingIndex());

        thumbnails.Add(thumbnail);
        addImageButton.interactable = selectedImages.Count < maximumImages;
    }

    public void ClearImages()
    {
        foreach (RawImage thumbnail in thumbnails)
            Destroy(thumbnail.gameObject);

        foreach (Texture2D image in selectedImages)
            Destroy(image);

        thumbnails.Clear();
        selectedImages.Clear();
        addImageButton.interactable = true;
    }
}
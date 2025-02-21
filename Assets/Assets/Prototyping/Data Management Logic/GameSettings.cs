using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;

public class GameSettings : MonoBehaviour
{
    [Header("Screen Resolution")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle FullscreenBool;

    [Header("V-Sync")]
    public Toggle vSyncToggle;

    [Header("Framerate")]
    public TMP_Dropdown framerateDropdown;

    [Header("Graphics Quality")]
    public TMP_Dropdown graphicsQualityDropdown;

    [Header("Shadows")]
    public TMP_Dropdown shadowDistanceDropdown;
    private int curShadowindex;

    [Header("Textures")]
    public TMP_Dropdown textureDropdown;
    private int curTexIndex;

    [Header("FSR (FidelityFX Super Resolution)")]
    [SerializeField] private UniversalRenderPipelineAsset pipelineAsset;
    public GameObject FSRSharpness;
    public Slider FSRSharpnessSlider;
    public Toggle fsrToggle;

    private Resolution[] resolutions;

    private void Awake()
    {
        // Set default framerate to 60 FPS
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        // Populate resolution dropdown
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set default values for V-Sync toggle
        vSyncToggle.isOn = QualitySettings.vSyncCount != 0;

        // Populate dropdowns
        PopulateDropdown(textureDropdown, true);
        PopulateDropdown(graphicsQualityDropdown);

        // Set default values for framerate dropdown
        SetFramerateDropdownDefault();
        framerateDropdown.RefreshShownValue();

        // Set default values for graphics quality dropdown
        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();
        DisableOptions();
        graphicsQualityDropdown.RefreshShownValue();

        // Set default values for texture dropdown
        //textureDropdown.value = QualitySettings.globalTextureMipmapLimit;
        //textureDropdown.RefreshShownValue();

        curTexIndex = GetTextureDropdownIndex();
        curShadowindex = GetShadowDistanceDropdownIndex();

        // Set default values for shadow distance dropdown
        shadowDistanceDropdown.value = GetShadowDistanceDropdownIndex();
        shadowDistanceDropdown.RefreshShownValue();

        // Set default values for FSR toggle
        pipelineAsset = (UniversalRenderPipelineAsset)QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());
        FSRSharpnessSlider.value = pipelineAsset.fsrSharpness;

        if (pipelineAsset.upscalingFilter != UpscalingFilterSelection.FSR)
        {
            fsrToggle.isOn = false;
            FSRSharpness.SetActive(false);
        }
        else
        {
            fsrToggle.isOn = true;
            FSRSharpness.SetActive(true);
        }

        FullscreenBool.isOn = Screen.fullScreen;
    }

    private void SetFramerateDropdownDefault()
    {
        // Populate framerate dropdown
        framerateDropdown.ClearOptions();
        framerateDropdown.options.Add(new TMP_Dropdown.OptionData("30 FPS"));
        framerateDropdown.options.Add(new TMP_Dropdown.OptionData("60 FPS"));
        framerateDropdown.options.Add(new TMP_Dropdown.OptionData("Unlocked"));

        // Set default value to 60 FPS
        framerateDropdown.value = 1; // Index 1 corresponds to "60 FPS"
        framerateDropdown.RefreshShownValue();
    }

    private void DisableOptions()
    {
        if (QualitySettings.GetQualityLevel() != 3)
        {
            shadowDistanceDropdown.gameObject.SetActive(false);
            textureDropdown.gameObject.SetActive(false);
        }
        else
        {
            shadowDistanceDropdown.gameObject.SetActive(true);
            textureDropdown.gameObject.SetActive(true);
        }
    }

    private int GetShadowDistanceDropdownIndex()
    {
        float shadowDistance = pipelineAsset.shadowDistance;

        if (shadowDistance == 500f)
        {
            return 3; // High
        }
        else if (shadowDistance == 200f)
        {
            return 2; // Medium
        }
        else if (shadowDistance == 50f)
        {
            return 1; // Low
        }
        else
        {
            return 0; // Off
        }
    }

    private int GetTextureDropdownIndex()
    {
        if (QualitySettings.globalTextureMipmapLimit == 3)
        {
            return 0; // Low
        }
        else if (QualitySettings.globalTextureMipmapLimit == 1)
        {
            return 1; // Medium
        }
        else if (QualitySettings.globalTextureMipmapLimit == 0)
        {
            return 2; // High
        }
        else
        {
            return QualitySettings.globalTextureMipmapLimit; // Default
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void SetFramerate()
    {
        string selectedLabel = framerateDropdown.options[framerateDropdown.value].text;

        switch (selectedLabel)
        {
            case "Unlocked":
                Application.targetFrameRate = -1;
                QualitySettings.vSyncCount = 0;
                vSyncToggle.isOn = false;
                break;
            case "30 FPS":
                Application.targetFrameRate = 30;
                break;
            case "60 FPS":
                Application.targetFrameRate = 60;
                break;
            default:
                Application.targetFrameRate = 60;
                break;
        }
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        pipelineAsset = (UniversalRenderPipelineAsset)QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());

        DisableOptions();

        //update values for Custom graphics settings
        if (graphicsQualityDropdown.value == 3)
        {
            //get current texture and shadow distance state and update the appropriate dropdowns

            textureDropdown.value = curTexIndex;
            textureDropdown.RefreshShownValue();

            shadowDistanceDropdown.value = curShadowindex;
            shadowDistanceDropdown.RefreshShownValue();

            return;
        }

        curTexIndex = GetTextureDropdownIndex();
        curShadowindex = GetShadowDistanceDropdownIndex();
    }

    private void PopulateDropdown(TMP_Dropdown dropdown, bool excludeCustom = false)
    {
        dropdown.ClearOptions();
        string[] names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++)
        {
            if (excludeCustom && names[i] == "Custom")
            {
                continue; // Skip the "Custom" quality name
            }

            dropdown.options.Add(new TMP_Dropdown.OptionData(names[i]));
        }

        dropdown.RefreshShownValue(); // Refresh the dropdown to reflect new options
    }

    public void SetTextureQuality()
    {
        string selectedLabel = textureDropdown.options[textureDropdown.value].text;

        switch (selectedLabel)
        {
            case "High":
                QualitySettings.globalTextureMipmapLimit = 0;
                break;
            case "Medium":
                QualitySettings.globalTextureMipmapLimit = 1;
                break;
            case "Low":
                QualitySettings.globalTextureMipmapLimit = 3;
                break;
            default:
                break;
        }
    }

    //FSR
    public void ToggleFSR(bool FSROn)
    {
        if (!FSROn)
        {
            pipelineAsset.upscalingFilter = UpscalingFilterSelection.Point;
            pipelineAsset.renderScale = 1f;
            FSRSharpness.SetActive(false);
        }
        else
        {
            pipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
            pipelineAsset.renderScale = 0.5f;
            FSRSharpness.SetActive(true);
        }
    }

    public void SetFSRSharpness(float fsrSharpness)
    {
        pipelineAsset.fsrSharpness = FSRSharpnessSlider.value;
    }

    public void SetShadowDistance()
    {
        string selectedLabel = shadowDistanceDropdown.options[shadowDistanceDropdown.value].text;

        switch (selectedLabel)
        {
            case "High":
                pipelineAsset.shadowDistance = 500f;
                break;
            case "Medium":
                pipelineAsset.shadowDistance = 200;
                break;
            case "Low":
                pipelineAsset.shadowDistance = 50;
                break;
            case "Off":
                pipelineAsset.shadowDistance = 0f;
                break;
            default:
                break;
        }
    }

    public void LoadLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene in the build settings
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

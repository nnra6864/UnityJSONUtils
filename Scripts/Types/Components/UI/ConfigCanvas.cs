using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NnUtils.Scripts;
using UnityEngine;

namespace UnityJSONUtils.Scripts.Types.Components.UI
{
    /// This class is used as a bridge between <see cref="Canvas"/> and JSON <br/>
    /// Make sure to assign null in the Reset function and default value in a function called after loading data if the value is still null <br/>
    /// This approach prevents data stacking in case not all data is defined in the config
    [Serializable]
    public class ConfigCanvas : ConfigComponent
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty] public RenderMode RenderMode;
        [JsonProperty] public bool PixelPerfect;
        [JsonProperty] public int SortOrder;
        [JsonProperty] public int TargetDisplay;
        [JsonProperty] public string CameraName;

        public ConfigCanvas()
        {
            RenderMode    = RenderMode.ScreenSpaceOverlay;
            PixelPerfect  = false;
            SortOrder     = 0;
            TargetDisplay = 0;
            CameraName    = "";
        }

        public ConfigCanvas(Canvas canvas)
        {
            RenderMode    = canvas.renderMode;
            PixelPerfect  = canvas.pixelPerfect;
            SortOrder     = canvas.sortingOrder;
            TargetDisplay = canvas.targetDisplay;
            CameraName    = canvas.worldCamera.gameObject.name;
        }

        public ConfigCanvas(
            RenderMode mode = RenderMode.ScreenSpaceOverlay,
            bool pixelPerfect = false,
            int sortOrder = 0,
            int targetDisplay = 0,
            string cameraName = ""
        )
        {
            RenderMode    = mode;
            PixelPerfect  = pixelPerfect;
            SortOrder     = sortOrder;
            TargetDisplay = targetDisplay;
            CameraName    = cameraName;
        }

        public static implicit operator ConfigCanvas(Canvas canvas) => new(canvas);

        /// Updates an existing <see cref="Canvas"/> component with config values
        public Canvas UpdateCanvas(Canvas canvas)
        {
            canvas.renderMode    = RenderMode;
            canvas.pixelPerfect  = PixelPerfect;
            canvas.sortingOrder  = SortOrder;
            canvas.targetDisplay = TargetDisplay;
            canvas.worldCamera   = GameObject.Find(CameraName)?.GetComponent<Camera>();
            return canvas;
        }

        public override void AddComponent(GameObject go) => UpdateCanvas(go.GetOrAddComponent<Canvas>());
    }
}
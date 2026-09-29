//SPDX-License-Identifier: Unlicense

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Tiger.Audio
{
    [RequireComponent(typeof(Slider))]
    [DisallowMultipleComponent]
    public class MixerGroupSlider : MonoBehaviour
    {
        [Tooltip("Audio Mixer Group cần điều khiển")]
        public AudioMixerGroup group;

        [Tooltip("Tên tham số đã Expose trong Audio Mixer (để trống sẽ tự lấy tên group)")]
        [SerializeField]
        private string exposedParamName;

        [Tooltip("The minimum decibel value of the slider")]
        public float minDB = -80f;

        [Tooltip("The maximum decibel value of the slider")]
        public float maxDB = 3f;

        [Tooltip(
            "Additionally scale the slider along a nonlinear curve to give more precision in the middle to top range."
        )]
        [Range(0.1f, 1f)]
        public float linearity = 0.5f;

        [SerializeField]
        [HideInInspector]
        private Slider slider;

        private string ParamName =>
            string.IsNullOrEmpty(exposedParamName) ? group.name : exposedParamName;
        private string PrefsKey => $"MixerGroup/{ParamName}";

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void Start()
        {
            // Lấy giá trị Decibel gốc (từ Prefs hoặc trực tiếp từ Mixer)
            float decibels = PlayerPrefs.HasKey(PrefsKey)
                ? PlayerPrefs.GetFloat(PrefsKey)
                : ReadInitialDecibelsFromMixer();

            // Đồng bộ thanh trượt
            slider.SetValueWithoutNotify(ValueForDecibels(decibels));

            // Ép Mixer nhận giá trị đã lưu ngay khi khởi động
            ApplyDecibels(decibels);
        }

        // Đọc giá trị decibels ban đầu trực tiếp từ tham số của Audio Mixer
        private float ReadInitialDecibelsFromMixer()
        {
            if (group.audioMixer.GetFloat(ParamName, out var decibels))
            {
                return decibels;
            }
            return 0f; // Mặc định nếu chưa lấy được
        }

        // Chuyển đổi giá trị Decibels sang giá trị tỉ lệ (0 đến 1) cho Slider theo thang phi tuyến
        private float ValueForDecibels(float decibels)
        {
            var normalized = math.saturate(math.remap(minDB, maxDB, 0, 1, decibels));
            var nonlinear = math.pow(normalized, 1f / linearity);
            var exponential = math.pow(10f, nonlinear);
            var remapped = math.remap(1, 10, 0, 1, exponential);
            return math.saturate(remapped);
        }

        // Chuyển đổi giá trị Slider (0 đến 1) sang giá trị Decibels tương ứng
        private float DecibelsForValue(float value)
        {
            var remapped = math.remap(0, 1, 1, 10, value);
            var logarithmic = math.saturate(math.log10(remapped));
            var nonlinear = math.pow(logarithmic, linearity);
            return math.remap(0f, 1f, minDB, maxDB, nonlinear);
        }

        // Xử lý sự kiện khi kéo Slider để cập nhật âm lượng vào Mixer và lưu vào PlayerPrefs
        private void OnValueChanged(float sliderValue)
        {
            var decibels = DecibelsForValue(sliderValue);
            ApplyDecibels(decibels);
            PlayerPrefs.SetFloat(PrefsKey, decibels);
        }

        // Áp dụng trực tiếp giá trị decibels vào tham số của Audio Mixer
        private void ApplyDecibels(float decibels)
        {
            group.audioMixer.SetFloat(ParamName, decibels);
        }

        private void OnValidate()
        {
            if (!slider)
                slider = GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }
    }
}

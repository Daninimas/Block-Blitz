using UnityEngine;

namespace GameAssets.Scripts.Tools
{
    public static class Tools
    {
        /// <summary>
        /// Normaliza un valor (value) en valores [minNormalized, maxNormalized] pasandole el rango minRange y maxRange donde puede estar 
        /// </summary>
        public static float NormalizeValues(float minNormalized, float maxNormalized, float minRange, float maxRange,
            float value, bool clampValue = false)
        {
            float normalizedValue = (maxNormalized - minNormalized) * ((value - minRange) / (maxRange - minRange)) +
                                    minNormalized;
            if (clampValue)
                normalizedValue = Mathf.Clamp(normalizedValue, minNormalized, maxNormalized);

            return normalizedValue;
        }
        
        
        /// <summary>
        /// Returns the position in world coords of the top left corner
        /// </summary>
        public static Vector3 GetWorldTopLeft(this RectTransform rectTransform)
        {
            // Offset local desde pivot hasta top-left
            Vector2 localTopLeft = new Vector2(
                -rectTransform.rect.width * rectTransform.pivot.x,        // x
                rectTransform.rect.height * (1 - rectTransform.pivot.y)   // y
            );

            // Convertimos a world position considerando rotación, escala y posición
            return rectTransform.TransformPoint(localTopLeft);
        }
    }
}
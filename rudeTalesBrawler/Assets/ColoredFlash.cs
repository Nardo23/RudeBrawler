using System.Collections;

using UnityEngine;


    public class ColoredFlash : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [Tooltip("Material to switch to during the flash.")]
        [SerializeField] private Material flashMaterial;

        [Tooltip("Duration of the flash.")]
        [SerializeField] private float duration;
        public Color color;
        public bool useRenderArray = false;
        #endregion
        #region Private Fields

        // The SpriteRenderer that should flash.
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer[] spriteRendereArray;
        // The material that was in use, when the script started.
        private Material originalMaterial;

        // The currently running coroutine.
        private Coroutine flashRoutine;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            // Get the SpriteRenderer to be used,
            // alternatively you could set it from the inspector.
            //spriteRenderer = GetComponent<SpriteRenderer>();

            // Get the material that the SpriteRenderer uses, 
            // so we can switch back to it after the flash ended.
            originalMaterial = spriteRenderer.material;

            // Copy the flashMaterial material, this is needed, 
            // so it can be modified without any side effects.
            flashMaterial = new Material(flashMaterial);
        }

        #endregion

    
        public void Flash()
        {
            // If the flashRoutine is not null, then it is currently running.
            if (flashRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(flashRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutine(color));
        }

        private IEnumerator FlashRoutine(Color color)
        {
            if (useRenderArray)
            {
                foreach (SpriteRenderer rend in spriteRendereArray)
                {
                    rend.material = flashMaterial;
                    flashMaterial.color = color;
                
                }
            
                yield return new WaitForSeconds(duration);
                
                foreach (SpriteRenderer rend in spriteRendereArray)
                {
                    rend.material = originalMaterial;
                    flashMaterial.color = color;
                    flashRoutine = null;
                }

            }
            else
            {
            // Swap to the flashMaterial.
            spriteRenderer.material = flashMaterial;

            // Set the desired color for the flash.
            flashMaterial.color = color;

            // Pause the execution of this function for "duration" seconds.
            yield return new WaitForSeconds(duration);

            // After the pause, swap back to the original material.
            spriteRenderer.material = originalMaterial;

            // Set the flashRoutine to null, signaling that it's finished.
            flashRoutine = null;
            }
            
        }

        #endregion
    }

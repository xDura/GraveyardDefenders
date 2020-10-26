using UnityEngine;

namespace XD
{
    public enum BUTTON_SIZES
    {
        SMALL,
        MEDIUM,
        LARGE,
    }

    [System.AttributeUsageAttribute(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        private BUTTON_SIZES size;
        public BUTTON_SIZES Size
        {
            get => size;
            set { size = value; }
        }

        public int Height()
        {
            switch (Size)
            {
                case BUTTON_SIZES.SMALL: return 18;
                case BUTTON_SIZES.MEDIUM: return 30;
                case BUTTON_SIZES.LARGE: return 60;
            }
            return 20;
        }

        public ButtonAttribute(BUTTON_SIZES buttonSize = BUTTON_SIZES.MEDIUM) { Size = buttonSize; }
    }
}

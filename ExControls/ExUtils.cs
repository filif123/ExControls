using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Utils for ExControls
    /// </summary>
    public static class ExUtils
    {
        private static float GetBrightness(Color c)
        {
            return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f;
        }

        /// <summary>
        /// Get ForegroundColor of text accrording to BackColor. <br></br>
        /// Source: https://stackoverflow.com/questions/50540301/c-sharp-get-good-color-for-label (edited)
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public static Color GetContrastForeColor(Color back)
        {
            return GetBrightness(back) < 0.55 ? Color.White : Color.Black;
        }

        /// <summary>
        ///     Turns off visual styles from the Control
        /// </summary>
        /// <param name="control"></param>
        public static void TurnOffVisualStyles(Control control)
        {
            Win32.SetWindowTheme(control.Handle, "", "");
        }

        /// <summary>
        ///     Gets a control whics is actually focused.
        /// </summary>
        /// <returns></returns>
        public static Control GetFocusedControl()
        {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = Win32.GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }
    }
}

using System.Windows.Forms;

namespace ScintillaNET
{
    /// <summary>
    /// Specifies the border style for a control.
    /// </summary>
    /// <remarks>
    /// Use the members of this enumeration to set the border style for controls that have a changeable border.
    /// </remarks>
    public enum BorderStyle
    {
        /// <summary>
        /// No border.
        /// </summary>
        None,

        /// <summary>
        /// A single-line border.
        /// </summary>
        FixedSingle,

        /// <summary>
        /// A three-dimensional border (classic).
        /// </summary>
        Fixed3D,

        /// <summary>
        /// A three-dimensional border. When visual styles are enabled, the border is displayed like that of a <see cref="TextBox"/>.
        /// When visual styles are not enabled, this is equivalent to <see cref="Fixed3D"/>.
        /// </summary>
        Fixed3DVisualStyles
    }
}

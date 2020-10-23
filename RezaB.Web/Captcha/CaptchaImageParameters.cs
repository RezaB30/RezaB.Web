using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Captcha
{
    /// <summary>
    /// Defines parameters for captcha image generation.
    /// All properties are optional with default values.
    /// </summary>
    public class CaptchaImageParameters
    {
        /// <summary>
        /// A string of characters to choose from. (default is all alphabet characters)
        /// </summary>
        public string CharacterPallete
        {
            get
            {
                return _chars;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _chars = value;
            }
        }
        /// <summary>
        /// Number of characters to display in the image.
        /// </summary>
        public int? CharacterCount
        {
            get
            {
                return _length;
            }
            set
            {
                if (value.HasValue)
                    _length = value.Value;
            }
        }
        /// <summary>
        /// Size of the image in pixels (default is 200 x 100).
        /// </summary>
        public Size ImageDimentions
        {
            get
            {
                return _dimentions;
            }
            set
            {
                if (value != null)
                    _dimentions = value;
            }
        }
        /// <summary>
        /// Size of font in points (default is 24).
        /// </summary>
        public float? FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (value.HasValue)
                    _fontSize = value.Value;
            }
        }
        /// <summary>
        /// The opacity of the font (default is 200/255).
        /// </summary>
        public ushort? FontAlpha
        {
            get
            {
                return _fontAlpha;
            }
            set
            {
                if (value.HasValue)
                    _fontAlpha = value.Value;
            }
        }
        /// <summary>
        /// The amount of noise in image (default is 0.25).
        /// </summary>
        public float? NoisePercentage
        {
            get
            {
                return _noisePercentage;
            }
            set
            {
                if (value.HasValue)
                    _noisePercentage = value.Value;
            }
        }
        /// <summary>
        /// Font families to use.
        /// </summary>
        public FontFamily[] Fonts
        {
            get
            {
                return _fontFaces;
            }
            set
            {
                if (value != null && value.Any())
                    _fontFaces = value.ToArray();
            }
        }

        private string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private int _length = 6;
        private Size _dimentions = new Size(200, 100);
        private float _fontSize = 24f;
        private ushort _fontAlpha = 200;
        private float _noisePercentage = 0.25f;
        private FontFamily[] _fontFaces = new FontFamily[]
        {
            new FontFamily("Arial"),
            new FontFamily("Calibri"),
            new FontFamily("Calisto MT"),
            new FontFamily("Cambria"),
            new FontFamily("Candara"),
            new FontFamily("Century"),
            new FontFamily("Gothic"),
            new FontFamily("Comic Sans MS"),
            new FontFamily("Consolas"),
            new FontFamily("Constantia"),
            new FontFamily("Corbel"),
            new FontFamily("Courier New"),
            new FontFamily("David"),
            new FontFamily("Euphemia"),
            new FontFamily("Georgia"),
            new FontFamily("Lucida Console"),
            new FontFamily("News Gothic MT"),
            new FontFamily("Segoe UI"),
            new FontFamily("Tahoma"),
            new FontFamily("Times New Roman"),
            new FontFamily("Trebuchet MS"),
            new FontFamily("Verdana"),
        };
    }
}

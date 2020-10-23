using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Captcha
{
    /// <summary>
    /// A class that represents an image key pair for generated captchas.
    /// </summary>
    public class CaptchaImage
    {
        /// <summary>
        /// Captcha key
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// Captcha image
        /// </summary>
        public Image Image { get; private set; }
        /// <summary>
        /// Creates a pair.
        /// </summary>
        /// <param name="key">Captcha key</param>
        /// <param name="image">Captcha image</param>
        public CaptchaImage(string key, Image image)
        {
            Key = key;
            Image = image;
        }
    }
}

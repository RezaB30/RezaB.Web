using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RezaB.Web.Helpers
{
    /// <summary>
    /// Contains usefull functions for interacting with captchas.
    /// </summary>
    public static class Captcha
    {
        private const string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int _length = 6;
        private const int _width = 250;
        private const int _height = 100;
        private const float _fontSize = 34f;
        private const int _fontAlpha = 250;
        private const float _noisePercentage = 0.15f;
        private const string _fontFaces = "Arial,Calibri,Calisto MT,Cambria,Candara,Century Gothic,Comic Sans MS,Consolas,Constantia,Corbel,Courier New,David,Euphemia,Georgia,Lucida Console,News Gothic MT,Segoe UI,Tahoma,Times New Roman,Trebuchet MS,Verdana";
        /// <summary>
        /// Generates a captcha Image
        /// </summary>
        /// <returns>Generated image object</returns>
        public static CaptchaImage Generate()
        {
            var rand = new Random();
            var text = new string(Enumerable.Repeat(_chars, _length).Select(s => s[rand.Next(s.Length)]).ToArray());
            var image = new Bitmap(_width, _height);
            var graph = Graphics.FromImage(image);

            graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graph.FillRectangle(Brushes.White, new Rectangle(0, 0, _width, _height));
            var colorList = new List<Color>();
            var fontFaces = _fontFaces.Split(',');
            var fonts = FontFamily.Families.Where(f => fontFaces.Contains(f.Name)).ToArray();

            for (int i = 0; i < _length; i++)
            {
                graph.ResetTransform();
                graph.TranslateTransform(_fontSize + _fontSize / 1.2f * i, image.Height / 2 - _fontSize / 2f);
                graph.RotateTransform(rand.Next(40) - 20f);
                colorList.Add(Color.FromArgb(_fontAlpha, rand.Next(200), rand.Next(200), rand.Next(200)));
                var brush = new SolidBrush(colorList.LastOrDefault());
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graph.DrawString(text[i].ToString(), new Font(fonts[rand.Next(fonts.Length)], _fontSize), brush, 0, 0, format);
            }
            for (int i = 0; i < image.Height * image.Width * _noisePercentage; i++)
            {
                graph.ResetTransform();
                var color = colorList[rand.Next(colorList.Count)];
                image.SetPixel(rand.Next(image.Width), rand.Next(image.Height), color);
            }

            return new CaptchaImage(text, image);
        }
        /// <summary>
        /// Checks if the captcha is matching
        /// </summary>
        /// <param name="key">The string key to compare</param>
        /// <param name="session">The session that contains captcha</param>
        /// <returns>Result of captcha match checking</returns>
        public static bool IsMatch(string key/*, HttpSessionStateBase session*/)
        {
            if (key.ToLower() == (string)HttpContext.Current.Session["captcha"])
            {
                return true;
            }
            HttpContext.Current.Session.Remove("captcha");
            return false;
        }
    }
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

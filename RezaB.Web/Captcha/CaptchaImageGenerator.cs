using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Captcha
{
    /// <summary>
    /// Contains usefull functions for interacting with captchas.
    /// </summary>
    public static class CaptchaImageGenerator
    {
        /// <summary>
        /// Generates a captcha Image
        /// </summary>
        /// <returns>Generated image object</returns>
        public static CaptchaImage Generate(CaptchaImageParameters parameters)
        {
            //initial set
            parameters = parameters ?? new CaptchaImageParameters();

            var rand = new Random();
            var text = new string(Enumerable.Repeat(parameters.CharacterPallete, parameters.CharacterCount.Value).Select(s => s[rand.Next(s.Length)]).ToArray());
            var image = new Bitmap(parameters.ImageDimentions.Width, parameters.ImageDimentions.Height);
            using (var graph = Graphics.FromImage(image))
            {
                graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                graph.FillRectangle(Brushes.White, new Rectangle(0, 0, parameters.ImageDimentions.Width, parameters.ImageDimentions.Height));
                var colorList = new List<Color>();

                for (int i = 0; i < parameters.CharacterCount.Value; i++)
                {
                    graph.ResetTransform();
                    graph.TranslateTransform(parameters.FontSize.Value + parameters.FontSize.Value / 1.2f * i, image.Height / 2 - parameters.FontSize.Value / 2f);
                    graph.RotateTransform(rand.Next(40) - 20f);
                    colorList.Add(Color.FromArgb(parameters.FontAlpha.Value, rand.Next(200), rand.Next(200), rand.Next(200)));
                    var brush = new SolidBrush(colorList.LastOrDefault());
                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    graph.DrawString(text[i].ToString(), new Font(parameters.Fonts[rand.Next(parameters.Fonts.Length)], parameters.FontSize.Value), brush, 0, 0, format);
                }
                for (int i = 0; i < image.Height * image.Width * parameters.NoisePercentage.Value; i++)
                {
                    graph.ResetTransform();
                    var color = colorList[rand.Next(colorList.Count)];
                    image.SetPixel(rand.Next(image.Width), rand.Next(image.Height), color);
                }
                graph.Save();
            }

            return new CaptchaImage(text, image);
        }
    }
}

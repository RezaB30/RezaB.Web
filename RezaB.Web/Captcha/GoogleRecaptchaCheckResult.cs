using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Captcha
{
    class GoogleRecaptchaCheckResult
    {
        public bool Success { get; set; }
        public List<string> ErrorCodes { get; set; }
    }
}

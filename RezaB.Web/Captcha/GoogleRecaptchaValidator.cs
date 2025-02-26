﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RezaB.Web.Captcha
{
    public static class GoogleRecaptchaValidator
    {
        public static GoogleRecaptchaResultType Check(string apiKey, string requestKey)
        {
            
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return GoogleRecaptchaResultType.NotWorking;
            }

            try
            {
                var client = new System.Net.WebClient();
                var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", apiKey, requestKey));
                var checkResult = JsonConvert.DeserializeObject<GoogleRecaptchaCheckResult>(googleReply);
                checkResult.ErrorCodes = checkResult.ErrorCodes ?? new List<string>();
                if (checkResult.ErrorCodes.Contains("invalid-input-secret"))
                {
                    return GoogleRecaptchaResultType.NotWorking;
                }
                return checkResult.Success ? GoogleRecaptchaResultType.Success : GoogleRecaptchaResultType.Fail;
            }
            catch
            {
                return GoogleRecaptchaResultType.NotWorking;
            }
        }
    }
}

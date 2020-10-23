using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RezaB.Web
{
    [Authorize]
    public class AuthorizedBaseController : BaseController {}
}

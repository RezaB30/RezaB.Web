using RezaB.Web.Helpers.HelperObjects;
using System.Linq;
using System.Web.Mvc;

namespace RezaB.Web.Helpers.Binders
{
    public class MonthOfYearBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            var result = bindingContext.ValueProvider.GetValue(key);
            if (result == null)
            {
                return null;
            }

            try
            {
                var parts = result.AttemptedValue.Trim().Split('-').Select(part=> int.Parse(part)).ToArray();
                if (parts.Count() != 2)
                    return null;
                var year = parts[0];
                var month = parts[1];
                if (year < 1753 || month > 12 || month < 0)
                    return null;

                return new MonthOfYear()
                {
                    Year = year,
                    Month = month
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
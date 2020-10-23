using RezaB.Data.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class LocalizedListEditorHelper
    {
        public static MvcHtmlString LocalizedListEditor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            return helper.LocalizedListEditor(expression, fullName);
        }

        public static MvcHtmlString LocalizedListEditor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, string id)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            object _enumType;
            object _resourceType;
            Type enumType;
            Type resourceType;
            if (metadata.AdditionalValues.TryGetValue("EnumType", out _enumType) && metadata.AdditionalValues.TryGetValue("EnumResourceType", out _resourceType))
            {
                enumType = _enumType as Type;
                resourceType = _resourceType as Type;
                var type = typeof(LocalizedList<,>).MakeGenericType(enumType, resourceType);
                var genericList = Activator.CreateInstance(type) as LocalizedList;

                var items = genericList.GenericList;
                var selectList = new SelectList(items, "ID", "Name", metadata.Model != null && Convert.ToInt32(metadata.Model) > 0 ? metadata.Model : null);

                object defaultText;
                if (!helper.ViewData.TryGetValue("DefaultText", out defaultText))
                    defaultText = Localization.Common.Select;
                return helper.Select(id, selectList, defaultText.ToString());
            }

            return new MvcHtmlString(metadata.Model.ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;

namespace ChangeableType.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ChangeableFor<TModel, TValue, TType>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Changeable<TType> changeable)
        {

            // helper to take our Changeable<T> property, and create lambdas to get 
            var controller = html.ViewContext.Controller;
            var actionName = controller.ValueProvider.GetValue("action").RawValue.ToString();
            var allMethods = controller.GetType().GetMethods();
            var methods = allMethods.Where(m => m.Name == actionName);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType().Equals(typeof(HttpPostAttribute)))
                    {
                        var a = attribute;
                    }
                }
            }
            var name = ExpressionHelper.GetExpressionText(expression);
            if (String.IsNullOrEmpty(name)) 
                throw new ArgumentNullException("name", "Name cannot be null");
            
            // get the metadata for our Changeable<T> property
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var type = metadata.ModelType;
            var containerType = metadata.ContainerType;

            // create a lambda expression to get the value of our Changeable<T>
            var arg = Expression.Parameter(containerType, "x");
            Expression expr = arg;
            expr = Expression.Property(expr, name);
            expr = Expression.Property(expr, "Value");
            var funcExpr = Expression.Lambda(expr, arg) as Expression<Func<TModel, TType>>;
            var valueModelMetadata = ModelMetadata.FromLambdaExpression(funcExpr, html.ViewData);

            // create a lambda expression to get whether our Changeable<T> has changed
            Expression exprChanged = arg;
            exprChanged = Expression.Property(exprChanged, name);
            exprChanged = Expression.Property(exprChanged, "Changed");
            var funcExprChanged = Expression.Lambda(exprChanged, arg) as Expression<Func<TModel, bool>>;

            // use a stringbuilder for our HTML markup
            // return label, checkbox, hidden input, EDITOR, and validation message field
            // NOTE:  this is over-simplified markup!
            // ALSO NOTE:  the EditorFor is passed the strongly typed model taken from our T.  Bonus!
            var htmlSb = new StringBuilder("\n");
            htmlSb.Append(LabelExtensions.Label(html, metadata.GetDisplayName()));
            htmlSb.Append("<br />\n");
            htmlSb.Append(InputExtensions.CheckBoxFor(html, funcExprChanged));
            htmlSb.Append(" Changed<br />\n");
            htmlSb.Append(InputExtensions.Hidden(html, name + ".OldValue", valueModelMetadata.Model) + "\n");
            htmlSb.Append(EditorExtensions.EditorFor(html, funcExpr, new KeyValuePair<string, object>("parentMetadata", metadata))); //new object[] { "parentMetadata", metadata })); 
            htmlSb.Append(ValidationExtensions.ValidationMessageFor(html, funcExpr));
            htmlSb.Append("<br />\n");

            return new MvcHtmlString(htmlSb.ToString());
        }
    }
}
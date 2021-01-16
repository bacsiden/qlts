using System.Collections.Generic;
using System.Web.Mvc;

namespace DK.Web
{
    public static class DKHelpers
    {
        public static MvcHtmlString BuildDrodownList(this HtmlHelper html, string name, IEnumerable<string> list, string selected, string optionText = "-:-", string className = "form-control form-control-sm")
        {
            var newList = new List<string>();
            if (optionText != null) newList.Add(optionText);
            newList.AddRange(list);

            var tagSelect = new TagBuilder("select");

            tagSelect.AddCssClass(className);
            tagSelect.Attributes.Add("name", name);
            tagSelect.Attributes.Add("id", name);
            foreach (var item in newList)
            {
                var tagOption = new TagBuilder("option");
                tagOption.SetInnerText(item);
                if (item != optionText)
                {
                    tagOption.Attributes.Add("value", item);
                }
                else
                    tagOption.Attributes.Add("value", null);
                if (!string.IsNullOrEmpty(item) && !string.IsNullOrEmpty(selected) && item.ToLower().Equals(selected.ToLower()))
                {
                    tagOption.Attributes.Add("selected", "selected");
                }
                tagSelect.InnerHtml += tagOption.ToString();
            }
            return new MvcHtmlString(tagSelect.ToString());
        }
    }
}
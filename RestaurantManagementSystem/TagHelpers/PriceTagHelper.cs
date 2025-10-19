using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RestaurantManagementSystem.TagHelpers
{
    [HtmlTargetElement("price", Attributes = "value")]
    public class PriceTagHelper : TagHelper
    {
        public decimal Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.Attributes.SetAttribute("class", "price");
            output.Content.SetContent($"${Value:0.00}");
        }
    }
}
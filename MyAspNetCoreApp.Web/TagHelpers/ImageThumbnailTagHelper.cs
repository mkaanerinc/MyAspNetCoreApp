using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyAspNetCoreApp.Web.TagHelpers
{
    [HtmlTargetElement("thumbnail")]
    public class ImageThumbnailTagHelper : TagHelper
    {
        public string ImageSrc { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //<img src="" />

            output.TagName = "img";

            string fileName = ImageSrc.Split(".")[0]; // noktadan sonrasını böl ve ilk indexi al.
            string fileExtensions = Path.GetExtension(ImageSrc); // nokta ile beraber uzantıyı aldık.

            output.Attributes.SetAttribute("src", $"{fileName}-100x100{fileExtensions}");
        }
    }
}

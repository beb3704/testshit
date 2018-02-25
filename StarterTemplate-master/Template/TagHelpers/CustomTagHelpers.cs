using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Template.Models;

namespace Template.TagHelpers
{
    [HtmlTargetElement(Attributes = "edit")]
    public class EditTagHelper : TagHelper
    {
        private readonly Context _context;
        private readonly IDistributedCache _cache;


        public EditTagHelper(Context context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public string Edit { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
        
            //var isInCache = await _cache.GetStringAsync(Edit);
            //if (!string.IsNullOrEmpty(isInCache))
            //{
            //    output.Content.SetHtmlContent(isInCache);
            //}
            //else
            //{
                var childContent = await output.GetChildContentAsync();
                var child = childContent.GetContent();
                var isInDatabase = await _context.EditFields.SingleOrDefaultAsync(m => m.UniqeKey == Edit);
                if (isInDatabase == null)
                {
                    await _context.EditFields.AddAsync(new EditField()
                    {
                        UniqeKey = Edit,
                        Text = child
                    });
                    await _context.SaveChangesAsync();
                    await _cache.SetStringAsync(Edit, child);
                }
                else
                {
                    await _cache.SetStringAsync(Edit, isInDatabase.Text);
                    output.Content.SetHtmlContent(isInDatabase.Text);
                }
            //}
        }
    }
}
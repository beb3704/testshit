using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Template.Models;

namespace Template.Pages.Admin
{
    public class EditsectionsModel : PageModel
    {
        private readonly Context _context;

        public List<EditField> EditFields;

        public EditsectionsModel(Context context)
        {
            _context = context;
        }

        public async void OnGetAsync()
        {
            EditFields = await _context.EditFields.AsNoTracking().ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> OnPostUpdateSection()
        {
            string text = "";
            int id = 0;
            
            {
                MemoryStream stream = new MemoryStream();
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string requestBody = reader.ReadToEnd();
                    if (requestBody.Length > 0)
                    {
                        var obj = JsonConvert.DeserializeObject<EditField>(requestBody);
                        if (obj != null)
                        {
                            text = obj.Text;
                            id = obj.Id;
                        }
                    }
                }
            }

            var fieldToEdit = await _context.EditFields.FindAsync(id);

            if (fieldToEdit != null)
            {
                fieldToEdit.Text = text;
                await _context.SaveChangesAsync();
            }

            return new JsonResult(fieldToEdit);
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Template.Models
{
    public class EditField
    {
        public int Id { get; set; }

        public string UniqeKey { get; set; }

        public string Text { get; set; }
    }
}

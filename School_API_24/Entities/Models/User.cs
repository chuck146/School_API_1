using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class User
    {
        [Column ("UserId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is a required field.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters.")]
        public string Position { get; set; }

        [ForeignKey(nameof(Organization))]
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}

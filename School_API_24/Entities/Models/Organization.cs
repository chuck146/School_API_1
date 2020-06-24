﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Organization
    {
        [Column("OrganizationId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Organization name is a required field.")]
        [MaxLength(60, ErrorMessage ="Maximum length for the name is 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Organization address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters.")]
        public string Address { get; set; }

        public string Country { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

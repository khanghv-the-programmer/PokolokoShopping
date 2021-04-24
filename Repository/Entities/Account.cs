using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Repository.Domain;

namespace Repository.Entities
{
    public class Account
    {
        [StringLength(20)]
        [Required]
        public string Username { get; set; }
        [StringLength(20)]
        [Required]
        public string Password { get; set; }
        [StringLength(50)]
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [StringLength(11)]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        public int? RoleId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
    }
}

﻿namespace Product.Identity.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }

        public List<string> Roles { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161838.ProductManagement.Repo.ViewModels.Member
{
    public class MemberViewModels
    {
        public int MemberId { get; set; }

        public string Email { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? Role { get; set; }
    }
    public class MemberLoginViewModels
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
    public class SearchMemberViewModels
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "email")]
        public string? email { get; set; }
        [FromQuery(Name = "company-name")]
        public string? CompanyName { get; set; }
        [FromQuery(Name = "city")]

        public string? City { get; set; } 
        [FromQuery(Name = "country")]
        public string? Country { get; set; }
    }
    public class MemberModelResponse
    {
        public int total { get; set; }
        public int currentPage { get; set; }
        public List<MemberViewModels> users { get; set; }
    } 
    public class MemberUpdateViewModels
    {
        public string Email { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? Role { get; set; }
    }
    public class MemberCreateViewModels
    {
        [Required(ErrorMessage = "MemberId is required.")]

        public int MemberId { get; set; }

        [Required(ErrorMessage = "Email is required.")]

        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "CompanyName is required.")]

        public string CompanyName { get; set; } = null!;
        [Required(ErrorMessage = "City is required.")]

        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Country is required.")]


        public string Country { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]


        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Role is required.")]

        public int? Role { get; set; }
    }
}

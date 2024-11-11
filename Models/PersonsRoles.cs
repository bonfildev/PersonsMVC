using Microsoft.AspNetCore.Mvc.Rendering;
using PersonsMVC.Tools;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PersonsMVC.Models
{
    public class PersonsRoles
    {
        // Display Attribute will appear in the Html.LabelFor
        [Display(Name = "User Role")]
        public int IDRole { get; set; }
        public string RoleName { get; set; }


    }
}

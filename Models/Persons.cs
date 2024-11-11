using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PersonsMVC.Models;

public partial class Persons
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }

    public string Email { get; set; } = null!;

}

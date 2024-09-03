using System;
using System.Collections.Generic;

namespace PersonsMVC.Models;

public partial class PersonsTask
{
    public int Idtask { get; set; }

    public string? Description { get; set; }

    public DateTime? RegisterDate { get; set; }

    public bool Finished { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonsMVC.Models;

public partial class PersonsTasks
{
    [Key]
    public long Idtask { get; set; }

    public string? Description { get; set; }

    public DateTime? RegisterDate { get; set; }

    public bool Finished { get; set; }
	public int IDPerson { get; set; }
}

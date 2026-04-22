using GenZCoders.Models.GenZCoders.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GenZCoders.Models;

public  class Weeks
{
    [Key]
    public long Id { get; set; }

    public string? WeekTitle { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? BusinessEntityName { get; set; }
    public virtual ICollection<CourseMaterial> CourseMaterials { get; set; }
            = new List<CourseMaterial>();
}

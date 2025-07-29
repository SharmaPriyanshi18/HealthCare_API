using HealthCareData.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Assessment
{
    [Key]
    public int AssessmentId { get; set; }

    [ForeignKey("schedulerDate")]
    public int schedulerId { get; set; }

    public schedulerDate schedulerDate { get; set; }
}

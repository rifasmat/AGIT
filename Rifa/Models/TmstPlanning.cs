using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rifa.Models
{
    [Table("tmst_planning")]
    public class TmstPlanning
    {
        [Key]
        [Column("planning_cd")]
        [StringLength(50)]
        public string? PlanningCd { get; set; }

        [Column("emp_nm")]
        [StringLength(150)]
        public string? EmpNm { get; set; }

        [Column("total")]
        public int Total { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }
    }

    [Table("tplanning_user")]
    public class TplanningUser
    {
        [Key]
        [Column("planning_cd")]
        [StringLength(50)]
        public string? PlanningCd { get; set; }

        [Column("senin")] public int Senin { get; set; }
        [Column("selasa")] public int Selasa { get; set; }
        [Column("rabu")] public int Rabu { get; set; }
        [Column("kamis")] public int Kamis { get; set; }
        [Column("jumat")] public int Jumat { get; set; }
        [Column("sabtu")] public int Sabtu { get; set; }
        [Column("minggu")] public int Minggu { get; set; }
    }

    [Table("tplanning_recommendation")]
    public class TplanningRecommendation
    {
        [Key]
        [Column("planning_cd")]
        [StringLength(50)]
        public string? PlanningCd { get; set; }

        [Column("senin")] public int Senin { get; set; }
        [Column("selasa")] public int Selasa { get; set; }
        [Column("rabu")] public int Rabu { get; set; }
        [Column("kamis")] public int Kamis { get; set; }
        [Column("jumat")] public int Jumat { get; set; }
        [Column("sabtu")] public int Sabtu { get; set; }
        [Column("minggu")] public int Minggu { get; set; }
    }

    public class ProductionDay
    {
        public string? EmpNm { get; set; }
        public int Senin { get; set; }
        public int Selasa { get; set; }
        public int Rabu { get; set; }
        public int Kamis { get; set; }
        public int Jumat { get; set; }
        public int Sabtu { get; set; }
        public int Minggu { get; set; }
    }
}
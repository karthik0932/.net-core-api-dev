using System;
using System.Collections.Generic;

namespace Tech_Arch_360.Models
{
    public partial class InventoryQuestionnaire
    {
        public int QuestionId { get; set; }
        public int TenantId { get; set; }
        public string Question { get; set; } = null!;
        public string? Answer { get; set; }
        public string? Instructions { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? AnsweredBy { get; set; }
        public DateTime? AnsweredOn { get; set; }
        public string? LastAnswerModifiedBy { get; set; }
        public DateTime? LastAnswerModifiedOn { get; set; }
    }
}




namespace DevFreela.Application.Models {
    public class UpdateProjectInputModel {
        public int IdProject { get; set; } // Id
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }

    }
}

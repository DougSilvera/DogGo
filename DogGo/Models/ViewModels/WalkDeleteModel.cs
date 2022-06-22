using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkDeleteModel
    {
        public List<Walk> Walks { get; set; }
        public List<int> SelectedWalkIds { get; set; }
    }
}

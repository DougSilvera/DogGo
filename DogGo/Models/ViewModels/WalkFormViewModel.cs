﻿using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkFormViewModel
    {
        public Walk Walk { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<int> SelectedDogIds { get; set; }
        public List<bool> IsSelected { get; set; }
        
    }
}

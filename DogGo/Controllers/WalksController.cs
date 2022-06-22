using DogGo.Interfaces;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepository;
        private readonly IDogRepository _dogRepository;
        private readonly IOwnerRepository _ownerRepository;
       
        

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalksController(IWalkerRepository walkerRepository, IWalksRepository walksRepository, IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _walkerRepo = walkerRepository;
            _walksRepository = walksRepository;
            _dogRepository = dogRepository;
            _ownerRepository = ownerRepository;
            
        }
        // GET: WalksController
        public ActionResult Index()
        {
           
            return View();
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create()
        {
            
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            List<Dog> dogs = _dogRepository.GetAllDogs();

            WalkFormViewModel vm = new WalkFormViewModel()
            {
                Walk = new Walk(),
                Dogs = dogs,
                Walkers = walkers,
                SelectedDogIds = new List<int>()
            };

            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkFormViewModel walkFormViewModel)
        {
           
            try
            {
                foreach (var id in walkFormViewModel.SelectedDogIds)
                {
                    walkFormViewModel.Walk.DogId = id;
                    _walksRepository.CreateWalk(walkFormViewModel.Walk);
                }
                return RedirectToAction("Index", "Walkers");
            }
            catch
            {
                walkFormViewModel.Dogs = _dogRepository.GetAllDogs();
                walkFormViewModel.Walkers = _walkerRepo.GetAllWalkers();
                return View(walkFormViewModel);
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete()
        {
            List<Walk> walks = _walksRepository.GetAllWalks();

            WalkDeleteModel vm = new WalkDeleteModel()
            {
                Walks = walks,
                SelectedWalkIds = new List<int>()


            };
            return View(vm);
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(WalkDeleteModel walkDelete)
        {
            try
            {
                foreach (var id in walkDelete.SelectedWalkIds)
                {
                    _walksRepository.DeleteWalk(id);
                }
                return RedirectToAction("Index", "Walkers");
            }
            catch
            {
                return View(walkDelete);
            }
        }
        private int GetCurrentUserId()
        {
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(id);
            }
        }
    }
}

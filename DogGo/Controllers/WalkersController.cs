﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGo.Repositories;
using System.Collections.Generic;
using DogGo.Models;
using DogGo.Interfaces;
using DogGo.Models.ViewModels;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepository;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalksRepository walksRepository)
        {
            _walkerRepo = walkerRepository;
            _walksRepository = walksRepository;
        }
        // GET: Walkers
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            return View(walkers);
        }

        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walks = _walksRepository.GetWalksByWalkerId(walker.Id);

            if (walker == null)
            {
                return NotFound();
            }
            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Walker = walker,
                Walks = walks
            };

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walker walker)
        {
            try
            {
                _walkerRepo.AddWalker(walker);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }
            return View(walker);
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walker walker)
        {
            try
            {
                _walkerRepo.UpdateWalker(walker);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            return View(walker);
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walker walker)
        {
            try
            {
                _walkerRepo.DeleteWalker(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }
    }
}

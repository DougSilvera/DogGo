using System.Collections.Generic;
using DogGo.Models;

namespace DogGo.Interfaces
{
    public interface IWalksRepository
    {
        List<Walk> GetWalksByWalkerId(int walkerId);
        void CreateWalk(Walk walk);
        List<Walk> GetAllWalks();
        void DeleteWalk(int id);
    }
}

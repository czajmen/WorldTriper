﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models.Models;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _logger = logger;
            _context = context;

        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetTripByName(tripName, username);

            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {

            _logger.LogInformation("Getting all trips from DB");

            return _context.Trips.ToList();

        }

        public Trip GetTripByName(string tripName, string username)
        {
          return  _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == username)
                .FirstOrDefault();
        }

        public IEnumerable<Trip> GetUserAllTrips(string name)
        {
            return _context.Trips
              .Include(t => t.Stops)
              .Where(t => t.UserName == name)
              .ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

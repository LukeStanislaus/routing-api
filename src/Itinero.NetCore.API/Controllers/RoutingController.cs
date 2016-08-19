﻿using System;
using System.Collections.Generic;
using Itinero.LocalGeo;
using Itinero.Profiles;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Itinero.API.Controllers
{
    [Route("api/[controller]")]
    public class RoutingController : Controller
    {
        [HttpGet]
        public Route Get([FromQuery] float fromLat, [FromQuery] float fromLon,
            [FromQuery] float toLat, [FromQuery] float toLon, [FromQuery] string profile)
        {
            var instance = RoutingBootstrapper.Get("belgium");

            var routingProfile = GetProfile(profile);

            var coordinates = new[] {new Coordinate(fromLat, fromLon), new Coordinate(toLat, toLon)};

            var result = instance.Calculate(routingProfile, coordinates, new Dictionary<string, object>());

            return result.Value;
        }

        [HttpPost]
        public Route Post([FromBody] Coordinate[] coordinates, [FromQuery] string profile)
        {
            var instance = RoutingBootstrapper.Get("belgium");

            var routingProfile = GetProfile(profile);
            
            var result = instance.Calculate(routingProfile, coordinates, new Dictionary<string, object>());

            return result.Value;
        }

        private static Profile GetProfile(string profile)
        {
            Profile routingProfile;
            if (string.IsNullOrWhiteSpace(profile))
            {
                routingProfile = Profile.GetAllRegistered().First();
            }
            else if (!Profile.TryGet(profile, out routingProfile))
            {
                throw new Exception("Profile was not found");
            }
            return routingProfile;
        }
    }
}
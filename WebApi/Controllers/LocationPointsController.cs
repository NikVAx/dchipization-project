﻿using Application.Abstractions.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attibutes.ValidationAttibutes;
using WebApi.DTOs.LocationPoint;

namespace WebApi.Controllers
{
    [Route("locations")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class LocationPointsController :
        ControllerBase
    {
        private readonly ILogger<LocationPointsController> _logger;
        private readonly ILocationPointService _locationPointService;
        private readonly IMapper _mapper;

        public LocationPointsController(
            ILogger<LocationPointsController> logger,
            ILocationPointService locationPointService,
            IMapper mapper)
        {
            _logger = logger;
            _locationPointService = locationPointService;
            _mapper = mapper;
        }

        [HttpGet("{pointId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetLocationPointDto>> Get(
            [MinInt64(1)] long pointId)
        {
            var point = await _locationPointService
                .GetByIdAsync(pointId);

            if(point == null)
                return NotFound();

            var result = _mapper
                .Map<GetLocationPointDto>(point);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(policy: ApplicationPolicies.Identified)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetLocationPointDto>> Create(
            CreateUpdateLocationPointDto createPointDto)
        {
            var point = _mapper
                .Map<LocationPoint>(createPointDto);

            await _locationPointService
                .CreateAsync(point);

            var result = _mapper
                .Map<GetLocationPointDto>(point);
            
            return Created($@"locations/{point.Id}", result);
        }

        [HttpPut("{pointId:long}")]
        [Authorize(policy: ApplicationPolicies.Identified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetLocationPointDto>> Update(
            [MinInt64(1)] long pointId,
            CreateUpdateLocationPointDto updatePointDto)
        {
            var point = _mapper
                .Map<LocationPoint>(updatePointDto);
            
            point.Id = pointId;

            await _locationPointService
                .UpdateAsync(point);

            var result = _mapper
                .Map<GetLocationPointDto>(point);

            return Ok(result);
        }

        [HttpDelete("{pointId:long}")]
        [Authorize(policy: ApplicationPolicies.Identified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(
            [MinInt64(1)] long pointId)
        {
            await _locationPointService
                .DeleteAsync(pointId);

            return Ok();
        }
    }
}

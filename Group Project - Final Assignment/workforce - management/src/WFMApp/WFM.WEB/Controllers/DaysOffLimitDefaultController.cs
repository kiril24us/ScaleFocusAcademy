using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

using WFM.Common;
using WFM.DAL.Entities;
using WFM.BLL.Interfaces;
using WFM.Models.DTO.Requests.DaysOffLimitDefaultRequests;
using WFM.Models.DTO.Responses.DaysOffLimitDefaultResponses;

namespace WFM.WEB.Controllers
{
    [ApiController]
    [Authorize(Roles = GlobalConstants.AdminRoleName)]
    [Route("api/DaysOffLimitDefault")]
    public class DaysOffLimitDefaultController : ControllerBase
    {
        private readonly IDaysOffLimitDefaultService _daysOffLimitDefaultService;
        private readonly IMapper _mapper;

        public DaysOffLimitDefaultController(
            IMapper mapper, 
            IDaysOffLimitDefaultService daysOffLimitDefaultService)
        {
            _mapper = mapper;
            _daysOffLimitDefaultService = daysOffLimitDefaultService;
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(List<DaysOffLimitDefaultResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<DaysOffLimitDefaultResponseDTO>> GetAll()
        {
            try
            {
                var countryCodesFromDb = await _daysOffLimitDefaultService.GetAllAsync();

                return Ok(_mapper.Map<IEnumerable<DaysOffLimitDefaultResponseDTO>>(countryCodesFromDb));
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("{countryCode}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DaysOffLimitDefaultResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<DaysOffLimitDefaultResponseDTO>> Get(
            [FromRoute] string countryCode)
        {
            try
            {
                DaysOffLimitDefault daysOffLimitDefaultFromDB = await _daysOffLimitDefaultService.FindByCountryCode(countryCode);

                if (daysOffLimitDefaultFromDB == null)
                {
                    return BadRequest();
                }

                return Ok(_mapper.Map<DaysOffLimitDefaultResponseDTO>(daysOffLimitDefaultFromDB));
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] DaysOffLimitDefaultCreateRequeseDTO daysOffLimitDefaultCreateRequeseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState.Values.Select(x => x.Errors.Select(y => y.ErrorMessage)).ToList());
                }

                DaysOffLimitDefault daysOffLimitDefault = _mapper.Map<DaysOffLimitDefault>(daysOffLimitDefaultCreateRequeseDTO);

                var daysOffLimitDefaultFromDB = await _daysOffLimitDefaultService.CreateAsync(daysOffLimitDefault);

                if (daysOffLimitDefaultFromDB is null)
                {
                    return BadRequest();
                }

                return CreatedAtAction(
                    "Get", 
                    "DaysOffLimitDefault", 
                    new { countryCode = daysOffLimitDefaultFromDB.CountryCode},
                    daysOffLimitDefaultCreateRequeseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }

        [HttpPut("{countryCode}")]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(
            [FromBody] DaysOffLimitDefaultEditRequeseDTO daysOffLimitDefaultEditRequeseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState.Values.Select(x => x.Errors.Select(y => y.ErrorMessage)).ToList());
                }

                DaysOffLimitDefault daysOffLimitDefault = await _daysOffLimitDefaultService.FindByCountryCode(daysOffLimitDefaultEditRequeseDTO.CountryCode);

                if (daysOffLimitDefault == null)
                {
                    return NotFound();
                }

                _mapper.Map(daysOffLimitDefaultEditRequeseDTO, daysOffLimitDefault,
                    typeof(DaysOffLimitDefaultEditRequeseDTO), typeof(DaysOffLimitDefault));


                var daysOffLimitDefaultFromDB = await _daysOffLimitDefaultService.UpdateAsync(daysOffLimitDefault);

                if (daysOffLimitDefaultFromDB == null)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpDelete("{countryCode}")]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [FromRoute] string countryCode)
        {
            try
            {
                DaysOffLimitDefault daysOffLimitDefault = await _daysOffLimitDefaultService.FindByCountryCode(countryCode);
                if (daysOffLimitDefault == null)
                {
                    return NotFound();
                }

                var daysOffLimitDefaultFromDB = await _daysOffLimitDefaultService.DeleteAsync(daysOffLimitDefault);

                if (daysOffLimitDefaultFromDB == null)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}

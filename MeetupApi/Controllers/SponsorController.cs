using AutoMapper;
using MeetupApi.Contracts.Models.Sponsor;
using MeetupApi.Data.Models;
using MeetupApi.Data.Queries;
using MeetupApi.Data.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetupApi.Controllers
{
    /// <summary>
    /// Контроллер спонсоров
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class SponsorController : ControllerBase
    {
        private ISponsorRepository _sponsors;
        private IMapper _mapper;

        public SponsorController(ISponsorRepository sponsors, IMapper mapper)
        {
            _sponsors = sponsors;
            _mapper = mapper;
        }

        /// <summary>
        /// Просмотр списка всех спонсоров
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Sponsor
        /// </remarks>
        /// <returns>Returns SponsorListVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response> 
        [HttpGet]
        [Authorize]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSponsors()
        {
            var sponsors = _sponsors.GetAllSponsors();

            var resp = new GetSponsorResponse
            {
                SponsorAmount = sponsors.Count,
                Sponsors = _mapper.Map<List<Sponsor>, List<SponsorView>>(sponsors)
            };

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Получение спонсора по идентификатору
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Sponsor/BB83CC26-58FB-43D1-A991-7C928F47A3E0
        /// </remarks>
        /// <param name="id">Sponsor id (guid)</param>
        /// <returns>Returns SponsorDetailsVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response> 
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMeetup(Guid id)
        {
            var sponsor = await _sponsors.GetSponsorById(id);

            var resp = _mapper.Map<Sponsor, GetSponsorByIdResponse>(sponsor);

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Добавление нового спосора
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /Sponsor
        /// {
        ///     Name: "Sponsor name",
        /// }
        /// </remarks>
        /// <param name="request">AddSponsorRequest</param>
        /// <returns>Returns added sponsor</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>  
        [HttpPost]
        [Authorize]
        [Route("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add(AddSponsorRequest request)
        {
            var newSponsor = new Sponsor
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };
            await _sponsors.SaveSponsor(newSponsor);

            return StatusCode(201, $"Спонсор: {request.Name} добавлен. Идентификатор: {newSponsor.Id}.");

        }

        /// <summary>
        /// Редактирование существующего спонсора
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Patch /Sponsor/C97B034F-296A-4B39-B05F-B6EC626232D9
        /// {
        ///     Name: "Updated meetup title",
        /// }
        /// </remarks>
        /// <param name="id">Sponsor id (guid)</param>
        /// <param name="request">EditSponsorRequest</param>
        /// <returns>Returns edited sponsor</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>  
        [HttpPatch]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditSponsorRequest request)
        {
            var sponsor = await _sponsors.GetSponsorById(id);

            await _sponsors.UpdateSponsor(sponsor, new UpdateSponsorQuery(request.NewName));

            return StatusCode(200, $"Информация о спонсоре: {sponsor.Name} обновлена!");
        }

        /// <summary>
        /// Удаление существующего спонсора
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /Sponsor/C97B034F-296A-4B39-B05F-B6EC626232D9
        /// </remarks>
        /// <param name="id">Sponsor id (guid)</param>
        /// <returns>Returns name of deleted sponsor</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>  
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var sponsor = await _sponsors.GetSponsorById(id);
            if (sponsor == null)
                return StatusCode(400, $"Ошибка:  Спонсор с идентификатором {id} не существует.");

            await _sponsors.DeleteSponsor(sponsor);

            return StatusCode(200, $"Спонсор {sponsor.Name} Удален! Идентификатор: {sponsor.Id}");
        }
    }
}

using AutoMapper;
using MeetupApi.Contracts.Models.Meetup;
using MeetupApi.Data.Models;
using MeetupApi.Data.Queries;
using MeetupApi.Data.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupApi.Controllers
{
    /// <summary>
    /// Контроллер событий
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class MeetupController : ControllerBase
    {
        private IMeetupRepository _meetups;
        private ISponsorRepository _sponsors;
        private IMapper _mapper;

        public MeetupController(IMeetupRepository meetups, ISponsorRepository sponsors, IMapper mapper)
        {
            _meetups = meetups;
            _sponsors = sponsors;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка событий
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Meetup
        /// </remarks>
        /// <returns>Returns MeetupListVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>       
        [HttpGet]
        [Authorize]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMeetups()
        {
            var meetups = await _meetups.GetMeetups();

            var resp = new GetMeetupResponse
            {
                MeetupAmount = meetups.Count(),
                Meetups = _mapper.Map<IEnumerable<Meetup>, List<MeetupView>>(meetups)
            };

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Получение события по идентификатору
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Meetup/148F85F0-2FE0-4971-8417-737548479B6C
        /// </remarks>
        /// <param name="id">Meetup id (guid)</param>
        /// <returns>Returns MeetupDetailsVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>    
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMeetup(Guid id)
        {
            var meetup = await _meetups.GetMeetupById(id);

            var resp = _mapper.Map<Meetup, GetMeetupByIdResponse>(meetup);

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Добавление нового события
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /Meetup
        /// {
        ///     Name: "Meetup title",
        ///     Description: "Meetup description",
        ///     Organizer: "Meetup organizer",
        ///     EventDate: "Meetup date",
        ///     LocationInfo: "Meetup location place",
        ///     SponsorNames: "Meetup sponsors"
        /// }
        /// </remarks>
        /// <param name="request">AddMeetupRequest</param>
        /// <returns>Returns added meetup</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>  
        [HttpPost]
        [Authorize]
        [Route("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add(AddMeetupRequest request)
        {
            // получаем из строки имена спонсоров
            char[] delimiterChars = { ',' };
            var spns = request.SponsorNames.Trim().Split(delimiterChars).ToList();

            // проверяем если спонсор в базе, если нет, добавляем
            foreach (var spName in spns)
            {
                var sponsor = await _sponsors.GetSponsorByName(spName);

                if (sponsor == null)
                {
                    var newSponsor = new Sponsor
                    {
                        Id = Guid.NewGuid(),
                        Name = spName
                    };

                    await _sponsors.SaveSponsor(newSponsor);
                }
            }

            // получаем из базы всех спонсоров данного митапа
            var sponosors = _sponsors.GetAllSponsors().Where(s => spns.Contains(s.Name)).ToList();

            // проверяем существует ли в базе митап с данным названием
            var meetup = await _meetups.GetMeetupByName(request.Name);
            if (meetup != null)
                return StatusCode(400, $"Ошибка: Митап: {request.Name} уже существует.");

            // создаем новый митап и добавляем в базу
            var newMeetup = new Meetup
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Organizer = request.Organizer,
                EventDate = request.EventDate,
                House = request.LocationInfo.House,
                Building = request.LocationInfo.Building,
                Street = request.LocationInfo.Street,
                Sponsors = sponosors,
            };
            await _meetups.SaveMeetup(newMeetup, sponosors);

            return StatusCode(201, $"Митап: {request.Name} добавлен. Идентификатор: {newMeetup.Id}. Спонсоры: {request.SponsorNames.ToString()}");
        }

        /// <summary>
        /// Редактирование существующего события
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Patch /Meetup/39FF8FCD-E849-4FCF-BED0-9EE190B509B2
        /// {
        ///     Name: "Updated meetup title",
        ///     Description: "Updated meetup description",
        ///     Organizer: "Updated meetup organizer",
        ///     EventDate: "Updated meetup date",
        ///     LocationInfo: "Updated meetup location place",
        ///     SponsorNames: "Updated meetup sponsors"
        /// }
        /// </remarks>
        /// <param name="id">Meetup id (guid)</param>
        /// <param name="request">EditMeetupRequest</param>
        /// <returns>Returns edited meetup</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>  
        [HttpPatch]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditMeetupRequest request)
        {
            // получаем из строки имена спонсоров
            char[] delimiterChars = { ',' };
            var spns = request.NewSponsorNames.Trim().Split(delimiterChars).ToList();

            // проверяем если спонсор в базе, если нет, добавляем
            foreach (var spName in spns)
            {
                var sponsor = await _sponsors.GetSponsorByName(spName);

                if (sponsor == null)
                {
                    var newSponsor = new Sponsor
                    {
                        Id = Guid.NewGuid(),
                        Name = spName
                    };

                    await _sponsors.SaveSponsor(newSponsor);
                }
            }

            // получаем из базы всех спонсоров данного митапа
            var newSponosors = _sponsors.GetAllSponsors().Where(s => spns.Contains(s.Name)).ToList();

            var metup = await _meetups.GetMeetupById(id);
            if (metup == null)
                return StatusCode(400, $"Ошибка: Событие с идентификатором {id} не существует.");

            await _meetups.UpdateMeetup(metup, new UpdateMeetupQuery(request.NewName, request.NewDescription,
                request.NewOrganizer, request.NewEventDate, request.NewLocationInfo.NewHouse, request.NewLocationInfo.NewBuilding,
                request.NewLocationInfo.NewStreet, request.NewSponsorNames.ToString()), newSponosors);

            return StatusCode(200, $"Событие обновлено! Название - {metup.Name}, Оргонизатор - {metup.Organizer}, Дата - {metup.EventDate}");
        }

        /// <summary>
        /// Удаление существующего события
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /Meetup/39FF8FCD-E849-4FCF-BED0-9EE190B509B2
        /// </remarks>
        /// <param name="id">Meetup id (guid)</param>
        /// <returns>Returns name of deleted meetup</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>  
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var meetup = await _meetups.GetMeetupById(id);
            if(meetup == null)
                return StatusCode(400, $"Ошибка: Митап с идентификатором {id} не существует.");

            await _meetups.DeleteMeetup(meetup);

            return StatusCode(200, $"Событие {meetup.Name} Удалено! Идентификатор: {meetup.Id}");
        }
    }
}

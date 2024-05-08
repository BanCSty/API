using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Command.DeleteIE;
using API.Application.IndividualEntrepreneurs.Command.UpdateIE;
using API.Application.IndividualEntrepreneurs.Queries.GetIEDetails;
using API.Application.IndividualEntrepreneurs.Queries.GetIEList;
using API.WebApi.Models.IndividualEntrepreneur;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    public class IndividualEntrepreneurController : BaseController
    {
        public readonly IMapper _mapper;

        public IndividualEntrepreneurController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of IndividualEntrepreneur
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /IndividualEntrepreneur
        /// </remarks>
        /// <returns>Returns IEListVm</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEListVm>> GetAll()
        {
            //Сформируем запрос и с помощью Mediatotr отправим, затем результат вернем колиенту
            var query = new GetIEListQuery();

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Gets the IndividualEntrepreneur by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /IndividualEntrepreneur/D34D349E-43B8-429E-BCA4-793C932FD580
        /// </remarks>
        /// <param name="id">IndividualEntrepreneur id (guid)</param>
        /// <returns>Returns IndividualEntrepreneur</returns>
        /// <response code="200">Success</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEDetailsVm>> GetAll(Guid id)
        {
            var query = new GetIEDetailsQuery
            {
                Id = id
            };
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Creates the IndividualEntrepreneur
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /IndividualEntrepreneur
        /// {
        ///     INN: "IndividualEntrepreneur INN",
        ///     Name: "IndividualEntrepreneur name"
        /// }
        /// </remarks>
        /// <param name="createIEDto">createIEDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateIEDto createIEDto)
        {
            var command = _mapper.Map<CreateIECommand>(createIEDto);
            var IEId = await Mediator.Send(command);

            return Ok(IEId);
        }

        /// <summary>
        /// Updates the IndividualEntrepreneur
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /IndividualEntrepreneur
        /// {
        ///     name: "updated IndividualEntrepreneur name"
        /// }
        /// </remarks>
        /// <param name="updateIEDto">updateIEDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateIEDto updateIEDto)
        {
            var command = _mapper.Map<UpdateIECommand>(updateIEDto);
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes the IndividualEntrepreneur by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /IndividualEntrepreneur/88DEB432-062F-43DE-8DCD-8B6EF79073D3
        /// </remarks>
        /// <param name="id">Id of the IndividualEntrepreneur (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var command = new DeleteIECommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}

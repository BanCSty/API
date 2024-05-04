using API.Application.Founders.Command.CreateFounder;
using API.Application.Founders.Command.DeleteFounder;
using API.Application.Founders.Command.UpdateFounder;
using API.Application.Founders.Queries.GetFoundDetails;
using API.Application.Founders.Queries.GetFounderList;
using API.Domain;
using API.WebApi.Models.Founder;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class FounderController : BaseController
    {
        public readonly IMapper _mapper;

        public FounderController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of founders
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /founder
        /// </remarks>
        /// <returns>Returns FounderListVm</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FounderListVm>> GetAll()
        {
            //Сформируем запрос и с помощью Mediatotr отправим, затем результат вернем колиенту
            var query = new GetFounderListQuery();

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Gets the founder by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /founder/D34D349E-43B8-429E-BCA4-793C932FD580
        /// </remarks>
        /// <param name="id">Founder id (guid)</param>
        /// <returns>Returns Founder</returns>
        /// <response code="200">Success</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Founder>> GetAll(Guid id)
        {
            //Сформируем запрос и с помощью Mediatotr отправим, затем результат вернем колиенту
            var query = new GetFounderDetailsQuery
            {
                Id = id
            };
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Creates the founder
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /founder
        /// {
        ///     INN: "Founder INN",
        ///     FirstName: "Founder FirstName"
        ///     LastName: "Founder LastName"
        ///     MiddleName: "Founder MiddleName"
        /// }
        /// </remarks>
        /// <param name="createFounderDto">createFounderDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateFounderDto createFounderDto)
        {
            var command = _mapper.Map<CreateFounderCommand>(createFounderDto);
            var founderId = await Mediator.Send(command);

            return Ok(founderId);
        }

        /// <summary>
        /// Updates the founder
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /founder
        /// {
        ///     name: "updated founder name"
        /// }
        /// </remarks>
        /// <param name="updateFounderDto">updateFounderDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateFounderDto updateFounderDto)
        {
            var command = _mapper.Map<UpdateFounderCommand>(updateFounderDto);
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes the founder by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /founder/88DEB432-062F-43DE-8DCD-8B6EF79073D3
        /// </remarks>
        /// <param name="id">Id of the founder (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var command = new DeleteFounderCommand
            {
                FounderId = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}

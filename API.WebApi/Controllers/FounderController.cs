using API.Application.Founders.Command.CreateFounder;
using API.Application.Founders.Command.DeleteFounder;
using API.Application.Founders.Command.UpdateFounder;
using API.Application.Founders.Queries.GetFoundDetails;
using API.Application.Founders.Queries.GetFounderList;
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
    public class FounderController : BaseController
    {
        

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
        /// GET /founder/1234567891101
        /// </remarks>
        /// <param name="inn">Founder INN</param>
        /// <returns>Returns FounderDto</returns>
        /// <response code="200">Success</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FounderDetailsVm>> GetAll(string inn)
        {
            //Сформируем запрос и с помощью Mediatotr отправим, затем результат вернем колиенту
            var query = new GetFounderDetailsQuery
            {
                INN = inn
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
        /// <param name="createFounderCommand">CreateFounderCommand object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateFounderCommand createFounderCommand)
        {
            var founderId = await Mediator.Send(createFounderCommand);

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
        /// <param name="updateFounderCommand">UpdateFounderCommand object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateFounderCommand updateFounderCommand)
        {
            await Mediator.Send(updateFounderCommand);
            return NoContent();
        }

        /// <summary>
        /// Deletes the founder by INN
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /founder/1234567891101
        /// </remarks>
        /// <param name="inn">INN of the founder (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Guid>> Delete(string inn)
        {
            var command = new DeleteFounderCommand
            {
                INN = inn
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}

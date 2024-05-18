using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Command.DeleteLegalEntity;
using API.Application.LegalEntitys.Command.UpdateLegalEntity;
using API.Application.LegalEntitys.Queries.GetLegalEntityDetails;
using API.Application.LegalEntitys.Queries.GetLegalEntityList;
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
    public class LegalEntityController : BaseController
    {


        /// <summary>
        /// Gets the list of LegalEntity
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /LegalEntity
        /// </remarks>
        /// <returns>Returns LegalEntityListVm</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LegalEntityListVm>> GetAll()
        {
            //Сформируем запрос и с помощью Mediatotr отправим, затем результат вернем колиенту
            var query = new GetLegalEntityListQuery();

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Gets the LegalEntity by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /LegalEntity/D34D349E-43B8-429E-BCA4-793C932FD580
        /// </remarks>
        /// <param name="id">LegalEntity id (guid)</param>
        /// <returns>Returns LegalEntity</returns>
        /// <response code="200">Success</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LegalEntityDetailsVm>> GetAll(Guid id)
        {
            //Сформируем запрос и с помощью Mediatotr отправим, затем результат вернем колиенту
            var query = new GetLegalEntityDetailsQuery
            {
                Id = id
            };
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Creates the LegalEntity
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /LegalEntity
        /// {
        ///     INN: "LegalEntity INN",
        ///     Name: "LegalEntity name"
        ///     FounderId: "FounderId id"
        /// }
        /// </remarks>
        /// <param name="createLegalEntityCommand">CreateLegalEntityCommand object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateLegalEntityCommand createLegalEntityCommand)
        {
            var LEId = await Mediator.Send(createLegalEntityCommand);

            return Ok(LEId);
        }

        /// <summary>
        /// Updates the LegalEntity
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /LegalEntity
        /// {
        ///     name: "updated LegalEntity name"
        /// }
        /// </remarks>
        /// <param name="UpdateLegalEntityCommand">UpdateLegalEntityCommand object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateLegalEntityCommand updateLegalEntityCommnad)
        {
            await Mediator.Send(updateLegalEntityCommnad);
            return NoContent();
        }

        /// <summary>
        /// Deletes the LegalEntity by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /LegalEntity/88DEB432-062F-43DE-8DCD-8B6EF79073D3
        /// </remarks>
        /// <param name="id">Id of the LegalEntity (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var command = new DeleteLegalEntityCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}

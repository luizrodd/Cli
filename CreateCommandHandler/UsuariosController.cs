
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        
        private readonly IMediator _mediatr;

        public UsuariosController(IMediator mediatr)
        {
            _mediatr = mediator;
        }

        [HttpGet]
        public Task<IActionResult> Get()
        {
            var qry = new GetUsuariosQuery();
            var rslt = _mediatr.Send(qry);
            if(rslt == null)
                return BadRequest();

            return Ok(rslt);
        }

           [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var qry = new GetUsuariosByIdQuery { Id = id };
            var rslt = await _mediator.Send(qry);
            if (rslt == null)
                return NotFound();

            return Ok(rslt);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuariosCommand command)
        {
            var rslt = await _mediator.Send(command);
            if (rslt == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = rslt.Id }, rslt);
        }

        [HttpPut(""{id}"")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuariosCommand command)
        {{
            if (id != command.Id)
                return BadRequest("Id do corpo da requisição não corresponde ao Id da URL.");

            var rslt = await _mediator.Send(command);
            if (rslt == null)
                return NotFound();

            return NoContent();
        }}

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUsuariosCommand { Id = id };
            var rslt = await _mediator.Send(command);
            if (!rslt)
                return NotFound();

            return NoContent();
        }

    }
}
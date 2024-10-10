using Spectre.Console;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Verifica se os argumentos foram passados
        if (args.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]Erro: Você precisa fornecer ao menos o nome do componente como argumento.[/]");
            AnsiConsole.MarkupLine("[yellow]Exemplo: mediatorcli Produto [tipoRetorno] [nomeController][/]");
            return;
        }

        string nomeBase = args[0];

        if (args.Length == 1)
        {
            CriarController(nomeBase);
            AnsiConsole.MarkupLine($"[bold green]Controller para {nomeBase} criado com sucesso![/]");
        }
        else if (args.Length == 2)
        {
            string tipoRetorno = args[1];
            CriarCommand(nomeBase, tipoRetorno);
            CriarCommandHandler(nomeBase, tipoRetorno);
            AnsiConsole.MarkupLine($"[bold green]Command para {nomeBase} criado com sucesso![/]");
        }
        else if (args.Length >= 3)
        {
            AnsiConsole.MarkupLine($"[bold green]Controller, Query e Command para {nomeBase} foram criados com sucesso![/]");
        }
    }

    static void CriarController(string nomeController)
    {
        string conteudo = $@"
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{{
    [ApiController]
    [Route(""api/[controller]"")]
    public class {nomeController}Controller : ControllerBase
    {{
        
        private readonly IMediator _mediatr;

        public {nomeController}Controller(IMediator mediatr)
        {{
            _mediatr = mediator;
        }}

        [HttpGet]
        public Task<IActionResult> Get()
        {{
            var qry = new Get{nomeController}Query();
            var rslt = _mediatr.Send(qry);
            if(rslt == null)
                return BadRequest();

            return Ok(rslt);
        }}

           [HttpGet(""{"id"}"")]
        public async Task<IActionResult> GetById(int id)
        {{
            var qry = new Get{nomeController}ByIdQuery {{ Id = id }};
            var rslt = await _mediator.Send(qry);
            if (rslt == null)
                return NotFound();

            return Ok(rslt);
        }}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create{nomeController}Command command)
        {{
            var rslt = await _mediator.Send(command);
            if (rslt == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new {{ id = rslt.Id }}, rslt);
        }}

        [HttpPut(""""{{id}}"""")]
        public async Task<IActionResult> Update(int id, [FromBody] Update{nomeController}Command command)
        {{{{
            if (id != command.Id)
                return BadRequest(""Id do corpo da requisição não corresponde ao Id da URL."");

            var rslt = await _mediator.Send(command);
            if (rslt == null)
                return NotFound();

            return NoContent();
        }}}}

        [HttpDelete(""{"id"}"")]
        public async Task<IActionResult> Delete(int id)
        {{
            var command = new Delete{nomeController}Command {{ Id = id }};
            var rslt = await _mediator.Send(command);
            if (!rslt)
                return NotFound();

            return NoContent();
        }}

    }}
}}";

        File.WriteAllText($"{nomeController}Controller.cs", conteudo);
    }

    static void CriarCommand(string nome, string tipoRetorno)
    {
        string conteudo = $@"
using MediatR;

namespace Application
{{
    public class {nome} : IRequest<{tipoRetorno}>
    {{
        // Defina as propriedades aqui
    }}
}}";

        File.WriteAllText($"{nome}.cs", conteudo);
    }

    static void CriarCommandHandler(string nome, string tipoRetorno)
    {
        string conteudo = $@"
using MediatR;

namespace Application
{{
    public class {nome}Handler : IRequestHandler<{nome}, {tipoRetorno}>
    {{
        public {tipoRetorno} Handle({nome} request, CancellationToken cancellationToken)
        {{
            return;
        }}
    }}
}}";

        File.WriteAllText($"{nome}Handler.cs", conteudo);
    }
}

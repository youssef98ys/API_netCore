using System;
using System.Threading.Tasks;
using APIwebNETCORE.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace APIwebNETCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IDocumentClient _documentClient;
        readonly String databseId;
        readonly String collectionId;
        public IConfiguration Configuration { get; }

        public PatientController(IDocumentClient documentClient, IConfiguration configuration)
        {
            _documentClient = documentClient;
            Configuration = configuration;

            databseId = Configuration["DatabaseId"];
            collectionId = "Patients";
            BuildCollection().Wait();
        }

        private async Task BuildCollection()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databseId });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databseId),
                new DocumentCollection { Id = collectionId });
        }

        [HttpGet]
        public IQueryable<Patient> Get()
        {
            return _documentClient.CreateDocumentQuery<Patient>(UriFactory.CreateDocumentCollectionUri(databseId, collectionId),new FeedOptions { MaxItemCount = 20});
        }

        [HttpGet("{id}")]
        public IQueryable<Patient> Get(string id)
        {
            return _documentClient.CreateDocumentQuery<Patient>(UriFactory.CreateDocumentCollectionUri(databseId, collectionId),
                new FeedOptions { MaxItemCount = 1 }).Where((i) => i.id == id);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Patient patient)
        {
            var response = await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databseId, collectionId), patient);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Patient item)
        {
            await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databseId, collectionId, id),
                item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databseId, collectionId, id));
            return Ok();
        }
    }
}

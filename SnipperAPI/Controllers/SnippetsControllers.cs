using Microsoft.AspNetCore.Mvc;

namespace SnippetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnippetsController : ControllerBase
    {
        private static readonly List<Snippet> Snippets = new List<Snippet>();

        // POST api/snippets
        [HttpPost]
        public ActionResult<Snippet> CreateSnippet([FromBody] SnippetRequest snippetRequest)
        {
            // Validating incoming data
            if (snippetRequest == null || string.IsNullOrEmpty(snippetRequest.Lang) || string.IsNullOrEmpty(snippetRequest.Content))
            {
                return BadRequest("Invalid request. Title and Content are required.");
            }

            // Creating new snippet
            Snippet newSnippet = new Snippet(Guid.NewGuid(), snippetRequest.Lang, snippetRequest.Content, DateTime.UtcNow);

            // Storing snippet
            Snippets.Add(newSnippet);

            // Returning the created snippet with Created status - use abstraction / restfulAPI
            return CreatedAtAction(nameof(GetSnippet), new { id = newSnippet.Id }, newSnippet);
        }

        // GET api/snippets/{id}
        [HttpGet("{id}")]
        public ActionResult<Snippet> GetSnippet(Guid id)
        {
            // Find the snippet by Id
            Snippet snippet = Snippets.Find(s => s.Id == id);

            // Check if the snippet is found
            if (snippet == null)
            {
                return NotFound("Snippet not found");
            }

            // Return the snippet
            return Ok(snippet);
        }

        //TEST CHANGE
        //create endpoint for all snippets 



        // retrive by query 
        [HttpGet("snippets/{lang}")]
        public ActionResult<IEnumerable<Snippet>> GetSnippetsByLang(string lang)
        {
            // Validate the input
            if (string.IsNullOrEmpty(lang))
            {
                return BadRequest("Language parameter is required.");
            }

            // Find snippets that match the specified language
            List<Snippet> matchingSnippets = Snippets.FindAll(s => s.Lang.Equals(lang, StringComparison.OrdinalIgnoreCase));

            // Check if any snippets are found
            if (matchingSnippets.Count == 0)
            {
                return NotFound($"No snippets found for the language '{lang}'.");
            }

            // Return the matching snippets
            return Ok(matchingSnippets);
        }
    }
}


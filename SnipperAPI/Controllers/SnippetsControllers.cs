using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using SnipperAPI.interfaces;


namespace SnippetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnippetsController : ControllerBase
    {
        private static readonly List<EncryptedSnippet> Snippets = new List<EncryptedSnippet>();


        private readonly IEncrypt _encryptionService;

        public object EncryptionHelper { get; private set; }

        public SnippetsController(IEncrypt encryptionService)
        {
            _encryptionService = encryptionService;
        }

        // POST api/snippets
        /*[HttpPost]
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
        }*/


        [HttpPost("snippets")]
        //[Authorize] // Requires authentication
        public ActionResult<EncryptedSnippet> CreateSnippet([FromBody] SnippetRequest snippetRequest)
        {
            if (ModelState.IsValid)
            {
               // var currentUser = _userManager.GetUserAsync(User).Result;

                // Encrypt the content before saving it
                var encryptedContent =_encryptionService.Encrypt(snippetRequest.Content);

                var newSnippet = new EncryptedSnippet(Id: Guid.NewGuid(), Title: snippetRequest.Title, Content: encryptedContent, CreatedAt: DateTime.UtcNow);
                

                Snippets.Add(newSnippet);

                return CreatedAtAction(nameof(GetSnippet), new { id = newSnippet.Id }, newSnippet);
            }

            return BadRequest("Invalid snippet data.");
        }
    


    //GET api/snippets/{id}
    [HttpGet("{id}")]
        public ActionResult<EncryptedSnippet> GetSnippet(Guid id)
        {
            // Find the snippet by Id
            EncryptedSnippet snippet = Snippets.Find(s => s.Id == id);

            // Check if the snippet is found
            if (snippet == null)
            {
                return NotFound("Snippet not found");
            }
            
            // Return the snippet
            return Ok(snippet);
        }

        //create endpoint for all snippets 

        /*[HttpGet("snippets")]
        public ActionResult<IEnumerable<Snippet>> GetAllSnippets()
        {
            // Assuming Snippets is a list of Snippet objects in your data store
            if (Snippets.Count == 0)
            {
                return NotFound("No snippets found.");
            }

            return Ok(Snippets);
        }*/


        [HttpGet("snippets")]
        //[Authorize] // Requires authentication
        public ActionResult<IEnumerable<EncryptedSnippet>> GetAllSnippets()
        {
            // Get the current user

            //  list of Snippet objects in your data store
            if (Snippets.Count == 0)
            {
                return NotFound("No snippets found.");
            }

            // Decrypt snippets before returning
            var decryptedSnippets = Snippets.Select(snippet =>
                new DecryptedSnippet(snippet.Id, snippet.Title,_encryptionService.Decrypt(snippet.Content), snippet.CreatedAt)
            );

            return Ok(decryptedSnippets);
        }




        // retrive by query 
        [HttpGet("snippets/{lang}")]
        public ActionResult<IEnumerable<EncryptedSnippet>> GetSnippetsByLang(string lang, EncryptedSnippet s)
        {
            // Validate the input
            if (string.IsNullOrEmpty(lang))
            {
                return BadRequest("Language parameter is required.");
            }

            // Find snippets that match the specified language
            List<EncryptedSnippet> matchingSnippets = Snippets.FindAll(s => s.Lang.Equals(lang, StringComparison.OrdinalIgnoreCase));

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



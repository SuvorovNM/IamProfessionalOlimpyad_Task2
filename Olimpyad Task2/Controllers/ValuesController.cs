using Olimpyad_Task2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace Olimpyad_Task2.Controllers
{
    public class NotesController : ApiController
    {
        public int N = Int32.Parse(ConfigurationManager.AppSettings["N"]);
        public NotesController()
        {

        }
        [HttpPost]
        [Route("notes")]
        public IHttpActionResult Post([FromBody] AddRequest request)
        {
            try
            {
                int id = 1;
                if (SingletonNotes.Notes.Any())
                    id = SingletonNotes.Notes.Last().id + 1;

                var title = request.title;
                if (string.IsNullOrEmpty(request.title))
                    title = request.content.Substring(0, N);

                var returnNote = new Note
                {
                    id = id,
                    content = request.content,
                    title = request.title
                };

                var note = new Note
                {
                    id = id,
                    content = request.content,
                    title = title
                };
                SingletonNotes.Notes.Add(note);

                return Ok(returnNote);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("notes")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(SingletonNotes.Notes);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("notes/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var note = SingletonNotes.Notes.Where(x => x.id == id).First();
                return Ok(note);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("notes/{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] AddRequest request)
        {
            try
            {
                var note = SingletonNotes.Notes.Where(x => x.id == id).First();
                note.title = request.title;
                note.content = request.content;
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("notes")]
        public IHttpActionResult Get(string query)
        {
            try
            {
                var notes = SingletonNotes.Notes.Where(x => x.content.Contains(query) ||x.title.Contains(query));
                return Ok(notes);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("notes/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var deleteItem = SingletonNotes.Notes.Where(x => x.id == id).First();
                SingletonNotes.Notes.Remove(deleteItem);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}

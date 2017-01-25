﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote.Models;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        // GET: api/questions
        [HttpGet]
        public string Get()
        {
            //Get All Groups
            using (var db = new EF_DB_Context())
            {
                var question = from x in db.Question select x;
                //groups = db.Group.First(g => g.Id >= 1);

                return JsonConvert.SerializeObject(question);
            }
        }

        // GET api/questions/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/questions
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/questions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/questions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

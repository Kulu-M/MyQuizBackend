using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class QuestionBlockController : Controller
    {
        #region GET
        
        // GET api/questionBlock/
        [HttpGet]
        public IActionResult GetAllQuestionBlocks()
        {
            List<QuestionBlock> questionBlockList = new List<QuestionBlock>();
            using (var db = new EF_DB_Context())
            {
                foreach (var questionBlockInDb in db.QuestionBlock)
                {
                    questionBlockInDb.fillWithValues();
                    questionBlockList.Add(questionBlockInDb);
                }
                
                return Ok(JsonConvert.SerializeObject(questionBlockList));
            }
        }

        // GET api/questionBlock/:id
        [HttpGet("{id}")]
        public IActionResult GetQuestionBlockById(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var questionBlockInDb = db.QuestionBlock.FirstOrDefault(qb => qb.Id == id);
                if (questionBlockInDb == null) return BadRequest("Id not found!");

                questionBlockInDb.fillWithValues();
                
                return Ok(JsonConvert.SerializeObject(questionBlockInDb));
            }
        }

        #endregion GET

        #region POST

        // POST api/values
        [HttpPost]
        public IActionResult PostNewQuestionBlock([FromBody] JObject value)
        {
            var questionBlock = new QuestionBlock();
            QuestionBlock existingQuestionBlock;
            if (value == null) return BadRequest();
            try
            {
                questionBlock = JsonConvert.DeserializeObject<QuestionBlock>(value.ToString());
            }
            catch (Exception)
            {
               return BadRequest("Could not deserialize!");
            }
            
            using (var db = new EF_DB_Context())
            {
                existingQuestionBlock = db.QuestionBlock.FirstOrDefault(qb => qb.Id == questionBlock.Id);
            }
            //QuestionBlock is new
            if (existingQuestionBlock == null)
            {
                saveNewQuestionBlockToDatabase(questionBlock);
            }

            //QuestionBlock already exists in Database
            else
            {
                removeQuestionBlockFromDatabase(existingQuestionBlock);
                saveNewQuestionBlockToDatabase(questionBlock);
            }
            return Ok(JsonConvert.SerializeObject(questionBlock));
        }
        
        #endregion POST

        #region DELETE

        // DELETE api/questionBlock/:id
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestionBlock(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var questionBlockInDb = db.QuestionBlock.FirstOrDefault(qb => qb.Id == id);
                if (questionBlockInDb == null) return BadRequest("Id not found!");

                removeQuestionBlockFromDatabase(questionBlockInDb);
                
                return Ok("Deleted: " + id);
            }
        }

        #endregion DELETE

        #region LOGIC

        public QuestionBlock saveNewQuestionBlockToDatabase(QuestionBlock questionBlock)
        {
            using (var db = new EF_DB_Context())
            {
                db.QuestionBlock.Add(questionBlock);
                db.SaveChanges();
                foreach (var question in questionBlock.Questions)
                {
                    db.Question.Add(question);
                    db.SaveChanges();

                    foreach (var answerOption in question.AnswerOptions)
                    {
                        db.AnswerOption.Add(answerOption);
                        db.SaveChanges();
                        db.QuestionAnswerOption.Add(new QuestionAnswerOption
                        {
                            AnswerOptionId = answerOption.Id,
                            QuestionId = question.Id
                        });
                        db.SaveChanges();
                    }

                    db.QuestionQuestionBlock.Add(new QuestionQuestionBlock
                    {
                        QuestionBlockId = questionBlock.Id,
                        QuestionId = question.Id
                    });
                    db.SaveChanges();
                }
            }
            return questionBlock;
        }

        private void removeQuestionBlockFromDatabase(QuestionBlock questionBlockInDb)
        {
            using (var db = new EF_DB_Context())
            {
                var questionsFinder =
                    from q in db.QuestionQuestionBlock
                    where q.QuestionBlockId == questionBlockInDb.Id
                    select q;

                var questionList = db.Question.Where(q => questionsFinder.Any(q2 => q2.QuestionId == q.Id));

                foreach (var question in questionList)
                {
                    var questionAnswerOptionsToDelete = (from a in db.QuestionAnswerOption
                        where a.QuestionId == question.Id
                        select a);

                    //Delete from AnswerOption
                    db.AnswerOption.RemoveRange(
                        db.AnswerOption.Where(a => questionAnswerOptionsToDelete.Any(a2 => a2.AnswerOptionId == a.Id)));

                    //Delete from QuestionAnswerOption
                    db.QuestionAnswerOption.RemoveRange(questionAnswerOptionsToDelete);
                }

                //Delete from Question
                db.Question.RemoveRange(questionList);

                //Delete from QuestionQuestionBlock
                db.QuestionQuestionBlock.RemoveRange(questionsFinder);

                //Delete the QuestionBlock
                db.QuestionBlock.Remove(questionBlockInDb);

                db.SaveChanges();
            }
        }

        #endregion LOGIC
    }
}

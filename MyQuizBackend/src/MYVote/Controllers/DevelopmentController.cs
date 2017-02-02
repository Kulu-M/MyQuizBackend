using System;
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
    public class DevelopmentController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            List<SingleTopic> singleTopicList = new List<SingleTopic>();

            var singleTopic = new SingleTopic();
            singleTopic.Name = "Lloyd";

            var singleTopic2 = new SingleTopic();
            singleTopic2.Name = "Marius";

            singleTopicList.Add(singleTopic);
            singleTopicList.Add(singleTopic2);

            return Ok(JsonConvert.SerializeObject(singleTopicList));
        }

        // GET: api/values
        [HttpGet("registerDevice")]
        public IActionResult GetSerializedRegisterDeviceObject()
        {
            var reg = new RegistrationDevice
            {
                deviceID = null,
                password = "",
                token = "bla"
            };

            return Ok(JsonConvert.SerializeObject(reg));
        }

        // GET: api/questionBlock
        [HttpGet("questionBlock")]
        public IActionResult GetQb()
        {
            QuestionBlock qb = new QuestionBlock();
            qb.Title = "DevelopmentQuestionBlock";
            var question1 = new Question();
            question1.Text = "Hallo";
            question1.MultipleChoice = 0;
            question1.Category = "quiz";
            var answer1 = new AnswerOption();
            answer1.Text = "Möglichkeit1";
            answer1.TrueFalse = "false";
            var answer2 = new AnswerOption();
            answer2.Text = "Möglichkeit2";
            answer2.TrueFalse = "true";
            question1.answerList.Add(answer1);
            question1.answerList.Add(answer2);

            var question2 = new Question();
            question2.Text = "Welt";
            question2.MultipleChoice = 1;
            question2.Category = "poll";
            var answer3 = new AnswerOption();
            answer3.Text = "Möglichkeit3";
            answer3.TrueFalse = "false";
            var answer4 = new AnswerOption();
            answer4.Text = "Möglichkeit4";
            answer4.TrueFalse = "true";
            question2.answerList.Add(answer3);
            question2.answerList.Add(answer4);

            qb.questionList.Add(question1);
            qb.questionList.Add(question2);

            return Ok(JsonConvert.SerializeObject(qb));
        }

        // GET: api/answerOption
        [HttpGet("answerOption")]
        public IActionResult GetAnswerOption()
        {
            
            List<AnswerOption> answerList = new List<AnswerOption>();

            using (var db = new EF_DB_Context())
            {
                var x = db.AnswerOption.FirstOrDefault(aO => aO.Text == "Möglichkeit1");
                answerList.Add(x);
            }
            return Ok(JsonConvert.SerializeObject(answerList));
        }

        //Just for Patrick
        // GET: api/questionBlock
        [HttpGet("fullanswer/group={groupdid}/question={questionid}/device={deviceid}")]
        public IActionResult GetCrapForPatrick(int groupid, int questionid, int topicid)
        {
            var paddyAnswer = new FullAnswer();

            using (var db = new EF_DB_Context())
            {
                paddyAnswer.Veranstaltung = db.Group.FirstOrDefault(gr => gr.Id == groupid);
                paddyAnswer.Person = db.SingleTopic.FirstOrDefault(t => t.Id == topicid);
                paddyAnswer.Frage = db.Question.FirstOrDefault(q => q.Id == questionid);
                paddyAnswer.Antwort = new GivenAnswer();
            }
            return Ok(JsonConvert.SerializeObject(paddyAnswer));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class FullAnswer
    {
        public Group Veranstaltung { get; set; }
        public SingleTopic Person { get; set; }
        public Question Frage { get; set; }
        public GivenAnswer Antwort { get; set; }
    }
}

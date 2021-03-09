using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestModule.Indexes;
using TestModule.Models;
using YesSql;

namespace TestModule.Controllers
{
    public class TestController : Controller
    {
        private readonly ISession _session;

        public TestController(ISession session)
        {
            _session = session;
        }

        [HttpGet]
        public async Task<IActionResult> AddTestData()
        {
            _session.Save(new Person
            {
                Firstname = "Dawid",
                Age = 99
            });
            _session.Save(new AnotherPerson
            {
                Firstname = "Dawid",
                Age = 99,
                AdditionalField1 = "test"
            });
            await _session.CommitAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTestData()
        {
            var query1 = _session.Query<AnotherPerson, AnotherPersonByAge>();
            var items1 = await query1.ListAsync();
            var query2 = _session.Query<Person, PersonByAge>();
            var items2 = await query2.ListAsync();
            return Ok(new { items1, items2 });
        }
    }
}

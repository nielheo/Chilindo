using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chillindo.Core.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chillindo.Api.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IAccountRepository _AccountRep;
        public AccountController(IAccountRepository _accountRep)
        {
            _AccountRep = _accountRep;
        }

        // GET: api/values
        [HttpGet]
        [Route("balance/{id}")]
        public async Task<AccountTransactionResponse> Balance(int id)
        {
            return await _AccountRep.Balance(id);
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
}

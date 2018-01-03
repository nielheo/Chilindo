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
        [Route("{id}/balance")]
        public async Task<AccountTransactionResponse> Balance(int id)
        {
            return await _AccountRep.Balance(id);
        }
        
        // POST api/values
        [HttpPost]
        [Route("{id}/deposit")]
        public async Task<AccountTransactionResponse> Deposit([FromBody]AccountTransactionRequest request)
        {
            return await _AccountRep.Deposit(request);
        }

        // POST api/values
        [HttpPost]
        [Route("{id}/withdraw")]
        public async Task<AccountTransactionResponse> Withdraww([FromBody]AccountTransactionRequest request)
        {
            return await _AccountRep.Withdraw(request);
        }
        
    }
}

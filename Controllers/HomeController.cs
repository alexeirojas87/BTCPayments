using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BTCPayments.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IConfiguration config;
        public HomeController(IConfiguration iConfig)
        {
            config = iConfig;
            var path = config.GetValue<string>("PathKey");
            var keypass= config.GetValue<string>("PassKey");
            Key _privateKey = null;
            if (System.IO.File.Exists(path))
            {
                var sec = new BitcoinEncryptedSecretNoEC(System.IO.File.ReadAllBytes(path), Network.TestNet);
                _privateKey = sec.GetKey(keypass);
            }
            else
            {
                _privateKey = new Key();
                var encKey = _privateKey.GetEncryptedBitcoinSecret(keypass, Network.TestNet);
                System.IO.File.WriteAllBytes(path, encKey.ToBytes());
            }

        }
        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var key = new Key();
            var address = key.PubKey.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet);
            return new string[] { "Your BTC address: {0}\n\n", address.ToString() };
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SmiteAPI
{
    public class Smite
    {
        private readonly Config _config;
        private readonly SignatureProvider _signatureProvider;
        private readonly EndpointBuilder _endpointBuilder;
        private bool _isSessionValid => DateTime.UtcNow - _sessionCreated < TimeSpan.FromMinutes(10);
        private DateTime _sessionCreated = DateTime.MinValue;

        public Smite(Config config)
        {
            _config = config;
            _signatureProvider = new SignatureProvider(_config.DevId, _config.DevAuthKey);
            _endpointBuilder = new EndpointBuilder(ResponseFormat.JSON, _config.SmiteEndpoint, _config.DevId, _signatureProvider);
        }

        private void MarkSessionInvalid()
        {
            _sessionCreated = DateTime.MinValue;
        }
        private bool IsSessionValid(object result)
        {
            // Maybe there is a better way to detect a session was not valid while request.
            if (result is Model.ResponseBase)
            {
                var r = result as Model.ResponseBase;
                if (string.IsNullOrEmpty(r.ret_msg))
                {
                    return true;
                }
            }
            else if (result is Model.ResponseBase[])
            {
                var r = result as Model.ResponseBase[];
                if (r.All((item) => string.IsNullOrEmpty(item.ret_msg)))
                {
                    return true;
                }
            }

            return false;
        }
        private async Task<T> Request<T>(Func<Task<T>> requestFactory) where T : class
        {
            await CreateSession();

            var result = await requestFactory();
            
            // check if session is still valid
            if (!IsSessionValid(result))
            {
                // if we detect invalid request a new session.
                MarkSessionInvalid();
                await CreateSession();
                
                // and request again. if it still fails.. :/
                return await requestFactory();
            }

            return result;
        }

        internal async Task CreateSession()
        {
            if (_isSessionValid) return;

            var result = await JsonRequest.Get<Model.Session>(_endpointBuilder.CreateSession());
            if (result != null && result.ret_msg == "Approved")
            {
                _sessionCreated = DateTime.UtcNow;
                _endpointBuilder.UpdateSession(result.session_id);
            }
        }

        public Task<Model.God[]> GetGods(int languageCode)
        {
            return Request(() => JsonRequest.Get<Model.God[]>(_endpointBuilder.GetGods(languageCode)));
        }

        public Task<Model.Player> GetPlayer(string playerName)
        {
            return Request(() => JsonRequest.Get<Model.Player>(_endpointBuilder.GetPlayer(playerName), true));
        }

        public Task<Model.MatchHistory[]> GetMatchHistory(string playerName)
        {
            return Request(() => JsonRequest.Get<Model.MatchHistory[]>(_endpointBuilder.GetMatchHistory(playerName)));
        }
        public Task<Model.Item[]> GetItems(int languageCode)
        {
            return Request(() => JsonRequest.Get<Model.Item[]>(_endpointBuilder.GetItems(languageCode)));
        }
    }
}

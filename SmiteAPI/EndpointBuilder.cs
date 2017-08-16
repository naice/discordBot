using System;
using System.Collections.Generic;
using System.Text;

namespace SmiteAPI
{
    internal enum ResponseFormat { JSON, XML }
    internal class EndpointBuilder
    {
        private readonly SignatureProvider _signatureProvider;
        private readonly ResponseFormat _responseFormat;
        private readonly string _smiteEndPoint;
        private readonly string _developerId;

        private string _responseFormatUri => _responseFormat.ToString();
        private string _session;
        private string _timestamp;

        public EndpointBuilder(ResponseFormat responseFormat, string smiteEndPoint, string devId, SignatureProvider signatureProvider)
        {
            _responseFormat = responseFormat;
            _smiteEndPoint = smiteEndPoint;
            _signatureProvider = signatureProvider;
            _developerId = devId;
        }

        public void UpdateSession(string session)
        {
            _session = session;
        }

        public string CreateSession()
        {
            ///createsession[ResponseFormat]/{developerId}/{signature}/{timestamp}
            _timestamp = DateTimeStringProvider.SmiteUtcNow;
            _signatureProvider.UpdateTimestamp(_timestamp);
            var methodName = "createsession";
            return _smiteEndPoint + $"/{methodName}{_responseFormatUri}/{_developerId}/{_signatureProvider.GetSignature(methodName)}/{_timestamp}";
        }

        public string GetPlayer(string player)
        {
            ///getplayer[ResponseFormat]/{developerId}/{signature}/{session}/{timestamp}/{player}
            var methodName = "getplayer";
            return _smiteEndPoint + $"/{methodName}{_responseFormatUri}/{_developerId}/{_signatureProvider.GetSignature(methodName)}/{_session}/{_timestamp}/{player}";
        }

        public string GetMatchHistory(string player)
        {
            ///getmatchhistory[ResponseFormat]/{developerId}/{signature}/{session}/{timestamp}/{player}
            var methodName = "getmatchhistory";
            return _smiteEndPoint + $"/{methodName}{_responseFormatUri}/{_developerId}/{_signatureProvider.GetSignature(methodName)}/{_session}/{_timestamp}/{player}";
        }
    }
}

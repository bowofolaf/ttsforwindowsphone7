using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using WPhoneMSTAccessToken;
using WPhoneSpeechTools.ServiceMSTranslator;

namespace WPhoneSpeechTools
{
    public class SpeechService
    {
        #region private members

        private Action _action;
        private string _quality = "";
        private LanguageServiceClient _serviceClient;
        private string _language = "";
        //private string[] _languages;
        
        //private string _appId = "";
        private string _clientID = "";
        private string _clientSecret = "";

        private AdmAuthentication admAuth;
        private AdmAccessToken _token;

        private string _textToSpeak;

        #endregion

        #region properties

        public enum Action
        {
            Speak
        }

        public enum SpeechQuality
        {
            Max,
            Min
        }


        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }


        public SpeechQuality Quality
        {
            set 
            {
                if (value == SpeechQuality.Max)
                    _quality = "MaxQuality";
                if (value == SpeechQuality.Min)
                    _quality = "MinSize";
            }
        }

        #endregion

        public SpeechService(string clientID, string clientSecret)
        {
            //_appId = appId;
            _clientID = clientID;
            _clientSecret = clientSecret;
            _serviceClient = new LanguageServiceClient();
            HookupEvents();
            admAuth = new AdmAuthentication(_clientID, _clientSecret, RetrieveToken);
            //admAuth.returnToken += RetrieveToken;
        }

        

        #region Event Handlers

        private void translator_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            var client = new WebClient();
            lock (client)
            {
                client.OpenReadCompleted += ((s, args) =>
                    {
                        FrameworkDispatcher.Update();
                        SoundEffect se = SoundEffect.FromStream(args.Result);
                        lock (se)
                        {
                            se.Play();
                        }
                    }
                );
                client.OpenReadAsync(new Uri(e.Result));
            }
        }

        #endregion

        #region Public

        public void SpeakText(string text)
        {
            if(String.IsNullOrWhiteSpace(_quality))
                _quality = "MinSize";
            if (!String.IsNullOrWhiteSpace(text))
            {
                lock (_textToSpeak)
                {
                    _textToSpeak = text;
                }
                admAuth.GetToken();
                _action = Action.Speak;
            }
        }

        #endregion

        #region Private Methods

        private void HookupEvents()
        {
            _serviceClient.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(translator_SpeakCompleted);

        }

        private void RetrieveToken(AdmAccessToken token)
        {
            _token = token;
            PerformAction();
        }

        private void PerformAction()
        {
            if (_action == Action.Speak)
            {
                string headerValue = "Bearer " + _token.access_token;
                _serviceClient.SpeakAsync(headerValue, _textToSpeak, _language, "audio/wav", _quality);
            }
        }

        #endregion
    }
}

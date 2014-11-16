﻿using Bugsnag.Core.Payload;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Reflection;

namespace Bugsnag.Core
{
    public class Notifier
    {
        public const string Name = ".NET Bugsnag Notifier (ALPHA)";
        public const string Url = "https://bugsnag.com";

        public static readonly string Version;

        private static readonly IWebProxy DetectedProxy;
        private static readonly JsonSerializerSettings JsonSettings;

        private Configuration Config { get; set; }
        private NotificationFactory Factory { get; set; }

        static Notifier()
        {
            Version = Assembly.GetExecutingAssembly()
                              .GetName()
                              .Version.ToString(3);

            DetectedProxy = WebRequest.DefaultWebProxy;
            JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public Notifier(Configuration config)
        {
            Config = config;
            Factory = new NotificationFactory(config);
        }

        public void Send(Event error)
        {
            var notification = Factory.CreateFromError(error);
            if (notification != null)
                Send(notification);
        }

        private void Send(Notification notification)
        {
            //  Post JSON to server:
            var request = WebRequest.Create(Config.FinalUrl);

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.Proxy = DetectedProxy;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(notification, JsonSettings);
                streamWriter.Write(json);
                streamWriter.Flush();
            }
            request.GetResponse().Close();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace cdb.virtualwalkthrough
{
    public static class GlobalConstants
    {




        internal static bool isAdmin = false;

        /// <summary>
        /// dictinary to store scene objects
        /// </summary>
        internal static Dictionary<string, Vector3> SCENE_OBJECTS= new Dictionary<string, Vector3>();

        internal static Dictionary<string, Action> AppActions = new Dictionary<string, Action>();
        internal static Dictionary<string, AgendaDetails> Agendas = new Dictionary<string, AgendaDetails>();

        internal static readonly string FEEDBACK_LINK = "";
        internal static Joystick movement, rotation;

        //Use this region of initialising Editor fields
#if UNITY_EDITOR

        /// <summary>
        /// link for resource
        /// </summary>
        internal static readonly string RESOURCES_URL="";

        /// <summary>
        /// link for media server
        /// </summary>
        internal static readonly string LOCAL_MEDIA_SERVER_URL="http://localhost:8888/";
        internal static readonly string MEDIA_SERVER_URL = "https://virtual-walkthrough-admin.s3.ap-south-1.amazonaws.com/";
        /*"https://virtualwalkthrough.s3.us-east-2.amazonaws.com/";*/
#else
        internal static readonly string RESOURCES_URL="";
        internal static readonly string LOCAL_MEDIA_SERVER_URL="";
                internal static readonly string MEDIA_SERVER_URL = "https://virtual-walkthrough-admin.s3.ap-south-1.amazonaws.com/";
        /*"https://virtualwalkthrough.s3.us-east-2.amazonaws.com/";*/
        //internal static readonly string MEDIA_SERVER_URL = "https://virtualwalkthrough.s3.us-east-2.amazonaws.com/";
#endif
    }

    public class AgendaDetails
    {
        public string AgendaSpeakers { get;  set; }
        public string AgendaTitle { get;  set; }
        public string ClientNames { get;  set; }
        public string Details{ get;  set; }

    }
}


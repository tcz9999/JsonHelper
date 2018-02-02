using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace chatServer
{
    /// <summary>
    /// server 的摘要说明
    /// </summary>
    public class server : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request["method"];
            if (method=="sendMsg")
            {
                if(context.Request["msg"] == "$clear")
                {
                    MessageList.clear();
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("清空成功");
                    return;
                }
                string strToWho = context.Request["toWho"];
                string[] arrToWho =  strToWho.Split(',');
                foreach( string toWho in arrToWho)
                {
                    MessageList.sendMsg(context.Request["userName"],toWho, context.Request["msg"]);
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("发送成功");
            }
            else if (method =="getMsg"){
                List<Message> msg;
                msg = MessageList.getMsg(context.Request["userName"]);
                if(msg != null)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonHelper.JsonSerializer<List<Message>>(msg));
                }
            }
            //StreamReader sr = new StreamReader(context.Request.InputStream);
            //string strJson = sr.ReadToEnd();
            //context.Response.Write(strJson);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
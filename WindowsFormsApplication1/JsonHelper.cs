using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;

namespace WindowsFormsApplication1
{
    public class JsonHelper
    {
        private const string OBJ_PATTERN = @"(?<=^\s*{\s*"").*?(?=""\s*:)";
        private const string ARR_PATTERN = @"(?<=^\s*[\s*"").*?(?=""\s*:)";
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
        public static Object Deserialize(string json)
        {
            if(string.IsNullOrWhiteSpace(json))
                return null;
            JsonParse jt = new JsonParse(json);
            return jt.getResult();
        }
    }
    public class JsonParse
    {
        public JsonParse(string json)
        {
            _s = json;
            _pos = 0;
        }
        private string _s;
        private int _pos;
        private Stack _st = new Stack();
        private const string KEYS = @"{}[]"":\";
        public string JsonString
        {
            get { return _s; }
            set {
                _pos = 0;
                _s = value.Trim(); ;
            }
        }
        
        public Object getResult()
        {
            _pos = 0;
            _st.Clear();
            if (string.IsNullOrWhiteSpace(_s))
                return null;
            if (_s.StartsWith("{") && _s.EndsWith("}"))
            {
                _st.Push('{');
                _pos++;
                return getObj();
            }
            if (_s.StartsWith("[") && _s.EndsWith("]"))
            {
                return getArr();
            }
            throw new Exception("不是正确的JSON字符串");
        }
        public Object getObj()
        {
            Object obj = null;
            while (_pos < _s.Length)
            {
                switch (_s[_pos])
                {
                    case '}':
                        if (_st.Pop().Equals('{'))
                        {
                            _pos++;
                            return obj;
                        }
                        else
                        {
                            throw new Exception("不是正确的JSON字符串");
                        }
                    case '"':
                        _pos++;
                        string key = getString();
                        getColon();
                        break;                        
                    default:
                        _pos++;
                        break;
                        
                }
            }
            return null;
        }
        public Array getArr()
        {
            _pos = 1;
            if ("arr"=="")
            {
                getArr();
            }
            return null;
        }
        public string getString()
        {
            string ret = string.Empty;
            int pStart = _pos;
            while (_pos < _s.Length)
            {
                switch (_s[_pos])
                {
                    case '\\':
                        _pos += 2;
                        break;
                    case '"':
                        _pos++;
                        ret = _s.Substring(pStart, _pos - pStart);
                        try
                        {
                            ret = Regex.Unescape(ret);
                        }
                        catch(Exception ex)
                        {
                            throw new Exception(string.Format("不是正确的JSON字符串,错误位置{0}",_pos));
                        }
                        return ret;
                    default:
                        _pos++;
                        break;
                }
            }
            throw new Exception(string.Format("不是正确的JSON字符串,错误位置{0}", _pos));
        }
        public void getColon()
        {

        }
    }
}
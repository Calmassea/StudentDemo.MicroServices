using StudentDemo.DominStd.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentDemo.DominStd.Response
{
    /// <summary>
    /// 响应根数据模型
    /// </summary>
    public  class ServiceResult
    {
        public ServiceResultCode Code { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public void IsSuccess(string ip,string port)
        {
            Code = ServiceResultCode.Success;
            Ip = ip;
            Port = port;
        }
        public void IsFailed(string ip, string port)
        {
            Code = ServiceResultCode.Failed;
            Ip = ip;
            Port = port;
        }
    }
}

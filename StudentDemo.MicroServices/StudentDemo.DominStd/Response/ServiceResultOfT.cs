using StudentDemo.DominStd.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentDemo.DominStd.Response
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public class ServiceResultOfT<T>:ServiceResult where T:class
    {
        /// <summary>
        /// 实体
        /// </summary>
        public T Entities { get; set; }
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        public void IsSuccess(string ip, string port,T entities)
        {
            Code = ServiceResultCode.Success;
            Entities = entities;
            Ip = ip;
            Port = port;
        }
    }
}

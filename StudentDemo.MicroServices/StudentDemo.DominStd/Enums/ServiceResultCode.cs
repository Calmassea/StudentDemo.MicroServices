using System;
using System.Collections.Generic;
using System.Text;

namespace StudentDemo.DominStd.Enums
{
    /// <summary>
    /// 响应码
    /// </summary>
    public  enum ServiceResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success=0,
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        /// <summary>
        /// 未认证
        /// </summary>
        Unauthorized,
    }
}

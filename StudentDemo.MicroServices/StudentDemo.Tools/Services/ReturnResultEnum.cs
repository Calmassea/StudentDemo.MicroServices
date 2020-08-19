using System;
using System.Collections.Generic;
using System.Text;

namespace StudentDemo.Tools.Services
{
    public class ReturnResultEnum
    {
        /// <summary>
        /// 返回值枚举
        /// </summary>
        public enum EnumResult
        {
            /// <summary>
            /// 成功
            /// </summary>
            success = 1,
            /// <summary>
            /// 失败
            /// </summary>
            Fail,
            /// <summary>
            /// 键值为空
            /// </summary>
            KeyIsNull,
            /// <summary>
            /// 传入的某个类型的数据为空
            /// </summary>
            DataIsNull,
            /// <summary>
            /// 传入的数据已存在
            /// </summary>
            DataIsExist


        }
    }
}

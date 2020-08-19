using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static StudentDemo.Tools.Services.ReturnResultEnum;

namespace StudentDemo.Tools.Services
{
    /// <summary>
    /// 通用服务接口
    /// </summary>
    public interface ICommonService
    {
        public EnumResult SaveAll();

        //专门处理数据列表的接口
        #region 泛型操作接口
        public IEnumerable<T> GetAll<T>() where T : class;
        public IEnumerable<T> GetAllNoTracking<T>() where T : class;
        public T Find<T>(int id) where T : class;
        public T Find<T>(string id) where T : class;
        public EnumResult Add<T>(T write) where T : class;

        public EnumResult AddRangeIE<T>(IEnumerable<T> Entities) where T : class;

        public EnumResult Romve<T>(int? id) where T : class;

        public EnumResult Romve<T>(string id) where T : class;

        public EnumResult Romve<T>(T t) where T : class;

        public EnumResult Edit<T>(T edit) where T : class;


        public IQueryable<T> Query<T>(Expression<Func<T, bool>> expressionWhere) where T : class;

        public T QueryOnly<T>(Expression<Func<T, bool>> expressionWhere, bool track) where T : class;
        #endregion
    }
}

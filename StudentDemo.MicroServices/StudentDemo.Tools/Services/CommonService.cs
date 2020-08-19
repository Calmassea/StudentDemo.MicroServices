using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static StudentDemo.Tools.Services.ReturnResultEnum;

namespace StudentDemo.Tools.Services
{
    public class CommonService<TDbcontext>:ICommonService where TDbcontext:DbContext
    {
        //创建私有只读类型为app数据模型的变量来存储将要注入的数据
        public TDbcontext _context { get; set; }
        public CommonService(TDbcontext dbcontext)
        {
            _context = dbcontext;
        }
        
        public EnumResult SaveAll()
        {
            try
            {
                _context.SaveChanges();
                return EnumResult.success;
            }
            catch (Exception)
            {
                return EnumResult.Fail;
            }
        }
        #region 泛型操作接口

        /// <summary>
        /// 查询某个表的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class => _context.Set<T>();
        /// <summary>
        /// 查询某个表的全部数据不跟踪对象，用于只显示不修改实体的情况
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllNoTracking<T>() where T : class => _context.Set<T>().AsNoTracking<T>();


        //(T)Convert.ChangeType(_context.Set<T>(), typeof(T));

        /// <summary>
        /// !注意Find<>只能查主键
        /// 如需查找其他属性需用Findxx<>
        /// 删除某个记录键值Id为id的
        /// </summary>
        /// <typeparam name="T">表对象</typeparam>
        /// <param name="id">要查询的键值id</param>
        /// <returns></returns>
        public T Find<T>(int id) where T : class => _context.Set<T>().Find(id);
        /// <summary>
        /// !注意Find<>只能查主键
        /// 如需查找其他属性需用Findxx<>
        /// Find重载
        /// 删除某个记录键值Id为id的
        /// </summary>
        /// <typeparam name="T">表对象</typeparam>
        /// <param name="id">要查询的键值id</param>
        /// <returns></returns>
        public T Find<T>(string id) where T : class => _context.Set<T>().Find(id);
       
        /// <summary>
        /// 向T表中添加Write记录
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="write">写入的某条记录</param>
        /// <returns></returns>
        public EnumResult Add<T>(T write) where T : class
        {
            if (write is null)
            {
                return EnumResult.KeyIsNull;
            }
            _context.Set<T>().Add(write);
            return SaveAll();
        }
        /// <summary>
        /// 添加多个实体或者说是元组
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Entities">实体列表</param>
        /// <returns></returns>
        public EnumResult AddRangeIE<T>(IEnumerable<T> Entities) where T : class
        {
            if (Entities != null)
            {
                try
                {
                    _context.Set<T>().AddRange(Entities);
                    return SaveAll();
                }
                catch (Exception ex)
                {
                    return EnumResult.Fail;
                }

            }
            return SaveAll();
        }
        /// <summary>
        /// 删除某个记录键值Id为id的
        /// </summary>
        /// <typeparam name="T">表对象</typeparam>
        /// <param name="id">要查询的键值id</param>
        /// <returns></returns>
        public EnumResult Romve<T>(int? id) where T : class
        {
            if (id is null)
            {
                return EnumResult.KeyIsNull;
            }

            _context.Set<T>().Remove(_context.Set<T>().Find(id));
            return SaveAll();
        }
        /// <summary>
        /// 删除某个记录键值Id为id的
        /// </summary>
        /// <typeparam name="T">表对象</typeparam>
        /// <param name="id">要查询的键值id</param>
        /// <returns></returns>
        public EnumResult Romve<T>(string id) where T : class
        {
            if (string.IsNullOrEmpty(id))
            {
                return EnumResult.KeyIsNull;
            }

            _context.Set<T>().Remove(_context.Set<T>().Find(id));
            return SaveAll();
        }
        /// <summary>
        /// 从数据库中删除某个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public EnumResult Romve<T>(T t) where T : class
        {
            if (t != null)
            {
                _context.Set<T>().Remove(t);
                return SaveAll();
            }
            return EnumResult.Fail;
        }
        /// <summary>
        /// 更新edit这一条记录
        /// </summary>
        /// <typeparam name="T">表对象</typeparam>
        /// <param name="edit">要更新的记录</param>
        /// <returns></returns>
        public EnumResult Edit<T>(T edit) where T : class
        {
            if (edit is null)
            {
                return EnumResult.DataIsNull;
            }
            try
            {
                var row = _context.Set<T>().Attach(edit);
                row.State = EntityState.Modified;
                return SaveAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return EnumResult.Fail;
            }

        }

        /// <summary>
        /// 查询某个符合条件的记录
        /// </summary>
        /// <typeparam name="T">表对象</typeparam>
        /// <param name="expressionWhere">lambda表达式</param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> expressionWhere) where T : class
        {
            return _context.Set<T>().Where(expressionWhere);
        }
        /// <summary>
        /// 查询符合条件得一个记录
        /// </summary>
        /// <typeparam name="T">数据表模型</typeparam>
        /// <param name="expressionWhere">Lambda表达式</param>
        /// <param name="track">是否为追踪数据(可直接修改)</param>
        /// <returns></returns>
        public T 
            QueryOnly<T>(Expression<Func<T, bool>> expressionWhere, bool track) where T : class
        {
            if (track)
            {
                return _context.Set<T>().FirstOrDefault(expressionWhere);
            }
            else
            {
                return _context.Set<T>().AsNoTracking().FirstOrDefault(expressionWhere);
            }

        }

        #endregion
    }
}

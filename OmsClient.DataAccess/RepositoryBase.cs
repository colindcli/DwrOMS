using Dapper;
using ExecuteSqlBulk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess
{
    public abstract class RepositoryBase
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ToString();

        /// <summary>
        /// 执行查询（有返回值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T Db<T>(Func<SqlConnection, T> func)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var result = func(conn);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        /// <summary>
        /// 执行查询（没返回值）
        /// </summary>
        /// <param name="action"></param>
        protected void Db(Action<SqlConnection> action)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                action(conn);
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 执行事务查询（有返回值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T Db<T>(Func<SqlConnection, SqlTransaction, T> func)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    var result = func(conn, tran);
                    tran.Commit();
                    conn.Close();
                    conn.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// 执行事务查询（有返回值）
        /// </summary>
        /// <param name="action"></param>
        protected void Db(Action<SqlConnection, SqlTransaction> action)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    action(conn, tran);
                    tran.Commit();
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }

        #region 操作

        /// <summary>
        /// 添加（主键int型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public int Add<T>(T m)
        {
            return Db(db => db.Insert<int, T>(m));
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public TReturn Add<TReturn, T>(T m)
        {
            return Db(db => db.Insert<TReturn, T>(m));
        }

        /// <summary>
        /// 添加（异步）
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public Task<TReturn> AddAsync<TReturn, T>(T m)
        {
            return Task.Run(() => Db(db => db.Insert<TReturn, T>(m)));
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(object id)
        {
            return Db(db => db.Get<T>(id));
        }

        /// <summary>
        /// 获取一条数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(object id)
        {
            return Task.Run(() => Db(db => db.Get<T>(id)));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public List<T> GetList<T>(object whereConditions = null)
        {
            if (whereConditions == null)
            {
                return Db(db => db.GetList<T>().ToList());
            }
            if (whereConditions is string)
            {
                return Db(db => db.GetList<T>(whereConditions.ToString()).ToList());
            }
            return Db(db => db.GetList<T>(whereConditions).ToList());
        }

        /// <summary>
        /// 获取数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public Task<List<T>> GetListAsync<T>(object whereConditions = null)
        {
            if (whereConditions == null)
            {
                return Task.Run(() => Db(db => db.GetList<T>().ToList()));
            }
            if (whereConditions is string)
            {
                return Task.Run(() => Db(db => db.GetList<T>(whereConditions.ToString()).ToList()));
            }
            return Task.Run(() => Db(db => db.GetList<T>(whereConditions).ToList()));
        }

        /// <summary>
        /// 批量获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public List<T> GetListByBulk<T>(object whereConditions)
        {
            return Db(db => db.GetListByBulk<T>(whereConditions).ToList());
        }

        /// <summary>
        /// 批量获取数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public Task<List<T>> GetListByBulkAsync<T>(object whereConditions)
        {
            return Task.Run(() => Db(db => db.GetListByBulk<T>(whereConditions).ToList()));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Update<T>(T m)
        {
            return Db(db => db.Update(m)) > 0;
        }

        /// <summary>
        /// 更新（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public Task<bool> UpdateAsync<T>(T m)
        {
            return Task.Run(() => Db(db => db.Update(m)) > 0);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Delete<T>(T m)
        {
            return Db(db => db.Delete(m)) > 0;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync<T>(T m)
        {
            return Task.Run(() => Db(db => db.Delete(m)) > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereConditions">new {}</param>
        /// <returns></returns>
        public int DeleteList<T>(object whereConditions)
        {
            return Db(db => db.DeleteList<T>(whereConditions));
        }

        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereConditions">new {}</param>
        /// <returns></returns>
        public Task<int> DeleteListAsync<T>(object whereConditions)
        {
            return Task.Run(() => Db(db => db.DeleteList<T>(whereConditions)));
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void BulkInsert<T>(List<T> list)
        {
            Db(db => db.BulkInsert(list));
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TUpdate"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="list"></param>
        /// <param name="columnUpdateExpression"></param>
        /// <param name="columnPrimaryKeyExpression"></param>
        /// <returns></returns>
        public int BulkUpdate<T, TUpdate, TPk>(List<T> list, Expression<Func<T, TUpdate>> columnUpdateExpression, Expression<Func<T, TPk>> columnPrimaryKeyExpression) where T : new()
        {
            return Db(db => db.BulkUpdate(list, columnUpdateExpression, columnPrimaryKeyExpression));
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="list"></param>
        /// <param name="columnPrimaryKeyExpression"></param>
        /// <returns></returns>
        public int BulkDelete<T, TPk>(List<T> list, Expression<Func<T, TPk>> columnPrimaryKeyExpression) where T : new()
        {
            return Db(db => db.BulkDelete(list, columnPrimaryKeyExpression));
        }

        /// <summary>
        /// 批量删除 (重置)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void BulkDeleteAll<T>()
        {
            Db(db => db.BulkDelete<T>());
        }

        #endregion
    }

    public static class RepositoryExtension
    {
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="tran"></param>
        public static void Update<T>(this SqlConnection db, object id, Action<T> action, SqlTransaction tran = null)
        {
            var obj = db.Get<T>(id, tran);
            if (obj == null)
            {
                return;
            }

            action.Invoke(obj);
            db.Update(obj, tran);
        }
    }
}

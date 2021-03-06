using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.Common;
using ykPermission.Common;
using System.Data;
using System.Collections;

namespace ykPermission.Dao
{
    public class MasterDao : BaseDao, IMasterDao
    {
        /// 用户列表
        /// </summary>
        public int GetMasterList(Pager p, Hashtable hs)
        {
            string sql = "select * from T_Master";
            IDbParameters param = AdoTemplate.CreateDbParameters();
            sql = PagerSql(sql, p);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, param);
            p.DataSource = ds.Tables[0];
            p.ItemCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            return 0;
        }
        /// 资源列表
        /// </summary>
        public DataTable GetActionList(Hashtable hs)
        {
            string sql = "select * from T_Action where 1=1 ";
            IDbParameters param = AdoTemplate.CreateDbParameters();
            if (hs.Contains("ParentCode"))
            {
                sql += " and ParentCode=@ParentCode";
                param.AddWithValue("ParentCode", hs["ParentCode"]);
            }
            if (hs.Contains("Disabled"))
            {
                sql += " and Disabled=@Disabled ";
                param.AddWithValue("Disabled", hs["Disabled"]);
            }
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, param);
            return ds.Tables[0];
        }
        /// 角色列表
        /// </summary>
        public int GetGroupList(Pager p, Hashtable hs)
        {
            string sql = @"select a.*,b.MasterName from T_Group a
                            left join T_Master b on b.ID=a.CreateBy";
            IDbParameters param = AdoTemplate.CreateDbParameters();
            sql = PagerSql(sql, p);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, param);
            p.DataSource = ds.Tables[0];
            p.ItemCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            return 0;
        }
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public DataTable GetMasterAction(int MasterID)
        {
            string sql = @"select * from t_action where Code in
                            (
                                select ActionCode from T_ActionGroup  where GroupID in 
                                (select GroupID from T_MasterGroup where MasterID=@MasterID )
                            )";
            IDbParameters param = AdoTemplate.CreateDbParameters();
            param.AddWithValue("MasterID", MasterID);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, param);
            return ds.Tables[0];
        }
    }
}

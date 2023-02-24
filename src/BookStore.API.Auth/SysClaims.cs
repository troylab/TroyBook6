using System;
namespace BookStore.API.Auth;

/// <summary>
/// 定義一個系統被權限保護的資源(Resources)與可以被操作的動作(Actions)
/// </summary>
public class SysClaims
{
    public const string PermissionManage = "權限管理";
    public const string BookManage = "書本管理";
    public const string OrderManage = "訂單管理";

    /*
     * 設定 system actions
     */
    public static class Values
    {
        public const string Create = "新增";
        public const string Read = "查看";
        public const string Update = "編輯";
        public const string Delete = "刪除";
        public const string Export = "匯出";
    }

    public static List<(string ClaimType, string[] ClaimValues)> GetAll()
    {
        /*
         * 設定所有的 system resources 以及預設 resource 可以被允許執行的 actions
         */

        return new List<(string ClaimType, string[] ClaimValues)>
        {
            ( PermissionManage, new []{ Values.Read, Values.Update } ),
            ( BookManage, Array.Empty<string>() ),
            ( OrderManage, Array.Empty<string>() ),
        };
    }
}


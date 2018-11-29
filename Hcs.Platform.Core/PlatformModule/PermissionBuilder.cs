using System;
using System.Collections.Generic;
using Hcs.Platform.Odata;

namespace Hcs.Platform.PlatformModule
{
    [System.Serializable]
    public class PermissionBuilder : IPlatformRole
    {
        public IEnumerable<string> Roles => roles;
        public IEnumerable<IOdataQueryPermission> QueryPermissions => queryPermissions;

        public string Code => $"{Function.Code}.{PermissionCode}";
        public string PermissionCode { get; }
        public ModelFunctionBuilder Function { get; }

        List<string> roles = new List<string>();
        List<IOdataQueryPermission> queryPermissions = new List<IOdataQueryPermission>();
        internal PermissionBuilder(string code, ModelFunctionBuilder function)
        {
            PermissionCode = code;
            Function = function;
        }
        public PermissionBuilder AddRole(Role.IRoleToken token)
        {
            roles.Add(token.Name);
            return this;
        }
        public PermissionBuilder AddRole(string role)
        {
            roles.Add(role);
            return this;
        }
        public PermissionBuilder AddOdataPermission<TEntity>(Action<OdataQueryPermissionBuilder<TEntity>> build)
        {
            var builder = new OdataQueryPermissionBuilder<TEntity>();
            build(builder);
            queryPermissions.Add(builder);
            return this;
        }
    }
}
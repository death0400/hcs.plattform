using System;
using System.Collections.Generic;
using System.Linq;

namespace Hcs.Platform.PlatformModule
{
    public static class StandardRoles
    {
        public const string Create = nameof(Create);
        public const string Delete = nameof(Delete);
        public const string Modify = nameof(Modify);
        public const string View = nameof(View);
    }
    public class ModelFunctionBuilder : IPlatformFunction
    {
        internal ModelFunctionBuilder(string code)
        {
            Code = code;
        }

        public string Code { get; }
        List<PermissionBuilder> permissions = new List<PermissionBuilder>();
        internal IReadOnlyList<PermissionBuilder> Permissions => permissions;

        IEnumerable<IPlatformRole> IPlatformFunction.Permissions => Permissions;

        public ModelFunctionBuilder AddPermission(string code, Action<PermissionBuilder> build)
        {
            var builder = new PermissionBuilder(code, this);
            build(builder);
            permissions.Add(builder);
            return this;
        }
    }
}
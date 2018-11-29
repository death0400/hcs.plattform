using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Hcs.Platform.Abstractions.Role;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Core;
using Hcs.Platform.Data;
using Hcs.Platform.User;
using Microsoft.EntityFrameworkCore;
namespace Hcs.PlatformModule.Basic.Pipes
{
    public static class PlatformGroupUserPipes
    {
        public static Func<IPrincipal, IPlatformUser, EntityValidationResult<PlatformUser>, EntityValidationResult<PlatformUser>> ValidateUserDelete = (principal, currentUser, c)
          =>
        {
            var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
            if (c.Data.CreatedBy != currentUser.Id)
            {
                AddPermissionError(c);
            }
            return c;
        };

        private static void AddPermissionError<T>(EntityValidationResult<T> c) where T : class
        {
            c.AddError(new ValidationError
            {
                Title = "Id",
                Message = "no permission for this entity"
            });
        }

        public static Action<IPrincipal, IPlatformUser, EntityValidationResult<PlatformUser>> ValidateUser = (principal, currentUser, c) =>
        {
            var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
            if (!isAdmin)
            {
                if (c.Data.CreatedBy != currentUser.Id && c.Data.Id != currentUser.Id)
                {
                    AddPermissionError(c);
                }
            }
        };
        public static Action<IPrincipal, IPlatformUser, EntityValidationResult<PlatformGroup>> ValidateGroup = (principal, currentUser, c) =>
           {
               var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
               if (!isAdmin)
               {
                   if (c.Data.CreatedBy != currentUser.Id && c.Data.Id != currentUser.Id)
                   {
                       AddPermissionError(c);
                   }
               }
           };
        public static Func<IPrincipal, IPlatformUser, IQueryable<PlatformUser>, IQueryable<PlatformUser>> FilterUser = (principal, currentUser, source) =>
        {
            var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
            if (!isAdmin)
            {
                return source.Where(x => x.CreatedBy == currentUser.Id || x.Id == currentUser.Id);
            }

            return source;
        };
        public static Func<IPrincipal, IPlatformUser, IQueryable<PlatformGroup>, IQueryable<PlatformGroup>> FilterGroup = (principal, currentUser, source) =>
          {
              var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
              if (!isAdmin)
              {
                  return source.Where(x => x.CreatedBy == currentUser.Id);
              }

              return source;
          };
        public static Func<IPrincipal, IPlatformUser, IQueryable<PlatformUserGroup>, IQueryable<PlatformUserGroup>> FilterUserGroup = (principal, currentUser, source) =>
        {
            var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
            if (!isAdmin)
            {
                return source.Where(x => x.PlatformGroup.PlatformUserGroups.Any(y => y.PlatformUserId == currentUser.Id) || x.PlatformGroup.CreatedBy == currentUser.Id);
            }

            return source;
        };
        public static Func<IPrincipal, IPlatformUser, IQueryable<PlatformGroupRole>, IQueryable<PlatformGroupRole>> FilterRoles = (principal, currentUser, source) =>
         {
             var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
             if (!isAdmin)
             {
                 return source.Where(x => x.PlatformGroup.PlatformUserGroups.Any(y => y.PlatformUserId == currentUser.Id) || x.PlatformGroup.CreatedBy == currentUser.Id);
             }

             return source;
         };
    }
}
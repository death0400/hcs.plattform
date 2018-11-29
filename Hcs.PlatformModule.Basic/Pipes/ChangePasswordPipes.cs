using System;
using System.Threading.Tasks;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Data;
using Hcs.Platform.Flow;
using Hcs.Platform.User;
using Hcs.PlatformModule.Basic.Views;
using Microsoft.EntityFrameworkCore;

namespace Hcs.PlatformModule.Basic.Pipes
{
    public static class ChangePasswordPipes
    {
        public static Func<DbContext, UpdateData<PlatformUser, PlatformDbContext>, GetKeyAndModelOutput<long, ChangePasswordView>, Task> ChangePassword = async (ctx, upd, output) =>
          {
              var pu = await ctx.Set<PlatformUser>().FindAsync(output.Key);
              pu.Password = output.Entity.Password;
              await upd.Run(pu);
          };
        public static Action<IPlatformUser, EntityValidationResult<GetKeyAndModelOutput<long, ChangePasswordView>>> ValidateUser = (user, vctx) =>
       {
           if (user.Id != vctx.Data.Entity.Id)
           {
               vctx.AddError(new Platform.Core.ValidationError
               {
                   Title = "ChangePassword",
                   Message = "No Permission"
               });
           }
       };
        public static Func<DbContext, EntityValidationResult<GetKeyAndModelOutput<long, ChangePasswordView>>, Task> ValidateOldPassword = async (dbctx, vctx) =>
         {
             var usr = await dbctx.Set<PlatformUser>().FindAsync(vctx.Data.Key);
             if (usr.Password != vctx.Data.Entity.OldPassword)
             {
                 vctx.AddError(new Platform.Core.ValidationError
                 {
                     Title = "OldPassword",
                     Message = "OldPasswordError"
                 });
             }
         };
    }
}
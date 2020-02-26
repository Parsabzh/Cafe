using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Cafe7.Infrastructure.ExceptionHelper;
using Cafe7.Model;
using Cafe7.Service.BackUp.Messaging.Request;
using Cafe7.Service.BackUp.Messaging.Response;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Cafe7.Service.BackUp.Implementation
{
    public class BackUpService
    {
        public BackupResponse Backup(BackupRequest request)
        {
            try
            {

                var dbServer = new Server(new ServerConnection("."));
                var dBackup = new Backup( );
                dBackup.Action = BackupActionType.Database;
                dBackup.Database = "DbCafeManagment";
                dBackup.Devices.AddDevice(@"D:\Data\Db.bak", DeviceType.File);
                dBackup.Initialize = false;
                dBackup.SqlBackup(dbServer);
                return new BackupResponse()
                {
                    Message = "Backup completed",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new BackupResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }
        public RestoreResponse Restore(RestoreRequest request)
        {
            try
            {

                var dbServer = new Server(new ServerConnection("."));
                var dbRestore = new Restore();
                dbRestore.Database = "DbCafeManagment";
                dbRestore.Action = RestoreActionType.Database;
                dbRestore.Devices.AddDevice(@"D:\Data\Db.bak", DeviceType.File);
                dbRestore.ReplaceDatabase = true;
                dbRestore.NoRecovery = false;
                dbRestore.SqlRestoreAsync(dbServer);
                return new RestoreResponse()
                {
                    Message = "Restore completed",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new RestoreResponse()
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }
    }
}

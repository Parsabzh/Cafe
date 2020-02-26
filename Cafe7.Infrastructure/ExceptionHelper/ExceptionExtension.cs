using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Cafe7.Infrastructure.ExceptionLocalize;

namespace Cafe7.Infrastructure.ExceptionHelper {
    public static class ExceptionExtension {
        public static IEnumerable<string> GetMessageList(this Exception ex)
        {
            return ex.GetMessages().Split(new[] {'\r', '\n'},
                StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
        }
        public static string GetMessages(this Exception ex) {
            var errors = new StringBuilder();

            do {
                if (ex is DbEntityValidationException)
                    errors.AppendLine(GetMessage(ex as DbEntityValidationException));
                else if (ex is DBConcurrencyException)
                    errors.AppendLine(GetMessage(ex as DBConcurrencyException));
                else if (ex is SqlException)
                    errors.AppendLine(GetMessage(ex as SqlException));
                else if (ex is DbException)
                    errors.AppendLine(GetMessage(ex as DbException));
                else if (ex is UpdateException)
                    errors.AppendLine(GetMessage(ex as UpdateException));
                else if (ex is DbUpdateException)
                    errors.AppendLine(GetMessage(ex as DbUpdateException));
                else if (ex is DataException)
                    errors.AppendLine(GetMessage(ex as DataException));
                else errors.AppendLine(ex.GetType().Name + ": " + ex.Message);
                //else if(ex is DbUpdateConcurrencyException)
                //    errors.AppendLine(GetDbUpdateConcurrencyException(ex as DbUpdateConcurrencyException));

                ex = ex.InnerException;
            } while (ex != null);
            return errors.ToString();
        }

        private static string GetMessage(DbEntityValidationException ex) {
            var errors = new StringBuilder();
#if DEBUG
            errors.AppendLine($"[{ex.GetType().Name}]");
#endif
            errors.AppendLine();
            foreach (var error in ex.EntityValidationErrors
                .SelectMany(validation => validation.ValidationErrors)) {
                errors.AppendLine($"{error.ErrorMessage}");
            }
            return errors.ToString();

        }
        private static string GetMessage(DBConcurrencyException ex) {
            var errors = new StringBuilder($"[{ex.GetType().Name}]");
            errors.AppendLine();
            return errors.ToString();
        }

        private static string GetMessage(SqlException ex) {
            var errors = new StringBuilder($"[{ex.GetType().Name}]");
            errors.AppendLine();
            string[] item;
            switch (ex.Number) {
            case -1:
                errors.AppendLine(ExceptionResource.Sql_N1);
                return errors.ToString();
            case 2:
                errors.AppendLine(ExceptionResource.Sql_2);
                return errors.ToString();
            case 53:
                errors.AppendLine(ExceptionResource.Sql_53);
                return errors.ToString();
            case 201:
                item = ex.Message.Split('\'');
                errors.AppendLine(string.Format(ExceptionResource.Sql_201, item[1], item[3]));
                return errors.ToString();
            case 207:
                item = ex.Message.Split('\'');
                errors.AppendLine(String.Format(ExceptionResource.Sql_207, item[1]));
                return errors.ToString();
            case 208:
                item = ex.Message.Split('\'');
                errors.AppendLine(String.Format(ExceptionResource.Sql_208, item[1]));
                return errors.ToString();
            case 515:
                item = ex.Message.Split('\'');
                errors.AppendLine(String.Format(ExceptionResource.Sql_515, item[1], item[3]));
                return errors.ToString();
            case 547:
                item = ex.Message.Split(' ');
                var parts = ex.Message.Split("'\"".ToCharArray());
                switch (item[1].ToUpper()) {
                case "INSERT":
                    errors.AppendLine(string.Format(ExceptionResource.Sql_547_Insert, parts[1], parts[5], parts[7]));
                    return errors.ToString();
                case "UPDATE":
                    errors.AppendLine(ExceptionResource.Sql_547_Update);
                    return errors.ToString();
                default:
                    errors.AppendLine(ExceptionResource.Sql_547_Delete);
                    return errors.ToString();
                }
            case 2627:
                item = ex.Message.Split("()".ToCharArray());
                errors.AppendLine(String.Format(ExceptionResource.Sql_2627, item[1]));
                return errors.ToString();
            case 2812:
                item = ex.Message.Split('\'');
                errors.AppendLine(String.Format(ExceptionResource.Sql_2812, item[1]));
                return errors.ToString();
            case 3702:
                item = ex.Message.Split('"');
                errors.AppendLine(String.Format(ExceptionResource.Sql_3702, item[1]));
                return errors.ToString();
            case 4060:
                // offline
                // detach
                // invalid name
                errors.AppendLine(ExceptionResource.Sql_4060);
                return errors.ToString();
            case 4121:
                item = ex.Message.Split('"');
                errors.AppendLine(String.Format(ExceptionResource.Sql_4121, item[1], item[3]));
                return errors.ToString();
            case 8114:
                item = ex.Message.Split(' ');
                errors.AppendLine(String.Format(ExceptionResource.Sql_8114, item[4], item[6].Replace(".", "")));
                return errors.ToString();
            case 8144:
                item = ex.Message.Split(' ');
                errors.AppendLine(String.Format(ExceptionResource.Sql_8144, item[3]));
                return errors.ToString();
            case 18456:
                item = ex.Message.Split('\'');
                errors.AppendLine(String.Format(ExceptionResource.Sql_18456, item[1]));
                return errors.ToString();
            default:
                errors.AppendLine($"{ex.Number}: {ex.Message}");
                return errors.ToString();
            }
        }
        private static string GetMessage(UpdateException ex) {
            var errors = new StringBuilder($"[{ex.GetType().Name}]");
            errors.AppendLine();
            // Unable to determine the principal end of the 'Psyco.Pishro.Model.AccountTypeCultureModel_Culture' relationship. Multiple added entities may have the same primary key.
            if (ex.Message.Contains("Multiple added entities may have the same primary key"))
                errors.AppendLine("چندین آیتم با کلید اصلی یکسان به پایگاه داده اضافه شده است");
            // An error occurred while updating the entries. See the inner exception for details.
            else if (ex.Message.Contains("An error occurred while updating the entries. See the inner exception for details"))
                errors.AppendLine("در زمان بروزرسانی پایگاه داده خطایی رخ داده است");
            else errors.AppendLine(ex.Message);

            foreach (var entry in ex.StateEntries) {
                var entityType = entry.Entity.GetType();
                var attributes = entityType.GetCustomAttributes(typeof(DisplayNameForClassAttribute), false);
                var displayAttribute = attributes.Length > 0 ? attributes[0] as DisplayNameForClassAttribute : null;
                var entityName = displayAttribute?.DisplayName ?? entry.Entity.GetType().Name;
                errors.AppendLine($"({entityName} دچار مشکل شده است)");
            }
            return errors.ToString();
        }
        private static string GetMessage(DbUpdateException ex) {
            var errors = new StringBuilder($"[{ex.GetType().Name}]");
            errors.AppendLine();
            // Unable to determine the principal end of the 'Psyco.Pishro.Model.AccountTypeCultureModel_Culture' relationship. Multiple added entities may have the same primary key.
            if (ex.Message.Contains("Multiple added entities may have the same primary key"))
                errors.AppendLine("چندین آیتم با کلید اصلی یکسان به پایگاه داده اضافه شده است");
            // An error occurred while updating the entries. See the inner exception for details.
            else if (ex.Message.Contains("An error occurred while updating the entries. See the inner exception for details"))
                errors.AppendLine("در زمان بروزرسانی پایگاه داده خطایی رخ داده است");
            else errors.AppendLine(ex.Message);

            errors.AppendLine();
            foreach (var entry in ex.Entries) {
                var entityType = entry.Entity.GetType();
                var attributes = entityType.GetCustomAttributes(typeof(DisplayNameForClassAttribute), false);
                var displayAttribute = attributes.Length > 0 ? attributes[0] as DisplayNameForClassAttribute : null;
                var entityName = displayAttribute?.DisplayName ?? entry.Entity.GetType().Name;
                errors.AppendLine($"({entityName} دچار مشکل شده است)");
                foreach (var error in entry.GetValidationResult().ValidationErrors) {
                    errors.AppendLine($"{error.PropertyName}: {error.ErrorMessage}");
                }
            }
            return errors.ToString();
        }
        private static string GetMessage(DbException ex) {
            var errors = new StringBuilder($"[{ex.GetType().Name}]");
            errors.AppendLine();
            switch (ex.ErrorCode) {
            default:
                errors.AppendLine($"{ex.ErrorCode}: {ex.Message}");
                return errors.ToString();
            }
        }
        private static string GetMessage(DataException ex) {
            // An exception occurred while initializing the database. See the InnerException for details.
            var errors = new StringBuilder($"[{ex.GetType().Name}]");
            errors.AppendLine();
            if (ex.Message.Contains("An exception occurred while initializing the database"))
                errors.AppendLine("در زمان مقداردهی اولیه به پایگاه داده خطایی رخ داده است");
            else
                errors.AppendLine(ex.Message);
            return errors.ToString();

        }
    }
}

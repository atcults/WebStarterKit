using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Common.Helpers;
using Common.Service.Impl;

namespace Core.Commands
{
    public interface ICommandRecorder
    {
        void Record<TCommand>(TCommand command, Guid userId, string userName) where TCommand : ICommand;
    }

    public class CommandRecorder : ICommandRecorder
    {
        public void Record<TCommand>(TCommand command, Guid userId, string userName) where TCommand : ICommand
        {
            var serializedCommand = SerialzeCommand(command);
            RecordCommand(command.Id, typeof (TCommand).Name, userId, userName, serializedCommand);
        }

        private static void RecordCommand(Guid aggregateId, string commandName, Guid userId, string userName,
            string serializedCommand)
        {
            using (var connection = new SqlConnection(ConfigProvider.GetDatabaseConfig().GetConnectionString()))
            {
                connection.Open();
                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "INSERT INTO [dbo].[AuditedCommand] ([AggregateId], [commandText], [CommandName], [PerformingUserId], [PerformingUserName], [PerformedOn]) SELECT @AggregateId, @commandText, @CommandName, @PerformingUserId, @PerformingUserName, @PerformedOn";
                    dbCommand.Parameters.Add("@AggregateId", SqlDbType.UniqueIdentifier).Value = aggregateId;
                    dbCommand.Parameters.Add("@CommandName", SqlDbType.VarChar).Value = commandName;
                    dbCommand.Parameters.Add("@CommandText", SqlDbType.VarChar).Value = serializedCommand;
                    dbCommand.Parameters.Add("@PerformingUserId", SqlDbType.UniqueIdentifier).Value = userId;
                    dbCommand.Parameters.Add("@PerformingUserName", SqlDbType.VarChar).Value = userName;
                    dbCommand.Parameters.Add("@PerformedOn", SqlDbType.DateTime).Value = SystemTime.Now();

                    dbCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Storing in custom format like below
        ///     CommandType [
        ///     propertykey:
        ///     <value />
        ///     propertykey:
        ///     <value />
        ///     ]
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        private static string SerialzeCommand<TCommand>(TCommand command)
        {
            var sb = new StringBuilder();
            var commandType = command.GetType();
            sb.AppendFormat("{0} [", commandType.Name);

            foreach (var property in commandType.GetProperties())
            {
                var propertyValue = (property.GetValue(command, null) + string.Empty)
                    .Replace("<", "&lt")
                    .Replace(">", "&gt");

                sb.AppendFormat("{0}:<{1}>", property.Name, propertyValue);
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
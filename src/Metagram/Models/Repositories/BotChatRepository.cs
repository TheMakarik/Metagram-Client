namespace Metagram.Models.Repositories;

public class BotChatRepository(
    ILogger<BotChatRepository> logger,
    ISqliteConnectionFactory connectionFactory
    ) : IBotChatRepository
{
    private const string GettingBotChatFromDatabaseLogMessage = "Got bot chat with id: {id}, last_update {last_upate}, last_content {last_content}, from database";

    private const string UpdatingBotChatLogMessage = "Updating bot chat with id {id}: new last_update: {last_update}, new last_content: {last_content}";
    
    public async Task<BotChat> GetAsync(int id)
    {
        const string query = @"
        SELECT 
          bot_chat_id as BotChatId,
          last_update as LastUpdate,
          last_content as LastContent
        FROM bot_chat
        WHERE
           bot_chat_id = @Id";
        
        IDbConnection dbConnection = OpenConnection();
        BotChat result = await dbConnection.QueryFirstAsync<BotChat>(query, new{ Id = id });
        logger.LogDebug(GettingBotChatFromDatabaseLogMessage, result.BotChatId, result.LastUpdate, result.LastContent);
        return result;
    }
    
    public async Task UpdateAsync(BotChat entity)
    {
        const string query = @"
        UPDATE 
            bot_chat
     
        SET 
            last_update = @LastUpdate, 
            last_content = @LastContent
        WHERE 
            bot_chat_id = @BotChatId";
        IDbConnection dbConnection = OpenConnection();
        await dbConnection.ExecuteAsync(query, entity);
        logger.LogDebug(UpdatingBotChatLogMessage, entity.BotChatId, entity.LastUpdate, entity.LastContent);
    }

    public async Task DeleteAsync(int id)
    {
        const string query = @"
        DELETE 
            FROM bot_chat
        WHERE
            bot_chat_id = @Id";

        IDbConnection dbConnection = OpenConnection();
        await dbConnection.ExecuteAsync(query, new { Id = id });

    }

    public async Task CreateAsync(BotChat entity)
    {
        const string query = @"
        INSERT INTO 
            bot_chat(last_update, last_content) 
        VALUES(@LastUpdate, @LastContent)";
        
        IDbConnection dbConnection = OpenConnection();
        await dbConnection.ExecuteAsync(query, entity);
    }
    
    private IDbConnection OpenConnection()
    {
        IDbConnection connection = connectionFactory.GetFactory();
        connection.Open();
        return connection;
    }
}
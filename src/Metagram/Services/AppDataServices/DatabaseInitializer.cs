namespace Metagram.Services.AppDataServices;

public sealed class DatabaseInitializer(
    ISqliteConnectionFactory connectionFactory,
    ILogger<DatabaseInitializer> logger
    ) : IDatabaseInitializer
{

    private const string SqliteCreatingQueryWasExecuted = "Sqlite database creational query was executed";
    private const string SqliteTransactionRollBackExecuted = "Transation's rollback was executed due to exception {name} from {method}";
    
    #if DEBUG
    private const string QueryExecutionTimerLogMessage = "Creational query was executed for {time} milliseconds";
    private readonly Stopwatch _executionTimer = new Stopwatch();
    #endif
    
    private const string CreatingQuery = @"
     CREATE TABLE IF NOT EXISTS bot_chat(
     bot_chat_id INTEGER PRIMARY KEY AUTOINCREMENT,
     last_update TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
     last_content TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS message_type(
    message_type_id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS user(
    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
    bot_chat_id INTEGER NOT NULL,
    telegram_user_id INTEGER NOT NULL UNIQUE,
    username TEXT CHECK(username LIKE '@%'),
    first_name TEXT NOT NULL,
    last_name TEXT NULL,
    bio_text TEXT NULL,
    avatars_path TEXT NULL,
    FOREIGN KEY (bot_chat_id) REFERENCES bot_chat(bot_chat_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS chat(
    chat_id INTEGER PRIMARY KEY AUTOINCREMENT,
    bot_chat_id INTEGER NOT NULL,
    title TEXT NULL,
    telegram_chat_id INTEGER NOT NULL UNIQUE,
    chat_name TEXT NOT NULL,
    chat_type_id INTEGER NOT NULL,
    FOREIGN KEY (bot_chat_id) REFERENCES bot_chat(bot_chat_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS message (
    message_id INTEGER PRIMARY KEY AUTOINCREMENT,
    telegram_message_id INTEGER NOT NULL,
    bot_chat_id INTEGER NOT NULL,
    media_path TEXT NULL,
    edited_at TEXT NULL,
    sent_at TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    message_type_id INTEGER NOT NULL,
    FOREIGN KEY (bot_chat_id) REFERENCES bot_chat(bot_chat_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (message_type_id) REFERENCES message_type(message_type_id) ON DELETE CASCADE ON UPDATE CASCADE
);
  -- TO DO: ADD INDEXES HERE AFTER SMALL DATABASE REBUILDING
";
    public async Task InitializeAsync()
    {
        StartTimer();
        using IDbConnection dbConnection = connectionFactory.GetFactory();
        dbConnection.Open();
        using IDbTransaction transaction = dbConnection.BeginTransaction();
        try
        {
            await dbConnection.ExecuteAsync(CreatingQuery);
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            logger.LogError(SqliteTransactionRollBackExecuted, e.Message, e.TargetSite);
        }
       
        logger.LogInformation(SqliteCreatingQueryWasExecuted);
        StopTimer();
        
    }

    private void StartTimer()
    {
        #if DEBUG
        _executionTimer.Start();
        #endif
    }

    private void StopTimer()
    {
        #if DEBUG
        _executionTimer.Stop();
        logger.LogDebug(QueryExecutionTimerLogMessage, _executionTimer.ElapsedMilliseconds);
        #endif
    }
}
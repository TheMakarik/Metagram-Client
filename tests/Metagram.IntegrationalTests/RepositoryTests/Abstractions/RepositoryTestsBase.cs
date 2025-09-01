namespace Metagram.IntegrationalTests.RepositoryTests.Abstractions;

public abstract class RepositoryTestBase : IAsyncLifetime
{
    private const float EmptyAvatarPossibility = 0.3f;
    private const float MessageIsNotEditedPossibility = 0.6f;
    private const int BotChatsCount = 40;
    private const int UsersCount = 12;
    private const int MaxRetriesCount = 5;
    private const int MessageCount = 30000;
    private const int SqliteUniqueConstraintErrorCode = 19;
    private const string DatabaseName = "test.db";
    private const string SqliteConnectionString = $"Data Source={DatabaseName}";
    
    private static HashSet<int> _takenBotChatIds = new HashSet<int>();
    
    private static readonly Faker<BotChat> BotChatFaker = new Faker<BotChat>()
        .RuleFor(static botChat => botChat.LastContent, static faker => faker.Lorem.Text())
        .RuleFor(static botChat => botChat.LastUpdate, static faker => faker.Date.Recent(10));
    
    private static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(static user => user.FirstName, static faker => faker.Name.FirstName())
        .RuleFor(static user => user.Username, static (_, user) => "@" + user.FirstName) //Telegram usernames looks like @durov, @pavel, @monk etc
        .RuleFor(static user => user.BioText, static faker => faker.Lorem.Text())
        .RuleFor(static user => user.BotChatId, static faker => GetAvailableBotChatId(faker))
        .RuleFor(static user => user.LastName, static faker => faker.Name.LastName())
        .RuleFor(static user => user.TelegramUserId, static faker => faker.Random.Long(100000, 999999))
        .RuleFor(static user => user.AvatarsPath, static faker => faker.System.FilePath().OrNull(faker, EmptyAvatarPossibility));

    private static readonly Faker<Chat> ChatFaker = new Faker<Chat>()
        .RuleFor(static user => user.BotChatId, static faker => GetAvailableBotChatId(faker))
        .RuleFor(static chat => chat.ChatName, static faker => faker.Commerce.ProductName())
        .RuleFor(static chat => chat.TelegramChatId, static faker => faker.Random.Long(-10099999999, -10000000))
        .RuleFor(static chat => chat.AvatarsPath, static faker => faker.System.FilePath().OrNull(faker, EmptyAvatarPossibility))
        .RuleFor(static chat => chat.Title, static faker => faker.Lorem.Sentence());

    private static readonly Faker<Message> MessageFaker = new Faker<Message>()
        .RuleFor(static message => message.BotChatId, static faker => faker.Random.Int(1, BotChatsCount))
        .RuleFor(static message => message.EditedAt,
            static faker => faker.Date.Recent().OrNull(faker, MessageIsNotEditedPossibility))
        .RuleFor(static message => message.SentAt, static (faker, message) => message.EditedAt is null
            ? faker.Date.Recent()
            : faker.Date.Past(1, message.EditedAt))
        .RuleFor(static message => message.MediaPath, static faker => faker.System.FileName())
        .RuleFor(static message => message.TelegramMessageId, static faker => faker.Random.Long(1, 300000));


    protected IDbConnection DbConnection { get; } = new SqliteConnection(SqliteConnectionString);
    protected ISqliteConnectionFactory Factory { get; }

    protected RepositoryTestBase()
    {
        Factory = A.Fake<ISqliteConnectionFactory>();
        A.CallTo(() => Factory.GetFactory()).Returns(DbConnection);
    }
        
    public async Task InitializeAsync()
    {
        ILogger<DatabaseInitializer> stubLogger = A.Dummy<ILogger<DatabaseInitializer>>();
        IDatabaseInitializer initializer = new DatabaseInitializer(Factory, stubLogger);
        await initializer.InitializeAsync();

        using (DbConnection)
        {
            DbConnection.Open();
            using (IDbTransaction transaction = DbConnection.BeginTransaction())
            {
                try
                {
                    await AddBotChatsAsync();
                    await AddUserAsync();
                    await AddChatsAsync();
                    await AddMessagesAsync();
            
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                } 
            }
        }
    }
    
    public Task DisposeAsync()
    {
        _takenBotChatIds = [];
        return Task.CompletedTask;
    }
    
    private static int GetAvailableBotChatId(Faker faker)
    {
        //Must have enough(not less not bigger) id or unexpected behavior may occured(I REALLY DUNNO WHAT WILL HAPPEN, EXCEPTION OR DEFAULT INT VALUE (0) AND.. infinity loop..?
        IEnumerable<int> availableIds = Enumerable
            .Range(1, BotChatsCount)
            .Where(id => !_takenBotChatIds.Contains(id));

        int id = faker.PickRandom(availableIds);
        _takenBotChatIds.Add(id);
        return id;
    }
    
    private async Task AddBotChatsAsync()
    {
        const string query = "INSERT INTO bot_chat(last_content, last_update) VALUES (@LastContent, @LastUpdate)";
        
        await GenerateAndExecuteQueryAsync(BotChatFaker, BotChatsCount, query);
    }
    
    private async Task AddMessagesAsync()
    {
        const string query = @"
   INSERT INTO 
          message(telegram_message_id, bot_chat_id, media_path, edited_at, sent_at) 
   VALUES(@TelegramMessageId, @BotChatId, @MediaPath, @EditedAt, @SentAt)";
        await GenerateAndExecuteQueryAsync(MessageFaker, MessageCount, query);
    }
    
    private async Task AddUserAsync()
    {
        const string query = @"
    INSERT INTO
      user(bot_chat_id,
         telegram_user_id, 
         username, 
         first_name, 
         last_name,
         bio_text,
         avatars_path)
    VALUES(@BotChatId,
           @TelegramUserId,
           @Username,
           @FirstName,
           @LastName,
           @BioText,
           @AvatarsPath);
         ";

        await GenerateAndExecuteQueryAsync(UserFaker, UsersCount, query);
    }
    
    private async Task AddChatsAsync()
    {
        const string query = @"
         INSERT INTO chat(bot_chat_id,
                chat_name,
                telegram_chat_id,
                avatars_path, 
                title) 
         VALUES(@BotChatId,
                @ChatName,
                @TelegramChatId,
                @AvatarsPath, @Title)";
        await GenerateAndExecuteQueryAsync(ChatFaker, BotChatsCount - UsersCount, query);
    }

    private async Task GenerateAndExecuteQueryAsync<TEntity>(Faker<TEntity> faker, int count, string query, int retriesCount = 0) where TEntity : class
    {
        List<TEntity> entities = faker.Generate(count);
        try
        {
            await DbConnection.ExecuteAsync(query, entities);
        }
        catch (SqliteException e)
        {
            
            if (e.SqliteExtendedErrorCode == SqliteUniqueConstraintErrorCode && retriesCount != MaxRetriesCount)
            {
                retriesCount++;
                //Small possibility, but bogus may generate same values for some rows with unique constraint
                await GenerateAndExecuteQueryAsync(faker, count, query, retriesCount); 
            }
            else
                throw;
        }
    }
}
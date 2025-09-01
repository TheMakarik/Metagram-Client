namespace Metagram.IntegrationalTests.RepositoryTests;

public class BotChatRepositoryTests : RepositoryTestBase
{
    private readonly BotChatRepository _repository;
    private readonly Faker _faker;

    public BotChatRepositoryTests()
    {
        _repository = new BotChatRepository(A.Dummy<ILogger<BotChatRepository>>(), Factory);
        _faker = new Faker();
    }

    [Fact]
    public async Task CreateAsync_Always_AddingEntityToDatabase()
    {
        //Arrnage
        BotChat entity = CreateBotChat();
        //Act
        await _repository.CreateAsync(entity);
        //Assert
        int lastId = GetLastId();
        BotChat result = await _repository.GetAsync(lastId);
        Assert.Multiple( //Do not compare Id 
            () => Assert.Equal(entity.LastContent, result.LastContent),
            () => Assert.Equal(entity.LastUpdate, result.LastUpdate)
        );
    }
    
    [Fact]
    public async Task GetAsync_ExistingEntity_ReturnCorrectEntity()
    {
        //Arrange
        BotChat entity = CreateBotChat();
        await _repository.CreateAsync(entity);
        int lastId = GetLastId();
        //Act
        BotChat result = await _repository.GetAsync(lastId);
        //Assert
        Assert.Equal(lastId, result.BotChatId);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_MustChangeEntityInDatabase()
    {
        //Arrange
        int lastId = GetLastId();
        BotChat entity = await _repository.GetAsync(lastId);
        BotChat updatingValuesForEntity = CreateBotChat();
        updatingValuesForEntity.BotChatId = lastId;
        //Act
        await _repository.UpdateAsync(updatingValuesForEntity);
        //Assert
        BotChat result = await _repository.GetAsync(lastId);
        Assert.Multiple(
            () => Assert.NotEqual(entity.LastContent, result.LastContent),
            () => Assert.NotEqual(entity.LastUpdate, result.LastUpdate),
            () => Assert.Equal(entity.BotChatId, result.BotChatId)
        );
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_ThrowExceptionAfterGettingDeletedRow()
    {
        //Arrange
        int lastId = GetLastId();
        BotChat entity = await _repository.GetAsync(lastId);
        //Act
        await _repository.DeleteAsync(entity.BotChatId);
        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.GetAsync(lastId));
    }

    private BotChat CreateBotChat()
    {
        return new BotChat
        {
            LastContent = _faker.Lorem.Sentence(),
            LastUpdate = _faker.Date.Recent()
        };
    }

    private int GetLastId()
    {
        const string query = "SELECT seq FROM sqlite_sequence WHERE name = 'bot_chat'";
        return DbConnection.ExecuteScalar<int>(query);
    }
}
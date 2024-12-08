#region Abstraction
public interface ISoftwareDeveloperRepository
{
    Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken);
    Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken);
    Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}


public interface ISoftwareDeveloperService
{
    Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken);
    Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken);
    Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}

#endregion


#region Implementation

public class SoftwareDeveloperRepository : ISoftwareDeveloperRepository
{
    private readonly List<SoftwareDeveloper> _developers = new()
    {
        new SoftwareDeveloper { Id = 1, Name = "Ali", Specialization = "Backend", Experience = 5 },
        new SoftwareDeveloper { Id = 2, Name = "Reza", Specialization = "Frontend", Experience = 3 }
    };

    public async Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken); // Simulate async behavior
        return _developers;
    }

    public async Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken); // Simulate async behavior
        return _developers.FirstOrDefault(d => d.Id == id);
    }

    public async Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken); // Simulate async behavior
        developer.Id = _developers.Any() ? _developers.Max(d => d.Id) + 1 : 1;
        _developers.Add(developer);
        return developer;
    }

    public async Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken); // Simulate async behavior
        var existing = _developers.FirstOrDefault(d => d.Id == developer.Id);
        if (existing == null) return null;

        existing.Name = developer.Name;
        existing.Specialization = developer.Specialization;
        existing.Experience = developer.Experience;

        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken); // Simulate async behavior
        var developer = _developers.FirstOrDefault(d => d.Id == id);
        if (developer == null) return false;

        _developers.Remove(developer);
        return true;
    }
}


//

public class SoftwareDeveloperService : ISoftwareDeveloperService
{
    private readonly ISoftwareDeveloperRepository _repository;

    public SoftwareDeveloperService(ISoftwareDeveloperRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken) =>
        _repository.GetAllAsync(cancellationToken);

    public Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _repository.GetByIdAsync(id, cancellationToken);

    public Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken) =>
        _repository.AddAsync(developer, cancellationToken);

    public Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken) =>
        _repository.UpdateAsync(developer, cancellationToken);

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken) =>
        _repository.DeleteAsync(id, cancellationToken);
}


#endregion
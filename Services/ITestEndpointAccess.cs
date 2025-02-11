using Newtonsoft.Json;

namespace Services;

public interface ITestEndpointAccess {
    public Task ExecuteTestsAsync();
}
using Newtonsoft.Json;

namespace BMSAPI.Models;

public class ErrorDTO {
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public override string ToString() => JsonConvert.SerializeObject(this);
}
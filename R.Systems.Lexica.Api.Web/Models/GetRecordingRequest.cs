using Microsoft.AspNetCore.Mvc;

namespace R.Systems.Lexica.Api.Web.Models;

public class GetRecordingRequest
{
    [FromRoute] public string Word { get; set; } = "";

    [FromQuery(Name = "wordType")] public string WordType { get; set; } = "";
}
